/*ToDo:
 * Vereerbung
 * Entscheidung, Freunden zu helfen (und Nahrung ggf. fallen lassen zu müssen), hinzufügen -> auch abhängig von ANzahl der Ameisen in der Nähe (je mehr in der Nähe, desto unwahrscheinlicher)
 * Spawn-Kaste abhängig von allgemeiner Stimmung (Mehr Wut -> mehr Aggro-Ameisen usw.)
 */

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
		GeschwindigkeitModifikator = 2,
		DrehgeschwindigkeitModifikator = -1,
		LastModifikator = -1,
		ReichweiteModifikator = 0,
		SichtweiteModifikator = 0,
		EnergieModifikator = 0,
		AngriffModifikator = 0
	)]
    [Kaste(
        Name = "Aggro",
        GeschwindigkeitModifikator = 1,
        DrehgeschwindigkeitModifikator = -1,
        LastModifikator = -1,
        ReichweiteModifikator = -1,
        SichtweiteModifikator = -1,
        EnergieModifikator = 1,
        AngriffModifikator = 2
    )]
    [Kaste(
        Name = "Foodloot",
        GeschwindigkeitModifikator = 2,
        DrehgeschwindigkeitModifikator = -1,
        LastModifikator = -1,
        ReichweiteModifikator = 1,
        SichtweiteModifikator = 1,
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

	public class SmartassAnt : Basisameise
    { 

        #region Character

        //readonly int MarkierungGrößeSpotter = 100;

        /// <summary>
        /// FÜr Ameisenstraßen.
        /// </summary>
        readonly int MarkierungGrößeAmeisenstraße = 3;

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

		internal Character character;

        internal Memory memory;

        #endregion

        int waitForFrame = 0, currentFrame = 0, breakActionAtFrame = 0;
        int breakActionAfterFrames = 500, awaitingFrames = 50;
        static List<SmartassAnt> DiedAnts = new List<SmartassAnt>();

        public SmartassAnt()
		{
            memory = new Memory(this);

            //Prüfen, ob Ameisen gestorben sind
            //-> wenn ja, dann sollen neue Ameisen Character erben
            //-> lernen von Todesart der letzten Ameise, Charakter der erfolgreichsten Ameise und einer zufälligen Ameise
            //Character inheritedCharacter = ...

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
            if(currentFrame <= waitForFrame)
            {
                return;
            }
            //Hat die Ameise etwas gemacht?
          

            if (EntfernungZuBau == 0)
            {
                if (greiftAn)
                {
                    greiftAn = false;
                    memory.ActionSuccessful();
                }

                if (trägtNahrung)
                {
                    trägtNahrung = false;
                    memory.ActionSuccessful();
                }

                if (hilftFreund)
                {
                    hilftFreund = false;
                    memory.ActionSuccessful();
                }
                //Entscheiden, ob letzter Zucker gesucht werden soll
                if (Memory.gemerkterZucker != null)
                {
                    if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnzucker, memory.GetDecisionValue(DecisionType.SammelnZucker)))
                    {
                        //Zucker sammeln
                        GeheZuZiel(Memory.gemerkterZucker);
                        memory.ActionDone(DecisionType.Laufen);
                        memory.ActionDone(DecisionType.SammelnZucker);
                        setActionBreak();
                    }
                    else
                    {
                        //Entscheiden, ob die Ameise laufen soll
                        Weitermachen();
                    }
                }
            }
            else
            {
                Weitermachen();
            }
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
            //Alle Ameisen sollen Zucker kennen
            if (Memory.gemerkterZucker == null)
            {
                Memory.gemerkterZucker = zucker;
            }
            else
            {
                //aktueller Zucker näher ran? -> in ZielErreicht(zucker) prüfen
                //aktueller Zucker voller?
                //-> neuen Zucker merken
                if (zucker.Menge > Memory.gemerkterZucker.Menge)
                {
                    Memory.gemerkterZucker = zucker;
                }
            }

            if (!trägtNahrung)
            {
               if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnzucker, memory.GetDecisionValue(DecisionType.SammelnZucker)))
               {
                    //SprüheMarkierung
                    SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.Zucker, zucker)), MarkierungGrößeInformation);

                    //SprüheMarkierung((int)Information.ZielNahrung, MarkierungGrößeSammler);
                    GeheZuZiel(zucker);
                    memory.ActionDone(DecisionType.SammelnZucker);
                    setActionBreak();
                }
                else
                    Weitermachen();
            }
            else
            {
                //traegt Nahrung
                //Weitermachen();
                GeheZuBau();
            }
        }

        /// <summary>
        /// Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
        /// Obststück sieht.
        /// </summary>
        /// <param name="obst">Das nächstgelegene Obststück.</param>
        public override void Sieht(Obst obst)
		{
            if (!trägtNahrung)
            {
                if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnobst, memory.GetDecisionValue(DecisionType.SammelnObst)) && FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.ameisenFreundeInNaehe, character.sammelnobst, memory.GetDecisionValue(DecisionType.Gruppieren)))
                {
                    GeheZuZiel(obst);
                    memory.ActionDone(DecisionType.SammelnObst);
                    memory.ActionDone(DecisionType.Gruppieren);
                    setActionBreak();

                    //SprüheMarkierung
                    SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.HilfeObst, obst)), MarkierungGrößeHilfeLokal);
                }
                else
                {
                    //kein Bock
                    Weitermachen();
                }
            }
            else
            {
                //trägt Nahrung
                //Weitermachen();
                GeheZuBau();
            }

        }

		/// <summary>
		/// Wird einmal aufgerufen, wenn di e Ameise einen Zuckerhaufen als Ziel
		/// hat und bei diesem ankommt.
		/// </summary>
		/// <param name="zucker">Der Zuckerhaufen.</param>
		public override void ZielErreicht(Zucker zucker)
        {
            //Zucker näher ran als gemerkter Zucker der Kollonie?
            if (EntfernungZuBau < Memory.gemerkterZucker_EntfernungZuBau)
            {
                Memory.gemerkterZucker = zucker;
            }

           // if (!trägtNahrung)
           // {
                //Zucker nehmen
                Nimm(zucker);
                trägtNahrung = true;

                SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.Zucker, zucker)), MarkierungGrößeInformation);
           // }
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
                SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.Obst, obst)), MarkierungGrößeInformation);
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
                                    greiftAn = true;
                                    hilftFreund = true;
                                    memory.ActionDone(DecisionType.AngreifenAmeise);
                                    setActionBreak();
                                }
                                else
                                    Weitermachen();
                            }
                            else
                                Weitermachen();
                        }
                        else
                            Weitermachen();
                        break;

                    case Marker.MarkerType.HilfeObst:
                        //Hilfsbereitschaft, Teamfähigkeit prüfen
                        //nur wenn keine eigene Nahrung
                        //helfen
                        if (!trägtNahrung && !greiftAn && !hilftFreund)
                        {
                            if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnobst, memory.GetDecisionValue(DecisionType.SammelnObst)) && FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.ameisenFreundeInNaehe, character.sammelnobst, memory.GetDecisionValue(DecisionType.Gruppieren)))
                            {
                                if (marker.markerInformation == Marker.MarkerInformationType.Object)
                                {
                                    GeheZuZiel(marker.Objekt);
                                    hilftFreund = true;
                                    memory.ActionDone(DecisionType.SammelnObst);
                                    memory.ActionDone(DecisionType.Gruppieren);
                                    setActionBreak();
                                    //SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.Obst, marker.Objekt)), MarkierungGrößeHilfeLokal);
                                }
                                else
                                    Weitermachen();
                            }
                            else
                                Weitermachen();
                        }
                        else
                            Weitermachen();
                        break;

                    case Marker.MarkerType.HilfeWanze:
                        //Hilfsbereitschaft, Teamfähigkeit prüfen
                        //Anzahl Freunde prüfen
                        //Nahrung fallen lassen
                        //helfen

                        //wenn sie nicht schon beim Angreifen ist
                        if (!greiftAn)
                        {
                            //dann über Angriff nachdenken
                            if (FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.ameisenFeindeInNaehe, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenWanze)))
                            {
                                if (marker.markerInformation == Marker.MarkerInformationType.Insekt)
                                {
                                    LasseNahrungFallen();
                                    trägtNahrung = false;
                                    GreifeAn(marker.Insekt);
                                    greiftAn = true;
                                    hilftFreund = true;
                                    memory.ActionDone(DecisionType.AngreifenWanze);
                                    setActionBreak();
                                    //SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.HilfeWanze, marker.Insekt)), MarkierungGrößeHilfeLokal);
                                }
                            }
                            else
                                Weitermachen();
                        }
                        else
                            Weitermachen();
                        break;

                    case Marker.MarkerType.Obst:
                        //das gleiche wie bei siehtObst()
                        if (!trägtNahrung && !greiftAn && !hilftFreund)
                        {
                            if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnobst, memory.GetDecisionValue(DecisionType.SammelnObst)) && FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.ameisenFreundeInNaehe, character.sammelnobst, memory.GetDecisionValue(DecisionType.Gruppieren)))
                            {
                                if (marker.markerInformation == Marker.MarkerInformationType.Object)
                                {
                                    GeheZuZiel(marker.Objekt);
                                    hilftFreund = true;
                                    memory.ActionDone(DecisionType.SammelnObst);
                                    memory.ActionDone(DecisionType.Gruppieren);
                                    setActionBreak();

                                    //SprüheMarkierung
                                    //SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.HilfeObst, marker.Objekt)), MarkierungGrößeHilfeLokal);
                                }
                                else
                                    Weitermachen();
                            }
                            else
                            {
                                //kein Bock, Obst aufzunehmen
                                Weitermachen();
                           }
                        }
                        else
                            Weitermachen();
                        break;

                    case Marker.MarkerType.Zucker:
                        //das Gleiche wie bei siehtZucker()
                        if (!trägtNahrung && !greiftAn && !hilftFreund)
                        {
                            if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.sammelnzucker, memory.GetDecisionValue(DecisionType.SammelnZucker)))
                            {
                                //SprüheMarkierung((int)Information.ZielNahrung, MarkierungGrößeSammler);
                                if (marker.markerInformation == Marker.MarkerInformationType.Object)
                                {
                                    GeheZuZiel(marker.Objekt);
                                    memory.ActionDone(DecisionType.SammelnZucker);
                                    setActionBreak();
                                }
                                else
                                {
                                    if (marker.markerInformation == Marker.MarkerInformationType.Richtung)
                                    {
                                        if (Ziel.GetType() != typeof(Zucker))
                                        {
                                            DreheInRichtung(marker.richtung);
                                            GeheGeradeaus();
                                            memory.ActionDone(DecisionType.SammelnZucker);
                                            setActionBreak();
                                        } 
                                        else
                                        {
                                            Weitermachen();
                                        }
                                        //SprüheMarkierung
                                        //SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.Zucker, marker.Objekt)), MarkierungGrößeInformation);
                                    }
                                    else
                                        Weitermachen();
                                }
                            }
                            else
                                Weitermachen();
                        }
                        else
                        {
                            //traegt Nahrung
                            Weitermachen();
                        }
                        break;

                    default:
                        Weitermachen();
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
            if (this.EntfernungZuBau > 5)
            {
                if (Ziel == null && !(trägtNahrung || hilftFreund || greiftAn))
                {
                    //grupperen entscheiden
                    if (FuzzyInferenceSystem.Superdecision5x5x2(character.teamfaehigkeit, character.energie, character.gruppieren, memory.GetDecisionValue(DecisionType.Gruppieren)))
                    {
                        this.GeheZuZiel(ameise);
                        memory.ActionDone(DecisionType.Gruppieren);
                        setActionBreak();
                    }
                    else
                        Weitermachen();
                }
                else
                    Weitermachen();
            }
            else
                Weitermachen();
		}

		/// <summary>
		/// Wird aufgerufen, wenn die Ameise eine befreundete Ameise eines anderen Teams trifft.
		/// </summary>
		/// <param name="ameise"></param>
		public override void SiehtVerbündeten(Ameise ameise)
		{
            Weitermachen();
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
            if (!trägtNahrung && !greiftAn)
            {
                if (FuzzyInferenceSystem.Superdecision5x5x2(character.wut, character.energie, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenWanze)) && FuzzyInferenceSystem.Superdecision5x5x2(character.wut, character.ameisenFreundeInNaehe, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenWanze)))
                {
                    //hilfe rufen
                    SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.HilfeWanze, wanze)), MarkierungGrößeHilfe);

                    //angreifen
                    GreifeAn(wanze);
                    greiftAn = true;
                    memory.ActionDone(DecisionType.AngreifenWanze);
                    setActionBreak();
                }
                else
                {
                    //Entscheidung wegrennen
                    GeheWegVon(wanze);
                    memory.ActionDone(DecisionType.Wegrennen);
                    setActionBreak();
                }
            }
            else
            {
                Weitermachen();
            }
        }

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise eines
		/// anderen Volkes sieht.
		/// </summary>
		/// <param name="ameise">Die nächstgelegen feindliche Ameise.</param>
		public override void SiehtFeind(Ameise ameise)
		{
            if (/*!trägtNahrung //rauskommentiert, damit feindliche Ameisen den Apfel nicht wegtragen können &&*/ !greiftAn)
            {
                if (FuzzyInferenceSystem.Superdecision5x5x2(character.wut, character.energie, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenAmeise)))
                {
                    //Hilfe rufen
                    SprüheMarkierung(Markers.Add(new Marker(Marker.MarkerType.HilfeAmeise, ameise)), MarkierungGrößeHilfeLokal);

                    //evtl. Nahrung fallen lassen
                    if (trägtNahrung)
                    {
                        LasseNahrungFallen();
                        trägtNahrung = false;
                    }

                    //Angreifen
                    GreifeAn(ameise);
                    greiftAn = true;
                    memory.ActionDone(DecisionType.AngreifenAmeise);
                    setActionBreak();
                }
                else
                    Weitermachen();
            }
            else
                Weitermachen();
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise von einer Wanze angegriffen
		/// wird.
		/// </summary>
		/// <param name="wanze">Die angreifende Wanze.</param>
		public override void WirdAngegriffen(Wanze wanze)
		{           
            //Entscheidung Angreifen
            //Wenn negativ, Entscheidung wegrennen
            if (FuzzyInferenceSystem.Superdecision5x5x2(character.wut, character.ameisenFreundeInNaehe, character.angreifen, memory.GetDecisionValue(1-DecisionType.Wegrennen))) //beeinflusst Entscheidung zum Angriff negativ
            {
                GreifeAn(wanze);
                greiftAn = true;
               memory.ActionDone(DecisionType.AngreifenWanze);
                setActionBreak();                
            }
            else
            {
                LasseNahrungFallen();
                trägtNahrung = false;
                memory.ActionDone(DecisionType.Wegrennen);
                setActionBreak();
                GeheWegVon(wanze);
            }
         
		}

		/// <summary>
		/// Wird wiederholt aufgerufen in der die Ameise von einer Ameise eines
		/// anderen Volkes Ameise angegriffen wird.
		/// </summary>
		/// <param name="ameise">Die angreifende feindliche Ameise.</param>
		public override void WirdAngegriffen(Ameise ameise)
		{
            

            //Entscheidung Angreifen
            //Wenn negativ, Entscheidung wegrennen
            if (FuzzyInferenceSystem.Superdecision5x5x2(character.wut, character.energie, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenAmeise)))
            {
                GreifeAn(ameise);
                greiftAn = true;
                memory.ActionDone(DecisionType.AngreifenAmeise);
                setActionBreak();
                
            }else
            {
                GeheWegVon(ameise);
                memory.ActionDone(DecisionType.Wegrennen);
                setActionBreak();
            }
            
        }

		#endregion

		#region Sonstiges

		/// <summary>
		/// Wird einmal aufgerufen, wenn die Ameise gestorben ist.
		/// </summary>
		/// <param name="todesart">Die Todesart der Ameise</param>
		public override void IstGestorben(Todesart todesart)
		{
            memory.ActionUnsuccessful();
		}

		/// <summary>
		/// Wird unabhängig von äußeren Umständen in jeder Runde aufgerufen.
		/// </summary>
		public override void Tick()
		{
            currentFrame++;

            //GeheGeradeaus() unterbrechen
            if (currentFrame % 50 == 0 && Ziel == null)
            {
                //BleibStehen(); //Nicht benötigt, ist in Weitermachen() enthalten
                //Weitermachen(); //nervt gerade, weil Ameisen ihr Ziel verlieren
            }

            //Erfolglose Aktionen abbrechen
            if (breakActionAtFrame != 0 && breakActionAtFrame < currentFrame)
            {
                breakActionAtFrame = 0;
                //LasseNahrungFallen(); //bringt nichts
                if (!trägtNahrung)
                {
                    BleibStehen();
                    DreheUmWinkel(90);
                    GeheGeradeaus();
                }
                memory.ActionUnsuccessful();
                return;
            }

            if (trägtNahrung)
            {
                //Richtung zum Bau beibehalten
                GeheZuBau();

                //Ameisenstraße
                //nur, wenn sie Zucker trägt
                if (GetragenesObst == null)
                {
                    if (EntfernungZuBau > 10)
                    {
                        if (currentFrame % 20 == 0) //nur alle x Frames ausführen
                            SprüheMarkierung(Markers.Add(new SmartassAnts.Marker(Marker.MarkerType.Zucker, (Richtung + 180) % 360)), MarkierungGrößeAmeisenstraße);
                    }
                }
                else
                {
                    //Apfelhelfer
                    if (BrauchtNochTräger(GetragenesObst))
                    {
                        SprüheMarkierung(Markers.Add(new SmartassAnts.Marker(Marker.MarkerType.HilfeObst, GetragenesObst)), MarkierungGrößeHilfeLokal);                        
                    }
                }
            }
            else
                Weitermachen();
		}

        /// <summary>
        /// Verfolgt das aktuelle Ziel weiter oder geht geradeaus, wenn kein Ziel vorhanden.
        /// </summary>
        private void Weitermachen()
        {
            if (Ziel != null)
            {
                if (Ziel.GetType() == typeof(Insekt)) {
                    GreifeAn((Insekt)Ziel);
                    greiftAn = true;
                }
                else {
                    GeheZuZiel(Ziel);
                }
            }
            else
            {
                if (FuzzyInferenceSystem.Superdecision5x5x2(character.faulheit, character.energie, character.laufen, memory.GetDecisionValue(DecisionType.Laufen)))
                {
                    GeheGeradeaus();
                    setActionBreak();
                    memory.ActionDone(DecisionType.Laufen);
                }
                else
                {
                    //kein Bock
                    BleibStehen();
                    WaitUntil(50);
                    memory.ActionDone(DecisionType.Warten);
                }
            }
        }

        public void WaitUntil(int numFrames)
        {
            waitForFrame = currentFrame + numFrames;
            
        }

        public void setActionBreak()
        {
            breakActionAtFrame = currentFrame + breakActionAfterFrames;
        }

		#endregion
		 
	}
}