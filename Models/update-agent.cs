﻿namespace EstateFInder.Models
{
    public class Update_agent
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public string? Address { get; set; }
    }
}
