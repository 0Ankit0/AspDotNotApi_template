using Microsoft.AspNetCore.Mvc;
using ServiceApp_backend.Models;
using ServiceApp_backend.Classes;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceApp_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
       DatabaseHelper db = new DatabaseHelper();

        // GET: api/<LoginController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LoginController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LoginController>
        [HttpPost]
        public IActionResult Post([FromBody] UsersModal br)
        {
            var jsonstring = "";
            StringBuilder Sb = new StringBuilder();
            SqlParameter[] parm =
            {
                new SqlParameter("@UserName", br.UserName),
                new SqlParameter("@UserEmail", br.UserEmail),
                new SqlParameter("@Password", br.Password),
                new SqlParameter("@Address", br.Address),
                new SqlParameter("@Phone", br.Phone),
                new SqlParameter("@Role", br.Role),
                new SqlParameter("@GUID", br.GUID)
            };
           DataTable dt= db.ReadDataTable("Usp_IU_Users", parm);
            var UserId = dt.Rows[0]["UserId"].ToString();
            if (dt.Rows.Count > 0)
            {
                JwtAuth jwtAuth =new JwtAuth("your_secret_key", "Ankit", "AllUsers");
               string TokenNo = jwtAuth.GenerateToken(br.UserName, Convert.ToInt32(UserId));
                 jsonstring = "{\"status\":200,\"message\":\"Data is sucessfully inserted\",\"data\":{\"tokenNo\":\"" + TokenNo + "\"}}";
               JObject myObj = (JObject)JsonConvert.DeserializeObject(jsonstring);
                return Ok(myObj);
            }
            else
            {
                ResponseModel rm = new ResponseModel
                {
                    message = "Couldn't save data please try again",
                    status = 404,
                    data = new { tokenNo = "" }
                };
               
                return Ok(rm);
            }
           }
        [HttpPost("LoginValidation")]
        public IActionResult LoginValidation([FromBody] UsersModal br)
        {
            try
            {
                var jsonstring = "";
                StringBuilder Sb = new StringBuilder();
                SqlParameter[] parm =
                       {
                        new SqlParameter("@UserEmail", br.UserEmail),
                        new SqlParameter("@Password", br.Password),
                    };
                DataTable dt = db.ReadDataTable("User_Login", parm);
                if (dt.Rows.Count > 0)
                {
                    var UserId = dt.Rows[0]["UserId"].ToString();
                    var UserName = dt.Rows[0]["UserName"].ToString();
                    JwtAuth jwtAuth = new JwtAuth("aBcDeFgHiJkLmNoPqRsTuVwXyZ0123456789!@#$%^&*()", "Ankit", "AllUsers");
                    string TokenNo = jwtAuth.GenerateToken(UserName, Convert.ToInt32(UserId));
                    var response = new
                    {
                        status = 200,
                        message = "Data is successfully inserted",
                        data = new { TokenNo = TokenNo }
                    };
                    jsonstring = JsonConvert.SerializeObject(response);
                    return Ok(response);
                }
                else
                {
                    ResponseModel rm = new ResponseModel
                    {
                        message = "Incorrect UserName or Password",
                        status = 404,
                        data = new { tokenNo = "" }
                    };
                   
                    return Ok(rm);
                }
            }
            catch (Exception ex)
            {
                ResponseModel rm = new ResponseModel
                {
                    message = ex.Message,
                    status = 501,
                    data = new { tokenNo = "" }
                };
                return Ok(rm);
            }
           }

        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
            public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
