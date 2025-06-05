using Haihv.Elis.Tool.VbdLis;
using Haihv.Elis.Tool.VbdLis.Models;

namespace VbdLisTest;

/// <summary>
/// Unit tests cho các model liên quan đến Chủ sở hữu
/// </summary>
[TestFixture]
public class ChuSoHuuTests
{
    [Test]
    public void DcCaNhan_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var caNhan = new DcCaNhan();

        // Assert
        Assert.That(caNhan.Ten, Is.Null);
        Assert.That(caNhan.Cccd, Is.Null);
        Assert.That(caNhan.GioiTinh, Is.Null);
        Assert.That(caNhan.NgaySinh, Is.Null);
    }

    [Test]
    public void DcCaNhan_SetProperties_ShouldRetainValues()
    {
        // Arrange
        var caNhan = new DcCaNhan();
        var ngaySinh = new DateTime(1990, 5, 15);

        // Act
        caNhan.Ten = "Nguyễn Văn A";
        caNhan.Cccd = "123456789012";
        caNhan.GioiTinh = "Nam";
        caNhan.NgaySinh = ngaySinh;
        caNhan.SoDienThoai = "0901234567";
        caNhan.Email = "nguyenvana@email.com";

        // Assert
        Assert.That(caNhan.Ten, Is.EqualTo("Nguyễn Văn A"));
        Assert.That(caNhan.Cccd, Is.EqualTo("123456789012"));
        Assert.That(caNhan.GioiTinh, Is.EqualTo("Nam"));
        Assert.That(caNhan.NgaySinh, Is.EqualTo(ngaySinh));
        Assert.That(caNhan.SoDienThoai, Is.EqualTo("0901234567"));
        Assert.That(caNhan.Email, Is.EqualTo("nguyenvana@email.com"));
    }

    [Test]
    public void ThongTinCaNhan_WithValidData_ShouldWrapCorrectly()
    {
        // Arrange
        var dcCaNhan = new DcCaNhan
        {
            Ten = "Trần Thị B",
            Cccd = "987654321098",
            GioiTinh = "Nữ"
        };

        // Act
        var wrapper = new ThongTinCaNhan
        {
            CaNhan = dcCaNhan
        };

        // Assert
        Assert.That(wrapper.CaNhan, Is.Not.Null);
        Assert.That(wrapper.CaNhan.Ten, Is.EqualTo("Trần Thị B"));
        Assert.That(wrapper.CaNhan.Cccd, Is.EqualTo("987654321098"));
        Assert.That(wrapper.CaNhan.GioiTinh, Is.EqualTo("Nữ"));
    }

    [Test]
    public void ThongTinChu_ShouldSupportMultipleOwnerTypes()
    {
        // Arrange & Act
        var thongTinChu = new ThongTinChu
        {
            ThongTinCaNhan = new ThongTinCaNhan
            {
                CaNhan = new DcCaNhan { Ten = "Cá nhân" }
            },
            ThongTinToChuc = new ThongTinToChuc
            {
                ToChuc = new DcToChuc { TenToChuc = "Tổ chức" }
            }
        };

        // Assert
        Assert.That(thongTinChu.ThongTinCaNhan?.CaNhan?.Ten, Is.EqualTo("Cá nhân"));
        Assert.That(thongTinChu.ThongTinToChuc?.ToChuc?.TenToChuc, Is.EqualTo("Tổ chức"));
        Assert.That(thongTinChu.ThongTinVoChong, Is.Null);
        Assert.That(thongTinChu.ThongTinHoGiaDinh, Is.Null);
        Assert.That(thongTinChu.ThongTinCongDong, Is.Null);
    }

    [Test]
    [TestCase("Nam")]
    [TestCase("Nữ")]
    [TestCase("")]
    [TestCase(null)]
    public void DcCaNhan_GioiTinh_ShouldAcceptValidValues(string? gioiTinh)
    {
        // Arrange & Act
        var caNhan = new DcCaNhan
        {
            GioiTinh = gioiTinh
        };

        // Assert
        Assert.That(caNhan.GioiTinh, Is.EqualTo(gioiTinh));
    }

    [Test]
    public void DcCaNhan_WithFullInformation_ShouldStoreAllData()
    {
        // Arrange
        var ngaySinh = new DateTime(1985, 12, 25);
        var caNhan = new DcCaNhan();

        // Act
        caNhan.Ten = "Lê Văn C";
        caNhan.Cccd = "456789123456";
        caNhan.GioiTinh = "Nam";
        caNhan.NgaySinh = ngaySinh;
        caNhan.SoDienThoai = "0987654321";
        caNhan.Email = "levanc@test.com";
        caNhan.QuocTich = "Việt Nam";

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(caNhan.Ten, Is.EqualTo("Lê Văn C"));
            Assert.That(caNhan.Cccd, Is.EqualTo("456789123456"));
            Assert.That(caNhan.GioiTinh, Is.EqualTo("Nam"));
            Assert.That(caNhan.NgaySinh, Is.EqualTo(ngaySinh));
            Assert.That(caNhan.SoDienThoai, Is.EqualTo("0987654321"));
            Assert.That(caNhan.Email, Is.EqualTo("levanc@test.com"));
            Assert.That(caNhan.QuocTich, Is.EqualTo("Việt Nam"));
        });
    }
}
