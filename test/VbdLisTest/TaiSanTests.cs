using Haihv.Elis.Tool.VbdLis;
using Haihv.Elis.Tool.VbdLis.Models;

namespace VbdLisTest;

/// <summary>
/// Unit tests cho các model liên quan đến Tài sản
/// </summary>
[TestFixture]
public class TaiSanTests
{
    [Test]
    public void DcThuaDat_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var thuaDat = new DcThuaDat();

        // Assert
        Assert.That(thuaDat.SoThuTuThua, Is.Null);
        Assert.That(thuaDat.SoToBanDo, Is.Null);
        Assert.That(thuaDat.DienTich, Is.Null);
        Assert.That(thuaDat.DonViDienTich, Is.Null);
    }

    [Test]
    public void DcThuaDat_SetProperties_ShouldRetainValues()
    {
        // Arrange
        var thuaDat = new DcThuaDat();

        // Act
        thuaDat.SoThuTuThua = "123";
        thuaDat.SoToBanDo = "45";
        thuaDat.DienTich = 150.75m;
        thuaDat.DonViDienTich = "m²";
        thuaDat.MucDichSuDung = "Đất ở";

        // Assert
        Assert.That(thuaDat.SoThuTuThua, Is.EqualTo("123"));
        Assert.That(thuaDat.SoToBanDo, Is.EqualTo("45"));
        Assert.That(thuaDat.DienTich, Is.EqualTo(150.75m));
        Assert.That(thuaDat.DonViDienTich, Is.EqualTo("m²"));
        Assert.That(thuaDat.MucDichSuDung, Is.EqualTo("Đất ở"));
    }

    [Test]
    public void ThongTinThuaDat_WithValidData_ShouldWrapCorrectly()
    {
        // Arrange
        var dcThuaDat = new DcThuaDat
        {
            SoThuTuThua = "456",
            SoToBanDo = "78",
            DienTich = 200.5m,
            DonViDienTich = "m²"
        };

        // Act
        var wrapper = new ThongTinThuaDat
        {
            ThuaDat = dcThuaDat
        };

        // Assert
        Assert.That(wrapper.ThuaDat, Is.Not.Null);
        Assert.That(wrapper.ThuaDat.SoThuTuThua, Is.EqualTo("456"));
        Assert.That(wrapper.ThuaDat.SoToBanDo, Is.EqualTo("78"));
        Assert.That(wrapper.ThuaDat.DienTich, Is.EqualTo(200.5m));
        Assert.That(wrapper.ThuaDat.DonViDienTich, Is.EqualTo("m²"));
    }

    [Test]
    public void ThongTinTaiSan_ShouldSupportMultipleAssetTypes()
    {
        // Arrange & Act
        var taiSan = new ThongTinTaiSan
        {
            ThongTinThuaDat = new ThongTinThuaDat
            {
                ThuaDat = new DcThuaDat { SoThuTuThua = "123" }
            },
            ThongTinNhaRiengLe = new ThongTinNhaRiengLe
            {
                NhaRiengLe = new DcNhaRiengLe { CapNha = "1 tầng" }
            }
        };

        // Assert
        Assert.That(taiSan.ThongTinThuaDat?.ThuaDat?.SoThuTuThua, Is.EqualTo("123"));
        Assert.That(taiSan.ThongTinNhaRiengLe?.NhaRiengLe?.CapNha, Is.EqualTo("1 tầng"));
        Assert.That(taiSan.ThongTinKhuChungCu, Is.Null);
        Assert.That(taiSan.ThongTinNhaChungCu, Is.Null);
        Assert.That(taiSan.ThongTinCanHo, Is.Null);
    }

    [Test]
    [TestCase(0)]
    [TestCase(50.5)]
    [TestCase(1000.75)]
    [TestCase(null)]
    public void DcThuaDat_DienTich_ShouldAcceptValidValues(decimal? dienTich)
    {
        // Arrange & Act
        var thuaDat = new DcThuaDat
        {
            DienTich = dienTich
        };

        // Assert
        Assert.That(thuaDat.DienTich, Is.EqualTo(dienTich));
    }

    [Test]
    public void DcNhaRiengLe_WithValidData_ShouldStoreCorrectly()
    {
        // Arrange & Act
        var nhaRiengLe = new DcNhaRiengLe
        {
            CapNha = "2 tầng",
            DienTichSan = 80.5m,
            DonViDienTichSan = "m²",
            KetCauChinh = "Bê tông cốt thép"
        };

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(nhaRiengLe.CapNha, Is.EqualTo("2 tầng"));
            Assert.That(nhaRiengLe.DienTichSan, Is.EqualTo(80.5m));
            Assert.That(nhaRiengLe.DonViDienTichSan, Is.EqualTo("m²"));
            Assert.That(nhaRiengLe.KetCauChinh, Is.EqualTo("Bê tông cốt thép"));
        });
    }

    [Test]
    public void DcCanHo_WithApartmentInfo_ShouldStoreCorrectly()
    {
        // Arrange & Act
        var canHo = new DcCanHo
        {
            SoCanHo = "A101",
            Tang = "Tầng 10",
            DienTichSan = 65.3m,
            DonViDienTichSan = "m²"
        };

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(canHo.SoCanHo, Is.EqualTo("A101"));
            Assert.That(canHo.Tang, Is.EqualTo("Tầng 10"));
            Assert.That(canHo.DienTichSan, Is.EqualTo(65.3m));
            Assert.That(canHo.DonViDienTichSan, Is.EqualTo("m²"));
        });
    }

    [Test]
    public void TaiSan_MultipleProperties_ShouldBeIndependent()
    {
        // Arrange
        var taiSan1 = new ThongTinTaiSan
        {
            ThongTinThuaDat = new ThongTinThuaDat
            {
                ThuaDat = new DcThuaDat { SoThuTuThua = "001" }
            }
        };

        var taiSan2 = new ThongTinTaiSan
        {
            ThongTinThuaDat = new ThongTinThuaDat
            {
                ThuaDat = new DcThuaDat { SoThuTuThua = "002" }
            }
        };

        // Assert
        Assert.That(taiSan1.ThongTinThuaDat?.ThuaDat?.SoThuTuThua, Is.EqualTo("001"));
        Assert.That(taiSan2.ThongTinThuaDat?.ThuaDat?.SoThuTuThua, Is.EqualTo("002"));
        Assert.That(taiSan1.ThongTinThuaDat?.ThuaDat?.SoThuTuThua,
                   Is.Not.EqualTo(taiSan2.ThongTinThuaDat?.ThuaDat?.SoThuTuThua));
    }
}
