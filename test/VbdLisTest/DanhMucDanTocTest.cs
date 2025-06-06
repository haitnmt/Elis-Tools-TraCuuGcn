using Haihv.Elis.Tool.VbdLis.DanhMuc;

namespace VbdLisTest;

[TestFixture]
public class DanhMucDanTocTest
{
    [Test]
    public void GetMaByTen_ReturnsCorrectCode_ForValidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiDanToc.GetMaByTen("Kinh"), Is.EqualTo(1));
            Assert.That(DanhMucLoaiDanToc.GetMaByTen("Tày"), Is.EqualTo(2));
            Assert.That(DanhMucLoaiDanToc.GetMaByTen("H'Mông"), Is.EqualTo(8));
            Assert.That(DanhMucLoaiDanToc.GetMaByTen("Ê Đê"), Is.EqualTo(11));
        });
    }
    [Test]
    public void GetMaByTen_ReturnsCorrectCode_WithNormalization()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiDanToc.GetMaByTen("kinh"), Is.EqualTo(1));
            Assert.That(DanhMucLoaiDanToc.GetMaByTen("tay"), Is.EqualTo(2));
            Assert.That(DanhMucLoaiDanToc.GetMaByTen("hmong"), Is.EqualTo(8));
            Assert.That(DanhMucLoaiDanToc.GetMaByTen("e de"), Is.EqualTo(11));
            Assert.That(DanhMucLoaiDanToc.GetMaByTen("cham"), Is.EqualTo(16));
            Assert.That(DanhMucLoaiDanToc.GetMaByTen("Ơ Đu"), Is.EqualTo(53));
            Assert.That(DanhMucLoaiDanToc.GetMaByTen("o du"), Is.EqualTo(53));
        });
    }
    [Test]
    public void GetMaByTen_ReturnsNull_ForInvalidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiDanToc.GetMaByTen("Nonexistent"), Is.Null);
            Assert.That(DanhMucLoaiDanToc.GetMaByTen(""), Is.Null);
            Assert.That(DanhMucLoaiDanToc.GetMaByTen(null!), Is.Null);
        });
    }
    [Test]
    public void GetTenByMa_ReturnsCorrectName_ForValidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiDanToc.GetTenByMa(1), Is.EqualTo("Kinh"));
            Assert.That(DanhMucLoaiDanToc.GetTenByMa(2), Is.EqualTo("Tày"));
            Assert.That(DanhMucLoaiDanToc.GetTenByMa(8), Is.EqualTo("H'Mông"));
            Assert.That(DanhMucLoaiDanToc.GetTenByMa(11), Is.EqualTo("Ê Đê"));
        });
    }
    [Test]
    public void GetTenByMa_ReturnsNull_ForInvalidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiDanToc.GetTenByMa(0), Is.Null);
            Assert.That(DanhMucLoaiDanToc.GetTenByMa(100), Is.Null);
        });
    }
    [Test]
    public void TatCa_ContainsAll54Ethnicities_AndOrderedByCode()
    {
        Assert.That(DanhMucLoaiDanToc.TatCa, Has.Count.EqualTo(54));
        for (var i = 0; i < DanhMucLoaiDanToc.TatCa.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(DanhMucLoaiDanToc.TatCa[i].Ma, Is.EqualTo(i + 1));
                Assert.That(string.IsNullOrWhiteSpace(DanhMucLoaiDanToc.TatCa[i].Ten), Is.False);
            });
        }
    }
}