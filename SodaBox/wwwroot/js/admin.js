function validateForm() {
    let isValid = true;
    document.querySelectorAll('input[type="number"]').forEach(input => {
        if (input.value < 0) {
            isValid = false;
        }
    });
    document.getElementById('saveButton').disabled = !isValid;
}

function updateQuantity(drinkId, change) {
    const input = document.getElementById(`quantity-${drinkId}`);
    let currentQuantity = parseInt(input.value, 10);
    if (!isNaN(currentQuantity)) {
        currentQuantity += change;
        if (currentQuantity < 0) currentQuantity = 0; // Нельзя устанавливать отрицательное значение
        input.value = currentQuantity;
    }
}

function saveQuantities() {
    const form = document.getElementById('adminForm');
    const formData = new FormData(form);
    const data = {};

    formData.forEach((value, key) => {
        if (key.startsWith('drinks[')) {
            const id = key.match(/\[(\d+)\]/)[1];
            data[id] = parseInt(value, 10);
        }
    });

    fetch('/Store/UpdateDrinksStock', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            window.location.href = '/Store/Index';
        })
        .catch(error => {
            console.error('Ошибка:', error);
        });
}

document.querySelectorAll('input[type="number"]').forEach(input => {
    input.addEventListener('input', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
        validateForm();
    });
});

// Начальная проверка состояния формы
validateForm();
