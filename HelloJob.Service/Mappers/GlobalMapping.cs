using AutoMapper;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Mappers
{
    public class GlobalMapping:Profile
    {
        public GlobalMapping() 
        {
           
            CreateMap<Blog, BlogGetDto>().ReverseMap();
            CreateMap<Blog, BlogPostDto>().ReverseMap();
            CreateMap<Category, CategoryGetDto>().ReverseMap();
            CreateMap<Category, CategoryPostDto>().ReverseMap();
            CreateMap<Setting, SettingPostDto>().ReverseMap();
            CreateMap<Setting, SettingGetDto>().ReverseMap();
            CreateMap<Course, CourseGetDto>().ReverseMap();
            CreateMap<Course, CoursePostDto>().ReverseMap();
            CreateMap<Tag, TagPostDto>().ReverseMap();
            CreateMap<Tag, TagGetDto>().ReverseMap();
            CreateMap<Language, LanguagePostDto>().ReverseMap();
            CreateMap<Language, LanguageGetDto>().ReverseMap();
            CreateMap<Education, EducationGetDto>().ReverseMap();
            CreateMap<Education, EducationPostDto>().ReverseMap();
            CreateMap<City, CityGetDto>().ReverseMap();
            CreateMap<City, CityPostDto>().ReverseMap();



        }

    }
}
