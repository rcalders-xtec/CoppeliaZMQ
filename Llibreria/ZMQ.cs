using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Llibreria
{
    public partial class ZMQ : UserControl
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public event EventHandler HaCanviat;
        public event EventHandler HiHaConnexio;

        private ConcurrentQueue<byte[]> cua;
        private ConcurrentQueue<byte[]> cuaentrades;
        private byte[] darrerrebut;
        public byte[] Entrada
        {
            get
            {
                return darrerrebut;
            }
        }
        public void AfegirCua(byte[] bytes)
        {
            cua.Enqueue((byte[])bytes.Clone());
        }

        public int NumeroPort { get; set; }
        public ZMQ()
        {
            cua = new ConcurrentQueue<byte[]>();
            cuaentrades = new ConcurrentQueue<byte[]>();

            InitializeComponent();
            baseCom1.EspecificaTitol("Connexió ZMQ");
        }

        private void ActualitzaEstat(string text)
        {
            baseCom1.ActualitzaEstat(text);
        }

        private void GestionaEntrades()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(GestionaEntrades));
            }
            else
            {
                while (cuaentrades.TryDequeue(out byte[] val))
                {
                    //                    richTextBox1.Text += "Rebut: " + val.Length + "\n";
                    darrerrebut = (byte[])val.Clone();
                    baseCom1.IncrementaRebuts();
                }
                HaCanviat?.Invoke(this, null);
            }
        }
        private void AssenyalaConnexio()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(AssenyalaConnexio));
            }
            else
            {
                HiHaConnexio?.Invoke(this, null);
            }
        }

        private void AfegeixMissatge(string text)
        {
            baseCom1.AfegeixMissatge(text);
        }

        void ConnexioZMQ(CancellationToken token)
        {
            DateTime data = DateTime.Now;
            while (true)
            {
                if (token.IsCancellationRequested)
                    return;
                System.Threading.Thread.Sleep(1000);
                bool connectat = false;
                bool abansconnectat = false;
                //                byte[] binaryMessage = 
                //                    new byte[] { 0x41, 0x42, 0x43 };

                //                ActualitzaEstat("Connectant...");
                try
                {
                    using (var pair = new PairSocket())
                    {
                        pair.Connect("tcp://localhost:5555");

                        while (true)
                        {
                            if (token.IsCancellationRequested)
                                return;
                            // 
                            if ((DateTime.Now - data).TotalMilliseconds > 3000)
                            {
                                connectat = false;

                            }
                            if (connectat && !abansconnectat)
                            {
                                AssenyalaConnexio();
                                ActualitzaEstat("Connectat");
                                abansconnectat = true;
                            }
                            else if (!connectat)
                            {
                                ActualitzaEstat("No connectat");
                                abansconnectat = false;
                            }

                            // Enviem missatges pendents si n'hi ha.
                            while (cua.TryPeek(out byte[] sortida))
                            {
                                if (token.IsCancellationRequested)
                                    return; 
                                bool enviat = pair.TrySendFrame(sortida);
                                if (enviat)
                                {
                                    cua.TryDequeue(out _);
                                    baseCom1.IncrementaEnviats();
                                    // System.Threading.Thread.Sleep(5);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            // Rebem missatges
                            // Hauriem de llegir missatges
                            while (true)
                            {
                                if (token.IsCancellationRequested)
                                    return;
                                bool rebut = pair.TryReceiveFrameBytes(out byte[] unsolicitedMessage);

                                if (rebut)
                                {
                                    int z = 4;
                                    data = DateTime.Now;
                                    connectat = true;
                                    // Afegim a la cua d'entrades
                                    if (unsolicitedMessage.Length > 0)
                                    {
                                        cuaentrades.Enqueue((byte[])unsolicitedMessage.Clone());
                                        GestionaEntrades();
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    AfegeixMissatge("Err: " + ex.Message);
                }
            }
        }

        private bool IsInDesignMode()
        {

            return DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }
        private void ZMQ_Load(object sender, EventArgs e)
        {
            if (IsInDesignMode())
            {
                baseCom1.EspecificaTitol("ZMQ Disseny"); ;
                return;
            }
            // Aquí hauríem d'arrencar el procés
            var token = _cancellationTokenSource.Token;
            _workerThread = new Thread(() => ConnexioZMQ(token));
            _workerThread.Start();
        }
        private Thread _workerThread;
        public void TancaThread()
        {
            _cancellationTokenSource.Cancel();
        }
    }

}
