export const praznaDestinacija = {
  naziv: '',
  lokacija: '',
  datumDolaska: '',
  datumOdlaska: '',
  kratakOpis: ''
};

export function destinacijaUFormu(destinacija) {
  if (!destinacija) {
    return praznaDestinacija;
  }

  return {
    naziv: destinacija.naziv || '',
    lokacija: destinacija.lokacija || '',
    datumDolaska: formatirajDatumZaInput(destinacija.datumDolaska),
    datumOdlaska: formatirajDatumZaInput(destinacija.datumOdlaska),
    kratakOpis: destinacija.kratakOpis || ''
  };
}

function formatirajDatumZaInput(vrijednost) {
  if (!vrijednost) {
    return '';
  }

  return new Date(vrijednost).toISOString().slice(0, 10);
}
