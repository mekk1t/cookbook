const BASE_URL = "https://localhost:5001"

/**
 * Отправляет запрос в апи по эндпоинту. Возвращает промис с ответом сервера в формате JSON.
 * @param {string} endpoint
 * @returns {Promise}
 */
function _fetch(endpoint) {
    let promise = fetch(`${BASE_URL}/api/${endpoint}`).then(response => response.json());
    return promise;
}