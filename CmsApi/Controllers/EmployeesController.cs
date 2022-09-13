
using CmsApi.Models;
using CmsClassLibrary;
using CmsClassLibrary.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
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
    public class EmployeesController : ControllerBase
    {
        private readonly CmsContext context;
        private readonly IConfiguration config;

        public EmployeesController(CmsContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await context.Employees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }


        [HttpPost]
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

        [HttpPut("{id}")]
       // [Authorize]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {

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

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        //[Authorize]
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

        [HttpPost("login")]
        public async Task<ActionResult<Employee>> Login(Employee employee)
        {
            //1) validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //2) check username & pwd
            var result = await context.Employees.FirstOrDefaultAsync(
                                e => e.EmpEmail == employee.EmpEmail
                                && e.Pass == employee.Pass);
            if (result == null) //login failed
            {
                return NotFound();  //return null
            }
            //login success
            //3) generate JWT
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.EmpName),
                    new Claim(JwtRegisteredClaimNames.Jti, result.EmpId.ToString()),
                    new Claim(ClaimTypes.Email, result.EmpEmail),
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
            var dto = new LoginDto
            {
                EmpName = result.EmpName,
                EmpEmail = result.EmpEmail,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return Ok(dto); //return dto;
        }
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Customer>> GetCustomer(int id)
        //{
        //    var customer = await context.Customers.FindAsync(id);

        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return customer;
        //}

        //[HttpPut("{id}")]
        //[Authorize]
        //public async Task<IActionResult> PutCargo(int id, Cargo cargo)
        //{
        //    if (id != cargo.CargoId)
        //    {
        //        return BadRequest();
        //    }

        //    context.Entry(cargo).State = EntityState.Modified;

        //    try
        //    {
        //        await context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CargoExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        private bool EmployeeExists(int id)
        {
            return context.Employees.Any(e => e.EmpId == id);
        }
    }

}

