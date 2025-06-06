using Haihv.Elis.Tool.VbdLis.DanhMuc;

namespace VbdLisTest;

[TestFixture]
public class DanhMucLoaiDoiTuongSuDungDatTest
{
    [Test]
    public void GetMaByTen_ReturnsCorrectCode_ForValidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetMaByTen("Người sử dụng đất"), Is.EqualTo("NSD"));
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetMaByTen("Hộ gia đình, cá nhân"), Is.EqualTo("GDC"));
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetMaByTen("Tổ chức kinh tế"), Is.EqualTo("TKT"));
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetMaByTen("Uỷ ban nhân dân cấp xã"), Is.EqualTo("UBQ"));
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetMaByTen("Doanh nghiệp có vốn đầu tư nước ngoài"), Is.EqualTo("TVN"));
        });
    }

    [Test]
    public void GetMaByTen_ReturnsCorrectCode_WithNormalization()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetMaByTen("nguoi su dung dat"), Is.EqualTo("NSD"));
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetMaByTen("ho gia dinh ca nhan"), Is.EqualTo("GDC"));
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetMaByTen("to chuc kinh te"), Is.EqualTo("TKT"));
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetMaByTen("uy ban nhan dan cap xa"), Is.EqualTo("UBQ"));
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetMaByTen("doanh nghiep co von dau tu nuoc ngoai"), Is.EqualTo("TVN"));
        });
    }

    [Test]
    public void GetMaByTen_ReturnsNull_ForInvalidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetMaByTen("Nonexistent"), Is.Null);
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetMaByTen(""), Is.Null);
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetMaByTen(null!), Is.Null);
        });
    }

    [Test]
    public void GetTenByMa_ReturnsCorrectName_ForValidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetTenByMa("NSD"), Is.EqualTo("Người sử dụng đất"));
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetTenByMa("GDC"), Is.EqualTo("Hộ gia đình, cá nhân"));
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetTenByMa("TKT"), Is.EqualTo("Tổ chức kinh tế"));
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetTenByMa("UBQ"), Is.EqualTo("Uỷ ban nhân dân cấp xã"));
        });
    }

    [Test]
    public void GetTenByMa_ReturnsNull_ForInvalidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetTenByMa("XXX"), Is.Null);
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetTenByMa(""), Is.Null);
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.GetTenByMa(null!), Is.Null);
        });
    }

    [Test]
    public void TatCa_ContainsAllUserTypes_AndNotEmpty()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.TatCa, Is.Not.Empty);
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.TatCa.Count, Is.GreaterThan(0));

            // Kiểm tra một số mục quan trọng có tồn tại
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.TatCa.Any(x => x.Ma == "NSD"), Is.True);
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.TatCa.Any(x => x.Ma == "GDC"), Is.True);
            Assert.That(DanhMucLoaiDoiTuongSuDungDat.TatCa.Any(x => x.Ma == "TKT"), Is.True);
        });
    }

    [Test]
    public void TatCa_AllItemsHaveValidMaAndTen()
    {
        foreach (var item in DanhMucLoaiDoiTuongSuDungDat.TatCa)
        {
            Assert.Multiple(() =>
            {
                Assert.That(string.IsNullOrWhiteSpace(item.Ma), Is.False, $"Mã không được rỗng cho item: {item.Ten}");
                Assert.That(string.IsNullOrWhiteSpace(item.Ten), Is.False, $"Tên không được rỗng cho mã: {item.Ma}");
            });
        }
    }
}
