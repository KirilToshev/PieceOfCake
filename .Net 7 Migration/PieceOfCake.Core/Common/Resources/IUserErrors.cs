namespace PieceOfCake.Core.Common.Resources;

public interface IUserErrors
{
    public string NameIsMandatory { get; }
    public string NameExceedsMaxLength { get; }
    public string NameBelowMinLength { get; }
    public string NameAlreadyExists { get; }
    public string SequenceContainsNoElements { get; }
    public string IdNotFound { get; }
    public string QuantityMustBeGraterThanZero { get; }
    public string DescriptionExceedsMaxLength { get; }
    public string DescriptionIsMandatory { get; }
    public string ServingSizeMustBeGraterThanOne { get; }
    public string ServingSizeMustBeLessThanByteLimit { get; }
    public string InvalidStateTransition { get; }
    public string IngredientAlreadyExists { get; }
    public string ItemIsInUse { get; }
    public string PeriodStartDateLaterThanEndDate { get; }
    public string MenuMustHaveAtLeastOneServing { get; }
    public string StartDateIsMandatory { get; }
    public string EndDateIsMandatory { get; }
    public string NotEnoughDishesOfMenuType { get; }
    public string DishMustHaveIngredients { get; }
    public string DishMustHaveMenuOfTheDayType { get; }
    public string MealOfTheDayTypeAlreadyExists { get; }
    public string MenuMustHaveAtleastOnePerson { get; }

}
