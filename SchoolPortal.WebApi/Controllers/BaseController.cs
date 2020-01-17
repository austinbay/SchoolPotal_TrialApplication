using System;
using Microsoft.AspNetCore.Mvc;
using SchoolPortal.Logic.BusinessLogic;

namespace SchoolPortal.WebApi.Controllers
{
   
   
    public class BaseController : ControllerBase
    {   
        public readonly IUserAccountService userAccountService;
      

        public BaseController(IUserAccountService _userAccountService)
        {

            userAccountService = _userAccountService;
            

        }
    }
}