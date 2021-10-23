'use strict';

var recipeIngredientsOrder = 0;
var recipeStepsOrder = 0;
var currentBlock = 1;
const stepIngredientsCounter = new StepIngredientsCounter();
const VERIFICATION_TOKEN = $('input[name="__RequestVerificationToken"]').val();

// Добавление ингредиента в рецепт
function getNewIngredientFromForm() {
    return {
        name: $('#new-ingredient-name').val(),
        type: Number.parseInt($('#new-ingredient-type').val())
    };
}
function appendIngredientDetails(html) {
    $('div.ingredients-list').append(html);
    recipeIngredientsOrder += 1;
}
function addIngredientToRecipeSelect2(ingredientId) {
    $('#ingredients-select-list').append(new Option($('#new-ingredient-name').val(), ingredientId, false, false)).trigger('change');
}
function closeNewIngredientForm() {
    $('#new-ingredient').hide();
    $('#new-ingredient form :input').val('');
}

async function Main() {
    let ingredientsSelect2 = new IngredientsSelect2();
    await ingredientsSelect2.initializeAsync();

    // Как-то бы отрефакторить
    ingredientsSelect2.$select2.on('select2:select', function (event) {
        let $this = $(this);
        let triggerChange = function () {
            $this.val(null).trigger('change');
        };
        let ingredient = event.params.data;
        if (ingredient.newTag === true) {
            $('#new-ingredient-name').val(ingredient.text);
            $('#new-ingredient').show();
        } else {
            if ($(`#ingredient-id-${ingredient.id}`).length === 1) {
                triggerChange();
                return;
            } else {
                $.ajax({
                    url: window.location.pathname + '?handler=IngredientToRecipe',
                    data: { order: recipeIngredientsOrder, ingredientId: $this.val() },
                    dataType: 'html',
                    method: 'GET'
                }).done(function (result) { appendIngredientDetails(result); });
            }
        }
        triggerChange();
    });
    // Вынести в Select2-инициализатор
    $('#tags-select-list').select2({ tags: true });
    $('#add-step-to-recipe').on('click', appendStepToRecipe);
    hideSecondaryBlocks();
    $('#navigation-button').on('click', navigationDisplay);
}

Main();

function appendStepToRecipe() {
    $.ajax({
        url: window.location.pathname + '?handler=StepToRecipe',
        data: { order: recipeStepsOrder },
        dataType: 'html',
        method: 'GET'
    }).done(function (result) {
        $('div.steps-list').append(result);
        initializeStepIngredientsSelect2();
    });
}

function initializeStepIngredientsSelect2() {
    let $currentStepIngredientsSelect2 = $(`#step-${recipeStepsOrder}-ingredients-select-list`);
    $currentStepIngredientsSelect2.data('step-id', recipeStepsOrder);
    recipeStepsOrder += 1;
    $currentStepIngredientsSelect2.select2({
        data: getRecipeIngredientsSelect2Results(),
        placeholder: 'Выбрать ингредиент из ингредиентов рецепта'
    }).on('select2:select', stepIngredientsSelect2Handler);
}

function getRecipeIngredientsSelect2Results() {
    let ids = $('div.ingredients-list div.ingredient div.w3-rest input:nth-child(2)');
    let names = $('div.ingredients-list div.ingredient div.w3-rest input:last-child');
    let select2Results = [];
    for (let i = 0; i < ids.length; i++) {
        select2Results.push({
            id: ids[i].value,
            text: names[i].value
        });
    }
    return select2Results;
}

function stepIngredientsSelect2Handler(event) {
    let $this = $(this);
    let triggerChange = function () {
        $this.val(null).trigger('change');
    }
    let ingredient = event.params.data;
    let stepNumber = $this.data('step-id');
    if ($(`#step-${stepNumber}-ingredients #ingredient-id-${ingredient.id}`).length === 1) {
        triggerChange();
        return;
    } else {
        $.ajax({
            url: window.location.pathname + '?handler=IngredientToStep',
            data: {
                stepOrder: stepNumber,
                ingredientOrder: stepIngredientsCounter.getCount(stepNumber),
                ingredientId: $(this).val()
            },
            dataType: 'html',
            method: 'GET'
        }).done(function (result) { appendIngredientDetailsToStep(result, stepNumber); });
    }
    triggerChange();
}

function navigationDisplay() {
    switch (currentBlock) {
        case 1: {
            $('#recipe-block').hide();
            $('#ingredients-block').show();
            break;
        }
        case 2: {
            $('#ingredients-block').hide();
            $('#steps-block').show();
            break;
        }
        case 3: {
            $('#steps-block').hide();
            $('#done-block').show();
            $('#navigation-button').hide();
            break;
        }
    }
    currentBlock += 1;
}

function hideSecondaryBlocks() {
    $('#ingredients-block').hide();
    $('#steps-block').hide();
    $('#done-block').hide();
}

class StepIngredientsCounter {
    constructor() {
        this.count = {};
    }

    getCount(stepNumber) {
        return this.count[stepNumber];
    }

    increment(stepNumber) {
        if (!this.count[stepNumber]) {
            this.count[stepNumber] = 1;
        } else {
            this.count[stepNumber] += 1;
        }
    }
}

function appendIngredientDetailsToStep(html, stepOrder) {
    $(`#step-${stepOrder}-ingredients`).append(html);
    stepIngredientsCounter.increment(stepOrder);
}

async function PostAsync(url, data) {
    let response = await fetch(url, {
        method: 'POST',
        headers: {
            'RequestVerificationToken': VERIFICATION_TOKEN,
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(data)
    });
    return await response.json();
}
async function GetAsync(url) {
    let response = await fetch(url, {
        method: 'GET',
        headers: {
            'RequestVerificationToken': VERIFICATION_TOKEN
        }
    });
    return await response.json();
}

class IngredientsSelect2 {
    constructor() {
        this.$select2 = $('#ingredients-select-list');
    }

    async initializeAsync() {
        var ingredientsJson = await GetAsync('api/ingredients');
        var ingredients = ingredientsJson.map(function (i) {
            return {
                id = i.id,
                text: i.name
            };
        });
        this.$select2.select2({
            data: ingredients,
            tags: true,
            placeholder: 'Выбрать ингредиент',
            createTag: function (params) {
                let term = params.term.trim();
                if (term.length < 3) { return null; }

                return {
                    id: term,
                    text: term,
                    newTag: true
                };
            }
        });
    }
}

class NewIngredientForm {

    async createIngredientAsync() {
        var id = Number.parseInt(await PostAsync('/api/ingredients', getNewIngredientFromForm()));

    }

    add

    addIngredientToRecipe() {

    }

    setHandlerOnForm(formId) {
        $('#new-ingredient form button').on('click', function (event) {
            event.preventDefault();
            postJson('/api/ingredients', getNewIngredientFromForm(), function (result) {
                $.ajax({
                    url: window.location.pathname + '?handler=IngredientToRecipe',
                    method: 'GET',
                    data: { order: recipeIngredientsOrder, ingredientId: Number.parseInt(result) }
                }).done(function (partial) {
                    appendIngredientDetails(partial);
                    addIngredientToRecipeSelect2(result);
                    closeNewIngredientForm();
                });
            });
        });
    }
}