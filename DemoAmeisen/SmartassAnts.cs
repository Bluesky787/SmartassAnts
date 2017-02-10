using System;
using System.Collections.Generic;
using AntMe.Deutsch;

// Füge hier hinter AntMe.Spieler einen Punkt und deinen Namen ohne Leerzeichen
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
	// und Nachname des Spielers. Der Volk-Name muß zugewiesen werden, sonst wird
	// das Volk nicht gefunden.
	[Spieler(
		Volkname = "SmartassAnts",
		Vorname = "Red",
		Nachname = "Ants"
	)]

	#region Kastendefinitionen
	// Das Typ-Attribut erlaubt das Ändern der Ameisen-Eigenschaften. Um den Typ
	// zu aktivieren muß ein Name zugewiesen und dieser Name in der Methode 
	// BestimmeTyp zurückgegeben werden. Das Attribut kann kopiert und mit
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

        //readonly int MarkierungGrößeSpotter = 100;

        /// <summary>
        /// FÜr Ameisenstraßen.
        /// </summary>
        readonly int MarkierungGrößeAmeisenstraße = 10;

        /// <summary>
        /// Bei Gefahr im Verzug.
        /// </summary>
        readonly int MarkierungGrößeHilfe = 200;

        /// <summary>
        /// Bei keiner direkten Gefahrenlage.
        /// </summary>
        readonly int MarkierungGrößeHilfeLokal = 20;

        /// <summary>
        /// Verrät Position interessanter Objekte.
        /// </summary>
        readonly int MarkierungGrößeInformation = 50;

		//readonly int MarkierungGrößeSammler = 50;
		//readonly int MarkierungGrößeJäger = 50;

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
            GeheGeradeaus();
            /*
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
			}*/
		}

		/// <summary>
		/// Wird einmal aufgerufen, wenn die Ameise ein Drittel ihrer maximalen
		/// Reichweite überschritten hat.
		/// </summary>
		public override void WirdMüde()
		{
            GeheZuBau();
		}

        #endregion

        #region Nahrung

        bool trägtNahrung = false;
        bool greiftAn = false;
        bool hilftFreund = false;

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindestens einen
		/// Zuckerhaufen sieht.
		/// </summary>
		/// <param name="zucker">Der nächstgelegene Zuckerhaufen.</param>
		public override void Sieht(Zucker zucker)
		{
            if (!trägtNahrung)
            {
                if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnzucker, memory.GetDecisionValue(DecisionType.SammelnZucker)))
                {
                    //SprüheMarkierung((int)Information.ZielNahrung, MarkierungGrößeSammler);
                    GeheZuZiel(zucker);
                    memory.ActionDone(DecisionType.SammelnZucker);

                    //SprüheMarkierung
                    SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.Zucker, CoordinateBase)), MarkierungGrößeInformation);
                }
            }
            else
            {
                //traegt Nahrung
            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
        /// Obststück sieht.
        /// </summary>
        /// <param name="obst">Das nächstgelegene Obststück.</param>
        public override void Sieht(Obst obst)
		{
            if(!trägtNahrung)
            {
               if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnobst, memory.GetDecisionValue(DecisionType.SammelnObst)) && FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.ameisenFreundeInNaehe, character.sammelnobst, memory.GetDecisionValue(DecisionType.Gruppieren)))
                {
                    GeheZuZiel(obst);
                    memory.ActionDone(DecisionType.SammelnObst);
                    memory.ActionDone(DecisionType.Gruppieren);

                    //SprüheMarkierung
                    SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.HilfeObst, CoordinateBase)), MarkierungGrößeHilfeLokal);
                }
                else {
                    //trägt Nahrung
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
            if (!trägtNahrung)
            {
                //Zucker nehmen
                Nimm(zucker);
                trägtNahrung = true;

                SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.Zucker, CoordinateBase)), MarkierungGrößeInformation);
            }
            GeheZuBau();
        }

		/// <summary>
		/// Wird einmal aufgerufen, wenn die Ameise ein Obststück als Ziel hat und
		/// bei diesem ankommt.
		/// </summary>
		/// <param name="obst">Das Obstück.</param>
		public override void ZielErreicht(Obst obst)
		{
            /*Nimm(obst);
            trägtNahrung = true;
            GeheZuBau();*/
            if (!trägtNahrung)
            {
                //Zucker nehmen
                Nimm(obst);
                trägtNahrung = true;
                SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.Obst, CoordinateBase)), MarkierungGrößeInformation);
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
		/// <param name="markierung">Die nächste neue Markierung.</param>
		public override void RiechtFreund(Markierung markierung)
		{
            Marker marker = Markers.Get(markierung.Information);
            try
            {
                switch (marker.markerType)
                {
                    case Marker.MarkerType.HilfeAmeise:
                        //Hilfsbereitschaft, Teamfähigkeit prüfen
                        //Nahrung fallen lassen
                        //helfen

                        //wenn sie nicht schon beim angreifen ist
                        if (!greiftAn)
                        {
                            //dann über Angriff nachdenken
                            if (FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.ameisenFreundeInNaehe, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenAmeise)))
                            {
                                if (marker.markerInformation == Marker.MarkerInformationType.Insekt)
                                {
                                    LasseNahrungFallen();
                                    trägtNahrung = false;
                                    GreifeAn(marker.Insekt);
                                    memory.ActionDone(DecisionType.AngreifenAmeise);
                                }
                            }
                        }
                        break;

                    case Marker.MarkerType.HilfeObst:
                        //Hilfsbereitschaft, Teamfähigkeit prüfen
                        //nur wenn keine eigene Nahrung
                        //helfen
                        if (!trägtNahrung)
                        {
                            if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnobst, memory.GetDecisionValue(DecisionType.SammelnObst)) && FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.ameisenFreundeInNaehe, character.sammelnobst, memory.GetDecisionValue(DecisionType.Gruppieren)))
                            {
                                if (marker.markerInformation == Marker.MarkerInformationType.Object)
                                {
                                    GeheZuZiel(marker.Objekt);

                                    memory.ActionDone(DecisionType.SammelnObst);
                                    memory.ActionDone(DecisionType.Gruppieren);
                                    //SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.Obst, marker.Objekt)), MarkierungGrößeHilfeLokal);
                                }
                            }
                        }
                        break;

                    case Marker.MarkerType.HilfeWanze:
                        //Hilfsbereitschaft, Teamfähigkeit prüfen
                        //Anzahl Freunde prüfen
                        //Nahrung fallen lassen
                        //helfen

                        //wenn sie nicht schon beim Angreifen ist
                        if (!this.greiftAn)
                        {
                            //dann über Angriff nachdenken
                            if (FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.ameisenFeindeInNaehe, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenWanze)))
                            {
                                if (marker.markerInformation == Marker.MarkerInformationType.Insekt)
                                {
                                    LasseNahrungFallen();
                                    trägtNahrung = false;
                                    GreifeAn(marker.Insekt);
                                    memory.ActionDone(DecisionType.AngreifenWanze);
                                    //SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.HilfeWanze, marker.Insekt)), MarkierungGrößeHilfeLokal);
                                }
                            }
                        }

                        break;

                    case Marker.MarkerType.Obst:
                        //das gleiche wie bei siehtObst()
                        if (!trägtNahrung)
                        {
                            if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnobst, memory.GetDecisionValue(DecisionType.SammelnObst)) && FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.ameisenFreundeInNaehe, character.sammelnobst, memory.GetDecisionValue(DecisionType.Gruppieren)))
                            {
                                if (marker.markerInformation == Marker.MarkerInformationType.Object)
                                {
                                    GeheZuZiel(marker.Objekt);
                                    memory.ActionDone(DecisionType.SammelnObst);
                                    memory.ActionDone(DecisionType.Gruppieren);

                                    //SprüheMarkierung
                                    //SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.HilfeObst, marker.Objekt)), MarkierungGrößeHilfeLokal);
                                }
                            }
                            else
                            {
                                //trägt Nahrung
                            }
                        }
                        break;

                    case Marker.MarkerType.Zucker:
                        //das Gleiche wie bei siehtZucker()
                        if (!trägtNahrung)
                        {
                            if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnzucker, memory.GetDecisionValue(DecisionType.SammelnZucker)))
                            {
                                //SprüheMarkierung((int)Information.ZielNahrung, MarkierungGrößeSammler);
                                if (marker.markerInformation == Marker.MarkerInformationType.Object)
                                    GeheZuZiel(marker.Objekt);

                                if (marker.markerInformation == Marker.MarkerInformationType.Richtung)
                                {
                                    DreheInRichtung(marker.richtung);
                                    GeheGeradeaus();


                                    memory.ActionDone(DecisionType.SammelnZucker);

                                    //SprüheMarkierung
                                    //SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.Zucker, marker.Objekt)), MarkierungGrößeInformation);
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
            catch (NullReferenceException)
            {
                //wahrscheinlich ungültige Markierung
            }
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindstens eine Ameise des
		/// selben Volkes sieht.
		/// </summary>
		/// <param name="ameise">Die nächstgelegene befreundete Ameise.</param>
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
		public override void SiehtVerbündeten(Ameise ameise)
		{
		}

		#endregion

		#region Kampf

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Wanze
		/// sieht.
		/// </summary>
		/// <param name="wanze">Die nächstgelegene Wanze.</param>
		public override void SiehtFeind(Wanze wanze)
		{
            if (!trägtNahrung)
            {
                if (FuzzyInferenceSystem.Superdecision5x5x2(character.wut, character.energie, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenWanze)))
                {
                    if (FuzzyInferenceSystem.Superdecision5x5x2(character.wut, character.ameisenFreundeInNaehe, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenWanze)))
                    {
                        //hilfe rufen
                        //this.SprüheMarkierung(0, 100);

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
            else
            {
                //wegrennen
                //Entscheidung flüchten? -> Nahrung sofort fallen lassen

                if (EntfernungZuBau == 0)
                {
                    LasseNahrungFallen();
                    trägtNahrung = false;
                }

                //this.GeheZuBau();

                //speichern, dass Ameise bereits weggerannt ist?
                //memory.ActionDone(DecisionType.Wegrennen);
            }
        }

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise eines
		/// anderen Volkes sieht.
		/// </summary>
		/// <param name="ameise">Die nächstgelegen feindliche Ameise.</param>
		public override void SiehtFeind(Ameise ameise)
		{
            if (!trägtNahrung)
            {
                if (FuzzyInferenceSystem.Superdecision5x5x2(character.wut, character.energie, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenAmeise)))
                {
                    //Hilfe rufen
                    SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.HilfeAmeise, this.CoordinateBase)), MarkierungGrößeHilfeLokal);

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
            SprüheMarkierung((int)Information.Hilfe, MarkierungGrößeHilfe);
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
            SprüheMarkierung((int)Information.Hilfe, MarkierungGrößeHilfe);
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
		/// Wird unabhängig von äußeren Umständen in jeder Runde aufgerufen.
		/// </summary>
		public override void Tick()
		{
            //Richtung zum Bau beibehalten
            if (trägtNahrung)
            {
                GeheZuBau();

                //Ameisenstraße
                //nur, wenn sie Zucker trägt
                if (GetragenesObst == null)
                {
                    if (EntfernungZuBau > 10)
                    {
                        SprüheMarkierung(Markers.Add(new SmartassAnts.Marker(Marker.MarkerType.Zucker, (Richtung + 180) % 360)), MarkierungGrößeAmeisenstraße);
                    }
                }
                else
                {
                    //Apfelhelfer
                    if (BrauchtNochTräger(GetragenesObst))
                    {
                        SprüheMarkierung(Markers.Add(new SmartassAnts.Marker(Marker.MarkerType.HilfeObst, CoordinateBase)), MarkierungGrößeHilfeLokal);
                        
                    }
                }
            }
		}

		#endregion
		 
	}
}