// Decompiled with JetBrains decompiler
// Type: Foody.Generic.RecipeComparer
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Models;
using System.Collections.Generic;

namespace Foody.Generic
{
  public class RecipeComparer : IComparer<Recipe>
  {
    public int Compare(Recipe x, Recipe y) => x.title == null || y.title == null ? 0 : x.title.CompareTo(y.title);
  }
}
