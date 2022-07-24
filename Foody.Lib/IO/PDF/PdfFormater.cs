// Decompiled with JetBrains decompiler
// Type: Foody.IO.PDF.PdfFormater
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace Foody.IO.PDF
{
  public static class PdfFormater
  {
    private static Color simple = Color.FromRgb((byte) 51, (byte) 149, (byte) 214);
    private static Color other = Color.FromRgb((byte) 37, (byte) 37, (byte) 38);
    private static Color text = Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue);
    private static Color SentenceText = Color.FromRgb((byte) 0, (byte) 0, (byte) 0);
    private static Color headerText = Color.FromRgb((byte) 229, (byte) 9, (byte) 20);

    public static void AddColumn(Table table, Unit u)
    {
      Column column = table.AddColumn(u);
      column.Format.Alignment = ParagraphAlignment.Center;
      column.Format.Borders.Color = Color.Empty;
      column.Format.Shading.Color = PdfFormater.simple;
      column.Format.Font.Color = PdfFormater.text;
    }

    public static void AddHeaderRow(Table table, string[] values)
    {
      Row row = table.AddRow();
      row.Format.Shading.Color = PdfFormater.other;
      for (int index = 0; index < values.Length; ++index)
        row.Cells[index].AddParagraph(values[index]);
    }

    public static void AddTitle(Section section, string title, Unit size)
    {
      Paragraph paragraph = new Paragraph();
      paragraph.AddText(title);
      paragraph.Format.Font.Size = size;
      paragraph.Format.Font.Color = PdfFormater.headerText;
      paragraph.Format.Alignment = ParagraphAlignment.Left;
      section.Add(paragraph);
    }

    public static void AddSentens(Section section, string text, Unit size)
    {
      Paragraph paragraph = new Paragraph();
      paragraph.AddText(text);
      paragraph.Format.Font.Size = size;
      paragraph.Format.Font.Color = PdfFormater.SentenceText;
      paragraph.Format.Alignment = ParagraphAlignment.Left;
      section.Add(paragraph);
    }

    public static void AddRow(Table table, string[] values, bool simpleCell)
    {
      Row row = table.AddRow();
      row.Format.Shading.Color = simpleCell ? PdfFormater.simple : PdfFormater.other;
      Paragraph paragraph = new Paragraph();
      paragraph.AddFormattedText("¨", new Font("Wingdings"));
      row.Cells[0].Add(paragraph);
      for (int index = 0; index < values.Length; ++index)
        row.Cells[index + 1].AddParagraph(values[index]);
    }
  }
}
