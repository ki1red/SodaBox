document.addEventListener("DOMContentLoaded", function () {
    updateCurrentAmount();
    updatePayButton();
});

function updateCoin(denomination, increment) {
    const coinCountElement = document.getElementById(`coin-count-${denomination}`);
    let currentCount = parseInt(coinCountElement.textContent) || 0;
    currentCount += increment;

    if (currentCount >= 0) {
        coinCountElement.textContent = currentCount;
        updateCurrentAmount();
    }

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

function updateCurrentAmount() {
    let total = 0;
    const coinElements = document.querySelectorAll("span[id^='coin-count-']");
    coinElements.forEach(element => {
        const denomination = parseInt(element.id.replace('coin-count-', ''));
        const count = parseInt(element.textContent) || 0;
        total += denomination * count;
    });

    document.getElementById("currentAmount").textContent = total;
    updatePayButton();
}

//function checkCanPay(currentAmount) {
//    const totalAmount = parseInt(document.querySelector("#totalAmount").textContent);
//    const payButton = document.getElementById("payButton");

//    if (currentAmount >= totalAmount) {
//        payButton.disabled = false;
//        payButton.href = '/Payment/Change';
//    } else {
//        payButton.disabled = true;
//        payButton.removeAttribute('href');
//    }
//}

function handlePayButtonClick() {
    const payButton = document.getElementById('payButton');
    const sum = parseFloat(document.getElementById('currentAmount').textContent);

    if (payButton.dataset.disabled === 'false') {

        fetch('/Payment/Pay', {
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

function updatePayButton() {
    var payButton = document.getElementById('payButton');
    const currentAmount = parseFloat(document.getElementById('currentAmount').textContent);
    if (Number(currentAmount) >= Number(needAmount)) {
        payButton.classList.remove('next-disabled');
        payButton.classList.add('next-enabled');
        payButton.dataset.disabled = 'false';
    } else {
        payButton.classList.remove('next-enabled');
        payButton.classList.add('next-disabled');
        payButton.dataset.disabled = 'true';
    }
}
