import { createContext, useContext, useReducer } from 'react';

const AppContext = createContext();

const initialState = {
  tripPlans: [],
  loading: false,
  error: '',
  successMessage: ''
};

function appReducer(state, action) {
  switch (action.type) {
    case 'requestStarted':
      return { ...state, loading: true, error: '', successMessage: '' };
    case 'tripPlansLoaded':
      return { ...state, loading: false, tripPlans: action.payload };
    case 'tripPlanCreated':
      return {
        ...state,
        loading: false,
        tripPlans: [...state.tripPlans, action.payload],
        successMessage: 'Plan putovanja je sacuvan.'
      };
    case 'requestFailed':
      return { ...state, loading: false, error: action.payload };
    case 'clearMessages':
      return { ...state, error: '', successMessage: '' };
    default:
      return state;
  }
}

export function AppProvider({ children }) {
  const [state, dispatch] = useReducer(appReducer, initialState);

  return (
    <AppContext.Provider value={{ state, dispatch }}>
      {children}
    </AppContext.Provider>
  );
}

export function useAppContext() {
  const context = useContext(AppContext);
  if (!context) {
    throw new Error('useAppContext mora biti koristen unutar AppProvider komponente.');
  }

  return context;
}
