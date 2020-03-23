using System;
using Microsoft.AspNetCore.Mvc;
using UserInfromationAPI.Models;

namespace UserInfromationAPI.Controllers
{
    
    [Route("api/[controller]")]

    public class UserController : Controller
    {
        private readonly IUserInformation _userInformation;

        public UserController(IUserInformation userInformation)
        {
            _userInformation = userInformation;
        }
        

        [HttpPost]
        [Route("add")]
        public JsonResult AddUser([FromBody]User user)
        {
           // Console.WriteLine(user.Address.Pincode);
            try
            {
                if (ModelState.IsValid)
                    return Json(new { status = "Success", message = _userInformation.AddUser(user) });
                else
                    throw new Exception("Please enter valid input.");
                
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }
            
        }

        [HttpGet]
        [Route("get/{ID?}")]
        public JsonResult GetUser(int? ID)
        {
            //string userList = JsonSerializer.Serialize(_userInformation.GetAllUser(ID));
            try
            {
                var result = _userInformation.GetAllUser(ID);
                if (result == null)
                {
                    return Json(new { status = "Success", message = "user does not exist." });
                }
                return Json(new { status = "Success", message = result});
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message});
            }
        }

        [HttpPut]
        [Route("update/{ID}")]
        public JsonResult UpdateUser(int ID, [FromBody]User user)
        {

            try
            {
                if (ID <= 0)
                {
                    return Json(new { status = "Error", message = "Please provide valid ID" });
                }
                if (user == null)
                {
                    return Json(new { status = "Success", message = "Nothing to update" });
                }
                return Json(new { status = "Success", message = _userInformation.UpdateUser(ID, user) });
            }
            catch (Exception ex)
            {

                return Json(new { status = "error", message = ex.Message });
            }
        }

        [HttpDelete]
        [Route("delete/{ID}")]
        public JsonResult DeleteUser(int ID)
        {
            try
            {
                if (ID <= 0)
                {
                    return Json(new { status = "Error", message = "Please provide valid ID" });
                }
                return Json(new { status = "Success", message = _userInformation.DeleteUser(ID) });
            }
            catch (Exception ex)
            {

                return Json(new { status = "error", message = ex.Message });
            }
        }
    }
}
