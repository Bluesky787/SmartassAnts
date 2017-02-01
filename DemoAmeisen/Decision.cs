using System;
using System.Collections.Generic;
using System.Text;

namespace AntMe.SmartassAnts
{
	public enum DecisionType
	{
		Laufen,
		Wegrennen,
		Gruppieren,
		AngreifenAmeise,
		AngreifenWanze,
		SammelnZucker,
		SammelnObst
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
			get { return option1; }
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
}
