using AutoMapper;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.IoModels;
using PieceOfCake.Core.Resources;
using PieceOfCake.Shared.ViewModels.Dish;
using PieceOfCake.Shared.ViewModels.Dish.Ingredient;
using PieceOfCake.Shared.ViewModels.MeasureUnit;
using PieceOfCake.Shared.ViewModels.Menu;
using PieceOfCake.Shared.ViewModels.Product;
using System.Linq;

namespace PieceOfCake.Api.Mapping
{
    public class AutoMapperMappingProfile : Profile
    {
        public AutoMapperMappingProfile(IResources resources)
        {
            CreateMap<MeasureUnit, MeasureUnitVm>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));

            CreateMap<Product, ProductVm>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));

            CreateMap<Ingredient, ReadIngredientVm>();

            CreateMap<Dish, DishVm>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => resources.CommonTerms.DishState(src.DishState.State)));

            CreateMap<Menu, MenuVm>()
                //TODO: Fix .Value call -> real NullReferenceException threat!
                .ForMember(dest => dest.DishesPerDay, opt => opt.MapFrom(src => src.CalculateDishesPerDay(resources).Value
                .ToDictionary(
                    kvPair => kvPair.Key.Date.ToShortDateString() + " " + resources.CommonTerms.DayOfWeek(kvPair.Key.Date.DayOfWeek),
                    kvPair => kvPair.Value)));

            CreateMap<AddIngredientVm, AddIngredientDto>().ReverseMap();
        }
    }
}
