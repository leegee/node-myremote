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
        ClientSize = new Size(350, 350);

        var qrGenerator = new QRCodeGenerator();
        var qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode(qrData);
        var qrImage = qrCode.GetGraphic(20);

        var pictureBox = new PictureBox
        {
            Dock = DockStyle.Fill,
            Image = qrImage,
            SizeMode = PictureBoxSizeMode.CenterImage
        };

        Controls.Add(pictureBox);
    }
}
