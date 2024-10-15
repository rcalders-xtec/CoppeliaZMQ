namespace Llibreria
{
    public partial class Filera : UserControl
    {
        public event EventHandler HaCanviat;
        public event EventHandler HaCanviatOverride;

        public bool ControlManual
        {
            get
            {
                return checkBox9.Checked;
            }
        }
        private void ActualitzaCheck()
        {
            checkBox1.Checked = ((_valor & 128) == 128);
            checkBox2.Checked = ((_valor & 64) == 64);
            checkBox3.Checked = ((_valor & 32) == 32);
            checkBox4.Checked = ((_valor & 16) == 16);
            checkBox5.Checked = ((_valor & 8) == 8);
            checkBox6.Checked = ((_valor & 4) == 4);
            checkBox7.Checked = ((_valor & 2) == 2);
            checkBox8.Checked = ((_valor & 1) == 1);
        }
        public int _pos = 0;
        public int _valor = 0;
        public int Valor
        {
            get
            {
                return _valor;
            }
            set
            {
                _valor = value;
                generaCheckEvent(false);
                ActualitzaCheck();
                textBox2.Text = _valor.ToString("D3");
                generaCheckEvent(true);
            }
        }
        public int Position
        {
            set
            {
                _pos = value;
                label1.Text = _pos.ToString("D2");
            }
            get
            {
                return _pos;
            }
        }
        public Filera()
        {
            InitializeComponent();
        }

        private void load(object sender, EventArgs e)
        {
            // En el load no canviaria res!!
            // MODIFICACIÓ 25/06
            // TRET
            //    canviat(null,null);
            //    checkBox9_CheckedChanged(null, null);
            // AFEGIT
            Valor = 0;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            checkBox4.Enabled = false;
            checkBox5.Enabled = false;
            checkBox6.Enabled = false;
            checkBox7.Enabled = false;
            checkBox8.Enabled = false;
            textBox2.Enabled = false;
            //
        }

        private void generaCheckOverrideEvent(bool genera)
        {
            if (!genera)
            {
                checkBox9.CheckedChanged -= checkBox9_CheckedChanged;
            }
            else
            {
                checkBox9.CheckedChanged += checkBox9_CheckedChanged;
            }
        }

        private void generaCheckEvent(bool genera)
        {
            if (!genera)
            {
                checkBox1.CheckedChanged -= canviat;
                checkBox2.CheckedChanged -= canviat;
                checkBox3.CheckedChanged -= canviat;
                checkBox4.CheckedChanged -= canviat;
                checkBox5.CheckedChanged -= canviat;
                checkBox6.CheckedChanged -= canviat;
                checkBox7.CheckedChanged -= canviat;
                checkBox8.CheckedChanged -= canviat;
                textBox2.TextChanged -= textBox2_TextChanged;
            }
            else
            {
                checkBox1.CheckedChanged += canviat;
                checkBox2.CheckedChanged += canviat;
                checkBox3.CheckedChanged += canviat;
                checkBox4.CheckedChanged += canviat;
                checkBox5.CheckedChanged += canviat;
                checkBox6.CheckedChanged += canviat;
                checkBox7.CheckedChanged += canviat;
                checkBox8.CheckedChanged += canviat;
                textBox2.TextChanged += textBox2_TextChanged;
            }
        }
        private void canviat(object sender, EventArgs e)
        {
            int res = 0;
            res += 1 * (checkBox8.Checked ? 1 : 0);
            res += 2 * (checkBox7.Checked ? 1 : 0);
            res += 4 * (checkBox6.Checked ? 1 : 0);
            res += 8 * (checkBox5.Checked ? 1 : 0);
            res += 16 * (checkBox4.Checked ? 1 : 0);
            res += 32 * (checkBox3.Checked ? 1 : 0);
            res += 64 * (checkBox2.Checked ? 1 : 0);
            res += 128 * (checkBox1.Checked ? 1 : 0);
            generaCheckEvent(false);
            _valor = res;
            textBox2.Text = res.ToString("D3");
            generaCheckEvent(true);
            HaCanviat?.Invoke(this, e);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length != 3)
            {
                textBox2.ForeColor = Color.Blue;
            }
            else
            {
                textBox2.ForeColor = Color.Black;
                int.TryParse(textBox2.Text, out int v);
                _valor = v & 255;
                generaCheckEvent(false);
                ActualitzaCheck();
                textBox2.Text = _valor.ToString("D3");
                generaCheckEvent(true);
                HaCanviat?.Invoke(this, e);
            }

        }
        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            // Passem-ho a només lectura
            checkBox1.Enabled = checkBox9.Checked;
            checkBox2.Enabled = checkBox9.Checked;
            checkBox3.Enabled = checkBox9.Checked;
            checkBox4.Enabled = checkBox9.Checked;
            checkBox5.Enabled = checkBox9.Checked;
            checkBox6.Enabled = checkBox9.Checked;
            checkBox7.Enabled = checkBox9.Checked;
            checkBox8.Enabled = checkBox9.Checked;
            textBox2.Enabled = checkBox9.Checked;
            // Notifiquem el canvi a Graella
            HaCanviatOverride?.Invoke(this, e);
            
        }
    }
}
