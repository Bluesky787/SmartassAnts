using AntMe.Deutsch;
using AntMe.SmartassAnts;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntMe.SmartassAnts
{
	public enum CircumstanceType
	{
		AmeisenFreundeInNähe,
		AmeisenFeindeInNähe,
		Energie,
		Punktzahl
	}

	abstract class Circumstance : CharacterType
	{
		/// <summary>
		/// Bestimmt den Wert des äußeren Einflusses
		/// </summary>
		internal abstract void setValue();
		protected Basisameise parentAnt;


		internal Circumstance(CircumstanceType circumstanceType, Basisameise parentAnt)
		{
			this.circumstanceType = circumstanceType;
			this.parentAnt = parentAnt;
		}

		internal CircumstanceType circumstanceType;

		public override double Value()
		{
			setValue();
			return value;
		}
	}

	class Energie : Circumstance
	{
		public Energie(CircumstanceType circumstanceType, Basisameise parentAnt) : base(circumstanceType, parentAnt)
		{
			characterParts = new DotFuzzy.LinguisticVariable("Energie");
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Sehr_Schwach", FuzzyValues[0][0], FuzzyValues[0][1], FuzzyValues[0][2], FuzzyValues[0][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Schwach", FuzzyValues[1][0], FuzzyValues[1][1], FuzzyValues[1][2], FuzzyValues[1][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Normal", FuzzyValues[2][0], FuzzyValues[2][1], FuzzyValues[2][2], FuzzyValues[2][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Ausgeruht", FuzzyValues[3][0], FuzzyValues[3][1], FuzzyValues[3][2], FuzzyValues[3][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Sehr_Ausgeruht", FuzzyValues[4][0], FuzzyValues[4][1], FuzzyValues[4][2], FuzzyValues[4][3]));
		}

		internal override void setValue()
		{
			this.value = parentAnt.AktuelleEnergie;
		}
	}

	class AmeisenFreundeInNaehe : Circumstance
	{
		public AmeisenFreundeInNaehe(CircumstanceType circumstanceType, Basisameise parentAnt) : base(circumstanceType, parentAnt)
		{
			characterParts = new DotFuzzy.LinguisticVariable("AmeisenFreundeInNaehe");
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Keine", FuzzyValues[0][0], FuzzyValues[0][1], FuzzyValues[0][2], FuzzyValues[0][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Kaum", FuzzyValues[1][0], FuzzyValues[1][1], FuzzyValues[1][2], FuzzyValues[1][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Wenige", FuzzyValues[2][0], FuzzyValues[2][1], FuzzyValues[2][2], FuzzyValues[2][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Genug", FuzzyValues[3][0], FuzzyValues[3][1], FuzzyValues[3][2], FuzzyValues[3][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Viele", FuzzyValues[4][0], FuzzyValues[4][1], FuzzyValues[4][2], FuzzyValues[4][3]));
		}

		internal override void setValue()
		{
			switch (parentAnt.AnzahlAmeisenDesTeamsInSichtweite)
			{
				case 0:
					value = 10;
					break;
				case 1:
				case 2:
					value = 35;
					break;
				case 3:
				case 4:
				case 5:
					value = 60;
					break;
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
					value = 80;
					break;
				default:
					value = 100;
					break;
			}
		}
	}

	class AmeisenFeindeInNaehe : Circumstance
	{
		public AmeisenFeindeInNaehe(CircumstanceType circumstanceType, Basisameise parentAnt) : base(circumstanceType, parentAnt)
		{
			characterParts = new DotFuzzy.LinguisticVariable("AmeisenFeindeInNaehe");
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Keine", FuzzyValues[0][0], FuzzyValues[0][1], FuzzyValues[0][2], FuzzyValues[0][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Kaum", FuzzyValues[1][0], FuzzyValues[1][1], FuzzyValues[1][2], FuzzyValues[1][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Wenige", FuzzyValues[2][0], FuzzyValues[2][1], FuzzyValues[2][2], FuzzyValues[2][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Genug", FuzzyValues[3][0], FuzzyValues[3][1], FuzzyValues[3][2], FuzzyValues[3][3]));
			characterParts.MembershipFunctionCollection.Add(new DotFuzzy.MembershipFunction("Viele", FuzzyValues[4][0], FuzzyValues[4][1], FuzzyValues[4][2], FuzzyValues[4][3]));
		}

		internal override void setValue()
		{
			switch (parentAnt.AnzahlFremderAmeisenInSichtweite)
			{
				case 0:
					value = 10;
					break;
				case 1:
				case 2:
					value = 35;
					break;
				case 3:
				case 4:
					value = 60;
					break;
				case 5:
				case 6:
				case 7:
					value = 80;
					break;
				default:
					value = 100;
					break;
			}
		}
	}
}
