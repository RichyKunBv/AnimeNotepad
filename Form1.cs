using System;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Reflection.Emit;
using static webiwabo.Form1.EditManager;

namespace webiwabo
{

    public partial class Form1 : Form
    {
        private FileManager fileManager;
        private EditManager editManager;
        private ViewManager viewManager;
        private HelpManager helpManager;

        public Form1()
        {
            InitializeComponent();
            fileManager = new FileManager(this, RichTextBox);
            editManager = new EditManager(RichTextBox);
            viewManager = new ViewManager(RichTextBox);
            helpManager = new HelpManager();
        }

        private void mnFileExit_Click(object sender, EventArgs e)
        {
            fileManager.ExitProgram();
        }

        private void mnFileNew_Click(object sender, EventArgs e)
        {
            fileManager.NewFile();
        }
        private void tmFileNew_Click(object sender, EventArgs e)
        {
            fileManager.NewFile();
        }

        private void mnFileSave_Click(object sender, EventArgs e)
        {
            fileManager.SaveFile();
        }
        private void tmFileSave_Click(object sender, EventArgs e)
        {
            fileManager.SaveFile();
        }

        private void mnFileOpen_Click(object sender, EventArgs e)
        {
            fileManager.OpenFile();
        }
        private void tmFileOpen_Click(object sender, EventArgs e)
        {
            fileManager.OpenFile();
        }

        private void mnEditCopy_Click(object sender, EventArgs e)
        {
            editManager.Copy();
        }
        private void tmEditCopy_Click(object sender, EventArgs e)
        {
            editManager.Copy();
        }

        private void mnEditPaste_Click(object sender, EventArgs e)
        {
            editManager.Paste();
        }
        private void tmFilePaste_Click(object sender, EventArgs e)
        {
            editManager.Paste();
        }

        private void mnEditCut_Click(object sender, EventArgs e)
        {
            editManager.Cut();
        }
        private void tmFileCut_Click(object sender, EventArgs e)
        {
            editManager.Cut();
        }

        private void mnEditSelectAll_Click(object sender, EventArgs e)
        {
            editManager.SelectAll();
        }

        private void mnViewAumentar_Click(object sender, EventArgs e)
        {
            viewManager.ZoomIn();
        }
        private void tmViewAumentar_Click(object sender, EventArgs e)
        {
            viewManager.ZoomIn();
        }

        private void mnViewDisminuir_Click(object sender, EventArgs e)
        {
            viewManager.ZoomOut();
        }
        private void tmViewDisminuir_Click(object sender, EventArgs e)
        {
            viewManager.ZoomOut();
        }

        private void mnFilePrint_Click(object sender, EventArgs e)
        {
            fileManager.PrintFile();
        }
        private void tmFilePrint_Click(object sender, EventArgs e)
        {
            fileManager.PrintFile();
        }

        private void mnHelpAcercaDe_Click(object sender, EventArgs e)
        {
            helpManager.ShowAbout();
        }
        private void mnHelpManual_Click(object sender, EventArgs e)
        {
            helpManager.ShowAbout();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            fileManager.ExitProgram();
            e.Cancel = true;
        }

        private void mnEditFont_Click(object sender, EventArgs e)
        {
            editManager.Fonts();
        }

        private void mnEditColors_Click(object sender, EventArgs e)
        {
            editManager.Colors();
        }

        private void tmFileFont_Click(object sender, EventArgs e)
        {
            editManager.Fonts();
        }

        private void tmFileColor_Click(object sender, EventArgs e)
        {
            editManager.Colors();
        }

        private void StatusName_Click(object sender, EventArgs e)
        {
            string fileName = "MiDocumento.txt";

            StatusName.Text = $"Documento: {fileName}";
        }

        private void StatusWeigth_Click(object sender, EventArgs e)
        {
            {
                string count = RichTextBox.Text;
                StatusWeigth.Text = (count.Split(new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries).Length + "kb").ToString();

            }
        }
        private void StatusCreation_Click(object sender, EventArgs e)
        {

        }
        private void StatusModification_Click(object sender, EventArgs e)
        {

        }


        public class FileManager
        {
            private Form owner;
            private RichTextBox richTextBox;

            public FileManager(Form owner, RichTextBox richTextBox)
            {
                this.owner = owner;
                this.richTextBox = richTextBox;
            }

            public void ExitProgram()
            {
                DialogResult option = MessageBox.Show(owner,
                                                       "Desea cerrar la aplicacion?",
                                                       "Salir",
                                                       MessageBoxButtons.YesNo,
                                                       MessageBoxIcon.Question,
                                                       MessageBoxDefaultButton.Button2);
                if (option == DialogResult.Yes)
                {
                    DialogResult saveOption = MessageBox.Show(owner,
                                                              "Desea guardar el documento antes de salir?",
                                                              "Guardar",
                                                              MessageBoxButtons.YesNo,
                                                              MessageBoxIcon.Question,
                                                              MessageBoxDefaultButton.Button1);

                    if (saveOption == DialogResult.Yes)
                    {
                        SaveFile();
                    }
                    owner.Dispose();
                }
            }

            public void NewFile()
            {
                DialogResult opcion = MessageBox.Show(owner,
                                                        "Desea guardar el documento?",
                                                        "Guardar",
                                                        MessageBoxButtons.YesNo,
                                                        MessageBoxIcon.Question,
                                                        MessageBoxDefaultButton.Button1);
                if (opcion == DialogResult.Yes)
                {
                    SaveFile();
                }
            }

            public void SaveFile()
            {
                using (SaveFileDialog guardar = new SaveFileDialog())
                {
                    guardar.Filter = "Documento de texto|*.txt";
                    guardar.Title = "Guardar RichTextBox";
                    guardar.FileName = "Sin titulo 1";
                    if (guardar.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter escribir = new StreamWriter(guardar.FileName))
                        {
                            foreach (string line in richTextBox.Lines)
                            {
                                escribir.WriteLine(line);
                            }
                        }
                    }
                }
            }

            public void OpenFile()
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "Abrir archivo";
                    openFileDialog.Filter = "Archivo (*.txt)|*.txt";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        try
                        {
                            string fileContent = File.ReadAllText(filePath);
                            richTextBox.Clear();
                            richTextBox.Text = fileContent;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al abrir el archivo: " + ex.Message,
                                            "Error",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                        }
                        string fileName = Path.GetFileName(filePath);
                    }
                }
            }

            public void PrintFile()
            {
                using (PrintDialog printDialog = new PrintDialog())
                {
                    PrintDocument printDocument = new PrintDocument();
                    printDialog.Document = printDocument;
                    printDocument.PrintPage += PrintPageHandler;
                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        printDocument.Print();
                    }
                }
            }

            private void PrintPageHandler(object sender, PrintPageEventArgs e)
            {
                if (richTextBox != null)
                {
                    Font printFont = richTextBox.Font;
                    string textToPrint = richTextBox.Text;
                    e.Graphics.DrawString(textToPrint, printFont, Brushes.Black,
                        new RectangleF(50, 50, e.PageBounds.Width - 100, e.PageBounds.Height - 100));
                }
            }
        }

        public class EditManager
        {
            private RichTextBox richTextBox;

            public EditManager(RichTextBox richTextBox)
            {
                this.richTextBox = richTextBox;
            }

            public void Copy()
            {
                richTextBox.Copy();
            }

            public void Paste()
            {
                richTextBox.Paste();
            }

            public void Cut()
            {
                richTextBox.Cut();
            }

            public void SelectAll()
            {
                richTextBox.SelectAll();
            }
            public void Fonts()
            {
                Font currentFont = richTextBox.Font;
                Color currentColor = richTextBox.ForeColor;

                using (FontDialogForm dialog = new FontDialogForm(currentFont, currentColor))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        richTextBox.Font = dialog.SelectedFont;
                        richTextBox.ForeColor = dialog.SelectedColor;
                    }
                }
            }
            public void Colors()
            {
                Font currentFont = richTextBox.Font;
                Color currentColor = richTextBox.ForeColor;

                using (FontDialogForm dialog = new FontDialogForm(currentFont, currentColor))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        richTextBox.Font = dialog.SelectedFont;
                        richTextBox.ForeColor = dialog.SelectedColor;
                    }
                }
            }

            public class ViewManager
            {
                private RichTextBox richTextBox;

                public ViewManager(RichTextBox richTextBox)
                {
                    this.richTextBox = richTextBox;
                }

                public void ZoomIn()
                {
                    if (richTextBox.ZoomFactor < 5.0f)
                    {
                        richTextBox.ZoomFactor += 0.1f;
                    }
                }

                public void ZoomOut()
                {
                    if (richTextBox.ZoomFactor > 0.1f)
                    {
                        richTextBox.ZoomFactor -= 0.1f;
                    }
                }
            }

            public class HelpManager
            {
                public void ShowAbout()
                {
                    MessageBox.Show("Soy flojo voy a juntar ambos, total nadie lo va a leer de todos modos" +
                        "\n\nAcerca de: Version preliminar de aplicacion tipo bloc de notas" +
                        "\n\nManuela: es casi lo mismo que el Bloc de notas pero con monas chinas xddd",
                        "Manual y Acerca De");
                }
            }
        }


    }
}

