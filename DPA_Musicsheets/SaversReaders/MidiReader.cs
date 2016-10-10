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
            this.Filename = Filename;
            var sequence = new Sequence();
            sequence.Load(Filename);

            if (sequence.Count == 0)
                return 0;

            for (int i = 0; i < sequence.Count; i++)
            {
                var t = sequence.ElementAt(i);

                OurTrack track = new OurTrack();
                Tracks.Add(track);
                track.Notes = new List<Notes.AbstractNote>();

                foreach (var mevent in t.Iterator())
                {
                    switch (mevent.MidiMessage.MessageType)
                    {
                        case MessageType.Channel:
                            var channelMessage = mevent.MidiMessage as ChannelMessage;
                            var note = CreateNote(channelMessage.Data1, channelMessage.Command, mevent.AbsoluteTicks, mevent.DeltaTicks);
                            if (note != null)
                                track.Notes.Add(note);
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
        }

        protected Notes.AbstractNote CreateNote(int data1, ChannelCommand command, int absoluteTicks, int deltaTicks)
        {
            Notes.AbstractNote Note = null;

            if (data1 % 12 == 0)
            {
                Note = Notes.AbstractNote.create("C");//new Notes.CNote();
            }
            else if ((data1 - 1) % 12 == 0)
            {
                Note = Notes.AbstractNote.create("C");
                Note.IsSharp = true;
                // C#
            }
            else if ((data1 - 2) % 12 == 0)
            {
                Note = Notes.AbstractNote.create("D");
            }
            else if ((data1 - 3) % 12 == 0)
            {
                Note = Notes.AbstractNote.create("D");
                Note.IsSharp = true;
                //D#
            }
            else if ((data1 - 4) % 12 == 0)
            {
                Note = Notes.AbstractNote.create("E");
            }
            // Er is geen E#
            else if ((data1 - 5) % 12 == 0)
            {
                Note = Notes.AbstractNote.create("F");
            }
            else if ((data1 - 6) % 12 == 0)
            {
                Note = Notes.AbstractNote.create("F");
                Note.IsSharp = true;
                // F#
            }
            else if ((data1 - 7) % 12 == 0)
            {
                Note = Notes.AbstractNote.create("G");
            }
            else if ((data1 - 8) % 12 == 0)
            {
                Note = Notes.AbstractNote.create("G");
                Note.IsSharp = true;
                // G#
            }
            else if ((data1 - 9) % 12 == 0)
            {
                Note = Notes.AbstractNote.create("A");
            }
            else if ((data1 - 10) % 12 == 0)
            {
                Note = Notes.AbstractNote.create("A");
                Note.IsSharp = true;
                //A#
            }
            else if ((data1 - 11) % 12 == 0)
            {
                Note = Notes.AbstractNote.create("B");
            }

            if (Note != null)
            {
                Note.Duration = deltaTicks;
                Note.TicksPosition = absoluteTicks;
                Note.NotePos = data1;
            }
            return Note;
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
