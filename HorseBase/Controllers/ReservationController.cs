using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HorseBase.Models;
using System.Linq;
using System.Threading.Tasks;
using HorseBase.Data;
using HorseBase.Models.ViewModels;

namespace HorseBase.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservation/Create
        public async Task<IActionResult> Create(int horseId)
        {
            var horse = await _context.horses.FindAsync(horseId);
            if (horse == null)
            {
                return NotFound();
            }

            var reservation = new ReservationViewModel
            {
                HorseId = horse.Id,
                Horse = horse,
                Price = 0
            };

            return View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationViewModel reservationRequest)
        {
            if (ModelState.IsValid)
            {
                // Fetch the horse details
                var horse = await _context.horses.FindAsync(reservationRequest.HorseId);
                if (horse == null)
                {
                    return NotFound();
                }

                // Check for overlapping reservations
                bool isOverlapping = await _context.reservations.AnyAsync(r =>
                    r.Horse.Id == reservationRequest.HorseId &&
                    r.TakeHour < reservationRequest.ReturnHour &&
                    r.ReturnHour > reservationRequest.TakeHour);

                if (isOverlapping)
                {
                    ModelState.AddModelError("", "This horse is already booked for the selected time. Please choose another date.");
                    return View(reservationRequest);
                }

                // Fetch the current user
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
                if (user == null)
                {
                    return Unauthorized();
                }

                // Create the reservation
                Reservation reservation = new Reservation()
                {
                    Horse = horse,
                    Price = (double)reservationRequest.Price,
                    TakeHour = reservationRequest.TakeHour,
                    ReturnHour = reservationRequest.ReturnHour,
                    UserId = user.Id
                };

                // Save the reservation to the database
                _context.reservations.Add(reservation);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(reservationRequest);
        }

        [HttpGet]
        public async Task<IActionResult> CheckOverlaps(int horseId, DateTime takeHour, DateTime returnHour)
        {
            bool isOverlapping = await _context.reservations.AnyAsync(r =>
                r.Horse.Id == horseId &&
                r.TakeHour < returnHour &&
                r.ReturnHour > takeHour);

            return Json(new { isOverlapping });
        }
    }
}