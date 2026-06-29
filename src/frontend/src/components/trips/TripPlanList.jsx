export default function TripPlanList({ tripPlans }) {
  if (tripPlans.length === 0) {
    return <p className="empty-state">Nema kreiranih planova putovanja.</p>;
  }

  return (
    <div className="trip-list">
      {tripPlans.map((plan) => (
        <article className="trip-item" key={plan.id}>
          <div>
            <h3>{plan.title}</h3>
            <p>{plan.description || 'Bez opisa'}</p>
          </div>
          <dl>
            <div>
              <dt>Period</dt>
              <dd>{formatDate(plan.startDate)} - {formatDate(plan.endDate)}</dd>
            </div>
            <div>
              <dt>Budzet</dt>
              <dd>{formatMoney(plan.plannedBudget)}</dd>
            </div>
            <div>
              <dt>Preostalo</dt>
              <dd>{formatMoney(plan.remainingBudget)}</dd>
            </div>
          </dl>
        </article>
      ))}
    </div>
  );
}

function formatDate(value) {
  return new Date(value).toLocaleDateString('sr-Latn-BA');
}

function formatMoney(value) {
  return `${Number(value || 0).toFixed(2)} KM`;
}
