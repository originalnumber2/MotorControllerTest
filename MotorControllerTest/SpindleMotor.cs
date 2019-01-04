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


        private new void Connect()
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

        private void MovePosModbus()
        {
            if (State.IsCon)
            {
                Mbus.WriteModbusQueue(1, 2000, 1, false);
            }
        }

        private void MoveNegModbus()
        {
            if (State.IsCon)
            {
                Mbus.WriteModbusQueue(1, 2000, 1, false);
            }
        }

        public new void Hault()//Stop the Spindle Motor
        {
            //Stop the spindle
            Mbus.WriteModbusQueue(1, 2000, 0, false);
            State.IsMoving = false;
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
