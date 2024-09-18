document.addEventListener("DOMContentLoaded", function () {
    const amount = parseInt(document.getElementById("changeAmount").getAttribute("data-amount"));
    console.log(amount);
    const denominations = [10, 5, 2, 1]; // Номиналы монет
    const changeList = document.getElementById("changeList");

    const change = calculateChange(amount, denominations);
    displayChange(change);
});

function calculateChange(amount, denominations) {
    const result = {};

    for (let denom of denominations) {
        if (amount >= denom) {
            result[denom] = Math.floor(amount / denom);
            amount %= denom;
        }
    }
    return result;
}

function displayChange(change) {
    changeList.innerHTML = ''; // Очистить контейнер

    for (let denom of Object.keys(change)) {
        const listItem = document.createElement("li");
        listItem.textContent = `${denom} руб. - ${change[denom]} штуки`;
        changeList.appendChild(listItem);
    }
}