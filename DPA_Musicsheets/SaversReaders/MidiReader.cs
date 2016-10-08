using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.SaversReaders
{
    public class MidiReader : IReader
    {
        

        //DPA_Musicsheets.MidiReader reader;

        public MidiReader()
        {
            

            Tracks = new List<OurTrack>();
            //_tracks = new List<OurTrack>();
            Filename = "";
        }

        public int Load(string Filename)
        {

            var sequence = new Sequence();
            sequence.Load(Filename);

            if (sequence.Count == 0)
                return 0;

            for (int i = 0; i < sequence.Count; i++)
            {
                var t = sequence.ElementAt(i);

                OurTrack track = new OurTrack();

                foreach (var mevent in t.Iterator())
                {
                    switch (mevent.MidiMessage.MessageType)
                    {
                        case MessageType.Channel:
                            var channelMessage = mevent.MidiMessage as ChannelMessage;
                            // Data1: De keycode. 0 = laagste C, 1 = laagste C#, 2 = laagste D etc.
                            // 160 is centrale C op piano.
                            //trackLog.Messages.Add(String.Format("Keycode: {0}, Command: {1}, absolute time: {2}, delta time: {3}"
                            //  , channelMessage.Data1, channelMessage.Command, midiEvent.AbsoluteTicks, midiEvent.DeltaTicks));
                            break;
                        default:
                            break;
                    }
                }
            }


            return 1;
            //throw new NotImplementedException();
        }

        public void LoadReserved()
        {
            throw new NotImplementedException();
        }

        public string Filename
        {
            get;
            set;
        }
        public List<OurTrack> Tracks
        {
            get;
            set;
        }
    }
}
