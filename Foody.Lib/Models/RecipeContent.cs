// Decompiled with JetBrains decompiler
// Type: Foody.Models.RecipeContent
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Generic;
using System.Runtime.Serialization;

namespace Foody.Models
{
  [DataContract]
  public class RecipeContent
  {
    [DataMember]
    public string uid { get; set; }

    [DataMember]
    public double Quantity { get; set; }

    [DataMember]
    public Unit QuantityUnit { get; set; }

    public RecipeContent()
    {
      this.uid = "Undefined";
      this.Quantity = -1.0;
      this.QuantityUnit = Unit.Undefined;
    }

    public RecipeContent Clone() => new RecipeContent()
    {
      Quantity = this.Quantity,
      QuantityUnit = this.QuantityUnit,
      uid = this.uid
    };
  }
}
