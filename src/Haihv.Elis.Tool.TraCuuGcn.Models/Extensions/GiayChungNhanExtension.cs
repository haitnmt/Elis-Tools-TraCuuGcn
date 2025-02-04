namespace Haihv.Elis.Tool.TraCuuGcn.Models.Extensions;

public static class GiayChungNhanExtension
{
    public static GiayChungNhanInfo ToGiayChungNhanInfo(this GiayChungNhan? giayChungNhan, MaQrInfo? maQrInfo)
    {
        var donVi = maQrInfo?.TenDonVi ?? string.Empty;
        donVi = string.IsNullOrWhiteSpace(donVi) ? donVi : $"{donVi} [{maQrInfo?.MaDonVi}]";
        return new GiayChungNhanInfo(
            giayChungNhan?.MaGcn ?? maQrInfo?.MaGcnElis ?? 0,
            giayChungNhan?.Serial ?? maQrInfo?.SerialNumber ?? string.Empty,
            maQrInfo?.MaGiayChungNhan ?? string.Empty,
            donVi,
            maQrInfo?.MaHoSoTthc ?? string.Empty,
            giayChungNhan?.NgayKy ?? DateTime.MinValue,
            giayChungNhan?.NguoiKy ?? string.Empty,
            giayChungNhan?.SoVaoSo ?? string.Empty,
            maQrInfo?.HieuLuc ?? false
        );
    }
}