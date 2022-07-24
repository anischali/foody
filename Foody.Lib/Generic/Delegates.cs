// Decompiled with JetBrains decompiler
// Type: Foody.Generic.Delegates
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Models;

namespace Foody.Generic
{
  public static class Delegates
  {
    public delegate void addToListOfContentsDelegate(RecipeContent content);

    public delegate void addContentsToDatabaseDelegate(Recipe content);

    public delegate void closeAddContentPanelDelegate();

    public delegate void editRecipeContentDelegate(string uid, double quantity, Unit unit);

    public delegate void removeRecipeContentDelegate(string uid);

    public delegate void returnToHomePanel();

    public delegate void Back();

    public delegate void GenerateEvent(int number);
  }
}
