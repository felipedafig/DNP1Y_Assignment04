
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Models;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUsersView
{

    private readonly IUserRepository userRepository;

    public CreateUsersView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task DisplayUserCreation()
    {
        Console.WriteLine("Register new user");
        Console.WriteLine("Username:");
        string? username = Console.ReadLine();
        Console.WriteLine("Password:");
        string? password = Console.ReadLine();

        User added = await userRepository.AddAsync(new User(0, username, password));

        Console.WriteLine($"User {added.Username} created with Id {added.Id}");

    }

}