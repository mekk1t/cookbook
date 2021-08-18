var recipeStepsOrder = 0;
var recipeIngredientsOrder = 0;
var stepsIngredientsCount = new Object();
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
            $('#new-ingredient').hide();
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
            placeholder: 'Выбрать ингредиент',
            createTag: function (params) {
                let term = $.trim(params.term);
                if (term.length < 3) { return null; }

                return {
                    id: term,
                    text: term,
                    newTag: true
                };
            }
        });

        $('#ingredients-select-list').on('select2:select', function (event) {
            let ingredient = event.params.data;
            if (ingredient.newTag === true) {
                $('#new-ingredient').show();
                $('#new-ingredient-name').val(ingredient.text);
            } else {
                if ($(`div.ingredients-list #ingredient-id-${ingredient.id}`).length === 1) {
                    $('#ingredients-select-list').val(null).trigger('change');
                    return;
                } else {
                    $.ajax({
                        url: window.location.pathname + '?handler=IngredientToRecipe',
                        data: { order: recipeIngredientsOrder, ingredientId: $(this).val() },
                        dataType: 'html',
                        method: 'GET'
                    }).done(function (result) { appendIngredientDetails(result); });
                }
            }
            $('#ingredients-select-list').val(null).trigger('change');
        });
    }
});

function appendIngredientDetails(html) {
    // Можно добавить кнопку удаления. Она будет скрывать или убирать HTML ингредиента, при этом
    // есть два варианта, как поступить с порядком элементов в массиве:
    // 1) Переописывать индекс каждого ингредиента.
    // 2) Обнулять ID ингредиента на клиенте, а на сервере убирать ингредиенты с нулевым ID.
    $('div.ingredients-list').append(html);
    recipeIngredientsOrder += 1;
}

$('#tags-select-list').select2({
    tags: true
});
$('#categories-select-list').select2();
$('#cooking-types-select-list').select2();

$('#add-step-to-recipe').on('click', function () {
    $.ajax({
        url: window.location.pathname + '?handler=StepToRecipe',
        data: { order: recipeStepsOrder },
        dataType: 'html',
        method: 'GET'
    }).done(function (result) {
        $('div.steps-list').append(result);
        let ids = $('div.ingredients-list div.ingredient div.w3-rest input:nth-child(2)');
        let names = $('div.ingredients-list div.ingredient div.w3-rest input:last-child');
        let select2Results = [];
        for (let i = 0; i < ids.length; i++) {
            select2Results.push({
                id: ids[i].value,
                text: names[i].value
            });
        }

        let $currentStepIngredientsSelect2 = $(`#step-${recipeStepsOrder}-ingredients-select-list`);
        $currentStepIngredientsSelect2.data('step-id', recipeStepsOrder);
        recipeStepsOrder += 1;
        $currentStepIngredientsSelect2.select2({
            data: select2Results,
            placeholder: 'Выбрать ингредиент из ингредиентов рецепта'
        }).on('select2:select', function (event) {
            let ingredient = event.params.data;
            let stepNumber = $currentStepIngredientsSelect2.data('step-id');
            if ($(`#step-${stepNumber}-ingredients #ingredient-id-${ingredient.id}`).length === 1) {
                $currentStepIngredientsSelect2.val(null).trigger('change');
                return;
            } else {
                $.ajax({
                    url: window.location.pathname + '?handler=IngredientToStep',
                    data: { stepOrder: stepNumber, ingredientOrder: stepsIngredientsCount[stepNumber], ingredientId: $(this).val() },
                    dataType: 'html',
                    method: 'GET'
                }).done(function (result) { appendIngredientDetailsToStep(result, stepNumber); });
            }
            $currentStepIngredientsSelect2.val(null).trigger('change');
        });
    });
});

function appendIngredientDetailsToStep(html, stepOrder) {
    $(`#step-${stepOrder}-ingredients`).append(html);
    if (!stepsIngredientsCount[stepOrder]) {
        stepsIngredientsCount[stepOrder] = 1;
    } else {
        stepsIngredientsCount[stepOrder] += 1;
    }
}

function verificationToken() {
    return $('input[name="__RequestVerificationToken"]').val();
}