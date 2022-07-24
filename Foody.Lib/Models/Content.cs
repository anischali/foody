// Decompiled with JetBrains decompiler
// Type: Foody.Models.Content
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Generic;
using System;
using System.Runtime.Serialization;

namespace Foody.Models
{
  [DataContract]
  public class Content
  {
    [DataMember]
    public string uid { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public ContentTag type { get; set; }

    public Content(string name, ContentTag tag, double quantity, Unit unit)
    {
      this.uid = Guid.NewGuid().ToString();
      this.Name = name;
      this.type = tag;
    }

    public Content()
    {
      this.uid = Guid.NewGuid().ToString();
      this.Name = "Undefined";
      this.type = ContentTag.Undefined;
    }
  }
}
