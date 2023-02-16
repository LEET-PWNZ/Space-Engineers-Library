using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.GUI.TextPanel;

namespace IngameScript
{
    public class ScreenManager
    {
        private MyGridProgram screenParentProgram;
        private IMyTextPanel textPanel = null;
        private List<string> screenLines = new List<string>();
        private string _screenName;
        private string _screenText;
        public ScreenManager(MyGridProgram inParentProgram,string screenName)
        {
            screenParentProgram = inParentProgram;
            _screenName = screenName;
            UpdateScreenBlock();
            WriteAllText("");
            UpdateScreen();
        }

        public void UpdateScreenBlock()
        {
            var textPanels = new List<IMyTextPanel>();
            screenParentProgram.GridTerminalSystem.GetBlocksOfType(textPanels, b=> b.CustomName.Equals(_screenName) && b.CubeGrid==screenParentProgram.Me.CubeGrid);
            textPanel = textPanels.FirstOrDefault();
            if(textPanel!=null) textPanel.ContentType = ContentType.TEXT_AND_IMAGE;
            //textPanel?.ShowPublicTextOnScreen();
        }

        public void UpdateScreen()
        {
            
               textPanel?.WriteText(_screenText);
        }

        public void WriteLine(string line)
        {
            _screenText += line+ "\n";
        }

        public void WriteAllText (string text)
        {
            _screenText = text;
        }

    }
}
