using BookWebsite.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWebsite.Models
{
    public class Book
    {
        // Mã sách 
        [DisplayName("Mã sách")]
        public int Id { get; set; }

        // Tên sách 
        [Required(ErrorMessage = "Tên sách không được để trống.")]
        [StringLength(255, ErrorMessage = "Tên sách không được vượt quá 255 ký tự.")]
        [DisplayName("Tên sách")]
        public string TenSach { get; set; }

        // Tác giả 
        [Required(ErrorMessage = "Tên tác giả không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên tác giả không được vượt quá 100 ký tự.")]
        [DisplayName("Tác giả")]
        public string TacGia { get; set; }

        // Giá sách 
        [Required(ErrorMessage = "Giá sách không được để trống.")]
        [Range(0, 1000000, ErrorMessage = "Giá phải nằm trong khoảng từ 0 đến 1,000,000.")]
        [DisplayName("Giá")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ", ApplyFormatInEditMode = true)] // Hiển thị giá dưới dạng tiền tệ VND
        public decimal Gia { get; set; }

        // Nhà xuất bản 
        [Required(ErrorMessage = "Nhà xuất bản không được để trống.")]
        [StringLength(150, ErrorMessage = "Tên nhà xuất bản không được vượt quá 150 ký tự.")]
        [DisplayName("Nhà xuất bản")]
        public string NXB { get; set; }

        // Năm xuất bản
        [Required(ErrorMessage = "Năm xuất bản không được để trống.")]
        [DisplayName("Năm xuất bản")]
        public DateTime NamXB { get; set; }

        // URL ảnh
        [Required(ErrorMessage = "URL ảnh không được để trống.")]
        [DisplayName("Ảnh")]
        public string UrlAnh { get; set; }

        // Mô tả 
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        [DisplayName("Mô tả")]
        public string MoTa { get; set; }

        // Khóa ngoại cho thể loại
        [ForeignKey("TheLoai")]
        [DisplayName("Thể loại")]
        public int TheLoaiId { get; set; }

        // Navigation properties
        public TheLoai? TheLoai { get; set; }
    }
}
