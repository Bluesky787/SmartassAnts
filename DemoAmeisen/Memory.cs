using AntMe.Deutsch;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntMe.SmartassAnts
{
	class Memory
	{
		private List<DecisionType> lastActions;
		private static double ratingLaufen = 0.5, ratingWegrennen = 0.5, ratingAngreifenAmeise = 0.5, ratingAngreifenWanze = 0.5, ratingSammelnZucker = 0.5, ratingSammelnObst = 0.5, ratingWarten = 0.5;
        private SmartassAnt parentAnt;

        internal static Zucker gemerkterZucker = null;
        internal static int gemerkterZucker_EntfernungZuBau = 9000000;

		double learneffect = 0.01;

		public Memory(SmartassAnt ParentAnt)
		{
			lastActions = new List<DecisionType>();

            //Nicht bei Static-Variablen!!!
            /*
            ratingLaufen = 0.50;
			ratingWegrennen = 0.50;
			ratingAngreifenAmeise = 0.50;
			ratingAngreifenWanze = 0.50;
			ratingSammelnZucker = 0.50;
			ratingSammelnObst = 0.50;
            */
            this.parentAnt = ParentAnt;
		}
		
        public DecisionType LastAction
        {
            get
            {
                if (lastActions.Count > 0)
                    return lastActions[lastActions.Count - 1];
                else return DecisionType.Laufen;
            }
        }

		public void ActionDone(DecisionType Decision)
		{
			lastActions.Add(Decision);
		}

		public void ActionSuccessful()
		{
            if (lastActions != null && lastActions.Count != 0)
            {
                foreach (DecisionType d in lastActions)
                {
                    switch (d)
                    {
                        case DecisionType.AngreifenAmeise:
                            ratingAngreifenAmeise = Math.Min(ratingAngreifenAmeise + learneffect, 1.0);
                            break;
                        case DecisionType.AngreifenWanze:
                            ratingAngreifenWanze = Math.Min(ratingAngreifenWanze + learneffect, 1.0);
                            break;
                        case DecisionType.Laufen:
                            ratingLaufen = Math.Min(ratingLaufen + learneffect, 1.0);
                            ratingWarten = Math.Max(ratingWarten - (0.5* learneffect), 0.0);
                            break;
                        case DecisionType.SammelnObst:
                            ratingSammelnObst = Math.Min(ratingSammelnObst + learneffect, 1.0);
                            break;
                        case DecisionType.SammelnZucker:
                            ratingSammelnZucker = Math.Min(ratingSammelnZucker + learneffect, 1.0);
                            break;
                        case DecisionType.Warten:
                            ratingWarten = Math.Min(ratingWegrennen + learneffect, 1.0);
                            ratingLaufen = Math.Max(ratingLaufen - (0.5 * learneffect), 0.0);
                            break;
                        case DecisionType.Wegrennen:
                            ratingWegrennen = Math.Min(ratingWegrennen + learneffect, 1.0);
                            break;
                        default:
                            break;

                    }
                }
                lastActions.Clear();
            }
		}

		public void ActionUnsuccessful()
		{
            if (lastActions != null && lastActions.Count != 0)
            {
                foreach (DecisionType d in lastActions)
                {
                    switch (d)
                    {
                        case DecisionType.AngreifenAmeise:
                            ratingAngreifenAmeise = Math.Max(ratingAngreifenAmeise - learneffect, 0.0);
                            break;
                        case DecisionType.AngreifenWanze:
                            ratingAngreifenWanze = Math.Max(ratingAngreifenWanze - learneffect, 0.0);
                            break;
                        case DecisionType.Laufen:
                            ratingLaufen = Math.Max(ratingLaufen - learneffect, 0.0);
                            ratingWarten = Math.Min(ratingWarten + (0.5 * learneffect), 1.0);
                            break;
                        case DecisionType.SammelnObst:
                            ratingSammelnObst = Math.Max(ratingSammelnObst - learneffect, 0.0);
                            break;
                        case DecisionType.SammelnZucker:
                            ratingSammelnZucker = Math.Max(ratingSammelnZucker - learneffect, 0.0);
                            break;
                        case DecisionType.Warten:
                            ratingWarten = Math.Max(ratingWarten - learneffect, 0.0);
                            ratingLaufen = Math.Min(ratingLaufen + (0.5 * learneffect), 1.0);
                            break;
                        case DecisionType.Wegrennen:
                            ratingWegrennen = Math.Max(ratingWegrennen - learneffect, 0.0);
                            break;
                        default:
                            break;
                    }
                }
                lastActions.Clear();
            }
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
