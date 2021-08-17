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
            appendIngredientDetails(partial);
            $('#ingredients-select-list').append(new Option($('#new-ingredient-name').val(), result, false, false)).trigger('change');
            $('#new-ingredient form').hide();
            $('#new-ingredient form :input').val('');
        });
    });
});

$.ajax({
    url: '/api/ingredients',
    method: 'GET',
    success: function (response) {
        var ingredients = jQuery.map(response, function (element, index) {
            return {
                id: element.id,
                text: element.name
            }
        });
        $('#ingredients-select-list').select2({
            data: ingredients,
            tags: true,
            createTag: function (params) {
                let term = $.trim(params.term);
                $('#new-ingredient').show();
                $('#new-ingredient-name').val(term);
                return null;
            }
        });

        $('#ingredients-select-list option').each(function () {
            $(this).on('click', function (event) {
                event.preventDefault();
                $.ajax({
                    url: window.location.pathname + '?handler=IngredientToRecipe',
                    data: { order: recipeIngredientsOrder, ingredientId: $(this).val() },
                    dataType: 'html',
                    method: 'GET'
                }).done(function (result) {
                    appendIngredientDetails(result);
                });
            });
        });
    }
});

function appendIngredientDetails(html) {
    let div = $('<div class="w3-row"><div class="w3-rest"></div></div>');
    $('div.ingredients-list').append(div);
    $('div.ingredients-list div.w3-rest').last().append(html);
    recipeIngredientsOrder += 1;
}

$('#tags-select-list').select2({
    tags: true
});
$('#categories-select-list').select2({
    tags: true
})
$('#cooking-types-select-list').select2({
    tags: true
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
    //
    //});

function verificationToken() {
    return $('input[name="__RequestVerificationToken"]').val();
}