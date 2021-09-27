using IngameScript.Classes;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        private ScreenManager consoleScreen;
        private TickEventManager tickEventManager;
        private static float sunSpeedRpm = 1f / 120f;
        private EnergyDiagSystem energyDiagSystem;

        private IMyMotorStator sunTrackRotor;
        private IMyMotorAdvancedStator hingeH,hingeV;
        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            tickEventManager = new TickEventManager();
            energyDiagSystem = new EnergyDiagSystem(this, tickEventManager);
            tickEventManager.OnEventTick += tmpTick;
            tickEventManager.OnEventTick10 += TickEventManager_OnEventTick10;
            tickEventManager.OnEventTick100 += TickEventManager_OnEventTick100;
            consoleScreen = new ScreenManager(this, "Console LCD");
            UpdateBlocks();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            tickEventManager.UpdateTickManager();

        }

        private void tmpTick()
        {
            if (sunTrackRotor != null && sunTrackRotor.IsWorking )
            {
                consoleScreen.UpdateScreen();
                var rotorSpeed = 0f;
                if (sunTrackRotor.Angle.ToString("0") != 0.ToString("0"))
                {
                    if (sunTrackRotor.Angle < 0f) rotorSpeed = 2f;
                    else rotorSpeed = -2f;
                }
                sunTrackRotor.TargetVelocityRPM = rotorSpeed;
            }
            var angleRadian = 0.0174533f;
            var hingeSpeed = 0.3f;
            if (hingeH != null && hingeH.IsWorking)
            {
                var horAngle = -(35f * angleRadian);
                var hspeed = 0f;
                if (hingeH.Angle != horAngle)
                {
                    if (hingeH.Angle < horAngle) hspeed = hingeSpeed;
                    else hspeed = -hingeSpeed;
                }
                hingeH.TargetVelocityRPM = hspeed;
            }
            if (hingeV != null && hingeV.IsWorking)
            {
                var verAngle = (35f * angleRadian);
                var vspeed = 0f;
                if (hingeV.Angle != verAngle)
                {
                    if (hingeV.Angle < verAngle) vspeed = hingeSpeed;
                    else vspeed = -hingeSpeed;
                }
                hingeV.TargetVelocityRPM = vspeed;
            }
        }

        private void TickEventManager_OnEventTick()
        {
            consoleScreen.UpdateScreen();
            float rotorSpeedRpm=sunSpeedRpm;
            if (energyDiagSystem.solarPanelManager.PanelEfficiency  < 95f)
            {
                rotorSpeedRpm = 2f;
            }
            rotorSpeedRpm = sunSpeedRpm;
            if (sunTrackRotor!=null&& sunTrackRotor.IsWorking && sunTrackRotor.TargetVelocityRPM != rotorSpeedRpm)
            {
                sunTrackRotor.TargetVelocityRPM = rotorSpeedRpm;
            }
            var angleRadian = 0.0174533f;
            var hingeSpeed = 0.3f;
            if (hingeH!=null &&hingeH.IsWorking)
            {
                var horAngle = -(45f* angleRadian);
                var hspeed = 0f;
                if(hingeH.Angle != horAngle)
                {
                    if (hingeH.Angle < horAngle) hspeed = hingeSpeed;
                    else hspeed = -hingeSpeed;
                }
                hingeH.TargetVelocityRPM = hspeed;
            }
            if (hingeV != null && hingeV.IsWorking)
            {
                var verAngle = (45f* angleRadian);
                var vspeed = 0f;
                if (hingeV.Angle != verAngle)
                {
                    if (hingeV.Angle < verAngle) vspeed = hingeSpeed;
                    else vspeed = -hingeSpeed;
                }
                hingeV.TargetVelocityRPM = vspeed;
            }
        }

        private void TickEventManager_OnEventTick10()
        {
            var consoleOutput = sunTrackRotor.Angle.ToString("0.00");
            if (hingeH != null && hingeH.IsWorking)
            {
                consoleOutput += "\nHinge H " + hingeH.Angle.ToString("0.00");
            }
            if (hingeV != null && hingeV.IsWorking)
            {
                consoleOutput += "\nHinge V " + hingeV.Angle.ToString("0.00");
            }
            consoleScreen.WriteAllText(consoleOutput);
        }

        private void TickEventManager_OnEventTick100()
        {
            UpdateBlocks();

            consoleScreen.UpdateScreenBlock();
        }

        private void UpdateBlocks()
        {
            sunTrackRotor = GridTerminalSystem.GetBlockWithName("Sun Rotor") as IMyMotorStator;
            hingeH= GridTerminalSystem.GetBlockWithName("Solar Hinge H") as IMyMotorAdvancedStator;
            hingeV = GridTerminalSystem.GetBlockWithName("Solar Hinge V") as IMyMotorAdvancedStator;
        }

        public void Save()
        {

        }

    }
}
