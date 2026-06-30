export default function ListaAktivnosti({ aktivnosti, onIzmjena, onBrisanje }) {
  if (aktivnosti.length === 0) {
    return <p className="prazno-stanje">Za ovaj plan jos nema unesenih aktivnosti.</p>;
  }

  const grupe = grupisiPoDatumu(aktivnosti);

  return (
    <div className="lista-aktivnosti">
      {grupe.map((grupa) => (
        <section className="grupa-aktivnosti" key={grupa.datum}>
          <h3>{formatirajDatum(grupa.datum)}</h3>
          {grupa.stavke.map((aktivnost) => (
            <article className="stavka-aktivnosti" key={aktivnost.id}>
              <div>
                <div className="red-naslova-aktivnosti">
                  <h4>{aktivnost.naziv}</h4>
                  <span className={`status-aktivnosti ${klasaStatusa(aktivnost.status)}`}>
                    {aktivnost.status}
                  </span>
                </div>
                <p>
                  {formatirajVrijeme(aktivnost.vrijeme)}
                  {aktivnost.lokacija ? ` | ${aktivnost.lokacija}` : ''}
                </p>
              </div>

              <dl>
                <div>
                  <dt>Procijenjeni trosak</dt>
                  <dd>{formatirajNovac(aktivnost.procijenjeniTrosak)}</dd>
                </div>
                <div className="siroko-polje">
                  <dt>Opis</dt>
                  <dd>{aktivnost.opis || 'Bez opisa'}</dd>
                </div>
              </dl>

              <div className="akcije-plana">
                <button className="dugme-sporedno" type="button" onClick={() => onIzmjena(aktivnost)}>
                  Izmijeni
                </button>
                <button className="dugme-opasno" type="button" onClick={() => onBrisanje(aktivnost.id)}>
                  Obrisi
                </button>
              </div>
            </article>
          ))}
        </section>
      ))}
    </div>
  );
}

function grupisiPoDatumu(aktivnosti) {
  const grupe = aktivnosti.reduce((akumulator, aktivnost) => {
    const datum = aktivnost.datum.slice(0, 10);
    const postojecaGrupa = akumulator.find((grupa) => grupa.datum === datum);

    if (postojecaGrupa) {
      postojecaGrupa.stavke.push(aktivnost);
      return akumulator;
    }

    return [...akumulator, { datum, stavke: [aktivnost] }];
  }, []);

  return grupe.map((grupa) => ({
    ...grupa,
    stavke: [...grupa.stavke].sort((prva, druga) =>
      formatirajVrijemeZaSortiranje(prva.vrijeme).localeCompare(formatirajVrijemeZaSortiranje(druga.vrijeme))
    )
  }));
}

function formatirajDatum(vrijednost) {
  return new Date(vrijednost).toLocaleDateString('sr-Latn-BA', {
    weekday: 'long',
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  });
}

function formatirajVrijeme(vrijednost) {
  return vrijednost ? vrijednost.slice(0, 5) : 'Bez vremena';
}

function formatirajVrijemeZaSortiranje(vrijednost) {
  return vrijednost || '99:99:99';
}

function formatirajNovac(vrijednost) {
  return Number(vrijednost || 0).toLocaleString('sr-Latn-BA', {
    style: 'currency',
    currency: 'BAM'
  });
}

function klasaStatusa(status) {
  return status.toLowerCase();
}
