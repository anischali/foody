// Decompiled with JetBrains decompiler
// Type: Foody.Generic.ContentComparer
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Models;
using System.Collections.Generic;

namespace Foody.Generic
{
  internal class ContentComparer : IComparer<Content>
  {
    public int Compare(Content x, Content y) => x.Name == null || y.Name == null ? 0 : x.Name.CompareTo(y.Name);
  }
}
