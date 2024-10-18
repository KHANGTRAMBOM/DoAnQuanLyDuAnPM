using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWebsite.Models
{
    public class ChiTietDonHang
    {
        [DisplayName("Mã chi tiết đơn hàng")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("DonHang")]
        [DisplayName("Mã đơn hàng")]
        public int IDDonHang { get; set; } // ID của đơn hàng - khóa ngoại tham chiếu tới mã đơn hàng

        [Required]
        [ForeignKey("Book")]
        [DisplayName("Mã sách")]
        public int BookId { get; set; } // ID sách
    
        [Required]
        [DisplayName("Số lượng")]
        [Range(0,1000000,ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; } // Số lượng sách đã đặt

        [Required]
        [DisplayName("Giá")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ", ApplyFormatInEditMode = true)] 
        public decimal Gia { get; set; } // Giá sách tại thời điểm đặt hàng

        //Navigation
        public Book Book { get; set; }
        public DonHang DonHang { get; set; }
    }
}
