// FrontEnd/js/api.js
const KEYS = {
  token: 'fh_token',
  profile: 'fh_profile',
  apiBase: 'fh_api_base'
};

// Match current backend listener (see dotnet run log: http://localhost:5001)
export const DEFAULT_BASE = 'http://localhost:5001/api';
const LEGACY_BASES = [
  'http://localhost:5000/api',
  'https://localhost:5000/api'
];

export const authStore = {
  get token() {
    return localStorage.getItem(KEYS.token);
  },
  set token(value) {
    if (!value) {
      localStorage.removeItem(KEYS.token);
      return;
    }
    localStorage.setItem(KEYS.token, value);
  },
  get profile() {
    const raw = localStorage.getItem(KEYS.profile);
    if (!raw) return null;
    try {
      return JSON.parse(raw);
    } catch {
      return null;
    }
  },
  set profile(value) {
    if (!value) {
      localStorage.removeItem(KEYS.profile);
      return;
    }
    localStorage.setItem(KEYS.profile, JSON.stringify(value));
  },
  clear() {
    localStorage.removeItem(KEYS.token);
    localStorage.removeItem(KEYS.profile);
  }
};

export function getApiBase() {
  const stored = (localStorage.getItem(KEYS.apiBase) || DEFAULT_BASE).replace(/\/$/, '');
  if (LEGACY_BASES.includes(stored)) {
    localStorage.setItem(KEYS.apiBase, DEFAULT_BASE);
    return DEFAULT_BASE;
  }
  return stored;
}

export function setApiBase(url) {
  if (!url) return;
  const clean = url.trim().replace(/\/$/, '');
  localStorage.setItem(KEYS.apiBase, clean);
  return clean;
}

async function handleResponse(res) {
  const contentType = res.headers.get('content-type') || '';
  const isJson = contentType.includes('application/json');
  const payload = isJson ? await res.json().catch(() => ({})) : await res.text();

  if (!res.ok) {
    const message = typeof payload === 'string' ? payload : payload?.message || payload?.error || 'Okänt fel';
    throw new Error(`${res.status}: ${message}`);
  }

  return payload;
}

export async function apiRequest(path, { method = 'GET', body, headers = {}, skipAuth = false } = {}) {
  const base = getApiBase();
  const finalHeaders = { Accept: 'application/json', ...headers };

  const hasBody = body !== undefined && body !== null;
  if (hasBody && !(body instanceof FormData)) {
    finalHeaders['Content-Type'] = 'application/json';
  }

  if (!skipAuth && authStore.token) {
    // Backend expects the custom header name "Authentication" (see Program.cs)
    finalHeaders.Authentication = `Bearer ${authStore.token}`;
  }

  const response = await fetch(`${base}${path}`, {
    method,
    headers: finalHeaders,
    body: hasBody && !(body instanceof FormData) ? JSON.stringify(body) : hasBody ? body : undefined
  });

  return handleResponse(response);
}

// API helpers
export const api = {
  fetchFilms() {
    return apiRequest('/films');
  },
  login(userName, password) {
    return apiRequest('/users/authenticate', {
      method: 'POST',
      body: { userName, password },
      skipAuth: true
    });
  },
  fetchMyRentals() {
    return apiRequest('/mystudio/rentals');
  },
  rentFilm(filmId, studioId) {
    return apiRequest(`/films/rent?id=${filmId}&studioId=${studioId}`, {
      method: 'POST',
      body: {}
    });
  },
  returnFilm(filmId, studioId) {
    return apiRequest(`/films/return?id=${filmId}&studioId=${studioId}`, {
      method: 'POST',
      body: {}
    });
  },
  getStudios() {
    return apiRequest('/filmstudios');
  }
};
