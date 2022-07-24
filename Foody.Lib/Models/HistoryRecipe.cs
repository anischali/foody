// Decompiled with JetBrains decompiler
// Type: Foody.Models.HistoryRecipe
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Foody.Models
{
  [DataContract]
  public class HistoryRecipe
  {
    [DataMember]
    public List<Recipe> GroceriesList { get; set; }

    [DataMember]
    public DateTime date { get; set; }

    [DataMember]
    public string uid { get; private set; }

    public HistoryRecipe()
    {
      this.uid = Guid.NewGuid().ToString();
      this.date = DateTime.UtcNow;
      this.GroceriesList = new List<Recipe>();
    }
  }
}
