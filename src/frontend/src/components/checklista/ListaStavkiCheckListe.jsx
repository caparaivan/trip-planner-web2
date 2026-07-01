export default function ListaStavkiCheckListe({ stavkeCheckListe, onPromjenaStatusa, onIzmjena, onBrisanje }) {
  if (stavkeCheckListe.length === 0) {
    return <p className="prazno-stanje">Za ovaj plan jos nema unesenih stavki checkliste.</p>;
  }

  return (
    <div className="lista-stavki-checkliste">
      {stavkeCheckListe.map((stavka) => (
        <article className={`stavka-checkliste ${stavka.zavrseno ? 'zavrsena' : ''}`} key={stavka.id}>
          <label className="red-checkliste">
            <input
              checked={stavka.zavrseno}
              type="checkbox"
              onChange={() => onPromjenaStatusa(stavka)}
            />
            <span className="naziv-stavke-checkliste">{stavka.naziv}</span>
          </label>

          <div className="akcije-plana">
            <button className="dugme-sporedno" type="button" onClick={() => onIzmjena(stavka)}>
              Izmijeni
            </button>
            <button className="dugme-opasno" type="button" onClick={() => onBrisanje(stavka.id)}>
              Obrisi
            </button>
          </div>
        </article>
      ))}
    </div>
  );
}
