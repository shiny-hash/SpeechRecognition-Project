using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();
        SpeechSynthesizer Sarah = new SpeechSynthesizer();
        SpeechRecognitionEngine startlistining = new SpeechRecognitionEngine();
        Random rnd = new Random();
        int RecTimeOut = 0;
       DateTime TimeNow = DateTime.Now;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Default_SpeechRecognized);
            _recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(_recognizer_SpeechRecognized);
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);


            startlistining.SetInputToDefaultAudioDevice();
            startlistining.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            startlistining.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(startlistining_SpeechRecognized);
        }

        private void Default_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            int ranNum;
            string speech = e.Result.Text;

            if (speech == "Hello")
            {
                Sarah.SpeakAsync("Hello,I am here");
            }
            if (speech == "How are you")
            {
                Sarah.SpeakAsync("I am working good");
            }
            if (speech == "What time is it")
            {
                Sarah.SpeakAsync(DateTime.Now.ToString("h mm tt"));
            }
            if( speech == "Stop talking")
            {
                Sarah.SpeakAsyncCancelAll();
                ranNum = rnd.Next(1, 5);

                if(ranNum ==1)
                {
                    Sarah.SpeakAsync("Yes,sir");

                }
                if(ranNum == 2)
                {
                    Sarah.SpeakAsync("I am sorry i will be quite");
                }
            }
            if(speech == "Stop listining")
            {
                Sarah.SpeakAsync("If you need me just ask");
                _recognizer.RecognizeAsyncCancel();
                startlistining.RecognizeAsync(RecognizeMode.Multiple);
            }
            if (speech == "Show commands")
            {
                string[] commands = (File.ReadAllLines(@"DefaultCommands.txt"));
                lstCommands.Items.Clear();
                lstCommands.SelectionMode = SelectionMode.None;
                lstCommands.Visible = true;
                foreach(string command in commands)
                {
                    lstCommands.Items.Add(command);
                }
            }
            if(speech == "Hide commands")
            {
                lstCommands.Visible = false;
            }
        }

        private void _recognizer_SpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            RecTimeOut = 0;
        }
        private void startlistining_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;
            if(speech =="Wakeup")
            {
                startlistining.RecognizeAsyncCancel();
                Sarah.SpeakAsync("yes, I am here");
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        private void TmrSpeaking_Tick(object sender, EventArgs e)
        {
            if(RecTimeOut == 10)
            {
                _recognizer.RecognizeAsyncCancel();
            }
            else if(RecTimeOut == 11)
            {
                TmrSpeaking.Stop();
                startlistining.RecognizeAsync(RecognizeMode.Multiple);
                RecTimeOut = 0;
            }
        }
    }
}
