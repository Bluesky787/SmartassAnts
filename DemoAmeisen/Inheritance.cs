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

        public static void InheritAnt(Todesart DeathReasonLastAnt)
        {
            InheritAnt(getOldestLivingAnt(), getRandomLivingAnt(), DeathReasonLastAnt);
        }

        public static void InheritAnt(SmartassAnt Parent1, SmartassAnt Parent2, Todesart DeathReason)
        {
            
        }
    }
}
