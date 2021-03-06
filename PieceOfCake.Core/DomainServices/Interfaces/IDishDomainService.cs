﻿using CSharpFunctionalExtensions;
using PieceOfCake.Core.Entities;
using PieceOfCake.Core.IoModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PieceOfCake.Core.DomainServices.Interfaces
{
    public interface IDishDomainService
    {
        Result<IReadOnlyCollection<Dish>> Get();
        Result<Dish> Get(long id);
        Result<Dish> Create(string? name, string? description);
        Result<Dish> UpdateNameAndDescritption(long id, string? name, string? description);
        Result Delete(long id);
        Result UpdateIngredients(long id, IEnumerable<AddIngredientDto> ingredientsVmList);
    }
}
