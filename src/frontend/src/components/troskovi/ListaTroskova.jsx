export default function ListaTroskova({ troskovi, onIzmjena, onBrisanje }) {
  if (troskovi.length === 0) {
    return <p className="prazno-stanje">Za ovaj plan jos nema unesenih troskova.</p>;
  }

  return (
    <div className="lista-troskova">
      {troskovi.map((trosak) => (
        <article className="stavka-troska" key={trosak.id}>
          <div>
            <div className="red-naslova-aktivnosti">
              <h3>{trosak.naziv}</h3>
              <span className="oznaka-kategorije">{trosak.kategorija}</span>
            </div>
            <p>{formatirajDatum(trosak.datum)}</p>
          </div>

          <dl>
            <div>
              <dt>Iznos</dt>
              <dd>{formatirajNovac(trosak.iznos)}</dd>
            </div>
            <div className="siroko-polje">
              <dt>Opis</dt>
              <dd>{trosak.opis || 'Bez opisa'}</dd>
            </div>
          </dl>

          <div className="akcije-plana">
            <button className="dugme-sporedno" type="button" onClick={() => onIzmjena(trosak)}>
              Izmijeni
            </button>
            <button className="dugme-opasno" type="button" onClick={() => onBrisanje(trosak.id)}>
              Obrisi
            </button>
          </div>
        </article>
      ))}
    </div>
  );
}

function formatirajDatum(vrijednost) {
  return new Date(vrijednost).toLocaleDateString('sr-Latn-BA');
}

function formatirajNovac(vrijednost) {
  return Number(vrijednost || 0).toLocaleString('sr-Latn-BA', {
    style: 'currency',
    currency: 'BAM'
  });
}
