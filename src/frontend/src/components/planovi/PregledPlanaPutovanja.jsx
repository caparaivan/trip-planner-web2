export default function PregledPlanaPutovanja({
  planPutovanja,
  destinacije,
  aktivnosti,
  troskovi,
  pregledBudzeta
}) {
  if (!planPutovanja) {
    return <p className="prazno-stanje">Izaberite plan za prikaz cjelokupnog pregleda.</p>;
  }

  const aktivnostiPoDanima = grupisiAktivnostiPoDanima(aktivnosti);
  const najnovijiTroskovi = troskovi.slice(0, 4);

  return (
    <article className="pregled-plana">
      <div className="zaglavlje-pregleda-plana">
        <div>
          <h3>{planPutovanja.naziv}</h3>
          <p>{planPutovanja.kratakOpis || 'Bez kratkog opisa'}</p>
        </div>
        <div className="period-pregleda">
          {formatirajDatum(planPutovanja.pocetniDatum)} - {formatirajDatum(planPutovanja.krajnjiDatum)}
        </div>
      </div>

      <nav className="navigacija-pregleda" aria-label="Navigacija kroz plan putovanja">
        <a href="#sekcija-destinacije">Destinacije</a>
        <a href="#sekcija-aktivnosti">Aktivnosti</a>
        <a href="#sekcija-troskovi">Troskovi</a>
        <a href="#sekcija-napomene">Napomene</a>
      </nav>

      <div className="mreza-pregleda-plana">
        <section className="blok-pregleda">
          <h4>Osnovni podaci</h4>
          <dl>
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
              <dd className={planPutovanja.preostaliBudzet < 0 ? 'negativan-budzet' : ''}>
                {formatirajNovac(planPutovanja.preostaliBudzet)}
              </dd>
            </div>
          </dl>
        </section>

        <section className="blok-pregleda">
          <h4>Destinacije</h4>
          {destinacije.length === 0 && <p className="prazno-stanje">Nema unesenih destinacija.</p>}
          <div className="sazeta-lista">
            {destinacije.slice(0, 4).map((destinacija) => (
              <div className="stavka-sazetka" key={destinacija.id}>
                <strong>{destinacija.naziv}</strong>
                <span>
                  {destinacija.lokacija} | {formatirajDatum(destinacija.datumDolaska)} -{' '}
                  {formatirajDatum(destinacija.datumOdlaska)}
                </span>
              </div>
            ))}
          </div>
          {destinacije.length > 4 && <p className="napomena-pregleda">Jos {destinacije.length - 4} destinacija.</p>}
        </section>

        <section className="blok-pregleda blok-pregleda-sirok">
          <h4>Raspored aktivnosti po danima</h4>
          {aktivnostiPoDanima.length === 0 && <p className="prazno-stanje">Nema unesenih aktivnosti.</p>}
          <div className="dani-pregleda">
            {aktivnostiPoDanima.slice(0, 5).map((grupa) => (
              <div className="dan-pregleda" key={grupa.datum}>
                <strong>{formatirajDatumSaDanom(grupa.datum)}</strong>
                <div>
                  {grupa.stavke.slice(0, 3).map((aktivnost) => (
                    <span className="aktivnost-pregleda" key={aktivnost.id}>
                      {formatirajVrijeme(aktivnost.vrijeme)} {aktivnost.naziv} ({aktivnost.status})
                    </span>
                  ))}
                  {grupa.stavke.length > 3 && (
                    <span className="napomena-pregleda">+{grupa.stavke.length - 3} aktivnosti</span>
                  )}
                </div>
              </div>
            ))}
          </div>
        </section>

        <section className="blok-pregleda">
          <h4>Budzet po kategorijama</h4>
          {!pregledBudzeta || pregledBudzeta.troskoviPoKategorijama.length === 0 ? (
            <p className="prazno-stanje">Nema troskova po kategorijama.</p>
          ) : (
            <div className="sazeta-lista">
              {pregledBudzeta.troskoviPoKategorijama.map((stavka) => (
                <div className="stavka-sazetka red-sazetka" key={stavka.kategorija}>
                  <strong>{stavka.kategorija}</strong>
                  <span>{formatirajNovac(stavka.ukupanIznos)}</span>
                </div>
              ))}
            </div>
          )}
        </section>

        <section className="blok-pregleda">
          <h4>Posljednji troskovi</h4>
          {najnovijiTroskovi.length === 0 && <p className="prazno-stanje">Nema unesenih troskova.</p>}
          <div className="sazeta-lista">
            {najnovijiTroskovi.map((trosak) => (
              <div className="stavka-sazetka red-sazetka" key={trosak.id}>
                <strong>{trosak.naziv}</strong>
                <span>{formatirajNovac(trosak.iznos)}</span>
              </div>
            ))}
          </div>
        </section>

        <section className="blok-pregleda blok-pregleda-sirok" id="sekcija-napomene">
          <h4>Napomene</h4>
          <p>{planPutovanja.opsteNapomene || 'Bez opstih napomena.'}</p>
        </section>
      </div>
    </article>
  );
}

function grupisiAktivnostiPoDanima(aktivnosti) {
  const grupe = aktivnosti.reduce((akumulator, aktivnost) => {
    const datum = aktivnost.datum.slice(0, 10);
    const postojecaGrupa = akumulator.find((grupa) => grupa.datum === datum);

    if (postojecaGrupa) {
      postojecaGrupa.stavke.push(aktivnost);
      return akumulator;
    }

    return [...akumulator, { datum, stavke: [aktivnost] }];
  }, []);

  return grupe
    .map((grupa) => ({
      ...grupa,
      stavke: [...grupa.stavke].sort((prva, druga) =>
        formatirajVrijemeZaSortiranje(prva.vrijeme).localeCompare(formatirajVrijemeZaSortiranje(druga.vrijeme))
      )
    }))
    .sort((prva, druga) => prva.datum.localeCompare(druga.datum));
}

function formatirajDatum(vrijednost) {
  return new Date(vrijednost).toLocaleDateString('sr-Latn-BA');
}

function formatirajDatumSaDanom(vrijednost) {
  return new Date(vrijednost).toLocaleDateString('sr-Latn-BA', {
    weekday: 'long',
    day: '2-digit',
    month: '2-digit',
    year: 'numeric'
  });
}

function formatirajVrijeme(vrijednost) {
  return vrijednost ? vrijednost.slice(0, 5) : '--:--';
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
