import { AppProvider } from './context/AppContext.jsx';
import Header from './components/layout/Header.jsx';
import TripPlansPage from './pages/TripPlansPage.jsx';

export default function App() {
  return (
    <AppProvider>
      <Header />
      <TripPlansPage />
    </AppProvider>
  );
}
