using CarChecker.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarChecker.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace CarChecker.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class VehicleController : ControllerBase
    {
        ApplicationDbContext db;

        public VehicleController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<Vehicle> ChangedVehicles([FromQuery] DateTime since)
        {
            return db.Vehicles.Where(v => v.LastUpdated >= since).Include(v => v.Notes);
        }

        [HttpPut]
        public async Task<IActionResult> Details(Vehicle vehicle)
        {
            var id = vehicle.LicenseNumber;
            var existingNotes = (await db.Vehicles.AsNoTracking().Include(v => v.Notes).SingleAsync(v => v.LicenseNumber == id)).Notes;
            var retainedNotes = vehicle.Notes.ToLookup(n => n.InspectionNoteId);
            var notesToDelete = existingNotes.Where(n => !retainedNotes.Contains(n.InspectionNoteId));
            db.RemoveRange(notesToDelete);

            vehicle.LastUpdated = DateTime.Now;
            db.Vehicles.Update(vehicle);

            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
