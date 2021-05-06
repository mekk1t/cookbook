const _pageBody = document.getElementById("admin-body");
const _initialPageState = _pageBody.innerHTML;
boldLink("#admin-categories-link");
document.getElementById("show-add-modal").addEventListener("click", function () {
    document.querySelector("#id01 div.w3-container").appendChild(newCategoryForm());
    document.getElementById('id01').style.display = 'block';
});
document.getElementById("close-add-modal").addEventListener("click", function () {
    document.getElementById('id01').style.display = 'none';
    let form = document.querySelector("form.add-category-form");
    if (form) {
        form.remove();
    }
});
document.getElementById("close-edit-modal").addEventListener("click", function () {
    document.getElementById('id02').style.display = 'none';
    let form = document.querySelector("form.edit-category-form");
    if (form) {
        form.remove();
    }
});

/**
 * Меняет состояние страницы.
 * @param {Element} newChild Новый HTML элемент-потомок.
 */
function replacePageBody(newChild) {
    _pageBody.innerHTML = '';
    _pageBody.appendChild(newChild);
}

function refreshList() {
    let _categoriesList = selectFirst("ul.categories-list");
    _categoriesList.innerHTML = '';
    fetch('https://localhost:5001/categories')
        .then(response => {
            return response.json();
        })
        .then(data => {
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
    refreshList();
}

function attachEventHandlersToActions() {
    let _categoriesListItems = document.querySelectorAll("li.category");
    for (let _li of _categoriesListItems) {
        let categoryId = _li.id;
        let categoryName = _li.textContent;
        let _edit = _li.querySelector(".edit-icon");
        let _delete = _li.querySelector(".delete-icon");
        _edit.addEventListener("click", function () {
            document.querySelector("#id02 div.w3-container").appendChild(editCategoryForm(categoryId, categoryName));
            document.getElementById('id02').style.display = 'block';
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

function editCategoryForm(categoryId, categoryName) {
    let _form = document.createElement("form");
    _form.classList.add("edit-category-form");
    document.querySelector("#id02 h2.edit-category-modal-title").textContent = "Новое имя для категории \"" + categoryName + "\"";
    let _titleInput = document.createElement("input");
    _titleInput.name = "category-name";
    _titleInput.id = "category-name";
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