using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateWebAPI.Models;

namespace RealEstateWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EstatesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public EstatesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Estates
        // show all estates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estate>>> GetEstate()
        {
            return await _context.Estate.ToListAsync();
        }

        // GET: api/Estates/5
        // show one estate with specific id
        [HttpGet("{id}")]
        public async Task<ActionResult<Estate>> GetEstate(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Estate estate = await _context.Estate.SingleOrDefaultAsync(m => m.EstateID == id);


            if (estate == null)
            {
                return NotFound();
            }

            return Ok(estate);
        }

        // GET: search by size
        [HttpGet("{smin},{smax}")]
        public async Task<ActionResult<IEnumerable<Estate>>> GetEstatesSize(int smin, int smax)
        {
            return await _context.Estate.Where(m => m.Size <= smax && m.Size >= smin).ToListAsync();
        }

        // GET: search by price
        [HttpGet("{pmin}-{pmax}")]
        public async Task<ActionResult<IEnumerable<Estate>>> GetEstatesPrice(int pmin, int pmax)
        {
            return await _context.Estate.Where(m => m.Price <= pmax && m.Price >= pmin).ToListAsync();
        }

        // PUT: api/Estates/5
        // edit estate       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstate(int id, Estate estate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != estate.EstateID)
            {
                return BadRequest(ModelState);
            }
            _context.Entry(estate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstateExists(id))
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

        // POST: api/Estates
        // create new estate
        [HttpPost]
        public async Task<IActionResult> PostEstate([FromBody] Estate estate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Estate.Add(estate);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EstateExists(estate.EstateID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetEstate", new { id = estate.EstateID }, estate);
        }

        // DELETE: api/Estates/5
        // delete estate with specific id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Estate>> DeleteEstate(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Estate estate = await _context.Estate.SingleOrDefaultAsync(m => m.EstateID == id);
            if (estate == null)
            {
                return NotFound();
            }
            _context.Estate.Remove(estate);
            await _context.SaveChangesAsync();

            return Ok(estate);
        }

        private bool EstateExists(int id)
        {
            return _context.Estate.Any(e => e.EstateID == id);
        }
    }
}
