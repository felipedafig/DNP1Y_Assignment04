namespace Models;
public class Comment
{

    public int Id { get; set; }
    public string? Body { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }


    public Comment(int Id, string Body, int UserId, int PostId)
    {
        this.Id = Id;
        this.Body = Body;
        this.UserId = UserId;
        this.PostId = PostId;
    }
}