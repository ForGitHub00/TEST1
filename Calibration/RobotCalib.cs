using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calibration {
    public class RobotCalib {

        private int port;
        private MainWindow mw;
        public RobotCalib(int port, MainWindow m) {
            this.port = port;
            mw = m;
            anyfunction();
        }

        private  void anyfunction() {
            // starting communication by separate process
            System.Threading.Thread secondThread;
            secondThread = new System.Threading.Thread(new System.Threading.ThreadStart(StartListening));
            secondThread.Start();
        }

        // second thread
        private  void StartListening() {
            uint Port = (uint)port;                           // port number TCP/IP
            uint AddressListIdx = 1;                    // network card index			
            System.Xml.XmlDocument SendXML = new System.Xml.XmlDocument();  // XmlDocument pattern
            System.Net.Sockets.Socket listener;         // create system socket
            System.Net.Sockets.Socket handler;          // create system socket

            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the  socket.
            // Dns.GetHostName returns the name of the 
            // host running the application.
            System.Net.IPHostEntry ipHostInfo = System.Net.Dns.Resolve(System.Net.Dns.GetHostName());
            System.Net.IPAddress ipAddress = ipHostInfo.AddressList[AddressListIdx];
            System.Net.IPEndPoint localEndPoint = new System.Net.IPEndPoint(ipAddress, (int)Port);

            // Create a TCP/IP socket.
            listener = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);

            // open Socket and listen on network
            listener.Bind(localEndPoint);
            listener.Listen(1);

            // Program is suspended while waiting for an incoming connection.
            // bind the first request
            handler = listener.Accept();

            // no connections are income 
            listener.Close();

            // string members for incoming and outgoing data
            String strReceive = null;
          
            while (true) {
                // wait for data and receive bytes
                int bytesRec = handler.Receive(bytes);
                if (bytesRec == 0) {
                    break; // Client closed Socket
                }

                strReceive = String.Concat(strReceive, System.Text.Encoding.ASCII.GetString(bytes, 0, bytesRec));

                mw.Dispatcher.Invoke(() => mw.IsGetXML(strReceive)); 

                strReceive = null;
            }
        }    
    }
}
