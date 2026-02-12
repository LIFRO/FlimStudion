// FrontEnd/js/register.js
import { apiRequest, setApiBase, getApiBase, DEFAULT_BASE, api, authStore } from './api.js';

const form = document.querySelector('#register-form');
const messageEl = document.querySelector('#register-message');
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

async function handleRegister(ev) {
  ev.preventDefault();
  const formData = new FormData(form);
  const userName = formData.get('username');
  const password = formData.get('password');
  const studioName = formData.get('studioName');
  const city = formData.get('city');
  const base = formData.get('api-base');

  if (base) setApiBase(base.toString());

  const trimmedUser = userName?.toString().trim();
  const trimmedPass = password?.toString();

  if (!trimmedUser || !trimmedPass) {
    setMessage('Fyll i användarnamn och lösenord.', 'error');
    return;
  }

  if (!studioName || !city) {
    setMessage('Fyll i studionamn och stad.', 'error');
    return;
  }

  setMessage('Registrerar filmstudio …', 'info');
  try {
    await apiRequest('/filmstudio/register', {
      method: 'POST',
      body: {
        userName: trimmedUser,
        password: trimmedPass,
        studioName: studioName?.toString(),
        city: city?.toString()
      },
      skipAuth: true
    });

    // Logga in direkt med samma uppgifter
    const loginResult = await api.login(trimmedUser, trimmedPass);
    authStore.token = loginResult.token;
    authStore.profile = loginResult.dto || loginResult;

    setMessage('Konto skapat och du är inloggad! Omdirigerar …', 'success');
    setTimeout(() => {
      window.location.href = 'dashboard.html';
    }, 600);
  } catch (err) {
    console.error(err);
    setMessage('Registreringen misslyckades: ' + err.message, 'error');
  }
}

if (form) {
  form.addEventListener('submit', handleRegister);
}

hydrateBase();

if (apiResetBtn && baseInput) {
  apiResetBtn.addEventListener('click', () => {
    setApiBase(DEFAULT_BASE);
    baseInput.value = DEFAULT_BASE;
    setMessage(`API-bas återställd: ${DEFAULT_BASE}`, 'info');
  });
}
