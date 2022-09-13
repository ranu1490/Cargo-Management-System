using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CmsApi.Models;
using CmsClassLibrary;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Cors;

namespace CmsApi.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoOrderDetailsController : ControllerBase
    {
        private readonly CmsContext _context;

        public CargoOrderDetailsController(CmsContext context)
        {
            _context = context;
        }

        // GET: api/CargoOrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CargoOrderDetail>>> GetCargoOrderDetails()
        {
            return await _context.CargoOrderDetails.ToListAsync();
        }

        // GET: api/CargoOrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CargoOrderDetail>> GetCargoOrderDetail(int id)
        {
            var cargoOrderDetail = await _context.CargoOrderDetails.FindAsync(id);

            if (cargoOrderDetail == null)
            {
                return NotFound();
            }
            return cargoOrderDetail;
        }

        //[HttpGet("CargoOrderList{id}")]
        //public async Task<ActionResult<CargoOrderDetail>> GetCargoOrderList(int id)
        //{
        //    var cargoOrderDetail = await _context.CargoOrderDetails.FindAsync(id);
        //    //var customer = await _context.Customers.FindAsync(cargoOrderDetail.CustId);
        //    //var cargo = await _context.Cargo.FindAsync(cargoOrderDetail.CargoId);

        //    //var result = new JObject();
        //    //result.Merge(customer);
        //    //result.Merge(cargo);

        //    if (cargoOrderDetail == null)
        //    {
        //        return NotFound();
        //    }
        //    return result;
        //}


        // PUT: api/CargoOrderDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCargoOrderDetail(int id, CargoOrderDetail cargoOrderDetail)
        //{
        //    if (id != cargoOrderDetail.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(cargoOrderDetail).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CargoOrderDetailExists(id))
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

        // POST: api/CargoOrderDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("CargoOrder")]
        public async Task<ActionResult<CargoOrderDetail>> PostCargoOrderDetail(CargoOrderDetail cargoOrderDetail)
        {
            _context.CargoOrderDetails.Add(cargoOrderDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCargoOrderDetail", new { id = cargoOrderDetail.Id }, cargoOrderDetail);
        }

        // DELETE: api/CargoOrderDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CargoOrderDetail>> DeleteCargoOrderDetail(int id)
        {
            var cargoOrderDetail = await _context.CargoOrderDetails.FindAsync(id);
            if (cargoOrderDetail == null)
            {
                return NotFound();
           }

            _context.CargoOrderDetails.Remove(cargoOrderDetail);
            await _context.SaveChangesAsync();

            return cargoOrderDetail;
        }

        [HttpGet]
        [Route("GetOrderList")]
        public ActionResult<IEnumerable<CargoOrderInfo>> GetOrderList()
        {
            //return await _context.CargoOrderDetails.ToListAsync();
            var cargoList = (from c in _context.CargoOrderDetails
                             join cu in _context.Customers
                             on c.CustId equals cu.CustId
                             join ca in _context.Cargo
                             on c.CargoId equals ca.CargoId
                             select new CargoOrderInfo()
                             {
                                 CustId = cu.CustId,
                                 CustName = cu.CustName,
                                 CustPhNo = cu.CustPhNo,
                                 CustEmail = cu.CustEmail,
                                 CargoId = ca.CargoId,
                                 CargoName = ca.CargoName,
                                 CargoPrice = ca.Price,

                             }).ToList();
            return Ok(cargoList);
        }

        private bool CargoOrderDetailExists(int id)
        {
            return _context.CargoOrderDetails.Any(e => e.Id == id);
        }
    }
}
