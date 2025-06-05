using System.Xml.Serialization;
using System.Text;
using Haihv.Elis.Tool.VbdLis.Models;

namespace VbdLisTest;

/// <summary>
/// Integration tests cho XML Serialization/Deserialization của VbdLis models với property names đã được sửa
/// </summary>
[TestFixture]
public class XmlSerializationTests
{
    [Test]
    public void GiayChungNhan_XmlSerialization_ShouldPreserveData()
    {
        // Arrange
        var originalData = new ThongTinGiayChungNhan
        {
            GiayChungNhan = new DcGiayChungNhan
            {
                GiayChungNhanId = "GCN123456",
                LoaiGiayChungNhan = "QSDĐ",
                SoVaoSo = "001/2024/QSDĐ",
                NgayCap = new DateTime(2024, 6, 15),
                DonViCap = "UBND TP. Hồ Chí Minh"
            }
        };

        // Act
        var xml = SerializeToXml(originalData);
        var deserializedData = DeserializeFromXml<ThongTinGiayChungNhan>(xml);

        // Assert
        Assert.That(deserializedData, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(deserializedData.GiayChungNhan?.GiayChungNhanId,
                    Is.EqualTo(originalData.GiayChungNhan.GiayChungNhanId));
            Assert.That(deserializedData.GiayChungNhan?.LoaiGiayChungNhan,
                Is.EqualTo(originalData.GiayChungNhan.LoaiGiayChungNhan));
            Assert.That(deserializedData.GiayChungNhan?.SoVaoSo, Is.EqualTo(originalData.GiayChungNhan.SoVaoSo));
            Assert.That(deserializedData.GiayChungNhan?.DonViCap, Is.EqualTo(originalData.GiayChungNhan.DonViCap));
        });
    }

    [Test]
    public void ChuSoHuu_CaNhan_XmlSerialization_ShouldPreserveData()
    {
        // Arrange
        var originalData = new ThongTinChu
        {
            ThongTinCaNhan = new ThongTinCaNhan
            {
                CaNhan = new DcCaNhan
                {
                    HoTen = "Nguyễn Văn Test",
                    MaSoDinhDanh = "123456789012",
                    GioiTinh = "Nam",
                    NgaySinh = new DateTime(1985, 3, 20),
                    SoDienThoai = "0901234567",
                    Email = "test@example.com",
                    QuocTichId = "VN"
                }
            }
        };

        // Act
        var xml = SerializeToXml(originalData);
        var deserializedData = DeserializeFromXml<ThongTinChu>(xml);

        // Assert
        Assert.That(deserializedData, Is.Not.Null);
        var caNhan = deserializedData.ThongTinCaNhan?.CaNhan;
        Assert.That(caNhan, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(caNhan.HoTen, Is.EqualTo("Nguyễn Văn Test"));
            Assert.That(caNhan.MaSoDinhDanh, Is.EqualTo("123456789012"));
            Assert.That(caNhan.GioiTinh, Is.EqualTo("Nam"));
            Assert.That(caNhan.SoDienThoai, Is.EqualTo("0901234567"));
            Assert.That(caNhan.Email, Is.EqualTo("test@example.com"));
            Assert.That(caNhan.QuocTichId, Is.EqualTo("VN"));
        });
    }

    [Test]
    public void TaiSan_ThuaDat_XmlSerialization_ShouldPreserveData()
    {
        // Arrange
        var originalData = new ThongTinTaiSan
        {
            ThongTinThuaDat = new ThongTinThuaDat
            {
                ThuaDat = new DcThuaDat
                {
                    SoThuTuThua = "123",
                    SoHieuToBanDo = "45",
                    DienTich = 250.75m,
                    // Note: Bỏ các property không tồn tại như DonViDienTich, MucDichSuDung, ThoiHanSuDung
                    LoaiThuaDat = "Đất ở tại đô thị"
                }
            }
        };

        // Act
        var xml = SerializeToXml(originalData);
        var deserializedData = DeserializeFromXml<ThongTinTaiSan>(xml);

        // Assert
        Assert.That(deserializedData, Is.Not.Null);
        var thuaDat = deserializedData.ThongTinThuaDat?.ThuaDat;
        Assert.That(thuaDat, Is.Not.Null);
        Assert.That(thuaDat.SoThuTuThua, Is.EqualTo("123"));
        Assert.That(thuaDat.SoHieuToBanDo, Is.EqualTo("45"));
        Assert.That(thuaDat.DienTich, Is.EqualTo(250.75m));
        Assert.That(thuaDat.LoaiThuaDat, Is.EqualTo("Đất ở tại đô thị"));
    }

    [Test]
    public void DiaChi_XmlSerialization_ShouldPreserveData()
    {
        // Arrange
        var originalData = new ThongTinDiaChi
        {
            DiaChi = new DcDiaChi
            {
                SoNha = "123/45",
                TenDuongPho = "Đường Nguyễn Huệ",
                TenXa = "Phường Bến Nghé",
                TenQuan = "Quận 1",
                TenTinh = "TP. Hồ Chí Minh"
            }
        };

        // Act
        var xml = SerializeToXml(originalData);
        var deserializedData = DeserializeFromXml<ThongTinDiaChi>(xml);

        // Assert
        Assert.That(deserializedData, Is.Not.Null);
        var diaChi = deserializedData.DiaChi;
        Assert.That(diaChi, Is.Not.Null);
        Assert.That(diaChi.SoNha, Is.EqualTo("123/45"));
        Assert.That(diaChi.TenDuongPho, Is.EqualTo("Đường Nguyễn Huệ"));
        Assert.That(diaChi.TenXa, Is.EqualTo("Phường Bến Nghé"));
        Assert.That(diaChi.TenQuan, Is.EqualTo("Quận 1"));
        Assert.That(diaChi.TenTinh, Is.EqualTo("TP. Hồ Chí Minh"));
    }

    [Test]
    public void ComplexModel_WithNullValues_ShouldSerializeCorrectly()
    {
        // Arrange
        var originalData = new ThongTinGiayChungNhan
        {
            GiayChungNhan = new DcGiayChungNhan
            {
                GiayChungNhanId = "GCN001",
                // Một số thuộc tính để null
                LoaiGiayChungNhan = null,
                SoVaoSo = "123/2024",
                NgayCap = null
            }
        };

        // Act & Assert - Không được throw exception
        Assert.DoesNotThrow(() =>
        {
            var xml = SerializeToXml(originalData);
            var deserializedData = DeserializeFromXml<ThongTinGiayChungNhan>(xml);

            Assert.Multiple(() =>
            {
                Assert.That(deserializedData.GiayChungNhan?.GiayChungNhanId, Is.EqualTo("GCN001"));
                Assert.That(deserializedData.GiayChungNhan?.SoVaoSo, Is.EqualTo("123/2024"));
            });
        });
    }

    [Test]
    public void XmlSerialization_WithUnicodeCharacters_ShouldPreserveEncoding()
    {
        // Arrange
        var originalData = new ThongTinChu
        {
            ThongTinCaNhan = new ThongTinCaNhan
            {
                CaNhan = new DcCaNhan
                {
                    HoTen = "Nguyễn Thị Ánh Xuân",
                    QuocTichId = "VN"
                }
            }
        };

        // Act
        var xml = SerializeToXml(originalData);
        var deserializedData = DeserializeFromXml<ThongTinChu>(xml);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(deserializedData.ThongTinCaNhan?.CaNhan?.HoTen, Is.EqualTo("Nguyễn Thị Ánh Xuân"));
            Assert.That(deserializedData.ThongTinCaNhan?.CaNhan?.QuocTichId, Is.EqualTo("VN"));
        });
    }

    [Test]
    public void HoSoXml_CompleteStructure_ShouldCreateXmlLikeCauTrucSample()
    {
        // Arrange - Tạo một hồ sơ XML hoàn chỉnh theo mẫu CauTruc.xml
        var hoSoXml = new HoSoXml
        {
            HoSoGuid = Guid.NewGuid().ToString(),

            // Thông tin tình hình đăng ký
            TinhHinhDangKy = new DcTinhHinhDangKy
            {
                TinhHinhDangKyId = "TTHDK001",
                MaXa = "26734", // Mã xã/phường
                MaDon = "DK2024001",
                NgayTiepNhan = DateTime.Now.AddDays(-30),
                ThoiDiemDangKyLanDau = DateTime.Now.AddDays(-60),
                ThoiDiemDangKy = DateTime.Now.AddDays(-30),
                SoThuTu = 1,
                CoQuyenSuDung = true,
                CoQuyenSoHuu = true,
                CoQuyenQuanLy = false,
                CapGiayNguoiDaiDien = false
            },

            // Thông tin giấy chứng nhận
            ThongTinGiayChungNhan = new ThongTinGiayChungNhan
            {
                GiayChungNhan = new DcGiayChungNhan
                {
                    GiayChungNhanId = "GCN2024001",
                    LoaiGiayChungNhan = "QSDĐ",
                    SoHoSoGoc = "HSG001/2024",
                    SoVaoSo = "123/2024/QSDĐ",
                    SoPhatHanh = "PH001/2024",
                    MaVach = "VN123456789012345678",
                    TenNguoiKy = "Nguyễn Văn Giám Đốc",
                    NgayCap = DateTime.Now.AddDays(-7),
                    DaCongNhanPhapLy = true,
                    DonViCap = "UBND TP. Hồ Chí Minh",
                    GhiChuTrang1 = "Giấy chứng nhận được cấp theo quy định"
                }
            },

            // Thông tin chủ sở hữu
            ThongTinChu = new ThongTinChu
            {
                ThongTinCaNhan = new ThongTinCaNhan
                {
                    CaNhan = new DcCaNhan
                    {
                        CaNhanId = "CN001",
                        LoaiDoiTuongSuDungDat = "Cá nhân",
                        HoTen = "Nguyễn Văn An",
                        NgaySinh = new DateTime(1980, 5, 15),
                        GioiTinh = "Nam",
                        MaSoThue = "1234567890",
                        SoDienThoai = "0901234567",
                        Email = "nguyenvanan@email.com",
                        MaSoDinhDanh = "001080012345",
                        QuocTichId = "VN",
                        DiaChi = "123 Đường Nguyễn Huệ, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh",

                        // Thông tin giấy tờ tùy thân
                        ThongTinGiayToTuyThan = new ThongTinGiayToTuyThan
                        {
                            GiayToTuyThan = new DcGiayToTuyThan
                            {
                                GiayToTuyThanId = "GTTT001",
                                LoaiGiayToTuyThan = "CCCD",
                                SoGiayTo = "001080012345",
                                NgayCap = new DateTime(2020, 1, 15),
                                NoiCap = "Cục Cảnh sát ĐKQL cư trú và DLQG về dân cư"
                            }
                        }
                    }
                }
            },

            // Thông tin tài sản - Thửa đất
            ThongTinTaiSan = new ThongTinTaiSan
            {
                ThongTinThuaDat = new ThongTinThuaDat
                {
                    ThuaDat = new DcThuaDat
                    {
                        ThuaDatId = "TD001",
                        MaXa = "26734",
                        SoHieuToBanDo = "15",
                        SoThuTuThua = "123",
                        DienTich = 150.5m,
                        DienTichPhapLy = 150.5m,
                        LaDoiTuongChiemDat = false,
                        DiaChi = "123 Đường Nguyễn Huệ, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh",
                        InSoLieuCu = false,
                        LoaiThuaDat = "Đất ở tại đô thị",
                        TaiLieuDoDacId = "TLDD001",

                        // Thông tin mục đích sử dụng
                        ThongTinMucDichSuDung = new ThongTinMucDichSuDung
                        {
                            MucDichSuDung =
                                new DcMucDichSuDung
                                {
                                    MucDichSuDungId = "MDSD001",
                                    SoThuTuMucDichSuDung = 1,
                                    MaMucDichSuDung = "OTD",
                                    MaMucDichSuDungPhu = "",
                                    MaMucDichSuDungQuyHoach = "ODT",
                                    DienTich = 150.5m,
                                    ThoiHanSuDung = "Lâu dài", // Thay đổi từ null sang string
                                    NgayHetHanSuDung = null,

                                    // Thông tin nguồn gốc đất
                                    ThongTinNguonGocDat = new ThongTinNguonGocDat
                                    {
                                        NguonGocDat =
                                            new DcNguonGocDat
                                            {
                                                NguonGocDatId = "NGD001",
                                                NguonGoc = "Nhà nước giao có thu tiền sử dụng đất",
                                                NguonGocChuyenQuyen = "Giao đất",
                                                NguonGocChiTiet = "Quyết định giao đất số 123/QĐ-UBND",
                                                DienTich = 150.5m
                                            }
                                    }
                                }
                        }
                    }
                },

                // Thông tin nhà riêng lẻ
                ThongTinNhaRiengLe = new ThongTinNhaRiengLe
                {
                    NhaRiengLe =
                        new DcNhaRiengLe
                        {
                            NhaRiengLeId = "NRL001",
                            MaXa = "26734",
                            LoaiNhaRiengLe = "Nhà ở",
                            LoaiQuyenSoHuuNhaRiengLe = "Sở hữu riêng",
                            SoNha = "123",
                            ThoiHanSoHuu = "Lâu dài", // Thay đổi từ null sang string
                            DienTichXayDung = 80.0m,
                            DienTichSuDung = 80.0m,
                            DienTichSan = 200.0m,
                            SoTang = 3,
                            SoTangHam = 0,
                            NamHoanThanh = 2018,
                            KetCau = "Bê tông cốt thép",
                            CapHang = "Cấp 4",
                            DiaChi = "123 Đường Nguyễn Huệ, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh"
                        }
                }
            },

            // Thông tin địa chỉ
            ThongTinDiaChi = new ThongTinDiaChi
            {
                DiaChi =
                    new DcDiaChi
                    {
                        DiaChiId = "DC001",
                        MaXa = "26734",
                        SoNha = "123",
                        TenDuongPho = "Đường Nguyễn Huệ",
                        TenToDanPho = "Tổ 1",
                        TenXa = "Phường Bến Nghé",
                        TenQuan = "Quận 1",
                        TenTinh = "TP. Hồ Chí Minh",
                        DiaChiChiTiet = "123 Đường Nguyễn Huệ, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh"
                    }
            },

            // Thông tin quyền sử dụng đất
            ThongTinQuyenSuDungDat = new ThongTinQuyenSuDungDat
            {
                QuyenSuDungDat =
                    new DcQuyenSuDungDat
                    {
                        QuyenSuDungDatId = "QSDD001",
                        LoaiDoiTuong = "Cá nhân",
                        ChuId = "CN001",
                        ThuaDatId = "TD001",
                        MucDichSuDungId = "MDSD001",
                        GiayChungNhanId = "GCN2024001"
                    }
            },

            // Thông tin quyền sở hữu tài sản gắn liền với đất
            ThongTinQuyenSoHuuTaiSanGanLienVoiDat = new ThongTinQuyenSoHuuTaiSanGanLienVoiDat
            {
                QuyenSoHuuTaiSanGanLienVoiDat =
                    new DcQuyenSoHuuTaiSanGanLienVoiDat
                    {
                        QuyenSoHuuTaiSanGanLienVoiDatId = "QSHTS001",
                        LoaiDoiTuong = "Cá nhân",
                        ChuId = "CN001",
                        LoaiTaiSan = "Nhà ở",
                        TaiSanId = "NRL001",
                        GiayChungNhanId = "GCN2024001"
                    }
            }
        };

        // Act
        var xml = SerializeToXml(hoSoXml);
        var deserializedData = DeserializeFromXml<HoSoXml>(xml);

        // Assert
        Assert.That(deserializedData, Is.Not.Null);
        Assert.That(deserializedData.HoSoGuid, Is.Not.Null.And.Not.Empty);

        // Kiểm tra thông tin giấy chứng nhận
        var gcn = deserializedData.ThongTinGiayChungNhan?.GiayChungNhan;
        Assert.That(gcn, Is.Not.Null);
        Assert.That(gcn.GiayChungNhanId, Is.EqualTo("GCN2024001"));
        Assert.That(gcn.LoaiGiayChungNhan, Is.EqualTo("QSDĐ"));
        Assert.That(gcn.SoVaoSo, Is.EqualTo("123/2024/QSDĐ"));

        // Kiểm tra thông tin chủ sở hữu
        var chuSoHuu = deserializedData.ThongTinChu?.ThongTinCaNhan?.CaNhan;
        Assert.That(chuSoHuu, Is.Not.Null);
        Assert.That(chuSoHuu.HoTen, Is.EqualTo("Nguyễn Văn An"));
        Assert.That(chuSoHuu.MaSoDinhDanh, Is.EqualTo("001080012345"));

        // Kiểm tra thông tin thửa đất
        var thuaDat = deserializedData.ThongTinTaiSan?.ThongTinThuaDat?.ThuaDat;
        Assert.That(thuaDat, Is.Not.Null);
        Assert.That(thuaDat.SoThuTuThua, Is.EqualTo("123"));
        Assert.That(thuaDat.DienTich, Is.EqualTo(150.5m));

        // Kiểm tra thông tin nhà ở
        var nhaRiengLe = deserializedData.ThongTinTaiSan?.ThongTinNhaRiengLe?.NhaRiengLe;
        Assert.That(nhaRiengLe, Is.Not.Null);
        Assert.That(nhaRiengLe.SoNha, Is.EqualTo("123"));
        Assert.That(nhaRiengLe.DienTichXayDung, Is.EqualTo(80.0m));

        // In ra file XML để kiểm tra cấu trúc
        Console.WriteLine("=== XML được tạo ra ===");
        Console.WriteLine(xml);

        // Lưu file XML để so sánh với CauTruc.xml
        var outputPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "GeneratedHoSo.xml");
        File.WriteAllText(outputPath, xml, Encoding.UTF8);
        Console.WriteLine($"\nFile XML đã được lưu tại: {outputPath}");
    }

    #region Helper Methods

    private static string SerializeToXml<T>(T obj)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var stringWriter = new StringWriter();
        serializer.Serialize(stringWriter, obj);
        return stringWriter.ToString();
    }

    private static T DeserializeFromXml<T>(string xml)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var stringReader = new StringReader(xml);
        var result = serializer.Deserialize(stringReader);
        return (T)result!;
    }

    #endregion
}