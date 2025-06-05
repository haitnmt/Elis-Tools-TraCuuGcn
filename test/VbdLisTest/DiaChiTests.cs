using Haihv.Elis.Tool.VbdLis;
using Haihv.Elis.Tool.VbdLis.Models;

namespace VbdLisTest;

/// <summary>
/// Unit tests cho các model liên quan đến Địa chỉ
/// </summary>
[TestFixture]
public class DiaChiTests
{
    [Test]
    public void DcDiaChi_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var diaChi = new DcDiaChi();

        // Assert
        Assert.That(diaChi.SoNha, Is.Null);
        Assert.That(diaChi.TenDuong, Is.Null);
        Assert.That(diaChi.Phuong, Is.Null);
        Assert.That(diaChi.Quan, Is.Null);
        Assert.That(diaChi.TinhTp, Is.Null);
    }

    [Test]
    public void DcDiaChi_SetFullAddress_ShouldRetainAllValues()
    {
        // Arrange
        var diaChi = new DcDiaChi();

        // Act
        diaChi.SoNha = "123";
        diaChi.TenDuong = "Đường Nguyễn Trãi";
        diaChi.Phuong = "Phường Bến Thành";
        diaChi.Quan = "Quận 1";
        diaChi.TinhTp = "TP. Hồ Chí Minh";

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(diaChi.SoNha, Is.EqualTo("123"));
            Assert.That(diaChi.TenDuong, Is.EqualTo("Đường Nguyễn Trãi"));
            Assert.That(diaChi.Phuong, Is.EqualTo("Phường Bến Thành"));
            Assert.That(diaChi.Quan, Is.EqualTo("Quận 1"));
            Assert.That(diaChi.TinhTp, Is.EqualTo("TP. Hồ Chí Minh"));
        });
    }

    [Test]
    public void ThongTinDiaChi_WithValidData_ShouldWrapCorrectly()
    {
        // Arrange
        var dcDiaChi = new DcDiaChi
        {
            SoNha = "456",
            TenDuong = "Đường Lê Lợi",
            Phuong = "Phường 1",
            Quan = "Quận 1",
            TinhTp = "TP. Hồ Chí Minh"
        };

        // Act
        var wrapper = new ThongTinDiaChi
        {
            DiaChi = dcDiaChi
        };

        // Assert
        Assert.That(wrapper.DiaChi, Is.Not.Null);
        Assert.That(wrapper.DiaChi.SoNha, Is.EqualTo("456"));
        Assert.That(wrapper.DiaChi.TenDuong, Is.EqualTo("Đường Lê Lợi"));
        Assert.That(wrapper.DiaChi.Phuong, Is.EqualTo("Phường 1"));
        Assert.That(wrapper.DiaChi.Quan, Is.EqualTo("Quận 1"));
        Assert.That(wrapper.DiaChi.TinhTp, Is.EqualTo("TP. Hồ Chí Minh"));
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    [TestCase("123A")]
    [TestCase("45/67")]
    public void DcDiaChi_SoNha_ShouldAcceptVariousFormats(string? soNha)
    {
        // Arrange & Act
        var diaChi = new DcDiaChi
        {
            SoNha = soNha
        };

        // Assert
        Assert.That(diaChi.SoNha, Is.EqualTo(soNha));
    }

    [Test]
    public void DcDiaChi_PartialAddress_ShouldStoreCorrectly()
    {
        // Arrange & Act
        var diaChi = new DcDiaChi
        {
            Phuong = "Phường Tân Bình",
            Quan = "Quận Tân Bình",
            TinhTp = "TP. Hồ Chí Minh"
        };

        // Assert
        Assert.That(diaChi.SoNha, Is.Null);
        Assert.That(diaChi.TenDuong, Is.Null);
        Assert.That(diaChi.Phuong, Is.EqualTo("Phường Tân Bình"));
        Assert.That(diaChi.Quan, Is.EqualTo("Quận Tân Bình"));
        Assert.That(diaChi.TinhTp, Is.EqualTo("TP. Hồ Chí Minh"));
    }

    [Test]
    public void DcDiaChi_MultipleInstances_ShouldBeIndependent()
    {
        // Arrange & Act
        var diaChi1 = new DcDiaChi
        {
            SoNha = "123",
            Phuong = "Phường A"
        };

        var diaChi2 = new DcDiaChi
        {
            SoNha = "456",
            Phuong = "Phường B"
        };

        // Assert
        Assert.That(diaChi1.SoNha, Is.EqualTo("123"));
        Assert.That(diaChi1.Phuong, Is.EqualTo("Phường A"));
        Assert.That(diaChi2.SoNha, Is.EqualTo("456"));
        Assert.That(diaChi2.Phuong, Is.EqualTo("Phường B"));
        Assert.That(diaChi1.SoNha, Is.Not.EqualTo(diaChi2.SoNha));
        Assert.That(diaChi1.Phuong, Is.Not.EqualTo(diaChi2.Phuong));
    }
}
