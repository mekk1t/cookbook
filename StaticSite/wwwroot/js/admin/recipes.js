const _pageBody = document.getElementById("admin-body");
let _previousPageState = _pageBody.innerHTML;

initAddRecipeEvent();

window.onload = function () {
    fetch('https://localhost:5001/recipes')
        .then(response => {
            return response.json();
        })
        .then(data => {
            let _recipesTable = document.querySelector("#recipes-admin-table").querySelector("tbody");
            for (let recipe of data) {
                let _tr = document.createElement("tr");
                let _tdTitle = td(recipe.title, `title_${recipe.id}`);
                _tdTitle.classList.add("recipe");
                let _tdId = td(recipe.id, "id");
                _tdId.style.display = "none";
                _tr.appendChild(_tdId);
                _tr.appendChild(_tdTitle);

                _recipesTable.appendChild(_tr);
            }

            _recipesTableRows = _recipesTable.querySelectorAll("tr td.recipe");
            for (let row of _recipesTableRows) {
                let actions = document.createElement("span");
                actions.classList.add("actions");
                let editIcon = document.createElement("img");
                editIcon.src = "/icons/edit-box.svg";
                let deleteIcon = document.createElement("img");
                deleteIcon.src = "/icons/cross-symbol.svg";
                actions.appendChild(editIcon);
                actions.appendChild(deleteIcon);
                row.appendChild(actions);
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

function td(innerHtml, id = null) {
    let result = document.createElement("td");
    result.innerHTML = innerHtml;
    if (id != null)
        result.id = id;
    return result;
}

function initAddRecipeEvent() {
    let _add = document.querySelector(".add");
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