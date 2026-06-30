export const statusiAktivnosti = ['Planirano', 'Rezervisano', 'Zavrseno', 'Otkazano'];

export const praznaAktivnost = {
  naziv: '',
  datum: '',
  vrijeme: '',
  lokacija: '',
  opis: '',
  procijenjeniTrosak: '',
  status: 'Planirano'
};

export function aktivnostUFormu(aktivnost) {
  if (!aktivnost) {
    return praznaAktivnost;
  }

  return {
    naziv: aktivnost.naziv || '',
    datum: formatirajDatumZaInput(aktivnost.datum),
    vrijeme: formatirajVrijemeZaInput(aktivnost.vrijeme),
    lokacija: aktivnost.lokacija || '',
    opis: aktivnost.opis || '',
    procijenjeniTrosak: aktivnost.procijenjeniTrosak ?? '',
    status: aktivnost.status || 'Planirano'
  };
}

export function aktivnostIzForme(podaciForme) {
  return {
    ...podaciForme,
    vrijeme: podaciForme.vrijeme ? `${podaciForme.vrijeme}:00` : null,
    procijenjeniTrosak: Number(podaciForme.procijenjeniTrosak || 0)
  };
}

function formatirajDatumZaInput(vrijednost) {
  if (!vrijednost) {
    return '';
  }

  return new Date(vrijednost).toISOString().slice(0, 10);
}

function formatirajVrijemeZaInput(vrijednost) {
  if (!vrijednost) {
    return '';
  }

  return vrijednost.slice(0, 5);
}
