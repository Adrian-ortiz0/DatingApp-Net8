﻿namespace DatingApp.DTOs
{
    public class UserDto
    {
        public required String Username { get; set; }
        public required String Token { get; set; }
        
        public string? PhotoUrl { get; set; }
    }
}
