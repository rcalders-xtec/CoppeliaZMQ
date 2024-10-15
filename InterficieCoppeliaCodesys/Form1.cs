using Opc.Ua.Configuration;

namespace InterficieCoppeliaCodesys
{
    public partial class Form1 : Form
    {
        private byte[] outputs;
        private byte[] inputs;

        private void CodeSysIN(object o, EventArgs e)
        {
            byte[] bytes = opc1.Entrada;
            graella2.SetElementsExterns(bytes);
            // Hauríem de recórrer la graella1
            // I en aquells casos
        }

        private void CoppeliaIN(object o, EventArgs e)
        {
            byte[] bytes = zmq1.Entrada;
            graella1.SetElementsExterns(bytes);
            // Hauríem de recórrer la graella1
            // I en aquells casos 
        }
        private void coppeliaconn(object o, EventArgs e)
        {
            byte[] elements = graella2.Elements;
            zmq1.AfegirCua(elements);

        }
        private void hacanviat(object o, EventArgs e)
        {
            Llibreria.Graella g = (Llibreria.Graella)o;
            zmq1.AfegirCua(g.Elements);
        }

        private void hacanviatopc(object o, EventArgs e)
        {
            Llibreria.Graella g = (Llibreria.Graella)o;
         // 
          //  opc1.EsborraCua();
            opc1.AfegirCua(g.Elements);
        }


        public Form1()
        {
            InitializeComponent();
            // Afegim esdeveniments
            graella2.HaCanviat += hacanviat;
            graella1.HaCanviat += hacanviatopc;
            zmq1.HaCanviat += CoppeliaIN;
            opc1.HaCanviat += CodeSysIN;
            zmq1.HiHaConnexio += coppeliaconn;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            zmq1.TancaThread();
            opc1.TancaThread();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = checkBox1.Checked;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 f2 = new Form2();
            bool previ = TopMost;
            f2.TopMost = true;
            (f2).ShowDialog();
            TopMost = previ;
        }
    }
}
