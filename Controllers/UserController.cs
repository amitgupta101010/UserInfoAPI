using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserInfromationAPI.DAL;
using UserInfromationAPI.Models;
using UserInfromationAPI.Validator;

namespace UserInfromationAPI.Controllers
{
    [Route("api/users")]

    public class UserController : Controller
    {
        private readonly IUserData _userInformation;

        public UserController(IUserData userInformation)
        {
            _userInformation = userInformation;
        }

        [HttpPost]
        public JsonResult AddUser([FromBody]User user)
        {
            try
            {
                var validator = new UserValidator();
                var results = validator.Validate(user);
                if (results.IsValid)
                { 
                    return Json(new { status = "Success", message = _userInformation.AddUser(user) });
                }
                else
                {
                    var error = results.Errors.Aggregate("", (current, failure) => current + failure.ErrorMessage);
                    throw new Exception(error);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetUser([FromQuery]int? id)
        {            
            try
            {
                var result = _userInformation.GetAllUser(id);
                return result == null
                    ? Json(new {status = "Success", message = "user does not exist."})
                    : Json(new {status = "Success", message = result});
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message});
            }
        }

        [HttpPut]
        [Route("{id}")]
        public JsonResult UpdateUser(int id, [FromBody]User user)
        {
            try
            {
                return id <= 0
                    ? Json(new {status = "Error", message = "Please provide valid ID"})
                    : Json(user is null
                        ? new {status = "Success", message = "Nothing to update"}
                        : new {status = "Success", message = _userInformation.UpdateUser(id, user)});
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public JsonResult DeleteUser(int id)
        {
            try
            {
                return Json(id <= 0 ? new { status = "Error", message = "Please provide valid ID" } : new { status = "Success", message = _userInformation.DeleteUser(id) });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
        }
    }
}
