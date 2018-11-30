using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XYJClient.Model;

namespace XYJClient.Util
{
    public class MessageParser
    {

        public static void Parser(string message)
        {
            State state = new State();
            string[] jsons = message.Split('^');
            JObject json = null;
            foreach (string jsonStr in jsons)
            {
                if (!string.IsNullOrEmpty(jsonStr) && jsonStr.StartsWith("{"))
                {
                    json = JObject.Parse(jsonStr);
                    string jsonName = json["t"].ToString();
                    if ("room".Equals(jsonName))
                    {
                        try
                        {
                            Global.LAST_ROOM = Global.CURRENT_ROOM;
                            Global.CURRENT_ROOM = ParserRoom(json);
                        }
                        catch (Exception)
                        {

                        }

                    }
                    else if ("inv".Equals(jsonName))
                    {
                        try
                        {
                            List<Inv> lstInv = new List<Inv>();
                            foreach (JProperty p in json.GetValue("p"))
                            {

                                Inv inv = new Inv()
                                {
                                    Name = p.Value.ToString(),
                                    Id = p.Name
                                };
                                lstInv.Add(inv);
                            }
                            Global.INV_LIST = lstInv;
                        }
                        catch (Exception)
                        {

                            state.FailParserInv = true;
                        }
                    }
                    else if ("mypal".Equals(jsonName))
                    {
                        try
                        {
                            List<MyPal> lstMyPal = new List<MyPal>();
                            foreach (JProperty p in json.GetValue("p"))
                            {

                                MyPal mypal = new MyPal()
                                {
                                    Name = p.Value.ToString(),
                                    UserId = p.Name
                                };
                                Global.USER_ID = p.Name;
                                lstMyPal.Add(mypal);
                            }
                            Global.MYPAL_LIST = lstMyPal;
                        }
                        catch (Exception)
                        {

                            state.FailParserMyPal = true;
                        }
                    }
                    else if ("mana".Equals(jsonName))
                    {
                        try
                        {
                            PlayerStatus.Neili = int.Parse(json["neili"].ToString());
                            PlayerStatus.MaxNeili = int.Parse(json["neili_max"].ToString());
                            PlayerStatus.Fali = int.Parse(json["fali"].ToString());
                            PlayerStatus.MaxFali = int.Parse(json["fali_max"].ToString());

                        }
                        catch (Exception)
                        {


                        }
                    }
                    else if ("info".Equals(jsonName))
                    {
                        try
                        {
                            string tmp = json["p"].ToString();
                            Match m = Regex.Match(tmp, @"气血.*?(?<cur>\d+)\s+/(?<max>\d+).*?\[\s(?<ss>\d+)%\]");
                            PlayerStatus.HP = int.Parse(m.Groups["cur"].Value);
                            PlayerStatus.MaxHP = int.Parse(m.Groups["max"].Value);
                            PlayerStatus.HPSS = int.Parse(m.Groups["ss"].Value);
                            m = Regex.Match(tmp, @"精神.*?(?<cur>\d+)\s+/(?<max>\d+).*?\[\s(?<ss>\d+)%\]");
                            PlayerStatus.Jing = int.Parse(m.Groups["cur"].Value);
                            PlayerStatus.MaxJing = int.Parse(m.Groups["max"].Value);
                            PlayerStatus.JingSS = int.Parse(m.Groups["max"].Value);
                            m = Regex.Match(tmp, @"内力.*?(?<cur>\d+)\s+/(?<max>\d+)");
                            PlayerStatus.Neili = int.Parse(m.Groups["cur"].Value);
                            PlayerStatus.MaxNeili = int.Parse(m.Groups["max"].Value);
                            m = Regex.Match(tmp, @"法力.*?(?<cur>\d+)\s+/(?<max>\d+)");
                            PlayerStatus.Fali = int.Parse(m.Groups["cur"].Value);
                            PlayerStatus.MaxFali = int.Parse(m.Groups["max"].Value);
                        }
                        catch (Exception)
                        {


                        }
                    }
                    else if ("busy".Equals(jsonName))
                    {
                        try
                        {
                            Global.BUSY_TIME = int.Parse(json["busy"].ToString());
                        }
                        catch (Exception)
                        {


                        }
                    }
                    else if ("status".Equals(jsonName))
                    {
                        try
                        {

                            JObject p = JObject.Parse(json["p"].ToString());


                            string obj = p["obj"].ToString();
                            if (obj.Equals(Global.USER_ID))
                            {
                                PlayerStatus.Kee = int.Parse(p["kee"].ToString());
                                PlayerStatus.Eff_Kee = int.Parse(p["eff_kee"].ToString());
                                PlayerStatus.Sen = int.Parse(p["sen"].ToString());
                                PlayerStatus.Eff_Sen = int.Parse(p["eff_sen"].ToString());
                            }

                        }
                        catch (Exception)
                        {


                        }
                    }

                }
            }


        }





        static Room ParserRoom(JObject json)
        {
            List<Direction> lstDir = new List<Direction>();
            List<RoomAction> lstAction = new List<RoomAction>();
            Room room = new Room();
            foreach (string dirStr in json.GetValue("dirs").ToString().Split('@'))
            {
                if (!string.IsNullOrEmpty(dirStr))
                {
                    string dirCode = dirStr.Split('&')[0];
                    string dirName = dirStr.Split('&')[1];
                    Direction dir = new Direction()
                    {
                        ChineseName = dirName,
                        EnglishName = dirCode
                    };

                    lstDir.Add(dir);
                }
            }
            foreach (string actionStr in json.GetValue("room_actions").ToString().Split('-'))
            {
                if (!string.IsNullOrEmpty(actionStr))
                {
                    string code = actionStr.Split(':')[1];
                    string name = actionStr.Split(':')[0];
                    RoomAction action = new RoomAction()
                    {
                        ActionName = name,
                        Action = code
                    };
                    lstAction.Add(action);
                }
            }
            room.Dirs = lstDir;
            room.RoomActions = lstAction;
            room.Desc = json.GetValue("env").ToString();
            room.ShortDesc = json.GetValue("short").ToString();

            return room;
        }

        static Direction[] ParserDirection(string message)
        {
            List<Direction> lstDir = new List<Direction>();
            string tmp = Regex.Match(message, "\"dirs\":\"(?<test>.+)\"").Groups["test"].Value;
            foreach (string sdir in tmp.Split('@'))
            {
                Direction dir = new Direction();
                string[] tmp2 = sdir.Split('&');
                dir.ChineseName = tmp2[1];
                dir.EnglishName = tmp2[0];
                lstDir.Add(dir);
            }

            return lstDir.ToArray();
        }

        static Direction[] ParserNPC(string message)
        {
            List<Direction> lstDir = new List<Direction>();
            string tmp = Regex.Match(message, "\"p\":\"(?<test>.+)\"").Groups["test"].Value;
            foreach (string sdir in tmp.Split('@'))
            {
                Direction dir = new Direction();
                string[] tmp2 = sdir.Split('&');
                dir.ChineseName = tmp2[1];
                dir.EnglishName = tmp2[0];
                lstDir.Add(dir);
            }

            return lstDir.ToArray();
        }

    }
}

