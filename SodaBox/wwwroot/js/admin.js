let draggedElement = null;
let drinkOrder = [];

function allowDrop(event) {
    event.preventDefault();
}

function drag(event) {
    draggedElement = event.currentTarget;
    event.dataTransfer.setData("text/html", draggedElement.outerHTML);
    draggedElement.style.opacity = "0.5"; // Прозрачность для визуализации захвата
}

function dragEnd(event) {
    if (draggedElement) {
        draggedElement.style.opacity = "1";
    }
}

function drop(event) {
    event.preventDefault();
    const targetElement = event.target.closest('.drink-item');

    if (draggedElement && targetElement && draggedElement !== targetElement) {
        // Обмен местами элементов в DOM
        const parent = draggedElement.parentNode;
        const draggedId = parseInt(draggedElement.id.replace('drink-', ''), 10);
        const targetId = parseInt(targetElement.id.replace('drink-', ''), 10);

        // Сохранение следующего элемента для правильного перемещения
        const nextSibling = targetElement.nextSibling === draggedElement ? targetElement : targetElement.nextSibling;

        parent.insertBefore(draggedElement, nextSibling);
        parent.insertBefore(targetElement, draggedElement);

        // Обновляем drinkOrder для отправки на сервер
        updateDrinkOrder();
    }
}

function updateDrinkOrder() {
    drinkOrder = Array.from(document.querySelectorAll('.drink-item')).map(item => parseInt(item.id.replace('drink-', ''), 10));
}

function updateSaveButton() {
    let isValid = true;
    document.querySelectorAll('input[type="number"]').forEach(input => {
        if (input.value < input.min) {
            isValid = false;
        }
    });

    const button = document.getElementById('saveButton');
    if (button) {
        if (isValid) {
            button.classList.remove('save-disabled');
            button.classList.add('save-enabled');
            button.disabled = false;
        } else {
            button.classList.remove('save-enabled');
            button.classList.add('save-disabled');
            button.disabled = true;
        }
    } else {
        console.error("Не найдена кнопка сохранения");
    }
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
            // Добавим запрос на обновление позиций напитков
            const responseUpdPositions = await fetch('/Store/UpdateDrinksPositions', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(drinkOrder)
            });

            if (responseUpdPositions.ok) {
                window.location.href = '/Store/Index';
            }
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

// Добавляем события dragend, drop и dragover
document.querySelectorAll('.drink-item').forEach(item => {
    item.addEventListener('dragstart', drag);
    item.addEventListener('dragend', dragEnd);
    item.addEventListener('drop', drop);
    item.addEventListener('dragover', allowDrop);
});
