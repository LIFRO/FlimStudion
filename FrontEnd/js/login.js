// FrontEnd/js/login.js
import { api, setApiBase, getApiBase, authStore, DEFAULT_BASE } from './api.js';

const form = document.querySelector('#login-form');
const messageEl = document.querySelector('#login-message');
const baseInput = document.querySelector('#api-base');
const apiResetBtn = document.querySelector('#api-reset');

function setMessage(text, tone = 'info') {
  if (!messageEl) return;
  messageEl.textContent = text;
  messageEl.dataset.tone = tone;
}

function hydrateBase() {
  if (baseInput) baseInput.value = getApiBase();
}

async function handleLogin(ev) {
  ev.preventDefault();
  const formData = new FormData(form);
  const userName = formData.get('username');
  const password = formData.get('password');
  const base = formData.get('api-base');

  if (base) setApiBase(base.toString());

  if (!userName || !password) {
    setMessage('Fyll i användarnamn och lösenord.', 'error');
    return;
  }

  setMessage('Loggar in …', 'info');
  try {
    const result = await api.login(userName.toString(), password.toString());
    authStore.token = result.token;
    authStore.profile = result.dto || result;

    const profile = authStore.profile;
    const name = profile?.userName || 'Okänd';
    setMessage(`Inloggad som ${name}. Omdirigerar …`, 'success');
    setTimeout(() => {
      window.location.href = 'dashboard.html';
    }, 500);
  } catch (err) {
    console.error(err);
    setMessage('Inloggningen misslyckades: ' + err.message, 'error');
  }
}

function guardAlreadyLoggedIn() {
  if (authStore.token) {
    setMessage('Du är redan inloggad. Gå till dashboard.', 'success');
  }
}

if (form) {
  form.addEventListener('submit', handleLogin);
}
hydrateBase();
guardAlreadyLoggedIn();

if (apiResetBtn && baseInput) {
  apiResetBtn.addEventListener('click', () => {
    setApiBase(DEFAULT_BASE);
    baseInput.value = DEFAULT_BASE;
    setMessage(`API-bas återställd: ${DEFAULT_BASE}`, 'info');
  });
}
