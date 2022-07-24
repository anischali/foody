// Decompiled with JetBrains decompiler
// Type: Foody.ControlHelpers
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using System.Drawing;
using System.Windows.Forms;

namespace Foody
{
  public static class ControlHelpers
  {
    public static DataGridViewComboBoxCell CreateStyledComboBox(
      string[] values)
    {
      DataGridViewComboBoxCell styledComboBox = new DataGridViewComboBoxCell();
      styledComboBox.Style.BackColor = Color.FromArgb(37, 37, 38);
      styledComboBox.AutoComplete = true;
      styledComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
      styledComboBox.FlatStyle = FlatStyle.Flat;
      styledComboBox.DataSource = (object) values;
      styledComboBox.MaxDropDownItems = 10;
      styledComboBox.ReadOnly = false;
      return styledComboBox;
    }

    public static DataGridViewButtonCell CreateStyledButtonCell(string name)
    {
      DataGridViewButtonCell styledButtonCell = new DataGridViewButtonCell();
      styledButtonCell.Style.ForeColor = Color.FromArgb(224, 224, 224);
      styledButtonCell.FlatStyle = FlatStyle.Flat;
      styledButtonCell.Value = (object) name;
      return styledButtonCell;
    }

    public static DataGridViewTextBoxCell CreateStyledTextBoxCell()
    {
      DataGridViewTextBoxCell styledTextBoxCell = new DataGridViewTextBoxCell();
      styledTextBoxCell.Style.BackColor = Color.FromArgb(37, 37, 38);
      styledTextBoxCell.ReadOnly = false;
      styledTextBoxCell.Style.ForeColor = Color.White;
      styledTextBoxCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
      return styledTextBoxCell;
    }
  }
}
