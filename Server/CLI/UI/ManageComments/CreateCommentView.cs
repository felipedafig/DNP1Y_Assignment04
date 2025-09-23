


using RepositoryContracts;
using Models;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    public readonly ICommentRepository commentRepository;
    public readonly IPostRepository postRepository;
    public readonly IUserRepository userRepository;

    public CreateCommentView(ICommentRepository commentRepository, IPostRepository postRepository, IUserRepository userRepository)
    {
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task DisplayCommentCreation()
    {

        Console.WriteLine("Add new comment");
        Console.WriteLine("Body:");
        string? body = Console.ReadLine();

        int userId;

        Console.WriteLine("UserId:");

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
                Console.WriteLine("This user id do not exist. Try again:");
            }
        }


        int postId;

        Console.WriteLine("PostId:");

        while (true)
        {
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out postId))
            {
                Console.WriteLine("Must be an integer, try again:");
                continue;
            }

            try
            {
                await postRepository.GetSingleAsync(postId);
                break;
            }

            catch
            {
                Console.WriteLine($"Post with id: {postId} does not exist. Try again:");
            }
        }

        Comment added = await commentRepository.AddAsync(new Comment(0,body,userId,postId));
        Console.WriteLine($"Comment with Body: {added.Body} by user: {added.UserId} on the post: {added.PostId}");

    }
}