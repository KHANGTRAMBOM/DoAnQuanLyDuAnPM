using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BookWebsite.Models
{
    public class GioHangItem
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("GioHang")]
        public int GioHangId { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [DisplayName("Số lượng")]
        public int SoLuong { get; set; }

        [DisplayName("Giá")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ", ApplyFormatInEditMode = true)] // Hiển thị giá dưới dạng tiền tệ VND
        public decimal Gia { get; set; }

        // Navigation properties
        public GioHang GioHang { get; set; }
        public Book Book { get; set; }
    }
 
}
