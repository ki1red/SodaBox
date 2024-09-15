// index.js

function updateMinPriceValue(value) {
    document.getElementById('minPriceValue').innerText = value;
    filterDrinks();  // Обновление списка напитков при изменении
}

function updateMaxPriceValue(value) {
    document.getElementById('maxPriceValue').innerText = value;
    filterDrinks();  // Обновление списка напитков при изменении
}

function filterDrinks() {
    var minPrice = document.getElementById('minPrice').value;
    var maxPrice = document.getElementById('maxPrice').value;
    var brandId = document.getElementById('brandFilter').value;

    var url = `/Store/FilterDrinks?minPrice=${minPrice}&maxPrice=${maxPrice}&brandId=${brandId}`;

    fetch(url)
        .then(response => response.text())
        .then(data => {
            document.getElementById('drinksContainer').innerHTML = data;
            checkSelectedDrinks();
            updateBucketButton();  // Обновляем кнопку корзины после фильтрации
        })
        .catch(error => console.error('Ошибка:', error));
}

function checkSelectedDrinks() {
    fetch('/Store/GetCartJson')
        .then(response => response.json())
        .then(cart => {
            var selectedDrinks = cart.map(item => item.drink.id);

            selectedDrinks.forEach(function (drinkId) {
                var button = document.querySelector(`button[data-drink-id='${drinkId}']`);
                if (button) {
                    button.classList.remove('selectdrink-available');
                    button.classList.add('selectdrink-selected');
                    button.querySelector('.button-text').textContent = 'Выбрано';
                }
            });

            updateBucketButton();  // Обновляем кнопку корзины после загрузки корзины
        })
        .catch(error => console.error('Ошибка:', error));
}

function addToCart(drinkId) {
    fetch("/Store/AddOrRemoveToCart", {
        method: "POST",
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: `drinkId=${drinkId}`
    }).then(response => {
        if (response.ok) {
            var button = document.querySelector(`button[data-drink-id='${drinkId}']`);
            if (button) {
                // Переключаем состояние кнопки
                if (button.classList.contains('selectdrink-available')) {
                    button.classList.remove('selectdrink-available');
                    button.classList.add('selectdrink-selected');
                    button.querySelector('.button-text').textContent = 'Выбрано';
                } else {
                    button.classList.remove('selectdrink-selected');
                    button.classList.add('selectdrink-available');
                    button.querySelector('.button-text').textContent = 'Выбрать';
                }
            }

            updateBucketButton();  // Обновляем кнопку корзины после изменения корзины
        } else {
            console.error("Ошибка при добавлении/удалении напитка");
        }
    });
}


function updateBucketButton() {
    fetch('/Store/GetCartJson')
        .then(response => response.json())
        .then(cart => {
            var selectedCount = cart.length;
            var cartButton = document.getElementById('cartButton');

            if (selectedCount === 0) {
                cartButton.classList.remove('button-selected');
                cartButton.classList.add('button-disabled');
                cartButton.querySelector('.button-text').textContent = 'Не выбрано';
                console.log(`Бакет опустел`);
            } else {
                cartButton.classList.remove('button-disabled');
                cartButton.classList.add('button-selected');
                cartButton.querySelector('.button-text').textContent = `Выбрано: ${selectedCount}`;
                console.log(`Бакет содержит: ${selectedCount}`);
            }
        })
        .catch(error => console.error('Ошибка:', error));
}

document.addEventListener("DOMContentLoaded", () => {
    checkSelectedDrinks();
    updateBucketButton();  // Обновляем кнопку корзины при загрузке страницы
});
