// Decompiled with JetBrains decompiler
// Type: Foody.Controls.AddRecipePopup
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Foody.Controls
{
  public class AddRecipePopup : UserControl
  {
    private int idx = -1;
    public Action<int> action;
    private readonly Form form = new Form();
    private IContainer components = (IContainer) null;
    private ComboBox Data;
    private Button cancelButton;
    private Button addRecipe;

    public AddRecipePopup(string[] toView)
    {
      this.InitializeComponent();
      this.PopulateData(toView);
    }

    private void InitForm()
    {
      this.form.Size = this.Size;
      this.form.ShowIcon = false;
      this.form.FormBorderStyle = FormBorderStyle.None;
      this.form.Controls.Add((Control) this);
      this.form.StartPosition = FormStartPosition.CenterScreen;
      int num = (int) this.form.ShowDialog();
    }

    private void PopulateData(string[] toView) => this.Data.Items.AddRange((object[]) toView);

    public void InitEvents()
    {
      this.addRecipe.Click += (EventHandler) ((s, e) =>
      {
        if (this.action != null)
          this.action(this.Data.SelectedIndex);
        this.Dispose();
        this.form.Close();
        this.form.Dispose();
      });
      this.cancelButton.Click += (EventHandler) ((s, e) =>
      {
        this.Dispose();
        this.form.Close();
        this.form.Dispose();
      });
    }

    public void ShowPopup()
    {
      this.InitEvents();
      this.InitForm();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.Data = new ComboBox();
      this.cancelButton = new Button();
      this.addRecipe = new Button();
      this.SuspendLayout();
      this.Data.BackColor = Color.FromArgb(37, 40, 42);
      this.Data.FlatStyle = FlatStyle.Flat;
      this.Data.Font = new Font("Microsoft Sans Serif", 10.8f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.Data.ForeColor = SystemColors.ButtonHighlight;
      this.Data.Location = new Point(17, 12);
      this.Data.Name = "Data";
      this.Data.Size = new Size(622, 30);
      this.Data.TabIndex = 0;
      this.cancelButton.BackColor = Color.FromArgb(222, 45, 32);
      this.cancelButton.FlatStyle = FlatStyle.Flat;
      this.cancelButton.ForeColor = SystemColors.WindowText;
      this.cancelButton.Location = new Point(448, 109);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new Size(75, 31);
      this.cancelButton.TabIndex = 1;
      this.cancelButton.Text = "Annuler";
      this.cancelButton.UseVisualStyleBackColor = false;
      this.addRecipe.BackColor = Color.FromArgb(142, 210, 138);
      this.addRecipe.FlatStyle = FlatStyle.Flat;
      this.addRecipe.ForeColor = SystemColors.MenuText;
      this.addRecipe.Location = new Point(529, 109);
      this.addRecipe.Name = "addRecipe";
      this.addRecipe.Size = new Size(75, 31);
      this.addRecipe.TabIndex = 2;
      this.addRecipe.Text = "Ajouter";
      this.addRecipe.UseVisualStyleBackColor = false;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(37, 37, 38);
      this.BorderStyle = BorderStyle.Fixed3D;
      this.Controls.Add((Control) this.addRecipe);
      this.Controls.Add((Control) this.cancelButton);
      this.Controls.Add((Control) this.Data);
      this.ForeColor = SystemColors.ActiveCaption;
      this.ImeMode = ImeMode.NoControl;
      this.Name ="AddRecipePopup";
      this.Size = new Size(652, 151);
      this.ResumeLayout(false);
    }
  }
}
