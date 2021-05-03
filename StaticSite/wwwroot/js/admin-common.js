/**
 * Выделяет ссылку жирным текстом.
 * @param {string} linkIdSelector селектор ссылки по ID, которую нужно выделить.
 */
function boldLink(linkIdSelector) {
    document.querySelector(linkIdSelector).style.fontWeight = "bold";
}

/**
 * Возвращает первый элемент по селектору.
 * @param {string} selector Селектор элемента.
 */
function selectFirst(selector) {
    return document.querySelector(selector);
}

/**
 * Возвращает новый созданный элемент по тегу.
 * @param {strng} elementTag Тег элемента.
 */
function _new(elementTag) {
    return document.createElement(elementTag);
}

/**
 * Создает ячейку таблицы.
 * @param {string} innerHtml Внутренний HTML ячейки.
 * @param {string} id ID элемента.
 */
function td(innerHtml, id = null) {
    let result = document.createElement("td");
    result.innerHTML = innerHtml;
    if (id != null)
        result.id = id;
    return result;
}

/**
 * Рендерит popup-иконки редактирования и удаления строки в таблице.
 * @param {HTMLTableSectionElement} tableBody
 * @param {string} tableCellsSelector
 */
function appendActionsToTable(_tableBody, tableCellsSelector) {
    _tableCells = _tableBody.querySelectorAll(tableCellsSelector);
    for (let cell of _tableCells) {
        let actions = document.createElement("span");
        actions.classList.add("actions");
        let editIcon = document.createElement("img");
        editIcon.src = "/icons/edit-box.svg";
        editIcon.classList.add("edit-icon");
        let deleteIcon = document.createElement("img");
        deleteIcon.src = "/icons/cross-symbol.svg";
        deleteIcon.classList.add("delete-icon");
        actions.appendChild(editIcon);
        actions.appendChild(deleteIcon);
        cell.appendChild(actions);
    }
}