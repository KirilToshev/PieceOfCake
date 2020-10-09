namespace PieceOfCake.Core.IoModels
{
    public class AddIngredientDto
    {
        public float Quantity { get; set; }

        public long MeasureUnitId { get; set; }

        public long ProductId { get; set; }
    }
}
