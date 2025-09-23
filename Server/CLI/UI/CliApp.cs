using System.Threading.Tasks;
using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using Models;
using RepositoryContracts;

public class CLIApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;
    private CreateUsersView createUsersView;
    private CreatePostView createPostView;
    private CreateCommentView createCommentView;
    private DisplayPostsView displayPostView;

    public CLIApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        createUsersView = new(userRepository);
        createPostView = new(postRepository, userRepository);
        createCommentView = new(commentRepository, postRepository, userRepository);
        displayPostView = new(postRepository, commentRepository);
        GenerateDummyData();
    }
    public async Task StartAsync()
    {
        int choice = -1;
        while (choice != 0)
        {
            Console.WriteLine("Menu");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Create Post");
            Console.WriteLine("3. Create Comment");
            Console.WriteLine("4. Show Posts Overview");
            Console.WriteLine("5. Show Post Info");
            Console.WriteLine("Select number:");
            choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    await createUsersView.DisplayUserCreation();
                    break;
                case 2:
                    await createPostView.DisplayPostCreation();
                    break;
                case 3:
                    await createCommentView.DisplayCommentCreation();
                    break;
                case 4:
                    displayPostView.DisplayPostsOverview();
                    break;
                case 5:
                    await displayPostView.DisplaySinglePost();
                    break;
                case 0:
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
    public async Task GenerateDummyData()
    {
        List<User> users = new List<User>();
        for (int i = 1; i <= 5; i++)
        {
           
            User user = new User(0, $"DummyUser{i}", $"Password{i}");
            User addedUser = await userRepository.AddAsync(user);
            users.Add(addedUser);
        }

        List<Post> posts = new List<Post>();
        for (int i = 1; i <= 5; i++)
        {
           
            Post post = new Post(0, $"Dummy Post {i}", $"This is the body of dummy post {i}.",
                users[(i - 1) % users.Count].Id);
            Post addedPost = await postRepository.AddAsync(post);
            posts.Add(addedPost);
        }

        for (int i = 1; i <= 5; i++)
        {
            Comment comment = new Comment(0, $"This is dummy comment {i}.",
                users[(i - 1) % users.Count].Id,
                posts[(i - 1) % posts.Count].Id);
            await commentRepository.AddAsync(comment);
        }

    }
}