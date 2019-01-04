using System;
using System.IO.Ports;
using System.Threading;

namespace MotorControllerTest
{
    public class VerticalMotor : Motor
    {

        //bools dealing with the state of connection of the motor communication
        public bool isVerContinous;
        public double VerAccel;

        //serial port for communicating with the vertical motor
        SerialPort VerPort;

        //motor state variables
        // true is the vertical motor down
        int VerCount;
        int verTurns;




        public VerticalMotor(MotorController controller) :
            base(controller, controller.VerticleMotorState, 4)
        {
            //need to do something about the modbus UDP problem and bring together
            isVerContinous = true;
            VerCount = 0; //Number of rotations to preform
            verTurns = 0;

            #region Speed Control of the Verticle motor
            VerAccel = 10;
            # endregion

            Controller = controller;
            State = Controller.VerticleMotorState;

            #region Setting up the serial communication port
            VerPort = new SerialPort
            {
                BaudRate = 9600,
                PortName = "COM5",
                DataBits = 8,
                Parity = System.IO.Ports.Parity.None,
                StopBits = System.IO.Ports.StopBits.One
            };
            VerPort.Open();
            #endregion


        }

        //movement needs to be cleaned up and improved to account for UDP and insure consistency though methods.
        //internal void Move(double IPM, bool dir)
        //{
        //    if (ChangeDir(dir) || ChangeIPM(IPM))
        //    {
        //        if (VerDir) { LowerTableModbus(); }
        //        else RaiseTableModbus();
        //    }
        //}

        //private bool ChangeIPM(double IPM)
        //{
        //    //allow for checking of maximum speeds and insure IPM is positive
        //    double CheckIPM = Math.Abs(IPM);
        //    if (CheckIPM > VerSpeedMax)
        //    {
        //        CheckIPM = VerSpeedMax;
        //    }
        //    else
        //    {
        //        if (CheckIPM < VerSpeedMin)
        //        {
        //            CheckIPM = VerSpeedMin;
        //        }

        //    }
        //    if (Math.Abs(CheckIPM - VerSpeed) > epsilon)
        //    {
        //        VerSpeed = CheckIPM;
        //        return true;
        //    }
        //    return false;

        //}

        //private bool ChangeDir(bool dir)
        //{
        //    if (dir = VerDir)
        //    {
        //        return false;
        //    }
        //    VerDir = dir;
        //    return true;
        //}

        //Setter for the Vertical Speed
        //internal string SetSpeed(double speed)
        //{
        //    double VerSpeedMagnitude = Math.Abs(speed);
        //    if (VerSpeedMagnitude > VerSpeedMax)
        //        VerSpeedMagnitude = VerSpeedMax;
        //    if (VerSpeedMagnitude < VerSpeedMin)
        //        VerSpeedMagnitude = VerSpeedMin;
        //    VerSpeed = VerSpeedMagnitude;
        //    return "Setting Vertical Speed to " + VerSpeedMagnitude.ToString();
        //}

        public void Move(double velocity)
        {
            if (State.IsSimulinkControl)
            {
                MoveSimulink();
            }
            MoveModbus(velocity);
        }

        internal void MoveModbus(double velocity)
        {
            bool change, dir;
            double speed;
            (change, dir, speed) = State.ChangeVelocity(velocity);
            if (change)
            {
                if (dir) { MovePosModbus(); }
                else MoveNegModbus();
            }

        }

        //Raises the table
        private void MoveNegModbus()
        {
            String VerMessage;
            VerMessage = String.Empty;

            if (isVerContinous)
            {
                VerMessage = "E MC H";
                VerMessage += "+";

                VerMessage += "A" + VerAccel + " V" + State.Speed.ToString() + " G\r";
            }
            else
            {
                VerMessage = "E MN A" + VerAccel + " V" + State.Speed.ToString() + " D";
                VerMessage += verTurns.ToString() + " G\r";
            }

            VerPort.Write(VerMessage);
        }

        //Lowers the table
        private void MovePosModbus()
        {
            String VerMessage;
            VerMessage = String.Empty;

            if (isVerContinous)
            {
                VerMessage = "E MC H";
                VerMessage += "-";
                VerMessage += "A" + VerAccel + " V" + State.Speed.ToString() + " G\r";
            }
            else
            {
                VerMessage = "E MN A" + VerAccel + " V" + State.Speed.ToString() + " D";
                VerMessage += "-";
                VerMessage += verTurns.ToString() + " G\r";
            }

            VerPort.Write(VerMessage);
        }


        public void Hault() // Used with the stop verticle button I dont know what the E does
        {
            VerPort.Write("E S\r");
        }

        public void VertHault() //Stop the vertical Motor
        {
            VerPort.Write("S\r");
        }

        //Connect to the Vertical Motor
        internal new string Connect()
        {
            string VerMessage, responce;
            //Clear out port
            VerPort.ReadExisting(); //this was edited out

            //Query the motor and establish RS-232 control
            VerPort.Write("E ON 1R\r"); //this was edited out
            Thread.Sleep(300); //this was edited out

            //Turn off limits
            VerPort.Write("1LD3\r"); //this was edited out
            Thread.Sleep(30); //this was edited out

            //Initialize Now
            VerPort.Write("1E 1MN 1A10 1V10 1D0 G\r"); //this was edited out
            Thread.Sleep(30); //this was edited out

            //Clear out port and place in holding string
            VerMessage = VerPort.ReadExisting(); //this was edited out

            if (VerMessage.Length < 2) //this was edited out
            {
                responce = "Connection to vertical motor failed";
            }
            else //this was edited out
            {
                //Update the boolean
                State.IsCon = true;
                responce = "Connected to vertical motor";

            }
            return responce;
        }

        //Disconect and turn off the Vertical Motor
        internal new string Disconnect()
        {
            //Turn off the motor for starters
            VerPort.Write("S\r");
            Thread.Sleep(300);
            VerPort.Write("OFF\r");
            State.IsCon = false;
            //show that vertical motor is disconected on gui
            return "Disconnected from vertical motor";
        }

        internal string BeginWeldControl()
        {
            VerPort.Write("E MC"); //I dont know why this is needed;
            VerAccel = 10;
            return "Vertical Motor ready for weld";
        }

        internal string UDPVerticalMove(double speed)
        {
            string VerMessage;
            double VerSpeedMagnitude = Math.Abs(speed);

            if (Math.Abs(State.Speed - speed) > State.epsilon)
            {
                State.Dir = TrueIfPositive(State.Speed);
                VerSpeedMagnitude = Math.Abs(State.Speed);
                State.ChangeVelocity(speed);
                VerMessage = "H";
                if (State.Dir)
                    VerMessage += "+";
                else
                    VerMessage += "-";

                VerMessage += "A" + VerAccel.ToString("F2") + " V" + VerSpeedMagnitude.ToString("F5") + " G\r";
                VerPort.Write(VerMessage);
                return VerMessage;
            }
            return "";
        }

        bool TrueIfPositive(double n)
        {
            if (n >= 0)
                return true;
            return false;
        }
    }
}



