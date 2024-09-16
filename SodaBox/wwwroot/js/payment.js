function changeCoins(denomination, change) {
    const coinElement = document.getElementById(`coin${denomination}`);
    const currentAmountElement = document.getElementById('currentAmount');
    const changeElement = document.getElementById('change');
    const payButton = document.getElementById('payButton');

    let currentCoins = parseInt(coinElement.textContent);
    currentCoins += change;
    coinElement.textContent = currentCoins;

    const totalAmount = parseFloat(document.getElementById('totalAmount').textContent);
    const currentAmount = Array.from(document.querySelectorAll('[id^=coin]'))
        .reduce((acc, el) => acc + parseInt(el.textContent) * parseInt(el.id.replace('coin', '')), 0);

    currentAmountElement.textContent = currentAmount;
    changeElement.textContent = currentAmount - totalAmount;

    payButton.disabled = currentAmount < totalAmount;
}
