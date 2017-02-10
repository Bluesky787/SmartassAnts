using System;
using System.Collections.Generic;
using AntMe.Deutsch;

// F�ge hier hinter AntMe.Spieler einen Punkt und deinen Namen ohne Leerzeichen
// ein! Zum Beispiel "AntMe.Spieler.WolfgangGallo".
namespace AntMe.SmartassAnts
{
	enum KasteTypen
    {
        Standard,
        Aggro,
        Foodloot,
        Spotter
    }
	// Das Spieler-Attribut erlaubt das Festlegen des Volk-Names und von Vor-
	// und Nachname des Spielers. Der Volk-Name mu� zugewiesen werden, sonst wird
	// das Volk nicht gefunden.
	[Spieler(
		Volkname = "SmartassAnts",
		Vorname = "Red",
		Nachname = "Ants"
	)]

	#region Kastendefinitionen
	// Das Typ-Attribut erlaubt das �ndern der Ameisen-Eigenschaften. Um den Typ
	// zu aktivieren mu� ein Name zugewiesen und dieser Name in der Methode 
	// BestimmeTyp zur�ckgegeben werden. Das Attribut kann kopiert und mit
	// verschiedenen Namen mehrfach verwendet werden.
	// Eine genauere Beschreibung gibts in Lektion 6 des Ameisen-Tutorials.
	[Kaste(
		Name = "Standard",
		GeschwindigkeitModifikator = 0,
		DrehgeschwindigkeitModifikator = 0,
		LastModifikator = 0,
		ReichweiteModifikator = 0,
		SichtweiteModifikator = 0,
		EnergieModifikator = 0,
		AngriffModifikator = 0
	)]
    [Kaste(
        Name = "Aggro",
        GeschwindigkeitModifikator = 1,
        DrehgeschwindigkeitModifikator = 0,
        LastModifikator = -1,
        ReichweiteModifikator = -1,
        SichtweiteModifikator = -1,
        EnergieModifikator = 0,
        AngriffModifikator = 2
    )]
    [Kaste(
        Name = "Foodloot",
        GeschwindigkeitModifikator = -1,
        DrehgeschwindigkeitModifikator = -1,
        LastModifikator = 2,
        ReichweiteModifikator = 2,
        SichtweiteModifikator = 0,
        EnergieModifikator = -1,
        AngriffModifikator = -1
    )]
    [Kaste(
        Name = "Spotter",
        GeschwindigkeitModifikator = 0,
        DrehgeschwindigkeitModifikator = -1,
        LastModifikator = -1,
        ReichweiteModifikator = 2,
        SichtweiteModifikator = 2,
        EnergieModifikator = -1,
        AngriffModifikator = -1
    )]

	#endregion

	public class MeineAmeise : Basisameise
	{
        #region Character

        //readonly int MarkierungGr��eSpotter = 100;

        /// <summary>
        /// F�r Ameisenstra�en.
        /// </summary>
        readonly int MarkierungGr��eAmeisenstra�e = 10;

        /// <summary>
        /// Bei Gefahr im Verzug.
        /// </summary>
        readonly int MarkierungGr��eHilfe = 200;

        /// <summary>
        /// Bei keiner direkten Gefahrenlage.
        /// </summary>
        readonly int MarkierungGr��eHilfeLokal = 20;

        /// <summary>
        /// Verr�t Position interessanter Objekte.
        /// </summary>
        readonly int MarkierungGr��eInformation = 50;

		//readonly int MarkierungGr��eSammler = 50;
		//readonly int MarkierungGr��eJ�ger = 50;

		Character character;

		Memory memory = new Memory();

		Nahrung ZielSammeln;
		Insekt ZielGegner;

		#endregion
		public MeineAmeise()
		{
			character = new Character(this);
		}
        
        #region Kaste

        /// <summary>
        /// Bestimmt die Kaste einer neuen Ameise.
        /// </summary>
        /// <param name="anzahl">Die Anzahl der von jeder Kaste bereits vorhandenen
        /// Ameisen.</param>
        /// <returns>Der Name der Kaste der Ameise.</returns>
        public override string BestimmeKaste(Dictionary<string, int> anzahl)
		{
            Random r = new Random();
            switch(r.Next(4))
            {
                case 0:
                    return KasteTypen.Standard.ToString();
                case 1:
                    return KasteTypen.Aggro.ToString();
                case 2:
                    return KasteTypen.Foodloot.ToString();
                case 3:
                    return KasteTypen.Spotter.ToString();
                default:
                    return KasteTypen.Standard.ToString();
            }
		}

		#endregion

		#region Fortbewegung

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn der die Ameise nicht weiss wo sie
		/// hingehen soll.
		/// </summary>
		public override void Wartet()
		{
			//if (FuzzyInferenceSystem.DecisionHaveABreak(character.faulheit, character.energie, character.laufen, memory.GetDecisionValue(Decisions.Laufen)))
			if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.laufen, memory.GetDecisionValue(DecisionType.Laufen)))
			{
				GeheGeradeaus();
				memory.ActionDone(DecisionType.Laufen);
                
				//Ziel?
                
			}
			else
            {
                //weiter stehen bleiben
                //this.BleibStehen();

                
			}
		}

		/// <summary>
		/// Wird einmal aufgerufen, wenn die Ameise ein Drittel ihrer maximalen
		/// Reichweite �berschritten hat.
		/// </summary>
		public override void WirdM�de()
		{
            GeheZuBau();
            
		}

        #endregion

        #region Nahrung

        bool tr�gtNahrung = false;
        bool greiftAn = false;
        bool hilftFreund = false;

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindestens einen
		/// Zuckerhaufen sieht.
		/// </summary>
		/// <param name="zucker">Der n�chstgelegene Zuckerhaufen.</param>
		public override void Sieht(Zucker zucker)
		{
            if (!tr�gtNahrung)
            {
                if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnzucker, memory.GetDecisionValue(DecisionType.SammelnZucker)))
                {
                    //Spr�heMarkierung((int)Information.ZielNahrung, MarkierungGr��eSammler);
                    GeheZuZiel(zucker);
                    memory.ActionDone(DecisionType.SammelnZucker);

                    //Spr�heMarkierung
                    Spr�heMarkierung(Markers.Add(new Marker(Marker.MarkerType.Zucker, CoordinateBase)), MarkierungGr��eInformation);
                }
            }
            else
            {
                //traegt Nahrung
                GeheZuBau();
            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
        /// Obstst�ck sieht.
        /// </summary>
        /// <param name="obst">Das n�chstgelegene Obstst�ck.</param>
        public override void Sieht(Obst obst)
		{
            if(!tr�gtNahrung)
            {
               if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnobst, memory.GetDecisionValue(DecisionType.SammelnObst)) && FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.ameisenFreundeInNaehe, character.sammelnobst, memory.GetDecisionValue(DecisionType.Gruppieren)))
                {
                    GeheZuZiel(obst);
                    memory.ActionDone(DecisionType.SammelnObst);
                    memory.ActionDone(DecisionType.Gruppieren);

                    //Spr�heMarkierung
                    Spr�heMarkierung(Markers.Add(new Marker(Marker.MarkerType.HilfeObst, CoordinateBase)), MarkierungGr��eHilfeLokal);
                }
                else {
                    //tr�gt Nahrung
                    GeheZuBau();
                    
                }
            }
        }

		/// <summary>
		/// Wird einmal aufgerufen, wenn di e Ameise einen Zuckerhaufen als Ziel
		/// hat und bei diesem ankommt.
		/// </summary>
		/// <param name="zucker">Der Zuckerhaufen.</param>
		public override void ZielErreicht(Zucker zucker)
        {
            if (!tr�gtNahrung)
            {
                //Zucker nehmen
                Nimm(zucker);
                tr�gtNahrung = true;

                Spr�heMarkierung(Markers.Add(new Marker(Marker.MarkerType.Zucker, CoordinateBase)), MarkierungGr��eInformation);
            }
            GeheZuBau();
        }

		/// <summary>
		/// Wird einmal aufgerufen, wenn die Ameise ein Obstst�ck als Ziel hat und
		/// bei diesem ankommt.
		/// </summary>
		/// <param name="obst">Das Obst�ck.</param>
		public override void ZielErreicht(Obst obst)
		{
            /*Nimm(obst);
            tr�gtNahrung = true;
            GeheZuBau();*/
            if (!tr�gtNahrung)
            {
                //Zucker nehmen
                Nimm(obst);
                tr�gtNahrung = true;
                Spr�heMarkierung(Markers.Add(new Marker(Marker.MarkerType.Obst, CoordinateBase)), MarkierungGr��eInformation);
            }
            GeheZuBau();
        }

		#endregion

		#region Kommunikation

		/// <summary>
		/// Wird einmal aufgerufen, wenn die Ameise eine Markierung des selben
		/// Volkes riecht. Einmal gerochene Markierungen werden nicht erneut
		/// gerochen.
		/// </summary>
		/// <param name="markierung">Die n�chste neue Markierung.</param>
		public override void RiechtFreund(Markierung markierung)
		{
            Marker marker = Markers.Get(markierung.Information);
            switch (marker.markerType)
            {
                case Marker.MarkerType.HilfeAmeise:
                    //Hilfsbereitschaft, Teamf�higkeit pr�fen
                    //Nahrung fallen lassen
                    //helfen
                    
                        if (FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.ameisenFreundeInNaehe, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenAmeise)))
                        {
                            if (marker.markerInformation == Marker.MarkerInformationType.Insekt)
                            {
                                LasseNahrungFallen();
                                GreifeAn(marker.Insekt);
                                memory.ActionDone(DecisionType.AngreifenAmeise);
                            
                            }
                        }
                    
                    break;

                case Marker.MarkerType.HilfeObst:
                    //Hilfsbereitschaft, Teamf�higkeit pr�fen
                    //nur wenn keine eigene Nahrung
                    //helfen
                    if (!tr�gtNahrung)
                    {
                        if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnobst, memory.GetDecisionValue(DecisionType.SammelnObst)) && FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.ameisenFreundeInNaehe, character.sammelnobst, memory.GetDecisionValue(DecisionType.Gruppieren)))
                        {
                            if (marker.markerInformation == Marker.MarkerInformationType.Object)
                            {
                                GeheZuZiel(marker.Objekt);

                                memory.ActionDone(DecisionType.SammelnObst);
                                memory.ActionDone(DecisionType.Gruppieren);
                                Spr�heMarkierung(Markers.Add(new Marker(Marker.MarkerType.Obst, marker.Objekt)), MarkierungGr��eHilfeLokal);
                            }
                        }
                    }
                            break;

                case Marker.MarkerType.HilfeWanze:
                    //Hilfsbereitschaft, Teamf�higkeit pr�fen
                    //Anzahl Freunde pr�fen
                    //Nahrung fallen lassen
                    //helfen
                    if (FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.ameisenFeindeInNaehe, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenWanze)))
                    {
                        if (marker.markerInformation == Marker.MarkerInformationType.Insekt)
                        {
                            LasseNahrungFallen();
                            GreifeAn(marker.Insekt);
                            memory.ActionDone(DecisionType.AngreifenWanze);
                            Spr�heMarkierung(Markers.Add(new Marker(Marker.MarkerType.HilfeWanze, marker.Insekt)), MarkierungGr��eHilfeLokal);
                        }
                    }

                    break;

                case Marker.MarkerType.Obst:
                    //das gleiche wie bei siehtObst()
                    if (!tr�gtNahrung)
                    {
                        if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnobst, memory.GetDecisionValue(DecisionType.SammelnObst)) && FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.ameisenFreundeInNaehe, character.sammelnobst, memory.GetDecisionValue(DecisionType.Gruppieren)))
                        {
                            if (marker.markerInformation == Marker.MarkerInformationType.Object)
                            {
                                GeheZuZiel(marker.Objekt);
                                memory.ActionDone(DecisionType.SammelnObst);
                                memory.ActionDone(DecisionType.Gruppieren);
                                
                                //Spr�heMarkierung
                                Spr�heMarkierung(Markers.Add(new Marker(Marker.MarkerType.HilfeObst, marker.Objekt)), MarkierungGr��eHilfeLokal);
                            }
                        }
                        else
                        {
                            //tr�gt Nahrung
                        }
                    }
                    break;

                case Marker.MarkerType.Zucker:
                    //das Gleiche wie bei siehtZucker()
                    if (!tr�gtNahrung)
                    {
                        if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnzucker, memory.GetDecisionValue(DecisionType.SammelnZucker)))
                        {
                            //Spr�heMarkierung((int)Information.ZielNahrung, MarkierungGr��eSammler);
                            if (marker.markerInformation == Marker.MarkerInformationType.Object)
                                GeheZuZiel(marker.Objekt);

                            if (marker.markerInformation == Marker.MarkerInformationType.Richtung)
                            {
                                DreheInRichtung(marker.richtung);
                                GeheGeradeaus();


                                memory.ActionDone(DecisionType.SammelnZucker);

                                //Spr�heMarkierung
                                Spr�heMarkierung(Markers.Add(new Marker(Marker.MarkerType.Zucker, marker.Objekt)), MarkierungGr��eInformation);
                            }
                        }
                    }
                    else
                    {
                        //traegt Nahrung
                    }
                    break;

                default:
                    //damit Ameisen nicht stehen bleiben
                    if (Ziel != null)
                    {
                        GeheZuZiel(Ziel);
                    }
                    break;
            }
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindstens eine Ameise des
		/// selben Volkes sieht.
		/// </summary>
		/// <param name="ameise">Die n�chstgelegene befreundete Ameise.</param>
		public override void SiehtFreund(Ameise ameise)
		{
            if (!(this.EntfernungZuBau == 0))
            {
                if (this.Ziel != null)
                {
                    //nicht gruppieren
                }
                else
                {
                    //grupperen entscheiden
                    if (FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.energie, character.gruppieren, memory.GetDecisionValue(DecisionType.Gruppieren)))
                    {
                        this.Spr�heMarkierung(0, 10);
                        this.GeheZuZiel(ameise);
                        memory.ActionDone(DecisionType.Gruppieren);
                    }
                    else
                    {
                        //nicht gruppieren
                    }
                }
            }
		}

		/// <summary>
		/// Wird aufgerufen, wenn die Ameise eine befreundete Ameise eines anderen Teams trifft.
		/// </summary>
		/// <param name="ameise"></param>
		public override void SiehtVerb�ndeten(Ameise ameise)
		{
		}

		#endregion

		#region Kampf

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Wanze
		/// sieht.
		/// </summary>
		/// <param name="wanze">Die n�chstgelegene Wanze.</param>
		public override void SiehtFeind(Wanze wanze)
		{
            if (!tr�gtNahrung)
            {
                if (FuzzyInferenceSystem.Superdecision5x5x2(character.wut, character.energie, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenWanze)) && FuzzyInferenceSystem.Superdecision5x5x2(character.wut, character.ameisenFreundeInNaehe, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenWanze)))
                {
                    //hilfe rufen
                    this.Spr�heMarkierung(0, 100);

                    //angreifen
                    this.GreifeAn(wanze);
                    memory.ActionDone(DecisionType.AngreifenWanze);
                }
                else
                {
                    //wegrennen
                    this.GeheZuBau();
                    memory.ActionDone(DecisionType.Wegrennen);
                }
            }
        }

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise eines
		/// anderen Volkes sieht.
		/// </summary>
		/// <param name="ameise">Die n�chstgelegen feindliche Ameise.</param>
		public override void SiehtFeind(Ameise ameise)
		{
            if (!tr�gtNahrung)
            {
                if (FuzzyInferenceSystem.Superdecision5x5x2(character.wut, character.energie, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenAmeise)))
                {
                    //Hilfe rufen
                    Spr�heMarkierung(Markers.Add(new Marker(Marker.MarkerType.HilfeAmeise, this.CoordinateBase)), MarkierungGr��eHilfeLokal);

                    //Angreifen
                    GreifeAn(ameise);
                    memory.ActionDone(DecisionType.AngreifenAmeise);
                }
            }
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise von einer Wanze angegriffen
		/// wird.
		/// </summary>
		/// <param name="wanze">Die angreifende Wanze.</param>
		public override void WirdAngegriffen(Wanze wanze)
		{
			/*
            Spr�heMarkierung((int)Information.Hilfe, MarkierungGr��eHilfe);
            GreifeAn(wanze);
            greiftAn = true;
			*/
		}

		/// <summary>
		/// Wird wiederholt aufgerufen in der die Ameise von einer Ameise eines
		/// anderen Volkes Ameise angegriffen wird.
		/// </summary>
		/// <param name="ameise">Die angreifende feindliche Ameise.</param>
		public override void WirdAngegriffen(Ameise ameise)
		{
			/*
            Spr�heMarkierung((int)Information.Hilfe, MarkierungGr��eHilfe);
            GreifeAn(ameise);
            greiftAn = true;
			*/
        }

		#endregion

		#region Sonstiges

		/// <summary>
		/// Wird einmal aufgerufen, wenn die Ameise gestorben ist.
		/// </summary>
		/// <param name="todesart">Die Todesart der Ameise</param>
		public override void IstGestorben(Todesart todesart)
		{

		}

		/// <summary>
		/// Wird unabh�ngig von �u�eren Umst�nden in jeder Runde aufgerufen.
		/// </summary>
		public override void Tick()
		{
            //Richtung zum Bau beibehalten
            if (tr�gtNahrung)
            {
                GeheZuBau();

                //Ameisenstra�e
                //nur, wenn sie Zucker tr�gt
                if (GetragenesObst == null)
                {
                    Spr�heMarkierung(Markers.Add(new SmartassAnts.Marker(Marker.MarkerType.Zucker, (Richtung + 180) % 360)), MarkierungGr��eAmeisenstra�e);
                }
                else
                {
                    //Apfelhelfer
                    if (BrauchtNochTr�ger(GetragenesObst))
                    {
                        Spr�heMarkierung(Markers.Add(new SmartassAnts.Marker(Marker.MarkerType.HilfeObst, CoordinateBase)), MarkierungGr��eHilfeLokal);
                        
                    }
                }
            }
		}

		#endregion
		 
	}
}