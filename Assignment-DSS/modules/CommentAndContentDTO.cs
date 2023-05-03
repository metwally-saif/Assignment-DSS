namespace Assignment_DSS.modules
{
    public class CommentAndContentDTO
    {
        public Posts blog { get; set; }
        public IEnumerable<Comment> AllComments { get; set; }
    }
}
