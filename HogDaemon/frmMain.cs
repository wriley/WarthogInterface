using System;
using System.Collections;
using System.Windows.Forms;
using IniFile;
using JoystickInterface;

namespace WarthogInterface
{
    public partial class frmMain : Form
    {
        private static string _myIniFileName = "WarthogInterface.ini";
        private IniFileReader _ifr;
        private EvaluationEngine.Parser.Token _token = null;
        private EvaluationEngine.Evaluate.Evaluator _eval = null;
        private ArrayList _commands = new ArrayList();
        private Joystick _joystick;
        private string _currentJoystick = null;

        private static string S_ID = "DeviceID";
        private static string S_RULE = "Rule";
        private static string S_BUTTON = "Button";
        private static string S_JOYSTICK = "Joystick";
        private static string S_SETTINGS = "Settings";

        private string _outputString = "";

        DateTime start;
        DateTime stop;
        int _numCmds = 0;

        //for debug
        bool[] inputs = new bool[8];

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _ifr = new IniFileReader(_myIniFileName, true);
            _ifr.OutputFilename = _myIniFileName;
            _loadIni();
            timer1.Enabled = true;
        }

        private void _loadIni()
        {
            _commands.Clear();
            foreach (string s in _ifr.AllSections)
            {
                if (s == S_SETTINGS)
                {
                    string j = _ifr.GetIniValue(s, S_JOYSTICK);
                    if (j != null)
                    {
                        _currentJoystick = j;
                    }
                }
                else
                {
                    string id = _ifr.GetIniValue(s, S_ID);
                    string button = _ifr.GetIniValue(s, S_BUTTON);
                    string rule = _ifr.GetIniValue(s, S_RULE);
                    if ((id != null) && (rule != null))
                    {
                        Command c = new Command(s, id, button, rule);
                        _commands.Add(c);
                    }
                }
            }

            tbDebug.AppendText("Loaded " + _commands.Count.ToString() + " commands from ini file\n");
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            inputs[0] = checkBox1.Checked;
            inputs[1] = checkBox2.Checked;
            inputs[2] = checkBox3.Checked;
            inputs[3] = checkBox4.Checked;
            inputs[4] = checkBox5.Checked;
            inputs[5] = checkBox6.Checked;
            inputs[6] = checkBox7.Checked;
            inputs[7] = checkBox8.Checked;

            _numCmds = 0;
            _outputString = "";

            start = DateTime.UtcNow;

            foreach (Command c in _commands)
            {

                try
                {
                    _token = new EvaluationEngine.Parser.Token(c.Rule);
                }
                catch (Exception err)
                {
                    MessageBox.Show("Failed to load the rule for " + c.Name + ": " + err.Message);
                    return;
                }

                if (_token.AnyErrors == true)
                {
                    MessageBox.Show("Error while loading and parsing the tokens for " + c.Name + ": " + _token.LastErrorMessage);
                    return;
                }

                _eval = new EvaluationEngine.Evaluate.Evaluator(_token);

                if ((_token != null) && (_eval != null))
                {
                    foreach (EvaluationEngine.Parser.Variable v in _token.Variables)
                    {
                        if (v.VariableName.Contains("input"))
                        {
                            int i = 0;
                            try
                            {
                                i = Int32.Parse(v.VariableName.Substring(5)) - 1;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error parsing variable name for " + c.Name + ": " + ex.Message);
                                return;
                            }
                            v.VariableValue = inputs[i].ToString();
                        }
                    }

                    string ErrorMsg = "";
                    string result = "";
                    if (_eval.Evaluate(out result, out ErrorMsg) == false)
                    {
                        MessageBox.Show("Error evaluating the tokens for " + c.Name + ": " + ErrorMsg);
                    }
                    else
                    {
                        _outputString += "C" + c.DeviceID + "," + c.Button + "," +  _token.Variables["output"].VariableValue.ToString() + "\r\n";
                    }

                    _numCmds++;
                }
            }

            stop = DateTime.UtcNow;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            tbOutput.Clear();
            tbOutput.Text = _outputString;
            TimeSpan interval = stop - start;
            tbDebug.Text = "Processed " + _numCmds + " commands in " + interval.ToString("g");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy == false)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }
    }
}
