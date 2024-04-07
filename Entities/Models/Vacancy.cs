using Entities.Common;
using HelloJob.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HelloJob.Entities.Models
{
    public class Vacancy:BaseEntity
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int Salary { get; set; }
        public string Position { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public Order order { get; set; }
        public int Required_Experience { get; set; }
        public int ViewCount { get; set; }
        public DateTime EndDate { get; set; }
        public JobMode Mode { get; set; }
        public bool IsPremium { get; set; }
        public List<About_Vacancy> abouts { get; set; }
        public List<WishlistItem> WishListItems { get; set; }
        public List<Request> Requests { get; set; }


        public Vacancy()
        {
            Requests = new List<Request>();
            abouts= new List<About_Vacancy>();
            WishListItems= new List<WishlistItem>();
        }



    }
}
