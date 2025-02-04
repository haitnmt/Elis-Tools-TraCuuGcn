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
    bool HieuLucMaQr);
    