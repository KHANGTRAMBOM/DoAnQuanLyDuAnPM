using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookWebsite.Models
{
    public class DonHang
    {
        [DisplayName("Mã đơn hàng")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Mã người dùng")]
        [ForeignKey("NguoiDung")]
        public string IdUser { get; set; } // ID người dùng đã đặt hàng - Khóa ngoại tham chiếu đến mã người dùng 

        [DisplayName("Ngày đặt")]
        [DataType(DataType.Date)]
        public DateTime NgayDat { get; set; } = DateTime.Now; // Ngày đặt hàng

        [Required]
        [DisplayName("Tổng đơn hàng")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ", ApplyFormatInEditMode = true)]
        public decimal TongTien { get; set; } // Tổng tiền của đơn hàng


        //Navigation Properties
        public NguoiDung NguoiDung { get; set; }

    }
}
