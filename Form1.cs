using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;

namespace AppLauncher
{
    public partial class Form1 : Form
    {
        private FlowLayoutPanel panel1;
        private Panel mainPanel;
        private const int headersz = 1024;
        PictureBox spbox;
        Label slbl;
        public Form1()
        {
            InitializeComponent();
            panel();
            loadall("data.txt");
        }

        private void panel()
        {
            panel1 = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };
            panel2.Controls.Add(panel1);

        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dd = new OpenFileDialog();
            dd.DefaultExt = ".exe";
            dd.Filter = "Arquivos Executáveis (*.exe)|*.exe|Todos os Arquivos (*.*)|*.*";
            dd.FilterIndex = 1;

            if (dd.ShowDialog() == DialogResult.OK)
            {
                NewExe(dd.FileName, true);
            }
        }
        private void NewExe(string fpath, bool save)
        {
            Icon icon = Icon.ExtractAssociatedIcon(fpath);
            String path = Path.GetFileName(fpath);
            Bitmap bmp = icon.ToBitmap();


            PictureBox pbox = new PictureBox
            {
                Size = new System.Drawing.Size(48, 48),
                Image = bmp,
                Margin = new Padding(0, 5, 0, 0)
            };
            Label label = new Label
            {
                Text = fpath,
                Width = 300,
                TextAlign = ContentAlignment.MiddleLeft
            };
            label.DoubleClick += (sender, e) => Process.Start(fpath);
            pbox.DoubleClick += (sender, e) => Process.Start(fpath);

            label.Click += (sender, e) =>
            {
                if (spbox != null && slbl != null)
                {
                    spbox.BackColor = SystemColors.Control;
                    slbl.BackColor = SystemColors.Control;
                }
                spbox = pbox;
                slbl = label;
                pbox.BackColor = Color.Blue;
                label.BackColor = Color.Blue;
            };
            pbox.Click += (sender, e) =>
            {
                if (spbox != null && slbl != null)
                {
                    spbox.BackColor = SystemColors.Control;
                    slbl.BackColor = SystemColors.Control;
                }
                spbox = pbox;
                slbl = label;
                pbox.BackColor = Color.Blue;
                label.BackColor = Color.Blue;
            };

            panel1.Controls.Add(pbox);
            panel1.Controls.Add(label);
            if (save)
                saveExe(fpath);
        }
        void saveExe(string path)
        {
            if (!exeexists(path))
            {
                using (StreamWriter f = new StreamWriter("data.txt", true))
                {
                    f.Write(path);
                    f.Write(Environment.NewLine);
                    f.Close();
                }
            }

        }
        bool exeexists(string path)
        {
            bool b = false;
            foreach (var line in File.ReadAllLines("data.txt"))
            {
                if (line == path)
                {
                    b = true;
                    break;
                }
            }
            return b;
        }
        void loadall(string path)
        {
            if (File.Exists(path))
            {
                foreach (var line in File.ReadAllLines(path))
                {
                    NewExe(line, false);
                }
            }
            else
            {
                File.Create("data.txt");
            }
        }
        private void rmv(string path)
        {
            var lines = File.ReadAllLines("data.txt").Where(line => line != path);
            File.WriteAllLines("data.txt", lines);
            panel1.Controls.Remove(spbox);
            panel1.Controls.Remove(slbl);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rmv(slbl.Text);
        }
    }
}


