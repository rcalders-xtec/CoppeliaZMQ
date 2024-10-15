namespace InterficieCoppeliaCodesys
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            graella1 = new Llibreria.Graella();
            label1 = new Label();
            label2 = new Label();
            graella2 = new Llibreria.Graella();
            zmq1 = new Llibreria.ZMQ();
            opc1 = new Llibreria.OPC();
            checkBox1 = new CheckBox();
            linkLabel1 = new LinkLabel();
            SuspendLayout();
            // 
            // graella1
            // 
            graella1.Location = new Point(0, 81);
            graella1.Name = "graella1";
            graella1.NombreBytes = 50;
            graella1.Size = new Size(467, 217);
            graella1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.Blue;
            label1.Location = new Point(0, 53);
            label1.Name = "label1";
            label1.Size = new Size(261, 25);
            label1.TabIndex = 2;
            label1.Text = "Entrades (Codesys <- Coppelia)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Blue;
            label2.Location = new Point(473, 53);
            label2.Name = "label2";
            label2.Size = new Size(258, 25);
            label2.TabIndex = 3;
            label2.Text = "Sortides (Codesys -> Coppelia)";
            // 
            // graella2
            // 
            graella2.Location = new Point(473, 81);
            graella2.Name = "graella2";
            graella2.NombreBytes = 50;
            graella2.Size = new Size(467, 217);
            graella2.TabIndex = 4;
            // 
            // zmq1
            // 
            zmq1.Location = new Point(473, 304);
            zmq1.Name = "zmq1";
            zmq1.NumeroPort = 0;
            zmq1.Size = new Size(454, 182);
            zmq1.TabIndex = 5;
            // 
            // opc1
            // 
            opc1.Location = new Point(0, 304);
            opc1.Name = "opc1";
            opc1.Size = new Size(433, 182);
            opc1.TabIndex = 6;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(22, 19);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(153, 29);
            checkBox1.TabIndex = 7;
            checkBox1.Text = "Sempre visible";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(816, 19);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(97, 25);
            linkLabel1.TabIndex = 8;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "A propòsit";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(925, 484);
            Controls.Add(linkLabel1);
            Controls.Add(checkBox1);
            Controls.Add(opc1);
            Controls.Add(zmq1);
            Controls.Add(graella2);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(graella1);
            MaximizeBox = false;
            Name = "Form1";
            Text = "Interfície Coppelia-Codesys v0.3";
            FormClosing += Form1_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Llibreria.Graella graella1;
        private Label label1;
        private Label label2;
        private Llibreria.Graella graella2;
        private Llibreria.ZMQ zmq1;
        private Llibreria.OPC opc1;
        private CheckBox checkBox1;
        private LinkLabel linkLabel1;
    }
}
