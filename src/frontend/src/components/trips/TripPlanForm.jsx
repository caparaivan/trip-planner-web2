import { useState } from 'react';
import { emptyTripPlan } from '../../models/tripPlan.js';

export default function TripPlanForm({ disabled, onSubmit }) {
  const [formData, setFormData] = useState(emptyTripPlan);
  const [error, setError] = useState('');

  const handleChange = (event) => {
    const { name, value } = event.target;
    setFormData((previous) => ({
      ...previous,
      [name]: value
    }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError('');

    if (formData.title.trim() === '') {
      setError('Naziv putovanja je obavezan.');
      return;
    }

    if (formData.startDate === '' || formData.endDate === '') {
      setError('Pocetni i krajnji datum su obavezni.');
      return;
    }

    if (new Date(formData.endDate) < new Date(formData.startDate)) {
      setError('Krajnji datum ne moze biti prije pocetnog datuma.');
      return;
    }

    if (Number(formData.plannedBudget) < 0) {
      setError('Budzet ne moze biti negativan.');
      return;
    }

    await onSubmit(formData);
    setFormData(emptyTripPlan);
  };

  return (
    <form className="trip-form" onSubmit={handleSubmit}>
      <div className="form-grid">
        <label>
          Naziv putovanja
          <input
            name="title"
            value={formData.title}
            onChange={handleChange}
            placeholder="Npr. Prag 2026"
          />
        </label>

        <label>
          Planirani budzet
          <input
            name="plannedBudget"
            type="number"
            min="0"
            step="0.01"
            value={formData.plannedBudget}
            onChange={handleChange}
            placeholder="0.00"
          />
        </label>

        <label>
          Pocetni datum
          <input
            name="startDate"
            type="date"
            value={formData.startDate}
            onChange={handleChange}
          />
        </label>

        <label>
          Krajnji datum
          <input
            name="endDate"
            type="date"
            value={formData.endDate}
            onChange={handleChange}
          />
        </label>
      </div>

      <label>
        Kratak opis
        <textarea
          name="description"
          value={formData.description}
          onChange={handleChange}
          rows="3"
        />
      </label>

      <label>
        Napomene
        <textarea
          name="notes"
          value={formData.notes}
          onChange={handleChange}
          rows="3"
        />
      </label>

      {error && <p className="message error">{error}</p>}

      <button type="submit" disabled={disabled}>
        Sacuvaj plan
      </button>
    </form>
  );
}
