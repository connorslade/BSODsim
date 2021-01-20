using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Utilities;

namespace BSODsim
{
    public partial class BSOD : Form
    {
        private System.Windows.Forms.Keys[] keysToAdd = { Keys.F8, Control.ModifierKeys };
        private globalKeyboardHook gkh = new globalKeyboardHook();
        private string exitKey = "F8";
        private bool preview = false;
        private Thread workerThread;
        Random rnd = new Random();
        private string complete;
        private bool doCount;
        private int percent;

        private void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (e.KeyCode.ToString() == exitKey)
                {
                    this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.BSOD_FormClosing);
                    if (doCount && percent < 100) { workerThread.Abort(); }
                    gkh.unhook();
                    Cursor.Show();
                    this.Close();
                }
            }

            e.Handled = true;
        }

        private void BSOD_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        public BSOD(bool Preview, System.Drawing.Color BackgroundColor, string Emote, string Main1, string Main2, string Complete, string Info, string Infomation, string Stop, string Fail, bool Count, int IntPercent)
        {
            InitializeComponent();

            preview = Preview;
            this.BackColor = BackgroundColor;
            this.label1.Text = Emote;
            this.label2.Text = Main1;
            this.label8.Text = Main2;
            this.label3.Text = IntPercent.ToString() + "% " + Complete;
            this.label5.Text = Info;
            this.label4.Text = Infomation;
            this.label6.Text = Stop;
            this.label7.Text = Fail;

            doCount = Count;
            percent = IntPercent;
            complete = Complete;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!preview)
            {
                Cursor.Hide();
                ProcessModule objCurrentModule = Process.GetCurrentProcess().MainModule;
                objKeyboardProcess = new LowLevelKeyboardProc(captureKey);
                ptrHook = SetWindowsHookEx(13, objKeyboardProcess, GetModuleHandle(objCurrentModule.ModuleName), 0);
                this.TopMost = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
            foreach (System.Windows.Forms.Keys addKey in keysToAdd)
            {
                gkh.HookedKeys.Add(addKey);
            }
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);

            if (doCount && percent < 100)
            {
                this.workerThread = new Thread(new ThreadStart(this.countUp));
                this.workerThread.Start();
            }
        }

        private void countUp()
        {
            Thread.Sleep(rnd.Next(1000, 5000));
            for (int i = percent; i < 100; i = i + rnd.Next(1, 6))
            {
                string newText = i.ToString() + "% " + complete;
                try
                {
                    label1.Invoke((MethodInvoker)delegate {label3.Text = newText;});
                }
                catch { }
                Thread.Sleep(rnd.Next(600,901));
            }
            try
            {
                label1.Invoke((MethodInvoker)delegate { label3.Text = "100% " + complete; });
            }
            catch { }
        }


        #region Disable Keybored

        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public Keys key;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr extra;
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int id, LowLevelKeyboardProc callback, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, IntPtr wp, IntPtr lp);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string name);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern short GetAsyncKeyState(Keys key);

        private IntPtr ptrHook;
        private LowLevelKeyboardProc objKeyboardProcess;

        private IntPtr captureKey(int nCode, IntPtr wp, IntPtr lp)
        {
            if (nCode >= 0)
            {
                KBDLLHOOKSTRUCT objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lp, typeof(KBDLLHOOKSTRUCT));

                // Disabling Windows keys

                if (objKeyInfo.key == Keys.RWin || objKeyInfo.key == Keys.LWin || objKeyInfo.key == Keys.Tab && HasAltModifier(objKeyInfo.flags) || objKeyInfo.key == Keys.Escape && (ModifierKeys & Keys.Control) == Keys.Control)
                {
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(ptrHook, nCode, wp, lp);
        }

        private bool HasAltModifier(int flags)
        {
            return (flags & 0x20) == 0x20;
        }

        #endregion Disable Keybored
    }
}