using AdminApp.Services;
using Microsoft.AspNetCore.Components;

namespace AdminApp.Pages.Index;

public partial class Index : ComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private UserService UserService { get; set; } = null!;

    [Inject]
    private UserState UserState { get; set; } = null!;

    private string _currentUserEmail = string.Empty;

    private async void SetFilter(string query)
    {
        Console.WriteLine(query);
        UserState.FilterQuery = query;
        await UpdateUsers();
    }

    private async Task UpdateUsers()
    {
        var newUsers = await UserService.SearchUsersAsync(UserState.FilterQuery);
        UserState.SetUsers(newUsers);
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        await CheckUserStatus();
        await UpdateUsers();
        await base.OnInitializedAsync();

        var currentUser = await UserService.GetCurrentUser();

        if (currentUser is not null && !string.IsNullOrEmpty(currentUser.Email))
        {
            _currentUserEmail = currentUser.Email;
        }
        else
        {
            _currentUserEmail = "Guest";
        }
    }

    private async Task CheckUserStatus()
    {
        var user = await UserService.GetCurrentUser();
        if (user == null || await UserService.IsLockedOut(user))
        {
            Logout();
        }
    }

    private async Task DoOperation(Func<string, Task<bool>> operation, string userId)
    {
        await CheckUserStatus();
        if (await operation(userId))
        {
            ShowNotification(true);
        }
        else
        {
            ShowNotification(false);
        }
    }

    private async Task BlockSelectedUsers()
    {
        foreach (var user in UserState.Users.Where(u => UserState.IsSelected[u.Id]))
        {
            await DoOperation(UserService.Block, user.Id);
        }
        await UpdateUsers();
    }

    private async Task UnblockSelectedUsers()
    {
        foreach (var user in UserState.Users.Where(u => UserState.IsSelected[u.Id]))
        {
            await DoOperation(UserService.Unblock, user.Id);
        }
        await UpdateUsers();
    }

    private async Task DeleteSelectedUsers()
    {
        foreach (var user in UserState.Users.Where(u => UserState.IsSelected[u.Id]))
        {
            await DoOperation(UserService.Delete, user.Id);
        }
        await UpdateUsers();
    }

    private void Logout()
    {
        NavigationManager.NavigateTo("/logout", true);
    }

    private bool IsSuccess = true;

    private bool IsNotificationShow = false;

    public void ShowNotification(bool isSuccess = true)
    {
        IsSuccess = isSuccess;
        IsNotificationShow = true;
        StateHasChanged();
        _ = AutoHideNotificationAsync();
    }

    private async Task AutoHideNotificationAsync()
    {
        await Task.Delay(2000);
        IsNotificationShow = false;
        StateHasChanged();
    }
}
