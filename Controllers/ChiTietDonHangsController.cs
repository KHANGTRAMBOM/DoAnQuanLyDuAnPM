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
    public class ChiTietDonHangsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChiTietDonHangsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChiTietDonHangs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ChiTietDonHang.Include(c => c.Book).Include(c => c.DonHang);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ChiTietDonHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ChiTietDonHang == null)
            {
                return NotFound();
            }

            var chiTietDonHang = await _context.ChiTietDonHang
                .Include(c => c.Book)
                .Include(c => c.DonHang)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chiTietDonHang == null)
            {
                return NotFound();
            }

            return View(chiTietDonHang);
        }

        // GET: ChiTietDonHangs/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "NXB");
            ViewData["IDDonHang"] = new SelectList(_context.Set<DonHang>(), "Id", "IdUser");
            return View();
        }

        // POST: ChiTietDonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IDDonHang,BookId,SoLuong,Gia")] ChiTietDonHang chiTietDonHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTietDonHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "NXB", chiTietDonHang.BookId);
            ViewData["IDDonHang"] = new SelectList(_context.Set<DonHang>(), "Id", "IdUser", chiTietDonHang.IDDonHang);
            return View(chiTietDonHang);
        }

        // GET: ChiTietDonHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ChiTietDonHang == null)
            {
                return NotFound();
            }

            var chiTietDonHang = await _context.ChiTietDonHang.FindAsync(id);
            if (chiTietDonHang == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "NXB", chiTietDonHang.BookId);
            ViewData["IDDonHang"] = new SelectList(_context.Set<DonHang>(), "Id", "IdUser", chiTietDonHang.IDDonHang);
            return View(chiTietDonHang);
        }

        // POST: ChiTietDonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IDDonHang,BookId,SoLuong,Gia")] ChiTietDonHang chiTietDonHang)
        {
            if (id != chiTietDonHang.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTietDonHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietDonHangExists(chiTietDonHang.Id))
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
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "NXB", chiTietDonHang.BookId);
            ViewData["IDDonHang"] = new SelectList(_context.Set<DonHang>(), "Id", "IdUser", chiTietDonHang.IDDonHang);
            return View(chiTietDonHang);
        }

        // GET: ChiTietDonHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ChiTietDonHang == null)
            {
                return NotFound();
            }

            var chiTietDonHang = await _context.ChiTietDonHang
                .Include(c => c.Book)
                .Include(c => c.DonHang)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chiTietDonHang == null)
            {
                return NotFound();
            }

            return View(chiTietDonHang);
        }

        // POST: ChiTietDonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ChiTietDonHang == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ChiTietDonHang'  is null.");
            }
            var chiTietDonHang = await _context.ChiTietDonHang.FindAsync(id);
            if (chiTietDonHang != null)
            {
                _context.ChiTietDonHang.Remove(chiTietDonHang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChiTietDonHangExists(int id)
        {
          return (_context.ChiTietDonHang?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
