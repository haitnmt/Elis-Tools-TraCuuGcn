using System.Text.Json.Serialization;

namespace Haihv.Elis.Tool.TraCuuGcn.Models;

public class ThuaDat(
    long MaGcn,
    int MaDvhc,
    string ThuaDatSo,
    string ToBanDo,
    int TyLeBanDo,
    string DiaChi,
    string DienTich,
    string LoaiDat,
    string ThoiHan,
    string HinhThuc,
    string NguonGoc,
    string GhiChu
)
{
    [JsonPropertyName("maGcn")]
    public long MaGcn { get; init; } = MaGcn;
    [JsonPropertyName("maDvhc")]
    public int MaDvhc { get; init; } = MaDvhc;
    [JsonPropertyName("thuaDatSo")]
    public string ThuaDatSo { get; init; } = ThuaDatSo;
    [JsonPropertyName("toBanDo")]
    public string ToBanDo { get; init; } = ToBanDo;
    [JsonPropertyName("tyLeBanDo")]
    public int TyLeBanDo { get; init; } = TyLeBanDo;
    [JsonPropertyName("diaChi")]
    public string DiaChi { get; init; } = DiaChi;
    [JsonPropertyName("dienTich")]
    public string DienTich { get; init; } = DienTich;
    [JsonPropertyName("loaiDat")]
    public string LoaiDat { get; init; } = LoaiDat;
    [JsonPropertyName("thoiHan")]
    public string ThoiHan { get; init; } = ThoiHan;
    [JsonPropertyName("hinhThuc")]
    public string HinhThuc { get; init; } = HinhThuc;
    [JsonPropertyName("nguonGoc")]
    public string NguonGoc { get; init; } = NguonGoc;
    [JsonPropertyName("ghiChu")]
    public string GhiChu { get; init; } = GhiChu;

    public void Deconstruct(out long MaGcn, out int MaDvhc, out string ThuaDatSo, out string ToBanDo, out int TyLeBanDo, out string DiaChi, out string DienTich, out string LoaiDat, out string ThoiHan, out string HinhThuc, out string NguonGoc, out string GhiChu)
    {
        MaGcn = this.MaGcn;
        MaDvhc = this.MaDvhc;
        ThuaDatSo = this.ThuaDatSo;
        ToBanDo = this.ToBanDo;
        TyLeBanDo = this.TyLeBanDo;
        DiaChi = this.DiaChi;
        DienTich = this.DienTich;
        LoaiDat = this.LoaiDat;
        ThoiHan = this.ThoiHan;
        HinhThuc = this.HinhThuc;
        NguonGoc = this.NguonGoc;
        GhiChu = this.GhiChu;
    }
}