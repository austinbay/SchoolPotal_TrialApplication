using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolPortal.Logic.BusinessLogic;
using SchoolPortal.Models.DataModels;

namespace SchoolPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : BaseController
    {
       
        private readonly IUserRoleService _userRoleService;
       
        public UserRoleController(IUserAccountService userAccountService, IUserRoleService userRoleService)
            : base(userAccountService)
        {

            _userRoleService = userRoleService;

        }
        [HttpGet, Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
           
            //var currentUserId = User.Identity.GetUserId();
            var responseData = await _userRoleService.GetAllAsync();
            return Ok(responseData);

        }
        [HttpPost, Route("add")]
        public async Task<IActionResult> Post([FromBody] UserRoleModel model)
        {
            var responseData = await _userRoleService.AddAsync(model);
            return Ok(responseData);
        }
        [HttpPut, Route("update")]
        public async Task<IActionResult> Put([FromBody] UserRoleModel model)
        {
            var responseData = await _userRoleService.UpdateAsync(model);
            return Ok(responseData);
        }
        [HttpGet, Route("getItem/{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var responseData = await _userRoleService.GetItemAsync(id);
            return Ok(responseData);
        }
        [HttpDelete, Route("deleteItem/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var responseData = await _userRoleService.DeleteAsync(id);
            return Ok(responseData);
        }
        [HttpGet, Route("getDetails/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var responseData = await _userRoleService.GetDetailsAsync(id);
            return Ok(responseData);
        }
        [HttpGet, Route("getLookup")]
        public async Task<IActionResult> GetLookup()
        {
            var responseData = await _userRoleService.GetLookupAsync();
            return Ok(responseData);
        }

    }
}