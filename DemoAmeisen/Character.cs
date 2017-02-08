using AntMe.Deutsch;

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

	class Teamfaehigkeit : CharacterType
	{
		public Teamfaehigkeit(double InitValue)
		{
			this.value = InitValue;

			characterParts = new DotFuzzy.LinguisticVariable("Teamfaehigkeit");
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Nicht_Teamfaehig", FuzzyValues[0][0], FuzzyValues[0][1], FuzzyValues[0][2], FuzzyValues[0][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Kaum_Teamfaehig", FuzzyValues[1][0], FuzzyValues[1][1], FuzzyValues[1][2], FuzzyValues[1][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Wenig_Teamfaehig", FuzzyValues[2][0], FuzzyValues[2][1], FuzzyValues[2][2], FuzzyValues[2][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Normal_Teamfaehig", FuzzyValues[3][0], FuzzyValues[3][1], FuzzyValues[3][2], FuzzyValues[3][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Sehr_Teamfaehig", FuzzyValues[4][0], FuzzyValues[4][1], FuzzyValues[4][2], FuzzyValues[4][3]));
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
        //Circumstances
        internal AmeisenFeindeInNaehe ameisenFeindeInNaehe;
        internal AmeisenFreundeInNaehe ameisenFreundeInNaehe;
        internal Energie energie;
        
		//CharacterTypes
		internal Faulheit faulheit;
        internal Wut wut;
		internal Teamfaehigkeit teamfaehigkeit;
		
		//Decisions
		internal Decision laufen = new Decision(DecisionType.Laufen, "Steht", "Laeuft", true);
		internal Decision angreifen = new Decision(DecisionType.AngreifenAmeise, "Greift_nicht_an", "Greift_an", true);
		internal Decision gruppieren = new Decision(DecisionType.Gruppieren, "Nicht_gruppieren", "Gruppieren", true);

		internal Basisameise parentAnt;

		// ...

		public Character(Basisameise Parent)
        {
			//Init
			parentAnt = Parent;

            //Circumstances
            energie = new Energie(CircumstanceType.Energie, parentAnt);


            faulheit = new Faulheit(20);
			wut = new Wut(0.0);
			teamfaehigkeit = new Teamfaehigkeit(20);
						
			//Vereerben

			// ...
		}
    }
}
