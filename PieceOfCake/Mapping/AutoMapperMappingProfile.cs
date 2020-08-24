using AutoMapper;
using PieceOfCake.Core.Entities;
using PieceOfCake.Shared.ViewModels.Dish;
using PieceOfCake.Shared.ViewModels.Dish.Ingredient;
using PieceOfCake.Shared.ViewModels.MeasureUnit;
using PieceOfCake.Shared.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieceOfCake.Api.Mapping
{
    public class AutoMapperMappingProfile : Profile
    {
        public AutoMapperMappingProfile()
        {
            CreateMap<MeasureUnit, MeasureUnitVm>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));

            CreateMap<Product, ProductVm>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));

            CreateMap<Ingredient, ReadIngredientVm>();

            CreateMap<Dish, DishVm>();
        }
    }
}
