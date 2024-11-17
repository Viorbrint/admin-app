using AdminApp.Models;
using AdminApp.Repositories;

namespace AdminApp.Services;

public class UserService(UserRepository userRepository)
{
    private readonly UserRepository _userRepository = userRepository;

    public async Task<List<User>> GetAll()
    {
        return await _userRepository.GetAll();
    }

    public async Task<User?> Get(int userId)
    {
        return await _userRepository.Get(userId);
    }

    public async Task<User> Create(User user)
    {
        await _userRepository.Create(user);
        return user;
    }

    public async Task<bool> Delete(int userId)
    {
        var user = await _userRepository.Get(userId);
        if (user == null)
        {
            return false;
        }

        await _userRepository.Delete(userId);
        return true;
    }

    public async Task<bool> Block(int userId)
    {
        var user = await _userRepository.Get(userId);
        if (user == null)
        {
            return false;
        }

        await _userRepository.Block(user);
        return true;
    }

    public async Task<bool> Unblock(int userId)
    {
        var user = await _userRepository.Get(userId);
        if (user == null)
        {
            return false;
        }

        await _userRepository.Unblock(user);
        return true;
    }
}
