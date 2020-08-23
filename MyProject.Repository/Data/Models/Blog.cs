using System;

namespace MyProject.Repository.Data.Models
{
    public partial class Blog
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}