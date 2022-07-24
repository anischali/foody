// Decompiled with JetBrains decompiler
// Type: Foody.Models.MealData
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Generic;
using System.Collections.Generic;

namespace Foody.Models
{
  public class MealData
  {
    private List<Tag> tags = new List<Tag>();
    private double quantity;
    private bool isEdited = true;

    public MealData()
    {
      this.quantity = 1.0;
      this.isEdited = true;
    }

    public bool IsEdited
    {
      get => this.isEdited;
      set => this.isEdited = value;
    }

    public double Quantity
    {
      get => this.quantity;
      set
      {
        if (double.IsNaN(value))
          return;
        this.quantity = value;
      }
    }

    public void Add(Tag t)
    {
      if (this.tags.Contains(t))
        return;
      this.tags.Add(t);
    }

    public void Remove(Tag t)
    {
      if (!this.tags.Contains(t))
        return;
      this.tags.Remove(t);
    }

    public Tag[] Tags
    {
      get => this.tags.ToArray();
      set
      {
        if (value == null)
          return;
        this.tags = new List<Tag>((IEnumerable<Tag>) value);
      }
    }
  }
}
