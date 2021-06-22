boldLink("#admin-recipes-link");

window.onload = function () {
    _fetch("recipes")
        .then(json => {
            let recipes = json.items;
            let table = document.querySelector('table tbody');
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

                table.appendChild(tableRow);
            }
        });
    let addRecipeForm = document.querySelector('form');
    addRecipeForm.addEventListener('submit', event => {
        event.preventDefault();
        addRecipe();
    })
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
                alert("Создал!");
            }
            else {
                alert(getApiErrorsAsString(response.json()));
            }
        });
}