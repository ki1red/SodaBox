document.addEventListener("DOMContentLoaded", function () {
    updateCurrentAmount();
});

function updateCoin(denomination, increment) {
    const coinCountElement = document.getElementById(`coin-count-${denomination}`);
    let currentCount = parseInt(coinCountElement.textContent) || 0;
    currentCount += increment;

    if (currentCount >= 0) {
        coinCountElement.textContent = currentCount;
        updateCurrentAmount();
    }
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
    checkCanPay(total);
}

function checkCanPay(currentAmount) {
    const totalAmount = parseInt(document.querySelector("#totalAmount").textContent);
    const payButton = document.getElementById("payButton");

    if (currentAmount >= totalAmount) {
        payButton.disabled = false;
        payButton.href = '/Payment/Change?amount=' + (currentAmount - totalAmount);
    } else {
        payButton.disabled = true;
        payButton.removeAttribute('href');
    }
}
