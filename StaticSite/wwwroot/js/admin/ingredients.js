const _pageBody = document.getElementById("admin-body");
let _previousPageState = _pageBody.innerHTML;
boldLink("#admin-ingredients-link");

window.onload = function () {
    fetch('https://localhost:5001/ingredients')
        .then(response => {
            return response.json();
        })
        .then(data => {
            let _ingredientsList = selectFirst("ul.ingredients-list");
            for (let ingredient of data.ingredients) {
                let _li = _new("li");
                _li.textContent = ingredient.name;
                _li.id = ingredient.id;
                _ingredientsList.appendChild(_li);
            }

            appendActionsToList("ul.ingredients-list");
        });
}