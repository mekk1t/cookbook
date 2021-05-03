const _pageBody = document.getElementById("admin-body");
let _previousPageState = _pageBody.innerHTML;
boldLink("#admin-recipes-link");

initAddRecipeEvent();

window.onload = function () {
    fetch('https://localhost:5001/recipes')
        .then(response => {
            return response.json();
        })
        .then(data => {
            let _recipesList = document.querySelector("ol.recipes-list");
            for (let recipe of data) {
                let _li = _new("li");
                _li.textContent = recipe.title;
                _li.id = recipe.id;
                _li.classList.add("recipe");
                _recipesList.appendChild(_li);
            }
        });
}

function renderAddRecipeForm() {
    _previousPageState = _pageBody.innerHTML;
    clearPage();
    let _addForm = getAddRecipeForm();
    _pageBody.appendChild(_addForm);
}

function clearPage() {
    _pageBody.innerHTML = '';
}

function initAddRecipeEvent() {
    let _add = document.querySelector(".add-recipe");
    _add.addEventListener("click", renderAddRecipeForm);
}

function getAddRecipeForm() {
    let _form = document.createElement("form");
    _form.classList.add("add-recipe-form");
    let _titleInput = document.createElement("input");
    _titleInput.name = "recipe-title";
    let _submitButton = document.createElement("button");
    _submitButton.type = "submit";
    _submitButton.textContent = "Создать";
    let _backButton = document.createElement("button");
    _backButton.type = "button";
    _backButton.textContent = "Назад";
    _backButton.addEventListener("click", function () {
        _pageBody.innerHTML = _previousPageState;
        initAddRecipeEvent();
    })

    _form.appendChild(_titleInput);
    _form.appendChild(_submitButton);
    _form.appendChild(_backButton);

    _form.addEventListener("submit", function (event) {
        event.preventDefault();
        let requestBody = {
            categories: [],
            ingredients: [],
            steps: [],
            title: _titleInput.value
        };
        fetch('https://localhost:5001/recipes', {
            method: 'POST',
            body: JSON.stringify(requestBody),
            headers: {
                'Content-Type': 'application/json;charset=utf-8',
            }
        })
    });

    return _form;
}