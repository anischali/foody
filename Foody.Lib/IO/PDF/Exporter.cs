// Decompiled with JetBrains decompiler
// Type: Foody.IO.PDF.Exporter
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Generic;
using Foody.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace Foody.IO.PDF
{
  public class Exporter
  {
    private PdfDocumentRenderer rendrer;
    private Document document;
    private readonly UnitConverter Converter = new UnitConverter();
    private MealGenerator generator;

    public Exporter()
    {
      this.document = new Document();
      this.rendrer = new PdfDocumentRenderer(true);
      this.rendrer.Document = this.document;
    }

    public Table ConvertToDataTable(RecipeContent[] contents)
    {
      Table table = new Table();
      table.Format.Borders.Style = BorderStyle.Single;
      PdfFormater.AddColumn(table, MigraDoc.DocumentObjectModel.Unit.FromCentimeter(0.5));
      PdfFormater.AddColumn(table, MigraDoc.DocumentObjectModel.Unit.FromCentimeter(5.0));
      PdfFormater.AddColumn(table, MigraDoc.DocumentObjectModel.Unit.FromCentimeter(5.0));
      PdfFormater.AddColumn(table, MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2.5));
      PdfFormater.AddColumn(table, MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2.5));
      int num = 0;
      for (int index = 0; index < contents.Length; ++index)
      {
        string contentsByMeal = this.generator.GetContentsByMeal(contents[index].uid);
        string name = Tools.FindContentByUid(contents[index].uid).Name;
        if (contents[index].Quantity > 0.0)
        {
          string[] values = new string[4]
          {
            name,
            contentsByMeal,
            contents[index].Quantity.ToString(),
            this.Converter.GetString(contents[index].QuantityUnit)
          };
          PdfFormater.AddRow(table, values, num % 2 == 0);
          ++num;
        }
      }
      return table;
    }

    public void AddContents(Section section, RecipeContent[] contents, string header = "")
    {
      PdfFormater.AddTitle(section, header, MigraDoc.DocumentObjectModel.Unit.FromPoint(12.0));
      Table dataTable = this.ConvertToDataTable(contents);
      section.Add(dataTable);
    }

    public void GenerateGroceriesList(
      Section section,
      Dictionary<ContentTag, List<RecipeContent>> list)
    {
      PdfFormater.AddTitle(section, "Liste de courses: ", MigraDoc.DocumentObjectModel.Unit.FromPoint(13.0));
      ContentTagConverter contentTagConverter = new ContentTagConverter();
      foreach (ContentTag contentTag in list.Keys.ToArray<ContentTag>())
      {
        RecipeContent[] array = list[contentTag].ToArray();
        this.AddContents(section, array, contentTagConverter.GetString(contentTag));
      }
    }

    public void AddMealsTitles(Section section, string[] names)
    {
      PdfFormater.AddTitle(section, "Nom des recettes: ", MigraDoc.DocumentObjectModel.Unit.FromPoint(13.0));
      for (int idx = 0; idx < names.Length; ++idx)
        PdfFormater.AddSentens(section, string.Format("{0}-{1}({2})", (object) (idx + 1), (object) names[idx], (object) this.generator.GetQuantity(idx)), MigraDoc.DocumentObjectModel.Unit.FromPoint(10.0));
    }

    public void Generate(MealGenerator mealGenerator)
    {
      this.generator = mealGenerator;
      Section section = this.document.AddSection();
      this.AddMealsTitles(section, this.generator.Names);
      this.GenerateGroceriesList(section, this.generator.SortedGroceriesValues);
    }

    public void Save(string path)
    {
      this.rendrer.RenderDocument();
      //this.rendrer.PdfDocument.Save(path);
    }
  }
}
