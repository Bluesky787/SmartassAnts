using System;
using System.Collections.Generic;
using System.Text;
using DotFuzzy;

namespace AntMe.SmartassAnts
{
    class FuzzyInferenceSystem
    {
        static double PreferenceWeight = 2.0;

        public FuzzyInferenceSystem()
        {

        }

		private static bool generateDecision(double probability)
		{
			Random rand = new Random();
			double randVal = rand.NextDouble();

			if (randVal * 100 <= probability)
				return true;
			else
				return false;
		}

        /*public static bool DecisionHaveABreak(Faulheit faulheit, Energie energie, Laufen laufen, double ratingLaufen)
        {
            FuzzyEngine defuzzer = new FuzzyEngine();
            defuzzer.LinguisticVariableCollection.Add(faulheit.characterParts);
            defuzzer.LinguisticVariableCollection.Add(energie.characterParts);
            defuzzer.LinguisticVariableCollection.Add(laufen.characterParts);

            defuzzer.Consequent = "Laufen";

            defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Faulheit IS Sehr_Faul) THEN Laufen IS Steht"));
            defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Faulheit IS Normal_Faul) AND (Energie IS Sehr_Ausgeruht) THEN Laufen IS Laeuft"));
            defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Faulheit IS Wenig_Faul) AND ((Energie IS Ausgeruht) OR (Energie IS Sehr_Ausgeruht))  THEN Laufen IS Laeuft"));
            defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Faulheit IS Kaum_Faul) AND ((Energie IS Normal) OR (Energie IS Ausgeruht) OR (Energie IS Sehr_Ausgeruht)) THEN Laufen IS Laeuft"));

            defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Energie IS Sehr_Schwach) THEN Laufen IS Steht"));
            defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Energie IS Schwach) AND ((Faulheit IS Kaum_Faul) OR (Faulheit IS Wenig_Faul) OR (Faulheit IS Normal_Faul) OR (Faulheit IS Sehr_Faul)) THEN Laufen IS Steht"));
            defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Energie IS Normal) AND ((Faulheit IS Wenig_Faul) OR (Faulheit IS Normal_Faul) OR (Faulheit IS Sehr_Faul)) THEN Laufen IS Steht"));
            defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Energie IS Ausgeruht) AND ((Faulheit IS Normal_Faul) OR (Faulheit IS Sehr_Faul)) THEN Laufen IS Steht"));
            defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Energie IS Sehr_Ausgeruht) THEN Laufen IS Laeuft"));

            faulheit.characterParts.InputValue = faulheit.Value();
            energie.characterParts.InputValue = energie.Value();

            double defuzzedValue = defuzzer.Defuzzify() / 100.0;

			//Gedächtnis einbeziehen (wie gut war die Entscheidung beim letzten Mal?)
			defuzzedValue += ratingLaufen;
			defuzzedValue /= 2.0;

            return generateDecision(defuzzedValue);
        }

		public static bool DecisionKillAnt(Wut wut, Energie energie, Angreifen angreifen, double ratingKillAnt)
		{
			//Unterschied Hilfe und selbstständiger Angriff?
			FuzzyEngine defuzzer = new FuzzyEngine();
			defuzzer.LinguisticVariableCollection.Add(wut.characterParts);
			defuzzer.LinguisticVariableCollection.Add(energie.characterParts);
			defuzzer.LinguisticVariableCollection.Add(angreifen.characterParts);

			defuzzer.Consequent = "Angreifen";

			defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Wut IS Sehr_Wuetend) THEN Angreifen IS Greift_an"));
			defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Wut IS Wuetend) AND ((Energie IS Sehr_Augeruht) OR (Energie IS Ausgeruht) OR (Energie IS Normal)) THEN Angreifen IS Greift_an"));
			defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Wut IS Bisschen_Wuetend) AND ((Energie IS Sehr_Augeruht) OR (Energie IS Ausgeruht)) THEN Angreifen IS Greift_an"));
			defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Wut IS Kaum_Wuetend) AND (Energie IS Sehr_Ausgeruht) THEN Angreifen IS Greift_an"));

			defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Wut IS Nicht_Wuetend) THEN Angreifen IS Greift_nicht_an"));


			defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (Energie IS Sehr Schwach) THEN Angreifen IS Greift_nicht_an"));

			//Standardwert Greift_nicht_an?
			wut.characterParts.InputValue = wut.Value();
			energie.characterParts.InputValue = energie.Value();

			double defuzzedValue = defuzzer.Defuzzify();

			//Gedächtnis einbeziehen (wie gut war die Entscheidung beim letzten Mal?)
			defuzzedValue += ratingKillAnt;
			defuzzedValue /= 2.0;

			return generateDecision(defuzzedValue);
		}

		public static bool DecisionKillBug()
		{
			return false;
		}

		public static bool DecisionFlee()
		{
			return false;
		}

		public static bool DecisionTakeSugar()
		{
			return false;
		}

		public static bool DecisionTakeFruit()
		{
			return false;
		}

		public static bool DecisionContinueTask()
		{
			return false;
		}

		public static bool DecisionNewTask()
		{
			return false;
		}

		public static bool DecisionGoHome()
		{
			return false;
		}
		*/

        /// <summary>
        /// Entscheidet eine Aktion basierend auf den CHaractereinflüssen, den aktuellen Umständen und dem Memory.
        /// </summary>
        /// <param name="CharacterDependency1"></param>
        /// <param name="CharacterDependency2"></param>
        /// <param name="CharacterConsequent"></param>
        /// <param name="ratingAction"></param>
        /// <returns>True, wenn Entscheidungsoption 1 gewählt wurde.</returns>
        public static bool Superdecision5x5x2(CharacterType CharacterDependency1, Circumstance CharacterDependency2, Decision CharacterConsequent, double ratingAction)
        {
            return generateDecision(Superdecision5x5x2_Double(CharacterDependency1, CharacterDependency2, CharacterConsequent, ratingAction));
        }

        /// <summary>
        /// Liefert Entscheidungswert für eine Aktion basierend auf den CHaractereinflüssen, den aktuellen Umständen und dem Memory.
        /// </summary>
        /// <param name="CharacterDependency1"></param>
        /// <param name="CharacterDependency2"></param>
        /// <param name="CharacterConsequent"></param>
        /// <param name="ratingAction"></param>
        /// <returns>Wert für Entscheidungsoption 1</returns>
        public static double Superdecision5x5x2_Double(CharacterType CharacterDependency1, Circumstance CharacterDependency2, Decision CharacterConsequent, double ratingAction)
        {
            //Abfangen von Characterwerten über 100
            //z.B. bei Energie > 100 (kann nur bei Dependency2 passieren)
            bool useAdjustedCharacterValue = false;
            double adjustedCharacterValue = 0;
            if (CharacterDependency2.Value() > 100)
            {
                useAdjustedCharacterValue = true;
                int diff = (int)CharacterDependency2.Value() - 100;
                switch (CharacterDependency1.TypeOfCharacter)
                {
                    case CharacterTypes.Faulheit:
                    case CharacterTypes.Wut:
                        adjustedCharacterValue = Math.Max(CharacterDependency1.Value() - diff, 0);
                        break;
                    case CharacterTypes.Teamfaehigkeit:
                        adjustedCharacterValue = Math.Min(CharacterDependency1.Value() + diff, 100);
                        break;
                    default:
                        break;
                }

            }


            FuzzyEngine defuzzer = new FuzzyEngine();
            defuzzer.LinguisticVariableCollection.Add(CharacterDependency1.characterParts);
            defuzzer.LinguisticVariableCollection.Add(CharacterDependency2.characterParts);
            defuzzer.LinguisticVariableCollection.Add(CharacterConsequent.characterParts);

            defuzzer.Consequent = CharacterConsequent.ToString();

            //Allgemeine Regel: C1_x AND C2_y → z_1, wenn y >= 5-x;   x,y = [0..4]
            //Greift keine Regel → Standardwert (nicht von DotFuzzy unterstützt)
            for (int x = 4; x >= 0; x--)
            {
                for (int y = x; y < 5; y++)
                {
                    defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (" + CharacterDependency1.characterParts.Name + " IS " + CharacterDependency1.characterParts.MembershipFunctionCollection[x].Name + ") AND (" + CharacterDependency2.characterParts.Name + " IS " + CharacterDependency2.characterParts.MembershipFunctionCollection[y].Name + ") THEN " + CharacterConsequent.characterParts.Name + " IS " + CharacterConsequent.characterParts.MembershipFunctionCollection[1].Name));
                }
            }

            //Wenn durch eine Ameisenklasse bspw. ein höherer Energiewert zugelassen wird, dann muss das hier berücksichtigt werden
            if (useAdjustedCharacterValue)
            {
                CharacterDependency1.characterParts.InputValue = adjustedCharacterValue; //für positiven Effekt veränderter Wert
                CharacterDependency2.characterParts.InputValue = 100; //normierter Wert
            }
            else
            {
                CharacterDependency1.characterParts.InputValue = CharacterDependency1.Value();
                CharacterDependency2.characterParts.InputValue = CharacterDependency2.Value();
            }
            double defuzzedValue = defuzzer.Defuzzify();

            //Standardwert setzen
            //Immer dann, wenn keine positive Entscheidung getroffen wird, es also keine Regel dazu gab, dann defuzzedValue auf Wert für negative (default) Entscheidung setzen.
            if (double.IsNaN(defuzzedValue))
            {
                defuzzedValue = CharacterConsequent.FirstOptionIsDefaultDecision ? 0.25 : 0.75;
            }


            //Gedächtnis einbeziehen (wie gut war die Entscheidung beim letzten Mal?)
            //Durchschnitt reicht nicht aus, beachtet extreme Werte nicht ausreichend
            //-> Extremere Werte sollen die Entscheidung stärker beeinflussen
            if (ratingAction > 0.5)
            {
                ratingAction = Math.Sqrt(ratingAction);
            }

            if (ratingAction < 0.5)
            {
                ratingAction *= ratingAction;
            }

            defuzzedValue = Overthink(ratingAction, defuzzedValue, PreferenceWeight); //zwischen 0..100

            return defuzzedValue;

        }

        /// <summary>
        /// Überdenkt die Basisentscheidung, indem sie sie mit den erlernten Entscheidungspräferenzen der Memory-Klasse verknüpft. Die Verknüpfung erfolgt über eine gewichtete Mittelung. 
        /// </summary>
        /// <param name="ratingAction">Die zu der Entscheidung gehörende Entscheidungspräferenz der Memory-Klasse</param>
        /// <param name="defuzzedValue">Der Basiswert der Entscheidung</param>
        /// <param name="preferenceWeight">Die Gewichtung der Entscheidungspräferenz als Verhältnis zur zum Basiswert. Ein Wert von 2 bedeutet, dass die Präferenz zu 2 Teilen, der Basiswert zu einem Teil in die neue Entscheidung eingeht.</param>
        /// <returns></returns>
        private static double Overthink(double ratingAction, double defuzzedValue, double preferenceWeight)
        {
            //Durchschnitt mit doppelter Gewichtung des Ratings
            //defuzzedValue stets zwischen 0..100
            //ratingAction stets zwischen 0..1
            defuzzedValue += ratingAction * preferenceWeight * 100; //bei weight=2 zwischen 0..300 
            defuzzedValue /= (preferenceWeight + 1);
            return defuzzedValue;
        }

        public static bool CorrelateDecisionfunctions(double ValueFunction1, double ValueFunction2)
        {
            double averageValue = (ValueFunction1 + ValueFunction2) / 2.0;
            return generateDecision(averageValue);
        }
    }
}
