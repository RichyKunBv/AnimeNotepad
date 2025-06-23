using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace webiwabo
{
    public partial class FontDialogForm : Form
    {
        public Font SelectedFont { get; private set; }
        public Color SelectedColor { get; private set; }

        public FontDialogForm(Font initialFont, Color initialColor)
        {
            InitializeComponent();
            ConfigureInitialSettings(initialFont, initialColor);
        }

        private void ConfigureInitialSettings(Font initialFont, Color initialColor)
        {
            // Configuración de fuente
            cmbFonts.Items.AddRange(FontFamily.Families.Select(f => f.Name).ToArray());
            cmbFonts.SelectedItem = initialFont.FontFamily.Name;
            numSize.Value = (decimal)initialFont.Size;
            chkBold.Checked = initialFont.Bold;
            chkItalic.Checked = initialFont.Italic;
            chkUnderline.Checked = initialFont.Underline;

            // Configuración de color
            pnlColorPreview.BackColor = initialColor;
            SelectedColor = initialColor;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                // Configurar fuente
                FontStyle style = FontStyle.Regular;
                if (chkBold.Checked) style |= FontStyle.Bold;
                if (chkItalic.Checked) style |= FontStyle.Italic;
                if (chkUnderline.Checked) style |= FontStyle.Underline;

                SelectedFont = new Font(
                    cmbFonts.SelectedItem.ToString(),
                    (float)numSize.Value,
                    style
                );

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Close();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnSelectColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = SelectedColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    SelectedColor = colorDialog.Color;
                    pnlColorPreview.BackColor = colorDialog.Color;
                }
            }
        }
    }
}