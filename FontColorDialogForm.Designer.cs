namespace webiwabo
{
    partial class FontDialogForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FontDialogForm));
            tabControl1 = new TabControl();
            tabPageFont = new TabPage();
            btnCancel = new Button();
            pictureBox1 = new PictureBox();
            btnOK = new Button();
            lblFont = new Label();
            cmbFonts = new ComboBox();
            lblSize = new Label();
            numSize = new NumericUpDown();
            chkBold = new CheckBox();
            chkItalic = new CheckBox();
            chkUnderline = new CheckBox();
            tabPageAdvanced = new TabPage();
            pictureBox2 = new PictureBox();
            pnlColorPreview = new Panel();
            btnSelectColor = new Button();
            tabControl1.SuspendLayout();
            tabPageFont.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSize).BeginInit();
            tabPageAdvanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageFont);
            tabControl1.Controls.Add(tabPageAdvanced);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(4, 3, 4, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(378, 296);
            tabControl1.TabIndex = 0;
            // 
            // tabPageFont
            // 
            tabPageFont.Controls.Add(btnCancel);
            tabPageFont.Controls.Add(pictureBox1);
            tabPageFont.Controls.Add(btnOK);
            tabPageFont.Controls.Add(lblFont);
            tabPageFont.Controls.Add(cmbFonts);
            tabPageFont.Controls.Add(lblSize);
            tabPageFont.Controls.Add(numSize);
            tabPageFont.Controls.Add(chkBold);
            tabPageFont.Controls.Add(chkItalic);
            tabPageFont.Controls.Add(chkUnderline);
            tabPageFont.Location = new Point(4, 24);
            tabPageFont.Margin = new Padding(4, 3, 4, 3);
            tabPageFont.Name = "tabPageFont";
            tabPageFont.Padding = new Padding(4, 3, 4, 3);
            tabPageFont.Size = new Size(370, 268);
            tabPageFont.TabIndex = 0;
            tabPageFont.Text = "Fuente";
            tabPageFont.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(282, 238);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 27);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancelar";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += BtnCancel_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(-4, 170);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(110, 102);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(186, 238);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(88, 27);
            btnOK.TabIndex = 1;
            btnOK.Text = "Aceptar";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += BtnOK_Click;
            // 
            // lblFont
            // 
            lblFont.AutoSize = true;
            lblFont.Location = new Point(7, 12);
            lblFont.Margin = new Padding(4, 0, 4, 0);
            lblFont.Name = "lblFont";
            lblFont.Size = new Size(46, 15);
            lblFont.TabIndex = 0;
            lblFont.Text = "Fuente:";
            // 
            // cmbFonts
            // 
            cmbFonts.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFonts.FormattingEnabled = true;
            cmbFonts.Location = new Point(70, 8);
            cmbFonts.Margin = new Padding(4, 3, 4, 3);
            cmbFonts.Name = "cmbFonts";
            cmbFonts.Size = new Size(233, 23);
            cmbFonts.TabIndex = 1;
            // 
            // lblSize
            // 
            lblSize.AutoSize = true;
            lblSize.Location = new Point(7, 46);
            lblSize.Margin = new Padding(4, 0, 4, 0);
            lblSize.Name = "lblSize";
            lblSize.Size = new Size(52, 15);
            lblSize.TabIndex = 2;
            lblSize.Text = "Tamaño:";
            // 
            // numSize
            // 
            numSize.Location = new Point(70, 44);
            numSize.Margin = new Padding(4, 3, 4, 3);
            numSize.Maximum = new decimal(new int[] { 72, 0, 0, 0 });
            numSize.Minimum = new decimal(new int[] { 6, 0, 0, 0 });
            numSize.Name = "numSize";
            numSize.Size = new Size(70, 23);
            numSize.TabIndex = 3;
            numSize.Value = new decimal(new int[] { 12, 0, 0, 0 });
            // 
            // chkBold
            // 
            chkBold.AutoSize = true;
            chkBold.Location = new Point(12, 81);
            chkBold.Margin = new Padding(4, 3, 4, 3);
            chkBold.Name = "chkBold";
            chkBold.Size = new Size(65, 19);
            chkBold.TabIndex = 4;
            chkBold.Text = "Negrita";
            chkBold.UseVisualStyleBackColor = true;
            // 
            // chkItalic
            // 
            chkItalic.AutoSize = true;
            chkItalic.Location = new Point(117, 81);
            chkItalic.Margin = new Padding(4, 3, 4, 3);
            chkItalic.Name = "chkItalic";
            chkItalic.Size = new Size(65, 19);
            chkItalic.TabIndex = 5;
            chkItalic.Text = "Cursiva";
            chkItalic.UseVisualStyleBackColor = true;
            // 
            // chkUnderline
            // 
            chkUnderline.AutoSize = true;
            chkUnderline.Location = new Point(222, 81);
            chkUnderline.Margin = new Padding(4, 3, 4, 3);
            chkUnderline.Name = "chkUnderline";
            chkUnderline.Size = new Size(82, 19);
            chkUnderline.TabIndex = 6;
            chkUnderline.Text = "Subrayado";
            chkUnderline.UseVisualStyleBackColor = true;
            // 
            // tabPageAdvanced
            // 
            tabPageAdvanced.Controls.Add(pictureBox2);
            tabPageAdvanced.Controls.Add(pnlColorPreview);
            tabPageAdvanced.Controls.Add(btnSelectColor);
            tabPageAdvanced.Location = new Point(4, 24);
            tabPageAdvanced.Margin = new Padding(4, 3, 4, 3);
            tabPageAdvanced.Name = "tabPageAdvanced";
            tabPageAdvanced.Padding = new Padding(4, 3, 4, 3);
            tabPageAdvanced.Size = new Size(370, 268);
            tabPageAdvanced.TabIndex = 1;
            tabPageAdvanced.Text = "Avanzado";
            tabPageAdvanced.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(-4, 170);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(110, 102);
            pictureBox2.TabIndex = 4;
            pictureBox2.TabStop = false;
            // 
            // pnlColorPreview
            // 
            pnlColorPreview.BorderStyle = BorderStyle.FixedSingle;
            pnlColorPreview.Location = new Point(117, 23);
            pnlColorPreview.Margin = new Padding(4, 3, 4, 3);
            pnlColorPreview.Name = "pnlColorPreview";
            pnlColorPreview.Size = new Size(58, 57);
            pnlColorPreview.TabIndex = 1;
            // 
            // btnSelectColor
            // 
            btnSelectColor.Location = new Point(23, 23);
            btnSelectColor.Margin = new Padding(4, 3, 4, 3);
            btnSelectColor.Name = "btnSelectColor";
            btnSelectColor.Size = new Size(70, 58);
            btnSelectColor.TabIndex = 0;
            btnSelectColor.Text = "Seleccionar Color";
            btnSelectColor.UseVisualStyleBackColor = true;
            btnSelectColor.Click += BtnSelectColor_Click;
            // 
            // FontDialogForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(378, 294);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FontDialogForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Configurar Fuente y Color";
            tabControl1.ResumeLayout(false);
            tabPageFont.ResumeLayout(false);
            tabPageFont.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSize).EndInit();
            tabPageAdvanced.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageFont;
        private System.Windows.Forms.Label lblFont;
        private System.Windows.Forms.ComboBox cmbFonts;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.NumericUpDown numSize;
        private System.Windows.Forms.CheckBox chkBold;
        private System.Windows.Forms.CheckBox chkItalic;
        private System.Windows.Forms.CheckBox chkUnderline;
        private System.Windows.Forms.TabPage tabPageAdvanced;
        private System.Windows.Forms.Panel pnlColorPreview;
        private System.Windows.Forms.Button btnSelectColor;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
    }
}