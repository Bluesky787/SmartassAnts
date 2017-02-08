using AntMe.Simulation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntMe.SmartassAnts
{
    class Marker
    {
        public enum MarkerType
        {
            Hilfe,          //wenn Ameise angegriffen wird
            HilfeAmeise,    //wenn Ameise eine andere Ameise angreifen möchte
            HilfeObst,      //wenn Ameise Obst sammeln möchte
            HilfeWanze,     //wenn Ameise Wanze angreifen möchte
            Obst,           //wenn Ameise Obst sieht
            Zucker,         //wenn Ameise Zucker sieht
        }

        public enum MarkerInformationType
        {
            Ort,
            Richtung,
        }

        internal CoreCoordinate ort;
        internal int richtung;
        internal MarkerInformationType markerInformation;
        internal MarkerType markerType;
        internal int id;

        public Marker(MarkerType markerType, CoreCoordinate ort)
        {
            this.markerInformation = MarkerInformationType.Ort;
            this.ort = ort;
            this.markerType = markerType;
        }

        public Marker(MarkerType markerType, int richtung)
        {
            this.markerType = markerType;
            this.markerInformation = MarkerInformationType.Richtung;
            this.richtung = richtung;
        }
    }

    class Markers
    {
        public List<Marker> MarkerList = new List<Marker>();

        private int markerCounter = 0;

        public void Add(Marker marker)
        {
            marker.id = markerCounter++;
            MarkerList.Add(marker);
        }
    }
}
