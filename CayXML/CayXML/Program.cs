using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // Đường dẫn xuất D
        string filePath = @"D:\bookstore.xml";

        // Câu 1: Tạo cây XML theo hình.
        XElement goc = new XElement("bookstore",
            new XElement("book",
                new XAttribute("category", "cooking"),
                new XElement("title",
                    new XAttribute("lang", "en"),
                    "Everyday Italian"),
                new XElement("author", "Giada De Laurentiis"),
                new XElement("year", 2005),
                new XElement("price", "30.00")
            )
        );

        XDocument taiLieu = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            goc
        );

        // Export cây ban đầu ra XML.
        taiLieu.Save(filePath);

        // Câu 2: Thêm các phần tử con.
        goc.Add(
            new XElement("book",
                new XAttribute("category", "children"),
                new XElement("title",
                    new XAttribute("lang", "en"),
                    "Harry Potter"),
                new XElement("author", "J K. Rowling"),
                new XElement("year", 2005),
                new XElement("price", "29.99")
            ),
            new XElement("book",
                new XAttribute("category", "web"),
                new XElement("title",
                    new XAttribute("lang", "en"),
                    "Learning XML"),
                new XElement("author", "Erik T. Ray"),
                new XElement("year", 2003),
                new XElement("price", "39.95")
            )
        );

        // Export lại XML, bao gồm các sách vừa thêm.
        taiLieu.Save(filePath);

        Console.WriteLine("Đã export XML tại: " + filePath);
        Console.WriteLine("\n--- NỘI DUNG XML ---");
        Console.WriteLine(taiLieu);

        // Câu 3a: Hiển thị phần tử gốc.
        Console.WriteLine("\n--- THÔNG TIN ---");
        Console.WriteLine("Phần tử gốc: " + goc.Name);

        // Câu 3b: Đếm phần tử con, cháu.
        Console.WriteLine(
            "Số phần tử con trực tiếp: "
            + goc.Elements().Count());

        Console.WriteLine(
            "Số phần tử cháu: "
            + goc.Elements().Elements().Count());

        Console.WriteLine(
            "Tổng số phần tử con, cháu...: "
            + goc.Descendants().Count());

        // Câu 3c: Sắp xếp sách theo giá tăng dần.
        var danhSachSach = goc.Elements("book")
            .OrderBy(sach => decimal.Parse(
                sach.Element("price").Value,
                CultureInfo.InvariantCulture))
            .ToList();

        if (danhSachSach.Count > 0)
        {
            Console.WriteLine("\n--- SÁCH GIÁ THẤP NHẤT ---");
            HienThiSach(danhSachSach.First());

            Console.WriteLine("\n--- SÁCH GIÁ CAO NHẤT ---");
            HienThiSach(danhSachSach.Last());
        }

        Console.WriteLine("\nNhấn Enter để thoát.");
        Console.ReadLine();
    }

    static void HienThiSach(XElement sach)
    {
        Console.WriteLine("Tên sách: " + sach.Element("title").Value);
        Console.WriteLine("Tác giả: " + sach.Element("author").Value);
        Console.WriteLine("Năm: " + sach.Element("year").Value);
        Console.WriteLine("Giá: " + sach.Element("price").Value);
    }
}