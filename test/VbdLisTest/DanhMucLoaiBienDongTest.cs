using Haihv.Elis.Tool.VbdLis.DanhMuc;

namespace VbdLisTest;

[TestFixture]
public class DanhMucLoaiBienDongTest
{
    [Test]
    public void GetMaByTen_ReturnsCorrectCode_ForValidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiBienDong.GetMaByTen("Cấp đổi, cấp lại"), Is.EqualTo("CL"));
            Assert.That(DanhMucLoaiBienDong.GetMaByTen("Chuyển nhượng"), Is.EqualTo("CN"));
            Assert.That(DanhMucLoaiBienDong.GetMaByTen("Thừa kế"), Is.EqualTo("TK"));
            Assert.That(DanhMucLoaiBienDong.GetMaByTen("Tặng cho"), Is.EqualTo("TA"));
            Assert.That(DanhMucLoaiBienDong.GetMaByTen("Thu hồi đất"), Is.EqualTo("TH"));
        });
    }

    [Test]
    public void GetMaByTen_ReturnsCorrectCode_WithNormalization()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiBienDong.GetMaByTen("cap doi cap lai"), Is.EqualTo("CL"));
            Assert.That(DanhMucLoaiBienDong.GetMaByTen("chuyen nhuong"), Is.EqualTo("CN"));
            Assert.That(DanhMucLoaiBienDong.GetMaByTen("thua ke"), Is.EqualTo("TK"));
            Assert.That(DanhMucLoaiBienDong.GetMaByTen("tang cho"), Is.EqualTo("TA"));
            Assert.That(DanhMucLoaiBienDong.GetMaByTen("thu hoi dat"), Is.EqualTo("TH"));
        });
    }

    [Test]
    public void GetMaByTen_ReturnsNull_ForInvalidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiBienDong.GetMaByTen("Nonexistent"), Is.Null);
            Assert.That(DanhMucLoaiBienDong.GetMaByTen(""), Is.Null);
            Assert.That(DanhMucLoaiBienDong.GetMaByTen(null!), Is.Null);
        });
    }

    [Test]
    public void GetTenByMa_ReturnsCorrectName_ForValidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiBienDong.GetTenByMa("CL"), Is.EqualTo("Cấp đổi, cấp lại"));
            Assert.That(DanhMucLoaiBienDong.GetTenByMa("CN"), Is.EqualTo("Chuyển nhượng"));
            Assert.That(DanhMucLoaiBienDong.GetTenByMa("TK"), Is.EqualTo("Thừa kế"));
            Assert.That(DanhMucLoaiBienDong.GetTenByMa("TA"), Is.EqualTo("Tặng cho"));
        });
    }

    [Test]
    public void GetTenByMa_ReturnsNull_ForInvalidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiBienDong.GetTenByMa("XX"), Is.Null);
            Assert.That(DanhMucLoaiBienDong.GetTenByMa(""), Is.Null);
            Assert.That(DanhMucLoaiBienDong.GetTenByMa(null!), Is.Null);
        });
    }

    [Test]
    public void TatCa_ContainsAllChangeTypes_AndNotEmpty()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiBienDong.TatCa, Is.Not.Empty);
            Assert.That(DanhMucLoaiBienDong.TatCa.Count, Is.GreaterThan(0));

            // Kiểm tra một số mục quan trọng có tồn tại
            Assert.That(DanhMucLoaiBienDong.TatCa.Any(x => x.Ma == "CL"), Is.True);
            Assert.That(DanhMucLoaiBienDong.TatCa.Any(x => x.Ma == "CN"), Is.True);
            Assert.That(DanhMucLoaiBienDong.TatCa.Any(x => x.Ma == "TK"), Is.True);
        });
    }

    [Test]
    public void TatCa_AllItemsHaveValidMaAndTen()
    {
        foreach (var item in DanhMucLoaiBienDong.TatCa)
        {
            Assert.Multiple(() =>
            {
                Assert.That(string.IsNullOrWhiteSpace(item.Ma), Is.False, $"Mã không được rỗng cho item: {item.Ten}");
                Assert.That(string.IsNullOrWhiteSpace(item.Ten), Is.False, $"Tên không được rỗng cho mã: {item.Ma}");
            });
        }
    }
}
