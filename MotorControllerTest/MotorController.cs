using System;
using System.Threading;

namespace MotorControllerTest
{
public class MotorController
    {
        //Connection Interfaces
        internal Modbus modbus;
        //internal UDPCom UDPCom;

        //Motors
        VerticalMotor verticalMotor;
        TransverseMotor transverseMotor;
        LateralMotor lateralMotor;
        SpindleMotor spindleMotor;



        //internal Controller Controller;

        internal Mutex commandMutex;

        //State Variables of the controller
        internal MotorState VerticleMotorState;
        internal MotorState TransverseMotorState;
        internal MotorState LateralMotorState;
        internal MotorState SpindleMotorState;

        //not completely happy with this constructor
        //public MotorController(Controller controller)
        public MotorController()
        {
            //creation of communication protocals
            modbus = new Modbus();
            //UDPCom = new UDPCom();

            //Controller = controller;

            //Motor States
            VerticleMotorState = new MotorState(5, 0.00001, 0.00001);
            TransverseMotorState = new MotorState(15, 1, 0.1);
            LateralMotorState = new MotorState(5, 1, 0.1);
            SpindleMotorState = new MotorState(2000, 0, 10);

            //creating the verticle motor, it communicates over USB RS424
            verticalMotor = new VerticalMotor(this);

            //creating the transverse motor, it communicatates over UDP or Modbus
            transverseMotor = new TransverseMotor(this);

            //creating the Lateral motor, it communicatates over UDP or Modbus
            lateralMotor = new LateralMotor(this);

            //creating the spindle motor, it communicatates over Modbus
            spindleMotor = new SpindleMotor(this);

        }

        //Continues to attempt to connect all motors
        internal void ConnectAll()
        {
            bool isConnected = false;
            while (!isConnected)
            {
                lateralMotor.Connect();
                transverseMotor.Connect();
                verticalMotor.Connect();
                spindleMotor.Connect();
                isConnected = LateralMotorState.IsCon || TransverseMotorState.IsCon || VerticleMotorState.IsCon || SpindleMotorState.IsCon;
            }
        }

        internal void DisconnectAll()
        {
            bool isConnected = true;
            while (isConnected)
            {
                lateralMotor.Disconnect();
                transverseMotor.Disconnect();
                verticalMotor.Disconnect();
                spindleMotor.Disconnect();
                isConnected = LateralMotorState.IsCon || TransverseMotorState.IsCon || VerticleMotorState.IsCon || SpindleMotorState.IsCon;
            }
        }

        internal void StopAll()
        {
            transverseMotor.Stop();
            lateralMotor.Stop();
            verticalMotor.Stop();
            spindleMotor.Stop();
        }

        //toggles to simulink control
        void SetInterfaceToggle(bool connection)
        {
            TransverseMotorState.IsSimulinkControl = connection;
            LateralMotorState.IsSimulinkControl = connection;
            VerticleMotorState.IsSimulinkControl = connection;
            SpindleMotorState.IsSimulinkControl = connection;
        }

        internal void MoveVertical(double velocity)
        {
            verticalMotor.Move(velocity);
        }

        internal void MoveLateral(double velocity)
        {
            lateralMotor.Move(velocity, 54.5);
        }

        internal void MoveTransverse(double velocity)
        {
            transverseMotor.Move(velocity, 171.594);
        }

        internal void MoveSpindle(double velocity)
        {
            spindleMotor.Move(velocity, 1);

        }

        internal void StopVertical()
        {
            verticalMotor.Stop();
        }

        internal void StopLateral()
        {
            lateralMotor.Stop();
        }

        internal void StopTransverse()
        {
            transverseMotor.Stop();
        }

        internal void StopSpindle()
        {
            spindleMotor.Stop();
        }

    }
}
