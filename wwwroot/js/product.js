document.addEventListener('DOMContentLoaded', function () {
    const AnimeId = getProductIdFromURL();
    
    if (!AnimeId) {
        showError('Не указан идентификатор товара');
        return;
    }
    
    console.log('Загрузка данных для товара ID:', AnimeId);
    loadProductData(AnimeId);
});

// Получение ID товара из URL
function getProductIdFromURL() {
    const params = new URLSearchParams(window.location.search);
    const AnimeId = params.get('AnimeId');
    
    if (!AnimeId) {
        console.error('Anime ID не найден в URL');
        return null;
    }
    
    console.log('Получен Anime ID из URL:', AnimeId);
    return parseInt(AnimeId);
}

// Загрузка данных товара
function loadProductData(AnimeId) {
    showLoader();
    
    fetch(`/api/Anime/${AnimeId}`, {
        credentials: 'include'
    })
    .then(response => {
        console.log('Статус ответа:', response.status);
        
        if (!response.ok) {
            return response.json().then(err => {
                throw new Error(err.error || 'Ошибка сервера');
            });
        }
        return response.json();
    })
    .then(Anime => {
        console.log('Получены данные товара:', Anime);
        updateProductPage(Anime);
    })
    .catch(error => {
        console.error('Ошибка загрузки:', error);
        showError(error.message);
    })
    .finally(() => {
        hideLoader();
    });
}

// Обновление страницы товара
function updateProductPage(Anime) {
    try {
        // Основная информация
        document.title = `${Anime.name} | United Audio`;
        setElementContent('.Anime-title', Anime.name);
        setElementContent('.Anime-price', `${Anime.price.toLocaleString()} ₽`);
        setImageSource('.Anime-main-image', Anime.imageURL);

        // Описание товара
        const descriptionContainer = document.querySelector('.Anime-description');
        if (Anime.description) {
            descriptionContainer.innerHTML = Anime.description;
        } else {
            descriptionContainer.innerHTML = '<p class="no-description">Описание товара отсутствует</p>';
        }

        // Обновление характеристик
        setElementContent('.description-title', `Технические характеристики ${Anime.name}`);
        updateSpecificationTable(Anime);

    } catch (error) {
        console.error('Ошибка обновления страницы:', error);
        showError('Ошибка отображения данных');
    }
}

// Обновление таблицы характеристик
function updateSpecificationTable(Anime) {
    const specs = {
        'Бренд': 'Sony',
        'Тип подключения': 'Беспроводные',
        'Способ ношения': 'Накладные',
        'Импеданс': Anime.impedance ? `${Anime.impedance} Ом` : '38',
        'Вес': Anime.weight ? `${Anime.weight} г` : '250'
    };

    const tbody = document.querySelector('.Anime-characteristics tbody');
    tbody.innerHTML = '';

    for (const [key, value] of Object.entries(specs)) {
        tbody.innerHTML += `
            <tr>
                <td>${key}</td>
                <td>${value}</td>
            </tr>
        `;
    }
}

// Обработчик добавления в корзину
document.querySelector('.Anime-submit-btn').addEventListener('click', function() {
    const AnimeId = getProductIdFromURL();
    
    if (!AnimeId) {
        showError('Не удалось определить товар');
        return;
    }

    addToCart(AnimeId);
});

// Функция добавления в корзину
function addToCart(AnimeId) {
    showLoader();
    
    fetch('/api/cart/add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
        body: JSON.stringify({ AnimeId: AnimeId })
    })
    .then(response => {
        if (!response.ok) {
            return response.json().then(err => {
                throw new Error(err.error || 'Ошибка добавления');
            });
        }
        return response.json();
    })
    .then(data => {
        showTempAlert('Товар успешно добавлен в корзину!');
        updateCartCounter(data.totalItems);
    })
    .catch(error => {
        console.error('Ошибка:', error);
        showTempAlert(error.message, true);
    })
    .finally(() => {
        hideLoader();
    });
}

// Вспомогательные функции
function setElementContent(selector, content) {
    const element = document.querySelector(selector);
    if (element) element.textContent = content;
}

function setImageSource(selector, src) {
    const img = document.querySelector(selector);
    if (img) {
        img.src = src;
        img.onerror = () => {
            img.src = 'images/placeholder.jpg';
            console.warn('Изображение не найдено, установлен заглушка');
        };
    }
}

function showLoader() {
    document.getElementById('loader').style.display = 'block';
}

function hideLoader() {
    document.getElementById('loader').style.display = 'none';
}

function showTempAlert(message, isError = false) {
    const alert = document.createElement('div');
    alert.className = `temp-alert ${isError ? 'error' : 'success'}`;
    alert.textContent = message;
    
    document.body.appendChild(alert);
    setTimeout(() => alert.remove(), 3000);
}

function showError(message) {
    document.querySelector('main').innerHTML = `
        <div class="error-container">
            <h2>Ошибка</h2>
            <p>${message}</p>
            <a href="catalog.html" class="back-link">Вернуться в каталог</a>
        </div>
    `;
}