using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace student
{
    // Chuyển record thành class truyền thống để tương thích C# phiên bản cũ
    public class Student
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Major { get; set; }
        public Student(string id, string name, int age, string email, string major)
        {
            Id = id;
            Name = name;
            Age = age;
            Email = email;
            Major = major;
        }
        // Ghi đè ToString() để in thông tin đẹp mắt
        public override string ToString()
        {
            return $"ID: {Id} | Tên: {Name} | Tuổi: {Age} | Email: {Email} | Ngành: {Major}";
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string filePath = "students.xml";
            EnsureXmlFile(filePath);

            // Đọc file XML
            var students = XDocument.Load(filePath)
                .Descendants("student")
                .Select(s => new Student(
                    (string)s.Attribute("id") ?? "",
                    (string)s.Element("name") ?? "",
                    (int?)s.Element("age") ?? 0,
                    (string)s.Element("email") ?? "",
                    (string)s.Element("major") ?? ""
                ))
                .ToList();
            // 1. In toàn bộ danh sách sinh viên
            Console.WriteLine("Toàn bộ danh sách sinh viên");
            students.ForEach(s => Console.WriteLine(s));

            // 2. In tên các sinh viên
            Console.WriteLine("\nIn ra màn hình tên các sinh viên ");
            students.ForEach(s => Console.WriteLine($"- {s.Name}"));
            // 3. Tìm sinh viên id="SV02"
            Console.WriteLine("\nSinh viên id=SV02 ");
            var sv02 = students.FirstOrDefault(s => s.Id.Equals("SV02", StringComparison.OrdinalIgnoreCase));
            Console.WriteLine(sv02 != null ? sv02.ToString() : "Không thấy SV02");
            // 4. Tính tổng số sinh viên
            Console.WriteLine($"\nTổng số sinh viên: {students.Count} ");
            // 5. Tìm sinh viên có age >= 20
            Console.WriteLine("\nSinh viên có tuổi lớn hơn hoặc bằng 20 ");
            students.Where(s => s.Age >= 20).ToList().ForEach(s => Console.WriteLine(s));
            Console.ReadLine(); // Giữ màn hình Console không bị đóng lập tức
        }
       
    }
}