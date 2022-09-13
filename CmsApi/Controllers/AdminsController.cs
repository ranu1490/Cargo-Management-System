
using CmsApi.Models;
using CmsClassLibrary;
using CmsClassLibrary.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CmsApi.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
   
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly CmsContext context;
        private readonly IConfiguration config;

        public AdminsController(CmsContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;

        }
        // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmin()
        {
            return await context.Admin.ToListAsync();
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdmin(int id)
        {
            var admin = await context.Admin.FindAsync(id);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }


        [HttpPost]
        public async Task<ActionResult<Admin>> Register(Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            context.Admin.Add(admin);
            await context.SaveChangesAsync();
            return CreatedAtAction("Register", new { id = admin.Id }, admin);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Admin>> Login(Admin admin)
        {
            //1) validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //2) check username & pwd
            var result = await context.Admin.FirstOrDefaultAsync(
                                e => e.Email == admin.Email
                                && e.Pass == admin.Pass);
            if (result == null) //login failed
            {
                return NotFound();  //return null
            }
            //login success
            //3) generate JWT
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, result.Id.ToString()),
                    new Claim(ClaimTypes.Email, result.Email),
                };
            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["Secret"]));
            var issuer = config["Issuer"];
            var audience = config["Audience"];
            var expiryDays = Convert.ToInt32(config["ExpiryDays"]);

            var token = new JwtSecurityToken(issuer, audience, authClaims,
                expires: DateTime.Now.AddDays(expiryDays),
                signingCredentials: new SigningCredentials(authSigningKey,
                                        SecurityAlgorithms.HmacSha256));
            //username, role and token
            var dto = new AdminLoginDto
            {
                Name = result.Name,
                Email = result.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return Ok(dto); //return dto;
        }

        [HttpPost("EmployeeAdd")]
        [Authorize]
        public async Task<ActionResult<Employee>> Register(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            return CreatedAtAction("Register", new { id = employee.EmpId }, employee);
        }

        [HttpGet("GetEmployee{id}")]
        [Authorize]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        [HttpPut("EmployeeUpdate/{id}")]
        [Authorize]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            //if (id != employee.EmpId)
            //{
            //    return BadRequest();
            //}

            //context.Entry(employee).State = EntityState.Modified;

            //try
            //{
            //    await context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmployeeExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
            var existingDetails = await context.Employees.FirstOrDefaultAsync(_ => _.EmpId == id);
            if (existingDetails != null)
            {
                existingDetails.EmpName = employee.EmpName;
                existingDetails.EmpEmail = employee.EmpEmail;
                existingDetails.EmpPhNo = employee.EmpPhNo;

                await context.SaveChangesAsync();
                return Ok("Updated Sucessfully");
            }
            return NotFound("Employee Not Found");
        }


        [HttpDelete("Employee/Delete/{id}")]
        [Authorize]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            context.Employees.Remove(employee);
            await context.SaveChangesAsync();

            return employee;
        }
        [HttpGet("GetCustomer/{id}")]
        [Authorize]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }


        [HttpPut("UpdateCargo/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCargo(int id, Cargo cargo)
        {
            if (id != cargo.CargoId)
            {
                return BadRequest();
            }

            context.Entry(cargo).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CargoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cargoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("AddCargo")]
        [Authorize]
        public async Task<ActionResult<Cargo>> AddCargo(Cargo cargo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            context.Cargo.Add(cargo);
               await context.SaveChangesAsync();

              return CreatedAtAction("Add", new { id = cargo.CargoId }, cargo);
        }
        //// DELETE: api/Cargoes/5
        [HttpDelete("DeleteCargo/{id}")]
        [Authorize]
        public async Task<ActionResult<Cargo>> DeleteCargo(int id)
        {
            var cargo = await context.Cargo.FindAsync(id);
            if (cargo == null)
            {
                return NotFound();
            }

            context.Cargo.Remove(cargo);
            await context.SaveChangesAsync();

            return cargo;
        }

        private bool CargoExists(int id)
        {
            return context.Cargo.Any(e => e.CargoId == id);
        }
        private bool EmployeeExists(int id)
        {
            return context.Employees.Any(e => e.EmpId == id);
        }
    }
}
