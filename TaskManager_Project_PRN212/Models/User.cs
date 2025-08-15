using System;
using System.Collections.Generic;

namespace TaskManager_Project_PRN212.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Role { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
