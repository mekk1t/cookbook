const _pageBody = document.getElementById("admin-body");
let _initialPageState = _pageBody.innerHTML;
boldLink("#admin-ingredients-link");

window.onload = function () {
    refreshList();
};

document.getElementById("show-add-modal").addEventListener("click", function () {
    document.querySelector("#id01 div.w3-container").appendChild(newIngredientForm());
    document.getElementById('id01').style.display = 'block';
});
document.getElementById("close-add-modal").addEventListener("click", function () {
    document.getElementById('id01').style.display = 'none';
    let form = document.querySelector("form.add-ingredient-form");
    if (form) {
        form.remove();
    }
});
document.getElementById("close-edit-modal").addEventListener("click", function () {
    document.getElementById('id02').style.display = 'none';
    let form = document.querySelector("form.edit-ingredient-form");
    if (form) {
        form.remove();
    }
});

function refreshList() {
    let _ingredientsList = selectFirst("ul.ingredients-list");
    _ingredientsList.innerHTML = '';
    fetch('https://localhost:5001/ingredients')
        .then(response => {
            return response.json();
        })
        .then(data => {
            for (let ingredient of data.ingredients) {
                let _li = _new("li");
                _li.textContent = ingredient.name;
                _li.id = ingredient.id;
                _li.classList.add("ingredient");
                _ingredientsList.appendChild(_li);
            }

            appendActionsToList("ul.ingredients-list");
        })
        .then(attachEventHandlersToActions);
}

function attachEventHandlersToActions() {
    let _ingredientsListItems = document.querySelectorAll("li.ingredient");
    for (let _li of _ingredientsListItems) {
        let ingredientId = _li.id;
        let ingredientName = _li.textContent;
        let _edit = _li.querySelector(".edit-icon");
        let _delete = _li.querySelector(".delete-icon");
        _edit.addEventListener("click", function () {
            document.querySelector("#id02 div.w3-container").appendChild(editIngredientForm(ingredientId, ingredientName));
            document.getElementById('id02').style.display = 'block';
        });
        _delete.addEventListener("click", function () {
            _li.style.display = "none";
            fetch(`https://localhost:5001/ingredients/${ingredientId}`, {
                method: 'DELETE'
            })
                .then(response => {
                    if (response.ok) {
                        alert("Категория " + ingredientName + " успешно удалена.");
                        _li.remove();
                    } else {
                        alert("Не удалось удалить категорию: " + response.statusText);
                    }
                })
        });
    }
}

function newIngredientForm() {
    let _form = document.createElement("form");
    _form.classList.add("add-ingredient-form");
    let _titleLabel = document.createElement("label");
    _titleLabel.htmlFor = "ingredient-name";
    _titleLabel.textContent = "Название";
    let _titleInput = document.createElement("input");
    _titleInput.name = "ingredient-name";
    _titleInput.id = "ingredient-name";
    let _submitButton = document.createElement("button");
    _submitButton.type = "submit";
    _submitButton.textContent = "Создать";

    let _br = function () {
        return document.createElement("br");
    }

    _form.appendChild(_titleLabel);
    _form.appendChild(_br());
    _form.appendChild(_titleInput);
    _form.appendChild(_br());
    _form.appendChild(_submitButton);

    let _closeButton = document.querySelector("#close-add-modal");

    _form.addEventListener("submit", function (event) {
        event.preventDefault();
        let requestBody = {
            name: _titleInput.value,
            categories: []
        };
        fetch(`https://localhost:5001/ingredients`, {
            method: 'POST',
            body: JSON.stringify(requestBody),
            headers: {
                'Content-Type': 'application/json;charset=utf-8',
            }
        })
            .then(response => {
                if (response.ok) {
                    alert("Создание прошло успешно!");
                    refreshList();
                    _closeButton.click();

                } else {
                    alert("Возникла ошибка во время создания: " + response.statusText);
                    refreshList();
                    _closeButton.click();
                }
            });
    });

    return _form;
}

function editIngredientForm(ingredientId, ingredientName) {
    let _form = document.createElement("form");
    _form.classList.add("edit-ingredient-form");
    document.querySelector("#id02 h2.edit-ingredient-modal-title").textContent = "Новое имя для ингредиента \"" + ingredientName + "\"";
    let _titleInput = document.createElement("input");
    _titleInput.name = "ingredient-name";
    _titleInput.id = "ingredient-name";
    let _submitButton = document.createElement("button");
    _submitButton.type = "submit";
    _submitButton.textContent = "Обновить";

    let _br = function () {
        return document.createElement("br");
    }

    let _closeButton = document.querySelector("#close-edit-modal");
    _form.appendChild(_br());
    _form.appendChild(_titleInput);
    _form.appendChild(_br());
    _form.appendChild(_submitButton);

    _form.addEventListener("submit", function (event) {
        event.preventDefault();
        let requestBody = {
            newName: _titleInput.value
        };
        fetch(`https://localhost:5001/ingredients/${ingredientId}`, {
            method: 'PUT',
            body: JSON.stringify(requestBody),
            headers: {
                'Content-Type': 'application/json;charset=utf-8',
            }
        })
            .then(response => {
                if (response.ok) {
                    alert("Обновление прошло успешно!");
                    refreshList();
                    _closeButton.click();
                } else {
                    alert("Возникла ошибка во время редактирования: " + response.statusText);
                    refreshList();
                    _closeButton.click();
                }
            });
    });

    return _form;
}