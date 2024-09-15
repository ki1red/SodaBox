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
        })
        .catch(error => console.error('Ошибка:', error));
}

function checkSelectedDrinks() {
    fetch('/Store/GetCart')
        .then(response => response.json())
        .then(cart => {
            var selectedDrinks = cart.map(item => item.drink.id);

            selectedDrinks.forEach(function (drinkId) {
                var button = document.querySelector(`button[data-drink-id='${drinkId}']`);
                if (button) {
                    button.classList.remove('available');
                    button.classList.add('selected');
                    button.querySelector('.button-text').textContent = 'Выбрано';
                    button.disabled = true;
                }
            });
        })
        .catch(error => console.error('Ошибка:', error));
}

document.addEventListener("DOMContentLoaded", checkSelectedDrinks);

function addToCart(drinkId) {
    fetch("/Store/AddToCart", {
        method: "POST",
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: `drinkId=${drinkId}`
    }).then(response => {
        if (response.ok) {
            console.log("Напиток добавлен в корзину");
            var button = document.querySelector(`button[data-drink-id='${drinkId}']`);
            if (button) {
                if (button.classList.contains('available')) {
                    button.classList.remove('available');
                    button.classList.add('selected');
                    button.querySelector('.button-text').textContent = 'Выбрано';
                } else {
                    button.classList.remove('selected');
                    button.classList.add('available');
                    button.querySelector('.button-text').textContent = 'Выбрать';
                }
            }
        } else {
            console.error("Ошибка при добавлении напитка");
        }
    });
}
