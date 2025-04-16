using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

public interface IDataServices
{
    Task<GiayChungNhanInfo?> GetGiayChungNhanInfoAsync(string query);
}