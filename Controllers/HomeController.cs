using System.Collections.Concurrent;
using System.Data;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Api_Template.Classes;
using Api_Template.Models;

namespace Api_Template.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        DatabaseHelper db = new DatabaseHelper();
        private readonly ConcurrentDictionary<string, string> _connectionIdToGuidMap;

        public HomeController(ConcurrentDictionary<string, string> connectionIdToGuidMap)
        {
            _connectionIdToGuidMap = connectionIdToGuidMap;
        }
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
                new SqlParameter("@Receiver", data.Receiver)
            };
            string dt = db.ReadDataWithResponse("Usp_IU_Message", parm);
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
                    StringBuilder Sb = new StringBuilder();
                    string jsonstring = "";
                    var userId = user.UserId;
                    SqlParameter[] parm =
                    {
                        new SqlParameter("@UserId", userId)
                    };
                    DataTable dt = db.ReadDataTable("Usp_S_UsersList", parm);
                    dt.Columns.Add("IsOnline", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (_connectionIdToGuidMap.Any(x => x.Value == dt.Rows[i]["GUID"].ToString()))
                        {
                            dt.Rows[i]["IsOnline"] = "online";
                        }
                        else
                        {
                            dt.Rows[i]["IsOnline"] = "offline";
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        Sb.Append(db.DataTableToJSON(dt, "data", 200, "Data Listed Successfully"));
                        jsonstring = Sb.ToString();
                        return Ok(jsonstring);
                    }
                    else
                    {
                        Sb.Append(db.DataTableToJSON(dt, "data", 401, "Data Not Found"));
                        jsonstring = Sb.ToString();
                        return BadRequest(jsonstring);
                    }

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

        [HttpPost("MessageList")]
        public IActionResult GetMessageList([FromBody] Message msg)
        {
            try
            {
                var user = HttpContext.Items["User"] as dynamic;
                if (user != null)
                {
                    var senderId = user.UserId;
                    SqlParameter[] parm =
                    {
                        new SqlParameter("@senderId", senderId),
                        new SqlParameter("@receiverId", msg.Receiver)
                    };
                    string dt = db.ReadDataWithResponse("Usp_S_MessageById", parm);
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
            catch (Exception ex)
            {

                ResponseModel response = new ResponseModel();
                response.status = 501;
                response.message = "Something went wrong" + ex;
                return BadRequest(response);
            }
        }
    }
}
