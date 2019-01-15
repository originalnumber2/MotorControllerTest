using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

            controller.MoveLateral(-1);
            Thread.Sleep(5000);
            controller.StopLateral();
            Thread.Sleep(5000);
            controller.MoveTransverse(-1);
            Thread.Sleep(5000);
            controller.StopTransverse();
            Thread.Sleep(5000);
            controller.MoveLateral(1);
            Thread.Sleep(5000);
            controller.StopLateral();
            Thread.Sleep(5000);
            controller.MoveTransverse(1);
            Thread.Sleep(5000);
            controller.StopTransverse();
            Thread.Sleep(5000);
            controller.MoveSpindle(100);
            Thread.Sleep(5000);
            controller.StopSpindle();
            Thread.Sleep(5000);
            controller.MoveSpindle(-100);
            Thread.Sleep(5000);
            controller.StopSpindle();
            Thread.Sleep(5000);
            controller.MoveVertical(1);
            Thread.Sleep(5000);
            controller.StopVertical();
            Thread.Sleep(5000);
            controller.MoveVertical(-1);
            Thread.Sleep(5000);
            controller.StopVertical();

            //do
            //{
            //if (Console.KeyAvailable)
            //{
            //c = Console.ReadKey().Key.ToString();
            //}

            //if (c == ConsoleKey.DownArrow.ToString())
            //{
            //controller.MoveLateral(1);
            //Console.WriteLine("Move Lateral");
            //}
            //else if (c == ConsoleKey.UpArrow.ToString())
            //{
            //controller.MoveLateral(-1);
            //Console.WriteLine("Move Lateral");
            //}
            //else
            //{
            //controller.StopLateral();
            //Console.WriteLine("Stop Lateral");
            //}

            //if (c == ConsoleKey.LeftArrow.ToString())
            //{
            //controller.MoveTransverse(1);
            //Console.WriteLine("Move Transverse");
            //}
            //else if (c == ConsoleKey.RightArrow.ToString())
            //{
            //controller.MoveTransverse(-1);
            //Console.WriteLine("Move Transverse");
            //}
            //else
            //{
            //controller.StopTransverse();
            //}

            //if (c == ConsoleKey.Insert.ToString())
            //{
            //controller.MoveVertical(1);
            //Console.WriteLine("Move Verticle");
            //}
            //else if (c == ConsoleKey.Delete.ToString())
            //{
            //controller.MoveVertical(-1);
            //Console.WriteLine("Move Verticle");
            //}
            //else
            //{
            //controller.StopVertical();
            //}

            //if (c == ConsoleKey.Home.ToString())
            //{
            //controller.MoveSpindle(100);
            //Console.WriteLine("Move Spindle");
            //}
            //else if (c == ConsoleKey.End.ToString())
            //{
            //controller.MoveSpindle(-100);
            //Console.WriteLine("Move Spindle");
            //}
            //else 
            //{
            //controller.StopSpindle();
            //}


            //} while (c != ConsoleKey.Enter.ToString());
            controller.StopAll();
            controller.DisconnectAll();
        }
    }
}
