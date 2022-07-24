// Decompiled with JetBrains decompiler
// Type: Foody.Primitives.CheckComboBox
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Foody.Primitives
{
  public class CheckComboBox : System.Windows.Forms.ComboBox
  {
    public CheckComboBox()
    {
      this.DrawMode = DrawMode.OwnerDrawFixed;
      this.DrawItem += new DrawItemEventHandler(this.CheckComboBox_DrawItem);
      this.SelectedIndexChanged += new EventHandler(this.CheckComboBox_SelectedIndexChanged);
      this.SelectedText = "Select Options";
    }

    private void CheckComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      CheckComboBoxItem selectedItem = (CheckComboBoxItem) this.SelectedItem;
      selectedItem.CheckState = !selectedItem.CheckState;
      if (this.CheckStateChanged == null)
        return;
      this.CheckStateChanged((object) selectedItem, e);
    }

    private void CheckComboBox_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index == -1)
        return;
      if (!(this.Items[e.Index] is CheckComboBoxItem))
      {
        Graphics graphics = e.Graphics;
        string s = this.Items[e.Index].ToString();
        Font font = this.Font;
        Brush white = Brushes.White;
        Rectangle bounds = e.Bounds;
        int x = bounds.X;
        bounds = e.Bounds;
        int y = bounds.Y;
        PointF point = (PointF) new Point(x, y);
        graphics.DrawString(s, font, white, point);
      }
      else
      {
        CheckComboBoxItem checkComboBoxItem = (CheckComboBoxItem) this.Items[e.Index];
        CheckBoxRenderer.RenderMatchingApplicationState = true;
        Graphics graphics = e.Graphics;
        Rectangle bounds1 = e.Bounds;
        int x = bounds1.X;
        bounds1 = e.Bounds;
        int y = bounds1.Y;
        Point glyphLocation = new Point(x, y);
        Rectangle bounds2 = e.Bounds;
        string text = checkComboBoxItem.Text;
        Font font = this.Font;
        int num = (e.State & DrawItemState.Focus) == DrawItemState.None ? 1 : 0;
        int state = checkComboBoxItem.CheckState ? 5 : 1;
        CheckBoxRenderer.DrawCheckBox(graphics, glyphLocation, bounds2, text, font, num != 0, (CheckBoxState) state);
      }
    }

    public event EventHandler CheckStateChanged;
  }
}
