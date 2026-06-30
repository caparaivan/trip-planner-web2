import { useEffect, useMemo, useState } from 'react';

const daniUSedmici = ['Pon', 'Uto', 'Sri', 'Cet', 'Pet', 'Sub', 'Ned'];

export default function KalendarAktivnosti({ aktivnosti, planPutovanja, onIzmjena }) {
  const [prikazaniDatum, setPrikazaniDatum] = useState(() => new Date());

  useEffect(() => {
    if (planPutovanja?.pocetniDatum) {
      setPrikazaniDatum(new Date(planPutovanja.pocetniDatum));
    }
  }, [planPutovanja?.id, planPutovanja?.pocetniDatum]);

  const aktivnostiPoDatumu = useMemo(() => grupisiAktivnostiPoDatumu(aktivnosti), [aktivnosti]);
  const daniKalendara = useMemo(() => kreirajDaneKalendara(prikazaniDatum), [prikazaniDatum]);

  const promijeniMjesec = (pomjeraj) => {
    setPrikazaniDatum((trenutni) => new Date(trenutni.getFullYear(), trenutni.getMonth() + pomjeraj, 1));
  };

  return (
    <div className="kalendar-aktivnosti">
      <div className="zaglavlje-kalendara">
        <button className="dugme-sporedno" type="button" onClick={() => promijeniMjesec(-1)}>
          Prethodni
        </button>
        <h3>{formatirajMjesec(prikazaniDatum)}</h3>
        <button className="dugme-sporedno" type="button" onClick={() => promijeniMjesec(1)}>
          Sljedeci
        </button>
      </div>

      <div className="kalendar-mreza">
        {daniUSedmici.map((dan) => (
          <div className="dan-sedmice" key={dan}>
            {dan}
          </div>
        ))}

        {daniKalendara.map((dan) => {
          const kljuc = kljucDatuma(dan);
          const aktivnostiDana = aktivnostiPoDatumu[kljuc] || [];
          const uTrenutnomMjesecu = dan.getMonth() === prikazaniDatum.getMonth();

          return (
            <div
              className={`dan-kalendara${uTrenutnomMjesecu ? '' : ' van-mjeseca'}`}
              key={kljuc}
            >
              <span className="datum-kalendara">{dan.getDate()}</span>

              <div className="aktivnosti-u-danu">
                {aktivnostiDana.slice(0, 3).map((aktivnost) => (
                  <button
                    className={`aktivnost-kalendara ${klasaStatusa(aktivnost.status)}`}
                    type="button"
                    onClick={() => onIzmjena(aktivnost)}
                    key={aktivnost.id}
                  >
                    <span>{formatirajVrijeme(aktivnost.vrijeme)}</span>
                    {aktivnost.naziv}
                  </button>
                ))}
                {aktivnostiDana.length > 3 && (
                  <span className="jos-aktivnosti">+{aktivnostiDana.length - 3}</span>
                )}
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}

function grupisiAktivnostiPoDatumu(aktivnosti) {
  return aktivnosti.reduce((akumulator, aktivnost) => {
    const kljuc = aktivnost.datum.slice(0, 10);
    const postojece = akumulator[kljuc] || [];
    return {
      ...akumulator,
      [kljuc]: [...postojece, aktivnost].sort((prva, druga) =>
        formatirajVrijemeZaSortiranje(prva.vrijeme).localeCompare(formatirajVrijemeZaSortiranje(druga.vrijeme))
      )
    };
  }, {});
}

function kreirajDaneKalendara(datum) {
  const godina = datum.getFullYear();
  const mjesec = datum.getMonth();
  const prviDanMjeseca = new Date(godina, mjesec, 1);
  const pomjerajDoPonedjeljka = (prviDanMjeseca.getDay() + 6) % 7;
  const pocetakKalendara = new Date(godina, mjesec, 1 - pomjerajDoPonedjeljka);

  return Array.from({ length: 42 }, (_, indeks) => {
    return new Date(
      pocetakKalendara.getFullYear(),
      pocetakKalendara.getMonth(),
      pocetakKalendara.getDate() + indeks
    );
  });
}

function kljucDatuma(datum) {
  const godina = datum.getFullYear();
  const mjesec = String(datum.getMonth() + 1).padStart(2, '0');
  const dan = String(datum.getDate()).padStart(2, '0');
  return `${godina}-${mjesec}-${dan}`;
}

function formatirajMjesec(datum) {
  return datum.toLocaleDateString('sr-Latn-BA', {
    month: 'long',
    year: 'numeric'
  });
}

function formatirajVrijeme(vrijednost) {
  return vrijednost ? vrijednost.slice(0, 5) : '--:--';
}

function formatirajVrijemeZaSortiranje(vrijednost) {
  return vrijednost || '99:99:99';
}

function klasaStatusa(status) {
  return status.toLowerCase();
}
