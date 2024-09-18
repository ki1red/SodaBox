document.addEventListener("DOMContentLoaded", function () {
    updateCurrentAmount();
    updatePayButton();
});

function updateCoin(denomination, change) {
    const coinSumElement = document.getElementById('coin-sum-' + denomination);
    const coinInputElement = document.getElementById('coin-input-' + denomination);
    let coinCount = parseInt(coinInputElement.value, 10);
    // Обновляем количество монет
    coinCount += change;
    if (coinCount < 0) {
        coinCount = 0;
    }

    // Обновляем отображение количества монет и суммы
    coinSumElement.textContent = (denomination * coinCount);
    coinInputElement.value = coinCount; // Обновляем значение текстового поля

    updateCurrentAmount();
}

function updateCoinFromInput(denomination) {
    const coinInputElement = document.getElementById('coin-input-' + denomination);
    const coinSumElement = document.getElementById('coin-sum-' + denomination);
    let coinCount = parseInt(coinInputElement.value, 10);

    //if (isNaN(coinCount) || coinCount < 0) {
    //    coinCount = 0;
    //}

    // Обновляем отображение количества монет и суммы
    coinInputElement.textContent = coinCount;
    coinSumElement.textContent = (denomination * coinCount);

    updateCurrentAmount();
}

// Функция обновления текущей суммы
function updateCurrentAmount() {
    let sum = 0;

    // Проходим по всем строкам таблицы и суммируем значения из столбца "Сумма"
    document.querySelectorAll('tbody tr').forEach(row => {
        const coinSumElement = row.querySelector('[id^="coin-sum-"]');
        if (coinSumElement) {
            const coinSum = parseFloat(coinSumElement.textContent);
            if (!isNaN(coinSum)) {
                sum += coinSum;
            }
        }
    });

    // Обновляем переменную currentAmount
    currentAmount = sum;
    console.log(currentAmount);
    // Обновляем отображение на странице
    document.getElementById('currentAmount').textContent = currentAmount.toFixed(2);

    // Обновляем кнопку "Оплатить"
    updatePayButton();
}

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

document.querySelectorAll('input[type="number"]').forEach(input => {
    input.addEventListener('input', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });
});