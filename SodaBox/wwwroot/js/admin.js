function updateSaveButton() {

    let isValid = true;
    document.querySelectorAll('input[type="number"]').forEach(input => {
        if (input.value < input.min) {
            isValid = false;
        }
    });

    var button = document.getElementById('saveButton');
    if (button) {
        if (isValid) {
            console.log("enab");
            button.classList.remove('save-disabled');
            button.classList.add('save-enabled');
            button.disabled = false;
        } else {
            console.log("disab");
            button.classList.remove('save-enabled');
            button.classList.add('save-disabled');
            button.disabled = true;
        }
    } else {
        console.error("Не найдена кнопка сохранения");
    }
    //document.getElementById('saveButton').disabled = !(isValidPrice * isValidNumber);
}

function clickButtonToUpdateQuantity(drinkId, change) {
    const input = document.getElementById(`quantity-${drinkId}`);
    let currentQuantity = parseInt(input.value, 10);
    if (!isNaN(currentQuantity)) {
        currentQuantity += change;
        if (currentQuantity < 0) currentQuantity = 0; // Нельзя устанавливать отрицательное значение
        input.value = currentQuantity;
    }
}

async function saveData() {
    const form = document.getElementById('adminForm');
    const formData = new FormData(form);
    const dataQuantity = {};
    const dataPrice = {};

    formData.forEach((value, key) => {
        if (key.startsWith('drinks-quantity[')) {
            const id = key.match(/\[(\d+)\]/)[1];
            dataQuantity[id] = parseInt(value, 10);
        }
        if (key.startsWith('drinks-price[')) {
            const id = key.match(/\[(\d+)\]/)[1];
            dataPrice[id] = parseInt(value, 10);
        }
    });

    try {
        const responseUpdQuantity = await fetch('/Store/UpdateDrinksStock', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dataQuantity)
        });

        const responseUpdPrice = await fetch('/Store/UpdateDrinksPrice', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dataPrice)
        });

        if (responseUpdQuantity.ok && responseUpdPrice.ok) {
            window.location.href = '/Store/Index';
        }
    } catch (error) {
        console.error("Ошибка: ", error);
    }
}

document.querySelectorAll('input[type="number"]').forEach(input => {
    input.addEventListener('input', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
        updateSaveButton();
    });
});

// Начальная проверка состояния формы
updateSaveButton();
