document.addEventListener('DOMContentLoaded', function () {
    updatePaymentButton();
});

function updateCart(drinkId, quantity) {
    fetch(`/Bucket/ReloadCart?drinkId=${drinkId}&quantity=${quantity}`, {
        method: 'PUT'
    }).then(response => {
        if (response.ok) {
            location.reload(); // Перезагружаем страницу для обновления данных
            updatePaymentButton();
        } else {
            alert('Failed to update cart');
        }
    }).catch(error => {
        console.error('Error:', error);
    });
}

function updateCartFromInput(drinkId) {
    const inputElement = document.getElementById('cart-input-' + drinkId);
    let quantity = parseInt(inputElement.value, 10);

    if (isNaN(quantity) || quantity < 1) {
        quantity = 1; // Минимальное значение — 1
        inputElement.value = 1;
    } else if (quantity > maxQuantity) {
        quantity = maxQuantity; // Максимальное значение — количество напитков на складе
        inputElement.value = maxQuantity; // Корректируем значение в поле
    }

    updateCart(drinkId, quantity); // Обновляем корзину с новым количеством
}

function handlePayButtonClick() {
    const payButton = document.getElementById('payButton');

    if (payButton.dataset.disabled === 'false') {

        fetch('/Bucket/Payment', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(sum)
        })
            .then(response => response.json())
            .then(result => {
                if (result.redirectUrl) {
                    window.location.href = result.redirectUrl;
                } else {
                    console.error('Ошибка:', result);
                }
            })
            .catch(error => {
                console.error('Ошибка:', error);
            });
    }
}

function updatePaymentButton() {
    fetch('/Store/GetCartJson')
        .then(response => response.json())
        .then(cart => {
            var selectedCount = cart.length;
            var payButton = document.getElementById('payButton');

            if (selectedCount === 0) {
                payButton.classList.remove('next-enabled');
                payButton.classList.add('next-disabled');
                payButton.dataset.disabled = 'true';
            } else {
                payButton.classList.remove('next-disabled');
                payButton.classList.add('next-enabled');
                payButton.dataset.disabled = 'false';
            }
        })
        .catch(error => console.error('Ошибка:', error));
}

function deleteFromCart(drinkId) {
    fetch(`/Bucket/DeleteItem?drinkId=${drinkId}`, {
        method: 'PUT'
    }).then(response => {
        if (response.ok) {
            location.reload(); // Перезагружаем страницу для обновления данных
        } else {
            alert('Failed to remove item from cart');
        }
    }).catch(error => {
        console.error('Error:', error);
    });
}

document.querySelectorAll('input[type="number"]').forEach(input => {
    input.addEventListener('input', function () {
        // Удаляем любые нечисловые символы, кроме цифр
        this.value = this.value.replace(/[^0-9]/g, '');

        // Запрещаем ввод значения меньше 1
        if (this.value === '' || parseInt(this.value, 10) < 1) {
            this.value = 1;
        }
    });
});