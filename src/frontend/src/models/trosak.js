export const kategorijeTroskova = ['Prevoz', 'Smjestaj', 'Hrana', 'Ulaznice', 'Kupovina', 'Ostalo'];

export const prazanTrosak = {
  naziv: '',
  kategorija: 'Ostalo',
  iznos: '',
  datum: '',
  opis: ''
};

export function trosakUFormu(trosak) {
  if (!trosak) {
    return prazanTrosak;
  }

  return {
    naziv: trosak.naziv || '',
    kategorija: trosak.kategorija || 'Ostalo',
    iznos: trosak.iznos ?? '',
    datum: formatirajDatumZaInput(trosak.datum),
    opis: trosak.opis || ''
  };
}

export function trosakIzForme(podaciForme) {
  return {
    ...podaciForme,
    iznos: Number(podaciForme.iznos || 0)
  };
}

function formatirajDatumZaInput(vrijednost) {
  if (!vrijednost) {
    return '';
  }

  return new Date(vrijednost).toISOString().slice(0, 10);
}
