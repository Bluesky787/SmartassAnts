using System;
using System.Collections.Generic;
using System.Text;

namespace AntMe.SmartassAnts
{
    class CharacterTypePart
    {
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
        internal double value;
        internal int numParts;
        internal List<CharacterTypePart> parts;  
        
        internal CharacterTypePart getPart(string Name)
        {
            return null;
        }      
    }

    class Egoismus : CharacterType
    {
        public Egoismus()
        {
            this.parts = new List<CharacterTypePart>();
            parts.Add(new CharacterTypePart("Sehr egoistisch", 0.8, 1.0));
            //...

            //parts[0].ToString();
            //getPart("Sehr egoistisch").ToString();
            
        }
    }

    class Faulheit : CharacterType
    {

    }

    class Character
    {
        Egoismus egoismus = new Egoismus();
        Faulheit faulheit = new Faulheit();
        // ...

        public Character()
        {
            //Init

            //Vereerben

            // ...
        }
    }
}
