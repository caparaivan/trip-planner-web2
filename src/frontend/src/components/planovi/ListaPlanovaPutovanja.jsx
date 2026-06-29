export default function ListaPlanovaPutovanja({ planoviPutovanja, odabraniPlanId, onDetalji, onIzmjena, onBrisanje }) {
  if (planoviPutovanja.length === 0) {
    return <p className="prazno-stanje">Nema kreiranih planova putovanja.</p>;
  }

  return (
    <div className="lista-planova">
      {planoviPutovanja.map((plan) => (
        <article className={plan.id === odabraniPlanId ? 'stavka-plana aktivna' : 'stavka-plana'} key={plan.id}>
          <div>
            <h3>{plan.naziv}</h3>
            <p>{plan.kratakOpis || 'Bez opisa'}</p>
          </div>
          <dl>
            <div>
              <dt>Period</dt>
              <dd>{formatirajDatum(plan.pocetniDatum)} - {formatirajDatum(plan.krajnjiDatum)}</dd>
            </div>
            <div>
              <dt>Budzet</dt>
              <dd>{formatirajNovac(plan.planiraniBudzet)}</dd>
            </div>
            <div>
              <dt>Preostalo</dt>
              <dd>{formatirajNovac(plan.preostaliBudzet)}</dd>
            </div>
          </dl>
          <div className="akcije-plana">
            <button className="dugme-sporedno" type="button" onClick={() => onDetalji(plan.id)}>
              Detalji
            </button>
            <button className="dugme-sporedno" type="button" onClick={() => onIzmjena(plan)}>
              Izmijeni
            </button>
            <button className="dugme-opasno" type="button" onClick={() => onBrisanje(plan.id)}>
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
  return `${Number(vrijednost || 0).toFixed(2)} KM`;
}
