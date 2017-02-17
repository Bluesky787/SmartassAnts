using AntMe.Deutsch;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntMe.SmartassAnts
{
    class Inheritance
    {
        
        public static SmartassAnt getRandomLivingAnt()
        {
            Random rand = new Random();
            //Keys werden nicht als Index abgefragt, sondern sind fortlaufend
            //daher muss zuerst ein gültiger Key aus der Auflistung aller Keys der derzeit lebenden Ameisen gesucht werden
            return SmartassAnt.Ants[SmartassAnt.Ants.Keys[rand.Next(SmartassAnt.Ants.Count)]];
        }

        public static SmartassAnt getOldestLivingAnt()
        {
            //Liste aller Ameisen in SmartassAnts ist bereits sortiert
            return SmartassAnt.Ants[SmartassAnt.Ants.Keys[0]];
        }

        public static Character InheritAnt(SmartassAnt DiedAnt)
        {
            return InheritAnt(getOldestLivingAnt(), getRandomLivingAnt(), DiedAnt);
        }

        private static Character InheritAnt(SmartassAnt Parent1, SmartassAnt Parent2, SmartassAnt DiedAnt)
        {
            Todesart DeathReason = DiedAnt.Todesart;
            Character inheritedCharacter = new Character();

            //Vereerbungsfunktion
            inheritedCharacter.faulheit.OverrideValue((Parent1.character.faulheit.Value() + Parent2.character.faulheit.Value()) / 2.0);
            inheritedCharacter.teamfaehigkeit.OverrideValue((Parent1.character.teamfaehigkeit.Value() + Parent2.character.teamfaehigkeit.Value()) / 2.0);
            inheritedCharacter.wut.OverrideValue((Parent1.character.wut.Value() + Parent2.character.wut.Value()) / 2.0);

            //Memory (inkl. Ratings vereerben?)


            //Lernen von gestorbener Ameise
            switch (DeathReason)
            {
                case Todesart.Besiegt:
                    //Weniger Wut, weil das zum Tod führt
                    inheritedCharacter.wut.OverrideValue(Math.Max(inheritedCharacter.wut.Value() - 0.2, 0.0));
                    //Mehr Teamfähigkeit, weil sie im Team seltener stirbt
                    inheritedCharacter.teamfaehigkeit.OverrideValue(Math.Min(inheritedCharacter.teamfaehigkeit.Value() + 0.2, 1.0));
                    break;

                case Todesart.Gefressen:
                    //Weniger Wut, weil das zum Tod führt
                    inheritedCharacter.wut.OverrideValue(Math.Max(inheritedCharacter.wut.Value() - 0.2, 0.0));
                    //Weniger Faulheit, damit sie Wanzen nicht mehr im Weg stehen bleiben
                    inheritedCharacter.faulheit.OverrideValue(Math.Max(inheritedCharacter.faulheit.Value() - 0.2, 0.0));
                    //Mehr Teamfähigkeit, weil sie im Team seltener stirbt
                    inheritedCharacter.teamfaehigkeit.OverrideValue(Math.Min(inheritedCharacter.teamfaehigkeit.Value() + 0.2, 1.0));
                    break;
                    
                case Todesart.Verhungert:
                    //Mehr Faulheit, weil sie nicht verhungern können, wenn sie stehen bleiben
                    inheritedCharacter.faulheit.OverrideValue(Math.Min(inheritedCharacter.faulheit.Value() + 0.2, 1.0));
                    //Weniger Teamfähigkeit, damit sie nicht mehr beim helfen verhungern kann
                    inheritedCharacter.teamfaehigkeit.OverrideValue(Math.Max(inheritedCharacter.teamfaehigkeit.Value() - 0.2, 0.0));
                   break;
            }

            //Mutation
            Random randBool = new Random(), randDirection = new Random(), randValue = new Random();

            //Faulheit
            if (randBool.Next(2) == 1)
            {
                if (randDirection.NextDouble() >= 0.5)
                {
                    //Wert verstärken, höchstens 50% mehr, aber nicht auf mehr als 1.0
                    inheritedCharacter.faulheit.OverrideValue(Math.Max(inheritedCharacter.faulheit.Value() + 0.15 * randValue.NextDouble(), 1.0));
                }
                else
                {
                    //Wert verringern, auf maximal um Hälfte, aber nicht auf weniger als 0.0
                    inheritedCharacter.faulheit.OverrideValue(Math.Min(inheritedCharacter.faulheit.Value() - 0.15 * randValue.NextDouble(), 0.0));
                }
            }

            //Teamfähigkeit
            if (randBool.Next(2) == 1)
            {
                if (randDirection.NextDouble() >= 0.5)
                {
                    //Wert verstärken, höchstens 50% mehr, aber nicht auf mehr als 1.0
                    inheritedCharacter.teamfaehigkeit.OverrideValue(Math.Max(inheritedCharacter.teamfaehigkeit.Value() + 0.15 * randValue.NextDouble(), 1.0));
                }
                else
                {
                    //Wert verringern, auf maximal um Hälfte, aber nicht auf weniger als 0.0
                    inheritedCharacter.teamfaehigkeit.OverrideValue(Math.Min(inheritedCharacter.teamfaehigkeit.Value() - 0.15 * randValue.NextDouble(), 0.0));
                }
            }

            //Wut
            if (randBool.Next(2) == 1)
            {
                if (randDirection.NextDouble() >= 0.5)
                {
                    //Wert verstärken, höchstens 50% mehr, aber nicht auf mehr als 1.0
                    inheritedCharacter.wut.OverrideValue(Math.Max(inheritedCharacter.wut.Value() + 0.15 * randValue.NextDouble(), 1.0));
                }
                else
                {
                    //Wert verringern, auf maximal um Hälfte, aber nicht auf weniger als 0.0
                    inheritedCharacter.wut.OverrideValue(Math.Min(inheritedCharacter.wut.Value() - 0.15 * randValue.NextDouble(), 0.0));
                }
            }
            return inheritedCharacter;
        }
    }
}
