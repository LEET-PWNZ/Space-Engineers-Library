using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Text;

namespace IngameScript
{
    public class EnergyDiagSystem
    {
        private readonly MyGridProgram _parentProgram;
        public const string ScreenName = "Energy Info LCD";
        public ScreenManager energyScreen;
        public SolarPanelManager solarPanelManager;
        public BatteryManager batteryManager;
        public EnergyDiagSystem(MyGridProgram parentProgram, TickEventManager tickEventManager)
        {
            _parentProgram = parentProgram;
            energyScreen = new ScreenManager(_parentProgram,ScreenName);
            solarPanelManager = new SolarPanelManager(_parentProgram);
            batteryManager = new BatteryManager(_parentProgram, b => b.CubeGrid == _parentProgram.Me.CubeGrid);
            tickEventManager.OnEventTick += diagTick;
            tickEventManager.OnEventTick10 += diagTick10;
            tickEventManager.OnEventTick100 += diagTick100;
        }

        private void diagTick()
        {
            solarPanelManager.UpdateSolarData();
            batteryManager.UpdateBatteryData();
            energyScreen.UpdateScreen();
        }

        private void diagTick10()
        {
            var totalSolarPotential = solarPanelManager.PanelPotentialMw * solarPanelManager.WorkingPanelCount;
            var solarBaseMax = SolarPanelManager.PanelBaseMw * solarPanelManager.WorkingPanelCount;
            var powerInfo = "Solar Efficiency: "+ totalSolarPotential.ToString("0.00") + " MW / " + solarBaseMax.ToString("0.00") + " MW (" + solarPanelManager.PanelEfficiency.ToString("0") + "%)";
            powerInfo += "\nSolar Drain: " + solarPanelManager.TotalDrainMw.ToString("0.00") + " MW / " + totalSolarPotential.ToString("0.00") + " MW";
            powerInfo += "\nWorking Panel Count: " + solarPanelManager.WorkingPanelCount.ToString();
            powerInfo += "\nBattery Count: " + batteryManager.WorkingBatteryCount.ToString();
            powerInfo += "\nBattery Capacity: " + batteryManager.TotalCapacityMw.ToString("0.00") + "MW / " + batteryManager.MaxCapacityMw.ToString("0.00") + "MW (" + ((batteryManager.TotalCapacityMw / batteryManager.MaxCapacityMw) * 100f).ToString("0")+"%)";
            powerInfo += "\nBattery Charge: " + batteryManager.TotalChargeMw.ToString("0.00") + "MW / " + batteryManager.MaxChargeMw.ToString("0.00") + "MW";
            powerInfo += "\nBattery Discharge: " + batteryManager.TotalDrainMw.ToString("0.00") + "MW / " + batteryManager.MaxDrainMw.ToString("0.00") + "MW";
            energyScreen.WriteAllText(powerInfo);
        }

        private void diagTick100()
        {
            solarPanelManager.UpdateSolarBlocks();
            batteryManager.UpdateBatteryBlocks();
            energyScreen.UpdateScreenBlock();
        }

    }
}
