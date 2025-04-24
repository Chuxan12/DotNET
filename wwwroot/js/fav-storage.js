// js/fav-storage.js
const favKey = 'anime-favorites';

export function getFavSet() {
  try { return new Set(JSON.parse(localStorage.getItem(favKey)) || []); }
  catch { return new Set(); }
}

export function saveFavSet(set) {
  localStorage.setItem(favKey, JSON.stringify([...set]));
  updateFavCounter(set.size);
}

export function updateFavCounter(count = getFavSet().size) {
  const el = document.querySelector('[aria-label="Избранное"] .counter');
  if (el) el.textContent = count;
}
if (typeof window !== 'undefined') {
    window.getFavSet       = getFavSet;
    window.saveFavSet      = saveFavSet;
    window.updateFavCounter = updateFavCounter;
  }
  (function initFavCounter() {
    /* 1. первый рендер после того, как DOM готов */
    if (document.readyState === 'loading') {
      document.addEventListener('DOMContentLoaded', updateFavCounter);
    } else {
      updateFavCounter();
    }
  
    /* 2. обновление во всех остальных вкладках */
    window.addEventListener('storage', e => {
      if (e.key === favKey) updateFavCounter();
    });
  })();