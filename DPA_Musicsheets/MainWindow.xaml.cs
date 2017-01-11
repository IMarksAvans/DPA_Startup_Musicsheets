﻿using Microsoft.Win32;
using PSAMControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DPA_Musicsheets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MidiPlayer _player;
        public ObservableCollection<MidiTrack> MidiTracks { get; private set; }

        // De OutputDevice is een midi device of het midikanaal van je PC.
        // Hierop gaan we audio streamen.
        // DeviceID 0 is je audio van je PC zelf.
        private OutputDevice _outputDevice = new OutputDevice(0);
        private IReader r;
        private ISaver s;
        private Song currentSong;
        public MainWindow()
        {
            this.MidiTracks = new ObservableCollection<MidiTrack>();
            InitializeComponent();
            DataContext = MidiTracks;
            //FillPSAMViewer();

            s = new SaversReaders.LilySaver();
            /*r.Load("Alle-eendjes-zwemmen-in-het-water.mid");

            IReader LilyReader = new SaversReaders.LilyReader();

            ISaver lilysaver = new SaversReaders.LilySaver();
            //lilysaver.SetFilename();
            lilysaver.SetSong(LilyReader.Load("Alle-eendjes-zwemmen-in-het-water.ly"));
            lilysaver.Save("test1.ly");
            lilysaver.SetSong(LilyReader.Load("Twee-emmertjes-water-halen.ly"));
            lilysaver.Save("test2.ly");*/
            //notenbalk.LoadFromXmlFile("Resources/example.xml");
        }

        private void FillPSAMViewer()
        {
            staff.ClearMusicalIncipit();

            // Clef = sleutel
            // had perfect kunnen zijn voor een interpreter
            if(currentSong.Pitch.Contains("treble"))
                staff.AddMusicalSymbol(new Clef(ClefType.GClef, 2));
            else if(currentSong.Pitch.Contains("alto") || currentSong.Pitch.Contains("tenor"))
                staff.AddMusicalSymbol(new Clef(ClefType.CClef, 2));
            else if (currentSong.Pitch.Contains("bass"))
                staff.AddMusicalSymbol(new Clef(ClefType.FClef, 2));
            //staff.AddMusicalSymbol(new TimeSignature(TimeSignatureType.Numbers, (uint)currentSong.Tempo, 4));
            //staff.AddMusicalSymbol()
            foreach (var track in currentSong.Tracks)
            {
                foreach (var note in track.Notes)
                {
                    var key = note.getKey().ToUpper();
                    var alternation = note.IsSharp ? 1 : (note.IsFlat ? -1 : 0);
                    var octave = note.Octave;
                    var duration = (MusicalSymbolDuration)note.Duration;
                    var direction = (int) note.Octave > 5 ? NoteStemDirection.Down : NoteStemDirection.Up;
                    var tie = NoteTieType.None;
                    var beam = new List<NoteBeamType>() { NoteBeamType.Start};

                    var dots = note.Punt ? 1 : 0;
                    var n = new Note(key, alternation, octave, duration, direction, tie, beam); //{ NumberOfDots = dots }
                    n.CurrentTempo = currentSong.Tempo;
                    n.NumberOfDots = dots;
                    staff.AddMusicalSymbol(n);
                }
                staff.AddMusicalSymbol(new Barline());
            }
            /*  
                The first argument of Note constructor is a string representing one of the following names of steps: A, B, C, D, E, F, G. 
                The second argument is number of sharps (positive number) or flats (negative number) where 0 means no alteration. 
                The third argument is the number of an octave. 
                The next arguments are: duration of the note, stem direction and type of tie (NoteTieType.None if the note is not tied). 
                The last argument is a list of beams. If the note doesn't have any beams, it must still have that list with just one 
                    element NoteBeamType.Single (even if duration of the note is greater than eighth). 
                    To make it clear how beamlists work, let's try to add a group of two beamed sixteenths and eighth:
                        Note s1 = new Note("A", 0, 4, MusicalSymbolDuration.Sixteenth, NoteStemDirection.Down, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Start, NoteBeamType.Start});
                        Note s2 = new Note("C", 1, 5, MusicalSymbolDuration.Sixteenth, NoteStemDirection.Down, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Continue, NoteBeamType.End });
                        Note e = new Note("D", 0, 5, MusicalSymbolDuration.Eighth, NoteStemDirection.Down, NoteTieType.None,new List<NoteBeamType>() { NoteBeamType.End });
                        viewer.AddMusicalSymbol(s1);
                        viewer.AddMusicalSymbol(s2);
                        viewer.AddMusicalSymbol(e); 
            */
            staff.AddMusicalSymbol(new Note("A", 0, 4, MusicalSymbolDuration.Sixteenth, NoteStemDirection.Down, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Start, NoteBeamType.Start }));
            staff.AddMusicalSymbol(new Note("C", 1, 5, MusicalSymbolDuration.Sixteenth, NoteStemDirection.Down, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Continue, NoteBeamType.End }));
            staff.AddMusicalSymbol(new Note("D", 0, 5, MusicalSymbolDuration.Eighth, NoteStemDirection.Down, NoteTieType.Start, new List<NoteBeamType>() { NoteBeamType.End }));
            staff.AddMusicalSymbol(new Barline());

            staff.AddMusicalSymbol(new Note("D", 0, 5, MusicalSymbolDuration.Whole, NoteStemDirection.Down, NoteTieType.Stop, new List<NoteBeamType>() { NoteBeamType.Single }));
            staff.AddMusicalSymbol(new Note("E", 0, 4, MusicalSymbolDuration.Quarter, NoteStemDirection.Up, NoteTieType.Start, new List<NoteBeamType>() { NoteBeamType.Single }) { NumberOfDots = 1 });
            staff.AddMusicalSymbol(new Barline());

            staff.AddMusicalSymbol(new Note("C", 0, 4, MusicalSymbolDuration.Half, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single }));
            staff.AddMusicalSymbol(
                new Note("E", 0, 4, MusicalSymbolDuration.Half, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single })
                { IsChordElement = true });
            staff.AddMusicalSymbol(
                new Note("G", 0, 4, MusicalSymbolDuration.Half, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single })
                { IsChordElement = true });
            staff.AddMusicalSymbol(new Barline());
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if(_player != null)
            {
                _player.Dispose();
            }

            _player = new MidiPlayer(_outputDevice);
            _player.Play(txt_MidiFilePath.Text);
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi Files (.mid)|*.mid|Lilypond Files (.ly)|*.ly" };
            if (openFileDialog.ShowDialog() == true)
            {
                txt_MidiFilePath.Text = openFileDialog.FileName;
                if (txt_MidiFilePath.Text.Substring(txt_MidiFilePath.Text.Length - 2, 2) == "ly")
                {
                    r = new SaversReaders.LilyReader();
                    currentSong = r.Load(txt_MidiFilePath.Text);
                    string[] content = File.ReadAllLines(@"" + txt_MidiFilePath.Text);
                    Displayer.Text = string.Join("\n", content);
                }
                else if (txt_MidiFilePath.Text.Substring(txt_MidiFilePath.Text.Length - 3, 3) == "mid")
                {
                    r = new SaversReaders.MidiReader();
                    currentSong = r.Load(txt_MidiFilePath.Text);
                }
            }

            FillPSAMViewer();
        }
        
        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (_player != null)
                _player.Dispose();
        }

        private void btn_ShowContent_Click(object sender, RoutedEventArgs e)
        {
            if (txt_MidiFilePath.Text.EndsWith(".ly"))
            {
                SaversReaders.LilyReader lr = new SaversReaders.LilyReader();
                lr.Load(txt_MidiFilePath.Text);
                ShowMidiTracks(LilyReader.ReadLilypond(txt_MidiFilePath.Text));
            }
            else
            {
                ShowMidiTracks(MidiReader.ReadMidi(txt_MidiFilePath.Text));
            }
        }

        private void ShowMidiTracks(IEnumerable<MidiTrack> midiTracks)
        {
            MidiTracks.Clear();
            foreach (var midiTrack in midiTracks)
            {
                MidiTracks.Add(midiTrack);
            }

            //tabCtrl_MidiContent.SelectedIndex = 0;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _outputDevice.Close();
            if (_player != null)
            {
                _player.Dispose();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            s.SetSong(currentSong);
            s.Save(txtFilename.Text);
        }
    }
}
