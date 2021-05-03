const _pageBody = document.getElementById("admin-body");
let _previousPageState = _pageBody.innerHTML;
boldLink("#admin-categories-link");

window.onload = function () {
    fetch('https://localhost:5001/categories')
        .then(response => {
            return response.json();
        })
        .then(data => {
            let _categoriesList = selectFirst("ol.categories-list");
            for (let category of data.categories) {
                let _li = _new("li");
                _li.textContent = category.name;
                _li.id = category.id;
                _li.classList.add("category");
                _categoriesList.appendChild(_li);
            }
        });
}

function attachEventHandlersToActions() {
    let _tableRows = document.querySelectorAll("tbody tr");
    for (let _row of _tableRows) {
        let categoryId = _row.querySelectorAll("td")[0].id;
        let categoryName = _row.querySelector("td.category").textContent;
        let _edit = _row.querySelector(".edit-icon");
        let _delete = _row.querySelector(".delete-icon");
        _edit.addEventListener("click", function () {
            _previousPageState = _pageBody.innerHTML;
            _pageBody.innerHTML = '';
            let _editForm = editCategoryForm(categoryId);
            _pageBody.appendChild(_editForm);
        });
        _delete.addEventListener("click", function () {
            fetch(`https://localhost:5001/categories/${categoryName}`, {
                method: 'DELETE'
            });
        });
    }
}

function editCategoryForm(categoryId) {
    let _form = document.createElement("form");
    _form.classList.add("edit-category-form");
    let _titleInput = document.createElement("input");
    _titleInput.name = "category-name";
    let _submitButton = document.createElement("button");
    _submitButton.type = "submit";
    _submitButton.textContent = "Редактировать";
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