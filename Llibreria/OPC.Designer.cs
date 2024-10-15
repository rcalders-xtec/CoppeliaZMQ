namespace Llibreria
{
    partial class OPC
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
            baseCom1.Size = new Size(411, 228);
            baseCom1.TabIndex = 0;
            // 
            // OPC
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(baseCom1);
            Name = "OPC";
            Size = new Size(412, 226);
            Load += OPC_Load;
            ResumeLayout(false);
        }

        #endregion

        private BaseCom baseCom1;
    }
}
