namespace InternshipAPI.Manager
{
    public class StatusManager
    {
        private readonly StatusContext _context;
        public StatusManager(StatusContext context)
        {
            _context = context;
        }
        public StatusManager()
        {

        }
    }
}
