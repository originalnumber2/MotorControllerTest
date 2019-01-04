using System;

namespace MotorControllerTest
{
    public class TransverseMotor : Motor
    {
        //current direction of the transverse axis true - Forward false - reverse
        //internal MotorController Controller;
        //internal MotorState State;
        //internal Modbus Mbus;


        public TransverseMotor(MotorController motorController) :
        base(motorController, motorController.TransverseMotorState, 2)
        {

        }
    }
}
