// Decompiled with JetBrains decompiler
// Type: Foody.Controls.GenerateHeader
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using Foody.Generic;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Foody.Controls
{
  public class GenerateHeader : UserControl
  {
    public Delegates.GenerateEvent generateEvent;
    private IContainer components = (IContainer) null;
    private Label lblMealNumber;
    private TextBox tbGenerateMealsNumber;
    private Button btnGenerateNumber;

    public GenerateHeader()
    {
      this.InitializeComponent();
      this.InitEvents();
    }

    private void InitEvents() => this.btnGenerateNumber.Click += (EventHandler) ((s, e) =>
    {
      if (this.generateEvent == null)
        return;
      this.generateEvent(Tools.ParseInt(this.tbGenerateMealsNumber.Text));
    });

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblMealNumber = new Label();
      this.tbGenerateMealsNumber = new TextBox();
      this.btnGenerateNumber = new Button();
      this.SuspendLayout();
      this.lblMealNumber.AutoSize = true;
      this.lblMealNumber.FlatStyle = FlatStyle.Flat;
      this.lblMealNumber.ForeColor = Color.FromArgb(228, 95, 1);
      this.lblMealNumber.Location = new Point(3, 19);
      this.lblMealNumber.Name = "lblMealNumber";
      this.lblMealNumber.Size = new Size(118, 17);
      this.lblMealNumber.TabIndex = 0;
      this.lblMealNumber.Text = "Nombre de repas";
      this.tbGenerateMealsNumber.BackColor = Color.FromArgb(37, 37, 38);
      this.tbGenerateMealsNumber.BorderStyle = BorderStyle.FixedSingle;
      this.tbGenerateMealsNumber.ForeColor = Color.FromArgb(228, 95, 1);
      this.tbGenerateMealsNumber.Location = new Point(141, 14);
      this.tbGenerateMealsNumber.Name = "tbGenerateMealsNumber";
      this.tbGenerateMealsNumber.Size = new Size(40, 22);
      this.tbGenerateMealsNumber.TabIndex = 1;
      this.tbGenerateMealsNumber.TextAlign = HorizontalAlignment.Center;
      this.btnGenerateNumber.FlatStyle = FlatStyle.Flat;
      this.btnGenerateNumber.Font = new Font("Roboto", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnGenerateNumber.ForeColor = Color.FromArgb(229, 9, 20);
      this.btnGenerateNumber.Location = new Point(200, 10);
      this.btnGenerateNumber.Name = "btnGenerateNumber";
      this.btnGenerateNumber.Size = new Size(88, 35);
      this.btnGenerateNumber.TabIndex = 2;
      this.btnGenerateNumber.Text = "Générer";
      this.btnGenerateNumber.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(37, 37, 38);
      this.Controls.Add((Control) this.lblMealNumber);
      this.Controls.Add((Control) this.tbGenerateMealsNumber);
      this.Controls.Add((Control) this.btnGenerateNumber);
      this.Name = nameof (GenerateHeader);
      this.Size = new Size(669, 51);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
