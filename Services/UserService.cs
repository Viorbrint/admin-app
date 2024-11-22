using AdminApp.Models;
using AdminApp.Repositories;

namespace AdminApp.Services;

public class UserService(UserRepository userRepository)
{
    public async Task<List<User>> GetAll()
    {
        return await userRepository.GetAll();
    }

    public async Task<User?> Get(string userId)
    {
        return await userRepository.Get(userId);
    }

    public async Task<User> Create(User user)
    {
        await userRepository.Create(user);
        return user;
    }

    public async Task<bool> Delete(string userId)
    {
        var user = await userRepository.Get(userId);
        if (user == null)
        {
            return false;
        }

        await userRepository.Delete(userId);
        return true;
    }

    public async Task<bool> Block(string userId)
    {
        var user = await userRepository.Get(userId);
        if (user == null)
        {
            return false;
        }

        await userRepository.Block(user);
        return true;
    }

    public async Task<bool> Unblock(string userId)
    {
        var user = await userRepository.Get(userId);
        if (user == null)
        {
            return false;
        }

        await userRepository.Unblock(user);
        return true;
    }

    public async Task<bool> AddLogin(string userId)
    {
        var user = await userRepository.Get(userId);
        if (user == null)
        {
            return false;
        }
        await userRepository.AddLogin(user);
        return true;
    }
}
