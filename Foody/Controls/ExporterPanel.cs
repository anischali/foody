// Decompiled with JetBrains decompiler
// Type: Foody.Controls.ExporterPanel
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using Foody.Models;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Foody.Controls
{
  public class ExporterPanel : UserControl
  {
    private MealGenerator generator;
    private Timer timer = new Timer();
    private DateTime InitTime;
    private IContainer components = (IContainer) null;
    private Button btnPdfExport;
    private Button btnTxtExport;
    private Button btnQRExport;

    public ExporterPanel()
    {
      this.InitializeComponent();
      this.InitEvents();
      this.timer.Interval = 5000;
      this.InitTime = DateTime.Now;
      this.timer.Tick += (EventHandler) ((s, e) =>
      {
        if (DateTime.Now.Subtract(this.InitTime).TotalSeconds <= 5.0 || !this.Visible)
          return;
        this.Hide();
      });
      this.timer.Start();
    }

    private void InitEvents()
    {
      this.btnPdfExport.Click += (EventHandler) ((s, e) =>
      {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "PDF Files *.pdf|*.pdf";
        saveFileDialog.CheckPathExists = true;
        if (saveFileDialog.ShowDialog() != DialogResult.OK)
          return;
        string path = saveFileDialog.FileName.EndsWith(".pdf") ? saveFileDialog.FileName : saveFileDialog.FileName + ".pdf";
        Foody.IO.PDF.Exporter exporter = new Foody.IO.PDF.Exporter();
        exporter.Generate(this.generator);
        exporter.Save(path);
      });
      this.btnQRExport.Click += (EventHandler) ((s, e) =>
      {
        QrCodeRenderer qrCodeRenderer = new QrCodeRenderer(this.generator.Contents);
        int num = (int) new Form()
        {
          ShowInTaskbar = false,
          ShowIcon = false,
          Height = (qrCodeRenderer.Height + qrCodeRenderer.Height / 2),
          Width = (qrCodeRenderer.Width + qrCodeRenderer.Width / 5),
          Controls = {
            (Control) qrCodeRenderer
          }
        }.ShowDialog();
      });
      this.btnTxtExport.Click += (EventHandler) ((s, e) =>
      {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "TXT Files *.txt|*.txt";
        saveFileDialog.CheckPathExists = true;
        if (saveFileDialog.ShowDialog() != DialogResult.OK)
          return;
        File.AppendAllText(saveFileDialog.FileName.EndsWith(".txt") ? saveFileDialog.FileName : saveFileDialog.FileName + ".txt", Foody.IO.TXT.Exporter.ExportToText(this.generator.Contents));
      });
    }

    public MealGenerator Generator
    {
      get => this.generator;
      set
      {
        this.generator = value;
        this.timer.Stop();
        this.timer.Start();
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ExporterPanel));
      this.btnPdfExport = new Button();
      this.btnTxtExport = new Button();
      this.btnQRExport = new Button();
      this.SuspendLayout();
      this.btnPdfExport.BackgroundImage = (Image) componentResourceManager.GetObject("btnPdfExport.BackgroundImage");
      this.btnPdfExport.BackgroundImageLayout = ImageLayout.Stretch;
      this.btnPdfExport.FlatAppearance.BorderColor = Color.FromArgb(51, 149, 214);
      this.btnPdfExport.FlatStyle = FlatStyle.Flat;
      this.btnPdfExport.Location = new Point(0, 2);
      this.btnPdfExport.Margin = new Padding(2, 2, 2, 2);
      this.btnPdfExport.Name = "btnPdfExport";
      this.btnPdfExport.Size = new Size(24, 26);
      this.btnPdfExport.TabIndex = 7;
      this.btnPdfExport.UseVisualStyleBackColor = true;
      this.btnTxtExport.BackgroundImage = (Image) componentResourceManager.GetObject("btnTxtExport.BackgroundImage");
      this.btnTxtExport.BackgroundImageLayout = ImageLayout.Stretch;
      this.btnTxtExport.FlatAppearance.BorderColor = Color.FromArgb(51, 149, 214);
      this.btnTxtExport.FlatStyle = FlatStyle.Flat;
      this.btnTxtExport.Location = new Point(29, 2);
      this.btnTxtExport.Margin = new Padding(2, 2, 2, 2);
      this.btnTxtExport.Name = "btnTxtExport";
      this.btnTxtExport.Size = new Size(24, 26);
      this.btnTxtExport.TabIndex = 6;
      this.btnTxtExport.UseVisualStyleBackColor = true;
      this.btnQRExport.BackgroundImage = (Image) componentResourceManager.GetObject("btnQRExport.BackgroundImage");
      this.btnQRExport.BackgroundImageLayout = ImageLayout.Stretch;
      this.btnQRExport.FlatAppearance.BorderColor = Color.FromArgb(51, 149, 214);
      this.btnQRExport.FlatStyle = FlatStyle.Flat;
      this.btnQRExport.Location = new Point(58, 2);
      this.btnQRExport.Margin = new Padding(2, 2, 2, 2);
      this.btnQRExport.Name = "btnQRExport";
      this.btnQRExport.Size = new Size(24, 26);
      this.btnQRExport.TabIndex = 5;
      this.btnQRExport.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(37, 37, 38);
      this.Controls.Add((Control) this.btnPdfExport);
      this.Controls.Add((Control) this.btnTxtExport);
      this.Controls.Add((Control) this.btnQRExport);
      this.Margin = new Padding(2, 2, 2, 2);
      this.Name = nameof (ExporterPanel);
      this.Size = new Size(83, 31);
      this.ResumeLayout(false);
    }
  }
}
