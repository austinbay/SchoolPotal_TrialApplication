using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SchoolPortal.Logic.BusinessLogic;
using SchoolPortal.Logic.Utilities;

namespace SchoolPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageUserController : ControllerBase
    {
       
        private readonly IUserAccountService _userAccountService;
        private readonly IErrorService _errorService;

        public ManageUserController(IUserAccountService userAccountService, IErrorService errorService)
        {

            _userAccountService = userAccountService;
            _errorService = errorService;
        }
        [HttpGet, Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var responseData = await _userAccountService.GetAllAsync();
                return Ok(responseData);

            }
            catch (Exception ex)
            {
                ex.ProcessException(_errorService);
                return Unauthorized(ex.Message);
            }

        }
    }
}