using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST1 {
    public static class Currection {
        public static void WriteToFile(string path, _Point p, string ipoc) {
            try {         
                   
                using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default)) {
                    //sw.WriteLine($"{ipoc}|{p.X}|{p.Y}|{p.Z}");
                    sw.WriteLine($"{ipoc}|{p.X}|{p.Y}|{p.Z}");
                }
           
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
        public static void WriteToFile(string path, _Point p) {
            try {

                using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default)) {
                    //sw.WriteLine($"{ipoc}|{p.X}|{p.Y}|{p.Z}");
                    sw.WriteLine($"{p.X}|{p.Y}|{p.Z}");
                }

            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        public static List<_Point> ReadFromFile(string path) {
            string[] readText = File.ReadAllLines(path);
            List<_Point> res = new List<_Point>();
            foreach (string item in readText) {
                var temp = item.Split('|');
                res.Add(new _Point() {
                    X = Convert.ToDouble(temp[0]),
                    Y = Convert.ToDouble(temp[1]),
                    Z = Convert.ToDouble(temp[2])
                });
            }
            return res;
        }
    }
}
