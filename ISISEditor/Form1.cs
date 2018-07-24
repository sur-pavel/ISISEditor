using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISISEditor
{
    public partial class Form1 : Form
    {
        Logging logging = new Logging();
        IrbisHandler irbisHandler = new IrbisHandler();
        public Form1()
        {
            InitializeComponent();
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logging.CreateLogFile();
            irbisHandler.logging = logging;
            this.comboBox1.SelectedIndex = 0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            irbisHandler.Disconnect();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                irbisHandler.CopyRecord(
                this.numericUpDown1.Text,
                this.numericUpDown2.Text,
                this.comboBox1.Text);
                MessageBox.Show("Done!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!");
                logging.WriteLine("IRBIS ERROR! " + ex);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {            
                irbisHandler.Connect(
                this.textBox1.Text,
                this.textBox2.Text,
                this.textBox3.Text);              
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
