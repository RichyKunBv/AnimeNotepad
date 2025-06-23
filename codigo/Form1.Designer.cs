namespace webiwabo
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            mnMain = new MenuStrip();
            mnMainFile = new ToolStripMenuItem();
            mnFileNew = new ToolStripMenuItem();
            mnFileOpen = new ToolStripMenuItem();
            mnFileSave = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            mnFilePrint = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            mnFileExit = new ToolStripMenuItem();
            mnMainEdit = new ToolStripMenuItem();
            mnEditFont = new ToolStripMenuItem();
            mnEditCopy = new ToolStripMenuItem();
            mnEditPaste = new ToolStripMenuItem();
            mnEditCut = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            mnEditSelectAll = new ToolStripMenuItem();
            mnMainView = new ToolStripMenuItem();
            mnViewAumentar = new ToolStripMenuItem();
            mnViewDisminuir = new ToolStripMenuItem();
            mnMainHelp = new ToolStripMenuItem();
            mnHelpManual = new ToolStripMenuItem();
            mnHelpAcercaDe = new ToolStripMenuItem();
            toolMain = new ToolStrip();
            tmFileNew = new ToolStripButton();
            tmFileOpen = new ToolStripButton();
            tmFileSave = new ToolStripButton();
            tmFilePrint = new ToolStripButton();
            toolStripSeparator5 = new ToolStripSeparator();
            tmEditCopy = new ToolStripButton();
            tmFilePaste = new ToolStripButton();
            tmFileCut = new ToolStripButton();
            tmFileFont = new ToolStripButton();
            toolStripSeparator6 = new ToolStripSeparator();
            tmViewAumentar = new ToolStripButton();
            tmViewDisminuir = new ToolStripButton();
            RichTextBox = new RichTextBox();
            saveFile = new SaveFileDialog();
            toolTip1 = new ToolTip(components);
            pictureBox1 = new PictureBox();
            StatusMain = new StatusStrip();
            StatusName = new ToolStripStatusLabel();
            StatusWeigth = new ToolStripStatusLabel();
            StatusCreation = new ToolStripStatusLabel();
            StatusModification = new ToolStripStatusLabel();
            mnMain.SuspendLayout();
            toolMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            StatusMain.SuspendLayout();
            SuspendLayout();
            // 
            // mnMain
            // 
            mnMain.ImageScalingSize = new Size(32, 32);
            mnMain.Items.AddRange(new ToolStripItem[] { mnMainFile, mnMainEdit, mnMainView, mnMainHelp });
            mnMain.Location = new Point(0, 0);
            mnMain.Name = "mnMain";
            mnMain.Padding = new Padding(4, 2, 0, 2);
            mnMain.Size = new Size(664, 24);
            mnMain.TabIndex = 1;
            mnMain.Text = "menuStrip1";
            // 
            // mnMainFile
            // 
            mnMainFile.DropDownItems.AddRange(new ToolStripItem[] { mnFileNew, mnFileOpen, mnFileSave, toolStripSeparator1, mnFilePrint, toolStripSeparator4, mnFileExit });
            mnMainFile.Name = "mnMainFile";
            mnMainFile.Size = new Size(60, 20);
            mnMainFile.Text = "Archivo";
            // 
            // mnFileNew
            // 
            mnFileNew.Image = (Image)resources.GetObject("mnFileNew.Image");
            mnFileNew.Name = "mnFileNew";
            mnFileNew.ShortcutKeys = Keys.Control | Keys.N;
            mnFileNew.Size = new Size(201, 38);
            mnFileNew.Text = "Niu";
            mnFileNew.Click += mnFileNew_Click;
            // 
            // mnFileOpen
            // 
            mnFileOpen.Image = (Image)resources.GetObject("mnFileOpen.Image");
            mnFileOpen.Name = "mnFileOpen";
            mnFileOpen.ShortcutKeys = Keys.Control | Keys.O;
            mnFileOpen.Size = new Size(201, 38);
            mnFileOpen.Text = "Abrido";
            mnFileOpen.Click += mnFileOpen_Click;
            // 
            // mnFileSave
            // 
            mnFileSave.Image = (Image)resources.GetObject("mnFileSave.Image");
            mnFileSave.Name = "mnFileSave";
            mnFileSave.ShortcutKeys = Keys.Control | Keys.S;
            mnFileSave.Size = new Size(201, 38);
            mnFileSave.Text = "Guardao";
            mnFileSave.Click += mnFileSave_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(198, 6);
            // 
            // mnFilePrint
            // 
            mnFilePrint.Image = (Image)resources.GetObject("mnFilePrint.Image");
            mnFilePrint.Name = "mnFilePrint";
            mnFilePrint.ShortcutKeys = Keys.Control | Keys.P;
            mnFilePrint.Size = new Size(201, 38);
            mnFilePrint.Text = "Inprimir";
            mnFilePrint.Click += mnFilePrint_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(198, 6);
            // 
            // mnFileExit
            // 
            mnFileExit.Image = (Image)resources.GetObject("mnFileExit.Image");
            mnFileExit.Name = "mnFileExit";
            mnFileExit.ShortcutKeys = Keys.Control | Keys.Q;
            mnFileExit.Size = new Size(201, 38);
            mnFileExit.Text = "Salir pa fuera";
            mnFileExit.Click += mnFileExit_Click;
            // 
            // mnMainEdit
            // 
            mnMainEdit.DropDownItems.AddRange(new ToolStripItem[] { mnEditFont, mnEditCopy, mnEditPaste, mnEditCut, toolStripSeparator3, mnEditSelectAll });
            mnMainEdit.Name = "mnMainEdit";
            mnMainEdit.Size = new Size(49, 20);
            mnMainEdit.Text = "Editar";
            // 
            // mnEditFont
            // 
            mnEditFont.Image = (Image)resources.GetObject("mnEditFont.Image");
            mnEditFont.Name = "mnEditFont";
            mnEditFont.Size = new Size(307, 38);
            mnEditFont.Text = "Fuentes y Colores";
            mnEditFont.Click += mnEditFont_Click;
            // 
            // mnEditCopy
            // 
            mnEditCopy.Image = (Image)resources.GetObject("mnEditCopy.Image");
            mnEditCopy.Name = "mnEditCopy";
            mnEditCopy.ShortcutKeys = Keys.Control | Keys.C;
            mnEditCopy.Size = new Size(307, 38);
            mnEditCopy.Text = "Copion";
            mnEditCopy.Click += mnEditCopy_Click;
            // 
            // mnEditPaste
            // 
            mnEditPaste.Image = (Image)resources.GetObject("mnEditPaste.Image");
            mnEditPaste.Name = "mnEditPaste";
            mnEditPaste.ShortcutKeys = Keys.Control | Keys.V;
            mnEditPaste.Size = new Size(307, 38);
            mnEditPaste.Text = "Pegao";
            mnEditPaste.Click += mnEditPaste_Click;
            // 
            // mnEditCut
            // 
            mnEditCut.Image = (Image)resources.GetObject("mnEditCut.Image");
            mnEditCut.Name = "mnEditCut";
            mnEditCut.ShortcutKeys = Keys.Control | Keys.X;
            mnEditCut.Size = new Size(307, 38);
            mnEditCut.Text = "como la de akame ga kill (cortao)";
            mnEditCut.Click += mnEditCut_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(304, 6);
            // 
            // mnEditSelectAll
            // 
            mnEditSelectAll.Image = (Image)resources.GetObject("mnEditSelectAll.Image");
            mnEditSelectAll.Name = "mnEditSelectAll";
            mnEditSelectAll.ShortcutKeys = Keys.Control | Keys.A;
            mnEditSelectAll.Size = new Size(307, 38);
            mnEditSelectAll.Text = "Toma todo";
            mnEditSelectAll.Click += mnEditSelectAll_Click;
            // 
            // mnMainView
            // 
            mnMainView.DropDownItems.AddRange(new ToolStripItem[] { mnViewAumentar, mnViewDisminuir });
            mnMainView.Name = "mnMainView";
            mnMainView.Size = new Size(35, 20);
            mnMainView.Text = "Ver";
            // 
            // mnViewAumentar
            // 
            mnViewAumentar.Image = (Image)resources.GetObject("mnViewAumentar.Image");
            mnViewAumentar.Name = "mnViewAumentar";
            mnViewAumentar.ShortcutKeys = Keys.Control | Keys.Oemplus;
            mnViewAumentar.Size = new Size(202, 22);
            mnViewAumentar.Text = "C acerca";
            mnViewAumentar.Click += mnViewAumentar_Click;
            // 
            // mnViewDisminuir
            // 
            mnViewDisminuir.Image = (Image)resources.GetObject("mnViewDisminuir.Image");
            mnViewDisminuir.Name = "mnViewDisminuir";
            mnViewDisminuir.ShortcutKeys = Keys.Control | Keys.OemMinus;
            mnViewDisminuir.Size = new Size(202, 22);
            mnViewDisminuir.Text = "C aleja";
            mnViewDisminuir.Click += mnViewDisminuir_Click;
            // 
            // mnMainHelp
            // 
            mnMainHelp.DropDownItems.AddRange(new ToolStripItem[] { mnHelpManual, mnHelpAcercaDe });
            mnMainHelp.Name = "mnMainHelp";
            mnMainHelp.Size = new Size(53, 20);
            mnMainHelp.Text = "Ayuda";
            // 
            // mnHelpManual
            // 
            mnHelpManual.Image = (Image)resources.GetObject("mnHelpManual.Image");
            mnHelpManual.Name = "mnHelpManual";
            mnHelpManual.ShortcutKeys = Keys.Control | Keys.F1;
            mnHelpManual.Size = new Size(160, 22);
            mnHelpManual.Text = "Manuel";
            mnHelpManual.Click += mnHelpManual_Click;
            // 
            // mnHelpAcercaDe
            // 
            mnHelpAcercaDe.Image = (Image)resources.GetObject("mnHelpAcercaDe.Image");
            mnHelpAcercaDe.Name = "mnHelpAcercaDe";
            mnHelpAcercaDe.Size = new Size(160, 22);
            mnHelpAcercaDe.Text = "Acerca de";
            mnHelpAcercaDe.Click += mnHelpAcercaDe_Click;
            // 
            // toolMain
            // 
            toolMain.BackColor = SystemColors.ActiveBorder;
            toolMain.ImageScalingSize = new Size(32, 32);
            toolMain.Items.AddRange(new ToolStripItem[] { tmFileNew, tmFileOpen, tmFileSave, tmFilePrint, toolStripSeparator5, tmEditCopy, tmFilePaste, tmFileCut, tmFileFont, toolStripSeparator6, tmViewAumentar, tmViewDisminuir });
            toolMain.Location = new Point(0, 24);
            toolMain.Name = "toolMain";
            toolMain.Padding = new Padding(0, 0, 3, 0);
            toolMain.Size = new Size(664, 39);
            toolMain.TabIndex = 2;
            toolMain.Text = "toolStrip1";
            // 
            // tmFileNew
            // 
            tmFileNew.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tmFileNew.Image = (Image)resources.GetObject("tmFileNew.Image");
            tmFileNew.ImageTransparentColor = Color.Magenta;
            tmFileNew.Name = "tmFileNew";
            tmFileNew.Size = new Size(36, 36);
            tmFileNew.Text = "Nuevo";
            tmFileNew.ToolTipText = "Crear un nuevo documento";
            tmFileNew.Click += tmFileNew_Click;
            // 
            // tmFileOpen
            // 
            tmFileOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tmFileOpen.Image = (Image)resources.GetObject("tmFileOpen.Image");
            tmFileOpen.ImageTransparentColor = Color.Magenta;
            tmFileOpen.Name = "tmFileOpen";
            tmFileOpen.Size = new Size(36, 36);
            tmFileOpen.Text = "Abrir";
            tmFileOpen.Click += tmFileOpen_Click;
            // 
            // tmFileSave
            // 
            tmFileSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tmFileSave.Image = (Image)resources.GetObject("tmFileSave.Image");
            tmFileSave.ImageTransparentColor = Color.Magenta;
            tmFileSave.Name = "tmFileSave";
            tmFileSave.Size = new Size(36, 36);
            tmFileSave.Text = "Guardar";
            tmFileSave.Click += tmFileSave_Click;
            // 
            // tmFilePrint
            // 
            tmFilePrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tmFilePrint.Image = (Image)resources.GetObject("tmFilePrint.Image");
            tmFilePrint.ImageTransparentColor = Color.Magenta;
            tmFilePrint.Name = "tmFilePrint";
            tmFilePrint.Size = new Size(36, 36);
            tmFilePrint.Text = "Imprimir";
            tmFilePrint.Click += tmFilePrint_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 39);
            // 
            // tmEditCopy
            // 
            tmEditCopy.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tmEditCopy.Image = (Image)resources.GetObject("tmEditCopy.Image");
            tmEditCopy.ImageTransparentColor = Color.Magenta;
            tmEditCopy.Name = "tmEditCopy";
            tmEditCopy.Size = new Size(36, 36);
            tmEditCopy.Text = "Copiar";
            tmEditCopy.Click += tmEditCopy_Click;
            // 
            // tmFilePaste
            // 
            tmFilePaste.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tmFilePaste.Image = (Image)resources.GetObject("tmFilePaste.Image");
            tmFilePaste.ImageTransparentColor = Color.Magenta;
            tmFilePaste.Name = "tmFilePaste";
            tmFilePaste.Size = new Size(36, 36);
            tmFilePaste.Text = "Pegar";
            tmFilePaste.Click += tmFilePaste_Click;
            // 
            // tmFileCut
            // 
            tmFileCut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tmFileCut.Image = (Image)resources.GetObject("tmFileCut.Image");
            tmFileCut.ImageTransparentColor = Color.Magenta;
            tmFileCut.Name = "tmFileCut";
            tmFileCut.Size = new Size(36, 36);
            tmFileCut.Text = "Cortar";
            tmFileCut.Click += tmFileCut_Click;
            // 
            // tmFileFont
            // 
            tmFileFont.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tmFileFont.Image = (Image)resources.GetObject("tmFileFont.Image");
            tmFileFont.ImageTransparentColor = Color.Magenta;
            tmFileFont.Name = "tmFileFont";
            tmFileFont.Size = new Size(36, 36);
            tmFileFont.Text = "Fuente y Color";
            tmFileFont.Click += tmFileFont_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(6, 39);
            // 
            // tmViewAumentar
            // 
            tmViewAumentar.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tmViewAumentar.Image = (Image)resources.GetObject("tmViewAumentar.Image");
            tmViewAumentar.ImageTransparentColor = Color.Magenta;
            tmViewAumentar.Name = "tmViewAumentar";
            tmViewAumentar.Size = new Size(36, 36);
            tmViewAumentar.Text = "Aumentar";
            tmViewAumentar.Click += tmViewAumentar_Click;
            // 
            // tmViewDisminuir
            // 
            tmViewDisminuir.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tmViewDisminuir.Image = (Image)resources.GetObject("tmViewDisminuir.Image");
            tmViewDisminuir.ImageTransparentColor = Color.Magenta;
            tmViewDisminuir.Name = "tmViewDisminuir";
            tmViewDisminuir.Size = new Size(36, 36);
            tmViewDisminuir.Text = "Disminuir";
            tmViewDisminuir.Click += tmViewDisminuir_Click;
            // 
            // RichTextBox
            // 
            RichTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            RichTextBox.BackColor = SystemColors.HighlightText;
            RichTextBox.ForeColor = SystemColors.WindowText;
            RichTextBox.Location = new Point(94, 63);
            RichTextBox.Margin = new Padding(4);
            RichTextBox.Name = "RichTextBox";
            RichTextBox.Size = new Size(570, 265);
            RichTextBox.TabIndex = 4;
            RichTextBox.Text = "";
            // 
            // saveFile
            // 
            saveFile.DefaultExt = "*.rtf";
            saveFile.Title = "Guardar documento";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 63);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(105, 143);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // StatusMain
            // 
            StatusMain.Items.AddRange(new ToolStripItem[] { StatusName, StatusWeigth, StatusCreation, StatusModification });
            StatusMain.Location = new Point(0, 328);
            StatusMain.Name = "StatusMain";
            StatusMain.Size = new Size(664, 22);
            StatusMain.TabIndex = 6;
            StatusMain.Text = "statusStrip1";
            // 
            // StatusName
            // 
            StatusName.Name = "StatusName";
            StatusName.Size = new Size(57, 17);
            StatusName.Text = "Nombre: ";
            StatusName.Click += StatusName_Click;
            // 
            // StatusWeigth
            // 
            StatusWeigth.Name = "StatusWeigth";
            StatusWeigth.Size = new Size(38, 17);
            StatusWeigth.Text = "Peso: ";
            StatusWeigth.Click += StatusWeigth_Click;
            // 
            // StatusCreation
            // 
            StatusCreation.Name = "StatusCreation";
            StatusCreation.Size = new Size(60, 17);
            StatusCreation.Text = "Creacion: ";
            StatusCreation.Click += StatusCreation_Click;
            // 
            // StatusModification
            // 
            StatusModification.Name = "StatusModification";
            StatusModification.Size = new Size(83, 17);
            StatusModification.Text = "Modificacion: ";
            StatusModification.Click += StatusModification_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(664, 350);
            Controls.Add(StatusMain);
            Controls.Add(RichTextBox);
            Controls.Add(pictureBox1);
            Controls.Add(toolMain);
            Controls.Add(mnMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = mnMain;
            Margin = new Padding(4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AnimeNotes";
            FormClosing += Form1_FormClosing;
            mnMain.ResumeLayout(false);
            mnMain.PerformLayout();
            toolMain.ResumeLayout(false);
            toolMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            StatusMain.ResumeLayout(false);
            StatusMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip mnMain;
        private System.Windows.Forms.ToolStripMenuItem mnMainFile;
        private System.Windows.Forms.ToolStripMenuItem mnMainEdit;
        private System.Windows.Forms.ToolStripMenuItem mnMainView;
        private System.Windows.Forms.ToolStripMenuItem mnMainHelp;
        private System.Windows.Forms.ToolStripMenuItem mnFileExit;
        private System.Windows.Forms.ToolStripMenuItem mnFileOpen;
        private System.Windows.Forms.ToolStripMenuItem mnFileSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnEditCopy;
        private System.Windows.Forms.ToolStripMenuItem mnEditPaste;
        private System.Windows.Forms.ToolStripMenuItem mnEditCut;
        private System.Windows.Forms.ToolStripMenuItem mnEditSelectAll;
        private System.Windows.Forms.ToolStripMenuItem mnHelpManual;
        private System.Windows.Forms.ToolStripMenuItem mnHelpAcercaDe;
        private System.Windows.Forms.ToolStripMenuItem mnViewAumentar;
        private System.Windows.Forms.ToolStripMenuItem mnViewDisminuir;
        private System.Windows.Forms.ToolStrip toolMain;
        private System.Windows.Forms.ToolStripMenuItem mnFileNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.RichTextBox RichTextBox;
        private System.Windows.Forms.ToolStripButton tmFileNew;
        private System.Windows.Forms.ToolStripButton tmFileOpen;
        private System.Windows.Forms.ToolStripButton tmFileSave;
        private System.Windows.Forms.ToolStripButton tmEditCopy;
        private System.Windows.Forms.ToolStripButton tmFilePaste;
        private System.Windows.Forms.ToolStripButton tmFileCut;
        private System.Windows.Forms.ToolStripButton tmViewAumentar;
        private System.Windows.Forms.ToolStripButton tmViewDisminuir;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.ToolTip toolTip1;
        private ToolStripMenuItem mnFilePrint;
        private ToolStripButton tmFilePrint;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private PictureBox pictureBox1;
        private ToolStripMenuItem mnEditFont;
        private ToolStripButton tmFileFont;
        private StatusStrip StatusMain;
        private ToolStripStatusLabel StatusName;
        private ToolStripStatusLabel StatusWeigth;
        private ToolStripStatusLabel StatusCreation;
        private ToolStripStatusLabel StatusModification;
    }
}

