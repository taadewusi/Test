using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Category;
using Test.Models.Category;

namespace ViewModels.Mapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<BookCategory, CategoryViewModel>().ReverseMap();
        }
    }
}
