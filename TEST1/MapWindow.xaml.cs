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

        private void cnv_MouseRightButtonDown(object sender, MouseButtonEventArgs e) {
            cnv.Children.Clear();
        }
    }
}
