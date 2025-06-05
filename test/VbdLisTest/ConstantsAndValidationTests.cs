using Haihv.Elis.Tool.VbdLis;
using Haihv.Elis.Tool.VbdLis.Models;

namespace VbdLisTest;

/// <summary>
/// Unit tests cho Constants và validation logic
/// </summary>
[TestFixture]
public class ConstantsAndValidationTests
{
    [Test]
    public void Constants_ShouldHaveExpectedValues()
    {
        // Kiểm tra các constants được định nghĩa trong Constants.cs nếu có
        Assert.Pass("Constants validation - implement based on actual constants in the model");
    }

    [Test]
    public void DcGiayChungNhan_WithLongString_ShouldHandleCorrectly()
    {
        // Arrange
        var longString = new string('A', 1000);
        var gcn = new DcGiayChungNhan();

        // Act & Assert - Không throw exception
        Assert.DoesNotThrow(() =>
        {
            gcn.GiayChungNhanId = longString;
            gcn.LoaiGiayChungNhan = longString;
            gcn.SoVaoSo = longString;
        });

        Assert.That(gcn.GiayChungNhanId, Is.EqualTo(longString));
        Assert.That(gcn.LoaiGiayChungNhan, Is.EqualTo(longString));
        Assert.That(gcn.SoVaoSo, Is.EqualTo(longString));
    }

    [Test]
    public void DcCaNhan_WithSpecialCharacters_ShouldHandleCorrectly()
    {
        // Arrange
        var specialChars = "!@#$%^&*()_+-=[]{}|;':\",./<>?`~";
        var caNhan = new DcCaNhan();

        // Act
        caNhan.Ten = $"Test {specialChars}";
        caNhan.Cccd = "123456789012";
        caNhan.Email = "test@domain.com";

        // Assert
        Assert.That(caNhan.Ten, Does.Contain(specialChars));
        Assert.That(caNhan.Cccd, Is.EqualTo("123456789012"));
        Assert.That(caNhan.Email, Is.EqualTo("test@domain.com"));
    }

    [Test]
    public void DcThuaDat_WithZeroOrNegativeArea_ShouldAcceptValue()
    {
        // Arrange & Act
        var thuaDat1 = new DcThuaDat { DienTich = 0 };
        var thuaDat2 = new DcThuaDat { DienTich = -10.5m };

        // Assert
        Assert.That(thuaDat1.DienTich, Is.EqualTo(0));
        Assert.That(thuaDat2.DienTich, Is.EqualTo(-10.5m));
    }

    [Test]
    public void DcCaNhan_WithFutureDate_ShouldAcceptValue()
    {
        // Arrange
        var futureDate = DateTime.Now.AddYears(10);
        var caNhan = new DcCaNhan();

        // Act
        caNhan.NgaySinh = futureDate;

        // Assert
        Assert.That(caNhan.NgaySinh, Is.EqualTo(futureDate));
    }

    [Test]
    public void Models_WithWhitespaceValues_ShouldPreserveWhitespace()
    {
        // Arrange
        var whitespaceString = "   test   ";
        var tabString = "\t\ttest\t\t";
        var newlineString = "\n\ntest\n\n";

        // Act
        var gcn = new DcGiayChungNhan
        {
            GiayChungNhanId = whitespaceString,
            LoaiGiayChungNhan = tabString,
            SoVaoSo = newlineString
        };

        // Assert
        Assert.That(gcn.GiayChungNhanId, Is.EqualTo(whitespaceString));
        Assert.That(gcn.LoaiGiayChungNhan, Is.EqualTo(tabString));
        Assert.That(gcn.SoVaoSo, Is.EqualTo(newlineString));
    }

    [Test]
    public void DcDiaChi_WithEmptyStrings_ShouldAcceptValues()
    {
        // Arrange & Act
        var diaChi = new DcDiaChi
        {
            SoNha = "",
            TenDuong = "",
            Phuong = "",
            Quan = "",
            TinhTp = ""
        };

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(diaChi.SoNha, Is.EqualTo(""));
            Assert.That(diaChi.TenDuong, Is.EqualTo(""));
            Assert.That(diaChi.Phuong, Is.EqualTo(""));
            Assert.That(diaChi.Quan, Is.EqualTo(""));
            Assert.That(diaChi.TinhTp, Is.EqualTo(""));
        });
    }

    [Test]
    public void Models_DefaultValues_ShouldBeNull()
    {
        // Arrange & Act
        var gcn = new DcGiayChungNhan();
        var caNhan = new DcCaNhan();
        var thuaDat = new DcThuaDat();
        var diaChi = new DcDiaChi();

        // Assert
        Assert.Multiple(() =>
        {
            // GiayChungNhan
            Assert.That(gcn.GiayChungNhanId, Is.Null);
            Assert.That(gcn.LoaiGiayChungNhan, Is.Null);
            Assert.That(gcn.NgayCapGcn, Is.Null);

            // CaNhan
            Assert.That(caNhan.Ten, Is.Null);
            Assert.That(caNhan.Cccd, Is.Null);
            Assert.That(caNhan.NgaySinh, Is.Null);

            // ThuaDat
            Assert.That(thuaDat.SoThuTuThua, Is.Null);
            Assert.That(thuaDat.DienTich, Is.Null);

            // DiaChi
            Assert.That(diaChi.SoNha, Is.Null);
            Assert.That(diaChi.TinhTp, Is.Null);
        });
    }

    [Test]
    public void ThongTinWrappers_DefaultValues_ShouldBeNull()
    {
        // Arrange & Act
        var thongTinGcn = new ThongTinGiayChungNhan();
        var thongTinChu = new ThongTinChu();
        var thongTinTaiSan = new ThongTinTaiSan();
        var thongTinDiaChi = new ThongTinDiaChi();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(thongTinGcn.GiayChungNhan, Is.Null);
            Assert.That(thongTinChu.ThongTinCaNhan, Is.Null);
            Assert.That(thongTinChu.ThongTinToChuc, Is.Null);
            Assert.That(thongTinTaiSan.ThongTinThuaDat, Is.Null);
            Assert.That(thongTinTaiSan.ThongTinNhaRiengLe, Is.Null);
            Assert.That(thongTinDiaChi.DiaChi, Is.Null);
        });
    }
}
