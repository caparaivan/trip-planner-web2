import { useEffect, useState } from 'react';
import { kategorijeTroskova, prazanTrosak, trosakIzForme, trosakUFormu } from '../../models/trosak.js';

export default function FormaTroska({ mod, trosakZaIzmjenu, disabled, onSubmit, onCancel }) {
  const [podaciForme, setPodaciForme] = useState(prazanTrosak);
  const [greska, setGreska] = useState('');

  useEffect(() => {
    setPodaciForme(trosakUFormu(trosakZaIzmjenu));
    setGreska('');
  }, [trosakZaIzmjenu]);

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
      setGreska('Naziv troska je obavezan.');
      return;
    }

    if (podaciForme.datum === '') {
      setGreska('Datum troska je obavezan.');
      return;
    }

    if (Number(podaciForme.iznos || 0) < 0) {
      setGreska('Iznos troska ne moze biti negativan.');
      return;
    }

    await onSubmit(trosakIzForme(podaciForme));

    if (mod === 'kreiranje') {
      setPodaciForme(prazanTrosak);
    }
  };

  return (
    <form className="forma-plana" onSubmit={handleSubmit}>
      <div className="mreza-forme">
        <label>
          Naziv troska
          <input
            name="naziv"
            value={podaciForme.naziv}
            onChange={handleChange}
            placeholder="Npr. Avio karta"
          />
        </label>

        <label>
          Kategorija
          <select name="kategorija" value={podaciForme.kategorija} onChange={handleChange}>
            {kategorijeTroskova.map((kategorija) => (
              <option value={kategorija} key={kategorija}>
                {kategorija}
              </option>
            ))}
          </select>
        </label>

        <label>
          Iznos
          <input
            min="0"
            name="iznos"
            step="0.01"
            type="number"
            value={podaciForme.iznos}
            onChange={handleChange}
          />
        </label>

        <label>
          Datum
          <input name="datum" type="date" value={podaciForme.datum} onChange={handleChange} />
        </label>
      </div>

      <label>
        Opis
        <textarea name="opis" value={podaciForme.opis} onChange={handleChange} rows="3" />
      </label>

      {greska && <p className="poruka greska">{greska}</p>}

      <div className="akcije-forme">
        <button type="submit" disabled={disabled}>
          {mod === 'izmjena' ? 'Sacuvaj izmjene' : 'Dodaj trosak'}
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
