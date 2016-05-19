using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowMuchInk
{
    class AnalyseState
    {
        public static String Pfad = "";
        static string status = "Ready";
        static int Cstatus = 0;
        public static string getStatus()
        {
            return status;
        }
        public static void setStatus(string i)
        {
            status = i;
        }
        public static int getCStatus()
        {
            return Cstatus;
        }
        public static void setCStatus(int i)
        {
            Cstatus = i;
        }
        public static float Cyan = 0;
        public static float Magenta = 0;
        public static float Yellow = 0;
        public static float Black = 0;
        public static void setResult(ulong[] input)
        {
            Cyan = input[0];
            Magenta = input[1];
            Yellow = input[2];
            Black = input[3];
        }
    }
}
