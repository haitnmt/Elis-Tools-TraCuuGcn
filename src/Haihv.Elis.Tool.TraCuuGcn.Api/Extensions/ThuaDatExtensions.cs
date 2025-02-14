using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Extensions;

public static class ThuaDatExtensions
{
    public static ThuaDatPublic ConvertToThuaDatPublic(this ThuaDat thuaDat)
    {
        return new ThuaDatPublic(
            thuaDat.MaDvhc,
            thuaDat.ThuaDatSo,
            thuaDat.ToBanDo,
            thuaDat.DiaChi,
            thuaDat.DienTich,
            thuaDat.GhiChu
        );
    }
}