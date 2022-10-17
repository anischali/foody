// Decompiled with JetBrains decompiler
// Type: Foody.IO.Database
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Generic;
using Foody.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Foody.IO
{
  public static class Database
  {
    private const string contents_fileName = "contents.json";
    private const string recipe_fileName = "recipes.json";
    private const string history_fileName = "history.json";
    private static RecipeComparer recipeComparer = new RecipeComparer();
    private static string path = Environment.GetEnvironmentVariable("APPDATA") + "\\FoodGenerator\\Databases\\";
    public static List<Content> contents;
    public static List<Recipe> AllMenus;
    public static List<HistoryRecipe> HistoryMenus;

    public static void RemoveContentAtIndexFromDatabase(int idx)
    {
      Database.contents.RemoveAt(idx);
      ContentComparer contentComparer = new ContentComparer();
      Database.contents.Sort((IComparer<Content>) contentComparer);
    }

    public static void AddContentToDatabase(Content content)
    {
      Database.contents.Add(content);
      Tools.SaveToJSON<List<Content>>(Database.contents, Database.path, "contents.json");
      ContentComparer contentComparer = new ContentComparer();
      Database.contents.Sort((IComparer<Content>) contentComparer);
    }

    public static bool ValidateRecipeExistance(int idx) => idx >= 0 && idx < Database.AllMenus.Count;

    public static void AddRecipeToDatabase(Recipe recipe, bool IsUpdate = false)
    {
      if (IsUpdate)
      {
        Database.UpdateRecipe(recipe);
      }
      else
      {
        Database.AllMenus.Add(recipe);
        Database.AllMenus.Sort((IComparer<Recipe>) Database.recipeComparer);
        Tools.SaveToJSON<List<Recipe>>(Database.AllMenus, Database.path, "recipes.json");
      }
    }

    public static void RemoveRecipeFromDatabase(int index)
    {
      Database.AllMenus.RemoveAt(index);
      Database.AllMenus.Sort((IComparer<Recipe>) Database.recipeComparer);
      Tools.SaveToJSON<List<Recipe>>(Database.AllMenus, Database.path, "recipes.json");
    }

    public static void RemoveRecipeFromDatabase(Recipe recipe)
    {
      Recipe recipe1 = Database.AllMenus.FirstOrDefault<Recipe>((Func<Recipe, bool>) (x => x.uid == recipe.uid));
      if (recipe1 == null)
        return;
      int index = Database.AllMenus.IndexOf(recipe1);
      Database.AllMenus.RemoveAt(index);
      Database.AllMenus.Sort((IComparer<Recipe>) Database.recipeComparer);
      Tools.SaveToJSON<List<Recipe>>(Database.AllMenus, Database.path, "recipes.json");
    }

    public static void AddRecipeListToHistoryDatabase(HistoryRecipe historyRecipe)
    {
      Database.HistoryMenus.Add(historyRecipe);
      Tools.SaveToJSON<List<HistoryRecipe>>(Database.HistoryMenus, Database.path, "history.json");
    }

    public static void SaveAllDatabases()
    {
      Tools.SaveToJSON<List<Recipe>>(Database.AllMenus, Database.path, "recipes.json");
      Tools.SaveToJSON<List<HistoryRecipe>>(Database.HistoryMenus, Database.path, "history.json");
      Tools.SaveToJSON<List<Content>>(Database.contents, Database.path, "contents.json");
    }

    private static void test()
    {
      foreach (Content content in Database.contents)
      {
        if (content.uid == "0")
          content.uid = Guid.NewGuid().ToString();
      }
    }

    public static void LoadAllDatabases()
    {
      Database.contents = Tools.LoadFromJSON<List<Content>>(Path.Combine(Database.path, "contents.json"));
      if (Database.contents == null)
        Database.contents = new List<Content>();
      if (Database.contents.Count > 2)
      {
        Database.test();
        ContentComparer contentComparer = new ContentComparer();
        Database.contents.Sort((IComparer<Content>) contentComparer);
      }
      Database.AllMenus = Tools.LoadFromJSON<List<Recipe>>(Path.Combine(Database.path, "recipes.json"));
      if (Database.AllMenus == null)
        Database.AllMenus = new List<Recipe>();
      if (Database.AllMenus.Count > 1)
        Database.AllMenus.Sort((IComparer<Recipe>) Database.recipeComparer);
      Database.HistoryMenus = Tools.LoadFromJSON<List<HistoryRecipe>>(Path.Combine(Database.path, "history.json"));
      if (Database.HistoryMenus != null)
        return;
      Database.HistoryMenus = new List<HistoryRecipe>();
    }

    public static void UpdateRecipe(Recipe recipe)
    {
      foreach (Recipe allMenu in Database.AllMenus)
      {
        if (recipe.uid.Equals(allMenu.uid))
        {
          allMenu.title = recipe.title;
          allMenu.Tags = recipe.Tags;
          allMenu.PeopleNumber = recipe.PeopleNumber;
          allMenu.Description = recipe.Description;
          allMenu.Contents = recipe.Contents;
          break;
        }
      }
      Tools.SaveToJSON<List<Recipe>>(Database.AllMenus, Database.path, "recipes.json");
    }
  }
}
