
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarInsurance.Models;
using CarInsurance.Database;

public class InsureesController : Controller
{
    private readonly InsuranceDbContext _context;

    public InsureesController(InsuranceDbContext context)
    {
        _context = context;
    }

    // GET: INSUREES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Insurees.ToListAsync());
    }

    // GET: INSUREES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var insuree = await _context.Insurees
            .FirstOrDefaultAsync(m => m.Id == id);
        if (insuree == null)
        {
            return NotFound();
        }

        return View(insuree);
    }

    // GET: INSUREES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: INSUREES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
    {
        if (ModelState.IsValid)
        {
            decimal quote = 50;

            int age = DateTime.Now.Year - insuree.DateOfBirth.Year;

            if (insuree.DateOfBirth.Date > DateTime.Now.AddYears(-age))
            {
                age--;
            }

            if (age <= 18)
            {
                quote += 100;
            }
            else if (age >= 19 && age <= 25)
            {
                quote += 50;
            }
            else
            {
                quote += 25;
            }

            if (insuree.CarYear < 2000)
            {
                quote += 25;
            }

            if (insuree.CarYear > 2015)
            {
                quote += 25;
            }

            if (insuree.CarMake == "Porsche")
            {
                quote += 25;

                if (insuree.CarModel == "911 Carrera")
                {
                    quote += 25;
                }
            }

            quote += insuree.SpeedingTickets * 10;

            if (insuree.DUI)
            {
                quote *= 1.25m;
            }

            if (insuree.CoverageType == "Full")
            {
                quote *= 1.50m;
            }

            insuree.Quote = quote;

            _context.Add(insuree);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(insuree);
    }

    // GET: INSUREES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var insuree = await _context.Insurees.FindAsync(id);
        if (insuree == null)
        {
            return NotFound();
        }
        return View(insuree);
    }

    // POST: INSUREES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
    {
        if (id != insuree.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(insuree);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsureeExists(insuree.Id))
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
        return View(insuree);
    }

    // GET: INSUREES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var insuree = await _context.Insurees
            .FirstOrDefaultAsync(m => m.Id == id);
        if (insuree == null)
        {
            return NotFound();
        }

        return View(insuree);
    }

    // POST: INSUREES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var insuree = await _context.Insurees.FindAsync(id);
        if (insuree != null)
        {
            _context.Insurees.Remove(insuree);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Admin()
    {
        return View(await _context.Insurees.ToListAsync());
    }

    private bool InsureeExists(int? id)
    {
        return _context.Insurees.Any(e => e.Id == id);
    }
}
