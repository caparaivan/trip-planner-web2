import { apiZahtjev } from './apiKlijent.js';

function resurs(planPutovanjaId) {
  return `/api/planovi-putovanja/${planPutovanjaId}/destinacije`;
}

export function vratiDestinacije(planPutovanjaId) {
  return apiZahtjev(resurs(planPutovanjaId));
}

export function kreirajDestinaciju(planPutovanjaId, destinacija) {
  return apiZahtjev(resurs(planPutovanjaId), {
    method: 'POST',
    body: destinacija
  });
}

export function izmijeniDestinaciju(planPutovanjaId, destinacijaId, destinacija) {
  return apiZahtjev(`${resurs(planPutovanjaId)}/${destinacijaId}`, {
    method: 'PUT',
    body: destinacija
  });
}

export function obrisiDestinaciju(planPutovanjaId, destinacijaId) {
  return apiZahtjev(`${resurs(planPutovanjaId)}/${destinacijaId}`, {
    method: 'DELETE'
  });
}
