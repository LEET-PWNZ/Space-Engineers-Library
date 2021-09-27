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
        /*
  *   R e a d m e
  *   -----------
  *   This script can be used as-is, and it is simple to understand and modify. Simply upload it to a Programmable block and compile. (No timer block needed)
  *   It automatically closes all the doors on the local grid, after a specified period (default 3 secs) has elapsed from the moment the door was opened.
  *   
  *   If you want to increase / decrease the close delay, pass a 2nd parameter of type int, to the constructor of the 'DoorManager' class (or just change the hard-coded value). The default is 3 seconds. Eg: `doorManager = new DoorManager(this,5);` This will set the close delay to 5 seconds.
  *   If you want the script to auto-close doors on connected grids as well, then pass a 3rd parameter of type bool, to the constructor of the 'DoorManager' class (or just change the hard-coded value). The default is false. Eg: `doorManager = new DoorManager(this,3,true);` This will also auto-close doors on connected grids.
  *   The bulk of this script was put into a class so that it can be easily combined with existing scripts.
  *   
  *   And that is all there is to it. Enjoy your auto-closing doors!
  *   
  *   Author: Stuyvenstein
  *   
  *   Thanks to malware-dev for the awesome MDK!
  *   
  */
        private DoorManager doorManager;

        public Program()
        {
            doorManager = new DoorManager(this);
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
        }

        public void Save()
        {

        }

        public void Main(string argument)
        {
            doorManager?.Run();
        }

        
    }
}
