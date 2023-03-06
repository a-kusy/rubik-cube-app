using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rubik.API.Models
{
    public class UserEntity : BaseEntity
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [JsonIgnore]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        public List<ScoreEntity> Scores { get; set; }

        [JsonConstructor]
        public UserEntity(int id, string username, string password, string email)
        {
            Id = id;
            Username = username;
            Password = password;
            Email = email;
            Scores = new List<ScoreEntity>();
        }

        public UserEntity(string username, string password, string email, IEnumerable<ScoreEntity>? scores = null)
        {
            Username = username;
            Password = password;
            Email = email;
            Scores = scores?.ToList() ?? new List<ScoreEntity>();
        }

        public UserEntity(int id, string username, string password, string email,
            IEnumerable<ScoreEntity>? scores = null) : this(username, password, email, scores)
        {
            Id = id;
        }
    }
}