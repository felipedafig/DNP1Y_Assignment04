    

namespace Models;

public class Post
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public int UserId { get; set; }

     public Post(int Id, string? Title, string? Body, int UserId)
        {
            this.Id = Id;
            this.Title = Title;
            this.Body = Body;
            this.UserId = UserId;

        }
}
