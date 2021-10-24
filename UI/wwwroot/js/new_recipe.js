'use strict';
let currentBlock = 0;

class IngredientsSelect2 {
    constructor() {
        this.$select2 = $('#ingredients-select-list');
    }

    async initializeAsync(select2Handler) {
        var ingredientsJson = await GetJsonAsync('api/ingredients');
        var ingredients = ingredientsJson.map(function (i) {
            return {
                id: i.id,
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
        this.$select2.on('select2:select', select2Handler);
    }

    addIngredient(id) {
        this.$select2.append(new Option($('#new-ingredient-name').val(), id, false, false)).trigger('change');
    }

    getRecipeIngredients() {
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
}
class StepIngredientsSelect2 {
    static initialize(stepNumber, recipeIngredients, selectHandler) {
        let $select2 = $(`#step-${stepNumber}-ingredients-select-list`);
        $select2.data('step-id', stepNumber);
        $select2.select2({
            data: recipeIngredients,
            placeholder: 'Выбрать ингредиент из ингредиентов рецепта'
        }).on('select2:select', selectHandler);
    }
}
class NewIngredientForm {
    constructor() {
        this.$name = $('#new-ingredient-name');
        this.$type = $('#new-ingredient-type');
        this.$container = $('#new-ingredient');
        this._setHandler();
    }

    async createIngredientAsync(ingredient) {
        var id = Number.parseInt(await PostAsync('/api/ingredients', ingredient));
        var recipeIngredientResponse = await GetHtmlAsync('IngredientToRecipe', {
            order: newRecipeForm.ingredientsCount,
            ingredientId: id
        });
        newRecipeForm.appendIngredient(recipeIngredientResponse)
        ingredientsSelect2.addIngredient(id);
    }

    get ingredient() {
        return {
            name: this.$name.val(),
            type: Number.parseInt(this.$type.val())
        };
    }
    set name(value) {
        this.$name.val(value);
    }

    show() {
        this.$container.show();
    }
    close() {
        this.$container.hide();
        this.$container.find(':input').val('');
    }
    _setHandler() {
        let form = this;
        this.$container.find('form button').on('click', async function (event) {
            event.preventDefault();
            await form.createIngredientAsync(form.ingredient);
            form.close();
        });
    }
}
class CustomSelect2 {
    static initializeTags(canCreateNewTags) {
        $('#tags-select-list').select2({ tags: canCreateNewTags });
    }
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
class NewRecipeForm {
    /**
     * @param {IngredientsSelect2} $ingredientsSelect2
     */
    constructor($ingredientsSelect2) {
        this.$ingredientsSelect2 = $ingredientsSelect2;
        this._ingredientsCount = 0;
        this._stepsCount = 0;
        currentBlock = 1;
        $('#ingredients-block').hide();
        $('#steps-block').hide();
        $('#done-block').hide();
        $('#navigation-button').on('click', this.nextBlock);
        this._setNewStepHandler();
    }
    nextBlock() {
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

    appendIngredient(html) {
        $('div.ingredients-list').append(html);
        this._ingredientsCount += 1;
    }

    get stepsCount() {
        return this._stepsCount;
    }

    get ingredientsCount() {
        return this._ingredientsCount;
    }

    _setNewStepHandler() {
        let form = this;
        $('#add-step-to-recipe').on('click', async function (event) {
            let newStepHtml = await GetHtmlAsync('StepToRecipe', { order: form._stepsCount });
            $('div.steps-list').append(newStepHtml);
            StepIngredientsSelect2.initialize(
                form._stepsCount,
                form.$ingredientsSelect2.getRecipeIngredients(),
                stepIngredientsSelect2Handler);
            form._stepsCount += 1;
        });
    }
}

function ingredientsSelect2Handler(event) {
    let $this = $(this);
    let triggerChange = function () {
        $this.val(null).trigger('change');
    };
    let ingredient = event.params.data;
    if (ingredient.newTag) {
        newIngredientForm.name = ingredient.text;
        newIngredientForm.show();
    } else {
        if ($(`#ingredient-id-${ingredient.id}`).length === 1) {
            triggerChange();
            return;
        } else {
            $.ajax({
                url: window.location.pathname + '?handler=IngredientToRecipe',
                data: { order: newRecipeForm.ingredientsCount, ingredientId: $this.val() },
                dataType: 'html',
                method: 'GET'
            }).done(function (result) {
                newRecipeForm.appendIngredient(result);
            });
        }
    }
    triggerChange();
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
function appendIngredientDetailsToStep(html, stepOrder) {
    $(`#step-${stepOrder}-ingredients`).append(html);
    stepIngredientsCounter.increment(stepOrder);
}

const stepIngredientsCounter = new StepIngredientsCounter();
const VERIFICATION_TOKEN = $('input[name="__RequestVerificationToken"]').val();
const newIngredientForm = new NewIngredientForm();

CustomSelect2.initializeTags(true);

const ingredientsSelect2 = new IngredientsSelect2();
ingredientsSelect2.initializeAsync(ingredientsSelect2Handler);

const newRecipeForm = new NewRecipeForm(ingredientsSelect2);

async function PostAsync(url, data) {
    let response = await fetch(`${window.location.origin}${url}`, {
        method: 'POST',
        headers: {
            'RequestVerificationToken': VERIFICATION_TOKEN,
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(data)
    });
    return await response.json();
}
async function GetJsonAsync(url, data) {
    let targetUrl = `${window.location.origin}/${url}`;
    if (data) {
        targetUrl = targetUrl + new URLSearchParams(data);
    }
    let response = await fetch(targetUrl, {
        method: 'GET',
        headers: {
            'RequestVerificationToken': VERIFICATION_TOKEN
        }
    });
    return await response.json();
}

/**
 *
 * @param {string} handlerName
 * @param {object} data
 */
async function GetHtmlAsync(handlerName, data) {
    data.handler = handlerName;
    let params = $.param(data);
    let response = await fetch(`${window.location.href}?${params}`, {
        method: 'GET',
        headers: {
            'RequestVerificationToken': VERIFICATION_TOKEN
        }
    });
    return await response.text();
}