using AutoFixture;
using PieceOfCake.Core.Common.Persistence;
using PieceOfCake.Core.Common.Resources;
using PieceOfCake.Core.IngredientFeature.Entities;
using PieceOfCake.Core.IngredientFeature.ValueObjects;

namespace PieceOfCake.Tests.Common.Fakes;

public class IngredientFakes : BaseFakes
{
    private ProductFakes _productFakes;
    private MeasureUnitFakes _measureUnitFakes;

    public IngredientFakes (IResources resources, IUnitOfWork uowMock) 
        : base(resources, uowMock)
    {
        _productFakes = new ProductFakes(_resources, _uowMock);
        _measureUnitFakes = new MeasureUnitFakes(_resources, _uowMock);
    }

    public Ingredient One_Number_Of_Carrots => Create(1, _measureUnitFakes.Number, _productFakes.Carrot);
    public Ingredient Two_Kilogram_Of_Peppers => Create(2, _measureUnitFakes.Kg, _productFakes.Pepper);
    public Ingredient Three_Litters_Of_Water => Create(3, _measureUnitFakes.Litter, _productFakes.Water);

    public Ingredient Create(
        float? quantity = null,
        MeasureUnit? measureUnit = null,
        Product? product = null)
    {
        if (measureUnit is null)
            measureUnit = _fixture.OneOf(
                _measureUnitFakes.Kg,
                _measureUnitFakes.Litter,
                _measureUnitFakes.Number);
        if (product is null)
            product = _fixture.OneOf(
                _productFakes.Pepper,
                _productFakes.Water,
                _productFakes.Carrot);

        return Ingredient.Create(
            quantity ?? _fixture.Create<ushort>(), 
            measureUnit, 
            product, 
            _resources).Value;
    }
}
