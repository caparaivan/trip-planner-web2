export const prazanPlanPutovanja = {
  naziv: '',
  kratakOpis: '',
  pocetniDatum: '',
  krajnjiDatum: '',
  planiraniBudzet: '',
  opsteNapomene: ''
};

export function planUFormu(planPutovanja) {
  if (!planPutovanja) {
    return prazanPlanPutovanja;
  }

  return {
    naziv: planPutovanja.naziv || '',
    kratakOpis: planPutovanja.kratakOpis || '',
    pocetniDatum: formatirajDatumZaInput(planPutovanja.pocetniDatum),
    krajnjiDatum: formatirajDatumZaInput(planPutovanja.krajnjiDatum),
    planiraniBudzet: String(planPutovanja.planiraniBudzet ?? ''),
    opsteNapomene: planPutovanja.opsteNapomene || ''
  };
}

function formatirajDatumZaInput(vrijednost) {
  if (!vrijednost) {
    return '';
  }

  return new Date(vrijednost).toISOString().slice(0, 10);
}
