using Haihv.Elis.Tool.VbdLis;
using Haihv.Elis.Tool.VbdLis.Models;

namespace VbdLisTest;

/// <summary>
/// Unit tests cho các model liên quan đến Giấy Chứng Nhận
/// </summary>
[TestFixture]
public class GiayChungNhanTests
{
    [Test]
    public void DcGiayChungNhan_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var gcn = new DcGiayChungNhan();

        // Assert
        Assert.That(gcn.GiayChungNhanId, Is.Null);
        Assert.That(gcn.LoaiGiayChungNhan, Is.Null);
        Assert.That(gcn.SoVaoSo, Is.Null);
    }

    [Test]
    public void DcGiayChungNhan_SetProperties_ShouldRetainValues()
    {
        // Arrange
        var gcn = new DcGiayChungNhan();
        var testDate = new DateTime(2024, 12, 1);

        // Act
        gcn.GiayChungNhanId = "GCN12345";
        gcn.LoaiGiayChungNhan = "QSDĐ";
        gcn.SoVaoSo = "001/2024";
        gcn.NgayCapGcn = testDate;

        // Assert
        Assert.That(gcn.GiayChungNhanId, Is.EqualTo("GCN12345"));
        Assert.That(gcn.LoaiGiayChungNhan, Is.EqualTo("QSDĐ"));
        Assert.That(gcn.SoVaoSo, Is.EqualTo("001/2024"));
        Assert.That(gcn.NgayCapGcn, Is.EqualTo(testDate));
    }

    [Test]
    public void ThongTinGiayChungNhan_WithValidData_ShouldWrapCorrectly()
    {
        // Arrange
        var dcGcn = new DcGiayChungNhan
        {
            GiayChungNhanId = "GCN001",
            LoaiGiayChungNhan = "QSDĐ",
            SoVaoSo = "123/2024"
        };

        // Act
        var wrapper = new ThongTinGiayChungNhan
        {
            GiayChungNhan = dcGcn
        };

        // Assert
        Assert.That(wrapper.GiayChungNhan, Is.Not.Null);
        Assert.That(wrapper.GiayChungNhan.GiayChungNhanId, Is.EqualTo("GCN001"));
        Assert.That(wrapper.GiayChungNhan.LoaiGiayChungNhan, Is.EqualTo("QSDĐ"));
        Assert.That(wrapper.GiayChungNhan.SoVaoSo, Is.EqualTo("123/2024"));
    }

    [Test]
    [TestCase("")]
    [TestCase(null)]
    public void DcGiayChungNhan_WithEmptyOrNullId_ShouldAcceptValue(string? id)
    {
        // Arrange & Act
        var gcn = new DcGiayChungNhan
        {
            GiayChungNhanId = id
        };

        // Assert
        Assert.That(gcn.GiayChungNhanId, Is.EqualTo(id));
    }

    [Test]
    public void DcGiayChungNhan_MultipleInstances_ShouldBeIndependent()
    {
        // Arrange & Act
        var gcn1 = new DcGiayChungNhan { GiayChungNhanId = "GCN001" };
        var gcn2 = new DcGiayChungNhan { GiayChungNhanId = "GCN002" };

        // Assert
        Assert.That(gcn1.GiayChungNhanId, Is.EqualTo("GCN001"));
        Assert.That(gcn2.GiayChungNhanId, Is.EqualTo("GCN002"));
        Assert.That(gcn1.GiayChungNhanId, Is.Not.EqualTo(gcn2.GiayChungNhanId));
    }
}
