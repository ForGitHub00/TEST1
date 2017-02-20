using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TEST1 {
    public class Robot {

        public int Port { get; set; }
        public string Ipoc { get; set; }
        public string Recive_data { get; set; }
        public string Send_data { get; set; }

        public double CurX { get; set; }
        public double CurY { get; set; }
        public double CurZ { get; set; }
        public double CurA { get; set; }
        public double CurB { get; set; }
        public double CurC { get; set; }
        public bool isCur { get; set; }

        public double RX { get; set; }
        public double RY { get; set; }
        public double RZ { get; set; }
        public double RA { get; set; }
        public double RB { get; set; }
        public double RC { get; set; }

        public UdpClient serveur;
        private MainWindow mW;

        public Robot() {
            Port = 6008;
        }
        public Robot(int port, MainWindow m) {
            Port = port;
            mW = m;
            //map = ma;
            CurData = new List<_Point>();
        }
        public void Start() {
            CurX = 0;
            CurY = 0;
            CurZ = 0;
            CurA = 0;
            CurB = 0;
            CurC = 0;
            isCur = false;

            RX = 864.8;
            RY = 89.3;
            RZ = 958.1;
            RA = 7.6;
            RB = -0.6;
            RC = 1.6;
                 


            anyfunction();
        }


        public List<_Point> CurData { get; set; }
        private void Cor(double xDef, string str) {
            // Dispatcher.Invoke(() => tb_ReciveData.Text = _R.Recive_data);                                             


            //double rx = MyXML.GetValuesPA(str, "x");
            //double ry = MyXML.GetValuesPA(str, "y");
            //double rz = MyXML.GetValuesPA(str, "z");


            double rx = MyXML.GetValues(str, "X");
            double ry = MyXML.GetValues(str, "Y");
            double rz = MyXML.GetValues(str, "Z");


            mW.Dispatcher.Invoke(() => mW.map.RPoint(rx, ry));

            int index = 0;
            if (CurData.Count > 0) {
                while (index < CurData.Count && rx > CurData[index].X) {
                    index++;
                }
                if (index < CurData.Count) {

                _Point p = Calculate.CalcPoint(new _Point() { X = rx, Y = ry, Z = rz }, 
                        new _Point() {X = CurData[index].X , Y = CurData[index].Y, Z = CurData[index].Z }, rx + xDef);
                    //Console.WriteLine($"Y = {p.Y - RY}  Z = {p.Z - RZ}  ");
                    if (p.Y == Double.NaN ) {
                        //Console.WriteLine($"Y = {p.Y - ry}  Z = {p.Z - rz}  ");
                    }
                    //Console.WriteLine($"dx = {xDef}");


                    CurY = (p.Y - ry) / 1;
                    CurZ = (p.Z - rz) / 1;

                    //CurY = (CurData[index].Y - ry) / 100;
                    //CurZ = (CurData[index].Z - rz) / 100;

                    if (CurY > 1) {
                        Console.WriteLine($"dx = {xDef}  curY = {CurY}  RY = {ry}   curDataY = { CurData[index].Y}  pY  = {p.Y}");
                    }
                }
            }
        }


        private void anyfunction() {
            // starting communication by separate process
            System.Threading.Thread secondThread;
            secondThread = new System.Threading.Thread(new System.Threading.ThreadStart(StartListening));
            secondThread.Start();
        }

        // second thread
        private void StartListening() {
            System.Xml.XmlDocument SendXML = new System.Xml.XmlDocument();  // XmlDocument pattern
            SendXML.PreserveWhitespace = true;
            SendXML.Load("ExternalData.xml");

            serveur = new UdpClient(Port);

            double prevX = 0;
            double x = 0;
            //double prevY = 0;
            //double y = 0;
            //double prevZ = 0;
            //double z = 0;
            //long prevIpoc = 0;
            //long ipoc;
            //List<double> mas = new List<double>();


            try {
                while (true) {
                    IPEndPoint client = null;
                    byte[] data = serveur.Receive(ref client);
                    //Console.WriteLine("Donnees recues en provenance de {0}:{1}.", client.Address, client.Port);
                    string message = Encoding.ASCII.GetString(data);
                    string strReceive = message;
                    Recive_data = message;


                  


                    #region
                    //вывод скорости по X
                    //if (prevX == 0) {
                    //    prevX = MyXML.GetValues(strReceive, "X");
                    //    prevY = MyXML.GetValues(strReceive, "Y");
                    //    prevZ = MyXML.GetValues(strReceive, "Z");
                    //} else {
                    //    x = MyXML.GetValues(strReceive, "X");
                    //    y = MyXML.GetValues(strReceive, "Y");
                    //    z = MyXML.GetValues(strReceive, "Z");
                    //    //Console.Clear();
                    //    //double speedX = ((x - prevX) / 1000) / 0.012;
                    //    //double speedY = ((y - prevY) / 1000) / 0.012;
                    //    //double speedZ = ((z - prevZ) / 1000) / 0.012;
                    //    //Console.WriteLine($"Speed = {speedX + speedY + speedZ} m/s");


                    //    double speed = Math.Sqrt((x - prevX) * (x - prevX) + (y - prevY) * (y - prevY) + (z - prevZ) * (z - prevZ)) / 0.012;
                    //    // Console.WriteLine($"Speed = {speed / 1000} m/s");
                    //    mas.Add(speed / 1000);
                    //    Console.WriteLine($"Speed = {mas.Average()} m/s");
                    //    prevX = x;
                    //    prevY = y;
                    //    prevZ = z;
                    //}

                    //if (prevIpoc == 0) {
                    //    prevIpoc = Convert.ToInt64(Ipoc);
                    //} else {
                    //    ipoc = Convert.ToInt64(Ipoc);
                    //    if (ipoc - prevIpoc != 12) {
                    //        Console.WriteLine($"Ipoc Error");
                    //    }
                    //    prevIpoc = ipoc;
                    //}
                    #endregion

                    if ((strReceive.LastIndexOf("</Rob>")) == -1) {
                        continue;
                    } else {

                        string strSend;
                        strSend = SendXML.InnerXml;
                        strSend = mirrorIPOC(strReceive, strSend);

                        #region
                        //if (isCur || !isCur) {
                        //    strSend = SetCur(strSend);
                        //    _Point p = new _Point() {
                        //        X = CurX,
                        //        Y = CurY,
                        //        Z = CurZ
                        //    };
                        //    Currection.WriteToFile("ipoc.txt", p, Ipoc);
                        //    CurX = 0;
                        //    CurY = 0;
                        //    CurZ = 0;
                        //    CurA = 0;
                        //    CurB = 0;
                        //    CurC = 0;
                        //    isCur = false;
                        //}

                        //if (isCur) {
                        //    CurX = MyXML.GetValues(strReceive, "X");
                        //    CurY = MyXML.GetValues(strReceive, "Y");
                        //    CurZ = MyXML.GetValues(strReceive, "Z");
                        //    CurA = MyXML.GetValues(strReceive, "A");
                        //    CurB = MyXML.GetValues(strReceive, "B");
                        //    CurC = MyXML.GetValues(strReceive, "C");
                        //    Console.WriteLine($"corection!!! {CurY}");
                        //    isCur = false;
                        //} 


                        /*
                        CurX = RX;
                        CurY = RY;
                        CurZ = RZ;
                        CurA = RA;
                        CurB = RB;
                        CurC = RC;
                        strSend = SetCur(strSend);

                        _Point p = new _Point() {
                            X = CurX,
                            Y = CurY,
                            Z = CurZ
                        };
                        Currection.WriteToFile("ipoc.txt", p, Ipoc);
                        */
                        #endregion


                        //Currection.WriteToFile("PosAct.txt", new double[] { MyXML.GetValues(strReceive, "X"), MyXML.GetValuesPA(strReceive, "x") / 1000});

                        CurX = 0;
                        CurY = 0;
                        CurZ = 0;
                        CurA = 0;
                        CurB = 0;
                        CurC = 0;

                        if (prevX == 0) {
                            prevX = MyXML.GetValues(strReceive, "X");
                            //prevX = MyXML.GetValues(strReceive, "X");
                        } else { 
                            x = MyXML.GetValues(strReceive, "X");
                            //x = MyXML.GetValues(strReceive, "X");
                            if (x - prevX > 0) {
                                Cor(x - prevX, strReceive);
                                //if (x - prevX < 0.01) {
                                //    Console.WriteLine($"CUR!  {x - prevX}");
                                //}
                            }


                            prevX = x;
                        }


                        strSend = SetCur(strSend);

                        Send_data = strSend;

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(strSend);
                        serveur.Send(msg, msg.Length, client);
                    }
                    strReceive = null;
                }
            } catch (Exception ex) {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
        }

        private string mirrorIPOC(string receive, string send) {

            //CurX = MyXML.GetValues(receive, "X");
            //CurY = MyXML.GetValues(receive, "Y");
            //CurZ = MyXML.GetValues(receive, "Z");
            //CurA = MyXML.GetValues(receive, "A");
            //CurB = MyXML.GetValues(receive, "B");
            //CurC = MyXML.GetValues(receive, "C");
            //isCur = true;

            // separate IPO counter as string
            int startdummy = receive.IndexOf("<IPOC>") + 6;
            int stopdummy = receive.IndexOf("</IPOC>");
            string Ipocount = receive.Substring(startdummy, stopdummy - startdummy);

            // find the insert position		
            startdummy = send.IndexOf("<IPOC>") + 6;
            stopdummy = send.IndexOf("</IPOC>");

            // remove the old value an insert the actualy value
            send = send.Remove(startdummy, stopdummy - startdummy);
            send = send.Insert(startdummy, Ipocount);

            Ipoc = Ipocount;
            //Console.WriteLine(Ipocount);

            // send back the string
            return send;
        }

        private string SetCur(string send) {
            int startdummy = send.IndexOf("X=") + 3;
            int stopdummy = send.IndexOf("X=") + 9;
            send = send.Remove(startdummy, stopdummy - startdummy);
            send = send.Insert(startdummy, CurX.ToString("0.####").Replace(",", "."));

            startdummy = send.IndexOf("Y=") + 3;
            stopdummy = send.IndexOf("Y=") + 9;
            send = send.Remove(startdummy, stopdummy - startdummy);
            send = send.Insert(startdummy, CurY.ToString("0.####").Replace(",", "."));

            startdummy = send.IndexOf("Z=") + 3;
            stopdummy = send.IndexOf("Z=") + 9;
            send = send.Remove(startdummy, stopdummy - startdummy);
            send = send.Insert(startdummy, CurZ.ToString("0.####").Replace(",", "."));

            startdummy = send.IndexOf("A=") + 3;
            stopdummy = send.IndexOf("A=") + 9;
            send = send.Remove(startdummy, stopdummy - startdummy);
            send = send.Insert(startdummy, CurA.ToString("0.####").Replace(",", "."));

            startdummy = send.IndexOf("B=") + 3;
            stopdummy = send.IndexOf("B=") + 9;
            send = send.Remove(startdummy, stopdummy - startdummy);
            send = send.Insert(startdummy, CurB.ToString("0.####").Replace(",", "."));

            startdummy = send.IndexOf("C=") + 3;
            stopdummy = send.IndexOf("C=") + 9;
            send = send.Remove(startdummy, stopdummy - startdummy);
            send = send.Insert(startdummy, CurC.ToString("0.####").Replace(",", "."));
            return send;
        }


    }
}
