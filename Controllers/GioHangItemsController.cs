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
    public class GioHangItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GioHangItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GioHangItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GioHangItem.Include(g => g.Book).Include(g => g.GioHang);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GioHangItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GioHangItem == null)
            {
                return NotFound();
            }

            var gioHangItem = await _context.GioHangItem
                .Include(g => g.Book)
                .Include(g => g.GioHang)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gioHangItem == null)
            {
                return NotFound();
            }

            return View(gioHangItem);
        }

        // GET: GioHangItems/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "NXB");
            ViewData["GioHangId"] = new SelectList(_context.GioHang, "Id", "IDNguoiDung");
            return View();
        }

        // POST: GioHangItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GioHangId,BookId,SoLuong,Gia")] GioHangItem gioHangItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gioHangItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "NXB", gioHangItem.BookId);
            ViewData["GioHangId"] = new SelectList(_context.GioHang, "Id", "IDNguoiDung", gioHangItem.GioHangId);
            return View(gioHangItem);
        }

        // GET: GioHangItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GioHangItem == null)
            {
                return NotFound();
            }

            var gioHangItem = await _context.GioHangItem.FindAsync(id);
            if (gioHangItem == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "NXB", gioHangItem.BookId);
            ViewData["GioHangId"] = new SelectList(_context.GioHang, "Id", "IDNguoiDung", gioHangItem.GioHangId);
            return View(gioHangItem);
        }

        // POST: GioHangItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GioHangId,BookId,SoLuong,Gia")] GioHangItem gioHangItem)
        {
            if (id != gioHangItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gioHangItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GioHangItemExists(gioHangItem.Id))
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
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "NXB", gioHangItem.BookId);
            ViewData["GioHangId"] = new SelectList(_context.GioHang, "Id", "IDNguoiDung", gioHangItem.GioHangId);
            return View(gioHangItem);
        }

        // GET: GioHangItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GioHangItem == null)
            {
                return NotFound();
            }

            var gioHangItem = await _context.GioHangItem
                .Include(g => g.Book)
                .Include(g => g.GioHang)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gioHangItem == null)
            {
                return NotFound();
            }

            return View(gioHangItem);
        }

        // POST: GioHangItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GioHangItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GioHangItem'  is null.");
            }
            var gioHangItem = await _context.GioHangItem.FindAsync(id);
            if (gioHangItem != null)
            {
                _context.GioHangItem.Remove(gioHangItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GioHangItemExists(int id)
        {
          return (_context.GioHangItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
