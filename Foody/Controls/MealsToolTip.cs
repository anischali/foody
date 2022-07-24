// Decompiled with JetBrains decompiler
// Type: Foody.Controls.MealsToolTip
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Foody.Controls
{
  public class MealsToolTip : UserControl
  {
    private List<string> strLabels = new List<string>();
    private List<Label> labelsList = new List<Label>();
    private Form ToolTipWindow = (Form) null;
    private IContainer components = (IContainer) null;
    private FlowLayoutPanel Labels;

    public MealsToolTip(string[] texts = null)
    {
      this.InitializeComponent();
      if (texts == null)
        return;
      this.Contents = texts;
    }

    private Form GetNewInstance(Control control)
    {
      this.ToolTipWindow = new Form();
      this.ToolTipWindow.FormBorderStyle = FormBorderStyle.None;
      this.ToolTipWindow.Visible = false;
      control.Dock = DockStyle.Fill;
      this.ToolTipWindow.Size = control.Size;
      this.ToolTipWindow.Location = control.Location;
      this.ToolTipWindow.Controls.Add(control);
      return this.ToolTipWindow;
    }

    private Label GetLabel(string text)
    {
      Label label = new Label();
      label.Text = text;
      label.ForeColor = Color.White;
      return label;
    }

    private Size SetAutoSize() => new Size(100, this.strLabels.Count * 10);

    public string[] Contents
    {
      get => this.strLabels.ToArray();
      set
      {
        if (value == null)
          return;
        this.strLabels.Clear();
        this.strLabels.AddRange((IEnumerable<string>) value);
        this.labelsList.Clear();
        foreach (string text in value)
          this.labelsList.Add(this.GetLabel(text));
        this.Labels.Controls.Clear();
        this.Size = this.Labels.Size = this.SetAutoSize();
        this.Labels.Controls.AddRange((Control[]) this.labelsList.ToArray());
      }
    }

    public void Render(Point position)
    {
      if (this.strLabels.Count == 0)
        return;
      this.Location = position;
      if (this.ToolTipWindow != null)
      {
        this.ToolTipWindow.Hide();
        this.ToolTipWindow.Dispose();
        this.ToolTipWindow = (Form) null;
      }
      this.ToolTipWindow = this.GetNewInstance((Control) this);
      if (this.ToolTipWindow != null && !this.ToolTipWindow.IsDisposed)
        this.ToolTipWindow.Show();
      ToolTip toolTip = new ToolTip();
    }

    public void HideToolTip()
    {
      if (this.ToolTipWindow.IsDisposed)
        return;
      this.ToolTipWindow.Close();
      this.ToolTipWindow.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.Labels = new FlowLayoutPanel();
      this.SuspendLayout();
      this.Labels.AutoSize = true;
      this.Labels.Dock = DockStyle.Fill;
      this.Labels.FlowDirection = FlowDirection.TopDown;
      this.Labels.Location = new Point(0, 0);
      this.Labels.Name = "Labels";
      this.Labels.Size = new Size(169, 39);
      this.Labels.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(37, 40, 42);
      this.Controls.Add((Control) this.Labels);
      this.Name = nameof (MealsToolTip);
      this.Size = new Size(169, 39);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
