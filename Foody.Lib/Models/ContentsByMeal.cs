// Decompiled with JetBrains decompiler
// Type: Foody.Lib.Models.ContentsByMeal
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using System.Collections.Generic;
using System.Linq;

namespace Foody.Lib.Models
{
  public class ContentsByMeal
  {
    private string contentUid;
    private Dictionary<string, double> quantities;
    private Dictionary<string, string> TagsToDisplay;

    public ContentsByMeal()
    {
      this.contentUid = string.Empty;
      this.quantities = new Dictionary<string, double>();
      this.TagsToDisplay = new Dictionary<string, string>();
    }

    public string Label
    {
      get
      {
        string str = "";
        foreach (string key in this.quantities.Keys.ToArray<string>())
          str += string.Format("{0}:{1}-", (object) this.TagsToDisplay[key], (object) this.quantities[key]);
        return str.Remove(str.Length - 1, 1);
      }
    }

    public void Set(string Tag, string RecipeUid, double Quantity)
    {
      if (this.quantities.ContainsKey(RecipeUid))
      {
        this.quantities[RecipeUid] = Quantity;
        if (this.TagsToDisplay.ContainsKey(RecipeUid))
          this.TagsToDisplay[RecipeUid] = Tag;
        else
          this.TagsToDisplay.Add(RecipeUid, Tag);
      }
      else
      {
        this.quantities.Add(RecipeUid, Quantity);
        this.TagsToDisplay.Add(RecipeUid, Tag);
      }
    }
  }
}
