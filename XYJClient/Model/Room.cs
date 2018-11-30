using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYJClient.Model
{
    public class Room
    {
        public List<Direction> Dirs
        { get; set; }
       
        public List<RoomAction> RoomActions
        {
            get;set;
        }
        public string ShortDesc
        {
            get; set;
        }
        public string Desc
        {
            get; set;
        }
       
    }
}
