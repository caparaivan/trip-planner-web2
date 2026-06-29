import { apiZahtjev } from './apiKlijent.js';

const RESURS = '/api/planovi-putovanja';

export function vratiPlanovePutovanja() {
  return apiZahtjev(RESURS);
}

export function vratiPlanPutovanja(id) {
  return apiZahtjev(`${RESURS}/${id}`);
}

export function kreirajPlanPutovanja(planPutovanja) {
  return apiZahtjev(RESURS, {
    method: 'POST',
    body: pripremiZahtjev(planPutovanja)
  });
}

export function izmijeniPlanPutovanja(id, planPutovanja) {
  return apiZahtjev(`${RESURS}/${id}`, {
    method: 'PUT',
    body: pripremiZahtjev(planPutovanja)
  });
}

export function obrisiPlanPutovanja(id) {
  return apiZahtjev(`${RESURS}/${id}`, {
    method: 'DELETE'
  });
}

function pripremiZahtjev(planPutovanja) {
  return {
    ...planPutovanja,
    planiraniBudzet: Number(planPutovanja.planiraniBudzet || 0)
  };
}
