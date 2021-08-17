var recipeStepsOrder = 0;
var recipeIngredientsOrder = 0;
var stepIngredientsOrder = 0;
$('#new-ingredient form button').on('click', function (event) {
    event.preventDefault();
    $.ajax({
        url: '/api/ingredients',
        method: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({
            name: $('#new-ingredient-name').val(),
            type: Number.parseInt($('#new-ingredient-type').val())
        }),
        headers: {
            'RequestVerificationToken': verificationToken()
        }
    }).done(function (result) {
        $.ajax({
            url: window.location.pathname + '?handler=IngredientToRecipe',
            method: 'GET',
            data: { order: recipeIngredientsOrder, ingredientId: Number.parseInt(result) }
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

function verificationToken() {
    return $('input[name="__RequestVerificationToken"]').val();
}