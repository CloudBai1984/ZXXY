using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WebSocket4Net;

namespace XYJClient.Robots
{

    public class Robot
    {
        public bool IsRuning
        { get; set; }
        public bool IsContinue
        { get; set; }
        public event GeneralDelegate SendMessageEvent;
        public event GeneralDelegate SendQuickMessageEvent;
        public event GeneralDelegate2 SendWaitMessageEvent;
        internal FightController fight;
        internal Robot()
        {
          
            fight = FightController.GetInstance();
        }

        internal void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var e1 = (MessageReceivedEventArgs)e;
            new Thread(() =>
            {
                Parser(e1.Message);

            }).Start();

        }


        internal virtual void Parser(string message)
        {
            if (Regex.IsMatch(message, "在这里不能使用这条指令") || Regex.IsMatch(message, "这里不能施用外功"))
            {
                fight.FightComplete();
            }
        }

        public virtual void Run()
        {
            IsContinue = true;
            IsRuning = true;
            new Thread(() =>
            {
                while (IsRuning)
                {
                    CheckCurrentStatus();
                    Thread.Sleep(1000);
                }

            }).Start();
        }

        public virtual void CheckCurrentStatus()
        {

        }

        public virtual void Complete()
        {
            IsRuning = false;
        }

        public virtual void Stop()
        {
        }

        public string[] Send(string message)
        {
            string[] msgs = (string[])SendMessageEvent(message);
            return msgs;
        }

        public void SendQuickMessage(string message)
        {
            SendQuickMessageEvent(message);
        }

        public void SendQuickMessage(string message, int waittime)
        {
            SendQuickMessageEvent(message);
            Thread.Sleep(waittime);
        }

        public string[] 
            SendWaitMessage(string message, int waittime)
        {
            string[] msgs = (string[])SendWaitMessageEvent(message, waittime);
            return msgs;
        }
    }
}
