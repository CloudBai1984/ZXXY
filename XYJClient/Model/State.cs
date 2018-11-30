using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYJClient.Model
{
    public class State
    {
        public Room RoomState
        { get; set; }

        public List<MyPal> MyPalList
        { get; set; }

        public List<Inv> InvList
        { get; set; }

        

        public bool FailParserRoom
        { get; set; }
        public bool FailParserInv
        { get; set; }

        public bool FailParserMyPal
        { get; set; }
    }
}
