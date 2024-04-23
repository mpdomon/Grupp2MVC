using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Grupp2MVC.Data;
using Grupp2MVC.Models;
using Grupp2MVC.Models.ViewModels;
using Microsoft.IdentityModel.Tokens;

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

        public IActionResult Parking()
        {
            return View();
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
                // Check if the vehicle with the given registration number already exists
                var existingVehicle = await _context.Vehicle.FirstOrDefaultAsync(m => m.RegistrationNumber.Equals(vehicle.RegistrationNumber));
                if (existingVehicle != null)
                {
                    ModelState.AddModelError(string.Empty, "Vehicle with the same registration number already exists");
                    return View(vehicle);
                }

                // Parking capacity check
                var countOfParked = _context.Vehicle.Count(v => v.IsParked);
                if (countOfParked >= Garage.Capacity)
                {
                    ModelState.AddModelError(string.Empty, "Parking spaces are full");
                    return View(vehicle);
                }

                // Set time of arrival and mark the vehicle as parked
                vehicle.TimeOfArrival = DateTime.Now;
                vehicle.IsParked = true;

                _context.Add(vehicle);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    var receipt = new Receipt
                    {
                        VehicleId = vehicle.Id,
                        TimeOfArrival = vehicle.TimeOfArrival
                    };

                    _context.Add(receipt);
                    await _context.SaveChangesAsync();
                }
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
                // Check if the edited registration number already exists for another vehicle
                var existingVehicle = await _context.Vehicle.FirstOrDefaultAsync(m => m.RegistrationNumber.Equals(vehicle.RegistrationNumber) && m.Id != id);
                if (existingVehicle != null)
                {
                    ModelState.AddModelError(string.Empty, "Vehicle with the same registration number already exists");
                    return View(vehicle);
                }

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
                //Changed to park status instead of deleting.
                vehicle.IsParked = false;

                //Todo: move to other function
                var timeOfDeparture = DateTime.Now;
                var receipt = await _context.Receipts.FirstOrDefaultAsync(r => r.VehicleId == id);
                //Todo: add possible null reference handling
                receipt.TimeOfDeparture = timeOfDeparture;
                receipt.Price = CalculateParkingPrice(vehicle.TimeOfArrival, timeOfDeparture);

                await _context.SaveChangesAsync();

                var receiptViewModel = new ReceiptViewModel
                {
                    RegistrationNumber = vehicle.RegistrationNumber,
                    Make = vehicle.Make,
                    Model = vehicle.Model,
                    TimeOfArrival = vehicle.TimeOfArrival,
                    TimeOfDeparture = timeOfDeparture,
                    Price = (double)receipt.Price
                };

                return View("Receipt", receiptViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ParkAgain(int id)
        {
            var countOfParked = _context.Vehicle.Count(v => v.IsParked);
            if (countOfParked >= Garage.Capacity)
            {
                ModelState.AddModelError(string.Empty, "Parking spaces are full");
                var vehicles = await _context.Vehicle.ToListAsync();
                return View("Index", vehicles); 
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            vehicle.TimeOfArrival = DateTime.Now;
            vehicle.IsParked = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); 
        }

        [ActionName("ReceiptList")]
        public async Task<IActionResult> ReceiptList(int vehicleId)
        {
            var receipts = await _context.Receipts.Where(r => r.VehicleId == vehicleId)
                .Select(r => new ReceiptViewModel
                {
                    Price = (r.TimeOfDeparture != null) ? (double)r.Price : CalculateParkingPrice(r.TimeOfArrival, r.TimeOfDeparture),
                    TimeOfArrival = r.TimeOfArrival,
                    TimeOfDeparture = r.TimeOfDeparture
                }).ToListAsync();

            var model = new ReceiptListViewModel
            {
                VehicleId = vehicleId,
                Receipts = receipts
            };

            return View(model);
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }

        private static double CalculateParkingPrice(DateTime timeOfArrival, DateTime? timeOfDeparture)
        {
            double hourlyRate = 7.5;
            var timeDifference = new TimeSpan();

            if (timeOfDeparture != null)
                timeDifference = (DateTime)timeOfDeparture - timeOfArrival;
            else
                timeDifference = DateTime.Now - timeOfArrival;
            
            return hourlyRate * Math.Round((double)timeDifference.TotalSeconds / 3600, MidpointRounding.ToZero);
        }
        public async Task<int> GetParkedVehicleCount()
        {
            return await _context.Vehicle.CountAsync(v => v.IsParked);
        }

    }
}
