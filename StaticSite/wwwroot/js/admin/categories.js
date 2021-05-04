const _pageBody = document.getElementById("admin-body");
const _initialPageState = _pageBody.innerHTML;
boldLink("#admin-categories-link");

/**
 * Меняет состояние страницы.
 * @param {Element} newChild Новый HTML элемент-потомок.
 */
function replacePageBody(newChild) {
    _pageBody.innerHTML = '';
    _pageBody.appendChild(newChild);
}

initAddEntityEvent(function () {
    let _addForm = newCategoryForm();
    replacePageBody(_addForm);
});

function refreshPage() {
    fetch('https://localhost:5001/categories')
        .then(response => {
            return response.json();
        })
        .then(data => {
            let _categoriesList = selectFirst("ul.categories-list");
            for (let category of data.categories) {
                let _li = _new("li");
                _li.textContent = category.name;
                _li.id = category.id;
                _li.classList.add("category");
                _categoriesList.appendChild(_li);
            }

            appendActionsToList("ul.categories-list");
        })
        .then(attachEventHandlersToActions);
}

window.onload = function () {
    refreshPage();
}

function attachEventHandlersToActions() {
    let _categoriesListItems = document.querySelectorAll("li.category");
    for (let _li of _categoriesListItems) {
        let categoryId = _li.id;
        let categoryName = _li.textContent;
        let _edit = _li.querySelector(".edit-icon");
        let _delete = _li.querySelector(".delete-icon");
        _edit.addEventListener("click", function () {
            let _editForm = editCategoryForm(categoryId, categoryName);
            replacePageBody(_editForm);
        });
        _delete.addEventListener("click", function () {
            _li.style.display = "none";
            fetch(`https://localhost:5001/categories/${categoryName}`, {
                method: 'DELETE'
            })
                .then(response => {
                    if (response.ok) {
                        alert("Категория " + categoryName + " успешно удалена.");
                        _li.remove();
                    } else {
                        alert("Не удалось удалить категорию: " + response.statusText);
                    }
                })
        });
    }
}

function newCategoryForm() {
    let _form = document.createElement("form");
    _form.classList.add("add-category-form");
    let _titleLabel = document.createElement("label");
    _titleLabel.htmlFor = "category-name";
    _titleLabel.textContent = "Название";
    let _titleInput = document.createElement("input");
    _titleInput.name = "category-name";
    _titleInput.id = "category-name";
    let _submitButton = document.createElement("button");
    _submitButton.type = "submit";
    _submitButton.textContent = "Создать";
    let _backButton = document.createElement("button");
    _backButton.type = "button";
    _backButton.textContent = "Назад";
    _backButton.addEventListener("click", function () {
        _pageBody.innerHTML = _initialPageState;
    })

    let _br = function () {
        return document.createElement("br");
    }

    _form.appendChild(_titleLabel);
    _form.appendChild(_br());
    _form.appendChild(_titleInput);
    _form.appendChild(_br());
    _form.appendChild(_submitButton);
    _form.appendChild(_backButton);

    _form.addEventListener("submit", function (event) {
        event.preventDefault();
        let requestBody = {
            name: _titleInput.value
        };
        fetch(`https://localhost:5001/categories`, {
            method: 'POST',
            body: JSON.stringify(requestBody),
            headers: {
                'Content-Type': 'application/json;charset=utf-8',
            }
        })
            .then(response => {
                if (response.ok) {
                    alert("Создание прошло успешно!");
                    _backButton.click();
                    refreshPage();
                } else {
                    alert("Возникла ошибка во время создания: " + response.statusText);
                    _backButton.click();
                    refreshPage();
                }
            });
    });

    return _form;
}

function editCategoryForm(categoryId, categoryName) {
    let _form = document.createElement("form");
    _form.classList.add("edit-category-form");
    let _titleLabel = document.createElement("label");
    _titleLabel.htmlFor = "category-name";
    _titleLabel.textContent = "Новое имя для категории \"" + categoryName + "\"";
    let _titleInput = document.createElement("input");
    _titleInput.name = "category-name";
    _titleInput.id = "category-name";
    let _submitButton = document.createElement("button");
    _submitButton.type = "submit";
    _submitButton.textContent = "Обновить";
    let _backButton = document.createElement("button");
    _backButton.type = "button";
    _backButton.textContent = "Назад";
    _backButton.addEventListener("click", function () {
        _pageBody.innerHTML = _initialPageState;
        initAddRecipeEvent();
    })

    let _br = function () {
        return document.createElement("br");
    }

    _form.appendChild(_titleLabel);
    _form.appendChild(_br());
    _form.appendChild(_titleInput);
    _form.appendChild(_br());
    _form.appendChild(_submitButton);
    _form.appendChild(_backButton);

    _form.addEventListener("submit", function (event) {
        event.preventDefault();
        let requestBody = {
            newName: _titleInput.value
        };
        fetch(`https://localhost:5001/categories/${categoryId}`, {
            method: 'PUT',
            body: JSON.stringify(requestBody),
            headers: {
                'Content-Type': 'application/json;charset=utf-8',
            }
        })
            .then(response => {
                if (response.ok) {
                    alert("Обновление прошло успешно!");
                    _backButton.click();
                    refreshPage();
                } else {
                    alert("Возникла ошибка во время редактирования: " + response.statusText);
                    _backButton.click();
                    refreshPage();
                }
            });
    });

    return _form;
}