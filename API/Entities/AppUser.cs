using System;

namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required byte[] PasswordHash { get; set; }
    public required byte[] PasswordSalt { get; set; }
    // public string Gender { get; set; }
    // public DateTime DateOfBirth { get; set; }
    // public string KnownAs { get; set; }
    // public DateTime Created { get; set; }
    // public DateTime LastActive { get; set; }
    // public string Introduction { get; set; }
    // public string LookingFor { get; set; }
    // public string Interests { get; set; }
    // public string City { get; set; }
    // public string Country { get; set; }
    // public virtual ICollection<Photo> Photos { get; set; }
    // public virtual ICollection<Like> Likers { get; set; }
    // public virtual ICollection<Like> Likees { get; set; }
    // public virtual ICollection<Message> MessagesSent { get; set; }
    // public virtual ICollection<Message> MessagesRecieved { get; set; }
}
