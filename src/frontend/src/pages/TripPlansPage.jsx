import { useEffect } from 'react';
import TripPlanForm from '../components/trips/TripPlanForm.jsx';
import TripPlanList from '../components/trips/TripPlanList.jsx';
import { useAppContext } from '../context/AppContext.jsx';
import { createTripPlan, getTripPlans } from '../services/tripPlanService.js';

export default function TripPlansPage() {
  const { state, dispatch } = useAppContext();

  useEffect(() => {
    let isActive = true;

    async function loadTripPlans() {
      dispatch({ type: 'requestStarted' });
      try {
        const data = await getTripPlans();
        if (isActive) {
          dispatch({ type: 'tripPlansLoaded', payload: data || [] });
        }
      } catch (error) {
        if (isActive) {
          dispatch({ type: 'requestFailed', payload: error.message });
        }
      }
    }

    loadTripPlans();

    return () => {
      isActive = false;
    };
  }, [dispatch]);

  const handleCreateTripPlan = async (formData) => {
    dispatch({ type: 'requestStarted' });
    try {
      const createdTripPlan = await createTripPlan(formData);
      dispatch({ type: 'tripPlanCreated', payload: createdTripPlan });
    } catch (error) {
      dispatch({ type: 'requestFailed', payload: error.message });
    }
  };

  return (
    <main className="page-shell">
      <section className="content-section">
        <div className="section-heading">
          <p className="eyebrow">Planovi putovanja</p>
          <h2>Novi plan</h2>
        </div>
        <TripPlanForm disabled={state.loading} onSubmit={handleCreateTripPlan} />
      </section>

      <section className="content-section">
        <div className="section-heading">
          <p className="eyebrow">Pregled</p>
          <h2>Sacuvani planovi</h2>
        </div>

        {state.loading && <p className="message info">Ucitavanje podataka...</p>}
        {state.error && <p className="message error">{state.error}</p>}
        {state.successMessage && <p className="message success">{state.successMessage}</p>}

        <TripPlanList tripPlans={state.tripPlans} />
      </section>
    </main>
  );
}
