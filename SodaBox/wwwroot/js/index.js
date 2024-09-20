// index.js

function updateMinPriceValue(value) {
    document.getElementById('minPriceValue').innerText = value;
    filterDrinks();  // Обновление списка напитков при изменении
}

function updateMaxPriceValue(value) {
    document.getElementById('maxPriceValue').innerText = value;
    filterDrinks();  // Обновление списка напитков при изменении
}

async function filterDrinks() {
    const brandId = document.getElementById('brandFilter').value;
    const minPrice = document.getElementById('minPrice').value;
    const maxPrice = document.getElementById('maxPrice').value;

    try {
        const response = await fetch(`/Store/FilterDrinks?brandId=${brandId}&minPrice=${minPrice}&maxPrice=${maxPrice}`);
        const data = await response.text();
        document.getElementById('drinksContainer').innerHTML = data;
        await checkSelectedDrinks();
    } catch (error) {
        console.error('Error:', error);
    }
}

async function checkSelectedDrinks() {
    try {
        const response = await fetch('/Store/GetCartJson');
        const cart = await response.json();
        const selectedDrinks = cart.map(item => item.drink.id);

        selectedDrinks.forEach(drinkId => {
            const button = document.querySelector(`button[data-drink-id='${drinkId}']`);
            if (button) {
                button.classList.remove('selectdrink-available');
                button.classList.add('selectdrink-selected');
                button.querySelector('.button-text').textContent = 'Выбрано';
            }
        });

        updateBucketButton();  // Обновляем кнопку корзины после загрузки корзины
    } catch (error) {
        console.error('Ошибка:', error);
    }
}

async function addToCart(drinkId) {
    try {
        const response = await fetch("/Store/AddOrRemoveToCart", {
            method: "PUT",
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body: `drinkId=${drinkId}`
        });

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
            await checkSelectedDrinks();
        } else {
            console.error("Ошибка при добавлении/удалении напитка");
        }
    } catch (error) {
        console.error('Ошибка:', error);
    }
}

async function updateBucketButton() {
    try {
        const response = await fetch('/Store/GetCartJson');
        const cart = await response.json();
        var selectedCount = cart.length;
        var cartButton = document.getElementById('cartButton');

        if (selectedCount === 0) {
            cartButton.classList.remove('bucket-selected');
            cartButton.classList.add('bucket-disabled');
            cartButton.querySelector('.button-text').textContent = 'Не выбрано';
            cartButton.dataset.disabled = 'true';
        } else {
            cartButton.classList.remove('bucket-disabled');
            cartButton.classList.add('bucket-selected');
            cartButton.querySelector('.button-text').textContent = `Выбрано: ${selectedCount}`;
            cartButton.dataset.disabled = 'false';
        }
    } catch (error) {
        console.error('Ошибка:', error);
    }
}

function handleCartButtonClick() {
    const cartButton = document.getElementById('cartButton');
    if (cartButton.dataset.disabled === 'false') {
        window.location.href = '/Bucket/Bucket';
    }
}

// Инициализация слайдера
document.addEventListener("DOMContentLoaded", async () => {
    var slider = document.getElementById('slider');

    if (slider) {
        noUiSlider.create(slider, {
            start: [0, 500],
            connect: true,
            range: {
                'min': 0,
                'max': 500
            },
            step: 1, // Шаг изменения
            tooltips: [true, true], // Показывать текущие значения на ручках
            format: {
                to: value => Math.round(value),
                from: value => Math.round(value)
            }
        });

        // Обработчик изменений слайдера
        slider.noUiSlider.on('update', (values, handle) => {
            if (handle === 0) {
                updateMinPriceValue(values[0]);
                document.getElementById('minPrice').value = values[0];
            } else {
                updateMaxPriceValue(values[1]);
                document.getElementById('maxPrice').value = values[1];
            }
        });

        // Сохраняем значения слайдера в localStorage при изменении
        slider.noUiSlider.on('change', (values) => {
            localStorage.setItem('minPrice', values[0]);
            localStorage.setItem('maxPrice', values[1]);
        });

        // Восстановление значений слайдера при загрузке страницы
        var minPrice = localStorage.getItem('minPrice') || 0;
        var maxPrice = localStorage.getItem('maxPrice') || 500;
        slider.noUiSlider.set([minPrice, maxPrice]);

        // Обновление отображаемых значений
        updateMinPriceValue(minPrice);
        updateMaxPriceValue(maxPrice);
        await filterDrinks();
    } else {
        console.error('Слайдер не найден на странице');
    }

    // Инициализация других функций
    await checkSelectedDrinks();
    await updateBucketButton();  // Обновляем кнопку корзины при загрузке страницы
});
