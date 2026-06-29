import { useEffect, useState } from 'react';
import FormaDestinacije from '../components/destinacije/FormaDestinacije.jsx';
import ListaDestinacija from '../components/destinacije/ListaDestinacija.jsx';
import DetaljiPlanaPutovanja from '../components/planovi/DetaljiPlanaPutovanja.jsx';
import FormaPlanaPutovanja from '../components/planovi/FormaPlanaPutovanja.jsx';
import ListaPlanovaPutovanja from '../components/planovi/ListaPlanovaPutovanja.jsx';
import { useAppContext } from '../context/AppContext.jsx';
import {
  kreirajDestinaciju,
  obrisiDestinaciju,
  izmijeniDestinaciju,
  vratiDestinacije
} from '../services/destinacijaService.js';
import {
  kreirajPlanPutovanja,
  obrisiPlanPutovanja,
  izmijeniPlanPutovanja,
  vratiPlanPutovanja,
  vratiPlanovePutovanja
} from '../services/planPutovanjaService.js';

export default function PlanoviPutovanjaPage() {
  const { stanje, dispatch } = useAppContext();
  const [modForme, setModForme] = useState('kreiranje');
  const [planZaIzmjenu, setPlanZaIzmjenu] = useState(null);
  const [modFormeDestinacije, setModFormeDestinacije] = useState('kreiranje');
  const [destinacijaZaIzmjenu, setDestinacijaZaIzmjenu] = useState(null);

  useEffect(() => {
    let aktivno = true;

    async function ucitajPlanovePutovanja() {
      dispatch({ type: 'zahtjevPokrenut' });
      try {
        const podaci = await vratiPlanovePutovanja();
        if (aktivno) {
          dispatch({ type: 'planoviUcitani', payload: podaci || [] });
        }
      } catch (error) {
        if (aktivno) {
          dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
        }
      }
    }

    ucitajPlanovePutovanja();

    return () => {
      aktivno = false;
    };
  }, [dispatch]);

  const handleSacuvajPlan = async (podaciForme) => {
    dispatch({ type: 'zahtjevPokrenut' });
    try {
      if (modForme === 'izmjena' && planZaIzmjenu) {
        const izmijenjeniPlan = await izmijeniPlanPutovanja(planZaIzmjenu.id, podaciForme);
        dispatch({ type: 'planIzmijenjen', payload: izmijenjeniPlan });
        setModForme('kreiranje');
        setPlanZaIzmjenu(null);
        return;
      }

      const kreiraniPlan = await kreirajPlanPutovanja(podaciForme);
      dispatch({ type: 'planKreiran', payload: kreiraniPlan });
      setModFormeDestinacije('kreiranje');
      setDestinacijaZaIzmjenu(null);
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handleDetalji = async (id) => {
    dispatch({ type: 'zahtjevPokrenut' });
    try {
      const planPutovanja = await vratiPlanPutovanja(id);
      const destinacije = await vratiDestinacije(id);
      dispatch({ type: 'detaljiPlanaUcitani', payload: planPutovanja });
      dispatch({ type: 'destinacijeUcitane', payload: destinacije || [] });
      setModFormeDestinacije('kreiranje');
      setDestinacijaZaIzmjenu(null);
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handleIzmjena = async (plan) => {
    try {
      const planPutovanja = await vratiPlanPutovanja(plan.id);
      const destinacije = await vratiDestinacije(plan.id);
      setPlanZaIzmjenu(planPutovanja);
      setModForme('izmjena');
      dispatch({ type: 'detaljiPlanaUcitani', payload: planPutovanja });
      dispatch({ type: 'destinacijeUcitane', payload: destinacije || [] });
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handleBrisanje = async (id) => {
    const potvrdjeno = window.confirm('Da li ste sigurni da zelite obrisati plan putovanja?');
    if (!potvrdjeno) {
      return;
    }

    dispatch({ type: 'zahtjevPokrenut' });
    try {
      await obrisiPlanPutovanja(id);
      dispatch({ type: 'planObrisan', payload: id });
      if (planZaIzmjenu?.id === id) {
        setPlanZaIzmjenu(null);
        setModForme('kreiranje');
      }
      setDestinacijaZaIzmjenu(null);
      setModFormeDestinacije('kreiranje');
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handleOdustani = () => {
    setPlanZaIzmjenu(null);
    setModForme('kreiranje');
  };

  const handleSacuvajDestinaciju = async (podaciForme) => {
    if (!stanje.odabraniPlan) {
      return;
    }

    dispatch({ type: 'zahtjevPokrenut' });
    try {
      if (modFormeDestinacije === 'izmjena' && destinacijaZaIzmjenu) {
        const izmijenjenaDestinacija = await izmijeniDestinaciju(
          stanje.odabraniPlan.id,
          destinacijaZaIzmjenu.id,
          podaciForme
        );
        dispatch({ type: 'destinacijaIzmijenjena', payload: izmijenjenaDestinacija });
        setDestinacijaZaIzmjenu(null);
        setModFormeDestinacije('kreiranje');
        return;
      }

      const kreiranaDestinacija = await kreirajDestinaciju(stanje.odabraniPlan.id, podaciForme);
      dispatch({ type: 'destinacijaKreirana', payload: kreiranaDestinacija });
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handleIzmjenaDestinacije = (destinacija) => {
    setDestinacijaZaIzmjenu(destinacija);
    setModFormeDestinacije('izmjena');
  };

  const handleBrisanjeDestinacije = async (id) => {
    if (!stanje.odabraniPlan) {
      return;
    }

    const potvrdjeno = window.confirm('Da li ste sigurni da zelite obrisati destinaciju?');
    if (!potvrdjeno) {
      return;
    }

    dispatch({ type: 'zahtjevPokrenut' });
    try {
      await obrisiDestinaciju(stanje.odabraniPlan.id, id);
      dispatch({ type: 'destinacijaObrisana', payload: id });
      if (destinacijaZaIzmjenu?.id === id) {
        setDestinacijaZaIzmjenu(null);
        setModFormeDestinacije('kreiranje');
      }
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handleOdustaniDestinacija = () => {
    setDestinacijaZaIzmjenu(null);
    setModFormeDestinacije('kreiranje');
  };

  return (
    <main className="okvir-stranice">
      <section className="sekcija-sadrzaja">
        <div className="naslov-sekcije">
          <p className="oznaka">Planovi putovanja</p>
          <h2>{modForme === 'izmjena' ? 'Izmjena plana' : 'Novi plan'}</h2>
        </div>
        <FormaPlanaPutovanja
          mod={modForme}
          planZaIzmjenu={planZaIzmjenu}
          disabled={stanje.ucitavanje}
          onSubmit={handleSacuvajPlan}
          onCancel={handleOdustani}
        />
      </section>

      <section className="sekcija-sadrzaja">
        <div className="naslov-sekcije">
          <p className="oznaka">Pregled</p>
          <h2>Sacuvani planovi</h2>
        </div>

        {stanje.ucitavanje && <p className="poruka info">Ucitavanje podataka...</p>}
        {stanje.greska && <p className="poruka greska">{stanje.greska}</p>}
        {stanje.porukaUspjeha && <p className="poruka uspjeh">{stanje.porukaUspjeha}</p>}

        <ListaPlanovaPutovanja
          planoviPutovanja={stanje.planoviPutovanja}
          odabraniPlanId={stanje.odabraniPlan?.id}
          onDetalji={handleDetalji}
          onIzmjena={handleIzmjena}
          onBrisanje={handleBrisanje}
        />
      </section>

      <section className="sekcija-sadrzaja">
        <div className="naslov-sekcije">
          <p className="oznaka">Detalji</p>
          <h2>Detalji plana</h2>
        </div>
        <DetaljiPlanaPutovanja planPutovanja={stanje.odabraniPlan} />
      </section>

      <section className="sekcija-sadrzaja">
        <div className="naslov-sekcije">
          <p className="oznaka">Destinacije</p>
          <h2>{modFormeDestinacije === 'izmjena' ? 'Izmjena destinacije' : 'Nova destinacija'}</h2>
        </div>

        {!stanje.odabraniPlan && <p className="prazno-stanje">Prvo izaberite plan putovanja.</p>}

        {stanje.odabraniPlan && (
          <div className="mreza-destinacija">
            <FormaDestinacije
              mod={modFormeDestinacije}
              destinacijaZaIzmjenu={destinacijaZaIzmjenu}
              disabled={stanje.ucitavanje}
              onSubmit={handleSacuvajDestinaciju}
              onCancel={handleOdustaniDestinacija}
            />
            <ListaDestinacija
              destinacije={stanje.destinacije}
              onIzmjena={handleIzmjenaDestinacije}
              onBrisanje={handleBrisanjeDestinacije}
            />
          </div>
        )}
      </section>
    </main>
  );
}
