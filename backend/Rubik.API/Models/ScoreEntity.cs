using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rubik.API.Models
{
    public class ScoreEntity : BaseEntity
    {
        [Required]
        public int Time { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public ScoreEntity(int time, DateTime date, int userId)
        {
            Time = time;
            Date = date;
            UserId = userId;
        }

        [JsonConstructor]
        public ScoreEntity(int id, int time, DateTime date, int userId) : this(time, date, userId)
        {
            Id = id;
        }
    }
}