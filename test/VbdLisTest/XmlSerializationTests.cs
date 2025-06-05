using Haihv.Elis.Tool.VbdLis;
using System.Xml.Serialization;
using System.Text;
using Haihv.Elis.Tool.VbdLis.Models;

namespace VbdLisTest;

/// <summary>
/// Integration tests cho XML Serialization/Deserialization của VbdLis models
/// </summary>
[TestFixture]
public class XmlSerializationTests
{
    [Test]
    public void GiayChungNhan_XmlSerialization_ShouldPreserveData()
    {
        // Arrange
        var originalData = new ThongTinGiayChungNhan
        {
            GiayChungNhan = new DcGiayChungNhan
            {
                GiayChungNhanId = "GCN123456",
                LoaiGiayChungNhan = "QSDĐ",
                SoVaoSo = "001/2024/QSDĐ",
                NgayCapGcn = new DateTime(2024, 6, 15),
                CoQuanCap = "UBND TP. Hồ Chí Minh"
            }
        };

        // Act
        var xml = SerializeToXml(originalData);
        var deserializedData = DeserializeFromXml<ThongTinGiayChungNhan>(xml);

        // Assert
        Assert.That(deserializedData, Is.Not.Null);
        Assert.That(deserializedData.GiayChungNhan?.GiayChungNhanId, Is.EqualTo(originalData.GiayChungNhan.GiayChungNhanId));
        Assert.That(deserializedData.GiayChungNhan?.LoaiGiayChungNhan, Is.EqualTo(originalData.GiayChungNhan.LoaiGiayChungNhan));
        Assert.That(deserializedData.GiayChungNhan?.SoVaoSo, Is.EqualTo(originalData.GiayChungNhan.SoVaoSo));
        Assert.That(deserializedData.GiayChungNhan?.CoQuanCap, Is.EqualTo(originalData.GiayChungNhan.CoQuanCap));
    }

    [Test]
    public void ChuSoHuu_CaNhan_XmlSerialization_ShouldPreserveData()
    {
        // Arrange
        var originalData = new ThongTinChu
        {
            ThongTinCaNhan = new ThongTinCaNhan
            {
                CaNhan = new DcCaNhan
                {
                    Ten = "Nguyễn Văn Test",
                    Cccd = "123456789012",
                    GioiTinh = "Nam",
                    NgaySinh = new DateTime(1985, 3, 20),
                    SoDienThoai = "0901234567",
                    Email = "test@example.com",
                    QuocTich = "Việt Nam"
                }
            }
        };

        // Act
        var xml = SerializeToXml(originalData);
        var deserializedData = DeserializeFromXml<ThongTinChu>(xml);

        // Assert
        Assert.That(deserializedData, Is.Not.Null);
        var caNhan = deserializedData.ThongTinCaNhan?.CaNhan;
        Assert.That(caNhan, Is.Not.Null);
        Assert.That(caNhan.Ten, Is.EqualTo("Nguyễn Văn Test"));
        Assert.That(caNhan.Cccd, Is.EqualTo("123456789012"));
        Assert.That(caNhan.GioiTinh, Is.EqualTo("Nam"));
        Assert.That(caNhan.SoDienThoai, Is.EqualTo("0901234567"));
        Assert.That(caNhan.Email, Is.EqualTo("test@example.com"));
        Assert.That(caNhan.QuocTich, Is.EqualTo("Việt Nam"));
    }

    [Test]
    public void TaiSan_ThuaDat_XmlSerialization_ShouldPreserveData()
    {
        // Arrange
        var originalData = new ThongTinTaiSan
        {
            ThongTinThuaDat = new ThongTinThuaDat
            {
                ThuaDat = new DcThuaDat
                {
                    SoThuTuThua = "123",
                    SoToBanDo = "45",
                    DienTich = 250.75m,
                    DonViDienTich = "m²",
                    MucDichSuDung = "Đất ở tại đô thị",
                    ThoiHanSuDung = "Lâu dài"
                }
            }
        };

        // Act
        var xml = SerializeToXml(originalData);
        var deserializedData = DeserializeFromXml<ThongTinTaiSan>(xml);

        // Assert
        Assert.That(deserializedData, Is.Not.Null);
        var thuaDat = deserializedData.ThongTinThuaDat?.ThuaDat;
        Assert.That(thuaDat, Is.Not.Null);
        Assert.That(thuaDat.SoThuTuThua, Is.EqualTo("123"));
        Assert.That(thuaDat.SoToBanDo, Is.EqualTo("45"));
        Assert.That(thuaDat.DienTich, Is.EqualTo(250.75m));
        Assert.That(thuaDat.DonViDienTich, Is.EqualTo("m²"));
        Assert.That(thuaDat.MucDichSuDung, Is.EqualTo("Đất ở tại đô thị"));
        Assert.That(thuaDat.ThoiHanSuDung, Is.EqualTo("Lâu dài"));
    }

    [Test]
    public void DiaChi_XmlSerialization_ShouldPreserveData()
    {
        // Arrange
        var originalData = new ThongTinDiaChi
        {
            DiaChi = new DcDiaChi
            {
                SoNha = "123/45",
                TenDuong = "Đường Nguyễn Huệ",
                Phuong = "Phường Bến Nghé",
                Quan = "Quận 1",
                TinhTp = "TP. Hồ Chí Minh"
            }
        };

        // Act
        var xml = SerializeToXml(originalData);
        var deserializedData = DeserializeFromXml<ThongTinDiaChi>(xml);

        // Assert
        Assert.That(deserializedData, Is.Not.Null);
        var diaChi = deserializedData.DiaChi;
        Assert.That(diaChi, Is.Not.Null);
        Assert.That(diaChi.SoNha, Is.EqualTo("123/45"));
        Assert.That(diaChi.TenDuong, Is.EqualTo("Đường Nguyễn Huệ"));
        Assert.That(diaChi.Phuong, Is.EqualTo("Phường Bến Nghé"));
        Assert.That(diaChi.Quan, Is.EqualTo("Quận 1"));
        Assert.That(diaChi.TinhTp, Is.EqualTo("TP. Hồ Chí Minh"));
    }

    [Test]
    public void ComplexModel_WithNullValues_ShouldSerializeCorrectly()
    {
        // Arrange
        var originalData = new ThongTinGiayChungNhan
        {
            GiayChungNhan = new DcGiayChungNhan
            {
                GiayChungNhanId = "GCN001",
                // Một số thuộc tính để null
                LoaiGiayChungNhan = null,
                SoVaoSo = "123/2024",
                NgayCapGcn = null
            }
        };

        // Act & Assert - Không được throw exception
        Assert.DoesNotThrow(() =>
        {
            var xml = SerializeToXml(originalData);
            var deserializedData = DeserializeFromXml<ThongTinGiayChungNhan>(xml);

            Assert.That(deserializedData?.GiayChungNhan?.GiayChungNhanId, Is.EqualTo("GCN001"));
            Assert.That(deserializedData?.GiayChungNhan?.SoVaoSo, Is.EqualTo("123/2024"));
        });
    }

    [Test]
    public void XmlSerialization_WithUnicodeCharacters_ShouldPreserveEncoding()
    {
        // Arrange
        var originalData = new ThongTinChu
        {
            ThongTinCaNhan = new ThongTinCaNhan
            {
                CaNhan = new DcCaNhan
                {
                    Ten = "Nguyễn Thị Ánh Xuân",
                    QuocTich = "Việt Nam"
                }
            }
        };

        // Act
        var xml = SerializeToXml(originalData);
        var deserializedData = DeserializeFromXml<ThongTinChu>(xml);

        // Assert
        Assert.That(deserializedData?.ThongTinCaNhan?.CaNhan?.Ten, Is.EqualTo("Nguyễn Thị Ánh Xuân"));
        Assert.That(deserializedData?.ThongTinCaNhan?.CaNhan?.QuocTich, Is.EqualTo("Việt Nam"));
    }

    #region Helper Methods

    private static string SerializeToXml<T>(T obj)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, obj);
        return stringWriter.ToString();
    }

    private static T DeserializeFromXml<T>(string xml)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var stringReader = new StringReader(xml);
        var result = serializer.Deserialize(stringReader);
        return (T)result!;
    }

    #endregion
}
