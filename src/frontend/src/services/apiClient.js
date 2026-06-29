const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:8801';

export async function apiRequest(path, options = {}) {
  const token = localStorage.getItem('tripPlannerToken');
  const headers = {
    'Content-Type': 'application/json',
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
    ...options.headers
  };

  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...options,
    headers,
    body: options.body ? JSON.stringify(options.body) : undefined
  });

  const contentType = response.headers.get('content-type') || '';
  const data = contentType.includes('application/json') ? await response.json() : null;

  if (!response.ok) {
    const message = data?.errors?.join(' ') || data?.message || 'Doslo je do greske prilikom API poziva.';
    throw new Error(message);
  }

  return data;
}
