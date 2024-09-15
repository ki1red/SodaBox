// bucket.js

function updateCart(drinkId, quantity) {
    fetch("/Store/UpdateCart", {
        method: "POST",
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: `drinkId=${drinkId}&quantity=${quantity}`
    }).then(() => {
        location.reload(); // Перезагрузка страницы
    });
}
