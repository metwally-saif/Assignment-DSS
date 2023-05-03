using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_DSS.modules
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string? body { get; set; }
        [ForeignKey("PostId")]
        public int PostId { get; set; }
        [ForeignKey("userId")]
        public int UserId { get; set; }
        [ForeignKey("Name")]
        public string UserName { get; set; }

    }
}
