using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Llibreria
{
    public partial class Graella : UserControl
    {
        // Declare the event
        public event EventHandler HaCanviat;

        private byte[] ElementsExterns;

        public void SetElementsExterns(byte[] elementsExterns)
        {
            for(int i=0; i<NombreBytes; i++)
            {
                byte valor = 0;
                if (i<elementsExterns.Length)
                {
                    valor= elementsExterns[i];  
                }
                ElementsExterns[i] = valor;  
            }
            // Refresquem els valors
            for (int i = 0; i < NombreBytes; i++)
            {
                ((Filera)controls[i]).Valor =
                    ElementsGraellaOverride[i] ?
                    ElementsGraella[i] :
                    ElementsExterns[i] ;
            }
            HaCanviat?.Invoke(this, null);
        }

        private byte[] ElementsGraella = null;
        private bool[] ElementsGraellaOverride = null; // Elements Graella modificats...

        public byte[] Elements
        {
            get
            {
                // Hauríem de recorrer 
                // la graella si està 
                // en automàtic retornar 
                byte[] compendi = new byte[NombreBytes];
                for (int i =0; i < NombreBytes; i++)
                {
                    compendi[i] = ElementsGraellaOverride[i] ? 
                            ElementsGraella[i] : ElementsExterns[i];
                }
                return compendi;
            }
        }

        private void canvi(object sender, EventArgs e)
        {
            Filera f = (Filera)sender;
            ElementsGraella[f.Position] = (byte)f.Valor;
            HaCanviat?.Invoke(this, e);
        }
        private void canvioverride(object sender, EventArgs e)
        {
            Filera f = (Filera)sender;
            ElementsGraellaOverride[f.Position] = f.ControlManual;
            if (!f.ControlManual)
            {
                // Hauríem de carregar extern
                f.Valor = ElementsExterns[f.Position]; ;
            }
            else
            {
                f.Valor = ElementsGraella[f.Position];
            }
            HaCanviat?.Invoke(this, e);
        }

        private List<Control> controls;
        public Graella()
        {
            //            Elements = new byte[500];

            InitializeComponent();
            controls = new List<Control>();

        }
        private int _nombrebytes = 0;
        public int NombreBytes
        {
            get
            {
                return _nombrebytes;
            }
            set
            {
                // Esborrem primer els controls que hi pugui haver
                foreach (var x in controls)
                {
                    panel1.Controls.Remove(x);
                }
                ElementsGraella = new byte[value];
                ElementsExterns = new byte[value];

                ElementsGraellaOverride = new bool[value];

                // Afegim els controls
                _nombrebytes = value;
                for (int i = 0; i < _nombrebytes; i++)
                {
                    Filera ww = new Filera();
                    ww.Position = i;
                    ww.Location = new System.Drawing.Point(0, 0 + 45 * i); ;
                    //  ww.Size = new Size(800, 95);
                    ww.HaCanviat += canvi;
                    ww.HaCanviatOverride += canvioverride;

                    panel1.Controls.Add(ww);
                    controls.Add(ww);

                }
            }
        }
    }
}