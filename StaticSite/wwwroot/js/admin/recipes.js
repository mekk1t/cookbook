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

function td(innerHtml, id = null) {
    let result = document.createElement("td");
    result.innerHTML = innerHtml;
    if (id != null)
        result.id = id;
    return result;
}