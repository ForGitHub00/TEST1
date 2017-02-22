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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Calibration {
    public struct Profile {
        public double[] X;
        public double[] Z;
    }
    public struct Index {
        public int Left;
        public int Right;
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            xmlList = new List<XDocument>();
            profList = new List<Profile>();
            _xml = new List<string>();
            Laser.Init();
            _R = new RobotCalib(59152, this);

            //double[] X;// = new double[1];
            //double[] Z;// = new double[1];                    
            //Laser.GetProfile(out X, out Z);

        }
        public List<XDocument> xmlList;
        public List<string> _xml;
        public List<Profile> profList;
        RobotCalib _R;

        public Index findPoints(double[] qvdValueX, double[] qvdValueZ) {
            Index result;
            //result.right = result.left = qvdValueX.size() / 2;
            result.Right = -1;
            result.Left = -1;

            int sizeXZ = qvdValueX.Count();

            int prevLeftIndex = 0;
            int prevRightIndex = sizeXZ - 1;


            while (qvdValueZ[prevLeftIndex] == 0) {
                prevLeftIndex++;
                if (prevLeftIndex == sizeXZ) {
                    return result;
                }
            }


            while (qvdValueZ[prevRightIndex] == 0) {
                prevRightIndex--;
                if (prevRightIndex == -1) {
                    return result;
                }
            }


            //Разница между высотой точек которая будет считаться перепадом
            const double STEP = 0.2;   //mm

            //L
            for (int i = prevLeftIndex; i < (sizeXZ); i++) {
                if (qvdValueZ[i] != 0) {
                    if (Math.Abs(Math.Abs(qvdValueZ[i]) - Math.Abs(qvdValueZ[prevLeftIndex])) > STEP) {
                        if (qvdValueZ[i] > qvdValueZ[prevLeftIndex]) {
                            prevLeftIndex = i;
                        };
                        break;
                    } else {
                        prevLeftIndex = i;
                    }
                }
            }

            //R
            for (int i = prevRightIndex; i >= 0; i--) {
                if (qvdValueZ[i] != 0) {
                    if (Math.Abs(Math.Abs(qvdValueZ[i]) - Math.Abs(qvdValueZ[prevRightIndex])) > STEP) {
                        if (qvdValueZ[i] > qvdValueZ[prevRightIndex]) {
                            prevRightIndex = i;
                        };
                        break;
                    } else {
                        prevRightIndex = i;
                    }
                }
            }

            if (prevLeftIndex == sizeXZ - 1 || prevRightIndex == 0) {
                return result;
            }

            result.Left = prevLeftIndex;
            result.Right = prevRightIndex;

            return result;
        }

        public void IsGetXML(string str) {
            double[] X;// = new double[1];
            double[] Z;// = new double[1];                    
            Laser.GetProfile(out X, out Z);
            profList.Add(new Profile() { X = X, Z = Z });
            xmlList.Add(XDocument.Parse(str));
            _xml.Add(str);
            lb_packets_count.Content = _xml.Count.ToString();
        }




        public void Calib() {
            //int step = 1;
            double x1 = 0;
            double x2 = 0;
            double x3 = 0;
            double x4;

            double dy = 0;
            double dx;
            double dz;

            double d2 = 0;
            double d3;

            for (int i = 1; i <= 4; i++) {
                if (i == 1) {
                    Index points = findPoints(profList[i - 1].X, profList[i - 1].Z);
                    x1 = GetValues(_xml[i - 1]);
                    dy = (profList[i - 1].X[points.Left] + profList[i - 1].Z[points.Right]) / 2;
                    tb_dy.Text = dy.ToString();
                } else if (i == 2) {
                    Index points = findPoints(profList[i - 1].X, profList[i - 1].Z);
                    x2 = GetValues(_xml[i - 1]);                  
                    d2 = profList[i - 1].X[points.Left];
                } else if (i == 3) {
                    Index points = findPoints(profList[i - 1].X, profList[i - 1].Z);
                    x3 = GetValues(_xml[i - 1]);
                    d3 = profList[i - 1].X[points.Left];
                    dx = ((d3 - dy) * (x2 - x3)) / ((d2 - dy) - (d3 - dy)) + (x1 - x3);
                    dz = (profList[i - 1].Z[points.Left] + profList[i - 1].Z[points.Right]) / 2;
                    tb_dx.Text = dx.ToString();
                    tb_dz.Text = dz.ToString();
                } else {
                    x4 = GetValues(_xml[i - 1]);
                }

            }

        }

        public static double GetValues(string strXML) {
            XDocument xdoc = XDocument.Parse(strXML);
            foreach (XElement phoneElement in xdoc.Element("Robot").Elements("Position")) {
                XAttribute nameAttribute = phoneElement.Attribute("X");
                if (nameAttribute != null) {
                    return Convert.ToDouble(nameAttribute.Value.Replace('.', ','));
                }
            }
            return 0;
        }

        private void bt_calib_Click(object sender, RoutedEventArgs e) {
            Calib();
        }


    }
}
