/*  «Избранное» — работа с localStorage и счётчиком  */
const favKey = 'anime-favorites';

/* ---------- получение / сохранение множества ID ---------- */
export function getFavSet() {
  try { return new Set(JSON.parse(localStorage.getItem(favKey)) || []); }
  catch { return new Set(); }
}

export function saveFavSet(set) {
  localStorage.setItem(favKey, JSON.stringify([...set]));
  /* показываем актуальный размер локального списка */
  updateFavCounter(set.size);
}

/* ---------- сам счётчик ---------- */
export function updateFavCounter(count = getFavSet().size) {
  const el = document.querySelector('[aria-label="Избранное"] .counter');
  if (!el) return;
  el.textContent = count > 0 ? count : '';   // «0» прячем
}

/* ---------- экспорт во вкладках ---------- */
if (typeof window !== 'undefined') {
  window.getFavSet        = getFavSet;
  window.saveFavSet       = saveFavSet;
  window.updateFavCounter = updateFavCounter;
}

/* ---------- инициализация счётчика ---------- */
(function initFavCounter() {
  /* 1. первый рендер после готовности DOM */
  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', updateFavCounter);
  } else {
    updateFavCounter();
  }

  /* 2. синхронизация между вкладками */
  window.addEventListener('storage', (e) => {
    if (e.key === favKey) updateFavCounter();
  });
})();