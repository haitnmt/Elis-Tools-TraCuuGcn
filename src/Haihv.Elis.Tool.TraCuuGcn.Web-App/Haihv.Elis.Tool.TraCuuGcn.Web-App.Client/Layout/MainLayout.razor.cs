using System.IdentityModel.Tokens.Jwt;
using Haihv.Elis.Tool.TraCuuGcn.WebLib;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Haihv.Elis.Tool.TraCuuGcn.Web_App.Client.Layout;

public partial class MainLayout
{
    [Inject]
    protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    [Inject]
    private IDialogService DialogService { get; set; } = null!;
    [Inject]
    private AppSettingsService AppSettingsService { get; set; } = null!;
    [Inject]
    private IAuthService AuthService { get; set; } = null!;
    
    private const string UrlGitHub = "https://github.com/haitnmt/";
    private string _displayName = string.Empty;
    private AuthenticationState? _authen;
    private bool _isDarkMode;
    private MudThemeProvider _mudThemeProvider = null!;
    private bool _isInitialized;
    protected override async Task OnInitializedAsync()
    {
        AuthenticationStateProvider.AuthenticationStateChanged += AuthenticationStateChanged;
        await base.OnInitializedAsync();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var authen = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            AuthenticationStateChanged(Task.FromResult(authen));
            _isDarkMode = await _mudThemeProvider.GetSystemPreference();
            _isInitialized = true;
            StateHasChanged();
        }
    }
    

    private void AuthenticationStateChanged(Task<AuthenticationState> task)
    {
        _authen = task.Result;
        _displayName = _authen.User.FindFirst(JwtRegisteredClaimNames.GivenName)?.Value ?? 
                       _authen.User.FindFirst("HoVaTen")?.Value ?? string.Empty;
        StateHasChanged();
    }

    private async Task OpenDangNhap(MouseEventArgs arg)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true
        };
        await DialogService.ShowAsync<XacThucNguoiDung>(null, options);
        StateHasChanged();
    }

    private async Task Logout(MouseEventArgs arg)
    {
        await AuthService.Logout();
        StateHasChanged();
    }
}