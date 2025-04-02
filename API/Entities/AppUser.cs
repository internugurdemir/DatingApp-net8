using System;

namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public required string Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public required string KnownAs { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; }= DateTime.UtcNow;
    public string? Introduction { get; set; }
    public string? LookingFor { get; set; }
    public string? Interests { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    // public List<Photo> Photos { get; set; }
    // public virtual ICollection<Photo> Photos { get; set; }
    // public virtual ICollection<Like> Likers { get; set; }
    // public virtual ICollection<Like> Likees { get; set; }
    // public virtual ICollection<Message> MessagesSent { get; set; }
    // public virtual ICollection<Message> MessagesRecieved { get; set; }

    // public int GetAge()
    // {
    //     return DateOfBirth.CalculateAge();
    // }
}
