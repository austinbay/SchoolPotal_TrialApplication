using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolPortal.Logic.BusinessLogic;
using SchoolPortal.Models.DataModels;

namespace SchoolPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentRecordController : BaseController
    {
       
        private readonly IStudentRecordService _studentRecordService;
       
        public StudentRecordController(IUserAccountService userAccountService, IStudentRecordService studentRecordService)
            : base(userAccountService)
        {

            _studentRecordService = studentRecordService;

        }
        [AllowAnonymous]
        [HttpGet, Route("LoadAll")]
        public async Task<object> LoadAll(DataSourceLoadOptionsBinder.DataSourceLoadOptions loadOptions)
        {
            var responseData = await _studentRecordService.GetAllAsync();
            return DataSourceLoader.Load(responseData.Data, loadOptions);
        }
        [HttpGet, Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
           
            var responseData = await _studentRecordService.GetAllAsync();
            return Ok(responseData);

        }
        [HttpPost, Route("AddItem")]
        public async Task<IActionResult> Post([FromBody] StudentRecordModel model)
        {
            var responseData = await _studentRecordService.AddAsync(model, User.Identity.GetUserId());
            return Ok(responseData);
        }
        [HttpPut, Route("UpdateItem")]
        public async Task<IActionResult> Put([FromBody] StudentRecordModel model)
        {
            var responseData = await _studentRecordService.UpdateAsync(model);
            return Ok(responseData);
        }
        [HttpGet, Route("GetItem/{id}")]
        public async Task<ActionResult> GetItem(int id)
        {
            var responseData = await _studentRecordService.GetItemAsync(id);
            return Ok(responseData);
        }
        [HttpDelete, Route("DeleteItem/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var responseData = await _studentRecordService.DeleteAsync(id);
            return Ok(responseData);
        }
        [HttpGet, Route("GetDetails/{id}")]
        public async Task<ActionResult> GetDetails(int id)
        {
            var responseData = await _studentRecordService.GetDetailsAsync(id);
            return Ok(responseData);
        }
        [HttpGet, Route("GetLookup")]
        public async Task<ActionResult> GetLookup()
        {
            var responseData = await _studentRecordService.GetLookupAsync();
            return Ok(responseData);
        }

    }
}