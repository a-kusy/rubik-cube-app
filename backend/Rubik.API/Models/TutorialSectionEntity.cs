using System.Text.Json.Serialization;

namespace Rubik.API.Models
{
    public class TutorialSectionEntity : BaseEntity
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public int PageId { get; set; }

        public TutorialSectionEntity(string name, string content, int pageId)
        {
            Name = name;
            Content = content;
            PageId = pageId;
        }

        [JsonConstructor]
        public TutorialSectionEntity(int id, string name, string content, int pageId) : this(name, content, pageId)
        {
            Id = id;
        }
    }
}