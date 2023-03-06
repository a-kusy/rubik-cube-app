using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rubik.API.Models
{
    public class RankingEntity : BaseEntity
    {
        [Required]
        public int Position { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ScoreId { get; set; }

        public RankingEntity(int position, int userId, int scoreId)
        {
            Position = position;
            UserId = userId;
            ScoreId = scoreId;
        }

        [JsonConstructor]
        public RankingEntity(int id, int position, int userId, int scoreId) : this(position, userId, scoreId)
        {
            Id = id;
        }
    }
}