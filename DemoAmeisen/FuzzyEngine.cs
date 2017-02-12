using System;
using System.Collections.Generic;
using System.Text;
using DotFuzzy;

namespace AntMe.SmartassAnts
{
    class FuzzyInferenceSystem
    {
        public FuzzyInferenceSystem()
        {

        }

		private static bool generateDecision(double probability)
		{
			Random rand = new Random();
			double randVal = rand.NextDouble();

			if (randVal <= probability)
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

		public static bool Superdecision5x5x2(CharacterType CharacterDependency1, Circumstance CharacterDependency2, Decision CharacterConsequent, double ratingAction)
		{
			FuzzyEngine defuzzer = new FuzzyEngine();
			defuzzer.LinguisticVariableCollection.Add(CharacterDependency1.characterParts);
			defuzzer.LinguisticVariableCollection.Add(CharacterDependency2.characterParts);
			defuzzer.LinguisticVariableCollection.Add(CharacterConsequent.characterParts);

			defuzzer.Consequent = CharacterConsequent.ToString();

			//Allgemeine Regel: C1_x AND C2_y → z_1, wenn y = 5-x;   x,y = [0..4]
			//Greift keine Regel → Standardwert (nicht von DotFuzzy unterstützt)
			for (int x = 4; x >= 0; x--)
			{
				for (int y = x; y < 5; y++)
				{
					defuzzer.FuzzyRuleCollection.Add(new FuzzyRule("IF (" + CharacterDependency1.characterParts.Name + " IS " + CharacterDependency1.characterParts.MembershipFunctionCollection[x].Name + ") AND ("+ CharacterDependency2.characterParts.Name + " IS " + CharacterDependency2.characterParts.MembershipFunctionCollection[y].Name + ") THEN " + CharacterConsequent.characterParts.Name + " IS " + CharacterConsequent.characterParts.MembershipFunctionCollection[1].Name));
				}
			} 
			
			CharacterDependency1.characterParts.InputValue = CharacterDependency1.Value();
			CharacterDependency2.characterParts.InputValue = CharacterDependency2.Value();

			double defuzzedValue = defuzzer.Defuzzify();

			//Standardwert setzen
			if (double.IsNaN(defuzzedValue))
			{
				return CharacterConsequent.FirstOptionIsDefaultDecision;
			}
			else
			{

				//Gedächtnis einbeziehen (wie gut war die Entscheidung beim letzten Mal?)
				defuzzedValue += ratingAction * 100;
				defuzzedValue /= 2.0;

				return generateDecision(defuzzedValue);
			}
		}
    }
}
