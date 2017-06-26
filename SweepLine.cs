
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
            // int - indeks odcinka, bool - lewy bok (true), pawy bok (false)
            List<Tuple<Geometry.Segment, bool>> segments = new List<Tuple<Geometry.Segment, bool>>();
            Dictionary<Geometry.Segment, Geometry.Segment> Dict = new Dictionary<Geometry.Segment, Geometry.Segment>();
            foreach (Geometry.Rectangle rectange in rectangles)
            {
                Geometry.Segment s1 = new Geometry.Segment(new Geometry.Point(rectange.MinX, rectange.MinY),
                                                           new Geometry.Point(rectange.MinX, rectange.MaxY));
                Geometry.Segment s2 = new Geometry.Segment(new Geometry.Point(rectange.MaxX, rectange.MinY),
                                                           new Geometry.Point(rectange.MaxX, rectange.MaxY));
                segments.Add(new Tuple<Geometry.Segment, bool>(s1, true));
                segments.Add(new Tuple<Geometry.Segment, bool>(s2, false));
                //if (!Dict.ContainsKey(s2))
                    Dict.Add(s2, s1);
            }
            segments.Sort((s1, s2) => s1.Item1.ps.x.CompareTo(s2.Item1.ps.x));
            List<Geometry.Segment> ptr = new List<Geometry.Segment>();
            ptr.Add(segments[0].Item1);
            double rectanglesUnionArea = 0;
            double x1 = segments[0].Item1.ps.x;
            for (int i = 1; i < segments.Count; i++)
            {
                double x2 = segments[i].Item1.ps.x;
                double D=0;
                if (ptr.Count > 0)
                {
                    D = VerticalSegmentsUnionLength(ptr.ToArray());
                    rectanglesUnionArea += Math.Abs(x2 - x1) * D;
                    x1 = x2;
                }
                if (segments[i].Item2)
                {
                    x1 = segments[i].Item1.ps.x;
                    ptr.Add(segments[i].Item1);
                }
                else
                {
                    //int nrTmp = dict2[segments[i].Item1];
                    //Geometry.Segment tmpSegment = dict[nrTmp];
                    Geometry.Segment tmpSegment = Dict[segments[i].Item1];
                    ptr.Remove(tmpSegment);
                }
            }
            return rectanglesUnionArea;
        }

    }

}
