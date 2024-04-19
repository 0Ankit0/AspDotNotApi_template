using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using ServiceApp_backend.Classes;
using ServiceApp_backend.Models;

namespace ServiceApp_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        DatabaseHelper db = new DatabaseHelper();

        public IActionResult Index()
        {
            return Ok("Welcome to the home page");
        }
        [HttpPost("message")]
        public string InsertMessage([FromBody] Message data)
        {
            SqlParameter[] parm =
                {
                new SqlParameter("@Message", data.MessageText),
                new SqlParameter("@Sender", data.Sender),
                new SqlParameter("@Receiver", data.Receiver),
                new SqlParameter("@TokenNo", data.TokenNo)
            };
            string dt = db.ReadDataWithResponse("Usp_IU_Users", parm);
            return dt;
        }

        [HttpGet("UserList")]
        public IActionResult GetUserList()
        {
            try
            {
                var user = HttpContext.Items["User"] as dynamic;
                if (user != null)
                {
                    var userId = user.UserId;
                    SqlParameter[] parm =
                    {
                        new SqlParameter("@UserId", userId)
                    };
                    string dt = db.ReadDataWithResponse("Usp_S_UsersList", parm);

                    return Ok(dt);
                }
                else
                {
                    ResponseModel response = new ResponseModel();
                    response.status = 401;
                    response.message = "Invalid Token";
                    return BadRequest(response);
                }

            }
            catch (System.Exception)
            {

                ResponseModel response = new ResponseModel();
                response.status = 501;
                response.message = "Something went wrong";
                return BadRequest(response);
            }
        }
    }
}
