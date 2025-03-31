namespace Haihv.Elis.Tool.TraCuuGcn.Models.Extensions;

public static class GiayChungNhanExtension
{
    public static GiayChungNhanInfo ToGiayChungNhanInfo(this GiayChungNhan? giayChungNhan, MaQrInfo? maQrInfo)
    {
        var donVi = maQrInfo?.TenDonVi ?? string.Empty;
        donVi = string.IsNullOrWhiteSpace(donVi) ? donVi : $"{donVi} [{maQrInfo?.MaDonVi}]";
        var gcnInfo = new GiayChungNhanInfo(
            giayChungNhan?.MaGcn ?? maQrInfo?.MaGcnElis ?? 0,
            !string.IsNullOrWhiteSpace(giayChungNhan?.Serial) ? giayChungNhan.Serial : maQrInfo?.SerialNumber,
            maQrInfo?.MaGiayChungNhan ?? string.Empty,
            donVi,
            maQrInfo?.MaHoSoTthc ?? giayChungNhan?.MaHoSoDVC,
            giayChungNhan?.NgayKy,
            giayChungNhan?.NguoiKy ?? string.Empty,
            giayChungNhan?.SoVaoSo ?? string.Empty,
            giayChungNhan?.NgayVaoSo,
            maQrInfo?.HieuLuc ?? false,
            maQrInfo?.KhoiTao ?? DateTime.MinValue,
            maQrInfo?.PhanMemInGcn ?? string.Empty,
            giayChungNhan?.MaGcn > 0,  
            !string.IsNullOrWhiteSpace(maQrInfo?.MaQrId.ToString()),
            maQrInfo?.Verified ?? false
        );
        return gcnInfo;
    }
}