using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using database_viewer.Properties;

namespace database_viewer
{
    public partial class Main : Form
    {
        private readonly List<string> arrays = new List<string>();

        public Main()
        {
            InitializeComponent();
            if (Settings.Default.IsLinkVisited) webPage.LinkVisited = true;
        }

        private void OnCheckBoxClicked(object sender, EventArgs e)
        {
            textLabel02.Enabled = !textLabel02.Enabled;
            textLabel03.Enabled = !textLabel03.Enabled;
            userID.Enabled = !userID.Enabled;
            itemID.Enabled = !itemID.Enabled;
        }

        private void OnDefineLocationClicked(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!openFileDialog1.FileName.EndsWith(".txt"))
                {
                    MessageBox.Show("Database Extension is \".txt\".", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                databaseLocation.Text = openFileDialog1.FileName;

                string file = openFileDialog1.FileName;
                FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                try
                {

                    while (sr.EndOfStream == false)
                    {
                        string line = sr.ReadLine();
                        arrays.Add(line);
                    }

                    textLabel01.Enabled = true;
                    textLabel03.Enabled = true;
                    itemID.Enabled = true;
                    checkBox01.Enabled = true;
                    searchButton.Enabled = true;
                }

                catch (Exception err)
                {
                    MessageBox.Show("Oops, error was occurred.\nDo you specify incorrect file?\n\nError:\n" + err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                finally
                {
                    sr.Close();
                    fs.Close();
                }
            }
        }

        private void OnSearchButtonClicked(object sender, EventArgs e)
        {
            if (userID.Enabled == true && userID.Text == string.Empty)
            {
                MessageBox.Show("User not specified!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else if (userID.Enabled == true)
            {
                ShowAll();
            }

            else
            {
                if (itemID.Text == string.Empty)
                {
                    MessageBox.Show("ItemID not specified!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ShowMsg(itemID.Text);
            }
        }

        private void ShowAll()
        {
            string result = string.Empty;
            int i = 0;

            while (i != arrays.Count)
            {
                string text = arrays[i];
                if (text.StartsWith(userID.Text))
                {
                    result += text.Split(";"[0])[1] + "\n";
                }

                i++;
            }

            MessageBox.Show(result);

        }

        private void ShowMsg(string item)
        {
            try
            {
                string result = string.Empty;
                int i = 0;

                while (i != arrays.Count)
                {
                    string text = arrays[i];
                    if (text.Split(";"[0])[2] == item)
                    {
                        result += text.Split(";"[0])[1];
                    }

                    i++;
                }

                MessageBox.Show(result);
            }

            catch (Exception err)
            {
                MessageBox.Show("An exception has occurred.\n\nError:\n" + err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void OnWebPageClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dr = MessageBox.Show("このリンクは github.com に飛びます。よろしいですか？", "Go to Website", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {
                webPage.LinkVisited = true;
                Settings.Default.IsLinkVisited = true;
                Settings.Default.Save();
                Process.Start("https://github.com/yuto0214w/database-viewer");
            }
        }
    }
}
