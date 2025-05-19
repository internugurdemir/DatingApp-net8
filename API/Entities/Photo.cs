using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;


[Table("Photos")]
public class Photo
{        
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }
    // public string Description { get; set; }
    // public DateTime DateAdded { get; set; }


    // Navigation properties
    public int AppUserId { get; set; }
    public virtual AppUser AppUser { get; set; } = null!;
}
