document.addEventListener('DOMContentLoaded', function () {
    loadProducts(1);
    fetch('/api/cart/count', {
        credentials: 'include'
    })
        .then(response => response.json())
        .then(data => updateCartCounter(data.totalItems))
        .catch(() => updateCartCounter(0))
});

const searchInput = document.querySelector('.sidebar-search');
const suggestionsContainer = document.getElementById('autocompleteSuggestions');
let searchTimer;

// Обработчик ввода для поисковых подсказок
searchInput.addEventListener('input', function () {
    clearTimeout(searchTimer);
    const searchTerm = this.value.trim();

    if (searchTerm.length === 0) {
        hideSuggestions();
        return;
    }

    searchTimer = setTimeout(() => fetchSuggestions(searchTerm), 300);
});

// Получение подсказок с сервера
function fetchSuggestions(searchTerm) {
    fetch(`/api/Anime/suggestions?searchTerm=${encodeURIComponent(searchTerm)}`, {
        credentials: 'include' // Исправлено: параметры должны быть внутри объекта
    })
        .then(response => response.json())
        .then(suggestions => showSuggestions(suggestions))
        .catch(error => console.error('Error:', error));
}

// Отображение подсказок
function showSuggestions(suggestions) {
    suggestionsContainer.innerHTML = '';

    if (suggestions.length === 0) {
        hideSuggestions();
        return;
    }

    suggestions.forEach(suggestion => {
        const div = document.createElement('div');
        div.className = 'suggestion-item';
        div.innerHTML = `
        <div class="suggestion-name">${suggestion.name}</div>
        `;
        div.addEventListener('click', () => {
            window.location.href = `/Anime.html?AnimeId=${suggestion.id}`;
        });
        suggestionsContainer.appendChild(div);
    });

    suggestionsContainer.style.display = 'block';
}

// Скрытие подсказок
function hideSuggestions() {
    suggestionsContainer.style.display = 'none';
}

// Закрытие подсказок при клике вне области
document.addEventListener('click', (e) => {
    if (!e.target.closest('.sidebar-search-container')) {
        hideSuggestions();
    }
});

function updateCartCounter(count) {
    const cartLink = document.querySelector('a[href="cart.html"]');
    cartLink.innerHTML = count > 0
        ? `Избранное (${count})`
        : 'Избранное';
}

function getSelectedValues(selector) {
    return Array.from(document.querySelectorAll(selector))
        .filter(checkbox => checkbox.checked)
        .map(checkbox => checkbox.value || checkbox.nextSibling.textContent.trim());
}

function getFilters() {
    const minPriceInput = document.getElementById('price-from');
    const maxPriceInput = document.getElementById('price-to');

    const minPrice = minPriceInput.value ? Number(minPriceInput.value) : null;
    const maxPrice = maxPriceInput.value ? Number(maxPriceInput.value) : null;

    if (minPrice !== null && maxPrice !== null && minPrice > maxPrice) {
        alert('Минимальная цена не может быть больше максимальной!');
        minPriceInput.value = maxPrice;
        maxPriceInput.value = minPrice;
        return getFilters();
    }

    return {
        connectionType: getSelectedValues('.sidebar-filter-category:nth-child(2) input[type="checkbox"]:checked'),
        wearingStyle: getSelectedValues('.sidebar-filter-category:nth-child(3) input[type="checkbox"]:checked'),
        brands: getSelectedValues('#brand-filters input[type="checkbox"]:checked'),
        minPrice: minPrice,
        maxPrice: maxPrice,
        searchTerm: document.querySelector('.sidebar-search').value.trim()
    };
}

// Обработчик применения фильтров
document.getElementById('apply-filters').addEventListener('click', function () {
    loadProducts(1);
});

function loadProducts(page) {
    const filters = getFilters();

    const params = {
        page: page,
        searchTerm: filters.searchTerm || '',
        connectionType: filters.connectionType.join(','),
        wearingStyle: filters.wearingStyle.join(','),
        brand: filters.brands.join(','),
        minPrice: filters.minPrice || '',
        maxPrice: filters.maxPrice || ''
    };

    Object.keys(params).forEach(key => {
        if (params[key] === '' || params[key] === null)
            delete params[key];
    });

    const queryParams = new URLSearchParams(params).toString();

    fetch(`/api/Anime?${queryParams}`, {
        credentials: 'include'
    })
        .then(response => {
            if (!response.ok) {
                return response.text().then(text => { throw new Error(text) });
            }
            return response.json();
        })
        .then(data => {
            updateProductList(data.Anime);
            generatePagination(data.totalPages, page);
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Ошибка: ' + error.message);
        });
}

function updateProductList(Anime) {
    const container = document.getElementById('productContainer');

    if (Anime.length === 0) {
        container.innerHTML = `
            <div class="no-results">
                <p>По вашему запросу ничего не найдено</p>
            </div>
        `;
    } else {
        container.innerHTML = Anime.map(Anime => `
            <div class="Anime-item" data-Anime-id="${Anime.AnimeId}">
                <img src="${Anime.imageURL}" alt="${Anime.name}">
                <p>${Anime.name}</p>
                <p>Цена: ${Anime.price} ₽</p>
                <div class="Anime-actions">
                    <button class="add-to-cart">Добавить в корзину</button>
                    <a href="Anime.html?AnimeId=${Anime.AnimeId}" class="details-link">Подробнее</a>
                </div>
            </div>
        `).join('');
    }
}

// Обработчик добавления в корзину
document.getElementById('productContainer').addEventListener('click', function (e) {
    if (e.target.classList.contains('add-to-cart')) {
        const productItem = e.target.closest('.Anime-item');
        const AnimeId = productItem.dataset.AnimeId;

        console.log('Добавление товара ID:', AnimeId);

        fetch('/api/cart/add', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            credentials: 'include',
            body: JSON.stringify({ AnimeId: parseInt(AnimeId) })
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(text => {
                        throw new Error(`Ошибка: ${response.status} (${text})`);
                    });
                }
                return response.json();
            })
            .then(data => {
                console.log('Ответ сервера:', data);
                updateCartCounter(data.totalItems);
            })
            .catch(error => {
                console.error('Ошибка:', error);
                alert(error.message);
            });
    }
});

// Пагинация
function generatePagination(totalPages, currentPage) {
    const pagination = document.getElementById('pagination');
    pagination.innerHTML = '';
    for (let i = 1; i <= totalPages; i++) {
        const btn = document.createElement('button');
        btn.classList.add('page-btn');
        btn.innerText = i;
        if (i === currentPage) {
            btn.disabled = true;
        }
        btn.addEventListener('click', () => {
            loadProducts(i);
        });
        pagination.appendChild(btn);
    }
}