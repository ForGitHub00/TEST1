using System;
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
using System.Windows.Shapes;

namespace TEST1 {
    /// <summary>
    /// Логика взаимодействия для MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window {
        public MapWindow() {
            InitializeComponent();
        }

        public void LPoint(double x, double y) {
            Ellipse el = new Ellipse() {
                Height = 3,
                Width = 3,
                Fill = new SolidColorBrush(Colors.Red)
            };
            Canvas.SetTop(el, y + 150);
            Canvas.SetLeft(el, x);
            cnv.Children.Add(el);
        }

        public void RPoint(double x, double y) {
            Ellipse el = new Ellipse() {
                Height = 3,
                Width = 3,
                Fill = new SolidColorBrush(Colors.DarkBlue)
            };
            Canvas.SetTop(el,y + 150);
            Canvas.SetLeft(el, x);
            cnv.Children.Add(el);
        }


        public void DrawLine(double x1, double y1, double x2, double y2, SolidColorBrush col) {
            
            for (int i = 0; i < cnv.Children.Count; i++) {
                if (cnv.Children[i].GetType() == typeof(Line)) {
                    cnv.Children.Remove(cnv.Children[i]);
                }
            }

            Line line = new Line() {
                Stroke = col,
                Fill = col,
                X1 = x1,
                X2 = x2,
                Y1 = y1,
                Y2 = y2
            };
            cnv.Children.Add(line);
        }


        private void cnv_MouseRightButtonDown(object sender, MouseButtonEventArgs e) {
            cnv.Children.Clear();
        }
    }
}
