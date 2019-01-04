﻿using System;
namespace MotorControllerTest
{
    public class MotorState
    {
        internal bool IsCon;
        internal bool IsMoving;
        internal bool IsSimulinkControl; // Determines if the motor is under modbus control or not.
        internal double Speed; //Current speed (IPM) of the lateral Axis
        internal bool Dir; //Current Direction of the Lateral Axis True - Out False - In
        internal double Max;
        internal double Min;
        internal double epsilon;

        public MotorState(double MaxSpeed, double MinSpeed, double error)
        {
            IsCon = false;
            IsSimulinkControl = false;
            Speed = 0;
            Dir = true;
            Max = MaxSpeed;
            Min = MinSpeed;
            epsilon = error;
        }

        internal void SetCon(bool conState)
        {
            IsCon = conState;
        }

        public override string ToString()
        {
            string mes = " Connected: " + IsCon.ToString() + " " +
                "Simulink: " + IsSimulinkControl.ToString() + " " +
                "Speed: " + Speed.ToString() + " " +
                "Direction: " + Dir.ToString() + " " +
                "Max Speed: " + Max.ToString() + " " +
                "Min Speed: " + Min.ToString() + " " +
                "epsilon: " + epsilon.ToString();
            return string.Format(mes);
        }


        //this function is the etry point for updating the direction and speed of the motor. true if update is required.
        internal (bool, bool, double) ChangeVelocity(double velocity)
        {
            bool changeDir, changeSpeed, dir;
            double speed;
            (changeDir, dir) = ChangeDir(CheckSign(velocity));
            (changeSpeed, speed) = ChangeSpeed(velocity);

            return (changeDir || changeSpeed, dir, speed);

        }

        private bool CheckSign(double num)
        {
            if(num < 0)
            {
                return false;
            }
            return true;
        }

        //this function checks if the IPM of the Lateral motor changes then changes it. returns true if so
        private (bool, double) ChangeSpeed(double SetSpeed)
        {
            //allow for checking of maximum speeds and insure IPM is positive
            double CheckSpeed = Math.Abs(SetSpeed);
            if (CheckSpeed > Max)
            {
                CheckSpeed = Max;
            }
            else
            {
                if (CheckSpeed < Min)
                {
                    CheckSpeed = Min;
                }

            }
            if (Math.Abs(CheckSpeed - Speed) > epsilon)
            {
                Speed = CheckSpeed;
                return (true, Speed);
            }
            return (false, 0);
        }

        //this function check if the direction of the lateral motor changes. changes it if required. Returns true if so.
        private (bool, bool) ChangeDir(bool dir)
        {
            if (dir = Dir)
            {
                return (false, Dir);
            }
            Dir = dir;
            return (true, Dir);
        }
    }
}
