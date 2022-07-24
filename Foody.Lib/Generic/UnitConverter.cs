// Decompiled with JetBrains decompiler
// Type: Foody.Generic.UnitConverter
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using System.Collections.Generic;

namespace Foody.Generic
{
  public class UnitConverter
  {
    private readonly Dictionary<Unit, string> UnitsToStrings = new Dictionary<Unit, string>();
    private readonly Dictionary<string, Unit> StringsToUnits = new Dictionary<string, Unit>();

    public UnitConverter() => this.Generate();

    private void Generate()
    {
      for (int index = 0; index < Consts.units.Length; ++index)
      {
        Unit key = (Unit) index;
        this.UnitsToStrings.Add(key, Consts.units[index]);
        this.StringsToUnits.Add(Consts.units[index], key);
      }
    }

    public Unit GetFromString(string unit) => this.StringsToUnits.ContainsKey(unit) ? this.StringsToUnits[unit] : Unit.Undefined;

    public string GetString(Unit unit) => this.UnitsToStrings.ContainsKey(unit) ? this.UnitsToStrings[unit] : "Inconnue";
  }
}
