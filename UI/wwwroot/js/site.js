class AjaxRecipe {
    constructor() {

    }

    /**
     * Удаляет рецепт по ID.
     * @param {string} recipeId
     * @param {boolean} redirectToList
     */
    deleteRecipeById(recipeId, redirectToList) {
        $.ajax({
            url: `${window.location.origin}/api/recipes/${recipeId}`,
            method: 'DELETE'
        }).done(function (result) {
            if (redirectToList) {
                window.location.replace(window.location.origin + "/recipes");
            }
        });
    }
}