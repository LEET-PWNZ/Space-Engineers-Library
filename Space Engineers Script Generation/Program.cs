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
        ///
        /// This is a basic template for writing a Space Engineers script using MDK
        /// It includes an example of a door manager used to automatically close doors after a delayed period on a grid.
        /// You can use this template to build your own scripts using the libraries from the Space Engineers Toolbox project included with this solution
        ///

        private DoorManager doorManager;

        public Program()
        {
            doorManager = new DoorManager(this);

            Runtime.UpdateFrequency = UpdateFrequency.Update10;
        }

        public void Save()
        {
            
        }

        public void Main(string argument, UpdateType updateSource)
        {
            doorManager.Run();
        }
    }
}
