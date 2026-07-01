import { useEffect, useState } from 'react';
import FormaAktivnosti from '../components/aktivnosti/FormaAktivnosti.jsx';
import KalendarAktivnosti from '../components/aktivnosti/KalendarAktivnosti.jsx';
import ListaAktivnosti from '../components/aktivnosti/ListaAktivnosti.jsx';
import FormaStavkeCheckListe from '../components/checklista/FormaStavkeCheckListe.jsx';
import ListaStavkiCheckListe from '../components/checklista/ListaStavkiCheckListe.jsx';
import PregledCheckListe from '../components/checklista/PregledCheckListe.jsx';
import FormaDestinacije from '../components/destinacije/FormaDestinacije.jsx';
import ListaDestinacija from '../components/destinacije/ListaDestinacija.jsx';
import FormaPlanaPutovanja from '../components/planovi/FormaPlanaPutovanja.jsx';
import ListaPlanovaPutovanja from '../components/planovi/ListaPlanovaPutovanja.jsx';
import PregledPlanaPutovanja from '../components/planovi/PregledPlanaPutovanja.jsx';
import FormaTroska from '../components/troskovi/FormaTroska.jsx';
import ListaTroskova from '../components/troskovi/ListaTroskova.jsx';
import PregledBudzeta from '../components/troskovi/PregledBudzeta.jsx';
import { useAppContext } from '../context/AppContext.jsx';
import {
  kreirajAktivnost,
  obrisiAktivnost,
  izmijeniAktivnost,
  vratiAktivnosti
} from '../services/aktivnostService.js';
import {
  kreirajDestinaciju,
  obrisiDestinaciju,
  izmijeniDestinaciju,
  vratiDestinacije
} from '../services/destinacijaService.js';
import {
  kreirajStavkuCheckListe,
  obrisiStavkuCheckListe,
  izmijeniStavkuCheckListe,
  vratiStavkeCheckListe
} from '../services/checkListaService.js';
import {
  kreirajPlanPutovanja,
  obrisiPlanPutovanja,
  izmijeniPlanPutovanja,
  vratiPlanPutovanja,
  vratiPlanovePutovanja
} from '../services/planPutovanjaService.js';
import {
  kreirajTrosak,
  obrisiTrosak,
  izmijeniTrosak,
  vratiPregledBudzeta,
  vratiTroskove
} from '../services/trosakService.js';

export default function PlanoviPutovanjaPage() {
  const { stanje, dispatch } = useAppContext();
  const [modForme, setModForme] = useState('kreiranje');
  const [planZaIzmjenu, setPlanZaIzmjenu] = useState(null);
  const [modFormeDestinacije, setModFormeDestinacije] = useState('kreiranje');
  const [destinacijaZaIzmjenu, setDestinacijaZaIzmjenu] = useState(null);
  const [modFormeAktivnosti, setModFormeAktivnosti] = useState('kreiranje');
  const [aktivnostZaIzmjenu, setAktivnostZaIzmjenu] = useState(null);
  const [modFormeTroska, setModFormeTroska] = useState('kreiranje');
  const [trosakZaIzmjenu, setTrosakZaIzmjenu] = useState(null);
  const [modFormeCheckListe, setModFormeCheckListe] = useState('kreiranje');
  const [stavkaCheckListeZaIzmjenu, setStavkaCheckListeZaIzmjenu] = useState(null);

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
      setModFormeAktivnosti('kreiranje');
      setAktivnostZaIzmjenu(null);
      setModFormeTroska('kreiranje');
      setTrosakZaIzmjenu(null);
      setModFormeCheckListe('kreiranje');
      setStavkaCheckListeZaIzmjenu(null);
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handleDetalji = async (id) => {
    dispatch({ type: 'zahtjevPokrenut' });
    try {
      const [planPutovanja, destinacije, aktivnosti, troskovi, pregledBudzeta, stavkeCheckListe] = await Promise.all([
        vratiPlanPutovanja(id),
        vratiDestinacije(id),
        vratiAktivnosti(id),
        vratiTroskove(id),
        vratiPregledBudzeta(id),
        vratiStavkeCheckListe(id)
      ]);
      dispatch({ type: 'detaljiPlanaUcitani', payload: planPutovanja });
      dispatch({ type: 'destinacijeUcitane', payload: destinacije || [] });
      dispatch({ type: 'aktivnostiUcitane', payload: aktivnosti || [] });
      dispatch({ type: 'troskoviUcitani', payload: troskovi || [] });
      dispatch({ type: 'pregledBudzetaUcitan', payload: pregledBudzeta });
      dispatch({ type: 'stavkeCheckListeUcitane', payload: stavkeCheckListe || [] });
      setModFormeDestinacije('kreiranje');
      setDestinacijaZaIzmjenu(null);
      setModFormeAktivnosti('kreiranje');
      setAktivnostZaIzmjenu(null);
      setModFormeTroska('kreiranje');
      setTrosakZaIzmjenu(null);
      setModFormeCheckListe('kreiranje');
      setStavkaCheckListeZaIzmjenu(null);
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handleIzmjena = async (plan) => {
    try {
      const [planPutovanja, destinacije, aktivnosti, troskovi, pregledBudzeta, stavkeCheckListe] = await Promise.all([
        vratiPlanPutovanja(plan.id),
        vratiDestinacije(plan.id),
        vratiAktivnosti(plan.id),
        vratiTroskove(plan.id),
        vratiPregledBudzeta(plan.id),
        vratiStavkeCheckListe(plan.id)
      ]);
      setPlanZaIzmjenu(planPutovanja);
      setModForme('izmjena');
      dispatch({ type: 'detaljiPlanaUcitani', payload: planPutovanja });
      dispatch({ type: 'destinacijeUcitane', payload: destinacije || [] });
      dispatch({ type: 'aktivnostiUcitane', payload: aktivnosti || [] });
      dispatch({ type: 'troskoviUcitani', payload: troskovi || [] });
      dispatch({ type: 'pregledBudzetaUcitan', payload: pregledBudzeta });
      dispatch({ type: 'stavkeCheckListeUcitane', payload: stavkeCheckListe || [] });
      setModFormeDestinacije('kreiranje');
      setDestinacijaZaIzmjenu(null);
      setModFormeAktivnosti('kreiranje');
      setAktivnostZaIzmjenu(null);
      setModFormeTroska('kreiranje');
      setTrosakZaIzmjenu(null);
      setModFormeCheckListe('kreiranje');
      setStavkaCheckListeZaIzmjenu(null);
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
      setAktivnostZaIzmjenu(null);
      setModFormeAktivnosti('kreiranje');
      setTrosakZaIzmjenu(null);
      setModFormeTroska('kreiranje');
      setStavkaCheckListeZaIzmjenu(null);
      setModFormeCheckListe('kreiranje');
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

  const handleSacuvajAktivnost = async (podaciForme) => {
    if (!stanje.odabraniPlan) {
      return;
    }

    dispatch({ type: 'zahtjevPokrenut' });
    try {
      if (modFormeAktivnosti === 'izmjena' && aktivnostZaIzmjenu) {
        const izmijenjenaAktivnost = await izmijeniAktivnost(
          stanje.odabraniPlan.id,
          aktivnostZaIzmjenu.id,
          podaciForme
        );
        dispatch({ type: 'aktivnostIzmijenjena', payload: izmijenjenaAktivnost });
        setAktivnostZaIzmjenu(null);
        setModFormeAktivnosti('kreiranje');
        return;
      }

      const kreiranaAktivnost = await kreirajAktivnost(stanje.odabraniPlan.id, podaciForme);
      dispatch({ type: 'aktivnostKreirana', payload: kreiranaAktivnost });
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handleIzmjenaAktivnosti = (aktivnost) => {
    setAktivnostZaIzmjenu(aktivnost);
    setModFormeAktivnosti('izmjena');
  };

  const handleBrisanjeAktivnosti = async (id) => {
    if (!stanje.odabraniPlan) {
      return;
    }

    const potvrdjeno = window.confirm('Da li ste sigurni da zelite obrisati aktivnost?');
    if (!potvrdjeno) {
      return;
    }

    dispatch({ type: 'zahtjevPokrenut' });
    try {
      await obrisiAktivnost(stanje.odabraniPlan.id, id);
      dispatch({ type: 'aktivnostObrisana', payload: id });
      if (aktivnostZaIzmjenu?.id === id) {
        setAktivnostZaIzmjenu(null);
        setModFormeAktivnosti('kreiranje');
      }
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handleOdustaniAktivnost = () => {
    setAktivnostZaIzmjenu(null);
    setModFormeAktivnosti('kreiranje');
  };

  const osvjeziPregledBudzeta = async (planPutovanjaId) => {
    const pregledBudzeta = await vratiPregledBudzeta(planPutovanjaId);
    dispatch({ type: 'pregledBudzetaUcitan', payload: pregledBudzeta });
  };

  const handleSacuvajTrosak = async (podaciForme) => {
    if (!stanje.odabraniPlan) {
      return;
    }

    dispatch({ type: 'zahtjevPokrenut' });
    try {
      if (modFormeTroska === 'izmjena' && trosakZaIzmjenu) {
        const izmijenjeniTrosak = await izmijeniTrosak(
          stanje.odabraniPlan.id,
          trosakZaIzmjenu.id,
          podaciForme
        );
        dispatch({ type: 'trosakIzmijenjen', payload: izmijenjeniTrosak });
        await osvjeziPregledBudzeta(stanje.odabraniPlan.id);
        setTrosakZaIzmjenu(null);
        setModFormeTroska('kreiranje');
        return;
      }

      const kreiraniTrosak = await kreirajTrosak(stanje.odabraniPlan.id, podaciForme);
      dispatch({ type: 'trosakKreiran', payload: kreiraniTrosak });
      await osvjeziPregledBudzeta(stanje.odabraniPlan.id);
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handleIzmjenaTroska = (trosak) => {
    setTrosakZaIzmjenu(trosak);
    setModFormeTroska('izmjena');
  };

  const handleBrisanjeTroska = async (id) => {
    if (!stanje.odabraniPlan) {
      return;
    }

    const potvrdjeno = window.confirm('Da li ste sigurni da zelite obrisati trosak?');
    if (!potvrdjeno) {
      return;
    }

    dispatch({ type: 'zahtjevPokrenut' });
    try {
      await obrisiTrosak(stanje.odabraniPlan.id, id);
      dispatch({ type: 'trosakObrisan', payload: id });
      await osvjeziPregledBudzeta(stanje.odabraniPlan.id);
      if (trosakZaIzmjenu?.id === id) {
        setTrosakZaIzmjenu(null);
        setModFormeTroska('kreiranje');
      }
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handleOdustaniTrosak = () => {
    setTrosakZaIzmjenu(null);
    setModFormeTroska('kreiranje');
  };

  const handleSacuvajStavkuCheckListe = async (podaciForme) => {
    if (!stanje.odabraniPlan) {
      return;
    }

    dispatch({ type: 'zahtjevPokrenut' });
    try {
      if (modFormeCheckListe === 'izmjena' && stavkaCheckListeZaIzmjenu) {
        const izmijenjenaStavka = await izmijeniStavkuCheckListe(
          stanje.odabraniPlan.id,
          stavkaCheckListeZaIzmjenu.id,
          podaciForme
        );
        dispatch({ type: 'stavkaCheckListeIzmijenjena', payload: izmijenjenaStavka });
        setStavkaCheckListeZaIzmjenu(null);
        setModFormeCheckListe('kreiranje');
        return;
      }

      const kreiranaStavka = await kreirajStavkuCheckListe(stanje.odabraniPlan.id, podaciForme);
      dispatch({ type: 'stavkaCheckListeKreirana', payload: kreiranaStavka });
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handlePromjenaStatusaStavkeCheckListe = async (stavka) => {
    if (!stanje.odabraniPlan) {
      return;
    }

    dispatch({ type: 'zahtjevPokrenut' });
    try {
      const izmijenjenaStavka = await izmijeniStavkuCheckListe(stanje.odabraniPlan.id, stavka.id, {
        naziv: stavka.naziv,
        zavrseno: !stavka.zavrseno
      });
      dispatch({ type: 'stavkaCheckListeIzmijenjena', payload: izmijenjenaStavka });

      if (stavkaCheckListeZaIzmjenu?.id === stavka.id) {
        setStavkaCheckListeZaIzmjenu(izmijenjenaStavka);
      }
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handleIzmjenaStavkeCheckListe = (stavka) => {
    setStavkaCheckListeZaIzmjenu(stavka);
    setModFormeCheckListe('izmjena');
  };

  const handleBrisanjeStavkeCheckListe = async (id) => {
    if (!stanje.odabraniPlan) {
      return;
    }

    const potvrdjeno = window.confirm('Da li ste sigurni da zelite obrisati stavku checkliste?');
    if (!potvrdjeno) {
      return;
    }

    dispatch({ type: 'zahtjevPokrenut' });
    try {
      await obrisiStavkuCheckListe(stanje.odabraniPlan.id, id);
      dispatch({ type: 'stavkaCheckListeObrisana', payload: id });
      if (stavkaCheckListeZaIzmjenu?.id === id) {
        setStavkaCheckListeZaIzmjenu(null);
        setModFormeCheckListe('kreiranje');
      }
    } catch (error) {
      dispatch({ type: 'zahtjevNeuspjesan', payload: error.message });
    }
  };

  const handleOdustaniStavkaCheckListe = () => {
    setStavkaCheckListeZaIzmjenu(null);
    setModFormeCheckListe('kreiranje');
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

      <section className="sekcija-sadrzaja" id="sekcija-pregled-plana">
        <div className="naslov-sekcije">
          <p className="oznaka">Pregled plana</p>
          <h2>Cjelokupan pregled putovanja</h2>
        </div>
        <PregledPlanaPutovanja
          planPutovanja={stanje.odabraniPlan}
          destinacije={stanje.destinacije}
          aktivnosti={stanje.aktivnosti}
          troskovi={stanje.troskovi}
          stavkeCheckListe={stanje.stavkeCheckListe}
          pregledBudzeta={stanje.pregledBudzeta}
        />
      </section>

      <section className="sekcija-sadrzaja" id="sekcija-destinacije">
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

      <section className="sekcija-sadrzaja" id="sekcija-troskovi">
        <div className="naslov-sekcije">
          <p className="oznaka">Troskovi</p>
          <h2>{modFormeTroska === 'izmjena' ? 'Izmjena troska' : 'Novi trosak'}</h2>
        </div>

        {!stanje.odabraniPlan && <p className="prazno-stanje">Prvo izaberite plan putovanja.</p>}

        {stanje.odabraniPlan && (
          <div className="mreza-troskova">
            <div className="panel-troskova">
              <FormaTroska
                mod={modFormeTroska}
                trosakZaIzmjenu={trosakZaIzmjenu}
                disabled={stanje.ucitavanje}
                onSubmit={handleSacuvajTrosak}
                onCancel={handleOdustaniTrosak}
              />
            </div>

            <div className="panel-troskova">
              <PregledBudzeta pregledBudzeta={stanje.pregledBudzeta} />
            </div>

            <div className="panel-troskova panel-troskova-sirok">
              <ListaTroskova
                troskovi={stanje.troskovi}
                onIzmjena={handleIzmjenaTroska}
                onBrisanje={handleBrisanjeTroska}
              />
            </div>
          </div>
        )}
      </section>

      <section className="sekcija-sadrzaja" id="sekcija-checklista">
        <div className="naslov-sekcije">
          <p className="oznaka">Checklista</p>
          <h2>{modFormeCheckListe === 'izmjena' ? 'Izmjena stavke' : 'Nova stavka'}</h2>
        </div>

        {!stanje.odabraniPlan && <p className="prazno-stanje">Prvo izaberite plan putovanja.</p>}

        {stanje.odabraniPlan && (
          <div className="mreza-checkliste">
            <div className="panel-checkliste">
              <FormaStavkeCheckListe
                mod={modFormeCheckListe}
                stavkaZaIzmjenu={stavkaCheckListeZaIzmjenu}
                disabled={stanje.ucitavanje}
                onSubmit={handleSacuvajStavkuCheckListe}
                onCancel={handleOdustaniStavkaCheckListe}
              />
            </div>

            <div className="panel-checkliste">
              <PregledCheckListe stavkeCheckListe={stanje.stavkeCheckListe} />
            </div>

            <div className="panel-checkliste panel-checkliste-sirok">
              <ListaStavkiCheckListe
                stavkeCheckListe={stanje.stavkeCheckListe}
                onPromjenaStatusa={handlePromjenaStatusaStavkeCheckListe}
                onIzmjena={handleIzmjenaStavkeCheckListe}
                onBrisanje={handleBrisanjeStavkeCheckListe}
              />
            </div>
          </div>
        )}
      </section>

      <section className="sekcija-sadrzaja" id="sekcija-aktivnosti">
        <div className="naslov-sekcije">
          <p className="oznaka">Aktivnosti</p>
          <h2>{modFormeAktivnosti === 'izmjena' ? 'Izmjena aktivnosti' : 'Nova aktivnost'}</h2>
        </div>

        {!stanje.odabraniPlan && <p className="prazno-stanje">Prvo izaberite plan putovanja.</p>}

        {stanje.odabraniPlan && (
          <div className="mreza-aktivnosti">
            <div className="panel-aktivnosti">
              <FormaAktivnosti
                mod={modFormeAktivnosti}
                aktivnostZaIzmjenu={aktivnostZaIzmjenu}
                disabled={stanje.ucitavanje}
                onSubmit={handleSacuvajAktivnost}
                onCancel={handleOdustaniAktivnost}
              />
            </div>

            <div className="panel-aktivnosti">
              <KalendarAktivnosti
                aktivnosti={stanje.aktivnosti}
                planPutovanja={stanje.odabraniPlan}
                onIzmjena={handleIzmjenaAktivnosti}
              />
            </div>

            <div className="panel-aktivnosti panel-aktivnosti-sirok">
              <ListaAktivnosti
                aktivnosti={stanje.aktivnosti}
                onIzmjena={handleIzmjenaAktivnosti}
                onBrisanje={handleBrisanjeAktivnosti}
              />
            </div>
          </div>
        )}
      </section>
    </main>
  );
}
