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
    class JJCRobot : Robot
    {
        public string TestCMD
        { get; set; }

        static JJCRobot Instance;

        public static JJCRobot GetInstance()
        {
            if (Instance == null)
                Instance = new JJCRobot();
            return Instance;
        }

        private JJCRobot() : base()
        {
        }

        int turn;


        public override void Run()
        {
            base.Run();
            Send("huanchao");
            Send("move /d/changan/nbridge");
            FightController.GetInstance().CheckStatusNoFighting(this);
            Send("enter_arena");
            turn = 1;
            WaitForFinish();
            Send("ask_reward");
            base.Complete();
            Console.WriteLine("==============================================完成竞技场");
        }


        void WaitForFinish()
        {
            while (turn <= 9)
            {
                Thread.Sleep(1000);
                Send("look");
                if (Global.CURRENT_ROOM.ShortDesc.Contains("泾水桥北") && turn >= 1)
                    break;

            }

        }


        internal override void Parser(string message)
        {

            base.Parser(message);
            if (Regex.IsMatch(message, "免费竞技场机会已用完或正在冷却"))
            {
                IsContinue = false;
            }
            else if (Regex.IsMatch(message, "缓缓走进场地"))
            {
                new Thread(() =>
                {
                    FightController.GetInstance().Fight(this);
                }).Start();

            }
            else if (Regex.IsMatch(message, "你要对谁施展"))
            {
                FightController.GetInstance().FightComplete();
            }
            else if (Regex.IsMatch(message, "轮竞技将于30秒后开始"))
            {
                new Thread(() =>
                {
                    FightController.GetInstance().FightComplete();
                    FightController.GetInstance().CheckStatusNoFighting(this);
                    int.TryParse(Regex.Match(message, @"第.*【 (?<turn>\d+) 】.*30秒后开始").Groups["turn"].Value, out turn);
                    Send("leave");
                }).Start();

            }
            else if (Regex.IsMatch(message, "化作一股青烟散去"))
            {

            }
            else if (Regex.IsMatch(message, "你尚有未兑换的奖励"))
            {
                new Thread(() =>
                {
                    Send("ask_reward");
                    Send("enter_arena");
                }).Start();

            }
            else if (Regex.IsMatch(message, "你已经完成了该级别所有竞技"))
            {
                turn = 10;

            }
            else if (Regex.IsMatch(message, "全身的流光溢彩渐渐消去了"))
            {
                FightController.GetInstance().Buff();

            }
            else if (Regex.IsMatch(message, "拉着你不让走"))
            {

                UtilFunction.DealWithDaoDanGui(this);
                Console.WriteLine("==============================================捣蛋鬼出现了");

            }


        }



    }
}
