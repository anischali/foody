// Decompiled with JetBrains decompiler
// Type: Foody.Lib.Generic.SearchEngine
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Generic;
using System;
using System.Threading.Tasks;

namespace Foody.Lib.Generic
{
  public class SearchEngine
  {
    private IndexCollection recipes = new IndexCollection();
    public EventHandler AsyncRefresh;

    public void ResetEngine()
    {
      this.recipes.Clear();
      if (this.AsyncRefresh == null)
        return;
      this.AsyncRefresh((object) this, (EventArgs) null);
    }

    public void Search(
      string title,
      Tag[] tags,
      bool tagInclusion,
      int[] ingredients,
      bool ingredientsInclusion)
    {
      this.recipes.Clear();
      Task.Factory.StartNew((Action) (() =>
      {
        this.SearchByTitle(title);
        int[] array1 = this.SearchByIngredients(ingredients, ingredientsInclusion);
        this.recipes.Clear();
        this.recipes.AddRange(array1, true);
        int[] array2 = this.SearchByTags(tags, tagInclusion);
        this.recipes.Clear();
        this.recipes.AddRange(array2, true);
        if (this.AsyncRefresh == null)
          return;
        this.AsyncRefresh((object) this, (EventArgs) null);
      }));
    }

    public void SearchByTitle(string title)
    {
      if (string.IsNullOrEmpty(title))
        this.recipes.AddRange(Tools.FindAllMatchTitleRecipesIndexs((string) null).ToArray(), true);
      else
        this.recipes.AddRange(Tools.FindAllMatchTitleRecipesIndexs(title).ToArray(), true);
    }

    public int[] SearchByIngredients(int[] ingredients, bool inclusion = false) => ingredients == null || ingredients.Length == 0 ? this.recipes.Values : Tools.FindAllRecipesMatchingContents(this.recipes.Values, ingredients, inclusion);

    public int[] SearchByTags(Tag[] tags, bool inclusion) => tags == null || tags.Length == 0 ? this.recipes.Values : Tools.FindAllRecipesMatchingTags(this.recipes.Values, tags, inclusion);

    public bool Active => this.recipes.Length > 0;

    public int[] SearchResult => this.recipes.Values;
  }
}
