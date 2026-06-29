export default function DetaljiPlanaPutovanja({ planPutovanja }) {
  if (!planPutovanja) {
    return <p className="prazno-stanje">Izaberite plan za prikaz detalja.</p>;
  }

  return (
    <article className="detalji-plana">
      <h3>{planPutovanja.naziv}</h3>
      <dl>
        <div>
          <dt>Kratak opis</dt>
          <dd>{planPutovanja.kratakOpis || 'Bez opisa'}</dd>
        </div>
        <div>
          <dt>Pocetni datum</dt>
          <dd>{formatirajDatum(planPutovanja.pocetniDatum)}</dd>
        </div>
        <div>
          <dt>Krajnji datum</dt>
          <dd>{formatirajDatum(planPutovanja.krajnjiDatum)}</dd>
        </div>
        <div>
          <dt>Planirani budzet</dt>
          <dd>{formatirajNovac(planPutovanja.planiraniBudzet)}</dd>
        </div>
        <div>
          <dt>Ukupan trosak</dt>
          <dd>{formatirajNovac(planPutovanja.ukupanTrosak)}</dd>
        </div>
        <div>
          <dt>Preostali budzet</dt>
          <dd>{formatirajNovac(planPutovanja.preostaliBudzet)}</dd>
        </div>
        <div className="siroko-polje">
          <dt>Opste napomene</dt>
          <dd>{planPutovanja.opsteNapomene || 'Bez napomena'}</dd>
        </div>
      </dl>
    </article>
  );
}

function formatirajDatum(vrijednost) {
  return new Date(vrijednost).toLocaleDateString('sr-Latn-BA');
}

function formatirajNovac(vrijednost) {
  return `${Number(vrijednost || 0).toFixed(2)} KM`;
}
