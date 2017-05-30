
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASD
{

    class SweepLine
    {

        /// <summary>
        /// Struktura pomocnicza opisująca zdarzenie
        /// </summary>
        /// <remarks>
        /// Można jej użyć, przerobić, albo w ogóle nie używać i zrobić po swojemu
        /// </remarks>
        struct SweepEvent
        {
            /// <summary>
            /// Współrzędna zdarzenia
            /// </summary>
            public double Coord;

            /// <summary>
            /// Czy zdarzenie oznacza początek odcinka/prostokąta
            /// </summary>
            public bool IsStartingPoint;

            /// <summary>
            /// Indeks odcinka/prodtokąta w odpowiedniej tablicy
            /// </summary>
            public int Idx;

            public SweepEvent(double c, bool sp, int i = -1) { Coord = c; IsStartingPoint = sp; Idx = i; }
        }

        /// <summary>
        /// Funkcja obliczająca długość teoriomnogościowej sumy pionowych odcinków
        /// </summary>
        /// <returns>Długość teoriomnogościowej sumy pionowych odcinków</returns>
        /// <param name="segments">Tablica z odcinkami, których teoriomnogościowej sumy długość należy policzyć</param>
        /// Każdy odcinek opisany jest przez dwa punkty: początkowy i końcowy
        /// </param>
        public double VerticalSegmentsUnionLength(Geometry.Segment[] segments)
        {
            // int - indeks odcinka, bool - poczatek (true), koniec (false)
            List<Tuple<Geometry.Point, bool>> points = new List<Tuple<Geometry.Point, bool>>();
            foreach (var segment in segments)
            {
                points.Add(new Tuple<Geometry.Point, bool>(segment.ps, true));
                points.Add(new Tuple<Geometry.Point, bool>(segment.pe, false));
            }
            points.Sort((x, y) => x.Item1.y.CompareTo(y.Item1.y));
            double unionLength = 0;
            int started = 0;
            int ended = 0;
            Geometry.Point startedPoint = points[0].Item1;
            Geometry.Point endPoint = new Geometry.Point();

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Item2)
                    started++;
                else
                    ended++;
                if (started == ended)
                {
                    endPoint = points[i].Item1;
                    unionLength += Math.Abs(endPoint.y - startedPoint.y);
                    if (i != points.Count - 1)
                        startedPoint = points[i + 1].Item1;
                }
            }
            return unionLength;
        }

        /// <summary>
        /// Funkcja obliczająca pole teoriomnogościowej sumy prostokątów
        /// </summary>
        /// <returns>Pole teoriomnogościowej sumy prostokątów</returns>
        /// <param name="rectangles">Tablica z prostokątami, których teoriomnogościowej sumy pole należy policzyć</param>
        /// Każdy prostokąt opisany jest przez cztery wartości: minimalna współrzędna X, minimalna współrzędna Y, 
        /// maksymalna współrzędna X, maksymalna współrzędna Y.
        /// </param>
        public double RectanglesUnionArea(Geometry.Rectangle[] rectangles)
        {
            return -1;
        }

    }

}
