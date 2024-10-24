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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Net;

namespace BookWebsite.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class GioHangItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GioHangItemsController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public GioHangItemsController(ApplicationDbContext context,ILogger<GioHangItemsController> logger , UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }
        

        // GET: GioHangItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GioHangItem
                .Include(g => g.Book)
                .Include(g => g.GioHang);

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
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "TenSach");
            ViewData["GioHangId"] = new SelectList(_context.GioHang, "Id", "IDNguoiDung");
            return View();
        }


        // GET: GioHangItems/Create2
        public async Task<IActionResult> Create2(int BookId)
        {
            // Lấy ID của người dùng
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // Tìm giỏ hàng dựa trên ID người dùng
            ViewData["GioHang"] = _context.GioHang
                                          .FirstOrDefault(g => g.IDNguoiDung == userId);

            // Lấy sách đã chọn
            ViewData["SelectedBook"] = _context.Book
                                                .FirstOrDefault(b => b.Id == BookId);

 
            return View();
        }

        // POST: GioHangItems/Create2
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2([Bind("Id,GioHangId,BookId,SoLuong,Gia")] GioHangItem gioHangItem)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;


            if (ModelState.IsValid)
            {
                var a = await _context.GioHangItem
                                .Where(model => model.GioHangId == gioHangItem.GioHangId)
                                .Where(model => model.BookId == gioHangItem.BookId)
                                .FirstOrDefaultAsync();

                var gioHang = await _context.GioHang
                                 .FirstOrDefaultAsync(model => model.Id == gioHangItem.GioHangId);


                var book = await _context.Book
                                 .FirstOrDefaultAsync(model => model.Id == gioHangItem.BookId);

                // đã có một cuốn sách giống loại muốn mua nằm trong giỏ hàng
                if (a != null)
                {
  
                    gioHang.TongTien += gioHangItem.SoLuong * book.Gia;

                    a.SoLuong += gioHangItem.SoLuong;


                }
                else
                {

                    _context.Add(gioHangItem);

                    //Gán lại giá của sách giống với model
                    gioHangItem.Gia = book.Gia;

                    if (gioHang != null)
                    {
                        // Cập nhật tổng tiền ngay lập tức
                        gioHang.TongTien += gioHangItem.SoLuong * book.Gia;


                    }
                }

                await _context.SaveChangesAsync();



                return RedirectToAction("Index2","Books");
            }



            ViewData["GioHang"] = _context.GioHang
                                         .FirstOrDefault(g => g.IDNguoiDung == userId);



            ViewData["SelectedBook"] = _context.Book
                                                .FirstOrDefault(b => b.Id == gioHangItem.BookId);
            
            return View(gioHangItem);
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
                // Thêm GioHangItem vào cơ sở dữ liệu
                _context.Add(gioHangItem);

                // Lấy đối tượng GioHang tương ứng với GioHangId từ GioHangItem
                var gioHang = await _context.GioHang
                                  .FirstOrDefaultAsync(model => model.Id == gioHangItem.GioHangId);

                var book = await _context.Book
                                 .FirstOrDefaultAsync(model => model.Id == gioHangItem.BookId);
        

                gioHangItem.Gia = book.Gia;
         

                if (gioHang != null)
                {
                    // Cập nhật tổng tiền ngay lập tức
                    gioHang.TongTien += gioHangItem.SoLuong * book.Gia;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "TenSach", gioHangItem.BookId);
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
            ViewData["BookId"] = new SelectList(_context.Book, "Id", "TenSach", gioHangItem.BookId);
            ViewData["GioHangId"] = new SelectList(_context.GioHang, "Id", "IDNguoiDung", gioHangItem.GioHangId);
            return View(gioHangItem);
        }


        // GET: GioHangItems/Edit/5
        public async Task<IActionResult> Edit2(int? id)
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
        public async Task<IActionResult> Edit2(int id, [Bind("Id,GioHangId,BookId,SoLuong,Gia")] GioHangItem gioHangItem)
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



        // GET: GioHangItems/Delete2/5
        public async Task<IActionResult> Delete2(int? id)
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

        // POST: GioHangItems/Delete2/5
        [HttpPost, ActionName("Delete2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed2(int id)
        {
            if (_context.GioHangItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GioHangItem'  is null.");
            }
            var gioHangItem = await _context.GioHangItem.FindAsync(id);

            var gioHang = await _context.GioHang.FirstOrDefaultAsync(m => m.Id == gioHangItem.GioHangId);

            gioHang.TongTien -= gioHangItem.SoLuong * gioHangItem.Gia;

            if (gioHangItem != null)
            {
                _context.GioHangItem.Remove(gioHangItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details","GioHangs", new { id = gioHang.Id });
        }

        private bool GioHangItemExists(int id)
        {
          return (_context.GioHangItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        private GioHangItem KiemTraTrungTenSach(int BookId)
        {
            return _context.GioHangItem.FirstOrDefault(model => model.BookId == BookId);
                             
        }
    }
}
