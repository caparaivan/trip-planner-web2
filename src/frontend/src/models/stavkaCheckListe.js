export const praznaStavkaCheckListe = {
  naziv: '',
  zavrseno: false
};

export function stavkaCheckListeUFormu(stavka) {
  if (!stavka) {
    return praznaStavkaCheckListe;
  }

  return {
    naziv: stavka.naziv || '',
    zavrseno: Boolean(stavka.zavrseno)
  };
}

export function stavkaCheckListeIzForme(podaciForme) {
  return {
    naziv: podaciForme.naziv,
    zavrseno: Boolean(podaciForme.zavrseno)
  };
}
