export default function ListaDestinacija({ destinacije, onIzmjena, onBrisanje }) {
  if (destinacije.length === 0) {
    return <p className="prazno-stanje">Za ovaj plan jos nema unesenih destinacija.</p>;
  }

  return (
    <div className="lista-destinacija">
      {destinacije.map((destinacija) => (
        <article className="stavka-destinacije" key={destinacija.id}>
          <div>
            <h3>{destinacija.naziv}</h3>
            <p>{destinacija.lokacija}</p>
          </div>
          <dl>
            <div>
              <dt>Dolazak</dt>
              <dd>{formatirajDatum(destinacija.datumDolaska)}</dd>
            </div>
            <div>
              <dt>Odlazak</dt>
              <dd>{formatirajDatum(destinacija.datumOdlaska)}</dd>
            </div>
            <div className="siroko-polje">
              <dt>Opis</dt>
              <dd>{destinacija.kratakOpis || 'Bez opisa'}</dd>
            </div>
          </dl>
          <div className="akcije-plana">
            <button className="dugme-sporedno" type="button" onClick={() => onIzmjena(destinacija)}>
              Izmijeni
            </button>
            <button className="dugme-opasno" type="button" onClick={() => onBrisanje(destinacija.id)}>
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
