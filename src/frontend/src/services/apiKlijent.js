const OSNOVNI_API_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:8801';

export async function apiZahtjev(putanja, opcije = {}) {
  const token = localStorage.getItem('tripPlannerToken');
  const zaglavlja = {
    'Content-Type': 'application/json',
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
    ...opcije.headers
  };

  const odgovor = await fetch(`${OSNOVNI_API_URL}${putanja}`, {
    ...opcije,
    headers: zaglavlja,
    body: opcije.body ? JSON.stringify(opcije.body) : undefined
  });

  const tipSadrzaja = odgovor.headers.get('content-type') || '';
  const podaci = tipSadrzaja.includes('application/json') ? await odgovor.json() : null;

  if (!odgovor.ok) {
    const poruka = podaci?.greske?.join(' ') || podaci?.message || 'Doslo je do greske prilikom API poziva.';
    throw new Error(poruka);
  }

  return podaci;
}
