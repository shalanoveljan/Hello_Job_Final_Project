using HelloJob.Data.DAL.Interfaces;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Data.DAL.Implementations
{
    public class RequestDAL : EFRepositoryBase<Request>, IRequestDAL
    {
        private readonly HelloJobDbContext _dbContext;
        public RequestDAL(HelloJobDbContext context) : base(context)
        {
            _dbContext= context;
        }
        public async Task<Request> GetByVacancyIdAndRequestId(int vacancyId, string userid)
        {
            return await _dbContext.Requests.FirstOrDefaultAsync(r => r.VacancyId == vacancyId && r.Resume.AppUserId==userid);
        }
    }
}
