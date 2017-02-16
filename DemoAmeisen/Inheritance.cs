using AntMe.Deutsch;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntMe.SmartassAnts
{
    class Inheritance
    {
        public static List<SmartassAnt> List = new List<SmartassAnt>();
        
        public static SmartassAnt getRandomLivingAnt()
        {
            return null;
        }

        public static SmartassAnt getOldestLivingAnt()
        {
            return null;
        }

        public static Character InheritAnt(SmartassAnt DiedAnt)
        {
            return InheritAnt(getOldestLivingAnt().character, getRandomLivingAnt().character, DiedAnt.Todesart);
        }

        private static Character InheritAnt(Character Parent1, Character Parent2, Todesart DeathReason)
        {
            Character inheritedCharacter = new Character();

            return inheritedCharacter;
        }
    }
}
