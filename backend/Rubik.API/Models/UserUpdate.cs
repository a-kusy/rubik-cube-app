namespace Rubik.API.Models
{
    public class UserUpdate
    {
        public string? Username { get; set; }

        public string? Password { get; set; }
        
        public bool? IsArchival { get; set; }
        
        public string? Email { get; set; }
    }
}