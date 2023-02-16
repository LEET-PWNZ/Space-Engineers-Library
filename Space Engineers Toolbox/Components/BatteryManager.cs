using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngameScript
{
    public class BatteryManager
    {
        private MyGridProgram batteryParentProgram;
        Func<IMyBatteryBlock, bool> blockFilter;
        private List<IMyBatteryBlock> batteries = new List<IMyBatteryBlock>();
        public static float BaseCapacityMw = 3f;
        public float TotalCapacityMw = 0f, TotalChargeMw = 0f, TotalDrainMw = 0f, MaxChargeMw = 0f, MaxDrainMw = 0f;
        public int WorkingBatteryCount=0;

        public float MaxCapacityMw { get { return BaseCapacityMw * WorkingBatteryCount; } }

        public BatteryManager(MyGridProgram inParentProgram, Func<IMyBatteryBlock, bool> filter = null)
        {
            batteryParentProgram = inParentProgram;
            blockFilter = filter;
            UpdateBatteryBlocks();
            UpdateBatteryData();
        }


        public void UpdateBatteryBlocks()
        {
                batteryParentProgram.GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(batteries, blockFilter);
        }

        public void UpdateBatteryData()
        {
            var workingBats = batteries.Where(b => b.IsWorking).ToList();
            WorkingBatteryCount = workingBats.Count;
            if (WorkingBatteryCount > 0)
            {
                TotalCapacityMw = workingBats.Sum(b => b.CurrentStoredPower);
                TotalDrainMw = workingBats.Sum(b => b.CurrentOutput);
                TotalChargeMw = workingBats.Sum(b => b.CurrentInput);
                MaxChargeMw = workingBats.Sum(b => b.MaxInput);
                MaxDrainMw = workingBats.Sum(b => b.MaxOutput);
            }
        }

    }
}
