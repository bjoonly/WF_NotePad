using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Menu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            saveFileDialog1.DefaultExt = ".txt";
            saveFileDialog1.Filter = "Text File|*.txt";
            openFileDialog1.Filter = "Text File|*.txt";
        }
        RichTextBox CurrentTextBox
        {
            get
            {
                if (tabControl1.SelectedIndex != -1)
                {
                    return (RichTextBox)tabControl1.SelectedTab.Controls[0];
                }
                return null;
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextChange();
        }
        private void TextChange()
        {
            int countLetters = 0;
            int countNumbers = 0;
            countSymbolsToolStripStatusLabel2.Text = CurrentTextBox.Text.Length.ToString();
            for (int i = 0; i < CurrentTextBox.Text.Length; i++)
            {
                if (Char.IsLetter(CurrentTextBox.Text[i]))
                    ++countLetters;
                else if (Char.IsDigit(CurrentTextBox.Text[i]))
                    ++countNumbers;

            }
            countNumbersToolStripStatusLabel3.Text = countNumbers.ToString();
            countLettersToolStripStatusLabel.Text = countLetters.ToString();
        }
      
        private void SaveAs()
        {
            if (CurrentTextBox == null )
                return;
            
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, CurrentTextBox.Text);
                tabControl1.SelectedTab.Text = saveFileDialog1.FileName;
            }
       

        }
        private void Save()
        {
            if (CurrentTextBox == null )
                return;
            string name;
            if (File.Exists(tabControl1.SelectedTab.Text))
                    name = tabControl1.SelectedTab.Text;
            else
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    name = saveFileDialog1.FileName;
                else
                    name = tabControl1.SelectedTab.Text + ".txt";
            }
                File.WriteAllText(name, CurrentTextBox.Text);

            tabControl1.SelectedTab.Text = name;
        }
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Save();
        }
        private void saveAsToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {


            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                if (CurrentTextBox == null)
                    CreateTab(openFileDialog1.FileName);

                else if (!String.IsNullOrWhiteSpace(CurrentTextBox.Text) )
                {
                    var res = MessageBox.Show("Overwrite current tab?", "Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res == DialogResult.No)
                    {
                        CreateTab(openFileDialog1.FileName);
                        tabControl1.SelectedIndex = tabControl1.TabCount - 1;
                    }
                  
                }
      
                CurrentTextBox.Text = File.ReadAllText(openFileDialog1.FileName);
                tabControl1.SelectedTab.Text = openFileDialog1.FileName;
      
            }
          
        }

        private void undoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CurrentTextBox.Undo();
            
        }

        private void cutToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            CurrentTextBox.Cut();
        }

        private void copyToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            CurrentTextBox.Copy();
        }

        private void PasteTolStripMenuItem4_Click(object sender, EventArgs e)
        {
            CurrentTextBox.Paste();
        }

        private void colorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                CurrentTextBox.SelectionColor = colorDialog1.Color;
               
        }

        private void fontToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
                CurrentTextBox.SelectionFont = fontDialog1.Font;
              
        }

        private void backgroundColorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                CurrentTextBox.BackColor = colorDialog1.Color;
        }

        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

   
        private void clearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CurrentTextBox.Clear();
        }

        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CurrentTextBox.SelectAll();
        }

        private void dateAndTimeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CurrentTextBox.AppendText(DateTime.Now.ToString());
        }

        private void deleteToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            CurrentTextBox.Text = CurrentTextBox.Text.Remove(CurrentTextBox.SelectionStart, CurrentTextBox.SelectionLength);
        }
        private void CreateTab(string name="New_Tab")
        {

            if (name == "New_Tab")
                name += $"_{tabControl1.TabPages.Count + 1}";

             TabPage newPage = new TabPage(name);
            newPage.Location = new System.Drawing.Point(4, 22);
            newPage.Padding = new System.Windows.Forms.Padding(3);
            newPage.Size = new System.Drawing.Size(613, 432);
            newPage.UseVisualStyleBackColor = true;

            RichTextBox newTextBox = new RichTextBox();
            newTextBox.ContextMenuStrip = this.contextMenuStrip1;
            newTextBox.Location = new System.Drawing.Point(0, 0);
            newTextBox.Multiline = true;
            newTextBox.Name = "richtextBox1";
            newTextBox.Size = Size = new System.Drawing.Size(613, 432);
            ContextMenuStrip = this.contextMenuStrip1;
            newTextBox.TextChanged += textBox1_TextChanged;
            newPage.Controls.Add(newTextBox);

            tabControl1.TabPages.Add(newPage);
        }
        private void newTabPageToolStripButton1_Click(object sender, EventArgs e)
        {
            CreateTab();
   
        }

        private void closeTabPageToolStripButton2_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == -1 )
                return;
            if ( MessageBox.Show("Do you want to save data to file?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (File.Exists(tabControl1.SelectedTab.Text))
                    Save();
                else
                    SaveAs();
            }
            tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            countSymbolsToolStripStatusLabel2.Text = "0";
            countNumbersToolStripStatusLabel3.Text = "0";
            countLettersToolStripStatusLabel.Text = "0";
            if (CurrentTextBox != null)
                TextChange();
           


        }

       
    }
}
