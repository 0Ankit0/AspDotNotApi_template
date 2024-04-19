using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public JObject InsertMessage([FromBody] Message data)
        {
            SqlParameter[] parm =
                {
                new SqlParameter("@Message", data.MessageText),
                new SqlParameter("@Sender", data.Sender),
                new SqlParameter("@Receiver", data.Receiver),
                new SqlParameter("@TokenNo", data.TokenNo)
            };
            JObject dt = db.ReadDataWithResponse("Usp_IU_Users", parm);
            return dt;
        }
    }
}
