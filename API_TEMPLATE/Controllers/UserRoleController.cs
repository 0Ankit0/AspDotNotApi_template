using API_TEMPLATE.Data;
using API_TEMPLATE.Data.Users;
using API_TEMPLATE.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_TEMPLATE.Controllers
{
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class UserRoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserRoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<UserRoleController>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all Roles", Description = "Retrieves a list of all User Roles.")]
        public async Task<ActionResult<IEnumerable<RoleModel>>> Get()
        {
            try
            {
                var roles = await _context.RoleMasters
                     .Select(ur => new RoleModel
                     {
                         RoleId = ur.RoleId,
                         RoleName = ur.RoleName,
                         GUID = ur.GUID
                     })
                    .ToListAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/<UserRoleController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleModel>> Get(int id)
        {
            try
            {
                var roleMaster = await _context.RoleMasters.FindAsync(id);

                if (roleMaster == null)
                {
                    return NotFound($"Role with ID {id} not found.");
                }

                return Ok(roleMaster);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/<UserRoleController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoleModel role)
        {
            try
            {
                RoleMaster roleMaster = new RoleMaster()
                {
                    RoleName = role.RoleName,
                    GUID = Guid.NewGuid().ToString()
                };

                await _context.RoleMasters.AddAsync(roleMaster);
                await _context.SaveChangesAsync();
                return Ok(roleMaster);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT api/<UserRoleController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleModel roleMaster)
        {
            try
            {
                var existingRole = await _context.RoleMasters.FindAsync(id);
                if (existingRole == null)
                {
                    return NotFound($"Role with ID {id} not found.");
                }

                existingRole.RoleName = roleMaster.RoleName;

                _context.RoleMasters.Update(existingRole);
                await _context.SaveChangesAsync();

                return Ok(existingRole);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/<UserRoleController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var roleMaster = await _context.RoleMasters.FindAsync(id);
                if (roleMaster == null)
                {
                    return NotFound($"Role with ID {id} not found.");
                }

                _context.RoleMasters.Remove(roleMaster);
                await _context.SaveChangesAsync();

                return Ok("Role deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
