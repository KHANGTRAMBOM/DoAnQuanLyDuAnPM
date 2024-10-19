using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using BookWebsite.Data;
using BookWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BookWebsite.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }


                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

                ClearDatabaseAsync(context);

                SeedTheLoai(context);
                SeedBooks(context);
                SeedLoaiPhuongThucThanhToan(context);

                logger.LogInformation("Seeding Completed");
      
            }
        }

        public static void ClearDatabaseAsync(ApplicationDbContext context)
        {
            context.Book.RemoveRange(context.Book);
            context.TheLoai.RemoveRange(context.TheLoai);
            context.LoaiPhuongThucThanhToan.RemoveRange(context.LoaiPhuongThucThanhToan);

        }

        private static void SeedTheLoai(ApplicationDbContext context)
        {
            var theLoais = new[]
            {
                new TheLoai { TenTheLoai = "Fiction" },
                new TheLoai { TenTheLoai = "Science" },
                new TheLoai { TenTheLoai = "History" },
                new TheLoai { TenTheLoai = "Psychology" },
                new TheLoai { TenTheLoai = "Economics" },
                new TheLoai { TenTheLoai = "Philosophy" },
                new TheLoai { TenTheLoai = "Memoir" },
                new TheLoai { TenTheLoai = "Self-help" },
                new TheLoai { TenTheLoai = "Textbooks - Reference" },
                new TheLoai { TenTheLoai = "Cookbooks" },
                new TheLoai { TenTheLoai = "Medical - Health Books" },
                new TheLoai { TenTheLoai = "Children's Books" },
                new TheLoai { TenTheLoai = "Comics" },
                new TheLoai { TenTheLoai = "Other" },
            };
            context.TheLoai.AddRange(theLoais);
            context.SaveChanges();
        }


        private static void SeedBooks(ApplicationDbContext context)
        {
            if (context.Book.Any())
            {
                return;   // Dữ liệu đã tồn tại, không cần seed lại
            }

            // Lấy Id của các thể loại sau khi đã được seed
            var TamLyHoc = context.TheLoai.FirstOrDefault(t => t.TenTheLoai == "Psychology")?.Id ?? 0;
            var KhoaHoc = context.TheLoai.FirstOrDefault(t => t.TenTheLoai == "Science")?.Id ?? 0;
            var LichSu = context.TheLoai.FirstOrDefault(t => t.TenTheLoai == "History")?.Id ?? 0;
            var KinhTe = context.TheLoai.FirstOrDefault(t => t.TenTheLoai == "Economics")?.Id ?? 0;
            var KyNangSong = context.TheLoai.FirstOrDefault(t => t.TenTheLoai == "Self-help")?.Id ?? 0;
            var TruyenTranh = context.TheLoai.FirstOrDefault(t => t.TenTheLoai == "Comics")?.Id ?? 0;
            var NauAn = context.TheLoai.FirstOrDefault(t => t.TenTheLoai == "Cookbooks")?.Id ?? 0;
            var TreEm = context.TheLoai.FirstOrDefault(t => t.TenTheLoai == "Children's Books")?.Id ?? 0;
            var VanHoc = context.TheLoai.FirstOrDefault(t => t.TenTheLoai == "Fiction")?.Id ?? 0;
            var GiaoKhoa = context.TheLoai.FirstOrDefault(t => t.TenTheLoai == "Textbooks - Reference")?.Id ?? 0;
            var Khac = context.TheLoai.FirstOrDefault(t => t.TenTheLoai == "Other")?.Id ?? 0;



            var books = new[]
            {
                new Book
                {
                    TenSach = "Đắc Nhân Tâm",
                    TacGia = "Dale Carnegie",
                    Gia = 120000,
                    NXB = "NXB Tổng hợp TP.HCM",
                    NamXB = new DateTime(2023, 1, 1),
                    UrlAnh = "Image/DacNhanTam/1.jpg",
                    MoTa = "Cuốn sách nổi tiếng về nghệ thuật giao tiếp, phiên bản cập nhật 2023",
                    TheLoaiId = TamLyHoc  // Gán đúng Id sau khi seed
                },
                new Book
                {
                    TenSach = "Vũ trụ trong lòng bàn tay",
                    TacGia = "Neil deGrasse Tyson",
                    Gia = 180000,
                    NXB = "NXB Trẻ",
                    NamXB = new DateTime(2023, 3, 15),
                    UrlAnh = "Image/VuTruTrongLongBanTay/1.jpg",
                    MoTa = "Khám phá vũ trụ qua góc nhìn của nhà thiên văn học nổi tiếng",
                    TheLoaiId = KhoaHoc  // Gán đúng Id
                },
                new Book
                {
                    TenSach = "Sapiens: Lược sử loài người",
                    TacGia = "Yuval Noah Harari",
                    Gia = 220000,
                    NXB = "NXB Thế Giới",
                    NamXB = new DateTime(2023, 2, 1),
                    UrlAnh = "Image/LichSuLoaiNguoi/1.jpg",
                    MoTa = "Lịch sử về sự phát triển của loài người, phiên bản cập nhật",
                    TheLoaiId = LichSu  // Gán đúng Id
                },
                new Book
                {
                    TenSach = "Học Cách Lạc Quan",
                    TacGia = "Martin E. P. Seligman",
                    Gia = 150000,
                    NXB = "NXB Lao động",
                    NamXB = new DateTime(2023, 4, 1),
                    UrlAnh = "Image/HocCachLacQuan/1.jpg",
                    MoTa = "Khám phá sức mạnh của tư duy tích cực",
                    TheLoaiId = TamLyHoc  // Gán đúng Id
                },
                new Book
                {
                    TenSach = "Kinh tế học vi mô",
                    TacGia = "N. Gregory Mankiw",
                    Gia = 280000,
                    NXB = "NXB Kinh tế Quốc dân",
                    NamXB = new DateTime(2023, 5, 1),
                    UrlAnh = "Image/KinhTeHocViMo/1.jpg",
                    MoTa = "Giáo trình kinh tế học vi mô cập nhật",
                    TheLoaiId = KinhTe  // Gán đúng Id
                },

                new Book
                {
                    TenSach = "7 Thói Quen Hiệu Quả",
                    TacGia = "Stephen R. Covey",
                    Gia = 210000,
                    NXB = "NXB Trẻ",
                    NamXB = new DateTime(2023, 6, 10),
                    UrlAnh = "Image/7ThoiQuenHieuQua/1.jpg",
                    MoTa = "Cuốn sách hướng dẫn những thói quen để đạt được hiệu quả cá nhân và tổ chức",
                    TheLoaiId = KyNangSong
                },
                new Book
                {
                    TenSach = "Đánh Thức Con Người Phi Thường Trong Bạn",
                    TacGia = "Anthony Robbins",
                    Gia = 190000,
                    NXB = "NXB Văn hóa Sài Gòn",
                    NamXB = new DateTime(2023, 5, 1),
                    UrlAnh = "Image/DanhThucConNguoiPhiThuong/1.jpg",
                    MoTa = "Bí quyết kiểm soát cuộc sống, vượt qua giới hạn và tìm kiếm thành công",
                    TheLoaiId = KyNangSong
                },

                new Book
                {
                    TenSach = "One Piece - Tập 100",
                    TacGia = "Eiichiro Oda",
                    Gia = 85000,
                    NXB = "NXB Kim Đồng",
                    NamXB = new DateTime(2023, 4, 1),
                    UrlAnh = "Image/OnePiece100/1.jpg",
                    MoTa = "Tập mới nhất của bộ truyện tranh đình đám về cuộc hành trình của Luffy và những người bạn",
                    TheLoaiId = TruyenTranh
                },
                new Book
                {
                    TenSach = "Naruto - Tập 72",
                    TacGia = "Masashi Kishimoto",
                    Gia = 90000,
                    NXB = "NXB Kim Đồng",
                    NamXB = new DateTime(2023, 3, 15),
                    UrlAnh = "Image/Naruto72/1.jpg",
                    MoTa = "Tập cuối cùng của bộ truyện Naruto, hành trình ninja vĩ đại kết thúc",
                    TheLoaiId = TruyenTranh
                },

                new Book
                {
                    TenSach = "Vị Ngon Món Việt",
                    TacGia = "Nguyễn Anh Tuấn",
                    Gia = 130000,
                    NXB = "NXB Phụ Nữ",
                    NamXB = new DateTime(2023, 7, 10),
                    UrlAnh = "Image/MonViet/1.jpg",
                    MoTa = "Hướng dẫn nấu các món ăn truyền thống Việt Nam",
                    TheLoaiId = NauAn
                },
                new Book
                {
                    TenSach = "30 Công Thức Nấu Ăn Của Yanny - Món Ăn Nhật Đậm Vị Việt",
                    TacGia = "Yanny Đặng",
                    Gia = 150000,
                    NXB = "NXB Trẻ",
                    NamXB = new DateTime(2023, 8, 1),
                    UrlAnh = "Image/MonNhat/1.jpg",
                    MoTa = "Khám phá những món ăn nổi tiếng của ẩm thực Nhật Bản qua cách làm đơn giản",
                    TheLoaiId = NauAn
                },

                new Book
                {
                    TenSach = "Harry Potter và Hòn đá Phù thủy",
                    TacGia = "J.K. Rowling",
                    Gia = 120000,
                    NXB = "NXB Kim Đồng",
                    NamXB = new DateTime(2023, 5, 1),
                    UrlAnh = "Image/HarryPotter1/1.jpg",
                    MoTa = "Cuốn sách đầu tiên trong series Harry Potter, kể về hành trình của cậu bé phù thủy.",
                    TheLoaiId = TreEm
                },

                new Book
                {
                    TenSach = "Harry Potter – Diagon Alley",
                    TacGia = "J.K. Rowling",
                    Gia = 120000,
                    NXB = "NXB Kim Đồng",
                    NamXB = new DateTime(2023, 6, 15),
                    UrlAnh = "Image/HarryPotter2/1.jpg",
                    MoTa = "Hẻm Xéo là khu mua sắm lát đá cuội dành cho các pháp sư và phù thủy, và đây là nơi đầu tiên Harry Potter giới thiệu về thế giới phù thủy. Trên con phố nhộn nhịp này, xuất hiện xuyên suốt các bộ phim Harry Potter, những cây chổi mới nhất được rao bán, các tác giả phù thủy ký tặng sách và các học sinh nhỏ tuổi của Hogwarts mua đồ dùng học tập - vạc, bút lông, áo choàng, đũa phép và chổi.",
                    TheLoaiId = TreEm
                },

                new Book
                {
                    TenSach = "Trăm Năm Cô Đơn",
                    TacGia = "Gabriel Garcia Marquez",
                    Gia = 200000,
                    NXB = "NXB Hội Nhà Văn",
                    NamXB = new DateTime(2023, 7, 20),
                    UrlAnh = "Image/TramNamCoDon/1.jpg",
                    MoTa = "Một tác phẩm kinh điển về một gia đình trong làng Macondo.",
                    TheLoaiId = VanHoc
                },

                new Book
                {
                    TenSach = "Nơi Khu Rừng Chạm Tới Những Vì Sao - Where the Forest Meets the Stars",
                    TacGia = "Glendy Vanderah",
                    Gia = 230000,
                    NXB = "NXB Văn học",
                    NamXB = new DateTime(2023, 8, 15),
                    UrlAnh = "Image/NoiKhuRung/1.jpg",
                    MoTa = "Một hành trình nội tâm đầy cảm xúc của con ngườiĐứa trẻ bí ẩn xuất hiện với đôi chân trần, trên người đầy vết bầm tím.\r\n\r\nCô bé tự xưng là Ursa, được cử đến từ những vì tinh tú và sẽ chỉ rời đi sau khi chứng kiến đủ năm điều kỳ diệu nơi Địa Cầu. Lo lắng cho đứa trẻ, Jo – một nhà sinh vật học thực địa sống giữa vùng nông thôn Illinois- đành để cô bé ở lại, chỉ tới khi cô biết được sự thật về quá khứ của đứa bé đó..",
                    TheLoaiId = VanHoc
                },

                new Book
                {
                    TenSach = "Nhà Giả Kim",
                    TacGia = "Paulo Coelho",
                    Gia = 150000,
                    NXB = "NXB Hội Nhà Văn",
                    NamXB = new DateTime(2023, 8, 1),
                    UrlAnh = "Image/NhaGiaKim/1.jpg",
                    MoTa = "Cuốn sách kể về hành trình tìm kiếm giấc mơ và những bài học quý giá trong cuộc sống.",
                    TheLoaiId = VanHoc
                },

                new Book
                {
                    TenSach = "Bộ Giáo Khoa Toán Lớp 10",
                    TacGia = "Bộ Giáo Dục và Đào Tạo",
                    Gia = 70000,
                    NXB = "NXB Giáo dục",
                    NamXB = new DateTime(2023, 1, 5),
                    UrlAnh = "Image/GiaoKhoa10/1.jpg",
                    MoTa = "Sách giáo khoa chương trình lớp 10 mới nhất",
                    TheLoaiId = GiaoKhoa
                },
                new Book
                {
                    TenSach = "Bộ Giáo Khoa Ngữ Văn Lớp 11",
                    TacGia = "Bộ Giáo Dục và Đào Tạo",
                    Gia = 75000,
                    NXB = "NXB Giáo dục",
                    NamXB = new DateTime(2023, 2, 10),
                    UrlAnh = "Image/GiaoKhoa11/1.jpg",
                    MoTa = "Sách giáo khoa chương trình lớp 11 mới nhất",
                    TheLoaiId = GiaoKhoa
                },

                new Book
                {
                    TenSach = "Bộ Giáo Khoa Ngữ Văn Lớp 12",
                    TacGia = "Bộ Giáo Dục và Đào Tạo",
                    Gia = 75000,
                    NXB = "NXB Giáo dục",
                    NamXB = new DateTime(2023, 2, 10),
                    UrlAnh = "Image/GiaoKhoa12/1.jpg",
                    MoTa = "Sách giáo khoa chương trình lớp 12 mới nhất",
                    TheLoaiId = GiaoKhoa
                },

                new Book
                {
                        TenSach = "Lịch sử văn minh thế giới",
                        TacGia = "Will và Ariel Durant",
                        Gia = 300000,
                        NXB = "NXB Thế Giới",
                        NamXB = new DateTime(2023, 6, 1),
                        UrlAnh = "Image/LichSuVanMinh/1.jpg",
                        MoTa = "Một cái nhìn tổng quan về các nền văn minh lớn trong lịch sử nhân loại.",
                        TheLoaiId = LichSu
                },

                new Book
                {
                    TenSach = "Destination B1",
                    TacGia = "David McKeegan và Gillian L. Brown",
                    Gia = 250000,
                    NXB = "Oxford University Press",
                    NamXB = new DateTime(2021, 1, 1),
                    UrlAnh = "Image/DestinationB1/1.jpg",
                    MoTa = "Cuốn sách giúp người học tiếng Anh đạt trình độ B1 với nhiều bài tập thực hành.",
                    TheLoaiId = Khac
                },
                new Book
                {
                    TenSach = "Destination B2",
                    TacGia = "David McKeegan và Gillian L. Brown",
                    Gia = 280000,
                    NXB = "Oxford University Press",
                    NamXB = new DateTime(2021, 6, 1),
                    UrlAnh = "Image/DestinationB2/1.jpg",
                    MoTa = "Cuốn sách nâng cao kỹ năng tiếng Anh lên trình độ B2, phù hợp cho học sinh và sinh viên.",
                    TheLoaiId = Khac
                }


            };
            context.Book.AddRange(books);
            context.SaveChanges();
        }


        private static void SeedLoaiPhuongThucThanhToan(ApplicationDbContext context)
        {
            var loaiPhuongThucThanhToans = new[]
            {
                new LoaiPhuongThucThanhToan { TenPhuongThucThanhToan = "Tiền mặt", KhuyenMai = 0 },
                new LoaiPhuongThucThanhToan { TenPhuongThucThanhToan = "Chuyển khoản", KhuyenMai = 0.05M },
                new LoaiPhuongThucThanhToan { TenPhuongThucThanhToan = "Ví điện tử", KhuyenMai = 0.1M },
                new LoaiPhuongThucThanhToan { TenPhuongThucThanhToan = "Thẻ tín dụng", KhuyenMai = 0.03M }
            };
            context.LoaiPhuongThucThanhToan.AddRange(loaiPhuongThucThanhToans);
            context.SaveChanges();
        }

    }
    
}
  
