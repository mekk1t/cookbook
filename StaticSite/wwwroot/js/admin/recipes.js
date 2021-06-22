boldLink("#admin-recipes-link");

window.onload = function () {
    refresh();
    let addRecipeForm = document.querySelector('form');
    addRecipeForm.addEventListener('submit', event => {
        event.preventDefault();
        addRecipe();
    })
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
    let form = document.querySelector('form');
    form.querySelector('input[name="title"]').value = recipe.title;
    form.querySelector('textarea').value = recipe.description;
    form.querySelector('input[name="id"]').value = recipe.id;

    document.querySelector('#add').style.display = 'none';
    document.querySelector('#update').style.display = 'block';
    document.querySelector('#delete').style.display = 'block';

    openModal();
}

function addRecipe() {
    let form = document.querySelector('form');
    let title = form.querySelector('input[name="title"]').value;
    let description = form.querySelector('textarea').value;
    _post("recipes", JSON.stringify({
        title,
        description
    }))
        .then(response => {
            if (response.ok) {
                refresh();
                closeModal();
            }
            else {
                alert(getApiErrorsAsString(response.json()));
                closeModal();
            }
        });
}

function openModal() {
    document.querySelector('.w3-modal').style.display = 'block';
}

function closeModal() {
    document.querySelector('.w3-modal').style.display = 'none';
}