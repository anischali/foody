// Decompiled with JetBrains decompiler
// Type: Foody.Models.MealGenerator
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Generic;
using Foody.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foody.Models
{
  public class MealGenerator
  {
    private List<MealData> MealsDataFilter = new List<MealData>();
    private List<IndexCollection> generatedMeals = new List<IndexCollection>();
    private List<IndexCollection> UnselectedMeals = new List<IndexCollection>();
    private GroceriesCollection Groceries = new GroceriesCollection();
    private IndexCollection selectedRecipes = new IndexCollection();
    private bool IsFirstTimeGeneration = true;

    public void ChangeGeneratedRecipesFor(int row, int[] newRecipes)
    {
      this.generatedMeals[row].Clear();
      this.generatedMeals[row].AddRange(newRecipes, true);
      int[] values = this.selectedRecipes.Values;
      values[row] = this.generatedMeals[row].Get(0);
      this.selectedRecipes.Values = values;
      this.UpdateSelectedRecipes();
      this.Groceries = this.generateGroceries();
    }

    public void RemoveAt(int idx)
    {
      if (this.MealsDataFilter.Count < 0 || this.MealsDataFilter.Count <= idx)
        return;
      this.MealsDataFilter.RemoveAt(idx);
      this.generatedMeals.RemoveAt(idx);
      this.selectedRecipes.RemoveAt(idx);
      this.Groceries = this.generateGroceries();
    }

    public string[] Names
    {
      get
      {
        List<string> names = new List<string>();
        new List<int>((IEnumerable<int>) this.selectedRecipes.Values).ForEach((Action<int>) (x => names.Add(Database.AllMenus[x].title)));
        return names.ToArray();
      }
    }

    public void Add(MealData meal)
    {
      if (this.MealsDataFilter.Contains(meal))
        return;
      this.MealsDataFilter.Add(meal);
    }

    public void Add(int meal)
    {
      this.MealsDataFilter.Add(new MealData());
      this.selectedRecipes.Enqueue(meal);
      this.UnselectedMeals.Add(new IndexCollection());
      IndexCollection indexCollection = new IndexCollection();
      indexCollection.Enqueue(meal);
      this.generatedMeals.Add(indexCollection);
      this.UpdateSelectedRecipes();
      this.Groceries = this.generateGroceries();
    }

    public void Remove(MealData meal)
    {
      if (!this.MealsDataFilter.Contains(meal))
        return;
      this.MealsDataFilter.Remove(meal);
    }

    public void RemoveTagFrom(int idx, Tag tag)
    {
      if (this.MealsDataFilter.Count <= idx)
        return;
      this.MealsDataFilter[idx].Remove(tag);
    }

    public void AddTagTo(int idx, Tag tag)
    {
      if (this.MealsDataFilter.Count <= idx)
        return;
      this.MealsDataFilter[idx].Add(tag);
    }

    public void SetContentsByMeal(
      string contentUid,
      string Tag,
      string RecipeUid,
      double Quantity)
    {
      this.Groceries.SetContentsByMeal(contentUid, Tag, RecipeUid, Quantity);
    }

    public string GetContentsByMeal(string contentUid) => this.Groceries.GetContentsByMeal(contentUid);

    public void SetQuantity(int idx, double quantity)
    {
      if (this.MealsDataFilter.Count <= idx)
        return;
      this.MealsDataFilter[idx].Quantity = quantity;
    }

    public void SetNumberOfMealsForContentsByRecipe(int idx)
    {
      int[] values = this.selectedRecipes.Values;
      RecipeContent[] contents = Database.AllMenus[values[idx]].Contents.ToArray();
      for (int i = 0; i < contents.Length; ++i)
      {
        double quantity1 = 0.0;
        this.Groceries.SetQuantity(contents[i].uid, quantity1);
        for (int index = 0; index < values.Length; ++index)
        {
          if (values[index] != values[idx] && Database.AllMenus[values[index]].Contents.Any<RecipeContent>((Func<RecipeContent, bool>) (u => u.uid == contents[i].uid)))
          {
            double quantity2 = Database.AllMenus[values[index]].Contents.First<RecipeContent>((Func<RecipeContent, bool>) (u => u.uid == contents[i].uid)).Quantity;
            quantity1 += quantity2 * this.MealsDataFilter[index].Quantity;
          }
        }
        double quantity3 = quantity1 + contents[i].Quantity * this.MealsDataFilter[idx].Quantity;
        this.Groceries.SetQuantity(contents[i].uid, quantity3);
      }
    }

    public double GetQuantity(int idx) => idx >= 0 && idx < this.MealsDataFilter.Count ? this.MealsDataFilter[idx].Quantity : double.NaN;

    public Tag[] GetTags(int idx) => this.MealsDataFilter.Count > idx ? this.MealsDataFilter[idx].Tags : (Tag[]) null;

    public int Length => this.MealsDataFilter.Count;

    public Dictionary<ContentTag, List<RecipeContent>> SortedGroceriesValues => this.Groceries.SortedGroceriesValues;

    public RecipeContent[] Contents => this.Groceries.RecipesContents;

    private void ShuffleAll(bool shuffle = false)
    {
      if (!shuffle)
        return;
      for (int index = 0; index < this.generatedMeals.Count; ++index)
        this.generatedMeals[index].Shuffle();
    }

    private void UpdateSelectedRecipes()
    {
      this.selectedRecipes.Clear();
      for (int index = 0; index < this.generatedMeals.Count; ++index)
        this.selectedRecipes.Push(this.generatedMeals[index].Get(0));
    }

    public string GetName(int idx)
    {
      if (idx < 0 || idx >= this.generatedMeals.Count)
        return string.Empty;
      int[] values = this.selectedRecipes.Values;
      return values[idx] < 0 ? string.Empty : Database.AllMenus[values[idx]].title;
    }

    public string GetRecipeUid(int idx)
    {
      if (idx < 0 || idx >= this.generatedMeals.Count)
        return string.Empty;
      int[] values = this.selectedRecipes.Values;
      return values[idx] < 0 ? string.Empty : Database.AllMenus[values[idx]].uid;
    }

    private GroceriesCollection generateGroceries()
    {
      this.UpdateSelectedRecipes();
      GroceriesCollection groceries = new GroceriesCollection();
      int[] values = this.selectedRecipes.Values;
      for (int index1 = 0; index1 < values.Length; ++index1)
      {
        int index2 = values[index1];
        if (index2 >= 0 && index2 < Database.AllMenus.Count)
        {
          RecipeContent[] contents = (RecipeContent[]) Database.AllMenus[index2].GetContents().ToArray().Clone();
          if (this.MealsDataFilter[index1].Quantity > 0.0)
          {
            foreach (RecipeContent recipeContent in contents)
              groceries.Add(recipeContent.Clone());
          }
          else
            groceries.AddRange(contents);
        }
      }
      return groceries;
    }

    private void GenerateMeals()
    {
      this.generatedMeals.Clear();
      if (this.IsFirstTimeGeneration)
      {
        for (int index = 0; index < this.MealsDataFilter.Count; ++index)
          this.UnselectedMeals.Add(new IndexCollection());
      }
      for (int index = 0; index < this.MealsDataFilter.Count; ++index)
      {
        IndexCollection recipesMatchingTags = Tools.FindAllRecipesMatchingTags(this.MealsDataFilter[index].Tags);
        recipesMatchingTags.RemoveRange(this.UnselectedMeals[index].Values);
        this.generatedMeals.Add(recipesMatchingTags);
      }
    }

    public void MealsGeneratorReinit(bool all)
    {
      if (all)
      {
        this.IsFirstTimeGeneration = true;
        this.MealsDataFilter.Clear();
        this.UnselectedMeals.Clear();
      }
      this.generatedMeals.Clear();
      this.Groceries.Clear();
    }

    public void ChangeMeal(int idx)
    {
      if (idx < 0 || idx >= this.generatedMeals.Count)
        return;
      bool flag = this.generatedMeals[idx].Length == 1;
      this.UnselectedMeals[idx].Push(this.generatedMeals[idx].Get(0));
      this.generatedMeals[idx].RemoveAt(0);
      if (flag)
        this.generatedMeals[idx].AddRange(this.UnselectedMeals[idx].Values, true);
      this.UpdateSelectedRecipes();
      this.Groceries = this.generateGroceries();
    }

    public Content[] GetContentsForThisRecipe(int idx)
    {
      if (idx < 0 || idx >= this.MealsDataFilter.Count)
        return (Content[]) null;
      int index = this.selectedRecipes.Get(idx);
      return this.selectedRecipes.Length <= 0 || index < 0 || index >= Database.AllMenus.Count ? (Content[]) null : (Content[]) Database.AllMenus[index].GetContents().Select<RecipeContent, Content>((Func<RecipeContent, Content>) (x => Tools.FindContentByUid(x.uid))).ToArray<Content>().Clone();
    }

    public int GetRecipeIndex(int idx)
    {
        if (idx < 0 || idx >= this.MealsDataFilter.Count)
            return -1;

        return this.selectedRecipes.Get(idx);
    }

    public double GetQuantityInStock(string uid) => this.Groceries.GetQuantityInStock(uid);

    public void SetQuantityInStock(string uid, double quantity) => this.Groceries.SetQuantityInStock(uid, quantity);

    public double GetQuantityInThisRecipe(int idx, string uid)
    {
      if (idx >= 0 && idx < this.MealsDataFilter.Count)
      {
        RecipeContent recipeContent = Database.AllMenus[this.selectedRecipes.Get(idx)].GetContents().First<RecipeContent>((Func<RecipeContent, bool>) (x => x.uid == uid));
        if (recipeContent != null)
          return recipeContent.Quantity;
      }
      return double.NaN;
    }

    public void Generate()
    {
      this.MealsGeneratorReinit(false);
      this.GenerateMeals();
      this.ShuffleAll(true);
      this.Groceries = this.generateGroceries();
      this.IsFirstTimeGeneration = false;
    }
  }
}
