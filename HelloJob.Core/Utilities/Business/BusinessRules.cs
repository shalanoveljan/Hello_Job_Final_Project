using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Core.Utilities.Business
{
    public static class BusinessRules
    {
        public static IResult Check(params IResult[] results)
        {
            foreach (var result in results)
            {
                if (!result.Success)
                {
                    return new ErrorResult();
                }
            }
            return new SuccessResult();
        }
    }
}
