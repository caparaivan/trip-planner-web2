import { apiZahtjev } from './apiKlijent.js';

function resurs(planPutovanjaId) {
  return `/api/planovi-putovanja/${planPutovanjaId}/aktivnosti`;
}

export function vratiAktivnosti(planPutovanjaId) {
  return apiZahtjev(resurs(planPutovanjaId));
}

export function kreirajAktivnost(planPutovanjaId, aktivnost) {
  return apiZahtjev(resurs(planPutovanjaId), {
    method: 'POST',
    body: aktivnost
  });
}

export function izmijeniAktivnost(planPutovanjaId, aktivnostId, aktivnost) {
  return apiZahtjev(`${resurs(planPutovanjaId)}/${aktivnostId}`, {
    method: 'PUT',
    body: aktivnost
  });
}

export function obrisiAktivnost(planPutovanjaId, aktivnostId) {
  return apiZahtjev(`${resurs(planPutovanjaId)}/${aktivnostId}`, {
    method: 'DELETE'
  });
}
