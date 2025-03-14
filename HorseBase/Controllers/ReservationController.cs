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
                Price = 0 // Initial value, dynamically updated based on user input
            };

            return View(reservation);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReservationViewModel reservationRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefaultAsync();
                // Fetch horse details to calculate price
                var horse = await _context.horses.FindAsync(reservationRequest.HorseId);

                if (horse == null)
                {
                    return NotFound();
                }
                Reservation reservation = new Reservation()
                {
                    Horse = horse,
                    Price = (double)reservationRequest.Price,
                    TakeHour = reservationRequest.TakeHour,
                    ReturnHour = reservationRequest.ReturnHour,
                    UserId = user.Id
                };

                _context.reservations.Add(reservation);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            return View(reservationRequest);
        }

    }
}
