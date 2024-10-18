
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookWebsite.Models
{
    public class TheLoai
    {
        [DisplayName("Mã Thể Loại")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên thể loại không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên thể loại không được vượt quá 100 ký tự.")]
        [DisplayName("Thể loại")]
        public string TenTheLoai { get; set; }

        // Navigation property
        public ICollection<Book> Books { get; set; }
    }
}
