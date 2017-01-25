using AntMe.DemoAmeisen;
using AntMe.Deutsch;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntMe.SmartassAnts
{
    class CharacterTypePart
    {
        //private int[] FuzzyValues = new int[] { 0, 0, 10, 20 };
        double min;
        double max;
        string name;
        public CharacterTypePart(string Name, double Min, double Max)
        {
            this.name = Name;
            this.min = Min;
            this.max = Max;

        }

        public override string ToString()
        {
            return name;
        }
    }

    abstract class CharacterType
    {
        protected int[][] FuzzyValues = {
			new int[] { 0, 0, 10, 20 },
			new int[] { 10, 20, 35, 45 },
			new int[] { 35, 45, 60, 70 },
			new int[] { 60, 70, 80, 95 },
			new int[] { 80, 95, 100, 100 } };

        protected double value;
        internal int numParts;
        internal DotFuzzy.LinguisticVariable characterParts;

		internal CharacterTypePart getPart(string Name)
        {
            return null;
        }  
		
		public override string ToString()
		{
			return characterParts.Name;
		}

		public virtual double Value()
		{
			return value;
		}

        /*        
        internal String DetermineCharacterForm()
        {
            if (characterParts == null)
                return "";

            DotFuzzy.FuzzyEngine defuzzer = new DotFuzzy.FuzzyEngine();
            defuzzer.FuzzyRuleCollection.Add()
            return ""; 
        }
        */
    }

	class Decision
	{ 
		public Decision(DecisionType DecisionType, string Option1, string Option2, bool FirstIsDefault)
		{
			option1 = Option1;
			option2 = Option2;
			firstIsDefault = FirstIsDefault;

			characterParts = new DotFuzzy.LinguisticVariable(DecisionType.ToString());
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction(Option1, 0, 0, 50, 50));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction(Option2, 50, 50, 100, 100));
		}

		private bool firstIsDefault;

		protected double value = 0.0;
		internal int numParts = 2;
		private string option1, option2;
		internal DotFuzzy.LinguisticVariable characterParts;
		private DecisionType decisionType; 

		public string Option1
		{
			get { return option1;}
			private set { option1 = value; }
		}

		public string Option2
		{
			get { return option2; }
			private set { option2 = value; }
		}

		public string DefaultDecision
		{
			get
			{
				if (firstIsDefault)
					return option1;
				else return option2;
			}
		}

		public bool FirstOptionIsDefaultDecision
		{
			get { return firstIsDefault; }
		}

		public override string ToString()
		{
			return characterParts.Name;
		}

		public virtual double Value()
		{
			return value;
		}
	}

    class Faulheit : CharacterType
    {
        
        public Faulheit(double InitValue)
        {
            this.value = InitValue;

            characterParts = new DotFuzzy.LinguisticVariable("Faulheit");
            characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Nicht_Faul", FuzzyValues[0][0], FuzzyValues[0][1], FuzzyValues[0][2], FuzzyValues[0][3]));
            characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Kaum_Faul", FuzzyValues[1][0], FuzzyValues[1][1], FuzzyValues[1][2], FuzzyValues[1][3]));
            characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Wenig_Faul", FuzzyValues[2][0], FuzzyValues[2][1], FuzzyValues[2][2], FuzzyValues[2][3]));
            characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Normal_Faul", FuzzyValues[3][0], FuzzyValues[3][1], FuzzyValues[3][2], FuzzyValues[3][3]));
            characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Sehr_Faul", FuzzyValues[4][0], FuzzyValues[4][1], FuzzyValues[4][2], FuzzyValues[4][3]));
        }
    }

    class Energie : CharacterType
    {
		protected Basisameise parentAnt;

        public Energie(double InitValue, Basisameise ParentAnt)
        {
			parentAnt = ParentAnt;
			value = InitValue;

            characterParts = new DotFuzzy.LinguisticVariable("Energie");
            characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Sehr_Schwach", FuzzyValues[0][0], FuzzyValues[0][1], FuzzyValues[0][2], FuzzyValues[0][3]));
            characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Schwach", FuzzyValues[1][0], FuzzyValues[1][1], FuzzyValues[1][2], FuzzyValues[1][3]));
            characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Normal", FuzzyValues[2][0], FuzzyValues[2][1], FuzzyValues[2][2], FuzzyValues[2][3]));
            characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Ausgeruht", FuzzyValues[3][0], FuzzyValues[3][1], FuzzyValues[3][2], FuzzyValues[3][3]));
            characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Sehr_Ausgeruht", FuzzyValues[4][0], FuzzyValues[4][1], FuzzyValues[4][2], FuzzyValues[4][3]));
        }

        public override double Value()
        {
            return parentAnt.AktuelleEnergie;
        }
    }

    class Wut : CharacterType
	{
		public Wut(double InitValue)
		{
			characterParts = new DotFuzzy.LinguisticVariable("Wut");
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Nicht_Wuetend", FuzzyValues[0][0], FuzzyValues[0][1], FuzzyValues[0][2], FuzzyValues[0][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Kaum_Wuetend", FuzzyValues[1][0], FuzzyValues[1][1], FuzzyValues[1][2], FuzzyValues[1][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Bisschen_Wuetend", FuzzyValues[2][0], FuzzyValues[2][1], FuzzyValues[2][2], FuzzyValues[2][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Normal_Wuetend", FuzzyValues[3][0], FuzzyValues[3][1], FuzzyValues[3][2], FuzzyValues[3][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Sehr_Wuetend", FuzzyValues[4][0], FuzzyValues[4][1], FuzzyValues[4][2], FuzzyValues[4][3]));
		}
	}

	class Character
    {
        internal Faulheit faulheit;
        internal Energie energie;
        internal Wut wut;
		
		internal Basisameise parentAnt;

		internal Decision laufen = new Decision(DecisionType.Laufen, "Steht", "Laeuft", true);
		internal Decision angreifen = new Decision(DecisionType.AngreifenAmeise, "Greift_nicht_an", "Greift_an", true);


		// ...

		public Character(Basisameise Parent)
        {
			//Init
			parentAnt = Parent;

			faulheit = new Faulheit(20);
			energie = new Energie(100, parentAnt);
			wut = new Wut(0.0);
						
			//Vereerben

			// ...
		}
    }
}
