using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Haihv.Elis.Tool.TraCuuGcn.Web_App.Client.Layout;

public partial class MainLayout
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;
    [Inject]
    private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
    [Inject]
    public IUserServices UserServices { get; set; } = null!;
    
    private const string UrlGitHub = "https://github.com/haitnmt/";
    private bool _isDarkMode = true;
    private MudThemeProvider _mudThemeProvider = null!;
    private bool _isInitialized;
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;
        _isDarkMode = await _mudThemeProvider.GetSystemPreference();
        _isInitialized = true;
        StateHasChanged();
    }
}