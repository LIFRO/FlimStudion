// FrontEnd/js/index.js
import { api, setApiBase, getApiBase, authStore, DEFAULT_BASE } from './api.js';

const filmListEl = document.querySelector('#film-list');
const statusEl = document.querySelector('#status');
const apiForm = document.querySelector('#api-form');
const apiBaseInput = document.querySelector('#api-base');
const apiResetBtn = document.querySelector('#api-reset');
const loginStateEl = document.querySelector('#login-state');

function setStatus(message, tone = 'info') {
  if (!statusEl) return;
  statusEl.textContent = message;
  statusEl.dataset.tone = tone;
}

function renderFilms(films) {
  if (!filmListEl) return;
  if (!films?.length) {
    filmListEl.innerHTML = '<p class="muted">Inga filmer hittades.</p>';
    return;
  }

  const cards = films.map((film) => {
    const available = Array.isArray(film.filmCopies)
      ? film.filmCopies.filter((c) => !c.loanedByFilmStudioId).length
      : null;

    return `
      <article class="card">
        <header>
          <p class="eyebrow">Genre: ${film.genre || 'Okänd'}</p>
          <h3>${film.title}</h3>
        </header>
        <p class="muted">ID: ${film.filmId}</p>
        ${available !== null ? `<p class="muted">Tillgängliga exemplar: ${available}</p>` : ''}
      </article>
    `;
  });

  filmListEl.innerHTML = cards.join('');
}

async function loadFilms() {
  setStatus('Hämtar filmer …', 'info');
  try {
    const films = await api.fetchFilms();
    renderFilms(films);
    setStatus(`Visar ${films.length} filmer`);
  } catch (err) {
    console.error(err);
    setStatus('Kunde inte hämta filmer: ' + err.message, 'error');
  }
}

function updateLoginBadge() {
  if (!loginStateEl) return;
  const profile = authStore.profile;
  if (authStore.token && profile) {
    loginStateEl.innerHTML = `Inloggad som <strong>${profile.userName || 'okänd användare'}</strong> (${profile.role || ''}). <a href="dashboard.html">Gå till dashboard</a>.`;
  } else {
    loginStateEl.innerHTML = 'Ej inloggad. <a href="login.html">Logga in</a> för att låna filmer.';
  }
}

function initApiBaseForm() {
  if (!apiForm || !apiBaseInput) return;
  apiBaseInput.value = getApiBase();
  apiForm.addEventListener('submit', (ev) => {
    ev.preventDefault();
    const newBase = setApiBase(apiBaseInput.value);
    setStatus(`API-bas sparad: ${newBase}`);
    loadFilms();
  });

  if (apiResetBtn) {
    apiResetBtn.addEventListener('click', () => {
      setApiBase(DEFAULT_BASE);
      apiBaseInput.value = DEFAULT_BASE;
      setStatus(`API-bas återställd: ${DEFAULT_BASE}`);
      loadFilms();
    });
  }
}

updateLoginBadge();
initApiBaseForm();
loadFilms();
