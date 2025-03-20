namespace Haihv.Elis.Tool.TraCuuGcn.Models;

public class MaQrInfo
{
    public Guid? MaQrId { get; set; }
    public DateTime KhoiTao { get; set; }
    public string? MaDonVi { get; set; }
    public string? TenDonVi { get; set; }
    public string? PhanMemInGcn { get; set; }
    public string? MaHoSoTthc { get; set; }
    public string? SerialNumber { get; set; }
    public string? MaGiayChungNhan { get; set; }
    public long MaGcnElis { get; set; }
    public DateTime ChinhSua { get; set; }
    public int SecurityCode { get; set; }
    public bool HieuLuc { get; set; }
    public bool Verified { get; set; } = true;
}