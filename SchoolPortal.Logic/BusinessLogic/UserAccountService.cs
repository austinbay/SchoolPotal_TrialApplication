using SchoolPortal.Data.Entities;
using SchoolPortal.Data.Helpers;
using SchoolPortal.Data.Repositories;
using SchoolPortal.Models.DataModels;
using AutoMapper;
using GeneralHelper.Lib.Models;
using MessageManager.Lib.Utilities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SchoolPortal.Logic.Utilities;
using MessageManager.Lib.Services;
using Microsoft.Extensions.Configuration;

namespace SchoolPortal.Logic.BusinessLogic
{
    public interface IUserAccountService
    {

        Task<ServiceResponseData<TokenData>> LoginAsync(LoginModel loginModel);

        Task<ServiceResponseData> CreateAsync(RegisterViewModel model);

        Task<ServiceResponseData> DeleteAsync(int id);

        Task<ServiceResponseData> UpdateAsync(AspNetUserModel model);
        Task<ServiceResponseData<AspNetUserModel>> GetItemAsync(int id);
        Task<ServiceResponseData<AspNetUserDetails>> GetDetailsAsync(int id);

        Task<ServiceResponseData<List<LookupModel>>> GetLookupAsync();
        Task<ServiceResponseData<List<AspNetUserItem>>> GetAllAsync();

        Task<ServiceResponseData<AspNetUserItem>> GetProfileByIdAsync(int userId);

        Task<ServiceResponseData> ChangePasswordAsync(ChangePasswordModel model, string userName);
        Task<ServiceResponseData> SendForgotPasswordLinkAsync(ForgotPasswordModel model);
        Task<ServiceResponseData> SendUserAccountConfirmationLinkAsync(string email);
        Task<ServiceResponseData> ResetPasswordAsync(ResetPasswordModel model);

        Task<ServiceResponseData> AddPasswordAsync(AddPasswordModel model);
        Task<ServiceResponseData> ConfirmEmailAndSetPasswordAsync(ConfirmEmailData model);

    }
    public class UserAccountService : IUserAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAspNetUserRepository _aspNetUserRepository;
        private readonly IMapper _mapper;
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailManager _emailManager;
        private readonly IEmailHelper _emailHelper;
        private readonly IErrorService _errorService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IConfiguration _configuration;

        public UserAccountService(IUnitOfWork unitOfWork, IAspNetUserRepository aspNetUserRepository, IMapper mapper,
            UserManager<ApplicationUser> userManager, IEmailManager emailManager, IEmailHelper emailHelper, IErrorService errorService,
            SignInManager<ApplicationUser> signInManager, IJwtTokenService jwtTokenService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _aspNetUserRepository = aspNetUserRepository;
            _mapper = mapper;
            _userManager = userManager;
            _emailManager = emailManager;
            _emailHelper = emailHelper;
            _errorService = errorService;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _configuration = configuration;
        }
        public async Task<ServiceResponseData<TokenData>> LoginAsync(LoginModel loginModel)
        {
            var responseData = new ServiceResponseData<TokenData>();
            try
            {
                var user = await _userManager.FindByNameAsync(loginModel.UserName);
                if (user == null)
                {
                    throw new Exception("Invalid username");
                }

                var isValidPassword = await _userManager.CheckPasswordAsync(user, loginModel.Password);
                if (!isValidPassword)
                {
                    throw new Exception("Invalid password");
                }
                responseData = await _jwtTokenService.GenerateToken(user);
            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }
        public Task<ServiceResponseData> UpdateAsync(AspNetUserModel model)
        {
            throw new NotImplementedException();
        }
        public async Task<ServiceResponseData> AddPasswordAsync(AddPasswordModel model)
        {
            var responseData = new ServiceResponseData();
            try
            {
                if (string.IsNullOrEmpty(model.Email))
                    throw new Exception("Email NOT found");

                if (!string.Equals(model.Password, model.ConfirmPassword))
                {
                    throw new Exception("Confirm password does't match the password");
                }
                var user = await _userManager.FindByEmailAsync(model.Email);

                var result = await _userManager.AddPasswordAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.FirstOrDefault().Description);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);

            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }

       

        public Task<ServiceResponseData> ChangePasswordAsync(ChangePasswordModel model, string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponseData> ConfirmEmailAndSetPasswordAsync(ConfirmEmailData model)
        {
            var responseData = new ServiceResponseData();
            try
            {

                if (!string.Equals(model.Password, model.ConfirmPassword))
                {
                    throw new Exception("Confirm password does't match");
                }
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                    throw new Exception("User info NOT found");

                if (user.EmailConfirmed && await _userManager.HasPasswordAsync(user))
                    throw new Exception("Your account has been confirmed already!");

                var confirmResult = await _userManager.ConfirmEmailAsync(user, model.Code);
                if (!confirmResult.Succeeded)
                    throw new Exception(confirmResult.Errors.FirstOrDefault().Description);


                var addPasswordResult = await _userManager.AddPasswordAsync(user, model.Password);
                if (!addPasswordResult.Succeeded)
                {
                    throw new Exception(addPasswordResult.Errors.FirstOrDefault().Description);
                }
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
               
            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);


        }
        public async Task<ServiceResponseData> CreateAsync(RegisterViewModel model)
        {
         
            var responseData = new ServiceResponseData();
            try
            {
                var emailExists = _aspNetUserRepository.Get().Any(c => c.Email == model.Email);
                if (emailExists)
                    throw new Exception("Sorry! This email already exists.");

                var phoneExists = _aspNetUserRepository.Get().Any(c => c.PhoneNumber == model.PhoneNumber);
                if (phoneExists)
                    throw new Exception("Sorry! This phone number already exists.");

                var adminUser = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsActive = false,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                };
                var result = await _userManager.CreateAsync(adminUser);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.FirstOrDefault().Description);
                  
                }

                responseData = await SendUserAccountConfirmationLinkAsync(model.Email);

            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }

        public Task<ServiceResponseData> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        private List<AspNetUserItem> ProcessMapping(IEnumerable<AspNetUsers> entities)
        {

            return _mapper.Map<List<AspNetUserItem>>(entities);

        }
        public async Task<ServiceResponseData<List<AspNetUserItem>>> GetAllAsync()
        {
           

            var responseData = new ServiceResponseData<List<AspNetUserItem>>();
            try
            {

                var filterBy = new AspNetUserFilter();
                var entities = _aspNetUserRepository.LoadAll(filterBy);

                var itemList = ProcessMapping(entities).OrderBy(x => x.FullName).ToList();
                responseData.Data = itemList;

            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }

        public Task<ServiceResponseData<AspNetUserDetails>> GetDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponseData<AspNetUserModel>> GetItemAsync(int id)
        {
            var responseData = new ServiceResponseData<AspNetUserModel>();
            try
            {

                var entity = GetAspNetUserEntity(id);
                var userModel = _mapper.Map<AspNetUserItem>(entity);

                responseData.Data = userModel;

            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }

        public Task<ServiceResponseData<List<LookupModel>>> GetLookupAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<ServiceResponseData<AspNetUserItem>> GetProfileByIdAsync(int userId)
        {
           
            var responseData = new ServiceResponseData<AspNetUserItem>();
            try
            {

                var entity = GetAspNetUserEntity(userId);
                var userModel = _mapper.Map<AspNetUserItem>(entity);

                responseData.Data = userModel;

            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }
        public async Task<ServiceResponseData> ResetPasswordAsync(ResetPasswordModel model)
        {

            var responseData = new ServiceResponseData();
            try
            {


                if (!string.Equals(model.Password, model.ConfirmPassword))
                {
                    throw new Exception("Confirm password does't match the password");
                }
                var user = await _userManager.FindByIdAsync(model.UserId);
                
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.FirstOrDefault().Description);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);

            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }

        public async Task<ServiceResponseData> SendForgotPasswordLinkAsync(ForgotPasswordModel model)
        {
            var responseData = new ServiceResponseData();
            try
            {

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    throw new Exception("Invalid email");


                if (!user.EmailConfirmed)
                    throw new Exception("Your account has not been confirmed! Please check the email we sent to you.");

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var baseUrl = _emailHelper.GetApplicationBaseUrl();
                var callBackUrl = string.Format("{0}/Auth/ResetPassword?userId={1}&code={2}", baseUrl, HttpUtility.UrlEncode(user.Id.ToString()), HttpUtility.UrlEncode(token));
                _emailManager.SendPasswordResetEmail(user.FirstName, user.Email, callBackUrl);

            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
           
        }

        public async Task<ServiceResponseData> SendUserAccountConfirmationLinkAsync(string email)
        {
            var responseData = new ServiceResponseData();
            try
            {

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    throw new Exception("Invalid email");


                if (user.EmailConfirmed)
                    throw new Exception("Your account has been confirmed already!");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var baseUrl = _configuration["AngularAppUrl"]; //_emailHelper.GetApplicationBaseUrl();
                //var callBackUrl = string.Format("{0}/confirmemail?userId={1}&code={2}", baseUrl, HttpUtility.UrlEncode(user.Id.ToString()), HttpUtility.UrlEncode(token));
                var callBackUrl = string.Format("{0}/confirmemail/{1}/{2}", baseUrl, user.Id, HttpUtility.UrlEncode(token.Trim()));
                _emailManager.SendAccountConfirmationEmail(user.FirstName, user.Email, callBackUrl);

            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }
        private void RunValidation(AspNetUserModel model)
        {
            var emailExists = _aspNetUserRepository.Get().Any(c => c.Email == model.Email && c.Id != model.Id);
            if (emailExists)
                throw new Exception("Sorry! This email already exists.");

            var phoneExists = _aspNetUserRepository.Get().Any(c => c.PhoneNumber == model.PhoneNumber && c.Id != model.Id);
            if (phoneExists)
                throw new Exception("Sorry! This phone number already exists.");
        }
      
        private AspNetUsers GetAspNetUserEntity(int id)
        {
            var item = _aspNetUserRepository.Find(id);
            return item;

        }

      

       
    }
}
