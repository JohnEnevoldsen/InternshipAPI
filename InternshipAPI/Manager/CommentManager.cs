namespace InternshipAPI.Manager
{
    public class CommentManager
    {
        private readonly CommentContext _context;
        public CommentManager(CommentContext context)
        {
            _context = context;
        }
        public CommentManager()
        {

        }
    }
}
