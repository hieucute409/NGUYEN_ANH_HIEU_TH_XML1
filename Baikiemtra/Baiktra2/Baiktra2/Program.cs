using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace XmlLinqStudentManager
{
    internal class Program
    {
        private const string FilePath = @"D:\Hoc_Tap\Chuyen_nganh\HK_126\Thuc_hanh\TH Công nghệ XML\Student.xml";

        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Xử lý kiểm tra file XML tồn tại và hợp lệ
            if (!File.Exists(FilePath))
            {
                Console.WriteLine($"[LỖI] File {FilePath} không tồn tại! Vui lòng kiểm tra lại thư mục.");
                return;
            }

            XDocument doc;
            try
            {
                doc = XDocument.Load(FilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LỖI] Không thể đọc file XML: {ex.Message}");
                return;
            }

            // Chạy chương trình với Menu
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("=================== QUẢN LÝ SINH VIÊN (LINQ TO XML) ===================");
                Console.WriteLine("1.  [Câu 1] Hiển thị tất cả sinh viên");
                Console.WriteLine("2.  [Câu 2] Sinh viên có GPA >= 8.0");
                Console.WriteLine("3.  [Câu 3] Sinh viên thuộc lớp CTK44");
                Console.WriteLine("4.  [Câu 4] Danh sách sinh viên nữ");
                Console.WriteLine("5.  [Câu 5] Sinh viên có GPA cao nhất");
                Console.WriteLine("6.  [Câu 6] Sinh viên có GPA thấp nhất");
                Console.WriteLine("7.  [Câu 7] Đếm tổng số sinh viên");
                Console.WriteLine("8.  [Câu 8] Tính GPA trung bình");
                Console.WriteLine("9.  [Câu 9] Thống kê số lượng sinh viên theo lớp");
                Console.WriteLine("10. [Câu 10] Danh sách sinh viên theo GPA giảm dần");
                Console.WriteLine("11. [Câu 11] Sinh viên từ 20 đến 21 tuổi");
                Console.WriteLine("12. [Câu 12] Sinh viên CNTT có GPA >= 8.0");
                Console.WriteLine("13. [Câu 13] Tìm kiếm sinh viên theo Mã SV");
                Console.WriteLine("14. [Câu 14] Tra cứu sinh viên theo Lớp (GPA giảm dần)");
                Console.WriteLine("15. [Câu 15] Top 3 sinh viên GPA cao nhất");
                Console.WriteLine("0.  Thoát chương trình");
                Console.WriteLine("=======================================================================");
                Console.Write("Chọn chức năng (0-15): ");

                string choice = Console.ReadLine();
                Console.WriteLine("\n-----------------------------------------------------------------------");

                switch (choice)
                {
                    case "1": Cau01_HienThiTatCa(doc); break;
                    case "2": Cau02_GPATren8(doc); break;
                    case "3": Cau03_LopCTK44(doc); break;
                    case "4": Cau04_SinhVienNu(doc); break;
                    case "5": Cau05_GPACaoNhat(doc); break;
                    case "6": Cau06_GPAThapNhat(doc); break;
                    case "7": Cau07_DemTongSinhVien(doc); break;
                    case "8": Cau08_GPATrungBinh(doc); break;
                    case "9": Cau09_ThongKeTheoLop(doc); break;
                    case "10": Cau10_SapXepGPAGiamDan(doc); break;
                    case "11": Cau11_Tuoi20Den21(doc); break;
                    case "12": Cau12_CNTTAndGPATren8(doc); break;
                    case "13": Cau13_TimTheoMaSV(doc); break;
                    case "14": Cau14_TimTheoLopSapXep(doc); break;
                    case "15": Cau15_Top3GPACaoNhat(doc); break;
                    case "0":
                        exit = true;
                        Console.WriteLine("Đã thoát chương trình. Tạm biệt!");
                        break;
                    default:
                        Console.WriteLine("Lựa chọn không hợp lệ. Bấm phím bất kỳ để thử lại...");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nBấm phím bất kỳ để quay lại menu...");
                    Console.ReadKey();
                }
            }
        }

        #region Các phương thức trợ giúp & In ấn

        private static IEnumerable<XElement> GetStudents(XDocument doc)
        {
            return doc.Descendants("Student");
        }

        private static double ParseDouble(string val)
        {
            return double.Parse(val, CultureInfo.InvariantCulture);
        }

        private static void PrintHeader()
        {
            Console.WriteLine("{0,-8} | {1,-20} | {2,-8} | {3,-5} | {4,-8} | {5,-22} | {6,-5}",
                "Mã SV", "Họ Tên", "Giới Tính", "Tuổi", "Lớp", "Chuyên Ngành", "GPA");
            Console.WriteLine(new string('-', 85));
        }

        private static void PrintStudent(XElement s)
        {
            Console.WriteLine("{0,-8} | {1,-20} | {2,-8} | {3,-5} | {4,-8} | {5,-22} | {6,-5}",
                (string)s.Attribute("id"),
                (string)s.Element("Name"),
                (string)s.Element("Gender"),
                (string)s.Element("Age"),
                (string)s.Element("Class"),
                (string)s.Element("Major"),
                (string)s.Element("GPA"));
        }

        private static void PrintStudentList(IEnumerable<XElement> list)
        {
            if (!list.Any())
            {
                Console.WriteLine("Không có dữ liệu phù hợp.");
                return;
            }
            PrintHeader();
            foreach (var s in list)
            {
                PrintStudent(s);
            }
        }

        #endregion

        #region Implement 15 Câu LINQ to XML

        // Câu 1: Hiển thị danh sách tất cả sinh viên
        private static void Cau01_HienThiTatCa(XDocument doc)
        {
            Console.WriteLine("=== CÂU 1: DANH SÁCH TẤT CẢ SINH VIÊN ===");
            var query = from s in GetStudents(doc) select s;
            PrintStudentList(query);
        }

        // Câu 2: Sinh viên có GPA >= 8.0
        private static void Cau02_GPATren8(XDocument doc)
        {
            Console.WriteLine("=== CÂU 2: DANH SÁCH SINH VIÊN CÓ GPA >= 8.0 ===");
            var query = from s in GetStudents(doc)
                        where ParseDouble((string)s.Element("GPA")) >= 8.0
                        select s;
            PrintStudentList(query);
        }

        // Câu 3: Sinh viên thuộc lớp CTK44
        private static void Cau03_LopCTK44(XDocument doc)
        {
            Console.WriteLine("=== CÂU 3: DANH SÁCH SINH VIÊN LỚP CTK44 ===");
            var query = from s in GetStudents(doc)
                        where (string)s.Element("Class") == "CTK44"
                        select s;
            PrintStudentList(query);
        }

        // Câu 4: Danh sách sinh viên nữ
        private static void Cau04_SinhVienNu(XDocument doc)
        {
            Console.WriteLine("=== CÂU 4: DANH SÁCH SINH VIÊN NỮ ===");
            var query = GetStudents(doc)
                        .Where(s => (string)s.Element("Gender") == "Nu");
            PrintStudentList(query);
        }

        // Câu 5: Tìm sinh viên có GPA cao nhất
        private static void Cau05_GPACaoNhat(XDocument doc)
        {
            Console.WriteLine("=== CÂU 5: SINH VIÊN CÓ GPA CAO NHẤT ===");
            double maxGpa = GetStudents(doc).Max(s => ParseDouble((string)s.Element("GPA")));
            var query = GetStudents(doc).Where(s => ParseDouble((string)s.Element("GPA")) == maxGpa);
            PrintStudentList(query);
        }

        // Câu 6: Tìm sinh viên có GPA thấp nhất
        private static void Cau06_GPAThapNhat(XDocument doc)
        {
            Console.WriteLine("=== CÂU 6: SINH VIÊN CÓ GPA THẤP NHẤT ===");
            double minGpa = GetStudents(doc).Min(s => ParseDouble((string)s.Element("GPA")));
            var query = GetStudents(doc).Where(s => ParseDouble((string)s.Element("GPA")) == minGpa);
            PrintStudentList(query);
        }

        // Câu 7: Đếm số lượng sinh viên
        private static void Cau07_DemTongSinhVien(XDocument doc)
        {
            Console.WriteLine("=== CÂU 7: ĐẾM SỐ LƯỢNG SINH VIÊN ===");
            int count = GetStudents(doc).Count();
            Console.WriteLine($"Tổng số sinh viên trong danh sách: {count} sinh viên.");
        }

        // Câu 8: Tính GPA trung bình của tất cả sinh viên
        private static void Cau08_GPATrungBinh(XDocument doc)
        {
            Console.WriteLine("=== CÂU 8: TÍNH GPA TRUNG BÌNH ===");
            double avgGpa = GetStudents(doc).Average(s => ParseDouble((string)s.Element("GPA")));
            Console.WriteLine($"GPA trung bình của tất cả sinh viên: {avgGpa:F2}");
        }

        // Câu 9: Đếm số lượng sinh viên theo từng lớp
        private static void Cau09_ThongKeTheoLop(XDocument doc)
        {
            Console.WriteLine("=== CÂU 9: THỐNG KÊ SỐ LƯỢNG SINH VIÊN THEO LỚP ===");
            var query = from s in GetStudents(doc)
                        group s by (string)s.Element("Class") into g
                        select new
                        {
                            ClassName = g.Key,
                            Count = g.Count()
                        };

            string resultStr = string.Join(" ", query.Select(x => $"{x.ClassName}: {x.Count} sinh viên"));
            Console.WriteLine(resultStr);
        }

        // Câu 10: Sắp xếp sinh viên theo GPA giảm dần
        private static void Cau10_SapXepGPAGiamDan(XDocument doc)
        {
            Console.WriteLine("=== CÂU 10: DANH SÁCH SINH VIÊN THEO GPA GIẢM DẦN ===");
            var query = GetStudents(doc)
                        .OrderByDescending(s => ParseDouble((string)s.Element("GPA")));
            PrintStudentList(query);
        }

        // Câu 11: Tìm các sinh viên có tuổi từ 20 đến 21 tuổi
        private static void Cau11_Tuoi20Den21(XDocument doc)
        {
            Console.WriteLine("=== CÂU 11: DANH SÁCH SINH VIÊN TỪ 20 ĐẾN 21 TUỔI ===");
            var query = from s in GetStudents(doc)
                        let age = int.Parse((string)s.Element("Age"))
                        where age >= 20 && age <= 21
                        select s;
            PrintStudentList(query);
        }

        // Câu 12: Sinh viên CNTT và có GPA >= 8.0
        private static void Cau12_CNTTAndGPATren8(XDocument doc)
        {
            Console.WriteLine("=== CÂU 12: SINH VIÊN CÔNG NGHỆ THÔNG TIN CÓ GPA >= 8.0 ===");
            var query = from s in GetStudents(doc)
                        where (string)s.Element("Major") == "Cong nghe thong tin"
                              && ParseDouble((string)s.Element("GPA")) >= 8.0
                        select s;
            PrintStudentList(query);
        }

        // Câu 13: Tìm sinh viên theo Mã SV nhập từ bàn phím
        private static void Cau13_TimTheoMaSV(XDocument doc)
        {
            Console.WriteLine("=== CÂU 13: TÌM SINH VIÊN THEO MÃ SV ===");
            Console.Write("Nhập mã sinh viên cần tìm (ví dụ SV003): ");
            string idInput = Console.ReadLine()?.Trim();

            var student = GetStudents(doc)
                          .FirstOrDefault(s => string.Equals((string)s.Attribute("id"), idInput, StringComparison.OrdinalIgnoreCase));

            if (student == null)
            {
                Console.WriteLine("Không tìm thấy sinh viên.");
            }
            else
            {
                PrintHeader();
                PrintStudent(student);
            }
        }

        // Câu 14: Nhập tên lớp, hiển thị danh sách xếp theo GPA giảm dần
        private static void Cau14_TimTheoLopSapXep(XDocument doc)
        {
            Console.WriteLine("=== CÂU 14: TÌM SINH VIÊN THEO LỚP & SẮP XẾP GPA GIẢM DẦN ===");
            Console.Write("Nhập tên lớp cần tra cứu (ví dụ CTK44): ");
            string classInput = Console.ReadLine()?.Trim();

            var query = GetStudents(doc)
                        .Where(s => string.Equals((string)s.Element("Class"), classInput, StringComparison.OrdinalIgnoreCase))
                        .OrderByDescending(s => ParseDouble((string)s.Element("GPA")));

            if (!query.Any())
            {
                Console.WriteLine($"Không tìm thấy sinh viên nào thuộc lớp '{classInput}'.");
            }
            else
            {
                PrintStudentList(query);
            }
        }

        // Câu 15: Tìm 3 sinh viên có GPA cao nhất
        private static void Cau15_Top3GPACaoNhat(XDocument doc)
        {
            Console.WriteLine("=== CÂU 15: TOP 3 SINH VIÊN CÓ GPA CAO NHẤT ===");
            var query = GetStudents(doc)
                        .OrderByDescending(s => ParseDouble((string)s.Element("GPA")))
                        .Take(3);
            PrintStudentList(query);
        }

        #endregion
    }
}