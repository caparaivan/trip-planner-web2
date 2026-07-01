import { apiZahtjev } from './apiKlijent.js';

function resurs(planPutovanjaId) {
  return `/api/planovi-putovanja/${planPutovanjaId}/checklista`;
}

export function vratiStavkeCheckListe(planPutovanjaId) {
  return apiZahtjev(resurs(planPutovanjaId));
}

export function kreirajStavkuCheckListe(planPutovanjaId, stavka) {
  return apiZahtjev(resurs(planPutovanjaId), {
    method: 'POST',
    body: stavka
  });
}

export function izmijeniStavkuCheckListe(planPutovanjaId, stavkaId, stavka) {
  return apiZahtjev(`${resurs(planPutovanjaId)}/${stavkaId}`, {
    method: 'PUT',
    body: stavka
  });
}

export function obrisiStavkuCheckListe(planPutovanjaId, stavkaId) {
  return apiZahtjev(`${resurs(planPutovanjaId)}/${stavkaId}`, {
    method: 'DELETE'
  });
}
