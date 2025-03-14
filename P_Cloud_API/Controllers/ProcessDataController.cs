using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P_Cloud_API.Models;

namespace P_Cloud_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessDataController : ControllerBase
    {
        private readonly cloudapixContext _context;

        public ProcessDataController(cloudapixContext context)
        {
            _context = context;
        }

        // GET: api/ProcessData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ProcessData>>> GetRecordsInTimeframe(int id, DateTime? start = null, DateTime? end = null)
        {
            DateTime minDate = new DateTime(1753, 1, 1); // das kleinste Datum, das von SQL Server unterstützt wird
            DateTime maxDate = new DateTime(9999, 12, 31, 23, 59, 59); // das größte Datum, das von SQL Server unterstützt wird

            //Überprüfen, ob id gültig ist

            var isIdValid = await _context.ProcessData
                                .Where(r => r.ControlModuleId == id)
                                .ToListAsync();
            if (isIdValid.Any())
            {


                // Überprüfen, ob Start- und Endzeit angegeben wurden
                if (start == null && end != null)
            {
                start = minDate;
            }
            else if (start != null && end == null)
            {
                end = maxDate;
            }
            else if (!start.HasValue && !end.HasValue)
            {
                start = minDate;
                end = maxDate;
            }

            var records = await _context.ProcessData
                                .Where(r => (r.Timestamp >= start && r.Timestamp <= end) && (r.ControlModuleId == id))
                                .Include(x => x.EditType)
                                .Include(x => x.Status)
                                .ToListAsync();

                return records;
            }

            return NotFound();
        }


        // POST: api/ProcessData
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<string>> PostProcessData(ProcessData processData)
        {
            var listOfTimestamps = await _context.ProcessData
                    .Where(r => r.ControlModuleId == processData.ControlModuleId)
                    .Select(x => x.Timestamp)
                    .ToListAsync();
            if (listOfTimestamps.Any() && listOfTimestamps.Max() >= processData.Timestamp)
            {
                // if the list of timestamps contains at least one item and its maximum value is greater than or equal to processData.Timestamp, return a BadRequest result
                return BadRequest("The timestamp is not greater than all existing timestamps for this ControlModule.");
            }
            else
            {
                // otherwise, add Object to database
                processData.UserIp = HttpContext.Items["RemoteIpAddress"] as string;
                processData.Username = HttpContext.Items["Username"].ToString();
                processData.EditTypeId = 1;
                _context.ProcessData.Add(processData);
                await _context.SaveChangesAsync();

                var savedProcessData = await _context.ProcessData
                    .Include(x => x.EditType)
                    .Include(x => x.Status)
                    .FirstOrDefaultAsync(x => x.Id == processData.Id);

                return CreatedAtAction(nameof(GetRecordsInTimeframe), new { id = savedProcessData.Id }, savedProcessData);

            }

        }

        private bool ProcessDataExists(int id)
        {
            return _context.ProcessData.Any(e => e.Id == id);
        }
    }
}
