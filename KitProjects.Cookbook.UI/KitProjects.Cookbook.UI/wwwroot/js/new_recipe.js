var recipeStepsOrder = 0;
var recipeIngredientsOrder = 0;
var stepIngredientsOrder = 0;
$('#new-ingredient form button').on('submit', function (event) {
    event.preventDefault();
    $.ajax({
        url: window.location.pathname + '?handler=NewIngredient',
        method: 'POST',
        data: JSON.stringify({ Name: $('#new-ingredient-name').val(), Type: $('#new-ingredient-type').val() })
    }).done(function (result) {
        $.ajax({
            url: window.location.pathname + '?handler=IngredientToRecipe',
            method: 'GET',
            data: JSON.stringify({ order: recipeIngredientsOrder, ingredientId: result.Id })
        }).done(function (partial) {
            $('div.ingredients-list').append(partial);
            $('#new-ingredient.form').hide();
        });
    });
});

$('#add-step-to-recipe').on('click', function () {
    $.ajax({
        url: window.location.pathname + '?handler=StepToRecipe',
        data: { order: recipeStepsOrder },
        dataType: 'html',
        method: 'GET'
    }).done(function (result) {
        $('div.steps-list').append(result);
        recipeStepsOrder += 1;
        //$('#add-ingredient-to-step').on('click', function () {
        //    $.ajax({
        //        url: window.location.pathname + '?handler=IngredientToStep',
        //        data: { stepOrder: recipeStepsOrder, ingredientOrder: stepIngredientsOrder },
        //        dataType: 'html',
        //        method: 'GET'
        //    }).done(function (result) {
        //        $('div.step-ingredients').append(result);
        //        stepIngredientsOrder += 1;
        //    });
        //});
    });
});
    //$('#add-ingredient-to-recipe').on('click', function () {
    //    $.ajax({
    //        url: window.location.pathname + '?handler=IngredientToRecipe',
    //        data: { order: recipeIngredientsOrder },
    //        dataType: 'html',
    //        method: 'GET'
    //    }).done(function (result) {
    //        $('div.ingredients-list').append(result);
    //        recipeIngredientsOrder += 1;
    //    });
    //});