import { apiZahtjev } from './apiKlijent.js';

function resurs(planPutovanjaId) {
  return `/api/planovi-putovanja/${planPutovanjaId}/troskovi`;
}

export function vratiTroskove(planPutovanjaId) {
  return apiZahtjev(resurs(planPutovanjaId));
}

export function vratiPregledBudzeta(planPutovanjaId) {
  return apiZahtjev(`${resurs(planPutovanjaId)}/pregled-budzeta`);
}

export function kreirajTrosak(planPutovanjaId, trosak) {
  return apiZahtjev(resurs(planPutovanjaId), {
    method: 'POST',
    body: trosak
  });
}

export function izmijeniTrosak(planPutovanjaId, trosakId, trosak) {
  return apiZahtjev(`${resurs(planPutovanjaId)}/${trosakId}`, {
    method: 'PUT',
    body: trosak
  });
}

export function obrisiTrosak(planPutovanjaId, trosakId) {
  return apiZahtjev(`${resurs(planPutovanjaId)}/${trosakId}`, {
    method: 'DELETE'
  });
}
