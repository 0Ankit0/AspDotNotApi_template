using API_TEMPLATE.CustomClass;
using API_TEMPLATE.Data;
using API_TEMPLATE.Data.Users;
using API_TEMPLATE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;

namespace API_TEMPLATE.Controllers
{
    [Route("api/{version:apiVersion}/User")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IJwtAuth _jwtAuth;
        private readonly IDataHandler _dh;
        private readonly ApplicationDbContext _context;

        public LoginController(IJwtAuth jwtAuth, IDataHandler dataHandler, ApplicationDbContext context)
        {
            _jwtAuth = jwtAuth;
            _dh = dataHandler;
            _context = context;
        }

        // POST: api/Login/UserLogin
        [HttpPost("Login")]
        public IActionResult UserLogin([FromBody] LoginModel br)
        {
            try
            {
                //SqlParameter[] param =
                //{
                // new SqlParameter("@UserEmail", br.UsernameOrEmail),
                // new SqlParameter("@Password", br.Password),
                //};
                //var result = _dh.ReadDataWithResponse("Usp_Sys_UserLogin", param);
                //var jsonResult = JsonConvert.DeserializeObject<ResponseModel>(result);
                //if (jsonResult.status == 200)
                //{
                //    var token = _jwtAuth.GenerateToken(br.UsernameOrEmail, br.GUID);
                //    jsonResult.data = new { Token=token};
                //    return Ok(jsonResult);
                //}
                //else
                //{

                //    return StatusCode(StatusCodes.Status204NoContent);
                //}
                var user = _context.UserMasters
                           .FirstOrDefault(u => u.UserEmail == br.UsernameOrEmail && u.Password == br.Password);

                if (user != null)
                {
                    var token = _jwtAuth.GenerateToken(user.UserEmail, user.GUID);
                    var response = new ResponseModel
                    {
                        status = 200,
                        data = new { Token = token }
                    };
                    return Ok(response);
                }
                else
                {
                    var res = new ResponseModel
                    {
                        status = 204,
                        message = "Invalid email or password."
                    };
                    return StatusCode(StatusCodes.Status404NotFound, res);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        // POST: api/Login/Register
        [HttpPost("Register")]
        public IActionResult RegisterUser([FromBody] RegisterModel br)
        {
            try
            {
                //SqlParameter[] param =
                //{
                // new SqlParameter("@UserName", br.Username),
                // new SqlParameter("@UserEmail", br.Email),
                // new SqlParameter("@Password", br.Password),
                // new SqlParameter("@PhoneNumber", br.PhoneNumber),
                // new SqlParameter("@Role",br.Role)
                //};
                //var result = _dh.ReadDataWithResponse("Usp_IU_UserMaster", param);

                //return Ok(result);

                // Find the role by name (assuming you have a method or logic to retrieve the role)
                var role = _context.RoleMasters.FirstOrDefault(r => r.RoleId == br.Role);
                if (role == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Invalid role specified.");
                }
                var existingUser = _context.UserMasters.FirstOrDefault(u => u.UserEmail == br.Email);
                if (existingUser != null)
                {
                    return StatusCode(StatusCodes.Status409Conflict, "User with this email already exists.");
                }

                // Create a new UserMaster entity
                var user = new UserMaster
                {
                    UserName = br.Username,
                    UserEmail = br.Email,
                    Password = br.Password, // Ensure that the password is hashed in production
                    Phone = br.PhoneNumber,
                    RoleId = role.RoleId, // Set the RoleId
                    CreatedAt = DateTime.Now,
                    GUID = Guid.NewGuid().ToString(),
                    Active = true
                };

                // Add the user to the database
                _context.UserMasters.Add(user);
                _context.SaveChanges();

                // Return a success response
                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

    }
}
