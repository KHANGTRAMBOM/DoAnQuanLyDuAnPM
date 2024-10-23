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
using System.Data;

namespace BookWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ThanhToansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThanhToansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ThanhToans
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ThanhToan.Include(t => t.DonHang).Include(t => t.LoaiPhuongThucThanhToan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ThanhToans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ThanhToan == null)
            {
                return NotFound();
            }

            var thanhToan = await _context.ThanhToan
                .Include(t => t.DonHang)
                .Include(t => t.LoaiPhuongThucThanhToan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thanhToan == null)
            {
                return NotFound();
            }

            return View(thanhToan);
        }

        // GET: ThanhToans/Create
        public IActionResult Create()
        {
            ViewData["DonHangId"] = new SelectList(_context.DonHang, "Id", "IdUser");
            ViewData["PhuongThucThanhToanId"] = new SelectList(_context.LoaiPhuongThucThanhToan, "Id", "TenPhuongThucThanhToan");
            return View();
        }

        // POST: ThanhToans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DonHangId,PhuongThucThanhToanId,NgayThanhToan,Total,Status")] ThanhToan thanhToan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thanhToan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DonHangId"] = new SelectList(_context.DonHang, "Id", "IdUser", thanhToan.DonHangId);
            ViewData["PhuongThucThanhToanId"] = new SelectList(_context.LoaiPhuongThucThanhToan, "Id", "TenPhuongThucThanhToan", thanhToan.PhuongThucThanhToanId);
            return View(thanhToan);
        }

        // GET: ThanhToans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ThanhToan == null)
            {
                return NotFound();
            }

            var thanhToan = await _context.ThanhToan.FindAsync(id);
            if (thanhToan == null)
            {
                return NotFound();
            }
            ViewData["DonHangId"] = new SelectList(_context.DonHang, "Id", "IdUser", thanhToan.DonHangId);
            ViewData["PhuongThucThanhToanId"] = new SelectList(_context.LoaiPhuongThucThanhToan, "Id", "TenPhuongThucThanhToan", thanhToan.PhuongThucThanhToanId);
            return View(thanhToan);
        }

        // POST: ThanhToans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DonHangId,PhuongThucThanhToanId,NgayThanhToan,Total,Status")] ThanhToan thanhToan)
        {
            if (id != thanhToan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thanhToan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThanhToanExists(thanhToan.Id))
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
            ViewData["DonHangId"] = new SelectList(_context.DonHang, "Id", "IdUser", thanhToan.DonHangId);
            ViewData["PhuongThucThanhToanId"] = new SelectList(_context.LoaiPhuongThucThanhToan, "Id", "TenPhuongThucThanhToan", thanhToan.PhuongThucThanhToanId);
            return View(thanhToan);
        }

        // GET: ThanhToans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ThanhToan == null)
            {
                return NotFound();
            }

            var thanhToan = await _context.ThanhToan
                .Include(t => t.DonHang)
                .Include(t => t.LoaiPhuongThucThanhToan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (thanhToan == null)
            {
                return NotFound();
            }

            return View(thanhToan);
        }

        // POST: ThanhToans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ThanhToan == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ThanhToan'  is null.");
            }
            var thanhToan = await _context.ThanhToan.FindAsync(id);
            if (thanhToan != null)
            {
                _context.ThanhToan.Remove(thanhToan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThanhToanExists(int id)
        {
          return (_context.ThanhToan?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
