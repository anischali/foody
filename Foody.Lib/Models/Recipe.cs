// Decompiled with JetBrains decompiler
// Type: Foody.Models.Recipe
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Generic;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Foody.Models
{
  [DataContract]
  public class Recipe : ICloneable
  {
    [DataMember]
    public string title;

    [DataMember]
    public ulong count { get; set; }

    [DataMember]
    public string uid { get; private set; }

    [DataMember]
    public List<Tag> Tags { get; set; }

    [DataMember]
    public List<RecipeContent> Contents { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public int PeopleNumber { get; set; }

    public Recipe()
    {
      this.uid = Guid.NewGuid().ToString();
      this.Tags = new List<Tag>();
      this.Contents = new List<RecipeContent>();
      this.Description = "";
      this.PeopleNumber = -1;
      this.title = "";
      this.count = 0UL;
    }

    public List<RecipeContent> GetContents()
    {
      List<RecipeContent> contents = new List<RecipeContent>();
      foreach (RecipeContent content in this.Contents)
        contents.Add(content.Clone());
      return contents;
    }

    public object Clone()
    {
      Recipe recipe = new Recipe();
      recipe.title = (string) this.title.Clone();
      recipe.Contents.AddRange((IEnumerable<RecipeContent>) this.Contents.ToArray());
      recipe.Tags.AddRange((IEnumerable<Tag>) this.Tags.ToArray());
      recipe.Description = (string) this.Description.Clone();
      recipe.PeopleNumber = this.PeopleNumber;
      return (object) recipe;
    }
  }
}
