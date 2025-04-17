﻿using System.Net.Http.Json;
using Haihv.Elis.Tool.TraCuuGcn.Models;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

internal sealed class ClientDataServices(HttpClient httpClient) : IDataServices
{
    // Constants
    private const string UrlSearch = "/elis/search";
    private const string UrlThuaDat = "/elis/thua-dat-public";
    private const string UrlClearCache = "/elis/delete-cache";
    private const string UrlGetPermissionsCanUpdate = "elis/permissions/can-update";
    private const string UrlGetPermissionsCanDelete = "elis/permissions/can-delete";
    private const string UrlGetUserInfo = "authentication/user/info";
    public async Task<GiayChungNhanInfo?> GetGiayChungNhanInfoAsync(string query)
    {
        var url = $"{UrlSearch}?query={query}";
        return await httpClient.GetFromJsonAsync<GiayChungNhanInfo>(url);
    }

    public async Task<UserInfo?> GetUserInfoAsync()
    {
        return await httpClient.GetFromJsonAsync<UserInfo>(UrlGetUserInfo);
    }
}