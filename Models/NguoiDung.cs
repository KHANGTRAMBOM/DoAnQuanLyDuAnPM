using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookWebsite.Models
{
    public class NguoiDung : IdentityUser
    {

        [Required(ErrorMessage = "Tên người dùng không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên người dùng không được vượt quá 100 ký tự.")]
        [RegularExpression(@"^[a-zA-ZàáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵđÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴĐ\s]+$"
, ErrorMessage = "Tên người dùng chỉ được chứa các ký tự chữ cái.")]
        [DisplayName("Họ tên người dùng")]
        public string HoTen{ get; set; }  // Họ tên người dùng


        //Navigation properties
        public ICollection<DonHang> DonHangs { get; set; }

        public ICollection<GioHang> GioHangs { get; set; }

        public ICollection<DanhGia> DanhGias { get; set; }
    }
}
