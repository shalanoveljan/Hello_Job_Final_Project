﻿using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HelloJob.Service.Responses
{
	public class PagginatedResponse<T>
	{
        public PagginatedResponse(IList<T> datas, int pageNumber, int pageSize, int totalCount, IList<T> otherdatas = default)
        {
            Datas = datas;
            OtherDatas= otherdatas;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            IsFirstPage = PageNumber == 1;
            IsLastPage = PageNumber == TotalPages;
        }
        public IList<T> Datas { get; set; }
        public IList<T> OtherDatas { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool IsFirstPage { get; set; }
        public bool IsLastPage { get; set; }
    }
}

