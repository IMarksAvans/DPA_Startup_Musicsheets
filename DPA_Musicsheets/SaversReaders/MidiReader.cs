using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.Notes;

namespace DPA_Musicsheets.SaversReaders
{
    public class MidiReader : IReader
    {
        Dictionary<int, Notes.Note> keycodesnotes = new Dictionary<int, Notes.Note>();

        //DPA_Musicsheets.MidiReader reader;
        protected int tpb;
        public MidiReader()
        {
            

            Tracks = new List<OurTrack>();
            //_tracks = new List<OurTrack>();
            Filename = "";
        }

        public Song Load(string Filename)
        {
            keycodesnotes.Clear();
            this.Filename = Filename;
            var sequence = new Sequence();
            sequence.Load(Filename);

            

            if (sequence.Count == 0)
                return null;
            OurTrack track = null;

            tpb = sequence.Division;
            for (int i = 0; i < sequence.Count; i++)
            {
                var t = sequence.ElementAt(i);

                float notedurationdata = 0.0f;

                foreach (var mevent in t.Iterator())
                {
                    if (track == null)
                    {
                        track = new OurTrack();
                        Tracks.Add(track);
                        track.Notes = new List<Notes.Note>();
                    }
                    switch (mevent.MidiMessage.MessageType)
                    {
                        case MessageType.Channel:
                            var channelMessage = mevent.MidiMessage as ChannelMessage;

                            var command = channelMessage.Command;
                            int keyCode = channelMessage.Data1;

                            if (keycodesnotes.ContainsKey(keyCode) && (channelMessage.Data2 == 0 || channelMessage.Command == ChannelCommand.NoteOff))
                            {
                                Notes.Note note = keycodesnotes[keyCode];
                                keycodesnotes.Remove(keyCode);
                                var ticks = mevent.AbsoluteTicks - note.StartTime;
                                float x = SetNoteData(note,ticks,mevent);
                                notedurationdata += 1 / x;
                                if (notedurationdata >= 1.0f)
                                {
                                    track = new OurTrack();
                                    Tracks.Add(track);
                                    track.Notes = new List<Notes.Note>();
                                    notedurationdata = 0.0f;
                                }
                            }
                            else if (command == ChannelCommand.NoteOn && channelMessage.Data2 > 0)
                            {
                                Notes.Note n = null;
                                if (mevent.DeltaTicks > 0)
                                {
                                    n = Notes.Note.create("r");

                                    int octave = keyCode / 12;

                                    if (octave > 5 || octave < 5)
                                        n.Octave = octave;

                                    n.StartTime = mevent.AbsoluteTicks;
                                }
                                else
                                {
                                    n = CreateNote(channelMessage.Data1/*keyCode*/, mevent);
                                }


                                if (n != null)
                                {
                                    track.Notes.Add(n);
                                    keycodesnotes.Add(keyCode, n);
                                }
                                /*int data = channelMessage.Data1;
                               if (channelMessage.Command == ChannelCommand.NoteOn)
                                {
                                    data = channelMessage.Data1;
                                }
                                else if (channelMessage.Command == ChannelCommand.NoteOff)
                                {
                                    data = channelMessage.Data2;
                                }

                                if (channelMessage.Data2 >= 0)
                                {
                                    var note = Notes.Note.create("r");
                                    if (note != null)
                                        track.Notes.Add(note);
                                }
                                //else


                                var note = CreateNote(data, channelMessage.Command, mevent.AbsoluteTicks, mevent.DeltaTicks);
                                if (note != null)
                                    track.Notes.Add(note);*/
                            }

                           
                            
                            break;
                        case MessageType.Meta:
                            var Message = mevent.MidiMessage as MetaMessage;

                            if (Message.MetaType == MetaType.TimeSignature)
                            {
                                byte[] bytes = Message.GetBytes();
                                var Measure = bytes[0];
                                var numofbeats = (int)Math.Pow(2, bytes[1]);

                                string value = Convert.ToString(Measure) + "," + Convert.ToString(numofbeats);

                                track.Time = Convert.ToDouble(value);
                            }
                            else if (Message.MetaType == MetaType.Tempo)
                            {
                                byte[] bytes = Message.GetBytes();
                                int msPerBeatTempo = (bytes[0] & 0xff) << 16 | (bytes[1] & 0xff) << 8 | (bytes[2] & 0xff);
                                var bpm = 60000000 / msPerBeatTempo;
                                track.Metronome = 4;
                                track.Tempo = bpm;
                            }
                            else if (Message.MetaType == MetaType.TrackName)
                            {
                                
                            }
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

        private float SetNoteData(Note note, int ticks, MidiEvent mevent)
        {
            double percentageOfBeatNote = (double)ticks / tpb;
            double percentageOfWholeNote = percentageOfBeatNote * (1d / 4);

            double noteDuration = -1;

            float duration = 0.0f;

            for (int noteLength = 128; noteLength >= 1; noteLength /= 2)
            {
                double absoluteNoteLength = (1.0 / noteLength);

                if (percentageOfWholeNote <= absoluteNoteLength)
                {
                    noteDuration = absoluteNoteLength;
                    if (percentageOfWholeNote <= absoluteNoteLength / 2 * 1.5)
                    {
                        noteDuration = absoluteNoteLength / 2;
                        noteLength *= 2;
                        note.Punt = true;
                    }
                    
                    duration = noteLength;

                    note.Duration = noteLength;

                    

                    break;
                }
            }

            return duration;
            /*
            Note.Duration = 1536 / deltaTicks;
            if (Note.Duration % 2 == 1)
                Note.Punt = true;
            if (Note.Duration == 5 && Note.Punt)
                Note.Duration = 8;
            Note.TicksPosition = absoluteTicks;
            Note.NotePos = data1;*/
        }

        public void LoadReserved()
        {
        }

        private Note CreateNote(int keycode, MidiEvent mevent)
        {
            Notes.Note Note = null;

            if (!this.keycodesnotes.ContainsKey(keycode))
            {


                if (keycode % 12 == 0)
                {

                    Note = Notes.Note.create("c");//new Notes.CNote();
                }
                else if ((keycode - 1) % 12 == 0)
                {
                    Note = Notes.Note.create("c");
                    Note.IsSharp = true;
                    // C#
                }
                else if ((keycode - 2) % 12 == 0)
                {

                    Note = Notes.Note.create("d");
                }
                else if ((keycode - 3) % 12 == 0)
                {
                    Note = Notes.Note.create("d");
                    Note.IsSharp = true;
                    //D#
                }
                else if ((keycode - 4) % 12 == 0)
                {
                    Note = Notes.Note.create("e");
                }
                // Er is geen E#
                else if ((keycode - 5) % 12 == 0)
                {
                    Note = Notes.Note.create("f");
                }
                else if ((keycode - 6) % 12 == 0)
                {
                    Note = Notes.Note.create("f");
                    Note.IsSharp = true;
                    // F#
                }
                else if ((keycode - 7) % 12 == 0)
                {

                    Note = Notes.Note.create("g");
                }
                else if ((keycode - 8) % 12 == 0)
                {
                    Note = Notes.Note.create("g");
                    Note.IsSharp = true;
                    // G#
                }
                else if ((keycode - 9) % 12 == 0)
                {
                    Note = Notes.Note.create("a");
                }
                else if ((keycode - 10) % 12 == 0)
                {
                    Note = Notes.Note.create("a");
                    Note.IsSharp = true;
                    //A#
                }
                else if ((keycode - 11) % 12 == 0)
                {
                    Note = Notes.Note.create("b");
                }

                int octave = keycode / 12;

                if (octave > 5 || octave < 5)
                    Note.Octave = octave;

                Note.StartTime = mevent.AbsoluteTicks;
            }

            return Note;
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
