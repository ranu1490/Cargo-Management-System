
using CmsApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using CmsClassLibrary;
using CmsClassLibrary.Dtos;
using Microsoft.AspNetCore.Cors;

namespace CmsApi.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]   
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CmsContext context;
        private readonly IConfiguration config;

        public CustomersController(CmsContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await context.Customers.ToListAsync();
        }

        [HttpGet("Customer/{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }


        [HttpPost]
        
        public async Task<ActionResult<Customer>> Register(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            return CreatedAtAction("Register", new { id = customer.CustId }, customer);
        }

        [HttpPut("Customer/{id}")]
        //[Authorize]
        public async Task<IActionResult> PutCustomer (int id, Customer customer)
        {
            //if (id != customer.CustId)
            //{
            //    return BadRequest();
            //}

            //context.Entry(customer).State = EntityState.Modified;

            //try
            //{
            //    await context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!CustomerExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
            var existingDetails = await context.Customers.FirstOrDefaultAsync(_ => _.CustId == id);
            if (existingDetails != null)
            {
                existingDetails.CustName = customer.CustName;
                existingDetails.CustEmail = customer.CustEmail;
                existingDetails.CustPhNo = customer.CustPhNo;
                existingDetails.CustAddress = customer.CustAddress;

                await context.SaveChangesAsync();
                return Ok("Updated Sucessfully");
            }
            return NotFound("Cargo Not Found");
        }

        // DELETE: api/Employees/5
        //[HttpDelete("Customer/{id}")]
        //[Authorize]
        //public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        //{
        //    var customer = await context.Customers.FindAsync(id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    context.Customers.Remove(customer);
        //    await context.SaveChangesAsync();

        //    return customer;
        //}

        [HttpPost("login")]
        public async Task<ActionResult<Customer>> Login(Customer customer)
        {
            //1) validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //2) check username & pwd
            var result = await context.Customers.FirstOrDefaultAsync(
                                e => e.CustEmail == customer.CustEmail
                                && e.CustPassword == customer.CustPassword);
            if (result == null) //login failed
            {
                return NotFound();  //return null
            }
            //login success
            //3) generate JWT
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.CustName),
                    new Claim(JwtRegisteredClaimNames.Jti, result.CustId.ToString()),
                    new Claim(ClaimTypes.Email, result.CustEmail),
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
            var dto = new CustLoginDto
            {
                CustId = result.CustId,
                CustName = result.CustName,
                CustEmail = result.CustEmail,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return Ok(dto); //return dto;
        }

        [HttpPost("CargoOrder")]
        [Authorize]
        public async Task<ActionResult<CargoOrderDetail>> Add(CargoOrderDetail cargoorder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            context.CargoOrderDetails.Add(cargoorder);
            await context.SaveChangesAsync();

            return CreatedAtAction("Add", new { id = cargoorder.Id }, cargoorder);
        }

        private bool CustomerExists(int id)
        {
            return context.Customers.Any(e => e.CustId == id);
        }
    }
}

