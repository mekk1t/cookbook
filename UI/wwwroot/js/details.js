function deleteRecipe(recipeId) {
    $.ajax({
        url: `${window.location.origin}/api/recipes/${recipeId}`,
        method: 'DELETE'
    }).done(function (result) {
        window.location.replace(window.location.origin + "/recipes");
    });
}