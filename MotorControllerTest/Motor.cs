using System;
namespace MotorControllerTest
{
    public abstract class Motor
    {

        internal MotorController Controller;
        internal MotorState State;
        internal Modbus Mbus;
        internal int Address;

        //protected Motor(MotorController motorController, MotorState state, int address)
        //{
        //    Controller = motorController;
        //    State = state;
        //    Mbus = Controller.modbus;
        //    Address = address;
        //}

        internal void Connect()
        {

            if (Mbus.WriteModbusQueue(Address, 1798, 1, true))
            {
                Mbus.WriteModbusQueue(Address, 0x0300, 04, false);//give motor master frequency control to rs-485
                Mbus.WriteModbusQueue(Address, 0x0301, 03, false);//give motor Source of operation command  to rs-485
                Mbus.WriteModbusQueue(Address, 0x010D, 0, false);//set motor direcction (Fwd/Rev) to be controled by rs-485
                Mbus.WriteModbusQueue(Address, 0x0705, 0, false);//set speed to zero
                State.SetCon(true);
            }
            else
            {
                //MesQue.WriteMessageQueue("Connection to Traverse Motor Failed");
                State.SetCon(false);
            }
        }

        internal void Disconnect()
        {
            Mbus.WriteModbusQueue(Address, 0x0706, 1, false);
            State.SetCon(false);
        }

        public void ConnectionToggle()
        {
            if (State.IsCon)
            {
                Disconnect();
            }
            else Connect();
        }

        public void Move(double velocity, double scale)
        {
            if (State.IsSimulinkControl)
            {
                MoveSimulink();
            }
            MoveModbus(velocity, scale);
        }

        //This function moves the Lateral Motor checking for direction and speed changes
        internal void MoveModbus(double velocity, double scale)
        {
            bool change, dir;
            double speed;
            (change, dir, speed) = State.ChangeVelocity(velocity);
            if (change)
            {
                double LatinRPM = speed * scale; // convert IPM to RPM
                double hz = LatinRPM / 60.0; // Convert RPM to HZ
                Mbus.WriteModbusQueue(Address, 0x0705, ((int)(hz * 10)), false);
                if (dir) { MovePosModbus(); }
                else MoveNegModbus();
            }

        }

        //Slow Movement Commands Forward Using Modbus
        private void MovePosModbus()
        {
            if (!State.IsCon)
            {
                //MessageBox.Show("Traverse not connected");
            }
            else
            {
                Mbus.WriteModbusQueue(Address, 0x0706, 0x22, false);
            }
        }

        //Slow Movement Commands Reverse Using Modbus
        private void MoveNegModbus()
        {
            if (!State.IsCon)
            {
                //MessageBox.Show("Traverse not connected");
            }
            else
            {
                Mbus.WriteModbusQueue(Address, 0x0706, 0x12, false);
            }
        }

        internal void MoveSimulink()
        {
            throw new NotImplementedException();
        }

        //Sends the command stop the motor if it is moving. Prevents spamming of the modbus
        public void Stop()
        {
            if (State.IsMoving)
            {
                Hault();
            }
        }

        //Sends the command to hault the motor regardless of state;
        internal void Hault()
        {
            Mbus.WriteModbusQueue(Address, 0x0706, 1, false);
            State.SetCon(false);
        }
    }
}
