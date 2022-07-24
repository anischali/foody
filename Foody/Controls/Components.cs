// Decompiled with JetBrains decompiler
// Type: Foody.Controls.Components
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using Foody.Properties;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Foody.Controls
{
  public static class Components
  {
    private static Assembly assembly = Assembly.GetExecutingAssembly();
    public static Image[] LabelsBackGround = Components.InitImages();

    private static Image[] InitImages() => new Image[2]
    {
      Image.FromStream(Components.assembly.GetManifestResourceStream(Components.assembly.GetName().Name + ".Resources." + Resources.SelectedImage)),
      Image.FromStream(Components.assembly.GetManifestResourceStream(Components.assembly.GetName().Name + ".Resources." + Resources.NotSelectedImage))
    };

    public static Button TagLabel(string text, bool selected)
    {
      Button button = new Button();
      button.Text = text;
      button.ForeColor = Color.White;
      button.FlatStyle = FlatStyle.Flat;
      button.TextAlign = ContentAlignment.MiddleCenter;
      button.BackgroundImage = selected ? Components.LabelsBackGround[0] : Components.LabelsBackGround[1];
      button.BackgroundImageLayout = ImageLayout.Stretch;
      button.FlatAppearance.BorderSize = 0;
      button.Tag = (object) selected;
      return button;
    }
  }
}
