using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IngameScript
{
    public class SolarPanelManager
    {
        private MyGridProgram solarParentProgram;
        private Func<IMySolarPanel, bool> blockFilter;
        private List<IMySolarPanel> solarPanels = new List<IMySolarPanel>();
        public static float PanelBaseMw = 160f/1000f;
        public float PanelPotentialMw=0f,TotalDrainMw=0f;
        public int WorkingPanelCount=0;
        public float PanelEfficiency { get { return (PanelPotentialMw / PanelBaseMw) * 100f; } }

        public SolarPanelManager(MyGridProgram inParentProgram, Func<IMySolarPanel, bool> filter=null)
        {
            solarParentProgram = inParentProgram;
            blockFilter = filter;
            UpdateSolarBlocks();
            UpdateSolarData();
        }

        public void UpdateSolarBlocks()
        {
                solarParentProgram.GridTerminalSystem.GetBlocksOfType<IMySolarPanel>(solarPanels, blockFilter);
        }

        public void UpdateSolarData()
        {
            var workingPanels = solarPanels.Where(p => p.IsWorking).ToList();
            WorkingPanelCount = workingPanels.Count;
            if (WorkingPanelCount > 0)
            {
                TotalDrainMw = workingPanels.Sum(p => p.CurrentOutput);
                PanelPotentialMw= workingPanels.Sum(p => p.MaxOutput) / WorkingPanelCount;
            }
        }

    }
}
