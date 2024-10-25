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
    [Authorize(Roles = "Admin,Member")]
    public class ThanhToansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ThanhToansController> _logger;

        public ThanhToansController(ApplicationDbContext context, ILogger<ThanhToansController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: ThanhToans
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ThanhToan.Include(t => t.DonHang).Include(t => t.LoaiPhuongThucThanhToan);
            return View(await applicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> Index2(string userId)
        {
            // Truy vấn `ThanhToan` theo `userId`
            var applicationDbContext = _context.ThanhToan
                                         .Include(t => t.DonHang)
                                         .Include(t => t.LoaiPhuongThucThanhToan)
                                         .Where(t => t.DonHang.IdUser == userId);
            

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

        public async Task<IActionResult> GetUserWallet()
        {
            // Lấy UserId từ người dùng hiện tại
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            return RedirectToAction("Index2", new { userId = userId });
        }

        public async Task<IActionResult> ThanhToan(int? id)
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

            var ChiTietDonHang = await _context.ChiTietDonHang
                                .Where(model => model.IDDonHang == thanhToan.DonHangId)
                                .Include(mode => mode.Book)
                                .ToListAsync();


            ViewBag.gioHangitem = ChiTietDonHang;
            ViewBag.thanhToan = thanhToan;
            ViewData["PhuongThucThanhToanId"] = new SelectList(_context.LoaiPhuongThucThanhToan, "Id", "TenPhuongThucThanhToan", thanhToan.PhuongThucThanhToanId);
            return View(thanhToan);
        }

        // POST: ThanhToan2
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThanhToan([Bind("THANHTOANID")]int ThanhToanId, [Bind("PhuongThucThanhToanId")] int PhuongThucThanhToanId)
        {

            var UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            // Tìm đối tượng thanh toán
            var ThanhToan = await _context.ThanhToan
                .Include(t => t.DonHang)
                .Include(t => t.LoaiPhuongThucThanhToan)
                .FirstOrDefaultAsync(m => m.Id == ThanhToanId);

            var PhuongThucThanhToan = await _context.LoaiPhuongThucThanhToan
                                                .Where(model => model.Id == PhuongThucThanhToanId)
                                                .FirstOrDefaultAsync();
            // Kiểm tra nếu thanhToan null
            if (ThanhToan == null)
            {
                _logger.LogError($"Thanh toán  Id {ThanhToanId} không tìm thấy.");
                return NotFound("Không tìm thấy thông tin thanh toán.");
            }


            TempData["CreateStatus"] = true;

            ThanhToan.PhuongThucThanhToanId = PhuongThucThanhToan.Id;

            ThanhToan.Total = ThanhToan.Total * (1 - PhuongThucThanhToan.KhuyenMai);

            ThanhToan.Status = true;

            _context.SaveChanges();

            return RedirectToAction("Index2", new { userId = UserId });
        }


    }
}
