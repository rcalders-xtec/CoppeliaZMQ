using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Opc.Ua.Client;
using Opc.Ua.Configuration;
using Opc.Ua;
using System.Security.Permissions;
using Newtonsoft.Json.Linq;

namespace Llibreria
{

    public partial class OPC : UserControl
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private Thread _workerThread;
        public event EventHandler HaCanviat;


        // Cua amb missatges sortints
        // Les I del PLC
        private ConcurrentQueue<byte[]> cua;
        // Cua amb missatges entrants
        // Les Q del PLC

        private ConcurrentQueue<byte[]> cuaentrades;
        private byte[] darrerrebut;

        public byte[] Entrada
        {
            get
            {
                return darrerrebut;
            }
        }

        public bool IdenticMissatge(byte[] miss1, byte[] miss2)
        {
            if (miss1 == null && miss2 == null)
                return true;
            if (miss1 == null && miss2 != null
                || miss1 != null && miss2 == null)
                return false;
            if (miss1.Length != miss2.Length)
                 return false;
            for (int i=0; i<miss1.Length;i++)
            {
                if (miss1[i] != miss2[i])
                    return false;
            }
            return true;
        }
        public OPC()
        {
            InitializeComponent();
            cua = new ConcurrentQueue<byte[]>();
            cuaentrades = new ConcurrentQueue<byte[]>();
            baseCom1.EspecificaTitol("Connexió OPC UA");
        }

        private void GestionaEntrades()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(GestionaEntrades));
            }
            else
            {
                bool canvis = false;
                while (cuaentrades.TryDequeue(out byte[] val))
                {
                    canvis = true;
                    if (val is null) continue;
                    //                    richTextBox1.Text += "Rebut: " + val.Length + "\n";
                    darrerrebut = (byte[])val.Clone();
                    baseCom1.IncrementaRebuts();
                }
                if (canvis) HaCanviat?.Invoke(this, null);
            }
        }

        public void AfegirCua(byte[] bytes)
        {
            cua.Enqueue((byte[])bytes.Clone());
        }


        private void OnMonitoredItemNotification(MonitoredItem item, MonitoredItemNotificationEventArgs e)
        {
            foreach (var value in item.DequeueValues())
            {
                if (value == null)
                {
                    continue;
                }
                cuaentrades.Enqueue((byte[])value.Value);
                baseCom1.IncrementaRebuts();
            }
            GestionaEntrades();
        }

        public void EsborraCua()
        {
            cua.Clear();
        }
        private bool connectat = false;
        async void ConnexioOPC(CancellationToken token)
        {
            string endpointUrl = "opc.tcp://localhost:4840"; // URL del servidor UPC UA
            string nodeIdToRead = "i=2256"; // Status del servidor

            // Nodes de les variables de sortida i entrada
            string nodeIdToSubscribe = "ns=4;s=|var|CODESYS Control Win V3 x64.Application.Sinc.Sortides";
            string nodeIdToWrite = "ns=4;s=|var|CODESYS Control Win V3 x64.Application.Sinc.Entrades";
            // 
            
            ApplicationInstance application = new ApplicationInstance
            {
                ApplicationName = "OPC UA Client",
                ApplicationType = ApplicationType.Client,
                ConfigSectionName = "OpcUaClient"
            };

            ApplicationConfiguration config = new ApplicationConfiguration
            {
                ApplicationName = "OPC UA Client",
                ApplicationUri = "urn:localhost:OPCUAClient",
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier(),
                    TrustedPeerCertificates = new CertificateTrustList(),
                    TrustedIssuerCertificates = new CertificateTrustList(),
                    RejectedCertificateStore = new CertificateStoreIdentifier { StoreType = "Directory", StorePath = "RejectedCertificates" },
                    AutoAcceptUntrustedCertificates = true
                },
                TransportConfigurations = new TransportConfigurationCollection(),
                TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
                ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 },
                DisableHiResClock = false
            };
            // Deshabilitem certificat
            config.SecurityConfiguration.AutoAcceptUntrustedCertificates = true;
            config.CertificateValidator.CertificateValidation += (sender, e) =>
            {
                e.Accept = (e.Error.StatusCode == StatusCodes.BadCertificateUntrusted);
            };
            WriteValue writeValue = new WriteValue
            {
                NodeId = new NodeId(nodeIdToWrite),
                AttributeId = Attributes.Value,
            };
            byte[] darrermiss = null;

            while (true)
            {
                if (token.IsCancellationRequested)
                    return;

                connectat = false;
                try
                {
                    config.Validate(ApplicationType.Client).GetAwaiter().GetResult();

                    var selectedEndpoint = CoreClientUtils.SelectEndpoint(endpointUrl, false, 15000);

                    var endpointConfiguration = EndpointConfiguration.Create(config);
                    var endpoint = new ConfiguredEndpoint(null, selectedEndpoint, endpointConfiguration);

                    var session = await Session.Create(config, endpoint, false, "OPC UA Client", 60000, new UserIdentity(new AnonymousIdentityToken()), null);

                    DataValue value = session.ReadValue(nodeIdToRead);
                    Console.WriteLine($"Value of node {nodeIdToRead}: {value.Value}");

                    ExtensionObject extensionObject = value.Value as ExtensionObject;
                    ServerStatusDataType serverStatus = (ServerStatusDataType)extensionObject.Body;

                    baseCom1.AfegeixMissatge("Servidor: " + serverStatus.BuildInfo.ProductName);
                    baseCom1.ActualitzaEstat("Connectat");
                    // Habilitem subscripció

                    Subscription subscription = new Subscription(session.DefaultSubscription)
                    {
                        PublishingInterval = 100
                    };

                    MonitoredItem monitoredItem = new MonitoredItem(subscription.DefaultItem)
                    {
                        StartNodeId = new NodeId(nodeIdToSubscribe),
                        AttributeId = Attributes.Value,
                        DisplayName = "MyMonitoredItem",
                        SamplingInterval = 100
                    };

                    monitoredItem.Notification += OnMonitoredItemNotification;

                    subscription.AddItem(monitoredItem);
                    session.AddSubscription(subscription);
                    subscription.Create();
                    connectat = true;
                    DateTime data = DateTime.Now;

                    while (connectat)
                    {
                        if (token.IsCancellationRequested)
                            return;
                        System.Threading.Thread.Sleep(50);

                        // Cada 10 segons enviem
                        // darrer missatge per si 
                        // el programa de la PLC s'ha reiniciat

                        if ((DateTime.Now - data).TotalSeconds >= 10)
                        {
                            data = DateTime.Now;
                      
                          
                            if (darrermiss !=null)
                            {
                                writeValue.Value = new DataValue
                                {
                                    Value = darrermiss,
                                    StatusCode = StatusCodes.Good,
                                    ServerTimestamp = DateTime.Now,
                                    SourceTimestamp = DateTime.Now
                                };
                                WriteValueCollection writeValues = new WriteValueCollection { writeValue };
                                ResponseHeader responseHeader = session.Write(null, writeValues, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos);
                                bool enviat = StatusCode.IsGood(results[0]);
                                if (enviat)
                                {
                                    baseCom1.IncrementaEnviats();
                                }
                            }
                        }
                        // Mirem si hi ha elements
                        // Enviem missatges pendents si n'hi ha.
                        while (cua.TryPeek(out byte[] sortida))
                        {
                            if (token.IsCancellationRequested)
                                return;
                            // Si el missatge és igual 
                            // que el darrer que hem enviat 
                            // no l'enviarem
                            if (IdenticMissatge(sortida,darrermiss))
                            {
                                cua.TryDequeue(out _);
                                continue;
                            }
                            
                            darrermiss = (byte[]) sortida.Clone();
                            writeValue.Value = new DataValue
                            {
                                Value = sortida,
                                StatusCode = StatusCodes.Good,
                                ServerTimestamp = DateTime.Now,
                                SourceTimestamp = DateTime.Now
                            };
                            WriteValueCollection writeValues = new WriteValueCollection { writeValue };
                            ResponseHeader responseHeader = session.Write(null, writeValues, out StatusCodeCollection results, out DiagnosticInfoCollection diagnosticInfos);
                            bool enviat = StatusCode.IsGood(results[0]);
                            //     bool enviat = pair.TrySendFrame(sortida);

                            cua.TryDequeue(out _);
                    
                            if (enviat)
                            {
                                baseCom1.IncrementaEnviats();
                                // System.Threading.Thread.Sleep(5);
                            }
                            else
                            {
                                baseCom1.AfegeixMissatge("Error: " + results[0].ToString());
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (token.IsCancellationRequested)
                        return;

                    baseCom1.ActualitzaEstat("No connectat");
                }
                if (token.IsCancellationRequested)
                    return;

                baseCom1.ActualitzaEstat("No connectat");
                
                await Task.Delay(1000);
            }
        }
        private bool IsInDesignMode()
        {

            return DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }
        private void OPC_Load(object sender, EventArgs e)
        {
            if (IsInDesignMode())
            {
                baseCom1.EspecificaTitol("OPC Disseny"); ;
                return;
            }
            // Aquí hauríem d'arrencar el procés
            var token = _cancellationTokenSource.Token;
            _workerThread = new Thread(() => ConnexioOPC(token));
            _workerThread.Start();
        }
        public void TancaThread()
        {
            _cancellationTokenSource.Cancel();
        }


    }
}
