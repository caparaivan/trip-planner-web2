export default function PregledBudzeta({ pregledBudzeta }) {
  if (!pregledBudzeta) {
    return <p className="prazno-stanje">Pregled budzeta ce biti prikazan nakon izbora plana.</p>;
  }

  const planiraniBudzet = Number(pregledBudzeta.planiraniBudzet || 0);
  const ukupanTrosak = Number(pregledBudzeta.ukupanTrosak || 0);
  const procenatPotrosnje = planiraniBudzet > 0
    ? Math.min(100, Math.round((ukupanTrosak / planiraniBudzet) * 100))
    : 0;

  return (
    <div className="pregled-budzeta">
      <div className="mreza-budzeta">
        <div className="stavka-budzeta">
          <span>Planirani budzet</span>
          <strong>{formatirajNovac(pregledBudzeta.planiraniBudzet)}</strong>
        </div>
        <div className="stavka-budzeta">
          <span>Ukupni troskovi</span>
          <strong>{formatirajNovac(pregledBudzeta.ukupanTrosak)}</strong>
        </div>
        <div className="stavka-budzeta">
          <span>Preostali budzet</span>
          <strong className={pregledBudzeta.preostaliBudzet < 0 ? 'negativan-budzet' : ''}>
            {formatirajNovac(pregledBudzeta.preostaliBudzet)}
          </strong>
        </div>
      </div>

      <div className="traka-budzeta" aria-label="Potroseni dio budzeta">
        <span style={{ width: `${procenatPotrosnje}%` }} />
      </div>

      <div className="pregled-kategorija">
        <h3>Troskovi po kategorijama</h3>
        {pregledBudzeta.troskoviPoKategorijama.length === 0 && (
          <p className="prazno-stanje">Nema troskova po kategorijama.</p>
        )}
        {pregledBudzeta.troskoviPoKategorijama.map((stavka) => (
          <div className="stavka-kategorije" key={stavka.kategorija}>
            <span>{stavka.kategorija}</span>
            <strong>{formatirajNovac(stavka.ukupanIznos)}</strong>
          </div>
        ))}
      </div>
    </div>
  );
}

function formatirajNovac(vrijednost) {
  return Number(vrijednost || 0).toLocaleString('sr-Latn-BA', {
    style: 'currency',
    currency: 'BAM'
  });
}
