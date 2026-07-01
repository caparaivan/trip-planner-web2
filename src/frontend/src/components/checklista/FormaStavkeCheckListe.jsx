import { useEffect, useState } from 'react';
import {
  praznaStavkaCheckListe,
  stavkaCheckListeIzForme,
  stavkaCheckListeUFormu
} from '../../models/stavkaCheckListe.js';

export default function FormaStavkeCheckListe({ mod, stavkaZaIzmjenu, disabled, onSubmit, onCancel }) {
  const [podaciForme, setPodaciForme] = useState(praznaStavkaCheckListe);
  const [greska, setGreska] = useState('');

  useEffect(() => {
    setPodaciForme(stavkaCheckListeUFormu(stavkaZaIzmjenu));
    setGreska('');
  }, [stavkaZaIzmjenu]);

  const handleChange = (event) => {
    const { checked, name, type, value } = event.target;
    setPodaciForme((prethodno) => ({
      ...prethodno,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setGreska('');

    if (podaciForme.naziv.trim() === '') {
      setGreska('Naziv stavke je obavezan.');
      return;
    }

    await onSubmit(stavkaCheckListeIzForme(podaciForme));

    if (mod === 'kreiranje') {
      setPodaciForme(praznaStavkaCheckListe);
    }
  };

  return (
    <form className="forma-plana" onSubmit={handleSubmit}>
      <label>
        Naziv stavke
        <input
          name="naziv"
          value={podaciForme.naziv}
          onChange={handleChange}
          placeholder="Npr. Pasos, karta, punjac"
        />
      </label>

      <label className="red-checkliste">
        <input
          checked={podaciForme.zavrseno}
          name="zavrseno"
          type="checkbox"
          onChange={handleChange}
        />
        <span>Stavka je zavrsena</span>
      </label>

      {greska && <p className="poruka greska">{greska}</p>}

      <div className="akcije-forme">
        <button type="submit" disabled={disabled}>
          {mod === 'izmjena' ? 'Sacuvaj izmjene' : 'Dodaj stavku'}
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
