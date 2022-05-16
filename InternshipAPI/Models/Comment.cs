using System;

namespace InternshipAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int PersonId { get; set; }
        public string CommentString { get; set; }
        public DateTime Date { get; set; }
    }
}
