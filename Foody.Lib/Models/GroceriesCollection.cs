// Decompiled with JetBrains decompiler
// Type: Foody.Models.GroceriesCollection
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Generic;
using Foody.Lib.Models;
using System.Collections.Generic;
using System.Linq;

namespace Foody.Models
{
  public class GroceriesCollection
  {
    private Dictionary<string, RecipeContent> groceries;
    private Dictionary<string, double> quantitiesInStock;
    private Dictionary<string, ContentsByMeal> contentsByMeal;

    public GroceriesCollection()
    {
      this.groceries = new Dictionary<string, RecipeContent>();
      this.quantitiesInStock = new Dictionary<string, double>();
      this.contentsByMeal = new Dictionary<string, ContentsByMeal>();
    }

    public Dictionary<ContentTag, List<RecipeContent>> SortedGroceriesValues
    {
      get
      {
        Dictionary<ContentTag, List<RecipeContent>> sortedGroceriesValues = new Dictionary<ContentTag, List<RecipeContent>>();
        foreach (RecipeContent recipeContent in this.groceries.Values)
        {
          ContentTag type = Tools.FindContentByUid(recipeContent.uid).type;
          if (!sortedGroceriesValues.ContainsKey(type))
            sortedGroceriesValues.Add(type, new List<RecipeContent>());
          if (this.quantitiesInStock.ContainsKey(recipeContent.uid))
            recipeContent.Quantity -= this.quantitiesInStock[recipeContent.uid];
          sortedGroceriesValues[type].Add(recipeContent);
        }
        return sortedGroceriesValues;
      }
    }

    public void SetContentsByMeal(
      string contentUid,
      string Tag,
      string RecipeUid,
      double Quantity)
    {
      if (this.contentsByMeal.ContainsKey(contentUid))
      {
        this.contentsByMeal[contentUid].Set(Tag, RecipeUid, Quantity);
      }
      else
      {
        this.contentsByMeal.Add(contentUid, new ContentsByMeal());
        this.contentsByMeal[contentUid].Set(Tag, RecipeUid, Quantity);
      }
    }

    public string GetContentsByMeal(string contentUid) => this.contentsByMeal.ContainsKey(contentUid) ? this.contentsByMeal[contentUid].Label : string.Empty;

    public RecipeContent[] RecipesContents => this.groceries.Values.ToArray<RecipeContent>();

    public Dictionary<string, double> QuantitiesInStock => this.quantitiesInStock;

    public void Add(RecipeContent content)
    {
      if (this.groceries.ContainsKey(content.uid))
        this.groceries[content.uid].Quantity += content.Quantity;
      else
        this.groceries.Add(content.uid, content);
    }

    public void AddRange(RecipeContent[] contents)
    {
      for (int index = 0; index < contents.Length; ++index)
        this.Add(contents[index].Clone());
    }

    private void Remove(RecipeContent content)
    {
      if (!this.groceries.ContainsKey(content.uid))
        return;
      this.groceries.Remove(content.uid);
      this.quantitiesInStock.Remove(content.uid);
    }

    public void SetQuantity(string uid, double quantity)
    {
      if (!this.groceries.ContainsKey(uid))
        return;
      this.groceries[uid].Quantity = quantity;
    }

    public double GetQuantity(string uid) => this.groceries.ContainsKey(uid) ? this.groceries[uid].Quantity : 0.0;

    public double GetQuantityInStock(string uid) => this.quantitiesInStock.ContainsKey(uid) ? this.quantitiesInStock[uid] : 0.0;

    public void SetQuantityInStock(string uid, double quantity)
    {
      if (this.quantitiesInStock.ContainsKey(uid))
        this.quantitiesInStock[uid] = quantity;
      else
        this.quantitiesInStock.Add(uid, quantity);
    }

    public void Clear()
    {
      this.groceries.Clear();
      this.groceries = new Dictionary<string, RecipeContent>();
      this.quantitiesInStock.Clear();
    }
  }
}
