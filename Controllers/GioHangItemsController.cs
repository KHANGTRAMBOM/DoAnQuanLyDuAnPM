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
using Microsoft.AspNetCore.Identity;

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


        // GET: GioHangItems/Create
        public IActionResult Create2(int BookId,string Usermail)
        {
            // Lấy người dùng dựa trên email

            var user = _context.Users.FirstOrDefault(model => model.Email == Usermail);

            if (user == null)
            {
                // Xử lý trường hợp không tìm thấy người dùng
                _logger.LogError("User null: " + Usermail);
                return NotFound(); // Hoặc chuyển hướng đến trang lỗi
            }

            // Lấy ID của người dùng
            var userId = user.Id; // Giả sử đây là kiểu int

            // Kiểm tra giá trị userId trước khi sử dụng
            if (userId == null)
            {
                // Xử lý trường hợp không tìm thấy ID
                _logger.LogError("UseId null");
                return NotFound(); // Hoặc chuyển hướng đến trang lỗi
            }

            // Tìm giỏ hàng dựa trên ID người dùng
            ViewData["GioHangId"] = _context.GioHang
                                            .FirstOrDefault(model => model.IDNguoiDung == userId);

            // Lấy sách đã chọn
            ViewData["SelectedBook"] = _context.Book
                                                .FirstOrDefault(model => model.Id == BookId);

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
                // Thêm GioHangItem vào cơ sở dữ liệu
                _context.Add(gioHangItem);

                // Lấy đối tượng GioHang tương ứng với GioHangId từ GioHangItem
                var gioHang = await _context.GioHang
                                  .FirstOrDefaultAsync(model => model.Id == gioHangItem.GioHangId);

                var book = await _context.Book
                                 .FirstOrDefaultAsync(model => model.Id == gioHangItem.BookId);


              
                    _logger.LogError("id book : " + gioHangItem.BookId);
                    _logger.LogError("id gio hang : " + gioHangItem.Id);
                    _logger.LogError("gio hang: " + gioHangItem.GioHangId);
                    _logger.LogError("so luong : " + gioHangItem.SoLuong);
                    _logger.LogError("gia : " + gioHangItem.Gia);
               
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
