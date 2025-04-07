namespace Haihv.Elis.Tool.TraCuuGcn.Models;

public class ChuSuDungInfo(long maChuSuDung, ChuSuDung chuSuDung, ChuSuDungQuanHe? chuSuDungQuanHe)
{
    public long MaChuSuDung { get; set; } = maChuSuDung;
    public ChuSuDung ChuSuDung { get; set; } = chuSuDung;
    public ChuSuDungQuanHe? ChuSuDungQuanHe { get; set; } = chuSuDungQuanHe;
}