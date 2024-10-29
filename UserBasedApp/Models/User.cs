﻿namespace UserBasedApp.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // Store hashed in production apps
    public string Role { get; set; } = "User"; // Either "User" or "Admin"
}
