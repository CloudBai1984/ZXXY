using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYJClient.Model
{
    static class PlayerStatus
    {
        static PlayerStatus()
        {
            Kee = 100;
            Eff_Kee = 100;
            Sen = 100;
            Eff_Sen = 100;
        }


        public static int HP
        { get; set; }
        public static int MaxHP
        { get; set; }
        public static int HPSS
        { get; set; }
        public static int Kee
        { get; set; }
        public static int Eff_Kee
        { get; set; }
        public static int Jing
        { get; set; }
        public static int MaxJing
        { get; set; }
        public static int JingSS
        { get; set; }
        public static int Sen
        { get; set; }
        public static int Eff_Sen
        { get; set; }
        public static int Neili
        { get; set; }
        public static int MaxNeili
        { get; set; }
        public static int Fali
        { get; set; }
        public static int MaxFali
        { get; set; }
    }
}
