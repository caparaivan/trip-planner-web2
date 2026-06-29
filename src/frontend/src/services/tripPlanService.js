import { apiRequest } from './apiClient.js';

export function getTripPlans() {
  return apiRequest('/api/trip-plans');
}

export function createTripPlan(tripPlan) {
  return apiRequest('/api/trip-plans', {
    method: 'POST',
    body: {
      ...tripPlan,
      plannedBudget: Number(tripPlan.plannedBudget)
    }
  });
}
