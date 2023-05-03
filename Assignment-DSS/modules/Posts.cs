using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using Assignment_DSS.Interfaces;
using Assignment_DSS.Repository;



namespace Assignment_DSS.modules
{
    
    public class Posts
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Img { get; set; }
        public string? Body { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
        public string? Username { get; set; }
    }
}
