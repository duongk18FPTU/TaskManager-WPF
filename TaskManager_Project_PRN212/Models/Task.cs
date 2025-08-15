using System;
using System.Collections.Generic;

namespace TaskManager_Project_PRN212.Models;

public partial class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; }
    public int UserId { get; set; }
    public int? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    public virtual User User { get; set; }

}

