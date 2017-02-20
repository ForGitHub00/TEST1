using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace TEST1 {
    public struct ValueXML {
        public string Name;
        public double Value;
        public string[] Parrents;
        public string[] Children;
    }
    public static class MyXML {
        //public static List<ValueXML> GetValue(string str) {
        public static void GetValue(string str) {
            //string[] Nodes = str.Split('/');


        }

      
        //public static void GetValues(string strXML) {
        //    XDocument xdoc = XDocument.Parse(strXML);
        //    foreach (XElement phoneElement in xdoc.Element("Rob").Elements("RIst")) {
        //        XAttribute nameAttribute = phoneElement.Attribute(name);              
        //        if (nameAttribute != null) {
        //            Console.WriteLine("X: {0}", nameAttribute.Value);
        //        }
        //        Console.WriteLine();
        //    }
        //}



        public static double GetValues(string strXML, string name) {
            XDocument xdoc = XDocument.Parse(strXML);
            foreach (XElement phoneElement in xdoc.Element("Rob").Elements("RIst")) {
                XAttribute nameAttribute = phoneElement.Attribute(name);
                if (nameAttribute != null) {
                    return Convert.ToDouble(nameAttribute.Value.Replace('.',','));
                }
            }
            return 0;
        }
        private static void Write(XmlNode node) {
            foreach (XmlNode item in node.ChildNodes) {
                if (item.ChildNodes.Count != 0) {
                    Write(item);
                } else {
                    Console.WriteLine($"Atr = {item.Attributes}  Val = {item.Value}  Text = {item.InnerText}");
                }
            }

            XDocument xdoc = XDocument.Load("phones.xml");
            foreach (XElement phoneElement in xdoc.Element("Sen").Elements("RKorr")) {
                XAttribute nameAttribute = phoneElement.Attribute("name");
                XElement companyElement = phoneElement.Element("company");
                XElement priceElement = phoneElement.Element("price");

                if (nameAttribute != null && companyElement != null && priceElement != null) {
                    Console.WriteLine("Смартфон: {0}", nameAttribute.Value);
                    Console.WriteLine("Компания: {0}", companyElement.Value);
                    Console.WriteLine("Цена: {0}", priceElement.Value);
                }
                Console.WriteLine();
            }


        }

        public static double GetValuesPA(string strXML, string name) {
            XDocument xdoc = XDocument.Parse(strXML);
            foreach (XElement phoneElement in xdoc.Element("Rob").Elements("PosA")) {
                XAttribute nameAttribute = phoneElement.Attribute(name);
                if (nameAttribute != null) {
                    return Convert.ToDouble(nameAttribute.Value.Replace('.', ',')) / 1000;
                }
            }
            return 0;
        }
    } 
}
