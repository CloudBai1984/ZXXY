using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WebSocket4Net;
using XYJClient.Model;
using XYJClient.Util;

namespace XYJClient.Robots
{
    class MYRobot : Robot
    {
        Random rnd = new Random();
        bool isFindYG = false;
        bool isKillNPC = false;
        bool isDaoDan = false;
        bool isFail = false;

        string target = "";

        static MYRobot Instance;

        public static MYRobot GetInstance()
        {
            if (Instance == null)
                Instance = new MYRobot();
            return Instance;
        }

        private MYRobot() : base()
        {
        }



        public override void Run()
        {
            isFindYG = false;
            isDaoDan = false;
            isFail = false;
            base.Run();
            Send("huanchao");
            Send("move /d/dntg/yunlou/yunloutai");
            FightController.GetInstance().CheckStatusNoFighting(this);

            string resp = UtilFunction.GetResponse(this, "ask li jing about 灭妖", "老夫不是派你");
            if (!string.IsNullOrEmpty(resp))
            {
                SendWaitMessage("ask li jing about 失败", 1000);
                SendWaitMessage("ask li jing about 灭妖", 1000);
            }

            Thread.Sleep(3000);

            if (FindYaoGuaiOrder())
            {
                FightController.GetInstance().Fight(this);
                Console.WriteLine("==============================================完成一轮灭妖");
            }
            else
            {
                Console.WriteLine("==============================================顺序找妖怪失败，开始随机找妖怪");
                if (FindYaoGuaiRnd())
                {
                    FightController.GetInstance().Fight(this);
                    Console.WriteLine("==============================================完成一轮灭妖");
                }
                else
                {
                    Console.WriteLine("==============================================未找到灭妖，灭妖失败");
                    Send("huanchao");
                    Send("move /d/dntg/yunlou/yunloutai");
                    Send("ask li jing about 放弃");
                }

            }
            base.Complete();
            isFindYG = false;
            isKillNPC = false;
        }

        bool FindYaoGuaiRnd()
        {
            Console.WriteLine("==============================================开始随机找妖怪");
            int num = 0;
            while (num < 5)
            {
                Send("findguai");
                Send("open door");
                Console.WriteLine("==============================================模拟命令findguai");
                for (int i = 0; i < 5; i++)
                {
                    List<string> dirs = Global.CURRENT_ROOM.Dirs.Select(dir => dir.EnglishName).ToList();
                    int rdnum = rnd.Next(dirs.Count);
                    if (GoFind(dirs[rdnum]))
                    {
                        return true;
                    }
                }


                num++;
            }
            return false;

        }

        bool FindYaoGuaiOrder()
        {
            int findnum = 0;
            FindAgain:
            findnum++;
            if (findnum >= 3)
                return false;
            Send("findguai");
            Send("look");
            Send("open Door");
            Console.WriteLine("==============================================模拟命令findguai");
            Thread.Sleep(1000);
            List<string> dirs = Global.CURRENT_ROOM.Dirs.Select(dir => dir.EnglishName).ToList();
            Console.WriteLine("=========================================开始在" + Global.CURRENT_ROOM.ShortDesc + "寻找");
            string tmp = "";
            foreach (string dir in dirs)
                tmp += dir + ",";
            Console.WriteLine("=========================================开始在下面的方向搜索" + tmp);
            for (int i = 0; i < dirs.Count; i++)
            {
                if (GoFind(dirs[i]))
                {
                    goto FindYG;
                }
                if (CheckIfSpecialPlace())
                {
                    return false;
                }

                string back = UtilFunction.GetBackDirection(dirs[i]);
                Send("look");
                Thread.Sleep(500);
                dirs = Global.CURRENT_ROOM.Dirs.Select(dir => dir.EnglishName).ToList();
                if (dirs.Count == 1) back = dirs[0];
                if (string.IsNullOrEmpty(back) || !dirs.Contains(back))
                {
                    Console.WriteLine("=========================================================顺序寻找没找到回去的路");
                    goto FindAgain;
                }
                if (GoFind(back))
                {
                    goto FindYG;
                }

            }
            Console.WriteLine("=========================================一轮顺序搜索完毕，地点是" + Global.CURRENT_ROOM.ShortDesc);
            goto FindAgain;
            FindYG:
            Console.WriteLine("=========================================================已经发现妖怪了");
            return true;

        }


        internal override void Parser(string message)
        {
            base.Parser(message);
            if (Regex.IsMatch(message, "你要对谁施展") || Regex.IsMatch(message, "你要用金砖砸谁"))
            {
                FightController.GetInstance().FightComplete();
                isKillNPC = false;
            }
            else if (Regex.IsMatch(message, "全身的流光溢彩渐渐消去了"))
            {
                FightController.GetInstance().Buff();

            }
            else if (Regex.IsMatch(message, "\"p\":\"任务给予"))
            {
                target = Regex.Match(message, @"任务目标.*?31m(?<yg>.*?)\(").Groups["yg"].Value;
                Console.WriteLine("==============================================接到灭妖任务，去杀" + target);
            }
            else if (Regex.IsMatch(message, "无可奈何，化做一道青光，直冲云霄去了"))
            {
                Console.WriteLine("========================================================消灭了妖怪了！");
                isFindYG = true;
            }
            else if (Regex.IsMatch(message, "看起来.*想杀死你！"))
            {
                fight.Fight(this);
                string name = Regex.Match(message, "看起来(?<name>.*?)想杀死你！").Groups["name"].Value;
                Console.WriteLine("==============================================有人要杀我，名字是" + name);
                if (name.Contains(target))
                    isFindYG = true;
                else
                    isKillNPC = true;

            }
            else if (Regex.IsMatch(message, "拉着你不让走"))
            {

                isDaoDan = true;
                UtilFunction.DealWithDaoDanGui(this);
                Console.WriteLine("==============================================捣蛋鬼出现了");
                isDaoDan = false;
            }
            else if (Regex.IsMatch(message, "老夫不是派你去"))
            {


                isFail = true;

            }

        }

        bool GoFind(string direction)
        {
            int flag = 1;
            while (isKillNPC || isDaoDan )
            {
                if (flag >= 10 && !isDaoDan)
                {
                    isKillNPC = false;
                    break;
                }
                Thread.Sleep(1000);
                flag++;
            }
            if (isFindYG) return true;
            Console.WriteLine("==============================================我去找妖怪，去的方式是" + direction);
            Send("go " + direction);

            string resp = UtilFunction.GetResponse(this, "kill myguai", target);

            if (!string.IsNullOrEmpty(resp))
            {
                Console.WriteLine("============================================发现妖怪" + target);
                return true;
            }
            return false;
        }


        bool CheckIfSpecialPlace()
        {
            if (Global.CURRENT_ROOM.ShortDesc.Contains("白云洞"))
            {
                return true;
            }
            return false;
        }
    }
}
