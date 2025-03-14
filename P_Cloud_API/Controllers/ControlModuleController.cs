using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using P_Cloud_API.Models;

namespace P_Cloud_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ControlModuleController : ControllerBase
    {
        private readonly cloudapixContext _context;

        public ControlModuleController(cloudapixContext context)
        {
            _context = context;
        }

        // GET: api/ControlModules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ControlModule>> GetControlModule(int id)
        {
            var controlModule = await _context.ControlModules
                .Where(r => r.Id == id)
                .Include(x => x.ControlModuleInfo)
                    .ThenInclude(x => x.EditType)
                .Include(x => x.ControlModuleInfo)
                    .ThenInclude(x => x.PhysicalUnit)
                .Include(x => x.ControlModuleInfo)
                    .ThenInclude(x => x.Status)
                .FirstOrDefaultAsync();


            if (controlModule == null)
            {
                return NotFound();
            }

            return controlModule;
        }

        // PUT: api/ControlModules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutControlModule(int id, ControlModuleInfo controlModuleInfo)
        {
            var controlModule = await _context.ControlModules.FindAsync(id);

            if (id != controlModule.Id)
            {
                return BadRequest();
            }

            try
            {

                // Create a new ControlModuleInfo with modifications
                controlModuleInfo.ControlModuleId = id;
                controlModuleInfo.Timestamp = DateTime.Now;
                controlModuleInfo.UserIp = HttpContext.Items["RemoteIpAddress"] as string;
                controlModuleInfo.Username = HttpContext.Items["Username"].ToString();
                //Set EditType to edit
                controlModuleInfo.EditTypeId = 2;
                _context.ControlModuleInfos.Add(controlModuleInfo);
                await _context.SaveChangesAsync();

                
                // Update the ControlModule with the new ControlModuleInfo
                controlModule.ControlModuleInfoId = controlModuleInfo.Id;
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ControlModuleExists(id))
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

       

        // POST: api/ControlModules
        [HttpPost]
        public async Task<ActionResult<ControlModule>> PostControlModule([FromQuery] string path, ControlModuleInfo controlModuleInfo)
        {
            // Split the path into its components
            string[] pathComponents = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            // Make sure path was submitted
            if (string.IsNullOrEmpty(path))
            {
                return BadRequest("Path is required");
            }

            // Ensure the path has exactly 6 components
            if (pathComponents.Length != 6)
            {
                return BadRequest("Invalid path");
            }

            // Get the corresponding IDs for each level of the path
            int enterpriseId = await _context.Enterprises.Where(e => e.Name == pathComponents[0]).Select(e => e.Id).FirstOrDefaultAsync();
            int siteId = await _context.Sites.Where(s => s.EnterpriseId == enterpriseId && s.Name == pathComponents[1]).Select(s => s.Id).FirstOrDefaultAsync();
            int areaId = await _context.Areas.Where(a => a.SiteId == siteId && a.Name == pathComponents[2]).Select(a => a.Id).FirstOrDefaultAsync();
            int processCellId = await _context.ProcessCells.Where(pc => pc.AreaId == areaId && pc.Name == pathComponents[3]).Select(pc => pc.Id).FirstOrDefaultAsync();
            int unitId = await _context.Units.Where(u => u.ProcessCellId == processCellId && u.Name == pathComponents[4]).Select(u => u.Id).FirstOrDefaultAsync();
            int equipmentModuleId = await _context.EquipmentModules.Where(em => em.UnitId == unitId && em.Name == pathComponents[5]).Select(em => em.Id).FirstOrDefaultAsync();

            // Ensure the equipment module ID was found
            if (equipmentModuleId == 0)
            {
                return BadRequest("Equipment module not found");
            }

            var controlModule = new ControlModule
            {
                // Set the equipment module ID on the control module
                EquipmentModuleId = equipmentModuleId
            };


            // Add the control module to the context and save changes
            _context.ControlModules.Add(controlModule);
            await _context.SaveChangesAsync();

            // Create a new control module info object with the "New entry created" edit type and link the controlmodul to its control module info object
            controlModuleInfo.ControlModuleId = controlModule.Id;
            controlModuleInfo.Timestamp = DateTime.Now;
            controlModuleInfo.UserIp = HttpContext.Items["RemoteIpAddress"] as string;
            controlModuleInfo.Username = HttpContext.Items["Username"].ToString();
            controlModuleInfo.EditTypeId = 1;
            // Add the control module info to the context and save changes
            _context.ControlModuleInfos.Add(controlModuleInfo);
            await _context.SaveChangesAsync();

            // Link the controlmoduleInfo to its control module object
            controlModule.ControlModuleInfoId = controlModuleInfo.Id;
            await _context.SaveChangesAsync();

            // Return the newly created control module
            var savedControlModule = await _context.ControlModules
               .Include(x => x.ControlModuleInfo)
                   .ThenInclude(x => x.EditType)
               .Include(x => x.ControlModuleInfo)
                   .ThenInclude(x => x.PhysicalUnit)
               .Include(x => x.ControlModuleInfo)
                   .ThenInclude(x => x.Status)
               .FirstOrDefaultAsync(x => x.Id == controlModule.Id);

            return CreatedAtAction(nameof(GetControlModule), new { id = savedControlModule.Id }, savedControlModule);
        }

        // DELETE: api/ControlModules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteControlModule(int id)
        {
            var controlModule = await _context.ControlModules.FindAsync(id);

            if (controlModule == null)
            {
                return NotFound();
            }
            try
            {
                // Create a new ControlModuleInfo with modifications
                var oldControlModuleInfo = await _context.ControlModuleInfos.FindAsync(controlModule.ControlModuleInfoId);
                var DeletedControlModuleInfo = new ControlModuleInfo();

                DeletedControlModuleInfo.ControlModuleId = oldControlModuleInfo.ControlModuleId;
                DeletedControlModuleInfo.Timestamp = DateTime.Now;
                DeletedControlModuleInfo.UserIp = HttpContext.Items["RemoteIpAddress"] as string;
                DeletedControlModuleInfo.Username = HttpContext.Items["Username"].ToString();
                DeletedControlModuleInfo.EditTypeId = 3; //3 = Deleted
                DeletedControlModuleInfo.StatusId = oldControlModuleInfo.StatusId; 
                DeletedControlModuleInfo.Name = oldControlModuleInfo.Name;
                DeletedControlModuleInfo.Type = oldControlModuleInfo.Type;
                DeletedControlModuleInfo.Tolerance = oldControlModuleInfo.Tolerance;
                DeletedControlModuleInfo.RangeLowerEnd = oldControlModuleInfo.RangeLowerEnd;
                DeletedControlModuleInfo.RangeUpperEnd = oldControlModuleInfo.RangeUpperEnd;
                DeletedControlModuleInfo.PhysicalUnitId = oldControlModuleInfo.PhysicalUnitId;


                _context.ControlModuleInfos.Add(DeletedControlModuleInfo);
                await _context.SaveChangesAsync();

                // Update the ControlModule with the new ControlModuleInfo
                controlModule.ControlModuleInfoId = DeletedControlModuleInfo.Id;
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ControlModuleExists(id))
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

        private bool ControlModuleExists(int id)
        {
            return _context.ControlModules.Any(e => e.Id == id);
        }
    }
}
