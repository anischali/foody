// Decompiled with JetBrains decompiler
// Type: Foody.Generic.TagConverter
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using System.Collections.Generic;

namespace Foody.Generic
{
  public class TagConverter
  {
    private readonly Dictionary<Tag, string> TagsToStrings = new Dictionary<Tag, string>();
    private readonly Dictionary<string, Tag> StringsToTags = new Dictionary<string, Tag>();

    public TagConverter() => this.Generate();

    private void Generate()
    {
      for (int index = 0; index < Consts.tags.Length; ++index)
      {
        Tag key = (Tag) index;
        this.TagsToStrings.Add(key, Consts.tags[index]);
        this.StringsToTags.Add(Consts.tags[index], key);
      }
    }

    public Tag GetFromString(string tag) => this.StringsToTags.ContainsKey(tag) ? this.StringsToTags[tag] : Tag.Undefined;

    public string GetString(Tag tag) => this.TagsToStrings.ContainsKey(tag) ? this.TagsToStrings[tag] : "Inconnue";
  }
}
