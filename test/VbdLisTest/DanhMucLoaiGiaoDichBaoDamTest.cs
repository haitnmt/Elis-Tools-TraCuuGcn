using Haihv.Elis.Tool.VbdLis.DanhMuc;

namespace VbdLisTest;

[TestFixture]
public class DanhMucLoaiGiaoDichBaoDamTest
{
    [Test]
    public void GetMaByTen_ReturnsCorrectCode_ForValidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetMaByTen("Thế chấp"), Is.EqualTo(0));
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetMaByTen("Thế chấp bổ sung"), Is.EqualTo(1));
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetMaByTen("Xóa thế chấp"), Is.EqualTo(2));
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetMaByTen("Rút tài sản thế chấp"), Is.EqualTo(3));
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetMaByTen("Thay đổi nội dung thế chấp"), Is.EqualTo(4));
        });
    }

    [Test]
    public void GetMaByTen_ReturnsCorrectCode_WithNormalization()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetMaByTen("the chap"), Is.EqualTo(0));
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetMaByTen("the chap bo sung"), Is.EqualTo(1));
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetMaByTen("xoa the chap"), Is.EqualTo(2));
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetMaByTen("rut tai san the chap"), Is.EqualTo(3));
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetMaByTen("thay doi noi dung the chap"), Is.EqualTo(4));
        });
    }

    [Test]
    public void GetMaByTen_ReturnsNull_ForInvalidName()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetMaByTen("Nonexistent"), Is.Null);
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetMaByTen(""), Is.Null);
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetMaByTen(null!), Is.Null);
        });
    }

    [Test]
    public void GetTenByMa_ReturnsCorrectName_ForValidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetTenByMa(0), Is.EqualTo("Thế chấp"));
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetTenByMa(1), Is.EqualTo("Thế chấp bổ sung"));
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetTenByMa(2), Is.EqualTo("Xóa thế chấp"));
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetTenByMa(3), Is.EqualTo("Rút tài sản thế chấp"));
        });
    }

    [Test]
    public void GetTenByMa_ReturnsNull_ForInvalidCode()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetTenByMa(-1), Is.Null);
            Assert.That(DanhMucLoaiGiaoDichBaoDam.GetTenByMa(100), Is.Null);
        });
    }

    [Test]
    public void TatCa_ContainsAllTransactionTypes_AndNotEmpty()
    {
        Assert.Multiple(() =>
        {
            Assert.That(DanhMucLoaiGiaoDichBaoDam.TatCa, Is.Not.Empty);
            Assert.That(DanhMucLoaiGiaoDichBaoDam.TatCa.Length, Is.GreaterThan(0));

            // Kiểm tra một số mục quan trọng có tồn tại
            Assert.That(DanhMucLoaiGiaoDichBaoDam.TatCa.Any(x => x.Ma == 0), Is.True);
            Assert.That(DanhMucLoaiGiaoDichBaoDam.TatCa.Any(x => x.Ma == 1), Is.True);
            Assert.That(DanhMucLoaiGiaoDichBaoDam.TatCa.Any(x => x.Ma == 2), Is.True);
        });
    }

    [Test]
    public void TatCa_AllItemsHaveValidMaAndTen()
    {
        foreach (var item in DanhMucLoaiGiaoDichBaoDam.TatCa)
        {
            Assert.Multiple(() =>
            {
                Assert.That(item.Ma, Is.GreaterThanOrEqualTo(0), $"Mã phải lớn hơn hoặc bằng 0 cho item: {item.Ten}");
                Assert.That(string.IsNullOrWhiteSpace(item.Ten), Is.False, $"Tên không được rỗng cho mã: {item.Ma}");
            });
        }
    }
}
