// Decompiled with JetBrains decompiler
// Type: Foody.IO.TXT.Exporter
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Generic;
using Foody.Models;
using System;
using System.Text;

namespace Foody.IO.TXT
{
  public static class Exporter
  {
    public static string ExportToText(RecipeContent[] contents)
    {
      UnitConverter unitConverter = new UnitConverter();
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < contents.Length; ++index)
      {
        if (contents[index].Quantity > 0.0)
          stringBuilder.Append(string.Format("{0}\t[{1}({2})]{3}", (object) Tools.FindContentByUid(contents[index].uid).Name, (object) contents[index].Quantity, (object) unitConverter.GetString(contents[index].QuantityUnit), (object) Environment.NewLine));
      }
      return stringBuilder.ToString();
    }
  }
}
