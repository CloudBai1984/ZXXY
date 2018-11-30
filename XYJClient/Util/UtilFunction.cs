using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using XYJClient.Robots;

namespace XYJClient.Util
{
    class UtilFunction
    {

        static Dictionary<string, string> dicDir = new Dictionary<string, string>() {
            { "north","south"},
            { "south","north"},
            { "west","east"},
            { "east","west"},
            { "up","down"},
            { "down","up"},
            { "southwest","northeast"},
            { "northeast","southwest"},
            { "southeast","northwest"},
            { "northwest","southeast"},
            { "northup","southdown"},
            { "southdown","northup"},
            { "northdown","southup"},
            { "southup","northdown"},
            { "enter","out"},
            { "out","enter"},
            { "eastup","westdown"},
            { "eastdown","westup"},
            { "westup","eastdown"},
            {"westdown","eastup" },
            {"left","right" },
            {"right","left" },
            {"rightup","leftdown" },
            {"leftup","rightdown" },
            {"rightdown","leftup" },
            {"leftdown","rightup" },
        };


        public static string GetBackDirection(string direction)
        {
            string back = dicDir[direction];
            if (string.IsNullOrEmpty(back))
                Console.WriteLine("=========================================找不到回去的路了，进来的方向是" + direction);
            return back;
        }

        public static List<T> Clone<T>(object List)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, List);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as List<T>;
            }
        }

        public static int GetBusyTime()
        {
            return Global.BUSY_TIME > 0 ? Global.BUSY_TIME : 0;
        }


        public static void DealWithDaoDanGui(Robot robot)
        {
            Console.Beep();
            new System.Threading.Thread(new System.Threading.ThreadStart(() => {
                //System.Windows.Forms.MessageBox.Show(Global.MAIN_FORM, "捣蛋鬼来了！！");
            })).Start();
          

            if (Global.INV_LIST.Where(inv => inv.Id.Contains("daodan")).Select(inv => inv.Id).Count() >= 0)
            {
                string daodanId = Global.INV_LIST.Where(inv => inv.Id.Contains("daodan")).Select(inv => inv.Id).First();
                string respon = GetResponse(robot, "query " + daodanId, "数学");
                string question = Regex.Match(respon, @"数学题.*?###(?<question>.*?)###.*?结果是").Groups["question"].Value;
                if (!string.IsNullOrEmpty(question))
                {
                    try
                    {
                        int result = ProcessMathQuestion(question);
                        robot.Send("zoukaii " + result.ToString());
                    }
                    catch (Exception)
                    {
                        Console.Beep();
                        Console.Beep();
                        Console.Beep();
                        System.Windows.Forms.MessageBox.Show(Global.MAIN_FORM, "捣蛋鬼答题失败了");
                    }
                }
                else
                {
                    respon = GetResponse(robot, "query " + daodanId, "你的道行修行有多少年");
                    if (Regex.IsMatch(respon, @"你的道行修行有多少年"))
                    {
                        respon = UtilFunction.GetResponse(robot, "hp", "\"t\":\"info\"");
                        question = Regex.Match(respon, @"道行.*?35m(?<dh>.*)\s+收益").Groups["dh"].Value;
                        string result = ProcessDaoHangQuestion(question);
                        robot.Send("zoukaii " + result);
                    }
                }
            }



        }

        public static string ProcessDaoHangQuestion(string question)
        {
            question = question.Replace(" ", "");
            string[] array = question.ToArray().Select(c =>
            {
                string str = string.Empty;
                switch (c)
                {
                    case '零':
                        str = "0";
                        break;
                    case '一':
                        str = "1";
                        break;
                    case '二':
                        str = "2";
                        break;
                    case '三':
                        str = "3";
                        break;
                    case '四':
                        str = "4";
                        break;
                    case '五':
                        str = "5";
                        break;
                    case '六':
                        str = "6";
                        break;
                    case '七':
                        str = "7";
                        break;
                    case '八':
                        str = "8";
                        break;
                    case '九':
                        str = "9";
                        break;

                    default:
                        str = string.Empty;
                        break;
                }
                if (Regex.IsMatch(c.ToString(), @"\d"))
                {
                    str = c.ToString();
                }
                return str;
            }

            ).Where(str => !str.Equals("")).Select(str => str).ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (string str in array)
            {
                sb.Append(str);
            }
            Console.WriteLine("=================================================解析捣蛋鬼道行问题成功，道行是" + sb.ToString());
            return sb.ToString();
        }

        public static int ProcessMathQuestion(string question)
        {
            question = question.Replace(" ", "");
            string[] array = question.ToArray().Select(c =>
                {
                    string str = string.Empty;
                    switch (c)
                    {
                        case '零':
                            str = "0";
                            break;
                        case '一':
                            str = "1";
                            break;
                        case '二':
                            str = "2";
                            break;
                        case '三':
                            str = "3";
                            break;
                        case '四':
                            str = "4";
                            break;
                        case '五':
                            str = "5";
                            break;
                        case '六':
                            str = "6";
                            break;
                        case '七':
                            str = "7";
                            break;
                        case '八':
                            str = "8";
                            break;
                        case '九':
                            str = "9";
                            break;
                        case '十':
                            str = "x";
                            break;
                        case '加':
                            str = "+";
                            break;
                        case '减':
                            str = "-";
                            break;
                        case '+':
                            str = "+";
                            break;
                        case '-':
                            str = "-";
                            break;
                        default:
                            str = string.Empty;
                            break;
                    }
                    if (Regex.IsMatch(c.ToString(), @"\d"))
                    {
                        str = c.ToString();
                    }
                    return str;
                }

            ).Where(str => !str.Equals("")).Select(str => str).ToArray();

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals("x"))
                {
                    if (i - 1 >= 0 && Regex.IsMatch(array[i - 1], @"\d"))
                    {
                        if (i + 1 < array.Length && Regex.IsMatch(array[i + 1], @"\d"))
                            array[i] = string.Empty;
                        else
                            array[i] = "0";
                    }
                    else
                    {
                        if (i + 1 < array.Length && Regex.IsMatch(array[i + 1], @"\d"))
                            array[i] = "1";
                        else
                            array[i] = "10";
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (string str in array.Where(str => !string.IsNullOrEmpty(str)).Select(str => str).ToArray())
            {
                sb.Append(str);
            }
            Console.WriteLine("=================================================捣蛋鬼问题，解析公式完成，公式是" + sb.ToString());
            var result = new System.Data.DataTable().Compute(sb.ToString(), "");
            Console.WriteLine("=================================================计算结果是" + result.ToString());
            return (int)result;
        }



        public static string GetResponse(Robot robot, string request, string keyword)
        {
            Console.WriteLine("=====================================模拟发送的信息：" + request);
            string respon = string.Empty;
            bool empty = true;
            string[] msgs = robot.Send(request);
            for (int i = 0; i < msgs.Length; i++)
            {
                empty = false;
                Console.WriteLine("=====================================模拟发送服务器返回的信息：" + msgs[i]);
                if (!string.IsNullOrEmpty(msgs[i]) && msgs[i].Contains(keyword))
                {
                    respon = msgs[i];
                    break;
                }
            }
            if (empty)
                Console.WriteLine("=====================================没收到服务器返回的消息");
            return respon;
        }
    }
}
