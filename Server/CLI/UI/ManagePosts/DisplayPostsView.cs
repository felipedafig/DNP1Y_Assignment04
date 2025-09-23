
using Models;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class DisplayPostsView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public DisplayPostsView(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public void DisplayPostsOverview()
    {

        List<Post> posts = postRepository.GetMany().ToList();

        foreach (Post post in posts)
        {
            Console.WriteLine($"--> Title: {post.Title}, Id: {post.Id}");
        }

    }

    public async Task DisplaySinglePost()
    {
        Post post;
        int postId = 0;
        Console.WriteLine("Post Id:");

        string? input = Console.ReadLine();

        while (true)
        {
            if (!int.TryParse(input, out postId))
            {
                Console.WriteLine("Must be an int. Try again:");
                continue;
            }

            try
            {
                post = await postRepository.GetSingleAsync(postId);
                break;

            }
            catch
            {
                Console.WriteLine($"Post with id: {postId} does not exist. Try again:");
            }
        }

        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");
        Console.WriteLine("Comments:");
        List<Comment> comments = commentRepository.GetMany().ToList();

        foreach (Comment comment in comments)
        {
            if (comment.PostId == postId)
            {
                Console.WriteLine($"{comment.Body} by user with Id: {comment.UserId}");
            }
        }

    }

}