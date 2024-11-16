using Microsoft.AspNetCore.Components;

namespace AdminApp.Pages.Index
{
    public partial class Index : ComponentBase
    {
        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        private string _filterQuery = string.Empty;
        private string FilterQuery
        {
            get { return _filterQuery; }
            set
            {
                _filterQuery = value;
                FilterUsers();
            }
        }
        private List<User> users =
            new()
            {
                new User
                {
                    Name = "Clare, Alex",
                    Email = "a_clare42@gmail.com",
                    LastSeen = "5 minutes ago",
                    IsBlocked = false,
                },
                new User
                {
                    Name = "Morrison, Jim",
                    Email = "dmtimer9@dealyaari.com",
                    LastSeen = "less than a minute ago",
                    IsBlocked = false,
                },
                new User
                {
                    Name = "Simone, Nina",
                    Email = "marishabelin@giftcode-ao.com",
                    LastSeen = "3 weeks ago",
                    IsBlocked = true,
                },
                new User
                {
                    Name = "Zappa, Frank",
                    Email = "zappa_f@citybank.com",
                    LastSeen = "less than a minute ago",
                    IsBlocked = false,
                },
                new User
                {
                    Name = "Zappa, Frank",
                    Email = "zappa_f@citybank.com",
                    LastSeen = "less than a minute ago",
                    IsBlocked = false,
                },
                new User
                {
                    Name = "Zappa, Frank",
                    Email = "zappa_f@citybank.com",
                    LastSeen = "less than a minute ago",
                    IsBlocked = false,
                },
                new User
                {
                    Name = "Zappa, Frank",
                    Email = "zappa_f@citybank.com",
                    LastSeen = "less than a minute ago",
                    IsBlocked = false,
                },
                new User
                {
                    Name = "Zappa, Frank",
                    Email = "zappa_f@citybank.com",
                    LastSeen = "less than a minute ago",
                    IsBlocked = false,
                },
                new User
                {
                    Name = "Clare, Alex",
                    Email = "a_clare42@gmail.com",
                    LastSeen = "5 minutes ago",
                    IsBlocked = false,
                },
                new User
                {
                    Name = "Morrison, Jim",
                    Email = "dmtimer9@dealyaari.com",
                    LastSeen = "less than a minute ago",
                    IsBlocked = false,
                },
                new User
                {
                    Name = "Simone, Nina",
                    Email = "marishabelin@giftcode-ao.com",
                    LastSeen = "3 weeks ago",
                    IsBlocked = true,
                },
                new User
                {
                    Name = "Zappa, Frank",
                    Email = "zappa_f@citybank.com",
                    LastSeen = "less than a minute ago",
                    IsBlocked = false,
                },
                new User
                {
                    Name = "Zappa, Frank",
                    Email = "zappa_f@citybank.com",
                    LastSeen = "less than a minute ago",
                    IsBlocked = false,
                },
                new User
                {
                    Name = "Zappa, Frank",
                    Email = "zappa_f@citybank.com",
                    LastSeen = "less than a minute ago",
                    IsBlocked = false,
                },
                new User
                {
                    Name = "Zappa, Frank",
                    Email = "zappa_f@citybank.com",
                    LastSeen = "less than a minute ago",
                    IsBlocked = false,
                },
                new User
                {
                    Name = "Zappa, Frank",
                    Email = "zappa_f@citybank.com",
                    LastSeen = "less than a minute ago",
                    IsBlocked = false,
                },
            };
        private List<User>? filteredUsers;

        protected override void OnInitialized()
        {
            filteredUsers = users;
        }

        private void FilterUsers()
        {
            if (string.IsNullOrEmpty(_filterQuery))
            {
                filteredUsers = users;
            }
            else
            {
                filteredUsers = users
                    .Where(u =>
                        u.Name.Contains(_filterQuery, StringComparison.OrdinalIgnoreCase)
                        || u.Email.Contains(_filterQuery, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
            }
        }

        private void SelectAllUsers()
        {
            foreach (var user in filteredUsers)
            {
                user.IsSelected = !user.IsSelected;
            }
        }

        private void BlockSelectedUsers()
        {
            foreach (var user in filteredUsers.Where(u => u.IsSelected))
            {
                user.IsBlocked = true;
            }
        }

        private void UnblockSelectedUsers()
        {
            foreach (var user in filteredUsers.Where(u => u.IsSelected))
            {
                user.IsBlocked = false;
            }
        }

        private void DeleteSelectedUsers()
        {
            users = users.Where(u => !u.IsSelected).ToList();
            FilterUsers();
        }

        private void Logout()
        {
            // Логика выхода, например, переход на страницу входа
            NavigationManager.NavigateTo("/login");
        }

        private static string GetRowClass(User user)
        {
            return user.IsBlocked ? "text-muted text-decoration-line-through" : string.Empty;
        }

        public class User
        {
            public required string Name { get; set; }
            public required string Email { get; set; }
            public required string LastSeen { get; set; }
            public bool IsBlocked { get; set; }
            public bool IsSelected { get; set; }
        }
    }
}
