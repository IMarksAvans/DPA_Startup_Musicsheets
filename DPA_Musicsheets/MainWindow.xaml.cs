using DPA_Musicsheets.Command.SubCommands;
using DPA_Musicsheets.CoR;
using DPA_Musicsheets.Memento;
using Microsoft.Win32;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
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
        private Timer GenerationTimer { get; set; }
        private OutputDevice _outputDevice = new OutputDevice(0);
        //private List<String> Mementos = new List<String>();
        private IReader r;
        private IWriter w;
        private Originator oo, no, so;
        private Caretaker oc, nc, sc;
        private Song currentSong;
        protected List<System.Windows.Input.Key> keyDownList = new List<System.Windows.Input.Key>();
        protected ChainOfResponsibility cor = new ChainOfResponsibility();

        public MainWindow()
        {
            InitializeComponent();
            GenerationTimer = new Timer();
            GenerationTimer.Interval = 1500;
            GenerationTimer.AutoReset = false;
            GenerationTimer.Elapsed += GenerationTimer_Elapsed;

            this.MidiTracks = new ObservableCollection<MidiTrack>();
            DataContext = MidiTracks;
            w = new SaversReaders.LilyWriter();

            oo = new Originator();
            no = new Originator();
            so = new Originator();

            oc = new Caretaker();
            nc = new Caretaker();
            sc = new Caretaker();

            InitChainOfResponsibility();
        }

        private void InitChainOfResponsibility()
        {
            var handler1 = new InsertTime6Handler();
            cor.SetSuccessor(handler1);
            var handler2 = new InsertTime3Handler();
            handler1.SetSuccessor(handler2);
            var handler3 = new InsertTime4Handler();
            handler2.SetSuccessor(handler3);
            var handler4 = new InsertTimeHandler();
            handler3.SetSuccessor(handler4);
            var handler5 = new InsertTempoHandler();
            handler4.SetSuccessor(handler5);
            var handler6 = new ClefHandler();
            handler5.SetSuccessor(handler6);
            var handler7 = new OpenHandler();
            handler6.SetSuccessor(handler7);
            var handler8 = new PdfHandler();
            handler7.SetSuccessor(handler8);
            var handler9 = new SaveHandler();
            handler8.SetSuccessor(handler9);

            handler1.SetCommand(new InsertTime6Command(this));
            handler2.SetCommand(new InsertTime3Command(this));
            handler3.SetCommand(new InsertTime4Command(this));
            handler4.SetCommand(new InsertTime4Command(this));
            handler5.SetCommand(new InsertTempoCommand(this));
            handler6.SetCommand(new ClefCommand(this));
            handler7.SetCommand(new OpenCommand(this));
            handler8.SetCommand(new PdfCommand(this));
            handler9.SetCommand(new SaveCommand(this));
        }

        private void GenerationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GenerationTimer.Stop();
            this.Dispatcher.Invoke(() =>
            {
                oo.State = no.State;
                oc.Mementos.Add(oo.CreateMemento());
                no.State = Displayer.Text;
                nc.Mementos.Add(no.CreateMemento());
                string[] lines = Displayer.Text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                r = new SaversReaders.LilyReader();
                currentSong = r.Load(lines);
                FillPSAMViewer();
            });
        }

        private void lilypondTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in Displayer.Text)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ',' || c == '\'' || c == '\\' || c=='/' || c=='\n' || c==' ' || c=='=' || c=='{' || c=='}' || c=='~')
                {
                    sb.Append(c);
                }
            }

            Displayer.Text = sb.ToString();
            //string[] lines = Displayer.Text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            //currentSong = r.Load(lines);
            ResetTimer();
        }

        private void ResetTimer()
        {
            GenerationTimer.Stop();
            GenerationTimer.Start();
        }

        private void FillPSAMViewer()
        {
            staff.ClearMusicalIncipit();
            if (currentSong == null)
                return;
            // Clef = sleutel
            // had perfect kunnen zijn voor een interpreter
            
            //staff.AddMusicalSymbol(new TimeSignature(TimeSignatureType.Numbers, (uint)currentSong.Tempo, 4));
            //staff.AddMusicalSymbol()
            foreach (var track in currentSong.Tracks)
            {
                if (track.Pitch.Contains("treble"))
                    staff.AddMusicalSymbol(new Clef(ClefType.GClef, 0));
                else if (track.Pitch.Contains("alto") || track.Pitch.Contains("tenor"))
                    staff.AddMusicalSymbol(new Clef(ClefType.CClef, 0));
                else if (track.Pitch.Contains("bass"))
                    staff.AddMusicalSymbol(new Clef(ClefType.FClef, 0));

                Note prevNote = null;
                //staff.AddMusicalSymbol();
                foreach (var note in track.Notes)
                {
                    var key = note.getKey().ToUpper();
                    if (key == "R")
                    {
                        var n = new Rest((MusicalSymbolDuration)note.Duration);
                        staff.AddMusicalSymbol(n);
                    }
                    else
                    {
                        var alternation = note.IsSharp ? 1 : (note.IsFlat ? -1 : 0);
                        var octave = note.Octave;
                        var duration = (MusicalSymbolDuration)note.Duration;
                        var direction = (int)note.Duration >= 5 ? NoteStemDirection.Down : NoteStemDirection.Up;
                        var beam = new List<NoteBeamType>();
                        var tie = NoteTieType.None;
                        if (note.Duration > 4)
                        {
                            if (prevNote != null && prevNote.BeamList.Contains(NoteBeamType.Start))
                            {
                                beam.Add(NoteBeamType.End);
                            }
                            else
                            {
                                beam.Add(NoteBeamType.Start);

                            }
                        }
                        else
                         beam.Add(NoteBeamType.Start);

                        var dots = note.Punt ? 1 : 0;
                        var n = new Note(key, alternation, octave, duration, direction, tie, beam); //{ NumberOfDots = dots }
                        n.CurrentTempo = track.Tempo;
                        n.NumberOfDots = dots;
                        staff.AddMusicalSymbol(n);
                        prevNote = n;
                    }
                    
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
           /* staff.AddMusicalSymbol(new Note("A", 0, 4, MusicalSymbolDuration.Sixteenth, NoteStemDirection.Down, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Start, NoteBeamType.Start }));
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
            staff.AddMusicalSymbol(new Barline());*/
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
            Open();
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

            RoutedCommand newcmd = new RoutedCommand();
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

            if (sc.Memento.State != Displayer.Text) {
                if (MessageBox.Show("Would you like to save before closing the application?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Save();
                }
            }
        }

        private void btnUndo_Click(object sender, RoutedEventArgs e)
        {
            if(oc.Mementos.Count > 0)
                oc.Mementos.RemoveAt(oc.Mementos.Count-1);
            if (oc.Mementos.Count > 0)
                oo.SetMemento(oc.Mementos.ElementAt(oc.Mementos.Count - 1));
            Displayer.Text = oo.State;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private string getTextFromLilypond()
        {
            w.SetSong(currentSong);
            return string.Join("\n", w.GetContent());
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (!keyDownList.Contains(e.Key))
                keyDownList.Add(e.Key);
            cor.Execute(keyDownList);
        }

        private void Displayer_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            if(!keyDownList.Contains(e.Key))
                keyDownList.Add(e.Key);
            cor.Execute(keyDownList);*/
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (keyDownList.Contains(e.Key))
                keyDownList.Remove(e.Key);
        }

        private void Displayer_KeyUp(object sender, KeyEventArgs e)
        {
            /*
            if (keyDownList.Contains(e.Key))
                keyDownList.Remove(e.Key);*/
        }

        // receiver commands
        public void Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Midi Files (.mid)|*.mid|Lilypond Files (.ly)|*.ly" };
            if (openFileDialog.ShowDialog() == true)
            {
                txt_MidiFilePath.Text = openFileDialog.FileName;
                if (txt_MidiFilePath.Text.Substring(txt_MidiFilePath.Text.Length - 2, 2) == "ly")
                {
                    r = new SaversReaders.LilyReader();
                    currentSong = r.Load(txt_MidiFilePath.Text);
                    w.SetSong(currentSong);
                    Displayer.Text = string.Join("\n", w.GetContent());
                }
                else if (txt_MidiFilePath.Text.Substring(txt_MidiFilePath.Text.Length - 3, 3) == "mid")
                {
                    r = new SaversReaders.MidiReader();
                    currentSong = r.Load(txt_MidiFilePath.Text);
                    w.SetSong(currentSong);
                    Displayer.Text = string.Join("\n", w.GetContent());
                }

                if (Displayer.Text != null)
                {
                    so.State = Displayer.Text;
                    sc.Memento = so.CreateMemento();
                }
                
            }

            FillPSAMViewer();
        }

        public void Save()
        {
            w.SetSong(currentSong);
            w.Save(txtFilename.Text + ".ly");

            so.State = Displayer.Text;
            sc.Memento = so.CreateMemento();
        }

        internal void SaveToPdf()
        {
            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";
            string sourceFolder = Directory.GetCurrentDirectory() + "\\";
            string sourceFileName = txtFilename.Text + ".ly";//"Twee-emmertjes-water-halen";
            string targetFolder = Directory.GetCurrentDirectory() + "\\";
            string targetFileName = txtFilename.Text;

            var process = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = sourceFolder,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = String.Format("--pdf \"{0}{1}\"", sourceFolder, sourceFileName),
                    FileName = lilypondLocation
                }
            };

            process.Start();
            while (!process.HasExited) {  }

            //File.Copy(sourceFolder + sourceFileName + ".pdf", targetFolder + targetFileName + ".pdf", true);
        }

        internal void InsertTime3()
        {
            var insertText = "\\time 3/4";
            var selectionIndex = Displayer.SelectionStart;
            Displayer.Text = Displayer.Text.Insert(selectionIndex, insertText);
            Displayer.SelectionStart = selectionIndex + insertText.Length;
            
        }

        internal void InsertTime4()
        {
            var insertText = "\\time 4/4";
            var selectionIndex = Displayer.SelectionStart;
            Displayer.Text = Displayer.Text.Insert(selectionIndex, insertText);
            Displayer.SelectionStart = selectionIndex + insertText.Length;
            
        }

        internal void InsertTime6()
        {
            var insertText = "\\time 6/8";
            var selectionIndex = Displayer.SelectionStart;
            Displayer.Text = Displayer.Text.Insert(selectionIndex, insertText);
            Displayer.SelectionStart = selectionIndex + insertText.Length;
            
        }

        internal void InsertTempo()
        {
            var insertText = "\\tempo 4=120";
            var selectionIndex = Displayer.SelectionStart;
            Displayer.Text = Displayer.Text.Insert(selectionIndex, insertText);
            Displayer.SelectionStart = selectionIndex + insertText.Length;
           
        }

        internal void InsertClef()
        {
            var insertText = "\\clef treble";
            var selectionIndex = Displayer.SelectionStart;
            Displayer.Text = Displayer.Text.Insert(selectionIndex, insertText);
            Displayer.SelectionStart = selectionIndex + insertText.Length;
            
        }
    }
}