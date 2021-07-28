using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
namespace test
{
    public partial class Form1 : Form
    {
        string version = "Version 1.2.3.1 pre-release";
        string version_secret = "Made by Oixro";
        int click_number;

        string clean_done = "Обновление успешно удалено!";

        string path1 = @"C:\ProgramData\NVIDIA Corporation\Downloader";
        string path2 = @"C:\ProgramData\NVIDIA Corporation\Downloader\latest";




        public Form1()
        {
            TopMost = false;
            InitializeComponent();
        }

        public bool updateexist(string line)
        {
            return Directory.EnumerateFiles(line, "*.exe", SearchOption.AllDirectories).Any();
        }

        void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = version;
            if (!Directory.Exists(path1))
            {
                button1.Location = new Point(38, 99);
                button1.Size = new Size(224, 23);
                button1.Enabled = false;
                //button3.Enabled = false;
                button1.Text = clean_done;
                label2.Text = "Папка \"Downloader\" не существует";
            }
            else
            {
                label2.Text = (!updateexist(path1)) ? "В папке \"Downloader\" нету обновления" :
                    "В папке \"Downloader\" ЕСТЬ обновление.";
                if (!updateexist(path1))
                {
                    button1.Enabled = false;
                }
            }

            FileSystemWatcher watcher = new FileSystemWatcher(path1);
            watcher.Filter = "*.exe";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            watcher.SynchronizingObject = this;
            watcher.Created += new FileSystemEventHandler(WatcherCreated);
        }


        void WatcherCreated(object s, FileSystemEventArgs e)
        {
            button1.Location = new Point(101, 99);
            button1.Size = new Size(99, 23);
            button1.Text = "Очистить!";
            label2.Text = "Из-за запуска начало скачиватся обновление, при следующем запуске GeForce Experience  обновится!";
            button1.Enabled = true;
            button3.Enabled = true;
        }


        void cmd(string line)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c {line}",
                WindowStyle = ProcessWindowStyle.Hidden
            }).WaitForExit();
        }
        void button1_Click(object s, EventArgs e)
        {
            try
            {
                cmd("taskkill /f /im \"NVIDIA GeForce Experience.exe\"");
                cmd("taskkill /f /im \"NVIDIA Web Helper.exe\"");
                Directory.GetFiles(path1, "*.exe", SearchOption.AllDirectories).ToList().ForEach(x => File.Delete(x));
                button1.Text = clean_done;
                button1.Enabled = false;
                label2.Text = clean_done;
                foreach (var process in Process.GetProcessesByName("NVIDIA GeForce Experience"))
                {
                    process.Kill();
                }
                if (checkBox1.Checked == true)
                {
                    Process.Start(@"C:\Program Files\NVIDIA Corporation\NVIDIA GeForce Experience\NVIDIA GeForce Experience.exe");
                }
            }
            catch (Exception ex)
            {
                Clipboard.Clear();
                Clipboard.SetText(ex.ToString());
                DialogResult result = MessageBox.Show("Возникла ошибка, и она скопирована в буфер обмена.\nОткрыть блокнот что бы вставить?", "ERR0R!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    Process.Start("notepad");
                }
            }
            try
            {
                Directory.Delete(path2, true);
            }
            catch
            {

            }

        }


        void label1_MouseHover(object s, EventArgs e)
        {
            label1.Text = version_secret;
        }

        void label1_MouseLeave(object s, EventArgs e)
        {
            label1.Text = version;
        }

        void button3_Click(object s, EventArgs e)
        {
            Process.Start("explorer", path1);
        }

        void checkBox1_Click(object s, EventArgs e)
        {
            if (button1.Enabled == false)
            {
                Process.Start(@"C:\Program Files\NVIDIA Corporation\NVIDIA GeForce Experience\NVIDIA GeForce Experience.exe");
            }
        }

        void label1_Click(object s, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Открыть видео?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Process.Start("https://youtu.be/1fzLmIZ7Tgc");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            ++click_number;
            //label2.Text = $"{click_number.ToString()}";
        }
    }

}

