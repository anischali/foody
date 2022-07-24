// Decompiled with JetBrains decompiler
// Type: Foody.Generic.Tools
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.IO;
using Foody.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Foody.Generic
{
  public static class Tools
  {
    public static void Shuffle<T>(this IList<T> list, int seed = 0)
    {
      Random random = new Random(seed);
      int count = list.Count;
      while (count > 1)
      {
        --count;
        int index = random.Next(count + 1);
        T obj = list[index];
        list[index] = list[count];
        list[count] = obj;
      }
    }

    public static string StringBonify(string old) => new Regex("[\\/:\"*?<>|]+").Replace(old, "");

    public static double ParseDouble(string number)
    {
      CultureInfo cultureInfo = new CultureInfo("fr");
      if (!new Regex("^[-+]?[0-9]*\\.?\\,?[0-9]+([eE][-+]?[0-9]+)?\\z").IsMatch(number))
        return double.NaN;
      Regex regex = new Regex("\\.");
      if (!regex.IsMatch(number) || !CultureInfo.CurrentCulture.Equals((object) cultureInfo))
        return double.Parse(number);
      number = regex.Replace(number, ",");
      return double.Parse(number);
    }

    public static int ParseInt(string number) => (int) Tools.ParseDouble(number);

    public static Type LoadFromJSON<Type>(string file_Name) => !File.Exists(file_Name) ? default (Type) : JsonConvert.DeserializeObject<Type>(File.ReadAllText(file_Name));

    public static void SaveToJSON<Type>(Type data, string path, string fileName)
    {
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      string contents = JsonConvert.SerializeObject((object) data);
      File.WriteAllText(Path.Combine(path, fileName), contents);
    }

    public static string ConvertToJSON<Type>(Type data) => JsonConvert.SerializeObject((object) data);

    public static string ContentToStringConverter(Content content) => content.Name;

    public static string FindContentNameByUid(string uid)
    {
      foreach (Content content in Database.contents)
      {
        if (content.uid == uid)
          return content.Name;
      }
      return "Unknown";
    }

    public static Content FindContentByName(string name)
    {
      foreach (Content content in Database.contents)
      {
        if (content.Name == name)
          return content;
      }
      return (Content) null;
    }

    public static Content FindContentByUid(string uid)
    {
      string contentNameByUid = Tools.FindContentNameByUid(uid);
      return contentNameByUid.Equals("Unknown") ? (Content) null : Tools.FindContentByName(contentNameByUid);
    }

    public static IndexCollection FindAllRecipesMatchingTags(
      Tag[] tags,
      bool insclusion = true)
    {
      IndexCollection recipesMatchingTags = new IndexCollection();
      int[] array = Database.AllMenus.Where<Recipe>((Func<Recipe, bool>) (m => !insclusion ? Tools.IsRecipeContainsAnyTags(m, new List<Tag>((IEnumerable<Tag>) tags)) : Tools.IsRecipeContainsAllTags(m, new List<Tag>((IEnumerable<Tag>) tags)))).Select<Recipe, int>((Func<Recipe, int>) (r => Database.AllMenus.IndexOf(r))).ToArray<int>();
      recipesMatchingTags.AddRange(array);
      return recipesMatchingTags;
    }

    public static int[] FindAllRecipesMatchingTags(
      int[] recipesToInclude,
      Tag[] tags,
      bool insclusion = true)
    {
      return Database.AllMenus.Where<Recipe>((Func<Recipe, bool>) (m => (insclusion ? (Tools.IsRecipeContainsAllTags(m, new List<Tag>((IEnumerable<Tag>) tags)) ? 1 : 0) : (Tools.IsRecipeContainsAnyTags(m, new List<Tag>((IEnumerable<Tag>) tags)) ? 1 : 0)) != 0 && ((IEnumerable<int>) recipesToInclude).Contains<int>(Database.AllMenus.IndexOf(m)))).Select<Recipe, int>((Func<Recipe, int>) (r => Database.AllMenus.IndexOf(r))).ToArray<int>();
    }

    public static int[] FindAllRecipesMatchingContents(
      int[] recipesToInclude,
      int[] ingredients,
      bool insclusion = true)
    {
      return Database.AllMenus.Where<Recipe>((Func<Recipe, bool>) (m => Tools.IsRecipeContainsContents(m, ingredients, insclusion) && ((IEnumerable<int>) recipesToInclude).Contains<int>(Database.AllMenus.IndexOf(m)))).Select<Recipe, int>((Func<Recipe, int>) (r => Database.AllMenus.IndexOf(r))).ToArray<int>();
    }

    public static int FindContentIndexByUid(string uid)
    {
      Content contentByUid = Tools.FindContentByUid(uid);
      return contentByUid == null ? -1 : Database.contents.IndexOf(contentByUid);
    }

    public static ContentTag GetContentTagByUid(string uid)
    {
      Content contentByUid = Tools.FindContentByUid(uid);
      return contentByUid != null ? contentByUid.type : ContentTag.Undefined;
    }

    public static RecipeContent FindRecipeContentByName(
      List<RecipeContent> contents,
      string name)
    {
      Content contentByName = Tools.FindContentByName(name);
      if (contentByName == null)
        return (RecipeContent) null;
      foreach (RecipeContent content in contents)
      {
        if (content.uid == contentByName.uid)
          return content;
      }
      return (RecipeContent) null;
    }

    public static Recipe FindRecipeByTitle(string title)
    {
      foreach (Recipe allMenu in Database.AllMenus)
      {
        if (allMenu.title == title)
          return allMenu;
      }
      return (Recipe) null;
    }

    public static List<string> FindAllMatchTitleRecipes(string title)
    {
      List<string> matchTitleRecipes = new List<string>();
      foreach (Recipe allMenu in Database.AllMenus)
      {
        if (allMenu.title.ToLower().Contains(title))
          matchTitleRecipes.Add(allMenu.title);
      }
      return matchTitleRecipes;
    }

    private static List<int> AllrecipesIndexs()
    {
      List<int> intList = new List<int>();
      for (int index = 0; index < Database.AllMenus.Count; ++index)
        intList.Add(index);
      return intList;
    }

    public static List<int> FindAllMatchTitleRecipesIndexs(string title)
    {
      if (string.IsNullOrEmpty(title))
        return Tools.AllrecipesIndexs();
      title = title.ToLowerInvariant();
      List<int> titleRecipesIndexs = new List<int>();
      for (int index = 0; index < Database.AllMenus.Count; ++index)
      {
        if (Database.AllMenus[index].title.ToLowerInvariant().Contains(title))
          titleRecipesIndexs.Add(index);
      }
      return titleRecipesIndexs;
    }

    public static bool IsRecipeContainsContents(Recipe recipe, int[] contents, bool inclusion)
    {
      int num = 0;
      foreach (int content1 in contents)
      {
        foreach (RecipeContent content2 in recipe.Contents)
        {
          if (content2.uid == Database.contents[content1].uid)
            ++num;
        }
      }
      return num > 0 & inclusion || num == contents.Length && !inclusion;
    }

    public static bool IsRecipeContainsAnyTags(Recipe recipe, List<Tag> tags)
    {
      foreach (Tag tag1 in tags)
      {
        foreach (Tag tag2 in recipe.Tags)
        {
          if (tag2 == tag1)
            return true;
        }
      }
      return false;
    }

    public static bool IsRecipeContainsAllTags(Recipe recipe, List<Tag> tags)
    {
      int num = 0;
      foreach (Tag tag1 in tags)
      {
        foreach (Tag tag2 in recipe.Tags)
        {
          if (tag2 == tag1)
            ++num;
        }
      }
      return num == tags.Count;
    }
  }
}
