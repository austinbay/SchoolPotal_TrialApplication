using SchoolPortal.Data.Entities;
using SchoolPortal.Data.Helpers;
using SchoolPortal.Data.Repositories;
using GeneralHelper.Lib.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SchoolPortal.Logic.Utilities
{
    public interface IJwtTokenService
    {
        Task<ServiceResponseData<TokenData>> GenerateToken(ApplicationUser appUse);
        Task<ServiceResponseData<TokenData>> RefreshToken(string accessToken, string refreshToken);


    }
    public class JwtTokenService : IJwtTokenService
    {
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IErrorService _errorService;
        private readonly SymmetricSecurityKey _authSigningKey;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public JwtTokenService(IConfiguration configuration, IErrorService errorService, UserManager<ApplicationUser> userManager,
            IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
        {           
            _configuration = configuration;
            _errorService = errorService;
            _authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["ApiSecretKey"]));
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResponseData<TokenData>> GenerateToken(ApplicationUser appUser)
        {
            var responseData = new ServiceResponseData<TokenData>();
            try
            {

                var authClaims = new[]
                  {
                        new Claim("UserId", appUser.Id.ToString()),
                        new Claim("UserName", appUser.UserName),
                        new Claim("FirstName", appUser.FirstName),
                        new Claim("LastName", appUser.LastName),
                        new Claim(ClaimTypes.Role, "Admin")
                    };

                
                var accessToken = new JwtSecurityToken(
                    issuer: _configuration["ApiServerUrl"],
                    audience: _configuration["ApiServerUrl"],//"Everyone"
                    expires: DateTime.Now.AddHours(5),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(_authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                var refreshToken = GenerateRefreshToken();
                var tokenData = new TokenData
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                    TokenExpiration = accessToken.ValidTo,
                    RefreshToken = refreshToken,
                    FirstName = appUser.FirstName
                };

                SaveRefreshToken(appUser.Id, refreshToken, tokenData.TokenExpiration);

                responseData.Data = tokenData;
            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }

        public async Task<ServiceResponseData<TokenData>> RefreshToken(string accessToken, string refreshToken)
        {
            var responseData = new ServiceResponseData<TokenData>();
            try
            {
                var principal = GetPrincipalFromExpiredToken(accessToken);
                var username = principal.Identity.Name;
                var appUser = await _userManager.FindByNameAsync(username);
              
                var oldRefreshToken = GetRefreshToken(appUser.Id, refreshToken); //retrieve the refresh token from a data store
                if (string.IsNullOrEmpty(oldRefreshToken))
                    throw new SecurityTokenException("Invalid refresh token");
             
                responseData = await GenerateToken(appUser);
              
                return await Task.FromResult(responseData);

            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }
        private void SaveRefreshToken(int userId, string refreshToken, DateTime tokenExpiration)
        {
            //saves the refresh token to a data store
            var oldRefreshToken = _refreshTokenRepository.Get().SingleOrDefault(x => x.UserId == userId);
            if (oldRefreshToken != null)
            {
                oldRefreshToken.Value = refreshToken;
                oldRefreshToken.TokenExpiration = tokenExpiration;
                _refreshTokenRepository.Update(oldRefreshToken);
            }
            else
            {
                var entity = new RefreshToken
                {
                    Value = refreshToken,
                    UserId = userId,
                    TokenExpiration = tokenExpiration
                };
                _refreshTokenRepository.Add(entity);
            }

            _unitOfWork.Commit();
        }
       
        private string GetRefreshToken(int userId, string refreshToken)
        {
            //retrieves the refresh token from a data store
            var entity = _refreshTokenRepository.Get().SingleOrDefault(x => x.UserId == userId && x.Value == refreshToken);
            if (entity != null)
                return entity.Value;

            return null;
        }
        private string GenerateRefreshToken()
        {

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }

           
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _authSigningKey,
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }

   
}
