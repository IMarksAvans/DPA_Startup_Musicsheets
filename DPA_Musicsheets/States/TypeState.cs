using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DPA_Musicsheets.States
{
    class TypeState : BaseState
    {
        private Timer GenerationTimer { get; set; }

        public TypeState(MainWindow main) : base(main)
        {
            GenerationTimer = new Timer();
            GenerationTimer.Interval = 1500;
            GenerationTimer.AutoReset = false;
            GenerationTimer.Elapsed += GenerationTimer_Elapsed;
        }

        private void GenerationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            main.SetState(new SetBookmarksState(main));
            
            main.handleState();
        }

        internal override void Handle()
        {
            ResetTimer();   
        }

        private void ResetTimer()
        {
            GenerationTimer.Stop();
            GenerationTimer.Start();
        }
    }
}
