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

/**
 * Отправляет POST-запрос в апи. Возвращает промис с ответом сервера.
 * @param {string} endpoint
 * @param {string} jsonBody
 * @returns {Promise<Response>}
 */
function _post(endpoint, jsonBody) {
    let promise = fetch(`${BASE_URL}/api/${endpoint}`, {
        method: 'POST',
        body: jsonBody,
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        }
    });
    return promise;
}

function _put(endpoint, jsonBody) {
    let promise = fetch(`${BASE_URL}/api/${endpoint}`, {
        method: 'PUT',
        body: jsonBody,
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        }
    });
    return promise;
}

/**
 * Извлекает все сообщения об ошибках из ответа сервера как одну строку.
 * @param {{ errors: [''] }} apiErrorResponse
 */
function getApiErrorsAsString(apiErrorResponse) {
    let result = "";
    for (let error of apiErrorResponse.errors) {
        result += `${error} `;
    }

    return result;
}