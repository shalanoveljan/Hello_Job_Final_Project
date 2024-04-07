using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record SettingPostDto
    {

        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}
