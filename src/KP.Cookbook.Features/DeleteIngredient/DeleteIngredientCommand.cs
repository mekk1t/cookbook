namespace KP.Cookbook.Features.DeleteIngredient
{
    public class DeleteIngredientCommand
    {
        public long Id { get; }

        public DeleteIngredientCommand(long ingredientId)
        {
            Id = ingredientId;
        }
    }
}
