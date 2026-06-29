import { useEffect, useState } from 'react';
import { planUFormu, prazanPlanPutovanja } from '../../models/planPutovanja.js';

export default function FormaPlanaPutovanja({ mod, planZaIzmjenu, onSubmit, onCancel, disabled }) {
  const [podaciForme, setPodaciForme] = useState(prazanPlanPutovanja);
  const [greska, setGreska] = useState('');

  useEffect(() => {
    setPodaciForme(planUFormu(planZaIzmjenu));
    setGreska('');
  }, [planZaIzmjenu]);

  const handleChange = (event) => {
    const { name, value } = event.target;
    setPodaciForme((prethodno) => ({
      ...prethodno,
      [name]: value
    }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setGreska('');

    if (podaciForme.naziv.trim() === '') {
      setGreska('Naziv putovanja je obavezan.');
      return;
    }

    if (podaciForme.pocetniDatum === '' || podaciForme.krajnjiDatum === '') {
      setGreska('Pocetni i krajnji datum su obavezni.');
      return;
    }

    if (new Date(podaciForme.krajnjiDatum) < new Date(podaciForme.pocetniDatum)) {
      setGreska('Krajnji datum ne moze biti prije pocetnog datuma.');
      return;
    }

    if (Number(podaciForme.planiraniBudzet) < 0) {
      setGreska('Budzet ne moze biti negativan.');
      return;
    }

    await onSubmit(podaciForme);

    if (mod === 'kreiranje') {
      setPodaciForme(prazanPlanPutovanja);
    }
  };

  return (
    <form className="forma-plana" onSubmit={handleSubmit}>
      <div className="mreza-forme">
        <label>
          Naziv putovanja
          <input
            name="naziv"
            value={podaciForme.naziv}
            onChange={handleChange}
            placeholder="Npr. Prag 2026"
          />
        </label>

        <label>
          Planirani budzet
          <input
            name="planiraniBudzet"
            type="number"
            min="0"
            step="0.01"
            value={podaciForme.planiraniBudzet}
            onChange={handleChange}
            placeholder="0.00"
          />
        </label>

        <label>
          Pocetni datum
          <input
            name="pocetniDatum"
            type="date"
            value={podaciForme.pocetniDatum}
            onChange={handleChange}
          />
        </label>

        <label>
          Krajnji datum
          <input
            name="krajnjiDatum"
            type="date"
            value={podaciForme.krajnjiDatum}
            onChange={handleChange}
          />
        </label>
      </div>

      <label>
        Kratak opis
        <textarea
          name="kratakOpis"
          value={podaciForme.kratakOpis}
          onChange={handleChange}
          rows="3"
        />
      </label>

      <label>
        Opste napomene
        <textarea
          name="opsteNapomene"
          value={podaciForme.opsteNapomene}
          onChange={handleChange}
          rows="3"
        />
      </label>

      {greska && <p className="poruka greska">{greska}</p>}

      <div className="akcije-forme">
        <button type="submit" disabled={disabled}>
          {mod === 'izmjena' ? 'Sacuvaj izmjene' : 'Sacuvaj plan'}
        </button>
        {mod === 'izmjena' && (
          <button className="dugme-sporedno" type="button" onClick={onCancel} disabled={disabled}>
            Odustani
          </button>
        )}
      </div>
    </form>
  );
}
