// FrontEnd/js/dashboard.js
import { api, setApiBase, getApiBase, authStore, DEFAULT_BASE } from './api.js';

const filmListEl = document.querySelector('#film-list');
const rentalListEl = document.querySelector('#rental-list');
const statusEl = document.querySelector('#dashboard-status');
const greetingEl = document.querySelector('#greeting');
const logoutBtn = document.querySelector('#logout-btn');
const apiForm = document.querySelector('#api-form');
const apiBaseInput = document.querySelector('#api-base');
const apiResetBtn = document.querySelector('#api-reset');

let films = [];
let rentals = [];

function ensureAuth() {
  if (!authStore.token) {
    window.location.replace('login.html');
  }
}

function studioId() {
  const profile = authStore.profile;
  const filmStudio = profile?.filmStudio || profile?.FilmStudio;
  return filmStudio?.filmStudioId ?? filmStudio?.FilmStudioId ?? filmStudio?.filmstudioId;
}

function setStatus(text, tone = 'info') {
  if (!statusEl) return;
  statusEl.textContent = text;
  statusEl.dataset.tone = tone;
}

function renderGreeting() {
  const profile = authStore.profile;
  if (!greetingEl || !profile) return;
  const name = profile.userName || 'filmstudio';
  const studioName = profile.filmStudio?.studioName || profile.FilmStudio?.StudioName;
  greetingEl.textContent = studioName ? `${studioName} (${name})` : name;
}

function renderFilms() {
  if (!filmListEl) return;
  if (!films.length) {
    filmListEl.innerHTML = '<p class="muted">Inga filmer kunde hämtas.</p>';
    return;
  }

  const currentStudioId = studioId();
  const rentedIds = new Set(rentals.map((r) => r.filmId || r.FilmId));

  filmListEl.innerHTML = films
    .map((film) => {
      const available = Array.isArray(film.filmCopies)
        ? film.filmCopies.filter((c) => !c.loanedByFilmStudioId).length
        : null;
      const alreadyRented = rentedIds.has(film.filmId || film.FilmId);
      const disableReason = !currentStudioId
        ? 'Ingen studio kopplad'
        : available !== null && available <= 0
          ? 'Inga exemplar kvar'
          : alreadyRented
            ? 'Redan lånad'
            : '';

      return `
        <article class="card">
          <header>
            <p class="eyebrow">Genre: ${film.genre || 'Okänd'}</p>
            <h3>${film.title}</h3>
          </header>
          <p class="muted">ID: ${film.filmId}</p>
          ${available !== null ? `<p class="muted">Tillgängliga exemplar: ${available}</p>` : ''}
          <button class="primary" data-action="rent" data-film-id="${film.filmId}" ${disableReason ? 'disabled' : ''}>
            ${disableReason || 'Låna filmen'}
          </button>
        </article>
      `;
    })
    .join('');
}

function renderRentals() {
  if (!rentalListEl) return;
  if (!rentals.length) {
    rentalListEl.innerHTML = '<p class="muted">Inga aktiva lån.</p>';
    return;
  }

  const titleById = new Map(films.map((f) => [f.filmId || f.FilmId, f.title]));

  rentalListEl.innerHTML = rentals
    .map((copy) => {
      const filmId = copy.filmId || copy.FilmId;
      const title = titleById.get(filmId) || `Film #${filmId}`;
      return `
        <article class="card subtle">
          <header>
            <p class="eyebrow">Exemplar ${copy.filmCopyId || copy.FilmCopyId}</p>
            <h3>${title}</h3>
          </header>
          <p class="muted">Film-ID: ${filmId}</p>
          <button class="ghost" data-action="return" data-film-id="${filmId}">
            Lämna tillbaka
          </button>
        </article>
      `;
    })
    .join('');
}

async function loadFilms() {
  setStatus('Hämtar filmer …');
  try {
    films = await api.fetchFilms();
    renderFilms();
    setStatus('Filmer uppdaterade.');
  } catch (err) {
    console.error(err);
    setStatus('Kunde inte hämta filmer: ' + err.message, 'error');
  }
}

async function loadRentals() {
  setStatus('Hämtar dina lån …');
  try {
    rentals = await api.fetchMyRentals();
    renderRentals();
    setStatus('Dina lån är uppdaterade.');
  } catch (err) {
    console.error(err);
    rentals = [];
    renderRentals();
    setStatus('Kunde inte hämta lån: ' + err.message, 'error');
  }
}

async function handleRent(filmId) {
  const sid = studioId();
  if (!sid) {
    setStatus('Ingen kopplad filmstudio till kontot.', 'error');
    return;
  }
  setStatus('Skickar låneförfrågan …');
  try {
    await api.rentFilm(filmId, sid);
    await Promise.all([loadRentals(), loadFilms()]);
    setStatus('Filmen är lånad!');
  } catch (err) {
    console.error(err);
    setStatus('Kunde inte låna film: ' + err.message, 'error');
  }
}

async function handleReturn(filmId) {
  const sid = studioId();
  if (!sid) {
    setStatus('Ingen kopplad filmstudio till kontot.', 'error');
    return;
  }
  setStatus('Skickar återlämning …');
  try {
    await api.returnFilm(filmId, sid);
    await Promise.all([loadRentals(), loadFilms()]);
    setStatus('Filmen är återlämnad.');
  } catch (err) {
    console.error(err);
    setStatus('Kunde inte lämna tillbaka film: ' + err.message, 'error');
  }
}

function wireEvents() {
  if (filmListEl) {
    filmListEl.addEventListener('click', (ev) => {
      const btn = ev.target.closest('button[data-action="rent"]');
      if (!btn) return;
      const filmId = Number(btn.dataset.filmId);
      if (Number.isFinite(filmId)) handleRent(filmId);
    });
  }

  if (rentalListEl) {
    rentalListEl.addEventListener('click', (ev) => {
      const btn = ev.target.closest('button[data-action="return"]');
      if (!btn) return;
      const filmId = Number(btn.dataset.filmId);
      if (Number.isFinite(filmId)) handleReturn(filmId);
    });
  }

  if (logoutBtn) {
    logoutBtn.addEventListener('click', () => {
      authStore.clear();
      window.location.replace('index.html');
    });
  }

  if (apiForm && apiBaseInput) {
    apiBaseInput.value = getApiBase();
    apiForm.addEventListener('submit', (ev) => {
      ev.preventDefault();
      setApiBase(apiBaseInput.value);
      loadFilms();
      loadRentals();
      setStatus('API-bas uppdaterad.');
    });

    if (apiResetBtn) {
      apiResetBtn.addEventListener('click', () => {
        setApiBase(DEFAULT_BASE);
        apiBaseInput.value = DEFAULT_BASE;
        loadFilms();
        loadRentals();
        setStatus(`API-bas återställd: ${DEFAULT_BASE}`);
      });
    }
  }
}

function init() {
  ensureAuth();
  renderGreeting();
  wireEvents();
  loadFilms();
  loadRentals();
}

init();
