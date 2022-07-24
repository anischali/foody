// Decompiled with JetBrains decompiler
// Type: Foody.Properties.Resources
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Foody.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Foody.Properties.Resources.resourceMan == null)
          Foody.Properties.Resources.resourceMan = new ResourceManager("Foody.Properties.Resources", typeof (Foody.Properties.Resources).Assembly);
        return Foody.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Foody.Properties.Resources.resourceCulture;
      set => Foody.Properties.Resources.resourceCulture = value;
    }

    internal static Bitmap defaultCellBG => (Bitmap) Foody.Properties.Resources.ResourceManager.GetObject(nameof (defaultCellBG), Foody.Properties.Resources.resourceCulture);

    internal static string NotSelectedImage => Foody.Properties.Resources.ResourceManager.GetString(nameof (NotSelectedImage), Foody.Properties.Resources.resourceCulture);

    internal static Bitmap partialSelect => (Bitmap) Foody.Properties.Resources.ResourceManager.GetObject(nameof (partialSelect), Foody.Properties.Resources.resourceCulture);

    internal static Bitmap selected => (Bitmap) Foody.Properties.Resources.ResourceManager.GetObject(nameof (selected), Foody.Properties.Resources.resourceCulture);

    internal static string SelectedImage => Foody.Properties.Resources.ResourceManager.GetString(nameof (SelectedImage), Foody.Properties.Resources.resourceCulture);

    internal static Bitmap unselected => (Bitmap) Foody.Properties.Resources.ResourceManager.GetObject(nameof (unselected), Foody.Properties.Resources.resourceCulture);
  }
}
