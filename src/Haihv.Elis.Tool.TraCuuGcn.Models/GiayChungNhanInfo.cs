namespace Haihv.Elis.Tool.TraCuuGcn.Models;

public record GiayChungNhanInfo(
    long MaGcnElis,
    string? Serial,
    string? MaGiayChungNhan,
    string? DonVi,
    string? MaHoSo,
    DateTime? NgayKy,
    string? NguoiKy,
    string? SoVaoSo,
    DateTime? NgayVaoSo,
    bool HieuLucMaQr,
    DateTime? KhoiTaoQr,
    string? PhanMemInGcn,
    bool HasGiayChungNhan,
    bool HasMaQr,
    bool MaQrVerified);
    