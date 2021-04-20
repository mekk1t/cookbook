window.onload = function() {
    var testResponse = fetch('https://localhost:5001/recipes')
        .then((response) => {
            return response.json();
        })
        .then((data) => {
            let firstRecipe = data[0];
            let ingredients = firstRecipe.ingredients;

            let ingredientsList = document.getElementsByClassName("ingredients")[0];
            for (let ingredient of ingredients) {
                let li = document.createElement("li");
                li.innerHTML = ingredient.name;
                ingredientsList.appendChild(li);
            }
        });
}