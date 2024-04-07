using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public int? ParentId { get; set; } = null;
        public Category? Parent { get; set; }
        public string Storage { get; set; }
        public List<Category> Children { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Resume> Resumes { get; set; }
        public Category()
        {
            Children=new List<Category>();
        }
    }
}
