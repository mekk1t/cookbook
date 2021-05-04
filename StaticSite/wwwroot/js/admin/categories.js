const _pageBody = document.getElementById("admin-body");
let _previousPageState = _pageBody.innerHTML;
boldLink("#admin-categories-link");

window.onload = function () {
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

function attachEventHandlersToActions() {
    let _categoriesListItems = document.querySelectorAll("li.category");
    for (let _li of _categoriesListItems) {
        let categoryId = _li.id;
        let categoryName = _li.textContent;
        let _edit = _li.querySelector(".edit-icon");
        let _delete = _li.querySelector(".delete-icon");
        _edit.addEventListener("click", function () {
            _previousPageState = _pageBody.innerHTML;
            let _editForm = editCategoryForm(categoryId, categoryName);
            _pageBody.innerHTML = '';
            _pageBody.appendChild(_editForm);
        });
        _delete.addEventListener("click", function () {
            fetch(`https://localhost:5001/categories/${categoryName}`, {
                method: 'DELETE'
            });
        });
    }
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
        _pageBody.innerHTML = _previousPageState;
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
    });

    return _form;
}