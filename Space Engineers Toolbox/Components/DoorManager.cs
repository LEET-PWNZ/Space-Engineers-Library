using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IngameScript
{
    public class DoorManager
    {
        private List<AutoDoor> AutoDoors = new List<AutoDoor>();
        private int _CloseDelaySeconds;
        private bool _AffectConnectedGrids;
        private MyGridProgram ParentProgram;
        private int ElapsedTicks = 0;


        public DoorManager(MyGridProgram callingProgram, int CloseDelaySeconds = 3, bool AffectConnectedGrids = false)
        {
            _CloseDelaySeconds = CloseDelaySeconds;
            _AffectConnectedGrids = AffectConnectedGrids;
            ParentProgram = callingProgram;
            UpdateDoorList();
        }

        //This function maintains the list of doors to be auto-closed, and executes every 300 ticks
        private void UpdateDoorList()
        {
            List<IMyDoor> allGridDoors = new List<IMyDoor>();
            //Gets all the doors on the local, and all the connected grids
            if (_AffectConnectedGrids) ParentProgram.GridTerminalSystem.GetBlocksOfType<IMyDoor>(allGridDoors, null);
            //Gets all the doors on the current grid only
            else ParentProgram.GridTerminalSystem.GetBlocksOfType<IMyDoor>(allGridDoors, d => d.CubeGrid == ParentProgram.Me.CubeGrid);

            //Add new doors
            foreach (IMyDoor gridDoor in allGridDoors)
            {
                if (AutoDoors.Where(d => d.doorRef == gridDoor).ToList().Count == 0)
                {
                    AutoDoor autoDoor = new AutoDoor();
                    autoDoor.doorRef = gridDoor;
                    AutoDoors.Add(autoDoor);
                }
            }

            //Remove missing doors
            foreach (AutoDoor autoDoor in AutoDoors)
            {
                if (!allGridDoors.Contains(autoDoor.doorRef)) AutoDoors.Remove(autoDoor);
            }
        }

        public void Run()
        {
            foreach (AutoDoor autoDoor in AutoDoors)
            {
                //Find open doors that aren't yet flagged for auto-closing
                if (autoDoor.doorRef?.Status == (DoorStatus.Open | DoorStatus.Opening) && !autoDoor.IsTiming)
                {
                    //Flag door for auto-closing
                    autoDoor.IsTiming = true;
                    autoDoor.TimeOpened = DateTime.Now;
                }

                //Check if opened door has reached or passed the door close delay period, and closes it if true
                if (autoDoor.doorRef?.Status == (DoorStatus.Open | DoorStatus.Opening) && autoDoor.IsTiming)
                {
                    if (DateTime.Now.Subtract(autoDoor.TimeOpened).Seconds >= _CloseDelaySeconds)
                    {
                        autoDoor.doorRef.CloseDoor();
                        autoDoor.IsTiming = false;
                    }
                }

                //Handle manually closed doors
                if (autoDoor.doorRef?.Status == (DoorStatus.Closed | DoorStatus.Closing) && autoDoor.IsTiming) autoDoor.IsTiming = false;
            }

            //This script runs every 10 ticks, and we want to update the door list every 300 ticks, hence the below
            if (ElapsedTicks == 30)
            {
                UpdateDoorList();
                ElapsedTicks = 0;
            }
            else
            {
                ElapsedTicks++;
            }
        }

        public class AutoDoor
        {
            public IMyDoor doorRef;
            public bool IsTiming = false;
            public DateTime TimeOpened;
        }

    }
}
