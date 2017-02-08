using AntMe.Deutsch;
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
            Object,
            Insekt
        }

        internal CoreCoordinate ort;
        internal int richtung;
        internal Spielobjekt Objekt;
        internal Insekt Insekt;
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

        public Marker(MarkerType markerType, Spielobjekt objekt)
        {
            this.markerType = markerType;
            this.markerInformation = MarkerInformationType.Object;
            this.Objekt = objekt;
        }

        public Marker(MarkerType markerType, Insekt insect)
        {
            this.markerType = markerType;
            this.markerInformation = MarkerInformationType.Insekt;
            this.Insekt = insect;
        }
    }

    class Markers
    {
        public static List<Marker> MarkerList = new List<Marker>();

        private static int markerCounter = 0;

        /// <summary>
        /// Fügt der Datenbank einen neuen Marker hinzu
        /// </summary>
        /// <param name="marker">new Marker</param>
        /// <returns>Marker-ID, für SprüheMarkierung-Funktion</returns>
        public static int Add(Marker marker)
        {
            marker.id = markerCounter++;
            MarkerList.Add(marker);

            return marker.id;
        }

        /// <summary>
        /// Ruft einen Marker mit der ID aus der empfangenen Marker-Information ab.
        /// </summary>
        /// <param name="id">Die empfangene Marker-Information</param>
        /// <returns>Marker</returns>
        public static Marker Get(int id)
        {
            return MarkerList[id];
        }
    }
}
