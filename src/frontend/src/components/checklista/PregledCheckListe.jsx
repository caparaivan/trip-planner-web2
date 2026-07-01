export default function PregledCheckListe({ stavkeCheckListe }) {
  const ukupanBrojStavki = stavkeCheckListe.length;
  const zavrseneStavke = stavkeCheckListe.filter((stavka) => stavka.zavrseno).length;
  const procenat = ukupanBrojStavki === 0 ? 0 : Math.round((zavrseneStavke / ukupanBrojStavki) * 100);

  return (
    <div className="pregled-checkliste">
      <div className="statistika-checkliste">
        <span>Zavrseno</span>
        <strong>
          {zavrseneStavke}/{ukupanBrojStavki}
        </strong>
      </div>

      <div className="traka-checkliste" aria-label={`Zavrseno ${procenat} procenata`}>
        <span style={{ width: `${procenat}%` }} />
      </div>

      <p className="prazno-stanje">
        {ukupanBrojStavki === 0
          ? 'Dodajte stvari i obaveze koje treba pripremiti prije puta.'
          : `${procenat}% checkliste je zavrseno.`}
      </p>
    </div>
  );
}
