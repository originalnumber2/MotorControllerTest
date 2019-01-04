using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorControllerTest
{
    class Program
    {
        static MotorController controller;

        static void Main(string[] args)
        {
            string c = "";
            controller = new MotorController();

            controller.ConnectAll();

            do
            {
                if (Console.KeyAvailable)
                {
                    c = Console.ReadKey().Key.ToString();

                    if (c == ConsoleKey.DownArrow.ToString())
                    {
                        controller.MoveLateral(1);
                    }
                    else if (c == ConsoleKey.UpArrow.ToString())
                    {
                        controller.MoveLateral(1);
                    }
                    else
                    {
                        controller.StopLateral();
                    }

                    if (c == ConsoleKey.LeftArrow.ToString())
                    {
                        controller.MoveTransverse(1);
                    }
                    else if (c == ConsoleKey.RightArrow.ToString())
                    {
                        controller.MoveTransverse(1);
                    }
                    else
                    {
                        controller.StopTransverse();
                    }

                    if (c == ConsoleKey.Insert.ToString())
                    {
                        controller.MoveVertical(1);
                    }
                    else if (c == ConsoleKey.Delete.ToString())
                    {
                        controller.MoveVertical(1);
                    }
                    else
                    {
                        controller.StopVertical();
                    }

                    if (c == ConsoleKey.Home.ToString())
                    {
                        controller.MoveSpindle(100);
                    }
                    else if (c == ConsoleKey.End.ToString())
                    {
                        controller.MoveSpindle(100);
                    }
                    else 
                    {
                        controller.StopSpindle();
                    }
                }
                else
                {
                    controller.StopAll();
                }
            } while (c != ConsoleKey.Enter.ToString());

            controller.DisconnectAll();
        }
    }
}
