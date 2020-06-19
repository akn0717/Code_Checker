using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace Checking
{
    public partial class Code_Checker : Form
    {
        string ExecutePath, CurrentPath, DebugPath, CompliedPath, ObjPath, JudPath;
        bool b1, b2, b3;
        public Code_Checker()
        {
            InitializeComponent();
            ExecutePath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            CurrentPath = ExecutePath + "\\MinGW\\bin";
            DebugPath = ExecutePath + "\\Managed\\Debug";
            CompliedPath = ExecutePath + "\\Managed\\Complied";
            ObjPath = ExecutePath + "\\Managed\\Obj";
            JudPath = ExecutePath + "\\Managed\\JudgeRoom";
            Directory.CreateDirectory(ExecutePath + "\\Managed");
            try
            {
                Directory.SetCurrentDirectory(ExecutePath);
                Directory.Delete(DebugPath, true);
                Directory.Delete(CompliedPath, true);
                Directory.Delete(ObjPath, true);
                Directory.Delete(JudPath, true);
            }
            catch { }
        }

        bool compiler(string s, string t, TextBox text)
        {
            var proc = new Process();
            string t_temp = t;
            if (File.Exists(t_temp)) File.Delete(t_temp);
            Directory.SetCurrentDirectory(CurrentPath);
            s = "\"" + s + "\"";
            t = "\"" + t + "\"";
            proc.StartInfo.FileName = "mingw32-g++.exe";
            proc.StartInfo.Arguments = " -Wall -fexceptions -g -std=c++14 -c " + s + " -o " + t;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.Start();
            Thread.Sleep(500);
            string ss = proc.StandardError.ReadToEnd();
            Directory.SetCurrentDirectory(ExecutePath);
            if (File.Exists(t_temp))
            {
                text.AppendText(ss);
                text.ForeColor = Color.Green;
                text.AppendText("Complie Successful..." + Environment.NewLine);
                return true;
            }
            else
            {
                text.ForeColor = Color.Red;
                text.AppendText("Error Occurred!" + Environment.NewLine);
                text.ForeColor = Color.Black;
                text.AppendText(ss);
                text.ForeColor = Color.Red;
                text.AppendText("Complie Failed...");
                return false;
            }
        }

        void Extractor(string s, string t)
        {
            StreamWriter file = new StreamWriter(t);
            file.Flush();
            file.Write(s);
            file.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                StreamReader text_file = new StreamReader(file.FileName);
                string temp = text_file.ReadToEnd();
                AC_Code.Clear();
                AC_Code.AppendText(temp);
                text_file.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                StreamReader text_file = new StreamReader(file.FileName);
                string temp = text_file.ReadToEnd();
                Checking_Code.Clear();
                Checking_Code.AppendText(temp);
                text_file.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                StreamReader text_file = new StreamReader(file.FileName);
                string temp = text_file.ReadToEnd();
                Generator.Clear();
                Generator.AppendText(temp);
                text_file.Close();
            }
        }

        private void Start_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.SetCurrentDirectory(ExecutePath);
                Directory.Delete(DebugPath, true);
                Directory.Delete(CompliedPath, true);
                Directory.Delete(ObjPath, true);
                Directory.Delete(JudPath, true);
            }
            catch { }
            Directory.CreateDirectory(DebugPath);
            Directory.CreateDirectory(CompliedPath);
            Directory.CreateDirectory(ObjPath);
            Directory.CreateDirectory(JudPath);
            status.Text = "";
            Extractor(AC_Code.Text, Path.Combine(ObjPath, "AC.cpp"));
            Extractor(Checking_Code.Text, Path.Combine(ObjPath, "Checking.cpp"));
            Extractor(Generator.Text, Path.Combine(ObjPath, "Generator.cpp"));
            AC_Log.Clear();
            Checking_Log.Clear();
            Generator_Log.Clear();
            b1 = b2 = b3 = false;
            if (compiler(Path.Combine(ObjPath, "AC.cpp"), Path.Combine(DebugPath, "AC.temp"), AC_Log)) b1 = Build(Path.Combine(DebugPath, "AC.temp"), Path.Combine(CompliedPath, "AC.cpl"), AC_Log);
            if (compiler(Path.Combine(ObjPath, "Checking.cpp"), Path.Combine(DebugPath, "Checking.temp"), Checking_Log)) b2 = Build(Path.Combine(DebugPath, "Checking.temp"), Path.Combine(CompliedPath, "Checking.cpl"), Checking_Log);
            if (compiler(Path.Combine(ObjPath, "Generator.cpp"), Path.Combine(DebugPath, "Generator.temp"), Generator_Log)) b3 = Build(Path.Combine(DebugPath, "Generator.temp"), Path.Combine(CompliedPath, "Generator.cpl"), Generator_Log);
            if (b1 && b2 && b3)
            {
                try
                {
                    Evaluation();
                }catch
                {
                    MessageBox.Show("Try to set your time limit bigger or your code gets a runtime error!", "Runtime Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            try
            {
                Directory.SetCurrentDirectory(ExecutePath);
                Directory.Delete(DebugPath, true);
                Directory.Delete(CompliedPath, true);
                Directory.Delete(ObjPath, true);
                Directory.Delete(JudPath, true);
            }
            catch { }
        }

        bool Build(string s, string t, TextBox text)
        {
            var proc = new Process();
            Directory.SetCurrentDirectory(CurrentPath);
            string t_temp = t;
            s = "\"" + s + "\"";
            t = "\"" + t + "\"";
            if (File.Exists(t_temp)) File.Delete(t_temp);
            proc.StartInfo.FileName = "mingw32-g++.exe";
            proc.StartInfo.Arguments = " -o " + t + " " + s;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();
            Thread.Sleep(500);
            string ss = proc.StandardError.ReadToEnd();
            Directory.SetCurrentDirectory(ExecutePath);
            if (File.Exists(t_temp))
            {
                text.AppendText(ss);
                text.AppendText("Build Successful..." + Environment.NewLine);
                return true;
            }
            else
            {
                text.AppendText("Error Occurred!" + Environment.NewLine);
                text.AppendText(ss);
                text.AppendText("Build Failed...");
                return false;
            }
        }

        void run_Generator()
        {
            Directory.SetCurrentDirectory(JudPath + "\\Generator");
            Process proc = new Process();
            proc.StartInfo.FileName = "Generator.exe";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.Start();
        }

        void run_AC()
        {
            Directory.SetCurrentDirectory(JudPath + "\\AC");
            Process proc = new Process();
            proc.StartInfo.FileName = "AC.exe";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.Start();
        }

        void run_Check()
        {
            Directory.SetCurrentDirectory(JudPath + "\\Check");
            Process proc = new Process();
            proc.StartInfo.FileName = "Check.exe";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.Start();
        }

        void Evaluation()
        {
            Directory.SetCurrentDirectory(ExecutePath + "\\Managed");
            Directory.CreateDirectory("JudgeRoom");
            Directory.SetCurrentDirectory(JudPath);
            Directory.CreateDirectory("Generator");
            Directory.CreateDirectory("AC");
            Directory.CreateDirectory("Check");
            if (File.Exists(Path.Combine(JudPath, "AC\\ac.exe"))) File.Delete(Path.Combine(JudPath, "AC\\ac.exe"));
            if (File.Exists(Path.Combine(JudPath, "Generator\\generator.exe"))) File.Delete(Path.Combine(JudPath, "Generator\\generator.exe"));
            if (File.Exists(Path.Combine(JudPath, "Check\\Check.exe"))) File.Delete(Path.Combine(JudPath, "Check\\Check.exe"));
            File.Copy(Path.Combine(CompliedPath, "AC.cpl"), Path.Combine(JudPath, "AC\\ac.exe"));
            File.Copy(Path.Combine(CompliedPath, "Generator.cpl"), Path.Combine(JudPath, "Generator\\generator.exe"));
            File.Copy(Path.Combine(CompliedPath, "Checking.cpl"), Path.Combine(JudPath, "Check\\Check.exe"));
            int times = 0;
            int Limit = 0;
            if (!Int32.TryParse(time.Text, out times))
            {
                MessageBox.Show("Incorrect Format", "Error Occurred~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Int32.TryParse(TL.Text, out Limit))
            {
                MessageBox.Show("Incorrect Format", "Error Occurred~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 1; i <= times; ++i)
            {
                Directory.SetCurrentDirectory(ExecutePath);
                status.Text = "Testcase " + i + "...";
                run_Generator();
                Thread.Sleep(500);
                bool cnt = false;
                foreach (var file in Directory.GetFiles(Path.Combine(JudPath, "Generator"), "*.inp"))
                {
                    cnt = true;
                    if (File.Exists(Path.Combine(JudPath, "AC\\" + Path.GetFileName(file)))) File.Delete(Path.Combine(JudPath, "AC\\" + Path.GetFileName(file)));
                    if (File.Exists(Path.Combine(JudPath, "Check\\" + Path.GetFileName(file)))) File.Delete(Path.Combine(JudPath, "Check\\" + Path.GetFileName(file)));
                    File.Copy(file, Path.Combine(JudPath, "AC\\" + Path.GetFileName(file)));
                    File.Copy(file, Path.Combine(JudPath, "Check\\" + Path.GetFileName(file)));
                }
                if (!cnt)
                {
                    MessageBox.Show("No input file in test code!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                run_Check();
                run_AC();
                Thread.Sleep(Limit*1000*2);
                string name = null;
                foreach (var file in Directory.GetFiles(Path.Combine(JudPath, "AC"), "*.out")) name = Path.GetFileName(file);
                if (!File.Exists(JudPath + "\\AC\\" + name))
                {
                    MessageBox.Show("No output file in Accepted code!","Error!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                StreamReader file1 = new StreamReader(JudPath + "\\AC\\" + name);
                if (!File.Exists(JudPath + "\\Check\\" + name))
                {
                    MessageBox.Show("No output file in Check code!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                StreamReader file2 = new StreamReader(JudPath + "\\Check\\" + name);
                string s1 = null, s2 = null;
                bool ans = false;
                while (!file1.EndOfStream && !file2.EndOfStream)
                {
                    string f1 = file1.ReadLine().Trim();
                    string f2 = file2.ReadLine().Trim();
                    if (f1 != f2)
                    {
                        s1 += f1 + Environment.NewLine;
                        s2 += f2 + Environment.NewLine;
                        ans = true;
                    }
                }
                if (!file1.EndOfStream || !file2.EndOfStream)
                {
                    ans = true;
                    while (!file1.EndOfStream)
                    {
                        string f1 = file1.ReadLine().Trim();
                        s1 += f1 + Environment.NewLine;
                    }
                    while (!file2.EndOfStream)
                    {
                        string f2 = file1.ReadLine().Trim();
                        s2 += f2 + Environment.NewLine;
                    }
                }
                status.Text = "Testcase " + i + "...Done!";
                file1.Close();
                file2.Close();
                if (ans)
                {
                    foreach (var tmp in Directory.GetFiles(Path.Combine(JudPath, "AC"), "*.inp")) name = Path.GetFileName(tmp);
                    StreamReader file;
                    file = new StreamReader(JudPath + "\\Generator\\" + name);
                    string input_test = file.ReadToEnd();
                    file.Close();
                    Preview_window Preview = new Preview_window(input_test, s1, s2, ans);
                    Preview.ShowDialog();
                    return;
                }
            }
            status.Text = "Process Completed";
            MessageBox.Show("No Difference!!!");
        }
    }
}
