using System;
using System.Collections;
using System.Windows.Forms;
using IniFile;
using JoystickInterface;
using System.Net.Sockets;
using System.Text;
using WarthogInterface;
using System.Threading;

namespace WarthogInterface
{
    public partial class frmMain : Form
    {
        private static string _myIniFileName = "WarthogInterface.ini";
        private IniFileReader _iniFileReader;
        private EvaluationEngine.Parser.Token _token = null;
        private EvaluationEngine.Evaluate.Evaluator _eval = null;
        private ArrayList _commands = new ArrayList();
        private Joystick _joystick;
        private string _selectedJoystick = null;
        private string _hostname = null;
        private int _port = -1;
        private UdpClient _udpClient;
        private bool _started = false;
        private double _activityRate = 0;
        private int _activityCounter = 0;
        
        private const string S_ID           = "DeviceID";
        private const string S_RULE         = "Rule";
        private const string S_BUTTON       = "Button";
        private const string S_SETTINGS     = "Settings";
        private const string S_JOYSTICK     = "Joystick";
        private const string S_HOSTNAME     = "Hostname";
        private const string S_PORT         = "Port";

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            bool firstInstance;
            Mutex mutex = new Mutex(false, "Local\\HogDaemon", out firstInstance);

            if (!firstInstance)
            {
                MessageBox.Show("There is already an instance running, exiting!");
                Application.Exit();
            }

            _loadIni();
            _joystick = new Joystick(this.Handle);
            updateDeviceList();
        }

        private void updateDeviceList()
        {
            string[] discoveredJoysticks = _joystick.FindJoysticks();

            if (discoveredJoysticks != null)
            {
                cbActiveDevice.Items.Clear();
                cbActiveDevice.Items.AddRange(discoveredJoysticks);
            }

            if (_selectedJoystick != null)
            {
                try
                {
                    cbActiveDevice.SelectedValue = _selectedJoystick;
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show("Could not find " + _selectedJoystick + " in detected devices\n" + ex.Message.ToString());
                    _selectedJoystick = null;
                }
            }
        }

        private void cbActiveDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedJoystick = cbActiveDevice.Items[cbActiveDevice.SelectedIndex].ToString();
            connectJoystick();
        }

        private void _loadIni()
        {
            _iniFileReader = new IniFileReader(_myIniFileName, true);
            _iniFileReader.OutputFilename = _myIniFileName;

            _commands.Clear();
            foreach (string s in _iniFileReader.AllSections)
            {
                switch(s)
                {
                    case S_SETTINGS:
                        string j = _iniFileReader.GetIniValue(s, S_JOYSTICK);
                        if (j != null)
                        {
                            _selectedJoystick = j;
                        }
                        string h = _iniFileReader.GetIniValue(s, S_HOSTNAME);
                        if (h != null)
                        {
                            _hostname = h;
                        }
                        string p = _iniFileReader.GetIniValue(s, S_PORT);
                        if (p != null)
                        {
                            try
                            {
                                _port = int.Parse(p);
                            }
                            catch (FormatException ex)
                            {
                                MessageBox.Show("Error reading port from " + _myIniFileName + "\n" + ex.Message.ToString());
                            }
                        }
                        break;
                    default:
                        string id = _iniFileReader.GetIniValue(s, S_ID);
                        string button = _iniFileReader.GetIniValue(s, S_BUTTON);
                        string rule = _iniFileReader.GetIniValue(s, S_RULE);
                        if (id != null)
                        {
                            Command c = new Command(s, id, button, rule);
                            _commands.Add(c);
                        }
                        break;
                }
            }

            tbHostname.Text = _hostname;
            tbPort.Text = _port.ToString();

            lblRulesNum.Text = _commands.Count.ToString();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if ((_selectedJoystick != null) && (_joystick.Buttons.Length > 0))
            {
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
                                int i = -1;

                                try
                                {
                                    i = Int32.Parse(v.VariableName.Substring(5)) - 1;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error parsing variable name for " + c.Name + ": " + ex.Message);
                                    return;
                                }

                                v.VariableValue = _joystick.Buttons[i] ? "1" : "0";
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
                            if (c.LastOutput == null)
                            {
                                c.LastOutput = _token.Variables["output"].VariableValue.ToString();
                            }
                            else
                            {
                                c.LastOutput = c.CurrentOutput;
                            }
                            c.CurrentOutput = _token.Variables["output"].VariableValue.ToString();
                        }
                    }
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            DateTime start = DateTime.UtcNow;
            sendToNetwork();
            DateTime stop = DateTime.UtcNow;
            TimeSpan t = stop - start;
            if (t.TotalMilliseconds > 0)
            {
                double r = (_commands.Count / t.TotalMilliseconds) * 1000;
                _activityRate += r;
                _activityRate /= 2;
            }
            _activityCounter++;
            if (_activityCounter == (int)(1000 / timer1.Interval))
            {
                lblCmdNum.Text = _activityRate.ToString("f0");
                _activityCounter = 0;
            }
        }

        private void sendToNetwork()
        {
            foreach (Command c in _commands)
            {
                if (c.CurrentOutput != c.LastOutput)
                {
                    string sendString = "C" + c.DeviceID + "," + c.Button + "," + c.CurrentOutput;
                    Byte[] sendBytes = Encoding.ASCII.GetBytes(sendString);
                    try
                    {
                        _udpClient.Send(sendBytes, sendBytes.Length);
                    }
                    catch (Exception ex)
                    {
                        _stop();
                        MessageBox.Show("Error sending to network: " + ex.Message.ToString());
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy == false)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void _stop()
        {
            timer1.Enabled = false;
            lblCmdNum.Text = "0";
            _started = false;
            _setInputs(true);
            btnStartStop.Text = "Start";
            _udpClient.Close();
        }

        private void _start()
        {
            timer1.Enabled = true;
            _started = true;
            _setInputs(false);
            btnStartStop.Text = "Stop";
            try
            {
                _udpClient = new UdpClient(_hostname, _port);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to " + _hostname + ":" + _port.ToString() + "\n" + ex.Message.ToString());
            }
        }

        private void _setInputs(bool mode)
        {
            cbActiveDevice.Enabled = mode;
            tbHostname.Enabled = mode;
            tbPort.Enabled = mode;
            btnReload.Enabled = mode;
            btnSave.Enabled = mode;
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (_started)
            {
                _stop();
            }
            else
            {
                _start();
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            string lastJoystick = _selectedJoystick;
            _loadIni();
            if (_selectedJoystick != lastJoystick)
            {
                connectJoystick();
            }
        }

        private void connectJoystick()
        {
            if (_joystick.AcquireJoystick(_selectedJoystick))
            {
                // yay?
            }
            else
            {
                MessageBox.Show("Error: could not acquire joystick " + _selectedJoystick);
                _selectedJoystick = null;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _iniFileReader = new IniFileReader(_myIniFileName, true);
            _iniFileReader.OutputFilename = _myIniFileName;
            if (tbHostname.Text.Length > 0)
            {
                _iniFileReader.SetIniValue(S_SETTINGS, S_HOSTNAME, tbHostname.Text);
                _hostname = tbHostname.Text;
            }
            if (tbPort.Text.Length > 0)
            {
                _iniFileReader.SetIniValue(S_SETTINGS, S_PORT, tbPort.Text);
                try
                {
                    _port = Int32.Parse(tbPort.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error setting port to " + tbPort.Text.ToString() + ": " + ex.Message.ToString());
                }
            }
            if (cbActiveDevice.SelectedIndex >= 0)
            {
                _iniFileReader.SetIniValue(S_SETTINGS, S_JOYSTICK, cbActiveDevice.SelectedValue.ToString());
            }
            _iniFileReader.Save();
        }
    }
}
