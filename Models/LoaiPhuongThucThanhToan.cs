using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWebsite.Models
{
    public class LoaiPhuongThucThanhToan
    {
        [DisplayName("Mã kiểu thanh toán")]
        public int Id { get; set; }

        [DisplayName("Tên kiểu thanh toán")]
        [Required(ErrorMessage = "Phương thức thanh toán không được để trống.")]
        [StringLength(50, ErrorMessage = "Phương thức thanh toán không được vượt quá 50 ký tự.")]

        public string TenPhuongThucThanhToan { get; set; }

        [DisplayName("Khuyến mãi")]
        [Range(0, 100, ErrorMessage = @"{0}phải nằm trong khoảng từ 0 đến 100.")]
        [Column(TypeName = "decimal(18,2)")]
        [Required(ErrorMessage = @"{0} không được để trống.")]
        public decimal KhuyenMai { get; set; }


        // navigation properties

        public ICollection<ThanhToan> thanhToans { get; set; }
    }
}