using System;
using System.Drawing;
using System.Windows.Forms;
using QRCoder;

public class QRForm : Form
{
    public QRForm(string url)
    {
        Text = "Scan to Connect to MyRemote";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        var qrGenerator = new QRCodeGenerator();
        var qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M);
        var qrCode = new QRCode(qrData);
        var qrImage = qrCode.GetGraphic(10);
        ClientSize = new Size(350, 350);

        var pictureBox = new PictureBox
        {
            Dock = DockStyle.Fill,
            Image = qrImage,
            SizeMode = PictureBoxSizeMode.Zoom
        };

        Controls.Add(pictureBox);
    }
}
