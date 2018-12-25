using System;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AgusRdz.Barcodes;

namespace AgusRdz.Barcodes.Demo
{
    public partial class Form1 : Form
    {
        private EAN13 _ean13 = new EAN13();
        private string _path = @"C:\myFolder\";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBarcode.Text))
                _ean13.CreateBarcode(txtBarcode.Text, _path + "basic.png", ImageFormat.Png);
            else
                MessageBox.Show("Barcode is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBarcode.Text) && !string.IsNullOrEmpty(txtProduct.Text))
                _ean13.CreateBarcode(txtBarcode.Text, txtProduct.Text, _path + "medium.bmp", ImageFormat.Bmp);
            else
                MessageBox.Show("Barcode and product are required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBarcode.Text) && !string.IsNullOrEmpty(txtProduct.Text) && !string.IsNullOrEmpty(txtDetails.Text))
                _ean13.CreateBarcode(txtBarcode.Text, txtProduct.Text, txtDetails.Text, _path + "full.jpg", ImageFormat.Jpeg);
            else
                MessageBox.Show("All fields are required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        
    }
}
