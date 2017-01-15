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

        public Song Load(string Filename)
        {
            this.Filename = Filename;
            var sequence = new Sequence();
            sequence.Load(Filename);

            if (sequence.Count == 0)
                return null;

            for (int i = 0; i < sequence.Count; i++)
            {
                var t = sequence.ElementAt(i);

                OurTrack track = new OurTrack();
                Tracks.Add(track);
                track.Notes = new List<Notes.Note>();

                foreach (var mevent in t.Iterator())
                {
                    switch (mevent.MidiMessage.MessageType)
                    {
                        case MessageType.Channel:
                            var channelMessage = mevent.MidiMessage as ChannelMessage;
                            if (mevent.DeltaTicks == 0)
                                continue;
                            
                            int data = 0;
                            if (channelMessage.Command == ChannelCommand.NoteOn)
                            {
                                data = channelMessage.Data1;
                            }
                            else if (channelMessage.Command == ChannelCommand.NoteOff)
                            {
                                data = channelMessage.Data2;
                            }
                            var note = CreateNote(data, channelMessage.Command, mevent.AbsoluteTicks, mevent.DeltaTicks);
                            if (note != null)
                                track.Notes.Add(note);
                            break;
                        case MessageType.Meta:
                            var Message = mevent.MidiMessage as MetaMessage;
                            break;
                        default:
                            break;
                    }
                }
            }
            Song s = new Song();
            s.Tracks = Tracks;

            return s;
            //throw new NotImplementedException();
        }

        public void LoadReserved()
        {
        }

        protected Notes.Note CreateNote(int data1, ChannelCommand command, int absoluteTicks, int deltaTicks)
        {
            Notes.Note Note = null;
            if (data1 % 12 == 0)
            { 
               
                Note = Notes.Note.create("c");//new Notes.CNote();
            }
            else if ((data1 - 1) % 12 == 0)
            {
                Note = Notes.Note.create("c");
                Note.IsSharp = true;
                // C#
            }
            else if ((data1 - 2) % 12 == 0)
            {
                
                Note = Notes.Note.create("d");
            }
            else if ((data1 - 3) % 12 == 0)
            {
                Note = Notes.Note.create("d");
                Note.IsSharp = true;
                //D#
            }
            else if ((data1 - 4) % 12 == 0)
            {
                Note = Notes.Note.create("e");
            }
            // Er is geen E#
            else if ((data1 - 5) % 12 == 0)
            {
                Note = Notes.Note.create("f");
            }
            else if ((data1 - 6) % 12 == 0)
            {
                Note = Notes.Note.create("f");
                Note.IsSharp = true;
                // F#
            }
            else if ((data1 - 7) % 12 == 0)
            {
                
                Note = Notes.Note.create("g");
            }
            else if ((data1 - 8) % 12 == 0)
            {
                Note = Notes.Note.create("g");
                Note.IsSharp = true;
                // G#
            }
            else if ((data1 - 9) % 12 == 0)
            {
                Note = Notes.Note.create("a");
            }
            else if ((data1 - 10) % 12 == 0)
            {
                Note = Notes.Note.create("a");
                Note.IsSharp = true;
                //A#
            }
            else if ((data1 - 11) % 12 == 0)
            {
                Note = Notes.Note.create("b");
            }

            if (Note != null)
            {
                int octave = data1 / 12;

                if (octave > 5 || octave < 5)
                    Note.Octave = octave;

                Note.Duration = 1536/deltaTicks;
                if (Note.Duration % 2 == 1)
                    Note.Punt = true;
                if (Note.Duration == 5 && Note.Punt)
                    Note.Duration = 8;
                Note.TicksPosition = absoluteTicks;
                Note.NotePos = data1;
            }
            return Note;
        }

        public Song Load(string[] lines)
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
