// Decompiled with JetBrains decompiler
// Type: Foody.RecipeExporter
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Generic;
using Foody.IO;
using Foody.Lib.Generic.Interfaces;
using Foody.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Foody
{
  public class RecipeExporter : IExporter
  {
    private StringBuilder formatedValue;
    private TagConverter converter = new TagConverter();
    private UnitConverter unitConverter = new UnitConverter();
    private List<string> recipes = new List<string>();

    public void Export(int index)
    {
      if (index < 0 && index >= Database.AllMenus.Count)
        return;
      Recipe allMenu = Database.AllMenus[index];
      this.formatedValue = new StringBuilder();
      this.formatedValue.Append("{" + Environment.NewLine);
      this.formatedValue.Append(allMenu.title + ";" + Environment.NewLine);
      this.formatedValue.Append(string.Format("{0};{1}", (object) allMenu.PeopleNumber, (object) Environment.NewLine));
      this.formatedValue.Append("{" + Environment.NewLine);
      for (int index1 = 0; index1 < allMenu.Tags.Count; ++index1)
        this.formatedValue.Append(this.converter.GetString(allMenu.Tags[index1]) + "," + Environment.NewLine);
      this.formatedValue.Remove(this.formatedValue.Length - 2, 2);
      this.formatedValue.Append(Environment.NewLine + "};{" + Environment.NewLine);
      for (int index2 = 0; index2 < allMenu.Contents.Count; ++index2)
        this.formatedValue.Append(string.Format("{0}:{1}:{2},{3}", (object) Tools.FindContentNameByUid(allMenu.Contents[index2].uid), (object) allMenu.Contents[index2].Quantity, (object) this.unitConverter.GetString(allMenu.Contents[index2].QuantityUnit), (object) Environment.NewLine));
      this.formatedValue.Remove(this.formatedValue.Length - 2, 2);
      this.formatedValue.Append(Environment.NewLine + "};" + Environment.NewLine);
      this.formatedValue.Append(allMenu.Description + Environment.NewLine);
      this.formatedValue.Append("}" + Environment.NewLine);
      this.recipes.Add(this.formatedValue.ToString());
      this.formatedValue = (StringBuilder) null;
    }

    public void Import()
    {
    }

    public void Load(string filename) => throw new NotImplementedException();

    public void Save(string filename)
    {
      foreach (string recipe in this.recipes)
        File.AppendAllText(filename, recipe);
    }
  }
}
