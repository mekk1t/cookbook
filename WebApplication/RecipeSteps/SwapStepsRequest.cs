using System;

namespace KitProjects.MasterChef.WebApplication.RecipeSteps
{
    /// <summary>
    /// Модель запроса на смену мест шагов в рецепте.
    /// </summary>
    public class SwapStepsRequest
    {
        /// <summary>
        /// ID одного шага.
        /// </summary>
        public Guid FirstStepId { get; set; }
        /// <summary>
        /// ID другого шага.
        /// </summary>
        public Guid SecondStepId { get; set; }
        /// <summary>
        /// ID рецепта, в котором будут меняться шаги.
        /// </summary>
        public Guid RecipeId { get; set; }
    }
}
