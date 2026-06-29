import { useEffect, useState } from 'react';
import { destinacijaUFormu, praznaDestinacija } from '../../models/destinacija.js';

export default function FormaDestinacije({ mod, destinacijaZaIzmjenu, disabled, onSubmit, onCancel }) {
  const [podaciForme, setPodaciForme] = useState(praznaDestinacija);
  const [greska, setGreska] = useState('');

  useEffect(() => {
    setPodaciForme(destinacijaUFormu(destinacijaZaIzmjenu));
    setGreska('');
  }, [destinacijaZaIzmjenu]);

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
      setGreska('Naziv destinacije je obavezan.');
      return;
    }

    if (podaciForme.lokacija.trim() === '') {
      setGreska('Lokacija destinacije je obavezna.');
      return;
    }

    if (podaciForme.datumDolaska === '' || podaciForme.datumOdlaska === '') {
      setGreska('Datum dolaska i datum odlaska su obavezni.');
      return;
    }

    if (new Date(podaciForme.datumOdlaska) < new Date(podaciForme.datumDolaska)) {
      setGreska('Datum odlaska ne moze biti prije datuma dolaska.');
      return;
    }

    await onSubmit(podaciForme);

    if (mod === 'kreiranje') {
      setPodaciForme(praznaDestinacija);
    }
  };

  return (
    <form className="forma-plana" onSubmit={handleSubmit}>
      <div className="mreza-forme">
        <label>
          Naziv destinacije
          <input
            name="naziv"
            value={podaciForme.naziv}
            onChange={handleChange}
            placeholder="Npr. Bec"
          />
        </label>

        <label>
          Lokacija
          <input
            name="lokacija"
            value={podaciForme.lokacija}
            onChange={handleChange}
            placeholder="Npr. Austrija"
          />
        </label>

        <label>
          Datum dolaska
          <input
            name="datumDolaska"
            type="date"
            value={podaciForme.datumDolaska}
            onChange={handleChange}
          />
        </label>

        <label>
          Datum odlaska
          <input
            name="datumOdlaska"
            type="date"
            value={podaciForme.datumOdlaska}
            onChange={handleChange}
          />
        </label>
      </div>

      <label>
        Kratak opis ili napomena
        <textarea
          name="kratakOpis"
          value={podaciForme.kratakOpis}
          onChange={handleChange}
          rows="3"
        />
      </label>

      {greska && <p className="poruka greska">{greska}</p>}

      <div className="akcije-forme">
        <button type="submit" disabled={disabled}>
          {mod === 'izmjena' ? 'Sacuvaj izmjene' : 'Dodaj destinaciju'}
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
