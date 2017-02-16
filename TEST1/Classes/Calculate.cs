using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST1 {
    public static class Calculate {

         public struct ArrData{
            public int FirstIndex;
            public int SecondIndex;
            public double Difference;
        }

        static double A(double x) {
            return Math.Abs(x);
        }
        public static List<MyPoint> Usred(List<MyPoint> Data2) {

            double differenceZ = 1;
            double differenceX = 1;
            for (int j = 0; j < Data2.Count; j++) {
                for (int i = 0; i < Data2.Count - 2; i++) {
                    if (A(A(Data2[i].Z) - A(Data2[i + 2].Z)) < differenceZ &&
                        A(A(Data2[i].X) - A(Data2[i + 2].X)) < differenceX) {
                        Data2[i + 1] = new MyPoint() {
                            X = (Data2[i].X + Data2[i + 2].X) / 2,
                            Z = (Data2[i].Z + Data2[i + 2].Z) / 2
                        };
                    }
                }
            }
            return Data2;
        }

        public static List<MyPoint> ZeroZ(double[] X, double[] Z) {
            var Data = new List<MyPoint>();
            for (int i = 0; i < X.Length; i++) {
                if (Z[i] != 0) {
                    Data.Add(new MyPoint() {
                        X = X[i],
                        Z = Z[i]
                    });
                }              
            }
            return Data;
        }

        public static MyPoint FindSpad(List<MyPoint> data) {
            double zRes = 0;
            for (int i = 0; i < 50; i++) {
                zRes += data[0].Z;
                zRes += data[data.Count - 1 - i].Z;
            }
            zRes /= 100;

            //double zRes = data.Average(x => x.Z);

            double Dif = 1;
            List<ArrData> DefData = new List<ArrData>();
            for (int i = 0; i < data.Count; i++) {
                if (Math.Abs(data[i].Z - zRes) > Dif) {
                    DefData.Add(new ArrData() {
                        FirstIndex = i,
                        Difference = Math.Abs(data[i].Z - zRes)
                    });
                }
            }

            List<ArrData> Def2 = new List<ArrData>();
            int tempCount = 0;
            int maxCount = 1;
            for (int i = 0; i < DefData.Count - 1; i++) {
                if (DefData[i].FirstIndex == DefData[i+ 1].FirstIndex - 1) {
                    tempCount++;
                } else {
                    if (tempCount > maxCount) {
                        maxCount = tempCount;
                        tempCount = 0;
                        Def2.Add(new ArrData() {
                            FirstIndex = i - maxCount,
                            SecondIndex = i,
                            Difference = maxCount
                        });
                    }
                }
            }
            int index = 0;
            if (Def2.Count != 0) {
                Def2 = Def2.OrderBy(x => x.Difference).ToList();
                if (Def2[0].Difference < Def2[Def2.Count - 1].Difference) {
                    index = Def2.Count - 1;
                }
                return new MyPoint() { X = (data[Def2[index].FirstIndex].X + data[Def2[index].SecondIndex].X) / 2, Z = zRes };


            }

            return new MyPoint() { X = 0, Z = 0 };
        }

        public static MyPoint FindSpad2(List<MyPoint> data) {
            double zRes = 0;
            for (int i = 0; i < 50; i++) {
                zRes += data[0].Z;
                zRes += data[data.Count - 1 - i].Z;
            }
            zRes /= 100;

            List<ArrData> Def = new List<ArrData>();
            for (int i = 0; i < data.Count - 1; i++) {
                Def.Add(new ArrData() {
                    FirstIndex = i,
                    SecondIndex = i + 1,
                    Difference = Math.Abs(data[i].Z - data[i + 1].Z)
                });
            }
            Def = Def.OrderBy(x => -x.Difference).ToList();
            int index1 = Def[0].FirstIndex;
            int index2 = Def[1].SecondIndex;
            double x1 = data[index1].X + data[index2].X;
            x1 /= 2;

            double z = data[index1].Z + data[index2].Z;
            z /= 2;
            return new MyPoint() { X = x1, Z =  zRes};
        }

        public static MyPoint FindSpad3(List<MyPoint> data, int a) {
            List<ArrData> Dif = new List<ArrData>();
            for (int i = 0; i < data.Count - 2; i++) {
                Dictionary<int, double> tempDic = new Dictionary<int, double>();
                double tx = data[i].X;
                double tz = data[i].Z;
                for (int j = i; j < data.Count - 1; j++) {
                    if (Math.Abs(data[j].X - tx) != 0) {
                        double difDistance = Math.Abs(data[j].Z - tz) / Math.Abs(data[j].X - tx);
                        tempDic.Add(j, difDistance);
                    } else {
                        tempDic.Add(j, Math.Abs(data[j].Z - tz));
                    }
                    
                }
                int index = tempDic.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                Dif.Add(new ArrData() {
                    FirstIndex = i,
                    SecondIndex = index,
                    Difference = tempDic[index]
                });
            }

            Dif = Dif.OrderBy(x => -x.Difference).ToList();
            int c = 1;
            //while (true) {

            //}


            if (a == 0) {
                return new MyPoint() { X = data[Dif[0].FirstIndex].X, Z = data[Dif[0].FirstIndex].Z };
            } else if (a == 1) {
                return new MyPoint() { X = data[Dif[0].SecondIndex].X, Z = data[Dif[0].SecondIndex].Z };
            } else if (a == 1) {
                return new MyPoint() { X = data[Dif[1].FirstIndex].X, Z = data[Dif[1].FirstIndex].Z };
            } else {
                return new MyPoint() { X = data[Dif[1].SecondIndex].X, Z = data[Dif[1].SecondIndex].Z };
            }
            
            return new MyPoint() { X = 0, Z = 0 };
        }

        public static MyPoint FindPointWithAngle(List<MyPoint> data, int step = 30, int count = 5) {
            //первая точка с последующими по углу
            List<ArrData> LeftData = new List<ArrData>();
            List<ArrData> RightData = new List<ArrData>();

            for (int i = 0; i < data.Count; i += step) {
                LeftData.Add(new ArrData() {
                    FirstIndex = 0,
                    SecondIndex = i,
                    Difference = Math.Atan2(data[i].Z - data[0].Z, data[i].X - data[0].X) * 180 / Math.PI
                });
            }
            //for (int i = data.Count - 1; i > 0; i -= step) {
            //    RightData.Add(new ArrData() {
            //        FirstIndex = data.Count - 1,
            //        SecondIndex = i,
            //        Difference = -Math.Atan2(data[i].Z - data[data.Count - 1].Z, data[i].X - data[data.Count - 1].X) * 180 / Math.PI
            //    });
            //}
            //Console.Clear();
            //Console.WriteLine($"LeftData Count {LeftData.Count}  :");
            //foreach (ArrData item in LeftData) {
            //    Console.WriteLine($"{item.FirstIndex} \t {item.SecondIndex} \t {item.Difference}");
            //}
            //Console.WriteLine($"||||||||||||||||||||||||||||");

            //Console.WriteLine($"RightData Count {RightData.Count}  :");
            //foreach (ArrData item in RightData) {
            //    Console.WriteLine($"{item.FirstIndex} \t {item.SecondIndex} \t {item.Difference}");
            //}
            //Console.WriteLine($"||||||||||||||||||||||||||||");

            int tempCount = 0;
            ArrData tempAngle = LeftData[0];
            int leftIndex = 0;
            int rightIndex = 0;

           

            for (int i = 0; i < LeftData.Count; i++) {
                //if (LeftData[i].Difference * tempAngle.Difference < 0) {
                if (LeftData[i].Difference < 0) {
                    tempCount++;
                } else {
                    if (tempCount >= count) {
                        leftIndex = tempAngle.SecondIndex;
                        rightIndex = LeftData[i].SecondIndex;
                        break;
                    } else {
                        tempAngle = LeftData[i];
                        tempCount = 0;
                    }
                }
            }
            if (leftIndex == 0 && rightIndex == 0) {
                //Console.WriteLine($"1 {data[rightIndex].Z}   {data[leftIndex].Z}");
                return FindPointWithAngleRight(data, step, count);
            }
            MyPoint result = new MyPoint() { X = (data[leftIndex].X + data[rightIndex].X) / 2, Z = (data[leftIndex].Z + data[rightIndex].Z) / 2 };
           
            int pCountOnLine = 0;
            for (int i = 0; i < data.Count; i++) {
                if (data[i].X < data[leftIndex].X && data[i].X > data[rightIndex].X) {
                    if( Math.Abs(data[i].Z -result.Z) < 2) {
                        pCountOnLine++;
                    }
                }
            }
            if (pCountOnLine < 20) {
                return result;
            } 
            //else {
            //   // Console.WriteLine($"11 {data[rightIndex].Z}   {data[leftIndex].Z}");
            //    return FindPointWithAngleRight(data, step, count);
            //}
            
            return new MyPoint() { X = 0, Z = 0 };
        }
        public static MyPoint FindPointWithAngleRight(List<MyPoint> data, int step = 30, int count = 5) {

            List<ArrData> RightData = new List<ArrData>();


            for (int i = data.Count - 1; i > 0; i -= step) {
                RightData.Add(new ArrData() {
                    FirstIndex = data.Count - 1,
                    SecondIndex = i,
                    Difference = Math.Atan2(data[i].Z - data[data.Count - 1].Z, data[i].X - data[data.Count - 1].X) * 180 / Math.PI
                });
            }
            if (RightData.Count == 0) {
                return new MyPoint() { X = 0, Z = 0 };
            }

            int tempCount = 0;
            ArrData tempAngle = RightData[0];
            int leftIndex = 0;
            int rightIndex = 0;
            for (int i = 0; i < RightData.Count; i++) {
                //if (LeftData[i].Difference * tempAngle.Difference < 0) {
                if (RightData[i].Difference < 0) {
                    tempCount++;
                } else {
                    if (tempCount >= count) {
                        leftIndex = tempAngle.SecondIndex;
                        rightIndex = RightData[i].SecondIndex;
                        break;
                    } else {
                        tempAngle = RightData[i];
                        tempCount = 0;
                    }
                }
            }

            MyPoint result = new MyPoint() { X = (data[leftIndex].X + data[rightIndex].X) / 2, Z = (data[leftIndex].Z + data[rightIndex].Z) / 2 };

            int pCountOnLine = 0;
            for (int i = 0; i < data.Count; i++) {
                if (data[i].X < data[rightIndex].X && data[i].X > data[leftIndex].X) {
                    if (Math.Abs(data[i].Z - result.Z) < 2) {
                        pCountOnLine++;
                    }
                }
            }
            if (pCountOnLine < 20) {
                return result;
            }


            //Console.WriteLine($"2 {data[rightIndex].Z}   {data[leftIndex].Z}");
            return new MyPoint() { X = 0, Z = 0 };

           // return new MyPoint() { X = (data[leftIndex].X + data[rightIndex].X) / 2, Z = (data[leftIndex].Z + data[rightIndex].Z) / 2 };
        }
        public static MyPoint FindPointWithAngle(List<MyPoint> data, int step = 30, int count = 5, double DifAngle = 10) {
            //первая точка с последующими по углу
            List<ArrData> LeftData = new List<ArrData>();
            List<ArrData> RightData = new List<ArrData>();

            for (int i = 0; i < data.Count; i += step) {
                LeftData.Add(new ArrData() {
                    FirstIndex = 0,
                    SecondIndex = i,
                    Difference = Math.Atan2(data[i].Z - data[0].Z, data[i].X - data[0].X) * 180 / Math.PI
                });
            }
          
            //Console.Clear();
            //Console.WriteLine($"LeftData Count {LeftData.Count}  :");
            //foreach (ArrData item in LeftData) {
            //    Console.WriteLine($"{item.FirstIndex} \t {item.SecondIndex} \t {item.Difference}");
            //}
            //Console.WriteLine($"||||||||||||||||||||||||||||");

            int tempCount = 0;
            ArrData tempAngle = LeftData[0];
            int leftIndex = 0;
            int rightIndex = 0;
            double AverageAngle = 0;
            for (int i = 3; i < 13; i++) {
                AverageAngle += LeftData[i].Difference;
            }
            AverageAngle /= 10;

            for (int i = 0; i < LeftData.Count; i++) {
                //if (LeftData[i].Difference * tempAngle.Difference < 0) {
                //if (LeftData[i].Difference < 0) {
                if (Math.Abs(LeftData[i].Difference - AverageAngle) > DifAngle) {
                    tempCount++;
                } else {
                    if (tempCount >= count) {
                        leftIndex = tempAngle.SecondIndex;
                        rightIndex = LeftData[i].SecondIndex;
                        break;
                    } else {
                        tempAngle = LeftData[i];
                        tempCount = 0;
                    }
                }
            }

            return new MyPoint() { X = (data[leftIndex].X + data[rightIndex].X) / 2, Z = (data[leftIndex].Z + data[rightIndex].Z) / 2 };
        }
    }
}
