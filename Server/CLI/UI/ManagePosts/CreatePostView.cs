


using Models;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreatePostView(IPostRepository postRepository, IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task DisplayPostCreation()
    {
        Console.WriteLine("Create new post");
        Console.WriteLine("Title:");
        string? title = Console.ReadLine();
        Console.WriteLine("Body:");
        string? body = Console.ReadLine();

        int userId;

Console.WriteLine("User Id: ");

while (true)
{
    string? input = Console.ReadLine();

    if (!int.TryParse(input, out userId))
    {
        Console.WriteLine("Must be an integer, try again:");
        continue;
    }

    try
    {
        await userRepository.GetSingleAsync(userId);
        break;
    }
    catch
    {
        Console.WriteLine($"User with id: {userId} does not exist. Try again:");
    }
}

        Post added = await postRepository.AddAsync(new Post(0, title, body, 0));

        Console.WriteLine("\n Post created successfully!");
        Console.WriteLine("------------------------------");
        Console.WriteLine($"ID:       {added.Id}");
        Console.WriteLine($"User ID:  {added.UserId}");
        Console.WriteLine($"Title:    {added.Title}");
        Console.WriteLine($"Body:     {added.Body}");
        Console.WriteLine("------------------------------\n");


    }
}