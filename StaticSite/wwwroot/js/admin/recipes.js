boldLink("#admin-recipes-link");

window.onload = function () {
    refresh();
    let addRecipeForm = document.querySelector('#add-recipe form');
    addRecipeForm.addEventListener('submit', event => {
        event.preventDefault();
        addRecipe();
    })
    let editRecipeForm = document.querySelector('#edit-recipe form');
    editRecipeForm.addEventListener('submit', event => {
        event.preventDefault();
        editRecipe();
    });
}

function refresh() {
    _fetch("recipes")
        .then(json => {
            let recipes = json.items;
            let table = document.querySelector('table tbody');
            table.innerHTML = '';
            for (let recipe of recipes) {
                let tableRow = document.createElement('tr');
                let name = document.createElement('td');
                name.innerHTML = recipe.title;
                let id = document.createElement('td');
                id.classList.add('w3-hide');
                id.innerHTML = recipe.id;
                let description = document.createElement('td');
                description.innerHTML = recipe.description;

                tableRow.appendChild(id);
                tableRow.appendChild(name);
                tableRow.appendChild(description);

                tableRow.addEventListener('click', function () {
                    openEditRecipeForm(recipe);
                });

                table.appendChild(tableRow);
            }
        });
}

function openEditRecipeForm(recipe) {
    let form = document.querySelector('#edit-recipe form');
    form.querySelector('input[name="title"]').value = recipe.title;
    form.querySelector('textarea').value = recipe.description;
    form.querySelector('input[name="id"]').value = recipe.id;
    document.querySelector('#edit-recipe h2').textContent = `Редактирование рецепта ${recipe.title}`;
    document.querySelector('#delete-recipe').onclick = function () {
        _delete(`recipes/${recipe.id}`)
            .then(response => {
                if (response.ok) {
                    refresh();
                    alert("Удаление прошло успешно");
                }
                else {
                    alert(getApiErrorsAsString(response.json()));
                }
            });
    };

    openModal('#edit-recipe');
}

function addRecipe() {
    let form = document.querySelector('#add-recipe form');
    let title = form.querySelector('input[name="title"]').value;
    let description = form.querySelector('textarea').value;
    _post("recipes", JSON.stringify({
        title,
        description
    }))
        .then(response => {
            if (response.ok) {
                refresh();
                closeModal('#add-recipe');
            }
            else {
                alert(getApiErrorsAsString(response.json()));
                closeModal('#add-recipe');
            }
        });
}

function editRecipe() {
    let form = document.querySelector('#edit-recipe form');
    let title = form.querySelector('input[name="title"]').value;
    let description = form.querySelector('textarea').value;
    let id = form.querySelector('input[name="id"]').value;

    _put(`recipes/${id}`, JSON.stringify({
        newTitle : title,
        newDescription : description
    }))
        .then(response => {
            if (response.ok) {
                refresh();
                closeModal('#edit-recipe');
            }
            else {
                alert(getApiErrorsAsString(response.json()));
                closeModal('#edit-recipe');
            }
        });
}

function openModal(modalIdSelector) {
    document.querySelector(modalIdSelector).style.display = 'block';
}

function closeModal(modalIdSelector) {
    document.querySelector(modalIdSelector).style.display = 'none';
}