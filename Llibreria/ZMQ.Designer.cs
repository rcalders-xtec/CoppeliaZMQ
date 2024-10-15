namespace Llibreria
{
    partial class ZMQ
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            baseCom1 = new BaseCom();
            SuspendLayout();
            // 
            // baseCom1
            // 
            baseCom1.Location = new Point(3, 3);
            baseCom1.Name = "baseCom1";
            baseCom1.Size = new Size(414, 231);
            baseCom1.TabIndex = 0;
            // 
            // ZMQ
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(baseCom1);
            Name = "ZMQ";
            Size = new Size(420, 237);
            Load += ZMQ_Load;
            ResumeLayout(false);
        }

        #endregion

        private BaseCom baseCom1;
    }
}
