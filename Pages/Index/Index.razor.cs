using AdminApp.Models;
using AdminApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AdminApp.Pages.Index;

public partial class Index : ComponentBase
{
    [Inject]
    private NavigationManager? NavigationManager { get; set; }

    [Inject]
    private UserService UserService { get; set; } = null!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    private string _filterQuery = string.Empty;

    private List<User> users = [];
    private List<User> _filteredUsers = [];
    private Dictionary<string, bool> _isSelected = [];
    private Dictionary<string, DateTime> _lastSeen = [];

    private string FilterQuery
    {
        get { return _filterQuery; }
        set
        {
            _filterQuery = value;
            FilterUsers();
        }
    }

    private TimeSpan _timeZoneOffset;

    private DateTime GetLocalTime(DateTime UTCTime)
    {
        return UTCTime.Add(_timeZoneOffset);
    }

    protected override async Task OnInitializedAsync()
    {
        users = await UserService.GetAll();
        _filteredUsers = users;

        foreach (var user in users)
        {
            // TODO: bad
            var lastLoginTime = user.Logins?.FirstOrDefault()?.Time ?? DateTime.MinValue;
            _lastSeen.Add(user.Email, lastLoginTime);
            _isSelected.Add(user.Email, false);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            int offsetMinutes = await JSRuntime.InvokeAsync<int>("getTimeZoneOffset");
            _timeZoneOffset = TimeSpan.FromMinutes(offsetMinutes);

            StateHasChanged();
        }
    }

    private async Task BlockUser(string userId)
    {
        await UserService.Block(userId);
        await LoadUsers();
    }

    private async Task UnblockUser(string userId)
    {
        await UserService.Unblock(userId);
        await LoadUsers();
    }

    private async Task DeleteUser(string userId)
    {
        await UserService.Delete(userId);
        await LoadUsers();
    }

    private async Task LoadUsers()
    {
        users = await UserService.GetAll();
    }

    private void FilterUsers()
    {
        if (string.IsNullOrEmpty(_filterQuery))
        {
            _filteredUsers = users;
        }
        else
        {
            _filteredUsers = users
                ?.Where(u =>
                    u.UserName.Contains(_filterQuery, StringComparison.OrdinalIgnoreCase)
                    || u.Email.Contains(_filterQuery, StringComparison.OrdinalIgnoreCase)
                )
                .ToList();
        }
    }

    private void SelectUser(string email)
    {
        _isSelected[email] = !_isSelected[email];
    }

    private void SelectAllUsers()
    {
        foreach (var user in _filteredUsers)
        {
            _isSelected[user.Email] =
                !_isSelected.ContainsKey(user.Email) || !_isSelected[user.Email];
        }
    }

    private async Task BlockSelectedUsers()
    {
        foreach (
            var user in _filteredUsers.Where(u =>
                _isSelected.ContainsKey(u.Email) && _isSelected[u.Email]
            )
        )
        {
            await BlockUser(user.Id);
        }
    }

    private async Task UnblockSelectedUsers()
    {
        foreach (
            var user in _filteredUsers.Where(u =>
                _isSelected.ContainsKey(u.Email) && _isSelected[u.Email]
            )
        )
        {
            await UnblockUser(user.Id);
        }
    }

    private async Task DeleteSelectedUsers()
    {
        foreach (
            var user in _filteredUsers.Where(u =>
                _isSelected.ContainsKey(u.Email) && _isSelected[u.Email]
            )
        )
        {
            await DeleteUser(user.Id);
        }
    }

    private void Logout()
    {
        NavigationManager.NavigateTo("/logout", true);
    }

    private static string GetRowClass(User user)
    {
        return user.IsBlocked ? "text-muted text-decoration-line-through" : string.Empty;
    }
}
