using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grupp2MVC.Data;
using Grupp2MVC.Models;
using Grupp2MVC.ViewModels;

namespace Grupp2MVC.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly Grupp2MVCContext _context;

        public VehiclesController(Grupp2MVCContext context)
        {
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vehicle.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Filter(string registrationNumber, int? vehicleType)
        {
            var model = string.IsNullOrWhiteSpace(registrationNumber) ?
                _context.Vehicle :
                _context.Vehicle.Where(m => m.RegistrationNumber.Contains(registrationNumber));

            model = vehicleType is null ?
                model :
                model.Where(m => (int)m.VehicleType == vehicleType);

            return View(nameof(Index), await model.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VehicleType,RegistrationNumber,Color,Make,Model,AmountOfWheels,TimeOfArrival")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleType,RegistrationNumber,Color,Make,Model,AmountOfWheels,TimeOfArrival")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);

            if (vehicle != null)
            {
                //Todo: move to other function?
                var timeOfDeparture = DateTime.Now;
                var receipt = new Receipt
                {
                    VehicleId = vehicle.Id,
                    TimeOfArrival = vehicle.TimeOfArrival,
                    TimeOfDeparture = timeOfDeparture,
                    Price = CalculateParkingPrice(vehicle.TimeOfArrival, timeOfDeparture)
                };
                
                _context.Receipts.Add(receipt);

                await _context.SaveChangesAsync();

                var receiptViewModel = new ReceiptViewModel
                {
                    RegistrationNumber = vehicle.RegistrationNumber,
                    Make = vehicle.Make,
                    Model = vehicle.Model,
                    TimeOfArrival = vehicle.TimeOfArrival,
                    TimeOfDeparture = timeOfDeparture,
                    Price = CalculateParkingPrice(vehicle.TimeOfArrival, timeOfDeparture)
                };

                //await _context.SaveChangesAsync();

                if (receipt.Price > 0)
                    return View("Receipt", receiptViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }

        private double CalculateParkingPrice(DateTime timeOfArrival, DateTime timeOfDeparture)
        {
            double hourlyRate = 0.75;

            var timeDifference = timeOfDeparture - timeOfArrival;
            return hourlyRate * Math.Round((double)timeDifference.TotalSeconds / 3600, 0);
        }
    }
}
