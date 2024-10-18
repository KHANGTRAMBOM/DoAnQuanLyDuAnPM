using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWebsite.Models
{
    public class GioHang
    {
        [DisplayName("Mã giỏ hàng")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("NguoiDung")]
        [DisplayName("Mã người dùng")]
        public string IDNguoiDung { get; set; }

        [DisplayName("Tổng tiền")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ", ApplyFormatInEditMode = true)] // Hiển thị giá dưới dạng tiền tệ VND
        public decimal TongTien { get; set; }

        // Navigation property
        public NguoiDung NguoiDung { get; set; }
        public ICollection<GioHangItem> GioHangItems { get; set; } = new List<GioHangItem>();
    }

}