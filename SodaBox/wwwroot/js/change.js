document.addEventListener("DOMContentLoaded", function () {
    const changeAmount = parseInt(document.getElementById("changeAmount").getAttribute("data-amount"), 10);
    const changeCoinsJson = document.getElementById("changeCoins").getAttribute("data-coins");
    const changeCoins = JSON.parse(changeCoinsJson); // Преобразуем строку JSON в объект
    const changeContainer = document.getElementById("changeContainer");

    if (changeAmount > 0 && changeCoins) {
        displayChange(changeCoins);
    }
});

function displayChange(coins) {
    const changeContainer = document.getElementById("changeContainer");
    changeContainer.innerHTML = ''; // Очистить контейнер

    coins.forEach(coin => {
        const changeItem = document.createElement("div");
        changeItem.className = "change-item";

        // Создание элемента для изображения
        const img = document.createElement("img");
        img.src = `/${coin.imagePath}`; // Путь к изображению монеты из модели
        img.alt = `${coin.price} руб.`;
        img.style.width = '50px';
        img.style.height = '50px';
        img.style.marginRight = '10px';

        // Создание элемента для текста
        const text = document.createElement("span");
        text.textContent = `${coin.price} руб. - ${coin.quantity} шт.`; // Используем данные о номинале и количестве

        // Добавление изображения и текста в элемент
        changeItem.appendChild(img);
        changeItem.appendChild(text);

        // Добавление элемента в контейнер
        changeContainer.appendChild(changeItem);
    });
}
