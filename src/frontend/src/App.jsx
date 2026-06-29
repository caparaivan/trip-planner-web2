import { AppProvider } from './context/AppContext.jsx';
import Zaglavlje from './components/layout/Zaglavlje.jsx';
import PlanoviPutovanjaPage from './pages/PlanoviPutovanjaPage.jsx';

export default function App() {
  return (
    <AppProvider>
      <Zaglavlje />
      <PlanoviPutovanjaPage />
    </AppProvider>
  );
}
