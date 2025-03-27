namespace Haihv.Elis.Tool.TraCuuGcn.Models;

public record GiayChungNhan(
    long MaGcn,
    long MaDangKy,
    int MaHinhThucSh,
    double DienTichRieng,
    double DienTichChung,
    string Serial,
    DateTime NgayKy,
    string NguoiKy,
    string SoVaoSo,
    string MaHoSoDVC,
    string MaDonViInGCN,
    string MaVach
);

public class PhapLyGiayChungNhan
{
    public long MaGcn { get; init; }
    public string? Serial { get; init; }
    public DateTime? NgayKy { get; set; }
    public string? NguoiKy { get; set; }
    public string? SoVaoSo { get; set; }
}