﻿using PieceOfCake.Application.Common.Services;
using PieceOfCake.Application.IngredientFeature.Dtos;

namespace PieceOfCake.Application.IngredientFeature.Services;

public interface IProductService : ICreateAndUpdateService<ProductGetDto, ProductCreateDto, ProductUpdateDto>
{
}
