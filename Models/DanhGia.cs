using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWebsite.Models
{
    public class DanhGia
    {
        [DisplayName("Mã đánh giá")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Mã người dùng")]
        [ForeignKey("NguoiDung")]
        public string IDNguoiDung { get; set; } // ID người dùng

        [Required]
        [ForeignKey("Book")]
        [DisplayName("Mã sách")]
        public int BookId { get; set; } // ID sách

        [Required]
        [Range(1, 5, ErrorMessage = "Đánh giá phải nằm trong khoảng từ 1 đến 5.")]
        [DisplayName("Điễm đánh giá")]
        public int DiemDanhGia { get; set; } // Điểm đánh giá

        [StringLength(1000, ErrorMessage = "Nội dung đánh giá không được vượt quá 1000 ký tự.")]
        [DisplayName("Nội dung")]
        public string NoiDung { get; set; } // Nội dung đánh giá


        // Navigation property
        public NguoiDung NguoiDung { get; set; }

        public Book Book { get; set; }
    }
}
