import { createContext, useContext, useReducer } from 'react';

const AppContext = createContext();

const pocetnoStanje = {
  planoviPutovanja: [],
  odabraniPlan: null,
  destinacije: [],
  aktivnosti: [],
  troskovi: [],
  stavkeCheckListe: [],
  pregledBudzeta: null,
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
    case 'troskoviUcitani':
      return { ...stanje, ucitavanje: false, troskovi: sortirajTroskove(akcija.payload) };
    case 'stavkeCheckListeUcitane':
      return { ...stanje, ucitavanje: false, stavkeCheckListe: sortirajStavkeCheckListe(akcija.payload) };
    case 'pregledBudzetaUcitan':
      return {
        ...stanje,
        ucitavanje: false,
        pregledBudzeta: akcija.payload,
        odabraniPlan: primijeniPregledBudzeta(stanje.odabraniPlan, akcija.payload),
        planoviPutovanja: stanje.planoviPutovanja.map((plan) =>
          plan.id === akcija.payload.planPutovanjaId ? primijeniPregledBudzeta(plan, akcija.payload) : plan
        )
      };
    case 'planKreiran':
      return {
        ...stanje,
        ucitavanje: false,
        planoviPutovanja: [...stanje.planoviPutovanja, akcija.payload],
        odabraniPlan: akcija.payload,
        destinacije: [],
        aktivnosti: [],
        troskovi: [],
        stavkeCheckListe: [],
        pregledBudzeta: null,
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
        pregledBudzeta: uskladiPregledSaPlanom(stanje.pregledBudzeta, akcija.payload),
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
        troskovi: stanje.odabraniPlan?.id === akcija.payload ? [] : stanje.troskovi,
        stavkeCheckListe: stanje.odabraniPlan?.id === akcija.payload ? [] : stanje.stavkeCheckListe,
        pregledBudzeta: stanje.odabraniPlan?.id === akcija.payload ? null : stanje.pregledBudzeta,
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
    case 'trosakKreiran':
      return {
        ...stanje,
        ucitavanje: false,
        troskovi: sortirajTroskove([...stanje.troskovi, akcija.payload]),
        porukaUspjeha: 'Trosak je sacuvan.'
      };
    case 'trosakIzmijenjen':
      return {
        ...stanje,
        ucitavanje: false,
        troskovi: sortirajTroskove(
          stanje.troskovi.map((trosak) => (trosak.id === akcija.payload.id ? akcija.payload : trosak))
        ),
        porukaUspjeha: 'Trosak je izmijenjen.'
      };
    case 'trosakObrisan':
      return {
        ...stanje,
        ucitavanje: false,
        troskovi: stanje.troskovi.filter((trosak) => trosak.id !== akcija.payload),
        porukaUspjeha: 'Trosak je obrisan.'
      };
    case 'stavkaCheckListeKreirana':
      return {
        ...stanje,
        ucitavanje: false,
        stavkeCheckListe: sortirajStavkeCheckListe([...stanje.stavkeCheckListe, akcija.payload]),
        porukaUspjeha: 'Stavka checkliste je sacuvana.'
      };
    case 'stavkaCheckListeIzmijenjena':
      return {
        ...stanje,
        ucitavanje: false,
        stavkeCheckListe: sortirajStavkeCheckListe(
          stanje.stavkeCheckListe.map((stavka) =>
            stavka.id === akcija.payload.id ? akcija.payload : stavka
          )
        ),
        porukaUspjeha: 'Stavka checkliste je izmijenjena.'
      };
    case 'stavkaCheckListeObrisana':
      return {
        ...stanje,
        ucitavanje: false,
        stavkeCheckListe: stanje.stavkeCheckListe.filter((stavka) => stavka.id !== akcija.payload),
        porukaUspjeha: 'Stavka checkliste je obrisana.'
      };
    case 'odabirPonisten':
      return {
        ...stanje,
        odabraniPlan: null,
        destinacije: [],
        aktivnosti: [],
        troskovi: [],
        stavkeCheckListe: [],
        pregledBudzeta: null
      };
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

function sortirajTroskove(troskovi) {
  return [...troskovi].sort((prvi, drugi) => {
    const datum = (drugi.datum || '').localeCompare(prvi.datum || '');
    return datum !== 0 ? datum : prvi.naziv.localeCompare(drugi.naziv);
  });
}

function sortirajStavkeCheckListe(stavkeCheckListe) {
  return [...stavkeCheckListe].sort((prva, druga) => {
    if (prva.zavrseno !== druga.zavrseno) {
      return Number(prva.zavrseno) - Number(druga.zavrseno);
    }

    return (prva.naziv || '').localeCompare(druga.naziv || '');
  });
}

function primijeniPregledBudzeta(plan, pregledBudzeta) {
  if (!plan || plan.id !== pregledBudzeta.planPutovanjaId) {
    return plan;
  }

  return {
    ...plan,
    planiraniBudzet: pregledBudzeta.planiraniBudzet,
    ukupanTrosak: pregledBudzeta.ukupanTrosak,
    preostaliBudzet: pregledBudzeta.preostaliBudzet
  };
}

function uskladiPregledSaPlanom(pregledBudzeta, plan) {
  if (!pregledBudzeta || pregledBudzeta.planPutovanjaId !== plan.id) {
    return pregledBudzeta;
  }

  return {
    ...pregledBudzeta,
    planiraniBudzet: plan.planiraniBudzet,
    ukupanTrosak: plan.ukupanTrosak,
    preostaliBudzet: plan.preostaliBudzet
  };
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
