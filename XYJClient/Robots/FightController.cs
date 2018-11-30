using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using XYJClient.Model;

namespace XYJClient.Robots
{
    class FightController
    {


        bool IsFighting = false;
        bool isBuff = true;

        static FightController instance;
        private FightController()
        {

        }

        public static FightController GetInstance()
        {
            if (instance == null)
                instance = new FightController();
            return instance;
        }

        public void CheckStatusNoFighting(Robot robot)
        {

            robot.SendQuickMessage("look",1000);
            if (PlayerStatus.Kee < 70 || PlayerStatus.Sen < 70)
            {
                Console.WriteLine("=========================================================休息中气血或精神过低，开始治疗！气血是" + PlayerStatus.Kee + ";精神是" + PlayerStatus.Sen);
                Thread.Sleep(Global.BUSY_TIME * 1000);
                robot.SendQuickMessage("exert xiaobutian");
            }
            //MyPal player = Global.MYPAL_LIST.Where(x => x.UserId == Global.USER_ID).First<MyPal>();
            //string[] status = player.Name.Split('&');
            //if (int.Parse(status[2]) < 60 || int.Parse(status[4]) < 60)
            //{
            //    Console.WriteLine("=========================================================休息中气血或精神过低，开始治疗！气血是" + status[2] + ";精神是" + status[4]);
            //    Thread.Sleep(Global.BUSY_TIME * 1000);
            //    robot.SendQuickMessage("exert xiaobutian");
            //}
            if (PlayerStatus.Neili < 3000)
            {
                Thread.Sleep(Global.BUSY_TIME * 1000);
                robot.SendQuickMessage("fullrecover all");
            }

        }


        public void Buff()
        {
            isBuff = false;
        }


        public bool CheckIsFighting()
        {
            return IsFighting;
        }


        public void Fight(Robot robot)
        {
            if (IsFighting)
            {
                while (IsFighting)
                {
                    Thread.Sleep(1000);
                }
                return;
            }
            bool isFaXi = false;
            if (Global.INV_LIST.Exists(inv => inv.Name.Contains("妖§"))) isFaXi = true;
            IsFighting = true;
            robot.SendQuickMessage("exert stone");
            if (!isFaXi)
                robot.SendQuickMessage("perform2 dunnoname", 1000);
            else
                robot.SendQuickMessage("cast gold", 1000);
            string target = "";
            int num = 0;
            while (IsFighting)
            {
                if (Global.INV_LIST.Exists(inv => inv.Name.Contains("妖§"))) isFaXi = true;
                if (!isFaXi)
                    robot.SendQuickMessage("perform2 dunnoname", 1000);
                else
                    robot.SendQuickMessage("cast gold", 1000);
                CheckStatusInFighting(robot);

                if (Global.INV_LIST != null && Global.INV_LIST.Count > 1 && (string.IsNullOrEmpty(target) || Global.INV_LIST.Where<Inv>(x => x.Id == target).ToList().Count == 0))
                {
                    target = Global.INV_LIST[0].Id; ;
                    robot.SendQuickMessage("settarget " + target);
                    Console.WriteLine("=========================================================狂撸一个人，这个人就是" + target);
                }


                if (num % 10 == 0)
                    robot.SendQuickMessage("moxingdafa");
                num++;

            }

        }

        public void FightComplete()
        {
            IsFighting = false;
        }

        public void CheckStatusInFighting(Robot robot)
        {

            if (PlayerStatus.Kee < 30 || PlayerStatus.Sen < 30)
            {
                Console.WriteLine("=========================================================要挂了开嗜血！气血是" + PlayerStatus.Kee + ";精神是" + PlayerStatus.Sen);
                robot.SendQuickMessage("shixue", 1000);
                robot.SendQuickMessage("look", 1000);


            }
            if (PlayerStatus.Kee < 20 || PlayerStatus.Sen < 20)
            {
                Console.WriteLine("=========================================================嗜血都不管用了吃行军丹了。气血是" + PlayerStatus.Kee + ";精神是" + PlayerStatus.Sen);
                robot.SendQuickMessage("eat xingjun dan", 100);
            }

            if (PlayerStatus.Kee < 50 || PlayerStatus.Sen < 50)
            {
                Console.WriteLine("=========================================================战斗中气血或精神过低，开始治疗！气血是" + PlayerStatus.Kee + ";精神是" + PlayerStatus.Sen);
                robot.SendQuickMessage("eat jinchuang yao", 100);
                robot.SendQuickMessage("eat jinchuang yao", 100);
                robot.SendQuickMessage("eat jinchuang yao", 100);
                robot.SendQuickMessage("eat hunyuan dan", 100);
                robot.SendQuickMessage("eat hunyuan dan", 100);
                robot.SendQuickMessage("eat hunyuan dan", 100);
                robot.SendQuickMessage("exert ningqi", 100);
                robot.SendQuickMessage("exert ningqi", 100);
                robot.SendQuickMessage("exert ningqi", 100);
            }
            if (PlayerStatus.Neili < 500)
            {
                Console.WriteLine("=========================================================内力过低，开始补充！内力过低：" + PlayerStatus.Neili);
                robot.SendQuickMessage("cast transfer", 100);
                robot.SendQuickMessage("cast transfer", 100);
                robot.SendQuickMessage("look", 100);
            }
            if (PlayerStatus.Fali * 1.0 / PlayerStatus.MaxFali < 0.3)
            {
                Console.WriteLine("========================================================法力不够了吃人参了");
                robot.SendQuickMessage("eat ren shen", 100);
            }
            if (!isBuff)
            {
                Console.WriteLine("=========================================================开始buff了");
                robot.SendQuickMessage("exert stone", 100);
                robot.SendQuickMessage("exert stone", 100);
                robot.SendQuickMessage("exert stone", 100);
                isBuff = true;
            }

        }

    }
}
