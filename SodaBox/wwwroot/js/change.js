document.addEventListener("DOMContentLoaded", function () {
    const amount = parseInt(document.getElementById("changeAmount").getAttribute("data-amount"), 10);
    const denominations = [10, 5, 2, 1]; // Номиналы монет
    const changeContainer = document.getElementById("changeContainer");

    if (amount > 0) {
        displayChange(change);
    }
});

function displayChange(change) {
    const changeContainer = document.getElementById("changeContainer");
    changeContainer.innerHTML = ''; // Очистить контейнер

    for (let denom of Object.keys(change)) {
        const changeItem = document.createElement("div");
        changeItem.className = "change-item";

        // Создание элемента для изображения
        const img = document.createElement("img");
        img.src = `/images/coins/${denom}.png`; // Путь к изображению монеты
        img.alt = `${denom} руб.`;
        img.style.width = '50px'; // Размеры изображения
        img.style.height = '50px';
        img.style.marginRight = '10px';

        // Создание элемента для текста
        const text = document.createElement("span");
        text.textContent = `${denom} руб. - ${change[denom]} шт.`;

        // Добавление изображения и текста в элемент
        changeItem.appendChild(img);
        changeItem.appendChild(text);

        // Добавление элемента в контейнер
        changeContainer.appendChild(changeItem);
    }
}
