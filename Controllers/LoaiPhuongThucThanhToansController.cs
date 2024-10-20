using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookWebsite.Data;
using BookWebsite.Models;

namespace BookWebsite.Controllers
{
    public class LoaiPhuongThucThanhToansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LoaiPhuongThucThanhToansController> _logger;

        public LoaiPhuongThucThanhToansController(ApplicationDbContext context, ILogger<LoaiPhuongThucThanhToansController> logger )
        {
            _context = context;
            _logger = logger;
        }

        // GET: LoaiPhuongThucThanhToans
        public async Task<IActionResult> Index()
        {
              return _context.LoaiPhuongThucThanhToan != null ? 
                          View(await _context.LoaiPhuongThucThanhToan.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.LoaiPhuongThucThanhToan'  is null.");
        }

        // GET: LoaiPhuongThucThanhToans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LoaiPhuongThucThanhToan == null)
            {
                return NotFound();
            }

            var loaiPhuongThucThanhToan = await _context.LoaiPhuongThucThanhToan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiPhuongThucThanhToan == null)
            {
                return NotFound();
            }

            return View(loaiPhuongThucThanhToan);
        }

        // GET: LoaiPhuongThucThanhToans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoaiPhuongThucThanhToans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenPhuongThucThanhToan,KhuyenMai")] LoaiPhuongThucThanhToan loaiPhuongThucThanhToan)
        {
            if (ModelState.IsValid)
            {
                var a = loaiPhuongThucThanhToan.KhuyenMai;
                _logger.LogError($"{a}");
                _context.Add(loaiPhuongThucThanhToan);
                TempData["CreateStatus"] = true;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiPhuongThucThanhToan);
        }

        // GET: LoaiPhuongThucThanhToans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LoaiPhuongThucThanhToan == null)
            {
                return NotFound();
            }

            var loaiPhuongThucThanhToan = await _context.LoaiPhuongThucThanhToan.FindAsync(id);
            if (loaiPhuongThucThanhToan == null)
            {
                return NotFound();
            }
            return View(loaiPhuongThucThanhToan);
        }

        // POST: LoaiPhuongThucThanhToans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenPhuongThucThanhToan,KhuyenMai")] LoaiPhuongThucThanhToan loaiPhuongThucThanhToan)
        {
            if (id != loaiPhuongThucThanhToan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(loaiPhuongThucThanhToan);
                    TempData["EditStatus"] = true;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiPhuongThucThanhToanExists(loaiPhuongThucThanhToan.Id))
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
            return View(loaiPhuongThucThanhToan);
        }

        // GET: LoaiPhuongThucThanhToans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LoaiPhuongThucThanhToan == null)
            {
                return NotFound();
            }

            var loaiPhuongThucThanhToan = await _context.LoaiPhuongThucThanhToan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loaiPhuongThucThanhToan == null)
            {
                return NotFound();
            }

            return View(loaiPhuongThucThanhToan);
        }

        // POST: LoaiPhuongThucThanhToans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LoaiPhuongThucThanhToan == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LoaiPhuongThucThanhToan'  is null.");
            }
            var loaiPhuongThucThanhToan = await _context.LoaiPhuongThucThanhToan.FindAsync(id);
            if (loaiPhuongThucThanhToan != null)
            {
                _context.LoaiPhuongThucThanhToan.Remove(loaiPhuongThucThanhToan);
            }

            TempData["DeleteStatus"] = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiPhuongThucThanhToanExists(int id)
        {
          return (_context.LoaiPhuongThucThanhToan?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
