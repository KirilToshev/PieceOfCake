namespace PieceOfCake.Core.IoModels;

public class AddIngredientDto
{
    public float Quantity { get; set; }

    public Guid MeasureUnitId { get; set; }

    public Guid ProductId { get; set; }
}
