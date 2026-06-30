import { createContext, useContext, useReducer } from 'react';

const AppContext = createContext();

const pocetnoStanje = {
  planoviPutovanja: [],
  odabraniPlan: null,
  destinacije: [],
  aktivnosti: [],
  ucitavanje: false,
  greska: '',
  porukaUspjeha: ''
};

function appReducer(stanje, akcija) {
  switch (akcija.type) {
    case 'zahtjevPokrenut':
      return { ...stanje, ucitavanje: true, greska: '', porukaUspjeha: '' };
    case 'planoviUcitani':
      return { ...stanje, ucitavanje: false, planoviPutovanja: akcija.payload };
    case 'detaljiPlanaUcitani':
      return { ...stanje, ucitavanje: false, odabraniPlan: akcija.payload };
    case 'destinacijeUcitane':
      return { ...stanje, ucitavanje: false, destinacije: akcija.payload };
    case 'aktivnostiUcitane':
      return { ...stanje, ucitavanje: false, aktivnosti: sortirajAktivnosti(akcija.payload) };
    case 'planKreiran':
      return {
        ...stanje,
        ucitavanje: false,
        planoviPutovanja: [...stanje.planoviPutovanja, akcija.payload],
        odabraniPlan: akcija.payload,
        destinacije: [],
        aktivnosti: [],
        porukaUspjeha: 'Plan putovanja je sacuvan.'
      };
    case 'planIzmijenjen':
      return {
        ...stanje,
        ucitavanje: false,
        planoviPutovanja: stanje.planoviPutovanja.map((plan) =>
          plan.id === akcija.payload.id ? akcija.payload : plan
        ),
        odabraniPlan: akcija.payload,
        porukaUspjeha: 'Plan putovanja je izmijenjen.'
      };
    case 'planObrisan':
      return {
        ...stanje,
        ucitavanje: false,
        planoviPutovanja: stanje.planoviPutovanja.filter((plan) => plan.id !== akcija.payload),
        odabraniPlan: stanje.odabraniPlan?.id === akcija.payload ? null : stanje.odabraniPlan,
        destinacije: stanje.odabraniPlan?.id === akcija.payload ? [] : stanje.destinacije,
        aktivnosti: stanje.odabraniPlan?.id === akcija.payload ? [] : stanje.aktivnosti,
        porukaUspjeha: 'Plan putovanja je obrisan.'
      };
    case 'destinacijaKreirana':
      return {
        ...stanje,
        ucitavanje: false,
        destinacije: [...stanje.destinacije, akcija.payload],
        porukaUspjeha: 'Destinacija je sacuvana.'
      };
    case 'destinacijaIzmijenjena':
      return {
        ...stanje,
        ucitavanje: false,
        destinacije: stanje.destinacije.map((destinacija) =>
          destinacija.id === akcija.payload.id ? akcija.payload : destinacija
        ),
        porukaUspjeha: 'Destinacija je izmijenjena.'
      };
    case 'destinacijaObrisana':
      return {
        ...stanje,
        ucitavanje: false,
        destinacije: stanje.destinacije.filter((destinacija) => destinacija.id !== akcija.payload),
        porukaUspjeha: 'Destinacija je obrisana.'
      };
    case 'aktivnostKreirana':
      return {
        ...stanje,
        ucitavanje: false,
        aktivnosti: sortirajAktivnosti([...stanje.aktivnosti, akcija.payload]),
        porukaUspjeha: 'Aktivnost je sacuvana.'
      };
    case 'aktivnostIzmijenjena':
      return {
        ...stanje,
        ucitavanje: false,
        aktivnosti: sortirajAktivnosti(
          stanje.aktivnosti.map((aktivnost) =>
            aktivnost.id === akcija.payload.id ? akcija.payload : aktivnost
          )
        ),
        porukaUspjeha: 'Aktivnost je izmijenjena.'
      };
    case 'aktivnostObrisana':
      return {
        ...stanje,
        ucitavanje: false,
        aktivnosti: stanje.aktivnosti.filter((aktivnost) => aktivnost.id !== akcija.payload),
        porukaUspjeha: 'Aktivnost je obrisana.'
      };
    case 'odabirPonisten':
      return { ...stanje, odabraniPlan: null, destinacije: [], aktivnosti: [] };
    case 'zahtjevNeuspjesan':
      return { ...stanje, ucitavanje: false, greska: akcija.payload };
    case 'porukeOciscene':
      return { ...stanje, greska: '', porukaUspjeha: '' };
    default:
      return stanje;
  }
}

function sortirajAktivnosti(aktivnosti) {
  return [...aktivnosti].sort((prva, druga) => {
    const prviDatum = `${prva.datum || ''} ${prva.vrijeme || '99:99:99'}`;
    const drugiDatum = `${druga.datum || ''} ${druga.vrijeme || '99:99:99'}`;
    return prviDatum.localeCompare(drugiDatum);
  });
}

export function AppProvider({ children }) {
  const [stanje, dispatch] = useReducer(appReducer, pocetnoStanje);

  return (
    <AppContext.Provider value={{ stanje, dispatch }}>
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
