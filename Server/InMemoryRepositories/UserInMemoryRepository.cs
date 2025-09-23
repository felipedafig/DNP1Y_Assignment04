using Models;
using RepositoryContracts;


namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private List<User> Users;
    public UserInMemoryRepository()
    {
        Users = new List<User>();
    }
    public Task<User> AddAsync(User user)
    {
        user.Id = Users.Any()
        ? Users.Max(p => p.Id) + 1
        : 1;
        Users.Add(user);
        return Task.FromResult(user);
    }

    public Task DeleteAsync(int id)
    {
        User? UserToRemove = Users.SingleOrDefault(p => p.Id == id);
        if (UserToRemove is null)
        {
            throw new InvalidOperationException(
                            $"User with ID '{id}' not found");
        }

        Users.Remove(UserToRemove);
        return Task.CompletedTask;
    }


    public IQueryable<User> GetMany()
    {
        return Users.AsQueryable();
    }


    public Task<User> GetSingleAsync(int id)
    {
        User? User = Users.SingleOrDefault(p => p.Id == id);
        if (User is null)
        {
            throw new InvalidOperationException(
                            $"User with ID '{id}' not found");
        }
        return Task.FromResult(User);
    }


    public Task UpdateAsync(User User)
    {
        User? existingUser = Users.SingleOrDefault(p => p.Id == User.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                    $"User with ID '{User.Id}' not found");
        }

        Users.Remove(existingUser);
        Users.Add(User);

        return Task.CompletedTask;
    }


}