using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rubik.API.Models
{
    public class TutorialPageEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public List<TutorialSectionEntity> Sections { get; set; }

        [JsonConstructor]
        public TutorialPageEntity(int id, string name, List<TutorialSectionEntity> sections)
        {
            Id = id;
            Name = name;
            Sections = sections;
        }

        public TutorialPageEntity(string name, IEnumerable<TutorialSectionEntity>? sections = null)
        {
            Name = name;
            Sections = sections?.ToList() ?? new List<TutorialSectionEntity>();
        }

        public TutorialPageEntity(int id, string name)
        {
            Id = id;
            Name = name;
            Sections = new List<TutorialSectionEntity>();
        }

        public TutorialPageEntity(int id, string name, List<TutorialSectionEntity> sections1,
            IEnumerable<TutorialSectionEntity>? sections = null) : this(name, sections)
        {
            Sections = sections1;
            Id = id;
        }
    }
}