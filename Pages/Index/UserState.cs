using AdminApp.Models;

namespace AdminApp.Pages.Index;

public class UserState
{
    public List<User> Users { get; private set; } = new();
    public Dictionary<string, bool> IsLockedOut { get; private set; } = new();
    public Dictionary<string, bool> IsSelected { get; private set; } = new();
    public Dictionary<string, DateTime> LastSeen { get; private set; } = new();

    private bool _flagState = false;

    public string FilterQuery { get; set; } = string.Empty;

    public void SetUsers(List<UserWithStatus> users)
    {
        Users = users.Select(u => u.User).ToList();
        IsLockedOut = users.ToDictionary(u => u.User.Id, u => u.IsLockedOut);
        IsSelected = Users.ToDictionary(u => u.Id, _ => false);
        LastSeen = Users.ToDictionary(
            u => u.Id,
            u => u.Logins?.FirstOrDefault()?.Time ?? DateTime.MinValue
        );
    }

    public void ToggleUserSelection(string userid)
    {
        IsSelected[userid] = !IsSelected[userid];
    }

    public void ToggleAllSelections()
    {
        foreach (var user in Users)
        {
            IsSelected[user.Id] = !_flagState;
        }
        _flagState = !_flagState;
    }
}
