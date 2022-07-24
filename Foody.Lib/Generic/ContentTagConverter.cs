// Decompiled with JetBrains decompiler
// Type: Foody.Generic.ContentTagConverter
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using System.Collections.Generic;

namespace Foody.Generic
{
  internal class ContentTagConverter
  {
    private readonly Dictionary<ContentTag, string> TagsToStrings = new Dictionary<ContentTag, string>();
    private readonly Dictionary<string, ContentTag> StringsToTags = new Dictionary<string, ContentTag>();

    public ContentTagConverter() => this.Generate();

    private void Generate()
    {
      for (int index = 0; index < Consts.contentTags.Length; ++index)
      {
        ContentTag key = (ContentTag) index;
        this.TagsToStrings.Add(key, Consts.contentTags[index]);
        this.StringsToTags.Add(Consts.contentTags[index], key);
      }
    }

    public ContentTag GetFromString(string tag) => this.StringsToTags.ContainsKey(tag) ? this.StringsToTags[tag] : ContentTag.Undefined;

    public string GetString(ContentTag tag) => this.TagsToStrings.ContainsKey(tag) ? this.TagsToStrings[tag] : "Inconnue";
  }
}
