using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Llibreria
{
    public partial class BaseCom : UserControl
    {
        private int paquetsrebuts = 0;
        private int paquetsenviats = 0;
        public BaseCom()
        {
            InitializeComponent();
        }

        public void IncrementaRebuts()
        {
            if (textBox3.InvokeRequired)
            {
                textBox3.Invoke(new Action(IncrementaRebuts));
            }
            else
            {
                paquetsrebuts++;
                paquetsrebuts = paquetsrebuts % 100000;
                textBox3.Text = paquetsrebuts.ToString();
            }

        }
        public void IncrementaEnviats()
        {
            if (textBox2.InvokeRequired)
            {
                textBox2.Invoke(new Action(IncrementaEnviats));
            }
            else
            {
                paquetsenviats++;
                paquetsenviats = paquetsenviats % 100000;
                textBox2.Text = paquetsenviats.ToString();
            }
        }
        public void ActualitzaEstat(string text)
        {
            if (label1.InvokeRequired)
            {
                label1.Invoke(new Action<string>(ActualitzaEstat), text);
            }
            else
            {
                textBox1.Text = text;
            }
        }

        public void EspecificaTitol(string text)
        {
            label1.Text = text;
        }
        public void AfegeixMissatge(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action<string>(AfegeixMissatge), text);
            }
            else
            {
                richTextBox1.Text = richTextBox1.Text
                    + text + "\n";
            }
        }
    }
}
