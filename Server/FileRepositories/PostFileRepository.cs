using System;
using System.Text.Json;
using Models;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        int maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 0;
        post.Id = maxId + 1;

        posts.Add(post);

        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);

        return post;
    }

    public async Task DeleteAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        Post? postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        posts.Remove(postToRemove);

        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
    }

    public IQueryable<Post> GetMany()
    {
        string postsAsJson = File.ReadAllText(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.AsQueryable();
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        Post? post = posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        return post;
    }

    public async Task<Post> UpdateAsync(Post post)
{
    // 1. Read and Deserialize the entire file content
    string postsAsJson = await File.ReadAllTextAsync(filePath);
    List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

    // 2. Find the post to update using its ID
    Post? postToUpdate = posts.SingleOrDefault(p => p.Id == post.Id);
    if (postToUpdate is null)
    {
        throw new InvalidOperationException(
            $"Post with ID '{post.Id}' not found");
    }

    // 3. FIX: Manually update the properties of the existing object.
    // This is the crucial step to ensure the correct object in the list is modified.
    postToUpdate.Title = post.Title; 
    postToUpdate.Body = post.Body;
    postToUpdate.UserId = post.UserId;
    // Add any other properties here (e.g., postToUpdate.Date = DateTime.Now;)

    // The 'posts' list now contains the modified 'postToUpdate' object.
    
    // 4. Serialize and write the entire list back to the file
    postsAsJson = JsonSerializer.Serialize(posts);
    await File.WriteAllTextAsync(filePath, postsAsJson);

    // 5. Return the updated object (which is the modified postToUpdate)
    return postToUpdate;
}
}
