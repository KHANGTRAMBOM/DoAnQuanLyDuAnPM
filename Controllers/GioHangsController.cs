using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookWebsite.Data;
using BookWebsite.Models;
using Microsoft.AspNetCore.Identity;

namespace BookWebsite.Controllers
{
    public class GioHangsController : Controller
    {
        private readonly ILogger<GioHangsController> _logger;
        private readonly ApplicationDbContext _context;

        public GioHangsController(ApplicationDbContext context, ILogger<GioHangsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: GioHangs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GioHang.Include(g => g.NguoiDung);
   
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GioHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GioHang == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHang
                .Include(g => g.NguoiDung)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (gioHang == null)
            {
                return NotFound();
            }

            // Lay sach cua gio hang ra
            var gioHangitems = await _context.GioHangItem
                                .Where(item => item.GioHangId == id)
                                .Include(item => item.Book) 
                                .ToListAsync();

           
            ViewBag.gioHangitem = gioHangitems.ToList();
       

            return View(gioHang);
        }

        // GET: GioHangs/Create
        public IActionResult Create()
        {
            ViewData["IDNguoiDung"] = new SelectList(_context.Set<IdentityUser>(), "Id", "UserName");
            return View();  
        }

        // POST: GioHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IDNguoiDung,TongTien")] GioHang gioHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gioHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            _logger.LogError(gioHang.Id + "");
            _logger.LogError(gioHang.IDNguoiDung + "");
            _logger.LogError(gioHang.TongTien + "");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    _logger.LogError(error);
                }
            }


            ViewData["IDNguoiDung"] = new SelectList(_context.Set<IdentityUser>(), "Id", "UserName", gioHang.IDNguoiDung);
            return View(gioHang);
        }

        // GET: GioHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GioHang == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHang.FindAsync(id);
            if (gioHang == null)
            {
                return NotFound();
            }
            ViewData["IDNguoiDung"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id", gioHang.IDNguoiDung);
            return View(gioHang);
        }

        // POST: GioHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IDNguoiDung,TongTien")] GioHang gioHang)
        {
            if (id != gioHang.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gioHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GioHangExists(gioHang.Id))
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
            ViewData["IDNguoiDung"] = new SelectList(_context.Set<IdentityUser>(), "Id", "Id", gioHang.IDNguoiDung);
            return View(gioHang);
        }

        // GET: GioHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GioHang == null)
            {
                return NotFound();
            }

            var gioHang = await _context.GioHang
                .Include(g => g.NguoiDung)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gioHang == null)
            {
                return NotFound();
            }

            return View(gioHang);
        }

        // POST: GioHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GioHang == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GioHang'  is null.");
            }
            var gioHang = await _context.GioHang.FindAsync(id);
            if (gioHang != null)
            {
                _context.GioHang.Remove(gioHang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GioHangExists(int id)
        {
          return (_context.GioHang?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        //Khi dang nhap click vao gio hang se hien thi chi tiet gio hang cua nguoi dung do luon chi tro phep xem thoi
        public async Task<IActionResult> GetUserCart()
        {
            // Lấy UserId từ người dùng hiện tại
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;

     

            // Truy vấn để lấy giỏ hàng của người dùng
            var gioHang = await _context.GioHang
                            .Where(g => g.IDNguoiDung == userId)
                            .FirstOrDefaultAsync();


      
            if (gioHang == null)
            {
                return NotFound("Giỏ hàng không tồn tại.");
            }

            // Trả về giỏ hàng hoặc mã giỏ hàng
            return RedirectToAction("Details", new { id = gioHang.Id });
        }

        public async Task<IActionResult> TaoDonHang()
        {
            // Lấy UserId từ người dùng hiện tại
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            var gioHang = await _context.GioHang
                            .Where(g => g.IDNguoiDung == userId)
                            .FirstOrDefaultAsync();

            var gioHangItems = await _context.GioHangItem
                                .Where(model => model.GioHangId == gioHang.Id)
                                .ToListAsync();

            // Tạo đơn hàng
            var DonHang = new DonHang
            {
                IdUser = userId,
                NgayDat = DateTime.Now,
                TongTien = gioHang.TongTien
            };

            await _context.DonHang.AddAsync(DonHang);

            // Lưu đơn hàng để tạo Id
            await _context.SaveChangesAsync();

            // Tạo chi tiết hóa đơn
            foreach (var gioHangItem in gioHangItems)
            {
                var ChiTietDonHang = new ChiTietDonHang
                {
                    IDDonHang = DonHang.Id,
                    BookId = gioHangItem.BookId,
                    SoLuong = gioHangItem.SoLuong,
                    Gia = gioHangItem.Gia
                };
                await _context.ChiTietDonHang.AddAsync(ChiTietDonHang);
            }

            // Tạo thanh toán
            var ThanhToan = new ThanhToan
            {
                NgayThanhToan = DateTime.Now,
                DonHangId = DonHang.Id,
                Status = false,
                PhuongThucThanhToanId = _context.LoaiPhuongThucThanhToan
                                            .Where(model => model.TenPhuongThucThanhToan == "Tiền mặt")
                                            .FirstOrDefault().Id,
                Total = DonHang.TongTien
            };

            await _context.ThanhToan.AddAsync(ThanhToan);

            // Xóa các mặt hàng trong giỏ hàng
            _context.GioHangItem.RemoveRange(gioHangItems);
            gioHang.TongTien = 0;

            // Lưu tất cả thay đổi
            await _context.SaveChangesAsync();

            // Trả về giỏ hàng hoặc mã giỏ hàng
            return RedirectToAction("Index2", "ThanhToans", new { userId });
        }

    }
}
