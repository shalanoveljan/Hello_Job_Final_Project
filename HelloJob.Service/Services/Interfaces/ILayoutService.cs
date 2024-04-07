using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Interfaces
{
    public interface ILayoutService
    {
         public Task<Dictionary<string, string>> GetSettings();
    }
}
