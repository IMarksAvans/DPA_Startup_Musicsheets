using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    public static class LilyReader
    {
        public static IEnumerable<MidiTrack> ReadLilypond(string lilyFileLocation)
        {
            string[] lines = System.IO.File.ReadAllLines(@"" + lilyFileLocation);

            return ReadSequence(lines);
        }

        public static IEnumerable<MidiTrack> ReadSequence(String[] lines)
        {
            // De sequence heeft tracks. Deze zijn per index benaderbaar.
            for (int i = 0; i < lines.Count(); i++)
            {
               
            }

            yield return new MidiTrack();
        }

        private static string GetMetaString(MetaMessage metaMessage)
        {
            byte[] bytes = metaMessage.GetBytes();
            switch (metaMessage.MetaType)
            {
                case MetaType.Tempo:
                    // Bitshifting is nodig om het tempo in BPM te be
                    int tempo = (bytes[0] & 0xff) << 16 | (bytes[1] & 0xff) << 8 | (bytes[2] & 0xff);
                    int bpm = 60000000 / tempo;
                    return metaMessage.MetaType + ": " + bpm;
                //case MetaType.SmpteOffset:
                //    break;
                case MetaType.TimeSignature:                               //kwart = 1 / 0.25 = 4
                    return metaMessage.MetaType + ": (" + bytes[0] + " / " + 1 / Math.Pow(bytes[1], -2) + ") ";
                //case MetaType.KeySignature:
                //    break;
                //case MetaType.ProprietaryEvent:
                //    break;
                case MetaType.TrackName:
                    return metaMessage.MetaType + ": " + Encoding.Default.GetString(metaMessage.GetBytes());
                default:
                    return metaMessage.MetaType + ": " + Encoding.Default.GetString(metaMessage.GetBytes());
            }
        }
    }
}
