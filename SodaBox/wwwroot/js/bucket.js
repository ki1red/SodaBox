// bucket.js

function updateCart(drinkId, quantity) {
    fetch("/Store/ReloadCart", {
        method: "PUT",
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: `drinkId=${drinkId}&quantity=${quantity}`
    }).then(() => {
        location.reload(); // Перезагрузка страницы
    });
}
