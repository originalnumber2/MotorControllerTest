using System;

namespace MotorControllerTest
{
    public class TransverseMotor : Motor
    {
        //current direction of the transverse axis true - Forward false - reverse
        //internal MotorController Controller;
        //internal MotorState State;
        //internal Modbus Mbus;


        public TransverseMotor(MotorController motorController)
        {
            //inherats this from parent
            Controller = motorController;
            State = Controller.TransverseMotorState;
            Mbus = Controller.modbus;
            Address = 2;
           
    }
    }
}
