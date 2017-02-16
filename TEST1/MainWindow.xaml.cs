using LaserDll;
using RobotDLL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace TEST1 {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            main();
            //Testinng();
            map.Show();
            //map.RPoint(1000, 50);

            data = new List<MyPoint>();
        }
        public Viewer _V;
        public Robot _R;
        List<_Point> CurData;
        List<MyPoint> data;

        MapWindow map = new MapWindow();

        public void main() {
            _V = new Viewer(this);
            Laser.Init();

            //CurData = Currection.ReadFromFile("temp2.txt");
            CurData = new List<_Point>();
            //CurData.OrderBy(x => x.X);


            _R = new Robot(6008);
             _R.Start();
            _INDX = -1;
            prevIpoc = "0";



            //Thread thrd = new Thread(new ThreadStart(ShowInfo));
            //thrd.Start();

            Thread thrd_map = new Thread(new ThreadStart(ShowInfo3));
            thrd_map.Start();
            Thread thrd_cur = new Thread(new ThreadStart(DoCur4));
            thrd_cur.Start();


            #region
            //double[] X;// = new double[1];
            //double[] Z;// = new double[1];                    
            //Laser.GetProfile(out X, out Z);
            //List<MyPoint> data = Calculate.ZeroZ(X, Z);


            //Calculate.FindPointWithAngle(data);



            //System.Xml.XmlDocument SendXML = new System.Xml.XmlDocument();  // XmlDocument pattern
            //SendXML.PreserveWhitespace = true;
            //SendXML.Load("tt.xml");

            //var a = MyXML.GetValue(SendXML.InnerText, new string[] { "RKorr", "X" });
            // MyXML.GetValue(SendXML);
            // MessageBox.Show(a.ToString());
            #endregion
        }


        public void DoCur() {
            while (true) {                         
                if (_R.Recive_data != "" && _R.Recive_data != null) {

                    // Dispatcher.Invoke(() => tb_ReciveData.Text = _R.Recive_data);                              

                    double rx = MyXML.GetValues(_R.Recive_data, "X");
                    double ry = MyXML.GetValues(_R.Recive_data, "Y");
                    double rz = MyXML.GetValues(_R.Recive_data, "Z");
                    Dispatcher.Invoke(() => map.RPoint(rx, ry));

                    int index = 0;

                    double curY = 0;
                    double curZ = 0;

                    //шаг - максимальная корекция за один пакет
                    double step = 0.15;

                    if (CurData.Count > 0) {
                        while (index < CurData.Count && rx > CurData[index].X) {
                            index++;
                        }
                        if (index < CurData.Count) {
                            curY = CurData[index].Y;
                            curZ = CurData[index].Z;
                        } 
                    }

                   //Dispatcher.Invoke(() => tb_ReciveData.Text = _R.Recive_data);

                   

                   // _R.CurY = 10;
                    _R.CurZ = 0.01;
                    _R.CurX = 0.01;

                    //if (_INDX < 1000) {
                    //    _R.CurX = 1;
                    //    _R.CurZ = 1;
                    //    _INDX++;
                    //    Console.WriteLine($"corection!!! {_INDX}");
                    //}
                    _R.isCur = true;
                }
            }
           
        }

        public void DoCur2() {
            while (true) {
                if (_R.Recive_data != "" && _R.Recive_data != null) {

                    // Dispatcher.Invoke(() => tb_ReciveData.Text = _R.Recive_data);                              

                    double rx = MyXML.GetValues(_R.Recive_data, "X");
                    double ry = MyXML.GetValues(_R.Recive_data, "Y");
                    double rz = MyXML.GetValues(_R.Recive_data, "Z");
                    Dispatcher.Invoke(() => map.RPoint(rx, ry));

                    int index = 0;

                    double curY = 0;
                    double curZ = 0;

                    if (CurData.Count > 0) {
                        while (index < CurData.Count && rx > CurData[index].X) {
                            index++;
                        }
                        if (index < CurData.Count) {
                                curY = CurData[index].Y - ry;
                                curZ = CurData[index].Z - rz;
                        } else {
                            //_R.serveur.Close();
                           // Console.WriteLine($"Server Close");
                        }
                    }

                    double step = 0.02;
                    if (Math.Abs(curY) > step) {
                        if (curY * -1 < 0) {
                            curY = step;
                        } else {
                            curY = -step;
                        }
                    }

                    if (Math.Abs(curZ) > step) {
                        if (curZ * -1 < 0) {
                            curZ = step;
                        } else {
                            curZ = -step;
                        }
                    }
                    _R.CurY = curY;
                    _R.CurZ = curZ;
                    // _R.CurX = 0.2;                  
                    _R.isCur = true;
                }
            }

        }

        public void DoCur3() {
            int milimeter = 0;
            int prevMil = 0;
            List<_Point> CurData1 = new List<_Point>();
            while (true) {
                if (_R.Recive_data != "" && _R.Recive_data != null) {

                    // Dispatcher.Invoke(() => tb_ReciveData.Text = _R.Recive_data);                              

                    double rx = MyXML.GetValues(_R.Recive_data, "X");
                    double ry = MyXML.GetValues(_R.Recive_data, "Y");
                    double rz = MyXML.GetValues(_R.Recive_data, "Z");
                    Dispatcher.Invoke(() => map.RPoint(rx, ry));

                    int index = 0;

                    double curY = 0;
                    double curZ = 0;
                   


                    if (CurData.Count > 0) {

                        milimeter = Convert.ToInt32(CurData[0].X);
                        CurData1.Add(CurData[0]);
                        for (int i = 0; i < CurData.Count; i++) {
                            if (milimeter != Convert.ToInt32(CurData[i].X)) {
                                CurData1.Add(CurData[i]);
                                milimeter = Convert.ToInt32(CurData[i].X);
                            }
                        }


                        while (index < CurData1.Count && rx > CurData1[index].X) {
                            index++;
                        }


                        //
                        //

                        if (index < CurData1.Count) {
                            if (index != _INDX) {
                                curY = CurData1[index].Y - ry;
                                curZ = CurData1[index].Z - rz;
                                _INDX = index;


                                double step = 0.1;
                                if (Math.Abs(curY) > step) {
                                    if (curY * -1 < 0) {
                                        curY = step;
                                    } else {
                                        curY = -step;
                                    }
                                }

                                if (Math.Abs(curZ) > step) {
                                    if (curZ * -1 < 0) {
                                        curZ = step / 2;
                                    } else {
                                        curZ = -step / 2;
                                    }
                                }
                            }

                        }
                    }

                    _R.CurY = curY;
                    //_R.CurZ = curZ;
                    // _R.CurX = 0.2;                  
                    _R.isCur = true;
                }
            }

        }

        public void DoCur4() {
            int milimeter = 0;
            int prevMil = 0;
            List<_Point> CurData1 = new List<_Point>();
            while (true) {
                if (_R.Recive_data != "" && _R.Recive_data != null) {

                    // Dispatcher.Invoke(() => tb_ReciveData.Text = _R.Recive_data);                              

                    double rx = MyXML.GetValues(_R.Recive_data, "X");
                    double ry = MyXML.GetValues(_R.Recive_data, "Y");
                    double rz = MyXML.GetValues(_R.Recive_data, "Z");
                    Dispatcher.Invoke(() => map.RPoint(rx, ry));

                    int index = 0;

                    double curY = 0;
                    double curZ = 0;



                    if (CurData.Count > 0) {

                     


                        while (index < CurData.Count && rx > CurData[index].X) {
                            index++;
                        }


                        //
                        //

                        if (index < CurData.Count) {
                            if (index != _INDX) {
                                curY = CurData[index].Y - ry;
                                curZ = CurData[index].Z - rz;
                                _INDX = index;


                                double step = 110.3;
                                if (Math.Abs(curY) > step) {
                                    if (curY * -1 < 0) {
                                        curY = step;
                                    } else {
                                        curY = -step;
                                    }
                                }

                                if (Math.Abs(curZ) > step) {
                                    if (curZ * -1 < 0) {
                                        curZ = step;
                                    } else {
                                        curZ = -step;
                                    }
                                }
                            }

                        }
                    }

                    _R.CurY = curY;
                    _R.CurZ = curZ;
                    // _R.CurX = 0.2;                  
                    _R.isCur = true;
                }
            }

        }


        public void Testinng() {
            double curY = 0.2;
            double curZ = 0.4;

            double step = 0.3;

            if (Math.Abs(curY) > step) {
                if (curY * -1 < 0) {
                    curY = step;
                } else {
                    curY = -step;
                }
            }

            if (Math.Abs(curZ) > step) {
                if (curZ * -1 < 0) {
                    curZ = step;
                } else {
                    curZ = -step;
                }
            }

            Console.WriteLine($"Y = {curY}   Z = {curZ}");
        }
        int _INDX;
        string prevIpoc;
        

        //вывод данных на экран
        public void ShowInfo() {
            while (true) {
                Dispatcher.Invoke(() => {
                    double[] X;// = new double[1];
                    double[] Z;// = new double[1];                    
                    Laser.GetProfile(out X, out Z);
                    List<MyPoint> data = Calculate.ZeroZ(X, Z);
                    _V.SetData(data);
                    _V.ReDraw();
                    MyPoint p = new MyPoint() { X = 0, Z = 0 };

                    if (data.Count != 0) {
                        p = Calculate.FindPointWithAngle(data, 10, 5);
                        //MyPoint p2 = Calculate.FindPointWithAngleRight(data, 10, 5);
                        //_V.DrawPoint(p2, new SolidColorBrush(Colors.Black));
                    }
                    _V.DrawPoint(p);


                    tb_ReciveData.Text = _R.Recive_data;
                    tb_SendData.Text = _R.Send_data;

                    if (_R.Recive_data != "" && _R.Recive_data != null) {
                        _R.RX = MyXML.GetValues(_R.Recive_data, "X");
                        _R.RY = MyXML.GetValues(_R.Recive_data, "Y");
                        _R.RZ = MyXML.GetValues(_R.Recive_data, "Z");
                        _R.RA = MyXML.GetValues(_R.Recive_data, "A");
                        _R.RB = MyXML.GetValues(_R.Recive_data, "B");
                        _R.RC = MyXML.GetValues(_R.Recive_data, "C");
                    }


                    //Console.Clear();
                    //Console.WriteLine(p.Z);                  
                    //_Point pTrans = Transform.Trans(_R.RX, _R.RY, _R.RZ, _R.RA, _R.RB, _R.RC, 0, p.X, p.Z - 350);
                    //Console.WriteLine($"X = {pTrans.X}  Y = {pTrans.Y}  Z = {pTrans.Z}");
                    //if (_R.Ipoc != null && Convert.ToInt64(_R.Ipoc) % 10 == 0 ) {
                    //    //Currection.WriteToFile("temp.txt", pTrans);
                    //    //Console.Clear();
                    //    //Console.WriteLine(_R.Ipoc);
                    //}

                    // Console.WriteLine()

                    #region
                    //data = Calculate.Usred(data);


                    //MyPoint p = Calculate.FindSpad(data);
                    //if (p.Z != 0) {
                    //    MessageBox.Show("dsad");
                    //}

                    //_V.SetData(data);
                    //_V.ReDraw();
                    //_V.DrawPoint(p);




                    //MyPoint p1 = Calculate.FindSpad3(data, 0);
                    //MyPoint p2 = Calculate.FindSpad3(data, 0);
                    //MyPoint p3 = Calculate.FindSpad3(data, 0);
                    //MyPoint p4 = Calculate.FindSpad3(data, 0);
                    //_V.DrawPoint(p1 ,new SolidColorBrush(Colors.Black));
                    //_V.DrawPoint(p2, new SolidColorBrush(Colors.DarkBlue));
                    //_V.DrawPoint(p3, new SolidColorBrush(Colors.Green));
                    //_V.DrawPoint(p4, new SolidColorBrush(Colors.Pink));
                    #endregion
                });
                Thread.Sleep(100);
            }
        }
        //запись маршрута
        public void ShowInfo2() {
            while (true) {

                Stopwatch sw = new Stopwatch();
                sw.Start();


                double[] X;// = new double[1];
                double[] Z;// = new double[1];                    
                Laser.GetProfile(out X, out Z);
                List<MyPoint> data = Calculate.ZeroZ(X, Z);
                MyPoint p = new MyPoint() { X = 0, Z = 0 };

                if (data.Count != 0) {
                    p = Calculate.FindPointWithAngle(data, 30, 5);
                }

                if (p.X != 0) {
                    if (_R.Recive_data != "" && _R.Recive_data != null) {
                        _R.RX = MyXML.GetValues(_R.Recive_data, "X");
                        _R.RY = MyXML.GetValues(_R.Recive_data, "Y");
                        _R.RZ = MyXML.GetValues(_R.Recive_data, "Z");
                        _R.RA = MyXML.GetValues(_R.Recive_data, "A");
                        _R.RB = MyXML.GetValues(_R.Recive_data, "B");
                        _R.RC = MyXML.GetValues(_R.Recive_data, "C");
                        //_Point pTrans = Transform.Trans(_R.RX, _R.RY, _R.RZ, _R.RA, _R.RB, _R.RC, 0, p.X, p.Z - 350);
                        _Point pTrans = Transform.Trans(_R.RX, _R.RY, _R.RZ, _R.RA, _R.RB, _R.RC, 76.97, p.X + 7.51, p.Z - 350 - 5.87);
                        Dispatcher.Invoke(() => map.LPoint(pTrans.X, pTrans.Y));
                        Dispatcher.Invoke(() => map.RPoint(_R.RX, _R.RY));
                    }
                    //_Point pTrans = Transform.Trans(_R.RX, _R.RY, _R.RZ, _R.RA, _R.RB, _R.RC, 0, p.X, p.Z - 350);


                   

                    //if (pTrans.X != 0) {
                    //    Currection.WriteToFile("temp2.txt", pTrans);
                    //}

                }





                //if (_R.Ipoc != null && Convert.ToInt64(_R.Ipoc) % 10 == 0) {

                //    Console.Clear();

                //}

                sw.Stop();
                //Console.WriteLine(sw.ElapsedMilliseconds);
            }
        }
        //относительная
       
        public void ShowInfo3() {

            _Point tempPoint = new _Point();
            bool yes = false;
            while (true) {



                double[] X;// = new double[1];
                double[] Z;// = new double[1];                    
                Laser.GetProfile(out X, out Z);
                List<MyPoint> data = Calculate.ZeroZ(X, Z);





                if (_R.Recive_data != "" && _R.Recive_data != null) {

                    // Dispatcher.Invoke(() => tb_ReciveData.Text = _R.Recive_data);


                    MyPoint p = new MyPoint() { X = 0, Z = 0 };
                    if (data.Count != 0) {
                        p = Calculate.FindPointWithAngle(data, 10, 5);
                    }
                    _R.RX = MyXML.GetValues(_R.Recive_data, "X");
                    _R.RY = MyXML.GetValues(_R.Recive_data, "Y");
                    _R.RZ = MyXML.GetValues(_R.Recive_data, "Z");
                    _R.RA = MyXML.GetValues(_R.Recive_data, "A");
                    _R.RB = MyXML.GetValues(_R.Recive_data, "B");
                    _R.RC = MyXML.GetValues(_R.Recive_data, "C");


                    _Point pTrans = Transform.Trans(_R.RX, _R.RY, _R.RZ, _R.RA, _R.RB, _R.RC, 76.97, p.X + 7.51, p.Z - 350 - 5.87);

                    //if (pTrans.X != 0) {
                    //    if (CurData.Count == 0) {
                    //        CurData.Add(new _Point() {
                    //            X = pTrans.X,
                    //            Y = pTrans.Y,
                    //            Z = pTrans.Z
                    //        });
                    //        Dispatcher.Invoke(() => map.LPoint(pTrans.X, pTrans.Y));
                    //    } else {                          
                    //        double yDif = 2;  // разница по игреку меду соседними точками при записи траэктории
                    //        if (Math.Abs(pTrans.Y - CurData[CurData.Count - 1].Y) < yDif) {
                    //            //Console.WriteLine($"{Math.Abs(pTrans.Y - CurData[CurData.Count - 1].Y)}");
                    //            CurData.Add(new _Point() {
                    //                X = pTrans.X,
                    //                Y = pTrans.Y,
                    //                Z = pTrans.Z
                    //            });
                    //            Dispatcher.Invoke(() => map.LPoint(pTrans.X, pTrans.Y));
                    //        }
                    //    }


                    if (pTrans.X != 0) {
                        if (!yes) {
                            tempPoint = new _Point() {
                                X = pTrans.X,
                                Y = pTrans.Y,
                                Z = pTrans.Z
                            };
                            yes = !yes;
                        } else {
                            double yDif = 5;  // разница по игреку меду соседними точками при записи траэктории
                            if (Math.Abs(pTrans.Y - tempPoint.Y) < yDif && Math.Abs(pTrans.X - tempPoint.X) >= 1) {
                                //Console.WriteLine($"{Math.Abs(pTrans.Y - CurData[CurData.Count - 1].Y)}");
                                _Point temp = Calculate.CalcPoint(tempPoint, pTrans);
                                CurData.Add(temp);
                                Dispatcher.Invoke(() => map.LPoint(temp.X, temp.Y));
                                tempPoint = pTrans;
                                Console.WriteLine($"X = {temp.X}  Y = {temp.Y}  Z = {temp.Z}  ");
                            }
                        }

                        //Dispatcher.Invoke(() => map.LPoint(pTrans.X, pTrans.Y));

                        CurData.OrderBy(x => x.X);
                        if (CurData.Count > 2) {
                            CurData = Calculate.UsredMap(CurData);
                        }
                    }
                    #region
                    //double rx = MyXML.GetValues(_R.Recive_data, "X");
                    //double ry = MyXML.GetValues(_R.Recive_data, "Y");
                    //double rz = MyXML.GetValues(_R.Recive_data, "Z");
                    //Dispatcher.Invoke(() => map.RPoint(rx, ry));


                    //#region

                    ////Console.WriteLine($"X = {rx}  Y = {ry}  Z = {rz}  ");



                    //int index = 0;
                    ////for (int i = 0; i < CurData.Count; i++) {
                    ////    if (CurData[i].X > rx) {
                    ////        index = i;
                    ////        break;
                    ////    }
                    ////}


                    //double curY = 0;
                    //double curZ = 0;

                    ////шаг - максимальная корекция за один пакет
                    //double step = 0.08;

                    //if (CurData.Count > 0) {
                    //    while (index < CurData.Count && rx > CurData[index].X) {
                    //        index++;
                    //    }
                    //    if (index < CurData.Count) {
                    //        curY = CurData[index].Y - ry;
                    //        curZ = CurData[index].Z - rz;
                    //    }
                    //    // Console.WriteLine($"Index = {index}");
                    //}

                    //if (Math.Abs(curY) > step) {
                    //    if (curY * -1 < 0) {
                    //        curY = step;
                    //    } else {
                    //        curY = -step;
                    //    }
                    //}

                    //if (Math.Abs(curZ) > step) {
                    //    if (curZ * -1 < 0) {
                    //        curZ = step/2;
                    //    } else {
                    //        curZ = -step/2;
                    //    }
                    //}

                    //_R.CurY = curY;
                    //_R.CurZ = curZ;
                    //// _R.CurX = 0.2;
                    ////Console.WriteLine($"Y = {_R.CurY }  Z = {_R.CurZ }");

                    //if (_INDX < 100) {
                    //    _R.CurX = 1;
                    //    _INDX++;
                    //}

                    //_R.isCur = true;


                    //#endregion
                    #endregion

                }
            }

        }
        //абсолютная
        public void ShowInfo4() {
            while (true) {
                double[] X;// = new double[1];
                double[] Z;// = new double[1];                    
                Laser.GetProfile(out X, out Z);
                List<MyPoint> data = Calculate.ZeroZ(X, Z);

                MyPoint p = new MyPoint() { X = 0, Z = 0 };
                if (data.Count != 0) {
                    p = Calculate.FindPointWithAngle(data, 10, 5);
                }

                _Point pTrans = Transform.Trans(_R.RX, _R.RY, _R.RZ, _R.RA, _R.RB, _R.RC, 0, p.X, p.Z - 350);


                if (_R.Recive_data != "" && _R.Recive_data != null) {

                    //double rx = MyXML.GetValues(_R.Recive_data, "X");
                    //double ry = MyXML.GetValues(_R.Recive_data, "Y");
                    //double rz = MyXML.GetValues(_R.Recive_data, "Z");


                    _R.RX = MyXML.GetValues(_R.Recive_data, "X");
                    _R.RY = MyXML.GetValues(_R.Recive_data, "Y");
                    _R.RZ = MyXML.GetValues(_R.Recive_data, "Z");
                    _R.RA = MyXML.GetValues(_R.Recive_data, "A");
                    _R.RB = MyXML.GetValues(_R.Recive_data, "B");
                    _R.RC = MyXML.GetValues(_R.Recive_data, "C");

                    _R.CurY = _R.RY;
                    _R.CurZ = _R.RZ;
                    _R.CurX = _R.RX;
                    _R.CurA = _R.RA;
                    _R.CurB = _R.RB;
                    _R.CurC = _R.RC;

                    if (prevIpoc != _R.Ipoc) {
                        _INDX++;
                        prevIpoc = _R.Ipoc;
                    }
                    if (_INDX >= 10) {
                        _INDX = 0;

                        int index = 0;
                        while (_R.RX > CurData[index].X && index < CurData.Count) {
                            index++;
                        }
                        _R.CurY = CurData[index].Y;
                        _R.CurZ = CurData[index].Z;
                        _R.CurX = CurData[index].X;
                    }
                }
            }
            #region
            //_R.RX = MyXML.GetValues(_R.Recive_data, "X");
            //_R.RY = MyXML.GetValues(_R.Recive_data, "Y");
            //_R.RZ = MyXML.GetValues(_R.Recive_data, "Z");
            //_R.RA = MyXML.GetValues(_R.Recive_data, "A");
            //_R.RB = MyXML.GetValues(_R.Recive_data, "B");
            //_R.RC = MyXML.GetValues(_R.Recive_data, "C");
            //CurData.OrderBy(x => x.X);

            //if (_INDX < CurData.Count) {
            //    if (_R.isCur == false) {
            //        Console.WriteLine($"Index = {_INDX }");
            //        _R.CurY = CurData[_INDX].Y;
            //        _R.CurZ = CurData[_INDX].Z;
            //        _R.CurX = CurData[_INDX].X;
            //        _R.CurA = _R.RA;
            //        _R.CurB = _R.RB;
            //        _R.CurC = _R.RC;
            //        _R.isCur = true;
            //        _INDX++;
            //    }
            //}

            //if (_R.isCur == false) {
            //    _R.CurY = _R.RY;
            //    _R.CurZ = _R.RZ;
            //    _R.CurX = _R.RX;
            //    _R.CurA = _R.RA;
            //    _R.CurB = _R.RB;
            //    _R.CurC = _R.RC;
            //    _R.isCur = true;
            //}




            #endregion
        }


        public void ShowInfo3_2() {
            while (true) {


                //double[] X;// = new double[1];
                //double[] Z;// = new double[1];                    
                //Laser.GetProfile(out X, out Z);
                //List<MyPoint> data = Calculate.ZeroZ(X, Z);

                if (_R.Recive_data != "" && _R.Recive_data != null) {

                    double rx = MyXML.GetValues(_R.Recive_data, "X");
                    double ry = MyXML.GetValues(_R.Recive_data, "Y");
                    double rz = MyXML.GetValues(_R.Recive_data, "Z");

                    double step = 0.1;
                    double curY = 0;
                    double curZ = 0;
                    if (_INDX < 50 ) {
                        curY = step;
                        //curZ = -step;
                    } else {
                        curY = -step;
                       // curZ = step;
                    }


                    _R.CurY = curY;
                    _R.CurZ = curZ;
                    _R.CurX = 0.1;
                    //Console.WriteLine($"Y = {_R.CurY }  Z = {_R.CurZ }");
                    if (!_R.isCur) {
                        _R.isCur = true;
                    }
                    _INDX++;
                    if (_INDX > 100) {
                        _INDX = 0;
                    }
                }
            }
        }        


        private void bt_start_listen_Click(object sender, RoutedEventArgs e) {
            _R.Start();
            Console.WriteLine("Start Listen Port");
        }
    }
}
