using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HorseBase.Data;
using HorseBase.Models;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HorseBase.Controllers
{
    public class HorseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HorseController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Horse
        public async Task<IActionResult> Index(
            string breed,
            int? birthYear,
            string gender,
            int? height,
            int? price)
        {
            // Start with the base query
            var horsesQuery = _context.horses.Include(h => h.Breed).AsQueryable();

            // Apply filters if they are provided
            if (!string.IsNullOrEmpty(breed))
            {
                horsesQuery = horsesQuery.Where(h => h.Breed.Name == breed);
            }

            if (birthYear.HasValue)
            {
                horsesQuery = horsesQuery.Where(h => h.BirhtYear.Year == birthYear.Value);
            }

            if (!string.IsNullOrEmpty(gender))
            {
                horsesQuery = horsesQuery.Where(h => h.Gender == gender);
            }

            if (height.HasValue)
            {
                horsesQuery = horsesQuery.Where(h => h.Height <= height.Value);
            }

            if (price.HasValue)
            {
                horsesQuery = horsesQuery.Where(h => h.Price <= price.Value);
            }

            // Pass filtered data to the view
            var horses = await horsesQuery.ToListAsync();

            // Populate ViewBag for filter options
            ViewBag.Breeds = new SelectList(_context.breeds, "Name", "Name");

            return View(horses);
        }

        // GET: Horse/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horse = await _context.horses
                .Include(h => h.Breed)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horse == null)
            {
                return NotFound();
            }

            return View(horse);
        }

        // GET: Horse/Create
        public IActionResult Create()
        {
            ViewData["BreedId"] = new SelectList(_context.breeds, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Number,BreedId,BirthYear,Gender,Height,Price,PhotoPath")] Horse horse, IFormFile[] photos)
        {
            if (photos != null && photos.Length > 0)
            {
                var imagePaths = new List<string>();

                foreach (var photo in photos)
                {
                    if (photo.Length > 0)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(photo.FileName);
                        var extension = Path.GetExtension(photo.FileName);
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName + extension);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }
                        imagePaths.Add("/images/" + fileName + extension);
                    }
                }
                horse.PhotoPath = string.Join(",", imagePaths);
            }

            _context.Add(horse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Horse/Edit/5
        // GET: Horse/Edit/5
        // GET: Horse/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horse = await _context.horses.FindAsync(id);
            if (horse == null)
            {
                return NotFound();
            }

            // Pass the existing photos to the view
            ViewBag.ExistingPhotos = string.IsNullOrEmpty(horse.PhotoPath) ? new List<string>() : horse.PhotoPath.Split(',').ToList();

            ViewData["BreedId"] = new SelectList(_context.breeds, "Id", "Name", horse.BreedId);
            return View(horse);
        }

        // POST: Horse/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile[] photos, [Bind("Id,Number,BreedId,BirhtYear,Gender,Height,Price,PhotoPath")] Horse horse)
        {
            if (id != horse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle photo uploads
                    if (photos != null && photos.Length > 0)
                    {
                        var photoPaths = new List<string>();
                        foreach (var photo in photos)
                        {
                            if (photo.Length > 0)
                            {
                                var filePath = Path.Combine("wwwroot/images", Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName));
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await photo.CopyToAsync(stream);
                                }
                                photoPaths.Add("/images/" + Path.GetFileName(filePath));
                            }
                        }

                        // Combine new photos with existing ones
                        if (!string.IsNullOrEmpty(horse.PhotoPath))
                        {
                            photoPaths.AddRange(horse.PhotoPath.Split(','));
                        }

                        horse.PhotoPath = string.Join(",", photoPaths.Distinct());
                    }

                    _context.Update(horse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorseExists(horse.Id))
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

            ViewData["BreedId"] = new SelectList(_context.breeds, "Id", "Name", horse.BreedId);
            return View(horse);
        }

        private bool HorseExists(int id)
        {
            return _context.horses.Any(e => e.Id == id);
        }

        // GET: Horse/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var horse = await _context.horses
                .Include(h => h.Breed)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horse == null)
            {
                return NotFound();
            }

            return View(horse);
        }

        // POST: Horse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var horse = await _context.horses.FindAsync(id);
            if (horse != null)
            {
                _context.horses.Remove(horse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public string GetAllHorses()
        {
            // Check if the user is logged in and has the "User" or "Admin" role
            bool isLoged = User.IsInRole("User") || User.IsInRole("Admin");

            // Fetch the list of horses with their associated breed information
            var horses = _context.horses.Include(x => x.Breed).ToList();

            // Create an anonymous object to hold both the horses and the login status
            var result = new
            {
                Horses = horses,
                IsLoged = isLoged
            };

            // Serialize the result to JSON and return it
            return JsonConvert.SerializeObject(result);
        }

    }
}