function getCategories() {
    let categories = fetch('https://localhost:5001/categories')
        .then(response => {
            return response.json();
        });
    return categories;
}