using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWebsite.Models
{
    public class ThanhToan
    {
        // ID thanh toán
        [DisplayName("Mã thanh toán")]
        public int Id { get; set; }

        // Khóa ngoại liên kết đến bảng Order
        [Required(ErrorMessage = "ID đơn hàng không được để trống.")]
        [DisplayName("Mã đơn hàng")]
        [ForeignKey("DonHang")]
        public int DonHangId { get; set; }

        // Phương thức thanh toán

        [Required(ErrorMessage = "Phương thức thanh toán không được để trống.")]
        [StringLength(50, ErrorMessage = "Phương thức thanh toán không được vượt quá 50 ký tự.")]
        [DisplayName("Phương thức thanh toán")]
        [ForeignKey("LoaiPhuongThucThanhToan")]
        public int PhuongThucThanhToanId { get; set; }

        // Ngày thanh toán
        [Required(ErrorMessage = "Ngày thanh toán không được để trống.")]
        [DisplayName("Ngày thanh toán")]
        [DataType(DataType.Date)]
        public DateTime NgayThanhToan { get; set; } = DateTime.Now; // Ngày thanh toán

        // Số tiền thanh toán
        [Required(ErrorMessage = "Số tiền thanh toán không được để trống.")]
        [DisplayName("Số tiền thanh toán")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ", ApplyFormatInEditMode = true)] // Hiển thị số tiền dưới dạng tiền tệ VND
        public decimal Total { get; set; }

        // Trạng thái thanh toán
        [DisplayName("Trạng thái thanh toán")]
        public Boolean Status { get; set; }

       // Navigation Properties
        public DonHang DonHang { get; set; }

        public LoaiPhuongThucThanhToan LoaiPhuongThucThanhToan { get; set; }
    }
}
