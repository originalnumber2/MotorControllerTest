using System;

namespace MotorControllerTest
{
    // Simplificatin and lubrication control is needed. if the spindle is going to fast the lube is required.
    public class SpindleMotor: Motor
    {

        //Spindle direction true - Clockwise false - counter clockwise

        public SpindleMotor(MotorController motorController) :
            base(motorController, motorController.SpindleMotorState, 1)
        {
        }


        internal new void Connect()
        {
            //attempt to sync with motor
            //int speed = (int)((double)nmSpiRPM.Value * 2.122);
            //int speed = (int)((double)nmSpiRPM.Value * 3.772);
            //int speed = (int)((double)nmSpiRPM.Value * 3.7022); //Adjusted by BG and CC on 9/7/12
            if (Mbus.WriteModbusQueue(1, 2000, 0, true))
            {
                State.IsCon = true;
                //message = "Spindle Connected";
            }
            else
                State.IsCon = false;
                //message = "Spindle Failed to connect";
        }

        private new void Disconnect()
        {
            Mbus.WriteModbusQueue(1, 2000, 0, false);
            State.IsCon = false;
        }

        //do not need if i can find out how to not use the motor version of MoveModbus
        public new void Move(double velocity, double scale)
        {
            if (State.IsSimulinkControl)
            {
                MoveSimulink();
            }
            MoveModbus(velocity, scale);
        }

        //This function moves the Lateral Motor checking for direction and speed changes
        internal new void MoveModbus(double velocity, double scale)
        {
            bool change, dir;
            double speed;
            (change, dir, speed) = State.ChangeVelocity(velocity);
            if (change)
            {
                //old pulley speed
                // int speed = (int)((double)nmSpiRPM.Value * 1.58);
                // int speed = (int)(RPM * 2.122);
                //int speed = (int)(RPM * 3.7022); //Adjusted by BG and CC 9/7/12
                Mbus.WriteModbusQueue(1, 2002, (int)(speed * 3.772), false);
                if (dir) { MovePosModbus(); }
                else MoveNegModbus();
            }

        }

        private void MovePosModbus()
        {
            if (State.IsCon)
            {
                Mbus.WriteModbusQueue(1, 2000, 1, false);
                State.IsMoving = true;
            }
        }

        private void MoveNegModbus()
        {
            if (State.IsCon)
            {
                Mbus.WriteModbusQueue(1, 2000, 3, false);
                State.IsMoving = true;
            }
        }

        //Sends the command stop the motor if it is moving. Prevents spamming of the modbus
        //dont need if i can find out how to not use the motor version of hault
        public new void Stop()
        {
            if (State.IsMoving)
            {
                Hault();
                State.IsMoving = false;
                State.Speed = 0;
            }
        }

        public new void Hault()//Stop the Spindle Motor
        {
            //Stop the spindle
            Mbus.WriteModbusQueue(1, 2000, 0, false);
        }

        //private bool ChangeDir(bool dir)
        //{
        //    if (dir = SpiDir)
        //    {
        //        return false;
        //    }
        //    SpiDir = dir;
        //    return true;
        //}

        //private bool ChangeRPM(Double RPM)
        //{
        //    //allow for checking of maximum speeds and insure IPM is positive
        //    double CheckRPM = Math.Abs(RPM);
        //    if (CheckRPM > RPMmax)
        //    {
        //        CheckRPM = RPMmax;
        //    }
        //    else
        //    {
        //        if (CheckRPM < RPMmin)
        //        {
        //            CheckRPM = RPMmin;
        //        }

        //    }
        //    if (Math.Abs(CheckRPM - SpiRPM) > epsilon)
        //    {
        //        SpiRPM = CheckRPM;
        //        //int speed = (int)(RPM * 3.7022); //Adjusted by BG and CC 9/7/12
        //        controller.WriteModbusQueue(1, 2002, (int)(SpiRPM * 3.772), false);
        //        return true;
        //    }
        //    return false;
        //}



        //public void MoveModbus(double RPM, bool dir)
        //{
        //    if (ChangeDir(dir) || ChangeRPM(RPM))
        //    {
        //        if (SpiDir) { MoveCCModbus() }
        //        else MoveCCWModbus();
        //    }

        //}

        //internal void SpindleUDPControl()
        //{
            //if (isSpiSpeedCW)
            //{
            //    StartSpiCW();
            //    isSpiCW = true;
            //}
            //else
            //{
            //    StartSpiCCW();
            //    isSpiCW = false;
            //}
            //SpiSpeed[0] = -99.9;
            //SpiSpeed[1] = SpiSpeed[0];
            //SpiSpeed[0] = BitConverter.ToDouble(RecieveBytes, 8);
            //if (SpiSpeed[0] != SpiSpeed[1])
            //{
            //    isSpiSpeedCW = trueifpositive(SpiSpeed[0]);
            //    SpiSpeedMagnitude = Math.Abs(SpiSpeed[0]);
            //    if (SpiSpeedMagnitude > 100)
            //        isLubWanted = true;
            //    else
            //        isLubWanted = false;
            //    //Limit Spindle speed
            //    if (SpiSpeedMagnitude > SpiSpeedLimit) SpiSpeedMagnitude = SpiSpeedLimit;
            //    if (isSpiSpeedCW)
            //    {
            //        if (!isSpiCW)
            //        {
            //            StartSpiCW();
            //            isSpiCW = true;
            //        }
            //        SpiMessage = "CW";
            //    }
            //    else
            //    {
            //        if (isSpiCW)
            //        {
            //            StartSpiCCW();
            //            isSpiCW = false;
            //        }
            //        SpiMessage = "CCW";
            //    }
            //    ChangeSpiRef(SpiSpeedMagnitude);
            //    WriteMessageQueue("Spi set to:" + SpiSpeedMagnitude.ToString("F0") + SpiMessage);

            //}
        //}
    }
}
