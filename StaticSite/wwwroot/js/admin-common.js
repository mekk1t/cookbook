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
 * Рендерит popup-иконки редактирования и удаления строки в таблице.
 * @param {HTMLTableSectionElement} tableBody
 * @param {string} tableCellsSelector
 */
function appendActionsToList(listSelector) {
    let _listItems = document.querySelector(listSelector).querySelectorAll("li");
    for (let _item of _listItems) {
        let actions = document.createElement("div");
        actions.classList.add("actions");
        let editIcon = document.createElement("img");
        editIcon.src = "/icons/edit-box.svg";
        editIcon.classList.add("edit-icon", "action-button");
        editIcon.height = _item.scrollHeight - 21;
        let deleteIcon = document.createElement("img");
        deleteIcon.src = "/icons/cross-symbol.svg";
        deleteIcon.classList.add("delete-icon", "action-button");
        deleteIcon.height = _item.scrollHeight - 21;
        actions.appendChild(editIcon);
        actions.appendChild(deleteIcon);
        _item.appendChild(actions);
    }
}