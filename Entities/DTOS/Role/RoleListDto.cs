using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record RoleListDto
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
