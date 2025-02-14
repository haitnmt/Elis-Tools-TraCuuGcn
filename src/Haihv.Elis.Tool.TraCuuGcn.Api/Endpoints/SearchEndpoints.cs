using Haihv.Elis.Tool.TraCuuGcn.Api.Data;
using Haihv.Elis.Tool.TraCuuGcn.Models;
using Microsoft.AspNetCore.Mvc;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Endpoints;

public static class SearchEndpoints
{
    public static void MapSearchEndpoints(this WebApplication app)
    {
        app.MapGet("/elis/search", GetSearchResultAsync)
            .WithName("GetGcnInfoAsync");
    }

    private static async Task<IResult> GetSearchResultAsync([FromQuery] string? query,
        ISearchService searchService)
    {
        var result = await searchService.GetResultAsync(query);
        return await Task.FromResult(result.Match(
            info=> Results.Ok(new Response<GiayChungNhanInfo>(info)),
            ex => Results.BadRequest(new Response<GiayChungNhanInfo>(ex.Message))));
    }
    
}