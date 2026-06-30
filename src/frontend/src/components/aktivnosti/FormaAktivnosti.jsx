import { useEffect, useState } from 'react';
import {
  aktivnostIzForme,
  aktivnostUFormu,
  praznaAktivnost,
  statusiAktivnosti
} from '../../models/aktivnost.js';

export default function FormaAktivnosti({ mod, aktivnostZaIzmjenu, disabled, onSubmit, onCancel }) {
  const [podaciForme, setPodaciForme] = useState(praznaAktivnost);
  const [greska, setGreska] = useState('');

  useEffect(() => {
    setPodaciForme(aktivnostUFormu(aktivnostZaIzmjenu));
    setGreska('');
  }, [aktivnostZaIzmjenu]);

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
      setGreska('Naziv aktivnosti je obavezan.');
      return;
    }

    if (podaciForme.datum === '') {
      setGreska('Datum aktivnosti je obavezan.');
      return;
    }

    if (Number(podaciForme.procijenjeniTrosak || 0) < 0) {
      setGreska('Procijenjeni trosak ne moze biti negativan.');
      return;
    }

    await onSubmit(aktivnostIzForme(podaciForme));

    if (mod === 'kreiranje') {
      setPodaciForme(praznaAktivnost);
    }
  };

  return (
    <form className="forma-plana" onSubmit={handleSubmit}>
      <div className="mreza-forme">
        <label>
          Naziv aktivnosti
          <input
            name="naziv"
            value={podaciForme.naziv}
            onChange={handleChange}
            placeholder="Npr. Obilazak muzeja"
          />
        </label>

        <label>
          Datum
          <input
            name="datum"
            type="date"
            value={podaciForme.datum}
            onChange={handleChange}
          />
        </label>

        <label>
          Vrijeme
          <input
            name="vrijeme"
            type="time"
            value={podaciForme.vrijeme}
            onChange={handleChange}
          />
        </label>

        <label>
          Lokacija
          <input
            name="lokacija"
            value={podaciForme.lokacija}
            onChange={handleChange}
            placeholder="Npr. Centar grada"
          />
        </label>

        <label>
          Procijenjeni trosak
          <input
            min="0"
            name="procijenjeniTrosak"
            step="0.01"
            type="number"
            value={podaciForme.procijenjeniTrosak}
            onChange={handleChange}
          />
        </label>

        <label>
          Status
          <select name="status" value={podaciForme.status} onChange={handleChange}>
            {statusiAktivnosti.map((status) => (
              <option value={status} key={status}>
                {status}
              </option>
            ))}
          </select>
        </label>
      </div>

      <label>
        Opis
        <textarea name="opis" value={podaciForme.opis} onChange={handleChange} rows="3" />
      </label>

      {greska && <p className="poruka greska">{greska}</p>}

      <div className="akcije-forme">
        <button type="submit" disabled={disabled}>
          {mod === 'izmjena' ? 'Sacuvaj izmjene' : 'Dodaj aktivnost'}
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
