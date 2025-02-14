using Haihv.Elis.Tool.TraCuuGcn.Models;
using LanguageExt.Common;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Data;

public interface ISearchService
{
    Task<Result<GiayChungNhanInfo>> GetResultAsync(string? query);
}