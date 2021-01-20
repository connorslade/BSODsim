using System;
using System.Windows.Forms;

namespace BSODsim
{
    public partial class Setup : Form
    {
        private System.Drawing.Color SettingsBackgroundColor;
        private string SettingsEmote = ":(";
        private string SettingsMain1 = "Your PC ran into a problem and needs to restart.";
        private string SettingsMain2 = "We're just collecting some error info, and then we'll restart for you.";
        private string SettingsComplete = "complete";
        private string SettingsPercent = "0";
        private string SettingsInfo = "If you call a support person, give then this info:";
        private string SettingsInfomation = "For more information about this issue and possible fixes, visit https://www.windows.com/stopcode";
        private string SettingsStop = "Stop code: CACHE MANAGER";
        private string SettingsFail = "What failed: FLTMGR.SYS";
        private bool SettingsCount = false;

        public Setup()
        {
            InitializeComponent();
            SettingsBackgroundColor = panel1.BackColor;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int intPercent = int.Parse(SettingsPercent);
                var m = new BSOD(false, SettingsBackgroundColor, SettingsEmote, SettingsMain1, SettingsMain2, SettingsComplete, SettingsInfo, SettingsInfomation, SettingsStop, SettingsFail, SettingsCount, intPercent);
                m.Show();
            }
            catch
            {
                MessageBox.Show("You may only enter Numbers for \"StartPercent\"", "BSOD Simulator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDlg = colorDialog1;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = colorDlg.Color;
                SettingsBackgroundColor = colorDlg.Color;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SettingsEmote = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            SettingsMain1 = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            SettingsMain2 = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            SettingsComplete = textBox4.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            SettingsPercent = textBox5.Text;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            SettingsInfo = textBox7.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            SettingsStop = textBox8.Text;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            SettingsFail = textBox9.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            SettingsInfomation = textBox6.Text;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SettingsCount = checkBox1.Checked;
        }
    }
}