using System;
using System.Collections.Generic;
using System.Text;

namespace AntMe.DemoAmeisen
{
	public enum DecisionType
	{
		Laufen,
		Wegrennen,
		AngreifenAmeise,
		AngreifenWanze,
		SammelnZucker,
		SammelnObst
	}

	class Memory
	{
		private List<DecisionType> lastActions;
		private double ratingLaufen, ratingWegrennen, ratingAngreifenAmeise, ratingAngreifenWanze, ratingSammelnZucker, ratingSammelnObst;

		double learneffect = 0.1;

		public Memory()
		{
			lastActions = new List<DecisionType>();
			ratingLaufen = 0.50;
			ratingWegrennen = 0.50;
			ratingAngreifenAmeise = 0.50;
			ratingAngreifenWanze = 0.50;
			ratingSammelnZucker = 0.50;
			ratingSammelnObst = 0.50;
		}
		
		public void ActionDone(DecisionType Decision)
		{
			lastActions.Add(Decision);
		}

		public void ActionSuccessful()
		{
			foreach (DecisionType d in lastActions)
			{
				switch (d)
				{
					case DecisionType.AngreifenAmeise:
						ratingAngreifenAmeise = Math.Min(ratingAngreifenAmeise + learneffect, 1.0);
						break;
					case DecisionType.AngreifenWanze:
						ratingAngreifenWanze= Math.Min(ratingAngreifenWanze + learneffect, 1.0);
						break;
					case DecisionType.Laufen:
						ratingLaufen = Math.Min(ratingLaufen + learneffect, 1.0); ;
						break;
					case DecisionType.SammelnObst:
						ratingSammelnObst = Math.Min(ratingSammelnObst + learneffect, 1.0); ;
						break;
					case DecisionType.SammelnZucker:
						ratingSammelnZucker = Math.Min(ratingSammelnZucker + learneffect, 1.0); ;
						break;
					case DecisionType.Wegrennen:
						ratingWegrennen = Math.Min(ratingWegrennen + learneffect, 1.0); ;
						break;
					deNormal_Fault:
						break;

				}
			}
			lastActions.Clear();
		}

		public void ActionUnsuccessful()
		{
			foreach (DecisionType d in lastActions)
			{
				switch (d)
				{
					case DecisionType.AngreifenAmeise:
						ratingAngreifenAmeise = Math.Max(ratingAngreifenAmeise - learneffect, 0.0); 
						break;
					case DecisionType.AngreifenWanze:
						ratingAngreifenWanze = Math.Max(ratingAngreifenWanze - learneffect, 0.0); ;
						break;
					case DecisionType.Laufen:
						ratingLaufen = Math.Max(ratingLaufen - learneffect, 0.0); ;
						break;
					case DecisionType.SammelnObst:
						ratingSammelnObst = Math.Max(ratingSammelnObst - learneffect, 0.0); ;
						break;
					case DecisionType.SammelnZucker:
						ratingSammelnZucker = Math.Max(ratingSammelnZucker - learneffect, 0.0); ;
						break;
					case DecisionType.Wegrennen:
						ratingWegrennen = Math.Max(ratingWegrennen - learneffect, 0.0); ;
						break;
					deNormal_Fault:
						break;

				}
			}
			lastActions.Clear();
		}

		public double GetDecisionValue(DecisionType Decision)
		{
			switch (Decision)
			{
				case DecisionType.AngreifenAmeise:
					return ratingAngreifenAmeise;
				case DecisionType.AngreifenWanze:
					return ratingAngreifenWanze;
				case DecisionType.Laufen:
					return ratingLaufen;
				case DecisionType.SammelnObst:
					return ratingSammelnObst;
				case DecisionType.SammelnZucker:
					return ratingSammelnZucker;
				case DecisionType.Wegrennen:
					return ratingWegrennen;
				default:
					return 0.5;
			}
		}
	}
}
