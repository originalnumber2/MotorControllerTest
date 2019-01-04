using System;

namespace MotorControllerTest
{
    public class LateralMotor : Motor
    {
        //Current Direction of the Lateral Axis True - Out False - In
        internal int address = 2;

        public LateralMotor(MotorController motorController):
            base(motorController, motorController.LateralMotorState, 3)
        {
        }
    }
}

