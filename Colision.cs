﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;
using System.Globalization;

namespace Asteroids_Rebirth
{
    class Colision
    {
        public Polygon Polygon;
        public PointCollection colisionpoints;
        public double centerX;
        public double centerY;
        public double positionX;
        public double positionY;
        public Colision(double centerx, double centery, double positionx, double positiony, double size, string path)
        {
            positionX = positionx;
            positionY = positiony;
            centerX = centerx + positionX;
            centerY = centery + positionY;
            SolidColorBrush yellowBrush = new SolidColorBrush();
            yellowBrush.Color = Colors.Yellow;
            SolidColorBrush blackBrush = new SolidColorBrush();
            blackBrush.Color = Colors.Black;
            Polygon = new Polygon();
            Polygon.Stroke = blackBrush;
            Polygon.Fill = yellowBrush;
            Polygon.StrokeThickness = 0.2;
            colisionpoints = new PointCollection();
            StreamReader re = new StreamReader(path);
            while (re.Peek() > -1)
            {
                double x = double.Parse(re.ReadLine(), CultureInfo.InvariantCulture) * size / 20.0;
                x += positionX;
                double y = double.Parse(re.ReadLine(), CultureInfo.InvariantCulture) * size / 20.0;
                y += positionY;
                System.Windows.Point point = new System.Windows.Point(x, y);

                colisionpoints.Add(point);
            }
            re.Close();
            Polygon.Points = colisionpoints;
        }
        public void move(double delX, double delY)
        {
            centerX += delX;
            centerY += delY;
            positionX += delX;
            positionY += delY;
            for (int i = 0; i < colisionpoints.Count; i++)
            {
                colisionpoints[i] = new System.Windows.Point(colisionpoints[i].X + delX, colisionpoints[i].Y + delY);
            }

        }
        public void rotate(double angle)
        {

            double cs = Math.Cos(angle);
            double sn = Math.Sin(angle);
            for (int i = 0; i < colisionpoints.Count; i++)
            {
                double xi = (colisionpoints[i].X - centerX) * cs - sn * (colisionpoints[i].Y - centerY);
                double yi = (colisionpoints[i].X - centerX) * sn + (colisionpoints[i].Y - centerY) * cs;
                colisionpoints[i] = new System.Windows.Point(xi + centerX, yi + centerY);
            }
        }
    }
}
