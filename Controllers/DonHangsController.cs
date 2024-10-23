using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookWebsite.Data;
using BookWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookWebsite.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class DonHangsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DonHangsController> _logger;

        public DonHangsController(ApplicationDbContext context, ILogger<DonHangsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: DonHangs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DonHang.Include(d => d.NguoiDung);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DonHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DonHang == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHang
                .Include(d => d.NguoiDung)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // GET: DonHangs/Create
        public IActionResult Create()
        {
            ViewData["IdUser"] = new SelectList(_context.Set<IdentityUser>(), "Id", "UserName");
            return View();
        }

        // POST: DonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUser,NgayDat,TongTien")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(donHang);
                await _context.SaveChangesAsync();
                _logger.LogError("Da them thanh cong");
                return RedirectToAction(nameof(Index));
                
            }

            ViewData["IdUser"] = new SelectList(_context.Set<IdentityUser>(), "Id", "UserName", donHang.IdUser);
            return View(donHang);
        }

        // GET: DonHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DonHang == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHang.FindAsync(id);
            if (donHang == null)
            {
                return NotFound();
            }
            ViewData["IdUser"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id", donHang.IdUser);
            return View(donHang);
        }

        // POST: DonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUser,NgayDat,TongTien")] DonHang donHang)
        {
            if (id != donHang.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.Id))
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
            ViewData["IdUser"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id", donHang.IdUser);
            return View(donHang);
        }

        // GET: DonHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DonHang == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHang
                .Include(d => d.NguoiDung)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // POST: DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DonHang == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DonHang'  is null.");
            }
            var donHang = await _context.DonHang.FindAsync(id);
            if (donHang != null)
            {
                _context.DonHang.Remove(donHang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonHangExists(int id)
        {
          return (_context.DonHang?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
