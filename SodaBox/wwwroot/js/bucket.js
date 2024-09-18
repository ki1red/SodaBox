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
