// Decompiled with JetBrains decompiler
// Type: Foody.Primitives.CheckComboBoxItem
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

namespace Foody.Primitives
{
  public class CheckComboBoxItem
  {
    private string _text = "";
    private bool _checkState = false;
    private object _tag = (object) null;

    public CheckComboBoxItem(string text, bool initialCheckState, object tag)
    {
      this._checkState = initialCheckState;
      this._text = text;
      this._tag = tag;
    }

    public bool CheckState
    {
      get => this._checkState;
      set => this._checkState = value;
    }

    public string Text
    {
      get => this._text;
      set => this._text = value;
    }

    public object Tag
    {
      get => this._tag;
      set => this._tag = value;
    }

    public override string ToString() => "Select Options";
  }
}
