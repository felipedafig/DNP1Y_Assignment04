using System.Formats.Asn1;
using Models;
using RepositoryContracts;


namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private List<Post> Posts;
    public PostInMemoryRepository()
    {
        Posts = new List<Post>();
    }
    public Task<Post> AddAsync(Post post)
    {
        post.Id = Posts.Any()
        ? Posts.Max(p => p.Id) + 1
        : 1;
        Posts.Add(post);
        return Task.FromResult(post);
    }


    public Task DeleteAsync(int id)
    {
        Post? postToRemove = Posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                            $"Post with ID '{id}' not found");
        }

        Posts.Remove(postToRemove);
        return Task.CompletedTask;
    }


    public IQueryable<Post> GetMany()
    {
        return Posts.AsQueryable();
    }


    public Task<Post> GetSingleAsync(int id)
    {
        Post? post = Posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException(
                            $"Post with ID '{id}' not found");
        }
        return Task.FromResult(post);
    }


    public Task UpdateAsync(Post post)
    {
        Post? existingPost = Posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null)
        {
            throw new InvalidOperationException(
                    $"Post with ID '{post.Id}' not found");
        }

        Posts.Remove(existingPost);
        Posts.Add(post);

        return Task.CompletedTask;
    }

}