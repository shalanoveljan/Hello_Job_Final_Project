using HelloJob.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record CategoryGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int? ParentId { get; set; }
        public List<Category> Children { get; set; }

        public CategoryGetDto()
        {
            Children= new List<Category>();
        }




    }
}
