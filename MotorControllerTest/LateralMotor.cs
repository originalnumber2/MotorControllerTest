using System;

namespace MotorControllerTest
{
    public class LateralMotor : Motor
    {
        //Current Direction of the Lateral Axis True - Out False - In
        internal MotorController Controller;
        internal MotorState State;
        internal Modbus Mbus;
        internal int address = 2;

        public LateralMotor(MotorController motorController)
        {
            Controller = motorController;
            State = Controller.LateralMotorState;
            Mbus = Controller.modbus;
            Address = 3;
        }
    }
}

