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
        Dictionary<int, float> demaatopticks = new Dictionary<int, float>();
        Dictionary<int, double> tracktimeopticks = new Dictionary<int, double>();
        //DPA_Musicsheets.MidiReader reader;
        protected int tpb;
        public MidiReader()
        {
            Tracks = new List<OurTrack>();
            //_tracks = new List<OurTrack>();
            Filename = "";
        }

        private void MergeControlTrack(Track controlTrack, IEnumerable<Track> tracks)
        {
            List<int> mergeEventIndexes = new List<int>();
            List<int> eraseEventIndexes = new List<int>();
            for (int i = 0; i < controlTrack.Iterator().Count(); i++)
            {
                var midiEvent = controlTrack.Iterator().ElementAt(i);

                if (midiEvent.MidiMessage.MessageType == MessageType.Meta)
                {
                    var message = midiEvent.MidiMessage as MetaMessage;
                    if (message.MetaType == MetaType.Tempo || message.MetaType == MetaType.TimeSignature)
                    {
                        mergeEventIndexes.Add(i);
                        eraseEventIndexes.Add(i);
                    }
                    else if (message.MetaType == MetaType.TrackName)
                    {
                        eraseEventIndexes.Add(i);
                    }
                }
            }

            foreach (var eventIndex in mergeEventIndexes)
            {
                var midiEvent = controlTrack.Iterator().ElementAt(eventIndex);

                foreach (var track in tracks)
                    track.Insert(midiEvent.AbsoluteTicks, midiEvent.MidiMessage);
            }

            int offset = 0;
            foreach (var eventIndex in eraseEventIndexes)
                controlTrack.RemoveAt(eventIndex - offset++);
        }

        private bool IsControlTrack(Track track)
        {
            bool isit = !track.Iterator().Any(item =>
            {
                if (item.MidiMessage.MessageType == MessageType.Channel)
                {
                    var message = item.MidiMessage as ChannelMessage;
                    if (message.Command == ChannelCommand.NoteOn || message.Command == ChannelCommand.NoteOff)
                        return true;
                }
                return false;
            });

            return isit;
        }

        public Song Load(string Filename)
        {
            keycodesnotes.Clear();
            this.Filename = Filename;
            var sequence = new Sequence();
            sequence.Load(Filename);

            // Get sets of normal tracks and control tracks.
            var normalTracks = sequence.Where(t => !IsControlTrack(t));
            var controlTracks = sequence.Where(t => IsControlTrack(t));

            // Merge the control tracks into the normal tracks.
            foreach (var controlTrack in controlTracks)
                MergeControlTrack(controlTrack, normalTracks);

            if (sequence.Count == 0)
                return null;

            OurTrack track = null;
            tpb = sequence.Division;

            float notedurationdata = 0.0f;
            float demaat = 1.0f;
            for (int i = 0; i < sequence.Count; i++)
            {
                //var t = sequence[i];

                foreach (var mevent in sequence[i].Iterator())
                {
                    if (track == null)
                    {
                        track = new OurTrack();
                        Tracks.Add(track);
                        track.Notes = new List<Notes.Note>();

                        // hax
                        track.Octave = 6;
                        track.Relative = 'c';
                        track.Pitch = "treble";
                    }

                    /*if (this.tracktimeopticks.ContainsKey(mevent.DeltaTicks))
                    {
                        track.Time = this.tracktimeopticks[mevent.DeltaTicks];
                        tracktimeopticks.Remove(mevent.DeltaTicks);
                    }

                    if (this.demaatopticks.ContainsKey(mevent.DeltaTicks))
                    {

                        demaat = this.demaatopticks[mevent.DeltaTicks];
                        this.demaatopticks.Remove(mevent.DeltaTicks);
                    }*/

                    switch (mevent.MidiMessage.MessageType)
                    {
                        case MessageType.Channel:
                            var channelMessage = mevent.MidiMessage as ChannelMessage;

                            var command = channelMessage.Command;
                            int keyCode = channelMessage.Data1;

                            if (command == ChannelCommand.Controller)
                            {
                                // op een of andere manier hier de relative bepalen?
                            }
                            if (keycodesnotes.ContainsKey(keyCode) && (channelMessage.Data2 == 0 || channelMessage.Command == ChannelCommand.NoteOff))
                            {
                                Notes.Note note = keycodesnotes[keyCode];
                                keycodesnotes.Remove(keyCode);
                                var ticks = mevent.AbsoluteTicks - note.StartTime;
                                float x = SetNoteData(note, ticks, mevent);
                                notedurationdata += 1 / x;
                                if (notedurationdata >= demaat)
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
                                    n = CreateRest(mevent, tpb, track.Time);// 

                                    track.Notes.Add(n);
                                    float x = n.Duration;
                                    notedurationdata += 1 / x;
                                    if (notedurationdata >= demaat)
                                    {
                                        track = new OurTrack();
                                        Tracks.Add(track);
                                        track.Notes = new List<Notes.Note>();
                                        notedurationdata = 0.0f;
                                    }
                                }
                                
                                {
                                    n = CreateNote(channelMessage.Data1/*keyCode*/, mevent);

                                    track.Notes.Add(n);
                                    keycodesnotes.Add(keyCode, n);
                                }
                            }
                           
                            
                            break;
                        case MessageType.Meta:
                            var Message = mevent.MidiMessage as MetaMessage;

                            if (Message.MetaType == MetaType.TimeSignature)
                            {
                                byte[] bytes = Message.GetBytes();
                                float Measure = bytes[0];
                                float numofbeats = (int)Math.Pow(2, bytes[1]);

                                demaat = Measure / numofbeats;

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

        private Note CreateRest(MidiEvent mevent, int tpb, double time)
        {
            var channelMessage = mevent.MidiMessage as ChannelMessage;
            var n = Notes.Note.create("r");
            int octave = channelMessage.Data1 / 12;
 
            n.Octave = octave;

            SetNoteData(n,mevent.DeltaTicks,mevent);

            return n;
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
                    if (percentageOfWholeNote <= absoluteNoteLength / 2 * 1.5)
                        duration /= 2;

                    note.Duration = noteLength;

                    

                    break;
                }
            }

            if (noteDuration != -1 && note.getKey() == "r")
            {

                int convertDuration = (int)(1d / noteDuration);
                note.Duration = convertDuration;
            }

           return duration;
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

                
                Note.Octave = octave;
                

                Note.StartTime = mevent.AbsoluteTicks;
            }

            return Note;
        }

        // deze klopt niet
        /*protected Notes.Note CreateNote(int data1, ChannelCommand command, int absoluteTicks, int deltaTicks)
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
        }*/

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
