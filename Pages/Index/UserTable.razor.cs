using AdminApp.Helpers;
using AdminApp.Models;
using AdminApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AdminApp.Pages.Index;

public partial class UserTable : ComponentBase
{
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    [Inject]
    private UserService UserService { get; set; }

    [Parameter]
    public List<User> Users { get; set; } = new();

    [Parameter]
    public Dictionary<string, bool> IsSelected { get; set; } = new();

    [Parameter]
    public Dictionary<string, bool> IsLockedOut { get; set; } = new();

    [Parameter]
    public Dictionary<string, DateTime> LastSeen { get; set; } = new();

    [Parameter]
    public EventCallback OnSelectAll { get; set; }

    [Parameter]
    public Action<string> OnSelectUser { get; set; }

    public TimeSpan TimeZoneOffset { get; set; }

    private List<int> GetUserActivity(User user)
    {
        var today = DateTime.UtcNow.Date;

        List<int> activity = new List<int>(new int[7]);

        foreach (var login in user.Logins)
        {
            var daysAgo = (today - login.Time.Date).Days;

            if (daysAgo >= 0 && daysAgo < 7)
            {
                activity[daysAgo]++;
            }
        }

        return activity;
    }

    private string GetRowClass(User user)
    {
        return IsLockedOut[user.Id] == true
            ? "text-secondary text-decoration-line-through"
            : string.Empty;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            int offsetMinutes = await JSRuntime.InvokeAsync<int>("getTimeZoneOffset");
            await JSRuntime.InvokeVoidAsync("initializeTooltips");
            TimeZoneOffset = TimeSpan.FromMinutes(offsetMinutes);

            StateHasChanged();
        }
    }

    private string DateTimeTooltip(User user)
    {
        var localTime = DateTimeHelper.toLocalTime(LastSeen[user.Id], TimeZoneOffset);
        return LastSeen[user.Id] == DateTime.MinValue ? "Never" : localTime.ToString();
    }
}
