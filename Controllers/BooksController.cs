using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookWebsite.Data;
using BookWebsite.Models;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace BookWebsite.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BooksController> _logger;


        //[BindProperty] public IFormFile FileAnh { get; set; }
        public BooksController(ApplicationDbContext context, ILogger<BooksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Book.Include(b => b.TheLoai);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.TheLoai)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["TheLoaiId"] = new SelectList(_context.Set<TheLoai>(), "Id", "TenTheLoai");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenSach,TacGia,Gia,NXB,NamXB,UrlAnh,MoTa,TheLoaiId")] Book book,IFormFile FileAnh)
        {

            TempData["CreateStatus"] = null;
            if (FileAnh != null && FileAnh.Length > 0)
            {
                // Đường dẫn lưu file
                var filePath = Path.Combine("wwwroot/Image/", FileAnh.FileName);

                // Lưu file vào thư mục trên server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await FileAnh.CopyToAsync(stream);
                }
                book.UrlAnh = "Image/" + FileAnh.FileName;
            }
          
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                TempData["CreateStatus"] = true;
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    _logger.LogError(error);
                }
            }
            ViewData["TheLoaiId"] = new SelectList(_context.Set<TheLoai>(), "Id", "TenTheLoai", book.TheLoaiId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["TheLoaiId"] = new SelectList(_context.Set<TheLoai>(), "Id", "TenTheLoai", book.TheLoaiId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TenSach,TacGia,Gia,NXB,NamXB,UrlAnh,MoTa,TheLoaiId")] Book book,IFormFile FileAnh)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            TempData["EditStatus"] = null;

            /*        _logger.LogWarning($"ID: {book.Id}");
        _logger.LogWarning($"TenSach: {book.TenSach}");
        _logger.LogWarning($"Tacgia: {book.TacGia}");
        _logger.LogWarning($"Gia: {book.Gia}");
        _logger.LogWarning($"NXB: {book.NXB}");
        _logger.LogWarning($"Namxb: {book.NamXB}");
        _logger.LogWarning($"UrlAnh: {book.UrlAnh}");
        _logger.LogWarning($"Mota: {book.MoTa}");
        _logger.LogWarning($"TheLoaiID: {book.TheLoaiId}");
        _logger.LogWarning($"FileAnh: {FileAnh.FileName}");*/




            if (FileAnh != null && FileAnh.Length > 0)
            {
                // Đường dẫn lưu file
                var filePath = Path.Combine("wwwroot/Image/", FileAnh.FileName);

                // Lưu file vào thư mục trên server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await FileAnh.CopyToAsync(stream);
                }

                // Gán đường dẫn tương đối của ảnh vào thuộc tính UrlAnh
                book.UrlAnh = "Image/" + FileAnh.FileName;
             
            }



            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    TempData["EditStatus"] = true;
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError("Có lỗi xảy ra khi chỉnh sửa: " + ex.Message);
                }
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    _logger.LogError(error);
                }
            }

            ViewData["TheLoaiId"] = new SelectList(_context.Set<TheLoai>(), "Id", "TenTheLoai", book.TheLoaiId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.TheLoai)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Book'  is null.");
            }
            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                _context.Book.Remove(book);
            }

            TempData["DeleteStatus"] = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
