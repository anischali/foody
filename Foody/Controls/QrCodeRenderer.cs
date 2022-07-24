// Decompiled with JetBrains decompiler
// Type: Foody.Controls.QrCodeRenderer
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using Foody.Generic;
using Foody.IO.QrCode;
using Foody.Models;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Foody.Controls
{
  public class QrCodeRenderer : UserControl
  {
    private IContainer components = (IContainer) null;
    private PictureBox pbQrCode;

    public QrCodeRenderer(RecipeContent[] contents = null)
    {
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.pbQrCode.Image = (Image) Exporter.GetQrCodeVersion<string>(this.GenerateContents(contents), 400, 400);
    }

    private string GenerateContents(RecipeContent[] contents)
    {
      UnitConverter unitConverter = new UnitConverter();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[");
      for (int index = 0; index < contents.Length; ++index)
      {
        if (contents[index].Quantity > 0.0)
        {
          stringBuilder.Append("{");
          stringBuilder.Append(string.Format("{0}:[{1},{2}]", (object) Tools.FindContentByUid(contents[index].uid).Name, (object) contents[index].Quantity, (object) unitConverter.GetString(contents[index].QuantityUnit)));
          stringBuilder.Append("},");
        }
      }
      stringBuilder.Remove(stringBuilder.Length - 1, 1);
      stringBuilder.Append("]");
      return stringBuilder.ToString();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pbQrCode = new PictureBox();
      ((ISupportInitialize) this.pbQrCode).BeginInit();
      this.SuspendLayout();
      this.pbQrCode.Dock = DockStyle.Fill;
      this.pbQrCode.Location = new Point(0, 0);
      this.pbQrCode.Margin = new Padding(5);
      this.pbQrCode.Name = "pbQrCode";
      this.pbQrCode.Padding = new Padding(5);
      this.pbQrCode.Size = new Size(645, 440);
      this.pbQrCode.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pbQrCode.TabIndex = 0;
      this.pbQrCode.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(37, 40, 42);
      this.Controls.Add((Control) this.pbQrCode);
      this.Name = nameof (QrCodeRenderer);
      this.Size = new Size(645, 440);
      ((ISupportInitialize) this.pbQrCode).EndInit();
      this.ResumeLayout(false);
    }
  }
}
