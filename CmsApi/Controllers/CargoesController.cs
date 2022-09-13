using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CmsApi.Models;
using CmsClassLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace CmsApi.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
   
    [ApiController]
    public class CargoesController : ControllerBase
    {
        private readonly CmsContext context;

        public CargoesController(CmsContext context)
        {
            this.context = context;
        }

        // GET: api/Cargoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cargo>>> GetCargo()
        {
            return await context.Cargo.ToListAsync();
        }

        // GET: api/Cargoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cargo>> GetCargo(int id)
        {
            var cargo = await context.Cargo.FindAsync(id);

            if (cargo == null)
            {
                return NotFound();
            }

            return cargo;
        }
        [HttpPut("{id}")]
        
        public async Task<IActionResult> UpdateCargo(int id, Cargo cargo)
        {
            //if (id != cargo.CargoId)
            //{
            //    return BadRequest();
            //}

            //context.Entry(cargo).State = EntityState.Modified;

            //try
            //{
            //    await context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!CargoExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
            var existingDetails = await context.Cargo.FirstOrDefaultAsync(_ => _.CargoId == id);
            if (existingDetails != null)
            {
                existingDetails.CargoName = cargo.CargoName;
                existingDetails.Place = cargo.Place;
                existingDetails.Price = cargo.Price;

                await context.SaveChangesAsync();
                return Ok("Updated Sucessfully");
            }
            return NotFound("Cargo Not Found");
        }

        // POST: api/Cargoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
       
        public async Task<ActionResult<Cargo>> PostCargo(Cargo cargo)
        {
            context.Cargo.Add(cargo);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetCargo", new { id = cargo.CargoId }, cargo);
        }

        // DELETE: api/Cargoes/5
        [HttpDelete("{id}")]
       
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

        // PUT: api/Cargoes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("UpdateCargo/{id}")]
        //public async Task<IActionResult> PutCargo(int id, Cargo cargo)
        //{
        //    if (id != cargo.CargoId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(cargo).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
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

        // POST: api/Cargoes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPost("AddCargo")]
        //public async Task<ActionResult<Cargo>> PostCargo(Cargo cargo)
        //{
        //    context.Cargo.Add(cargo);
        //    await context.SaveChangesAsync();

        //    return CreatedAtAction("GetCargo", new { id = cargo.CargoId }, cargo);
        //}

        // DELETE: api/Cargoes/5
        //[HttpDelete("DeleteCargo/{id}")]
        //public async Task<ActionResult<Cargo>> DeleteCargo(int id)
        //{
        //    var cargo = await _context.Cargo.FindAsync(id);
        //    if (cargo == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Cargo.Remove(cargo);
        //    await _context.SaveChangesAsync();

        //    return cargo;
        //}

        //private bool CargoExists(int id)
        //{
        //    return _context.Cargo.Any(e => e.CargoId == id);
        //}
    }
}
