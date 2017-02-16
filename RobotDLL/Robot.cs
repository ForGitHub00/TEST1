using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace RobotDLL
{
    public struct _Point {
        public double X;
        public double Y;
        public double Z;
    }
    public class Robot
    {

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

        public Robot() {
            Port = 6008;
        }
        public Robot(int port) {
            Port = port;
        }
        public void Start() {
            CurX = 0;
            CurY = 0;
            CurZ = 0;
            CurA = 0;
            CurB = 0;
            CurC = 0;
            isCur = false;

            //RX = 864.8;
            //RY = 89.3;
            //RZ = 958.1;
            //RA = 7.6;
            //RB = -0.6;
            //RC = 1.6;

            anyfunction();
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

            try {
                while (true) {
                    IPEndPoint client = null;
                    //On ecoute jusqu'a recevoir un message.
                    byte[] data = serveur.Receive(ref client);
                    //Console.WriteLine("Donnees recues en provenance de {0}:{1}.", client.Address, client.Port);

                    //Decryptage et affichage du message.
                    string message = Encoding.ASCII.GetString(data);

                    // wait for data and receive bytes
                    //Console.Clear();
                    //Console.WriteLine(message);

                    // convert bytes to string
                    string strReceive = message;
                    Recive_data = message;



                    // take a look to the end of data
                    if ((strReceive.LastIndexOf("</Rob>")) == -1) {
                        continue;
                    } else {

                        string strSend;
                        // mirror the IPO counter you received yet                        
                        strSend = SendXML.InnerXml;
                        strSend = mirrorIPOC(strReceive, strSend);

                        if (isCur || !isCur) {
                            strSend = SetCur(strSend);
                            _Point p = new _Point() {
                                X = CurX,
                                Y = CurY,
                                Z = CurZ
                            };
                            //Currection.WriteToFile("ipoc.txt", p, Ipoc);
                            CurX = 0;
                            CurY = 0;
                            CurZ = 0;
                            CurA = 0;
                            CurB = 0;
                            CurC = 0;
                            isCur = false;
                        }

                        #region
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

                        //----------------------

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

                        Send_data = strSend;

                        //send data as requested 
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(strSend);
                        serveur.Send(msg, msg.Length, client);
                    }
                    strReceive = null;
                }
            } catch (Exception ex) {
                Console.WriteLine("Возникло исключение: " + ex.ToString() + "\n  " + ex.Message);
            }
        }

        // send immediately incoming IPO counter to have a timestamp
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
