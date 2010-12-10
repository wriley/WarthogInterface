using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EvaluationEngine;
using IniFile;
using System.Collections;
using JoystickInterface;

namespace ConfigBuilder
{
    public partial class frmMain : Form
    {
        private static string _myIniFileName = "WarthogInterface.ini";
        private IniFileReader _ifr;
        private EvaluationEngine.Parser.Token _token = null;
        private EvaluationEngine.Evaluate.Evaluator _eval = null;
        private ArrayList _commands = new ArrayList();
        private Command _selectedCommand = null;
        private Joystick _joystick = null;
        private string _currentJoystick = null;

        private static string S_ID = "ID";
        private static string S_RULE = "Rule";
        private static string S_JOYSTICK = "Joystick";
        private static string S_SETTINGS = "Settings";

        //for debug
        bool[] inputs = new bool[4];

        public frmMain()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout a = new frmAbout();
            a.ShowDialog(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _setInputFields(true, true, 1);
        }

        private void _setInputFields(bool enable, bool clear, int mode = 0)
        {
            if (clear)
            {
                tbName.Text = "";
                tbID.Text = "";
                tbRule.Text = "";
                lblDisplayValue.Text = "###";
            }

            tbName.Enabled = Enabled && (mode == 1);
            tbID.Enabled = enable;
            tbRule.Enabled = enable;
            btnTest.Enabled = enable;
            btnSave.Enabled = enable;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _validateInputs();

            int i = _getCommandID(tbName.Text);

            if (i >= 0)
            {
                _commands.RemoveAt(i);
            }


            Command c = new Command(tbName.Text, tbID.Text, tbRule.Text);
            _commands.Add(c);
            _updateCommandList();
            _saveIni();

            _setInputFields(false, true);
        }

        private void _updateCommandList()
        {
            lbCommands.Items.Clear();
            foreach (Command c in _commands)
            {
                lbCommands.Items.Add(c.Name);
            }
        }

        private bool _validateInputs()
        {
            return true;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            inputs[0] = checkBox1.Checked;
            inputs[1] = checkBox2.Checked;
            inputs[2] = checkBox3.Checked;
            inputs[3] = checkBox4.Checked;

            try
            {
                _token = new EvaluationEngine.Parser.Token(tbRule.Text);
            }
            catch (Exception err)
            {
                MessageBox.Show("Failed to load the rule: " + err.Message);
                return;
            }

            if (_token.AnyErrors == true)
            {
                MessageBox.Show("Error while loading and parsing the tokens: " + _token.LastErrorMessage);
                return;
            }

            _eval = new EvaluationEngine.Evaluate.Evaluator(_token);

            if ((_token != null) && (_eval != null))
            {
                foreach (EvaluationEngine.Parser.Variable v in _token.Variables.ToArray())
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
                            MessageBox.Show("Error parsing variable name: " + ex.Message);
                            return;
                        }
                        v.VariableValue = inputs[i].ToString();
                    }
                }

                string ErrorMsg = "";
                string result = "";
                if (_eval.Evaluate(out result, out ErrorMsg) == false)
                {
                    MessageBox.Show("Error evaluating the tokens: " + ErrorMsg);
                }
                else
                {
                    lblDisplayValue.Text = _token.Variables["output"].VariableValue.ToString();
                }
            }
        }

        private void lbCommands_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (lbCommands.SelectedIndex >= 0))
            {
                _selectedCommand = _searchCommandName(lbCommands.SelectedItem.ToString());
                if (_selectedCommand != null)
                {
                    tbName.Text = _selectedCommand.Name;
                    if (_selectedCommand.ID != null)
                    {
                        tbID.Text = _selectedCommand.ID.ToString();
                    }
                    if (_selectedCommand.Rule != null)
                    {
                        tbRule.Text = _selectedCommand.Rule;
                    }
                    _setInputFields(true, false);
                }
            }
        }

        private Command _searchCommandName(string n)
        {
            foreach (Command c in _commands)
            {
                if (c.Name == n)
                {
                    return c;
                }
            }

            return null;
        }

        private int _getCommandID(string n)
        {
            int i = 0;
            foreach (Command c in _commands)
            {
                if (c.Name == n)
                {
                    return i;
                }
                i++;
            }

            return -1;
        }

        private void _saveIni()
        {
            foreach (string s in _ifr.AllSections)
            {
                _ifr.SetIniValue(s, null, null);
            }

            foreach (Command c in _commands)
            {
                _ifr.SetIniValue(c.Name, S_ID, c.ID);
                _ifr.SetIniValue(c.Name, S_RULE, c.Rule);
            }

            if (_currentJoystick != null)
            {
                _ifr.SetIniValue(S_SETTINGS, S_JOYSTICK, _currentJoystick);
            }

            _ifr.Save();
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
                    string rule = _ifr.GetIniValue(s, S_RULE);
                    if ((id != null) && (rule != null))
                    {
                        Command c = new Command(s, id, rule);
                        _commands.Add(c);
                        _updateCommandList();
                    }
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _ifr = new IniFileReader(_myIniFileName, true);
            _ifr.OutputFilename = _myIniFileName;
            _loadIni();
            if (_currentJoystick != null)
            {
                if (_joystick.AcquireJoystick(_currentJoystick))
                {
                }
                else
                {
                    MessageBox.Show("Could not acquire joystick: " + _currentJoystick);
                }
            }
        }

        internal string[] getJoystickNames()
        {
            return _joystick.FindJoysticks();
        }

        internal void setJoystick(string p)
        {
            _currentJoystick = p;
        }
    }
}
