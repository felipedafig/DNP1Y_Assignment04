using Models;
using RepositoryContracts;


namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private List<Comment> Comments;
    public CommentInMemoryRepository()
    {
        Comments = new List<Comment>();
    }
    public Task<Comment> AddAsync(Comment Comment)
    {
        Comment.Id = Comments.Any()
        ? Comments.Max(p => p.Id) + 1
        : 1;
        Comments.Add(Comment);
        return Task.FromResult(Comment);
    }


    public Task DeleteAsync(int id)
    {
        Comment? CommentToRemove = Comments.SingleOrDefault(p => p.Id == id);
        if (CommentToRemove is null)
        {
            throw new InvalidOperationException(
                            $"Comment with ID '{id}' not found");
        }

        Comments.Remove(CommentToRemove);
        return Task.CompletedTask;
    }


    public IQueryable<Comment> GetMany()
    {
        return Comments.AsQueryable();
    }


    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? Comment = Comments.SingleOrDefault(p => p.Id == id);
        if (Comment is null)
        {
            throw new InvalidOperationException(
                            $"Comment with ID '{id}' not found");
        }
        return Task.FromResult(Comment);
    }


    public Task UpdateAsync(Comment Comment)
    {
        Comment? existingComment = Comments.SingleOrDefault(p => p.Id == Comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                    $"Comment with ID '{Comment.Id}' not found");
        }

        Comments.Remove(existingComment);
        Comments.Add(Comment);

        return Task.CompletedTask;
    }


}