// Decompiled with JetBrains decompiler
// Type: Foody.Generic.IndexCollection
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using System;
using System.Collections.Generic;

namespace Foody.Generic
{
  public class IndexCollection
  {
    private List<int> Indexs;
    private readonly object sync = new object();

    public IndexCollection() => this.Indexs = new List<int>();

    public int Get(int idx) => idx >= 0 && idx < this.Indexs.Count ? this.Indexs[idx] : -1;

    public void Push(int value) => this.Indexs.Add(value);

    public int Pop()
    {
      lock (this.sync)
      {
        int num = -1;
        if (this.Indexs.Count > 0)
        {
          int index = this.Indexs.Count - 1;
          num = this.Indexs[index];
          this.Indexs.RemoveAt(index);
        }
        return num;
      }
    }

    public void Enqueue(int value) => this.Push(value);

    public int Dequeue()
    {
      int num = -1;
      if (this.Indexs.Count > 0)
      {
        num = this.Indexs[0];
        this.Indexs.RemoveAt(0);
      }
      return num;
    }

    public void Remove(int value)
    {
      if (!this.Indexs.Contains(value))
        return;
      this.Indexs.Remove(value);
    }

    public int Pop(int value)
    {
      if (!this.Indexs.Contains(value))
        return -1;
      this.Indexs.Remove(value);
      return value;
    }

    public void AddRange(int[] array, bool unique = false)
    {
      if (array == null)
        return;
      foreach (int num in array)
      {
        if (!unique)
          this.Push(num);
        else
          this.PushUnique(num);
      }
    }

    public void RemoveRange(int[] array)
    {
      if (array == null)
        return;
      foreach (int num in array)
        this.Remove(num);
    }

    public void Shuffle() => this.Indexs.Shuffle<int>((int) DateTime.Now.Ticks);

    public int[] Values
    {
      get => this.Indexs.ToArray();
      set
      {
        if (value == null || value.Length == 0)
          return;
        this.Indexs.Clear();
        this.AddRange(value);
      }
    }

    public int PopAt(int idx)
    {
      int num = -1;
      if (idx >= 0 && idx < this.Indexs.Count)
      {
        num = this.Indexs[idx];
        this.Indexs.RemoveAt(idx);
      }
      return num;
    }

    public void RemoveAt(int idx)
    {
      if (idx < 0 || idx >= this.Indexs.Count)
        return;
      this.Indexs.RemoveAt(idx);
    }

    public int Length => this.Indexs.Count;

    public void PushUnique(int value)
    {
      lock (this.sync)
      {
        if (this.Indexs.Contains(value))
          return;
        this.Push(value);
      }
    }

    public bool Contains(int value) => this.Indexs.Contains(value);

    public void Clear()
    {
      this.Indexs.Clear();
      this.Indexs = new List<int>();
    }
  }
}
