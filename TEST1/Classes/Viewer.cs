using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TEST1 {
    public struct MyPoint {
        public double X;
        public double Z;
    }

    public class Viewer {
        public MainWindow m;
        public Viewer(MainWindow main) {
            m = main;
        }
     
        public void SetData(double[] X, double[] Z) {
            Data = new List<MyPoint>();
            for (int i = 0; i < X.Length; i++) {
                Data.Add(new MyPoint() {
                    X = X[i],
                    Z = Z[i]
                });
            }
        }
        public void SetData(List<MyPoint> list) {
            Data = list;        
        }

        private List<MyPoint> Data;

        double _x(double value) {
            if (value < 0) {
                return (70 - Math.Abs(value)) * 3;
            }
            return (Math.Abs(value) + 70) * 3;
        }
        double _z(double value) {
            return (Math.Abs(value) - 120) * 3;
        }
        double _ix(double value) {
            return value / 3 - 70;
        }
        double _iz(double value) {
            return value / 3 + 120;
        }


        private double H_Cnv = 810;
        private double W_Cnv = 450;
        public void DrawCells() {

            double stepX = W_Cnv / 15;
            double stepZ = H_Cnv / 27;
            for (int i = 1; i < 27; i++) {
                Line line = new Line();
                line.Stroke = SystemColors.WindowFrameBrush;
                line.X1 = 0;
                line.Y1 = i * stepZ;
                line.X2 = W_Cnv;
                line.Y2 = i * stepZ;
                line.StrokeThickness = 1;
                line.StrokeDashArray = new DoubleCollection() { 4, 6 };
                m.Cnv.Children.Add(line);

                TextBlock t = new TextBlock() {
                    Text = $"{i * 10 + 120}",
                };
                Canvas.SetTop(t, i * stepX - 10);
                m.Cnv_Left.Children.Add(t);


            }
            for (int i = 1; i < 15; i++) {
                Line line = new Line();
                line.Stroke = SystemColors.WindowFrameBrush;
                line.X1 = i * stepX;
                line.Y1 = 0;
                line.X2 = i * stepX;
                line.Y2 = H_Cnv;
                line.StrokeDashArray = new DoubleCollection() { 4, 6 };
                m.Cnv.Children.Add(line);


                TextBlock t = new TextBlock() {
                    Text = $"{i * 10 - 70}",
                };
                Canvas.SetLeft(t, i * stepX - 10 + i);
                m.Cnv_Bot.Children.Add(t);
            }


            Rectangle rec = new Rectangle() {
                Height = 100,
                Width = 1000,
                Fill = Brushes.Red
            };
        }
        public void ReDraw() {
            m.Cnv.Children.Clear();
            m.Cnv_Bot.Children.Clear();
            m.Cnv_Left.Children.Clear();         
            Data = Data.Where(x => x.Z != 0).ToList();

            DrawCells();
            for (int i = 0; i < Data.Count; i++) {
                Ellipse el = new Ellipse() {
                    Width = 2,
                    Height = 2,
                    Fill = Brushes.Red
                };
                Canvas.SetLeft(el, _x(Data[i].X));
                Canvas.SetTop(el, _z(Data[i].Z));
                m.Cnv.Children.Add(el);
            }
        }
        public void DrawPoint(MyPoint p) {
            Ellipse el = new Ellipse() {
                Width = 5,
                Height = 5,
                Fill = Brushes.DarkBlue
            };
            Canvas.SetLeft(el, _x(p.X));
            Canvas.SetTop(el, _z(p.Z));
            m.Cnv.Children.Add(el);
        }
        public void DrawPoint(MyPoint p, SolidColorBrush c) {
            Ellipse el = new Ellipse() {
                Width = 7,
                Height = 7,
                Fill = c
            };
            Canvas.SetLeft(el, _x(p.X));
            Canvas.SetTop(el, _z(p.Z));
            m.Cnv.Children.Add(el);
        }

    }

}
