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
    class DFFBRobot : Robot
    {
        delegate void LevelFuncDelegate();
        LevelFuncDelegate[] func = new LevelFuncDelegate[8];

        bool isDaoDan = false;
        static DFFBRobot Instance;

        public static DFFBRobot GetInstance()
        {
            if (Instance == null)
                Instance = new DFFBRobot();
            return Instance;
        }

        private DFFBRobot() : base()
        {

            func[0] = new LevelFuncDelegate(FirtLevel);
            func[1] = new LevelFuncDelegate(SecondLevel);
            func[2] = new LevelFuncDelegate(ThirdLevel);
            func[3] = new LevelFuncDelegate(ForthLevel);
            func[4] = new LevelFuncDelegate(FifthLevel);
            func[5] = new LevelFuncDelegate(SixthLevel);
            func[6] = new LevelFuncDelegate(SeventhLevel);
            func[7] = new LevelFuncDelegate(EighthLevel);
        }



        public override void Run()
        {
            base.Run();
            Send("huanchao");
            Send("move /d/changan/nbridge");
            Send("go south");
            Send("go enter");
            Send("callguard");
            Thread.Sleep(UtilFunction.GetBusyTime());
            FightController.GetInstance().CheckStatusNoFighting(this);
            Send("enter_fuben");
            try
            {
                for (int i = 0; i < func.Length; i++)
                {
                    Console.WriteLine("======================================进入层数为:" + i.ToString());
                    func[i]();

                }
            }
            catch (Exception e)
            {
                if (IsContinue)
                    Check();
            }
            Send("look");
            if (Global.INV_LIST != null && Global.INV_LIST.Count > 0)
            {
                string corp = Global.INV_LIST[0].Id;
                Send("get all from " + corp);
                Console.WriteLine("======================================收刮尸体了");
            }
            Send("chuqu");
            Console.WriteLine("======================================完成地府副本了");
            base.Complete();
        }

        void FirtLevel()
        {
            Send("look");
            Thread.Sleep(1000);
            string dirStr = Global.CURRENT_ROOM.Dirs.Select(dir => dir.EnglishName).ToList().First();
            GoFight(dirStr);
        }

        void SecondLevel()
        {
            List<string> dirs = Global.CURRENT_ROOM.Dirs.Where(dir => dir.ChineseName.Contains("鬼门关")).Select(dir => dir.EnglishName).ToList();
            for (int i = 0; i < dirs.Count; i++)
            {
                GoFight(dirs[i]);
                string back = UtilFunction.GetBackDirection(dirs[i]);
                GoFight(back);
            }

            string dirStr = Global.CURRENT_ROOM.Dirs.Where(dir => dir.ChineseName.Contains("招魂司")).Select(dir => dir.EnglishName).ToList().First();



            GoFight(dirStr);
        }

        void ThirdLevel()
        {
            List<string> dirs = Global.CURRENT_ROOM.Dirs.Where(dir => dir.ChineseName.Contains("招魂司")).Select(dir => dir.EnglishName).ToList();
            for (int i = 0; i < dirs.Count; i++)
            {
                GoFight(dirs[i]);
                string back = UtilFunction.GetBackDirection(dirs[i]);
                GoFight(back);
            }
            string dirStr = Global.CURRENT_ROOM.Dirs.Where(dir => dir.ChineseName.Contains("奉祭场")).Select(dir => dir.EnglishName).ToList().First();
            GoFight(dirStr);
        }

        void ForthLevel()
        {
            List<string> dirs = Global.CURRENT_ROOM.Dirs.Where(dir => dir.ChineseName.Contains("奉祭场")).Select(dir => dir.EnglishName).ToList();
            for (int i = 0; i < dirs.Count; i++)
            {
                GoFight(dirs[i]);
                string back = UtilFunction.GetBackDirection(dirs[i]);
                GoFight(back);
            }
            string dirStr = Global.CURRENT_ROOM.Dirs.Where(dir => dir.ChineseName.Contains("阴司")).Select(dir => dir.EnglishName).ToList().First();
            GoFight(dirStr);
        }

        void FifthLevel()
        {
            List<string> dirs = Global.CURRENT_ROOM.Dirs.Where(dir => dir.ChineseName.Contains("阴司")).Select(dir => dir.EnglishName).ToList();
            for (int i = 0; i < dirs.Count; i++)
            {
                GoFight(dirs[i]);
                string back = UtilFunction.GetBackDirection(dirs[i]);
                GoFight(back);
            }
            string dirStr = Global.CURRENT_ROOM.Dirs.Where(dir => dir.ChineseName.Contains("阴阳塔")).Select(dir => dir.EnglishName).ToList().First();
            GoFight(dirStr);
        }

        void SixthLevel()
        {
            List<string> dirs = Global.CURRENT_ROOM.Dirs.Where(dir => dir.ChineseName.Contains("阴阳塔")).Select(dir => dir.EnglishName).ToList();
            for (int i = 0; i < dirs.Count; i++)
            {
                GoFight(dirs[i]);
                string back = UtilFunction.GetBackDirection(dirs[i]);
                GoFight(back);
            }
            string dirStr = Global.CURRENT_ROOM.Dirs.Where(dir => dir.ChineseName.Contains("幽司")).Select(dir => dir.EnglishName).ToList().First();
            GoFight(dirStr);
        }

        void SeventhLevel()
        {
            List<string> dirs = Global.CURRENT_ROOM.Dirs.Where(dir => dir.ChineseName.Contains("幽司")).Select(dir => dir.EnglishName).ToList();
            for (int i = 0; i < dirs.Count; i++)
            {
                GoFight(dirs[i]);
                string back = UtilFunction.GetBackDirection(dirs[i]);
                GoFight(back);
            }
            string dirStr = Global.CURRENT_ROOM.Dirs.Where(dir => dir.ChineseName.Contains("奉祭场")).Select(dir => dir.EnglishName).ToList().First();
            GoFight(dirStr);
        }

        void EighthLevel()
        {
            List<string> dirs = Global.CURRENT_ROOM.Dirs.Where(dir => dir.ChineseName.Contains("奉祭场")).Select(dir => dir.EnglishName).ToList();
            for (int i = 0; i < dirs.Count; i++)
            {
                GoFight(dirs[i]);
                string back = UtilFunction.GetBackDirection(dirs[i]);
                GoFight(back);
            }
            string dirStr = Global.CURRENT_ROOM.Dirs.Where(dir => dir.ChineseName.Contains("阎罗殿")).Select(dir => dir.EnglishName).ToList().First();
            GoFight(dirStr);
        }


        internal override void Parser(string message)
        {
            base.Parser(message);
            if (Regex.IsMatch(message, "你要对谁施展") || Regex.IsMatch(message, "你要用金砖砸谁"))
            {
                FightController.GetInstance().FightComplete();

            }
            else if (Regex.IsMatch(message, "全身的流光溢彩渐渐消去了"))
            {
                FightController.GetInstance().Buff();

            }
            else if (Regex.IsMatch(message, "拉着你不让走"))
            {

                isDaoDan = true;
                UtilFunction.DealWithDaoDanGui(this);
                Console.WriteLine("==============================================捣蛋鬼出现了");
                isDaoDan = false;


            }
            else if (Regex.IsMatch(message, "免费副本机会已用完或正在冷却"))
            {
                IsContinue = false;
            }
        }

        void GoFight(string direction)
        {
            while (isDaoDan)
            {
                Thread.Sleep(1000);
            }
            int time = Global.BUSY_TIME > 0 ? Global.BUSY_TIME : 0;
            Thread.Sleep(time * 1000);
            Console.WriteLine("==============================================走向" + direction);
            Send("go " + direction);
            FightController.GetInstance().Fight(this);
            FightController.GetInstance().CheckStatusNoFighting(this);
        }

        void Check()
        {
            Console.WriteLine("======================================出错了检查问题");
            int lvl = 0;
            try
            {
                Send("look");
                Thread.Sleep(1000);
                if (Global.CURRENT_ROOM.Dirs.Count == 1)
                {
                    GoFight(Global.CURRENT_ROOM.Dirs[0].EnglishName);
                    Send("look");
                    Thread.Sleep(1000);
                }
                if (Global.CURRENT_ROOM.ShortDesc.Contains("鬼门关"))
                {
                    lvl = 1;
                }
                else if (Global.CURRENT_ROOM.ShortDesc.Contains("招魂司"))
                {
                    lvl = 2;
                }
                else if (Global.CURRENT_ROOM.ShortDesc.Contains("奉祭场") && Global.CURRENT_ROOM.Dirs.Exists(dir => dir.ChineseName.Contains("阴司")))
                {
                    lvl = 3;
                }
                else if (Global.CURRENT_ROOM.ShortDesc.Contains("阴司"))
                {
                    lvl = 4;
                }
                else if (Global.CURRENT_ROOM.ShortDesc.Contains("阴阳塔"))
                {
                    lvl = 5;
                }
                else if (Global.CURRENT_ROOM.ShortDesc.Contains("幽司"))
                {
                    lvl = 6;
                }
                else if (Global.CURRENT_ROOM.ShortDesc.Contains("奉祭场") && Global.CURRENT_ROOM.Dirs.Exists(dir => dir.ChineseName.Contains("阎罗殿")))
                {
                    lvl = 7;
                }
                else
                {
                    Console.WriteLine("======================================不地府副本，重新进入");
                    Send("huanchao");
                    Send("move /d/changan/nbridge");
                    Send("go south");
                    Send("go enter");
                    Send("enter_fuben");
                    lvl = 0;

                }
                for (int i = lvl; i < 8; i++)
                {
                    Console.WriteLine("======================================进入层数为:" + i.ToString());
                    func[i]();

                }
            }
            catch (Exception)
            {

                Check();
            }


        }

    }
}
