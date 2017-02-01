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
		readonly int MarkierungGrößeSpotter = 100;
		readonly int MarkierungGrößeHilfe = 200;
		readonly int MarkierungGrößeSammler = 50;
		readonly int MarkierungGrößeJäger = 50;

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
		/// Reichweite überschritten hat.
		/// </summary>
		public override void WirdMüde()
		{
            //GeheZuBau();
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
            /*if (Kaste == KasteTypen.Foodloot.ToString()) {
                if (!trägtNahrung)
                {
                    //Neues Ziel
                    ZielSammeln = zucker;

                    //Weitererzählen
                    SprüheMarkierung((int)Information.ZielNahrung, MarkierungGrößeSammler);

                    GeheZuZiel(zucker);
                }
            }
            if (Kaste == KasteTypen.Spotter.ToString()) {
                SprüheMarkierung((int)Information.ZielNahrung, MarkierungGrößeSpotter);
            }*/
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindstens ein
		/// Obststück sieht.
		/// </summary>
		/// <param name="obst">Das nächstgelegene Obststück.</param>
		public override void Sieht(Obst obst)
		{
            /*if (Kaste == KasteTypen.Foodloot.ToString() && BrauchtNochTräger(obst))
            {
                if (!trägtNahrung)
                {
                    //Neues Ziel
                    ZielSammeln = obst;

                    //Weitererzählen
                    SprüheMarkierung((int)Information.ZielNahrung, MarkierungGrößeSammler);

                    //Obst nehmen
                    GeheZuZiel(obst);
                }
            }
            if (Kaste == KasteTypen.Spotter.ToString())
            {
                SprüheMarkierung((int)Information.ZielNahrung, MarkierungGrößeSpotter);
            }*/
        }

		/// <summary>
		/// Wird einmal aufgerufen, wenn di e Ameise einen Zuckerhaufen als Ziel
		/// hat und bei diesem ankommt.
		/// </summary>
		/// <param name="zucker">Der Zuckerhaufen.</param>
		public override void ZielErreicht(Zucker zucker)
        { 
			/*
            //Zucker nehmen
            Nimm(zucker);
            trägtNahrung = true;
            GeheZuBau();
			*/
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
		}

		#endregion

		#region Kommunikation

        enum Information
        {
            ZielGegner,
            ZielNahrung,
            Hilfe
        }

		/// <summary>
		/// Wird einmal aufgerufen, wenn die Ameise eine Markierung des selben
		/// Volkes riecht. Einmal gerochene Markierungen werden nicht erneut
		/// gerochen.
		/// </summary>
		/// <param name="markierung">Die nächste neue Markierung.</param>
		public override void RiechtFreund(Markierung markierung)
		{
			/*
            switch(markierung.Information)
            {
                case (int)Information.ZielNahrung:
                    if (Kaste == KasteTypen.Foodloot.ToString()) {
                        if (!trägtNahrung)
                            GeheZuZiel(markierung);
                    }
                    break;
                case (int)Information.ZielGegner:
                    if (Kaste == KasteTypen.Aggro.ToString()) {
                        if (!greiftAn && Ziel == null)
                            GeheZuZiel(markierung);
                    }
                    break;
                case (int)Information.Hilfe:
                    if (!hilftFreund)
                    {
                        LasseNahrungFallen();
                        GeheZuZiel(markierung);
                    }
                    break;
					
            }*/
		}

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindstens eine Ameise des
		/// selben Volkes sieht.
		/// </summary>
		/// <param name="ameise">Die nächstgelegene befreundete Ameise.</param>
		public override void SiehtFreund(Ameise ameise)
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
					this.SprüheMarkierung(0, 10);
					this.GeheZuZiel(ameise);
					memory.ActionDone(DecisionType.Gruppieren);
				}
				else
				{
					//nicht gruppieren
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
			/*
            if (Kaste == KasteTypen.Aggro.ToString())
            {
                //Neues Ziel
                ZielGegner = wanze;

                //Weitererzählen
                SprüheMarkierung((int)Information.ZielGegner, MarkierungGrößeJäger);

                GreifeAn(wanze);
                greiftAn = true;
            }
            if (Kaste == KasteTypen.Spotter.ToString())
            {
                SprüheMarkierung((int)Information.ZielGegner, MarkierungGrößeSpotter);
            }*/
        }

		/// <summary>
		/// Wird wiederholt aufgerufen, wenn die Ameise mindestens eine Ameise eines
		/// anderen Volkes sieht.
		/// </summary>
		/// <param name="ameise">Die nächstgelegen feindliche Ameise.</param>
		public override void SiehtFeind(Ameise ameise)
		{
			
			//if (FuzzyInferenceSystem.DecisionKillAnt(character.wut, character.energie, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenAmeise)))
			if (FuzzyInferenceSystem.Superdecision5x5x2(character.wut, character.energie, character.angreifen, memory.GetDecisionValue(DecisionType.AngreifenAmeise)))
			{
				//Hilfe rufen

				//Angreifen
				GreifeAn(ameise);
				memory.ActionDone(DecisionType.AngreifenAmeise);
			}


			/*if (Kaste == KasteTypen.Aggro.ToString())
            {
                //Neues Ziel
                ZielGegner = ameise;

                //Weitererzählen
                SprüheMarkierung((int)Information.ZielGegner, MarkierungGrößeJäger);

                GreifeAn(ameise);
                greiftAn = true;
            }
            if (Kaste == KasteTypen.Spotter.ToString())
            {
                SprüheMarkierung((int)Information.ZielGegner, MarkierungGrößeSpotter);
            }*/
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

		}

		#endregion
		 
	}
}