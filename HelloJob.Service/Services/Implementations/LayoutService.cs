using HelloJob.Data.DAL.Interfaces;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public class LayoutService : ILayoutService
    {
        readonly ILayoutDAL _layoutDAL;

        public LayoutService(ILayoutDAL layoutDAL)
        {
            _layoutDAL = layoutDAL;
        }

        public async Task< Dictionary<string, string>> GetSettings()
        {
            Dictionary<string, string> settings = await _layoutDAL.GetQuery(x=>!x.IsDeleted).ToDictionaryAsync(s => s.Key, s => s.Value);
            return settings;
        }
    }
}
