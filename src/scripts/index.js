window.onload = function() {
    var testResponse = fetch('https://localhost:5001/recipes')
        .then((response) => {
            return response.json();
        })
        .then((data) => {
            let firstRecipe = data[0];
            let titleElement = document.getElementsByClassName("random-recipe-title")[0];
            titleElement.innerHTML = firstRecipe.title;
        });
}