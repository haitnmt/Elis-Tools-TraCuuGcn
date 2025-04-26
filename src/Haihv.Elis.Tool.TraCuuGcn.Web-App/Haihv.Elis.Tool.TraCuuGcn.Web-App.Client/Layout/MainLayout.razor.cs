using Haihv.Elis.Tool.TraCuuGcn.Models;
using Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace Haihv.Elis.Tool.TraCuuGcn.Web_App.Client.Layout;

public partial class MainLayout
{
    [Inject]
    private IDialogService DialogService { get; set; } = null!;
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;
    [Inject]
    private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
    [Inject]
    public IUserServices UserServices { get; set; } = null!;
    
    private const string UrlGitHub = "https://github.com/haitnmt/";
    private bool _isDarkMode;
    private MudThemeProvider _mudThemeProvider = null!;
    private bool _isInitialized;
    private bool _toggleInfoMenu;
    private string? _currentUrl;
    private UserInfo? _userInfo;
    protected override async Task OnInitializedAsync()
    {
        _currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
        await base.OnInitializedAsync();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;
        
        try
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity?.IsAuthenticated == true)
            {
                _userInfo = await UserServices.GetUserInfoAsync();
            }
            _isDarkMode = await _mudThemeProvider.GetSystemPreference();
            _isInitialized = true;
            StateHasChanged();
        }
        catch (Exception)
        {
            _userInfo = null;
        }
    }
    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        _currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }
    
}