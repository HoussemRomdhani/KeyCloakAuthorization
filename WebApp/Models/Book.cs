﻿namespace WebApp.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public string Author { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Year { get; set; }
}

