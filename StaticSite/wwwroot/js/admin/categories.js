boldLink("#admin-categories-link");

window.onload = function () {
    fetch('https://localhost:5001/categories')
        .then(response => {
            return response.json();
        })
        .then(data => {
            let _categoriesTableBody = selectFirst("table tbody");
            for (let category of data.categories) {
                let _tr = _new("tr");
                let _tdTitle = td(category.name, `title_${category.id}`);
                _tdTitle.classList.add("category");
                let _tdId = td(category.id, "id")
                _tdId.style.display = "none";
                _tr.appendChild(_tdId);
                _tr.appendChild(_tdTitle);

                _categoriesTableBody.appendChild(_tr);
            }

            appendActionsToTable(_categoriesTableBody, "tr td.category");
        });
}