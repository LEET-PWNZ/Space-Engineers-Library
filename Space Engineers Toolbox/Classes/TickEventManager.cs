using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceEngineersToolbox.Classes
{
    public class TickEventManager
    {
        public delegate void OnTickEvent();
        public event OnTickEvent OnEventTick;
        public event OnTickEvent OnEventTick10;
        public event OnTickEvent OnEventTick100;
        private int tickCount100=0;
        private int tickCount10 = 0;

        public void UpdateTickManager()
        {
            OnEventTick?.Invoke();
            tickCount10++;
            tickCount100++;
            if (tickCount10 >= 10)
            {
                OnEventTick10?.Invoke();
                tickCount10 = 0;
            }
            if (tickCount100 >= 100)
            {
                OnEventTick100?.Invoke();
                tickCount100 = 0;
            }
        }

    }
}
