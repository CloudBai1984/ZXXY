using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYJClient.Robots
{
    public class RobotFactory
    {
        public static Robot GetRobot(string robotName)
        {
            Robot robot = null;
            switch(robotName)
            {
                case "JJC":
                    robot = JJCRobot.GetInstance();
                    break;
                case "DFFB":
                    robot = DFFBRobot.GetInstance();
                    break;
                case "MY":
                    robot = MYRobot.GetInstance();
                    break;
                default:
                    robot = JJCRobot.GetInstance();
                    break;
            }
            return robot;
        }
    }
}
