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
using System.Data;

namespace BookWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DanhGiasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DanhGiasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DanhGias
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DanhGia.Include(d => d.Book).Include(d => d.NguoiDung);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DanhGias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DanhGia == null)
            {
                return NotFound();
            }

            var danhGia = await _context.DanhGia
                .Include(d => d.Book)
                .Include(d => d.NguoiDung)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (danhGia == null)
            {
                return NotFound();
            }

            return View(danhGia);
        }

        // GET: DanhGias/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "NXB");
            ViewData["IDNguoiDung"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id");
            return View();
        }

        // POST: DanhGias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IDNguoiDung,BookId,DiemDanhGia,NoiDung")] DanhGia danhGia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danhGia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "NXB", danhGia.BookId);
            ViewData["IDNguoiDung"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id", danhGia.IDNguoiDung);
            return View(danhGia);
        }

        // GET: DanhGias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DanhGia == null)
            {
                return NotFound();
            }

            var danhGia = await _context.DanhGia.FindAsync(id);
            if (danhGia == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "NXB", danhGia.BookId);
            ViewData["IDNguoiDung"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id", danhGia.IDNguoiDung);
            return View(danhGia);
        }

        // POST: DanhGias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IDNguoiDung,BookId,DiemDanhGia,NoiDung")] DanhGia danhGia)
        {
            if (id != danhGia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danhGia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhGiaExists(danhGia.Id))
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
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "NXB", danhGia.BookId);
            ViewData["IDNguoiDung"] = new SelectList(_context.Set<IdentityUser>(), "Id", "UserName", danhGia.IDNguoiDung);
            return View(danhGia);
        }

        // GET: DanhGias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DanhGia == null)
            {
                return NotFound();
            }

            var danhGia = await _context.DanhGia
                .Include(d => d.Book)
                .Include(d => d.NguoiDung)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (danhGia == null)
            {
                return NotFound();
            }

            return View(danhGia);
        }

        // POST: DanhGias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DanhGia == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DanhGia'  is null.");
            }
            var danhGia = await _context.DanhGia.FindAsync(id);
            if (danhGia != null)
            {
                _context.DanhGia.Remove(danhGia);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanhGiaExists(int id)
        {
          return (_context.DanhGia?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
