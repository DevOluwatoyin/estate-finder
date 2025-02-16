namespace EstateFInder.Models.Entities
{
    public class Agent
    {
        public Guid Id { get; set; }
        public  required  string Name { get; set; }
        public  required string Email { get; set; }
        public  required string Phone { get; set; }
        public string? Address { get; set; }

    }
}
