using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QLThuVien_DOM
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            string filePath = "SachInventory.xml";
            TaoFileXmlMauNeuChuaCo(filePath);

            // 1. Nạp file XML bằng XmlDocument
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            Console.WriteLine("--- NỘI DUNG BAN ĐẦU ---");
            Innoidung(doc);

            // 2. Sử dụng XPath để tìm sách Công nghệ
            // XPath: //Sach[@TheLoai='CongNghe']
            XmlNodeList sachCongNghe = doc.SelectNodes("//Sach[@TheLoai='CongNghe']");
            if (sachCongNghe != null)
            {
                foreach (XmlNode sach in sachCongNghe)
                {
                    XmlNode soluong = sach.SelectSingleNode("SoLuongTon");
                    if (soluong != null && int.TryParse(soluong.InnerText, out int soLuongHienTai))
                    {
                        soluong.InnerText = (soLuongHienTai + 10).ToString();
                    }
                }
            }

            // 3. Xóa tất cả sách có số lượng = 0
            XmlNodeList sachhh = doc.SelectNodes("//Sach[SoLuongTon=0]");
            if (sachhh != null)
            {
                foreach (XmlNode sach in sachhh)
                {
                    sach.ParentNode?.RemoveChild(sach);
                }
            }

            // 4. Lưu lại kết quả
            doc.Save(filePath);

            Console.WriteLine("\n--- DỮ LIỆU SẠU KHỊ CẬP NHẬT VÀ XÓA ---");
            Innoidung(doc);

            Console.ReadLine(); // Dừng màn hình để xem kết quả
        }

        /// <summary>
        /// Hàm hỗ trợ in nội dung XML ra Console (Đặt ngoài hàm Main)
        /// </summary>
        static void Innoidung(XmlDocument doc)
        {
            using (StringWriter stringWriter = new StringWriter())
            using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented })
            {
                doc.WriteTo(xmlWriter);
                Console.WriteLine(stringWriter.ToString());
            }
        }

        /// <summary>
        /// Tạo file dữ liệu ban đầu phục vụ chạy thử (Đặt ngoài hàm Main)
        /// </summary>
        static void TaoFileXmlMauNeuChuaCo(string filePath)
        {
            if (!File.Exists(filePath))
            {
                string xmlContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<ThuVien>
    <Sach Ma=""S01"" TheLoai=""CongNghe"">
        <TieuDe>Lập trình C# nâng cao với .NET 8</TieuDe>
        <SoLuongTon>15</SoLuongTon>
    </Sach>
    <Sach Ma=""S02"" TheLoai=""VanHoc"">
        <TieuDe>Lão Hạc và cuộc đời</TieuDe>
        <SoLuongTon>0</SoLuongTon>
    </Sach>
    <Sach Ma=""S03"" TheLoai=""CongNghe"">
        <TieuDe>Thiết kế cơ sở dữ liệu XML</TieuDe>
        <SoLuongTon>5</SoLuongTon>
    </Sach>
</ThuVien>";

                File.WriteAllText(filePath, xmlContent, Encoding.UTF8);
            }
        }
    }
}