namespace Models;
public class User
{

    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }

    public User(int Id, string? username, string? password)
    {
        this.Id = Id;
        this.Username = username;
        this.Password = password;
    }

}