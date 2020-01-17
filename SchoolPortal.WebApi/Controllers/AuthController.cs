using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GeneralHelper.Lib.Models;
using SchoolPortal.Logic.Utilities;
using SchoolPortal.Logic.BusinessLogic;
using System.Collections.Generic;
using System.Linq;

namespace SchoolPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private static readonly string[] Summaries = new[]
       {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
            "Sunny", "Raining", "Cold", "Cloudy", "Winther", "Warming", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public readonly IJwtTokenService _jwtTokenService;
        public AuthController(IUserAccountService userAccountService, IJwtTokenService jwtTokenService)
            : base(userAccountService)
        {

            _jwtTokenService = jwtTokenService;
        }
        [AllowAnonymous]
        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            
            var responseData = await userAccountService.LoginAsync(model);
            return Ok(responseData);
        }
        [AllowAnonymous]
        [HttpPost, Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest requestData)
        {
            var responseData = await _jwtTokenService.RefreshToken(requestData.Token, requestData.RefreshToken);
            return Ok(responseData);

        }
        [AllowAnonymous]
        [HttpPost, Route("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {

            var responseData = await userAccountService.CreateAsync(model);
            return Ok(responseData);
        }
        [AllowAnonymous]
        [HttpPost, Route("ConfirmEmailAndSetPassword")]
        public async Task<IActionResult> ConfirmEmailAndSetPassword([FromBody]ConfirmEmailData model)
        {

            var responseData = await userAccountService.ConfirmEmailAndSetPasswordAsync(model);
            return Ok(responseData);
        }
        [AllowAnonymous]
        [HttpGet, Route("GetWeatherForecast")]
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            var rng = new Random();
            return Enumerable.Range(1, 10).Select(index => new WeatherForecast
            {
                Id = index,
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}