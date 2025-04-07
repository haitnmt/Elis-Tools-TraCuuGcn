using Haihv.Elis.Tool.TraCuuGcn.Models;
using LanguageExt.Common;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Services;

public interface ITaiSanService
{
    Task<List<TaiSan>> GetTaiSanInDataBaseAsync(string? serial, List<long> dsMaThuaDat, List<long> dsMaChuSuDung);
    Task<Result<List<TaiSan>>> GetTaiSanAsync(string? serial, List<long> dsMaThuaDat, List<long> dsMaChuSuDung);
    Task SetCacheAsync(string? serial, IThuaDatService thuaDatService, IChuSuDungService chuSuDungService);
}