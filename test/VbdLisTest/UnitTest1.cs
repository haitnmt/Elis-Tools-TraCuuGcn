using Haihv.Elis.Tool.VbdLis;
using System.Xml.Serialization;
using System.Text;
using Haihv.Elis.Tool.VbdLis.Models;

namespace VbdLisTest;

/// <summary>
/// Test chung cho việc Serialize/Deserialize XML của các model VbdLis
/// </summary>
public class VbdLisModelTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GiayChungNhan_ShouldSerializeAndDeserialize_Successfully()
    {
        // Arrange
        var giayChungNhan = new ThongTinGiayChungNhan
        {
            GiayChungNhan = new DcGiayChungNhan
            {
                GiayChungNhanId = "GCN001",
                LoaiGiayChungNhan = "QSDĐ",
                SoVaoSo = "123/2024",
                NgayCap = DateTime.Now
            }
        };

        // Act & Assert
        var result = SerializeAndDeserialize(giayChungNhan);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.GiayChungNhan?.GiayChungNhanId, Is.EqualTo("GCN001"));
        Assert.That(result.GiayChungNhan?.LoaiGiayChungNhan, Is.EqualTo("QSDĐ"));
        Assert.That(result.GiayChungNhan?.SoVaoSo, Is.EqualTo("123/2024"));
    }

    [Test]
    public void ChuSoHuu_CaNhan_ShouldSerializeAndDeserialize_Successfully()
    {
        // Arrange
        var chuSoHuu = new ThongTinChu
        {
            ThongTinCaNhan = new ThongTinCaNhan
            {
                CaNhan = new DcCaNhan
                {
                    Ten = "Nguyễn Văn A",
                    Cccd = "123456789012",
                    GioiTinh = "Nam",
                    NgaySinh = new DateTime(1990, 1, 1)
                }
            }
        };

        // Act & Assert
        var result = SerializeAndDeserialize(chuSoHuu);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ThongTinCaNhan?.CaNhan?.HoTen, Is.EqualTo("Nguyễn Văn A"));
        Assert.That(result.ThongTinCaNhan?.CaNhan?.ThongTinGiayToTuyThan Is.EqualTo("123456789012"));
        Assert.That(result.ThongTinCaNhan?.CaNhan?.GioiTinh, Is.EqualTo("Nam"));
    }

    [Test]
    public void TaiSan_ThuaDat_ShouldSerializeAndDeserialize_Successfully()
    {
        // Arrange
        var taiSan = new ThongTinTaiSan
        {
            ThongTinThuaDat = new ThongTinThuaDat
            {
                ThuaDat = new DcThuaDat
                {
                    SoThuTuThua = "123",
                    SoToBanDo = "45",
                    DienTich = 100.5m,
                    DonViDienTich = "m²"
                }
            }
        };

        // Act & Assert
        var result = SerializeAndDeserialize(taiSan);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ThongTinThuaDat?.ThuaDat?.SoThuTuThua, Is.EqualTo("123"));
        Assert.That(result.ThongTinThuaDat?.ThuaDat?.SoHieuToBanDo, Is.EqualTo("45"));
        Assert.That(result.ThongTinThuaDat?.ThuaDat?.DienTich, Is.EqualTo(100.5m));
    }

    /// <summary>
    /// Helper method để test XML serialization/deserialization
    /// </summary>
    private static T SerializeAndDeserialize<T>(T obj) where T : class
    {
        var serializer = new XmlSerializer(typeof(T));

        // Serialize
        using var memoryStream = new MemoryStream();
        serializer.Serialize(memoryStream, obj);

        // Deserialize
        memoryStream.Position = 0;
        var result = serializer.Deserialize(memoryStream) as T;

        return result ?? throw new InvalidOperationException("Deserialization returned null");
    }
}