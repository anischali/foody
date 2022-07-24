// Decompiled with JetBrains decompiler
// Type: Foody.Controls.GeneratePanel
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using Foody.Generic;
using Foody.IO;
using Foody.Lib.Generic;
using Foody.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Foody.Controls
{
  public class GeneratePanel : UserControl
  {
    private readonly MealGenerator mealGenerator = new MealGenerator();
    private int mealsSelectedRow = 0;
    private int mealsNumber = 0;
    private ExporterPanel exporterPanel = new ExporterPanel();
    private SearchEngine searcher = new SearchEngine();
    private TagConverter converter = new TagConverter();
    private UnitConverter unitConverter = new UnitConverter();
    private IContainer components = (IContainer) null;
    private Panel panel1;
    private Panel panel3;
    private Panel panel4;
    private Button btnAddContents;
    private Label label1;
    private Panel footerPanel;
    private Button btnGenerate;
    private Button btnValidate;
    private Panel panel6;
    private BindingSource recipeBindDataBindingSource;
    private Panel panel7;
    private Panel panel2;
    private Panel TagsPanel;
    private FlowLayoutPanel SelectedTags;
    private DataGridView MealsDataGridView;
    private DataGridView contentsGridView;
    private Panel panel8;
    private FlowLayoutPanel NotSelectedTags;
    private Button btnAddRecipe;
    private DataGridViewTextBoxColumn Num;
    private DataGridViewTextBoxColumn Nom;
    private DataGridViewTextBoxColumn NbPersonne;
    private DataGridViewButtonColumn Tags;
    private DataGridViewButtonColumn Change;
    private DataGridViewButtonColumn Remove;
    private DataGridViewTextBoxColumn NumContent;
    private DataGridViewTextBoxColumn ContentName;
    private DataGridViewTextBoxColumn ContentQuantity;
    private DataGridViewTextBoxColumn EnStock;
    private DataGridViewTextBoxColumn Total;
    private DataGridViewTextBoxColumn Unit;
    private DataGridViewButtonColumn moins;
    private DataGridViewButtonColumn Plus;

    public GeneratePanel()
    {
      this.InitializeComponent();
      this.InitEvents();
      this.ManualInit();
    }

    private void InitEvents()
    {
      this.MealsDataGridView.CellClick += new DataGridViewCellEventHandler(this.MealsDataGridViewCell_Click);
      this.contentsGridView.CellClick += new DataGridViewCellEventHandler(this.ContentsDataGridViewCell_Click);
      this.btnValidate.Enabled = true;
      this.btnValidate.Click += (EventHandler) ((s, e) =>
      {
        this.mealGenerator.Generate();
        this.UpdateAllDataGrids();
        this.btnValidate.Enabled = false;
      });
      this.MealsDataGridView.CellEndEdit += (DataGridViewCellEventHandler) ((s, e) =>
      {
        if (e.ColumnIndex != 2 || e.RowIndex < 0 || e.RowIndex >= this.mealGenerator.Length)
          return;
        this.mealGenerator.SetQuantity(e.RowIndex, Tools.ParseDouble(this.MealsDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()));
        this.mealGenerator.SetNumberOfMealsForContentsByRecipe(e.RowIndex);
        this.UpdateContentsGrid();
      });
      this.contentsGridView.CellEndEdit += (DataGridViewCellEventHandler) ((s, e) =>
      {
        if (e.ColumnIndex != 3)
          return;
        RecipeContent[] contents = this.mealGenerator.Contents;
        if (e.RowIndex >= 0 && e.RowIndex < contents.Length && this.contentsGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
        {
          double d = Tools.ParseDouble(this.contentsGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
          double quantity = double.IsNaN(d) ? this.mealGenerator.GetQuantityInStock(contents[e.RowIndex].uid) : d;
          this.mealGenerator.SetQuantityInStock(contents[e.RowIndex].uid, quantity);
          this.UpdateContentsGrid();
        }
      });
      this.btnGenerate.Click += (EventHandler) ((s, e) =>
      {
        if (!this.footerPanel.Controls.Contains((Control) this.exporterPanel))
        {
          this.exporterPanel = new ExporterPanel();
          this.footerPanel.Controls.Add((Control) this.exporterPanel);
          this.exporterPanel.Dock = DockStyle.Left;
        }
        this.exporterPanel.Generator = this.mealGenerator;
        this.exporterPanel.Visible = true;
      });
      this.searcher.AsyncRefresh += (EventHandler) ((s, e) =>
      {
        if (this.MealsDataGridView.InvokeRequired)
          this.MealsDataGridView.Invoke(new Action(() => this.ChangeMealsAfterTagEdit()));
        else
          this.ChangeMealsAfterTagEdit();
      });
    }

    private void ChangeMealsAfterTagEdit()
    {
      this.mealGenerator.ChangeGeneratedRecipesFor(this.mealsSelectedRow, this.searcher.SearchResult);
      this.UpdateAllDataGrids();
    }

    private void ManualInit()
    {
      this.contentsGridView.MultiSelect = false;
      this.MealsDataGridView.MultiSelect = false;
      this.DoubleBuffered = true;
    }

    private string GetCorrespondingToolTip(int rowIndex)
    {
      Content[] contentsForThisRecipe = this.mealGenerator.GetContentsForThisRecipe(rowIndex);
      string correspondingToolTip = "";
      if (contentsForThisRecipe != null)
      {
        foreach (string str in ((IEnumerable<Content>) contentsForThisRecipe).Select<Content, string>((Func<Content, string>) (x => x.Name + Environment.NewLine)).ToArray<string>())
          correspondingToolTip += str;
      }
      return correspondingToolTip;
    }

    private string GetQuantitiesByRecipe(RecipeContent content)
    {
      for (int idx = 0; idx < this.mealsNumber; ++idx)
      {
        string recipeUid = this.mealGenerator.GetRecipeUid(idx);
        Content[] contentsForThisRecipe = this.mealGenerator.GetContentsForThisRecipe(idx);
        if (contentsForThisRecipe != null)
        {
          foreach (Content content1 in contentsForThisRecipe)
          {
            if (content1.uid == content.uid)
            {
              double quantity = this.mealGenerator.GetQuantity(idx);
              double Quantity = this.mealGenerator.GetQuantityInThisRecipe(idx, content1.uid) * (quantity > 0.0 ? quantity : 1.0);
              this.mealGenerator.SetContentsByMeal(content1.uid, (idx + 1).ToString(), recipeUid, Quantity);
            }
          }
        }
      }
      return this.mealGenerator.GetContentsByMeal(content.uid);
    }

    private void UpdateContentsGrid()
    {
      if (this.contentsGridView.InvokeRequired)
      {
        this.contentsGridView.Invoke((Delegate) new MethodInvoker(this.UpdateContentsGrid));
      }
      else
      {
        this.contentsGridView.SuspendLayout();
        RecipeContent[] contents = this.mealGenerator.Contents;
        bool flag = this.contentsGridView.Rows.Count != contents.Length;
        if (flag)
          this.contentsGridView.Rows.Clear();
        for (int index1 = 0; index1 < contents.Length; ++index1)
        {
          int index2 = flag ? this.contentsGridView.Rows.Add() : index1;
          this.contentsGridView.Rows[index2].Cells[0].Value = (object) (index1 + 1);
          this.contentsGridView.Rows[index2].Cells[1].Value = (object) Tools.FindContentByUid(contents[index1].uid).Name;
          this.contentsGridView.Rows[index2].Cells[2].Value = (object) this.GetQuantitiesByRecipe(contents[index1]);
          double quantityInStock = this.mealGenerator.GetQuantityInStock(contents[index1].uid);
          this.contentsGridView.Rows[index2].Cells[3].Value = (object) quantityInStock;
          this.contentsGridView.Rows[index2].Cells[4].Value = (object) (contents[index1].Quantity - quantityInStock);
          this.contentsGridView.Rows[index2].Cells[5].Value = (object) this.unitConverter.GetString(contents[index1].QuantityUnit);
          this.contentsGridView.Rows[index2].Cells[6].Value = (object) "➖";
          this.contentsGridView.Rows[index2].Cells[7].Value = (object) "➕";
        }
        this.contentsGridView.ResumeLayout();
      }
    }

    private void UpdateMealsGrid()
    {
      this.MealsDataGridView.Rows.Clear();
      for (int index1 = 0; index1 < this.mealsNumber; ++index1)
      {
        int index2 = this.MealsDataGridView.Rows.Add();
        this.MealsDataGridView.Rows[index2].Cells[0].Value = (object) (index1 + 1);
        this.MealsDataGridView.Rows[index2].Cells[1].Value = (object) this.mealGenerator.GetName(index1);
        this.MealsDataGridView.Rows[index2].Cells[2].Value = (object) this.mealGenerator.GetQuantity(index1);
        this.MealsDataGridView.Rows[index2].Cells[3].Value = (object) "➕";
        this.MealsDataGridView.Rows[index2].Cells[4].Value = (object) "♻";
        this.MealsDataGridView.Rows[index2].Cells[5].Value = (object) "\uD83D\uDDD1";
        this.MealsDataGridView.Rows[index2].Cells[1].ToolTipText = this.GetCorrespondingToolTip(index1);
      }
    }

    private void UpdateAllDataGrids()
    {
      this.UpdateMealsGrid();
      this.UpdateContentsGrid();
    }

    public void GenerateMealsList(int n)
    {
      this.MealsDataGridView.Rows.Clear();
      this.contentsGridView.Rows.Clear();
      this.btnValidate.Enabled = true;
      this.mealGenerator.MealsGeneratorReinit(true);
      this.mealsNumber = n;
      for (int idx = 0; idx < n; ++idx)
      {
        this.mealGenerator.Add(new MealData());
        int index = this.MealsDataGridView.Rows.Add();
        this.MealsDataGridView.Rows[index].Cells[0].Value = (object) (idx + 1);
        this.MealsDataGridView.Rows[index].Cells[1].Value = (object) "";
        this.MealsDataGridView.Rows[index].Cells[2].Value = (object) this.mealGenerator.GetQuantity(idx);
        this.MealsDataGridView.Rows[index].Cells[3].Value = (object) "➕";
        this.MealsDataGridView.Rows[index].Cells[4].Value = (object) "♻";
        this.MealsDataGridView.Rows[index].Cells[5].Value = (object) "\uD83D\uDDD1";
      }
    }

    private void UpdateQuantities()
    {
      for (int index = 0; index < this.mealGenerator.Length; ++index)
        this.MealsDataGridView.Rows[index].Cells[1].Value = (object) this.mealGenerator.GetQuantity(index).ToString();
    }

    private void ShowTagsCompenents(bool visible) => this.SelectedTags.Visible = this.NotSelectedTags.Visible = visible;

    private void SetAllQuantities()
    {
      for (int index = 0; index < this.mealGenerator.Length; ++index)
      {
        string number = index < this.MealsDataGridView.Rows.Count ? this.MealsDataGridView.Rows[index].Cells[1].Value.ToString() : "0";
        if (string.IsNullOrEmpty(number))
          return;
        this.mealGenerator.SetQuantity(index, Tools.ParseDouble(number));
      }
      this.UpdateQuantities();
    }

    private void TagLabelClick(object s, EventArgs e)
    {
      Button button = (Button) s;
      if ((bool) button.Tag)
        this.mealGenerator.RemoveTagFrom(this.mealsSelectedRow, this.converter.GetFromString(button.Text));
      else
        this.mealGenerator.AddTagTo(this.mealsSelectedRow, this.converter.GetFromString(button.Text));
      if (!this.btnValidate.Enabled)
        this.searcher.Search(string.Empty, this.mealGenerator.GetTags(this.mealsSelectedRow), false, (int[]) null, false);
      this.PopulateTagsPanels();
    }

    private void PopulateTagsPanels()
    {
      TagConverter converter = new TagConverter();
      this.SelectedTags.Controls.Clear();
      this.NotSelectedTags.Controls.Clear();
      Tag[] tags1 = this.mealGenerator.GetTags(this.mealsSelectedRow);
      if (tags1 != null && tags1.Length != 0)
      {
        for (int index = 0; index < tags1.Length; ++index)
        {
          Button button = Components.TagLabel(converter.GetString(tags1[index]), true);
          button.Click += new EventHandler(this.TagLabelClick);
          this.SelectedTags.Controls.Add((Control) button);
        }
      }
      string[] tags = Consts.tags;
      for (int i = 0; i < tags.Length; ++i)
      {
        if (!((IEnumerable<Tag>) tags1).Any<Tag>((Func<Tag, bool>) (t => converter.GetString(t).Equals(tags[i]))))
        {
          Button button = Components.TagLabel(tags[i], false);
          button.Click += new EventHandler(this.TagLabelClick);
          this.NotSelectedTags.Controls.Add((Control) button);
        }
      }
    }

    private void ContentsDataGridViewCell_Click(object sender, DataGridViewCellEventArgs e)
    {
      switch (e.ColumnIndex)
      {
        case 6:
          RecipeContent[] contents1 = this.mealGenerator.Contents;
          if (e.RowIndex < 0 || e.ColumnIndex >= contents1.Length)
            break;
          double quantity1 = this.mealGenerator.GetQuantityInStock(contents1[e.RowIndex].uid) + 1.0;
          this.mealGenerator.SetQuantityInStock(contents1[e.RowIndex].uid, quantity1);
          this.UpdateContentsGrid();
          break;
        case 7:
          RecipeContent[] contents2 = this.mealGenerator.Contents;
          if (e.RowIndex < 0 || e.ColumnIndex >= contents2.Length)
            break;
          double quantity2 = this.mealGenerator.GetQuantityInStock(contents2[e.RowIndex].uid) - 1.0;
          this.mealGenerator.SetQuantityInStock(contents2[e.RowIndex].uid, quantity2);
          this.UpdateContentsGrid();
          break;
      }
    }

    private void MealsDataGridViewCell_Click(object sender, DataGridViewCellEventArgs e)
    {
      switch (e.ColumnIndex)
      {
        case 3:
          this.mealsSelectedRow = e.RowIndex;
          if (this.mealsSelectedRow < 0 || this.mealsSelectedRow >= this.MealsDataGridView.Rows.Count)
            break;
          this.ShowTagsCompenents(true);
          this.PopulateTagsPanels();
          break;
        case 4:
          this.mealGenerator.SetQuantity(e.RowIndex, 1.0);
          this.mealGenerator.ChangeMeal(e.RowIndex);
          this.UpdateAllDataGrids();
          break;
        case 5:
          this.mealGenerator.RemoveAt(e.RowIndex);
          this.mealsNumber = this.mealGenerator.Length;
          this.UpdateAllDataGrids();
          break;
      }
    }

    private void btnAddRecipe_Click(object sender, EventArgs e) => new AddRecipePopup(Database.AllMenus.Select<Recipe, string>((Func<Recipe, string>) (x => x.title)).ToArray<string>())
    {
      action = ((Action<int>) (s =>
      {
        ++this.mealsNumber;
        this.mealGenerator.Add(s);
        this.UpdateAllDataGrids();
      }))
    }.ShowPopup();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle7 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle8 = new DataGridViewCellStyle();
      this.panel1 = new Panel();
      this.MealsDataGridView = new DataGridView();
      this.Num = new DataGridViewTextBoxColumn();
      this.Nom = new DataGridViewTextBoxColumn();
      this.NbPersonne = new DataGridViewTextBoxColumn();
      this.Tags = new DataGridViewButtonColumn();
      this.Change = new DataGridViewButtonColumn();
      this.Remove = new DataGridViewButtonColumn();
      this.panel3 = new Panel();
      this.footerPanel = new Panel();
      this.btnGenerate = new Button();
      this.panel6 = new Panel();
      this.contentsGridView = new DataGridView();
      this.NumContent = new DataGridViewTextBoxColumn();
      this.ContentName = new DataGridViewTextBoxColumn();
      this.ContentQuantity = new DataGridViewTextBoxColumn();
      this.EnStock = new DataGridViewTextBoxColumn();
      this.Total = new DataGridViewTextBoxColumn();
      this.Unit = new DataGridViewTextBoxColumn();
      this.moins = new DataGridViewButtonColumn();
      this.Plus = new DataGridViewButtonColumn();
      this.panel4 = new Panel();
      this.TagsPanel = new Panel();
      this.panel8 = new Panel();
      this.NotSelectedTags = new FlowLayoutPanel();
      this.SelectedTags = new FlowLayoutPanel();
      this.panel7 = new Panel();
      this.btnValidate = new Button();
      this.btnAddContents = new Button();
      this.panel2 = new Panel();
      this.btnAddRecipe = new Button();
      this.label1 = new Label();
      this.recipeBindDataBindingSource = new BindingSource(this.components);
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.MealsDataGridView).BeginInit();
      this.panel3.SuspendLayout();
      this.footerPanel.SuspendLayout();
      this.panel6.SuspendLayout();
      ((ISupportInitialize) this.contentsGridView).BeginInit();
      this.panel4.SuspendLayout();
      this.TagsPanel.SuspendLayout();
      this.panel8.SuspendLayout();
      this.panel7.SuspendLayout();
      this.panel2.SuspendLayout();
      ((ISupportInitialize) this.recipeBindDataBindingSource).BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.MealsDataGridView);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(823, 165);
      this.panel1.TabIndex = 0;
      this.MealsDataGridView.AllowUserToAddRows = false;
      this.MealsDataGridView.AllowUserToDeleteRows = false;
      this.MealsDataGridView.AllowUserToResizeColumns = false;
      this.MealsDataGridView.AllowUserToResizeRows = false;
      gridViewCellStyle1.BackColor = Color.FromArgb(37, 37, 38);
      gridViewCellStyle1.ForeColor = Color.White;
      gridViewCellStyle1.SelectionBackColor = Color.FromArgb(37, 37, 38);
      gridViewCellStyle1.SelectionForeColor = Color.White;
      this.MealsDataGridView.AlternatingRowsDefaultCellStyle = gridViewCellStyle1;
      this.MealsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
      this.MealsDataGridView.BackgroundColor = Color.FromArgb(37, 40, 42);
      this.MealsDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = Color.FromArgb(37, 37, 38);
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = Color.White;
      gridViewCellStyle2.SelectionBackColor = Color.FromArgb(37, 37, 38);
      gridViewCellStyle2.SelectionForeColor = Color.White;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.True;
      this.MealsDataGridView.ColumnHeadersDefaultCellStyle = gridViewCellStyle2;
      this.MealsDataGridView.ColumnHeadersHeight = 40;
      this.MealsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
      this.MealsDataGridView.Columns.AddRange((DataGridViewColumn) this.Num, (DataGridViewColumn) this.Nom, (DataGridViewColumn) this.NbPersonne, (DataGridViewColumn) this.Tags, (DataGridViewColumn) this.Change, (DataGridViewColumn) this.Remove);
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle3.BackColor = Color.FromArgb(37, 40, 42);
      gridViewCellStyle3.Font = new Font("Arial", 10.2f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle3.ForeColor = Color.FromArgb(37, 37, 38);
      gridViewCellStyle3.SelectionBackColor = Color.FromArgb(37, 40, 42);
      gridViewCellStyle3.SelectionForeColor = Color.White;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.False;
      this.MealsDataGridView.DefaultCellStyle = gridViewCellStyle3;
      this.MealsDataGridView.Dock = DockStyle.Fill;
      this.MealsDataGridView.EnableHeadersVisualStyles = false;
      this.MealsDataGridView.GridColor = Color.White;
      this.MealsDataGridView.Location = new Point(0, 0);
      this.MealsDataGridView.Margin = new Padding(2, 2, 2, 2);
      this.MealsDataGridView.Name = "MealsDataGridView";
      this.MealsDataGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
      this.MealsDataGridView.RowHeadersVisible = false;
      this.MealsDataGridView.RowHeadersWidth = 51;
      gridViewCellStyle4.BackColor = Color.FromArgb(37, 40, 42);
      gridViewCellStyle4.ForeColor = Color.White;
      gridViewCellStyle4.SelectionBackColor = Color.FromArgb(37, 40, 42);
      gridViewCellStyle4.SelectionForeColor = Color.White;
      this.MealsDataGridView.RowsDefaultCellStyle = gridViewCellStyle4;
      this.MealsDataGridView.RowTemplate.Height = 24;
      this.MealsDataGridView.Size = new Size(823, 165);
      this.MealsDataGridView.TabIndex = 1;
      this.Num.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.Num.HeaderText = "N°";
      this.Num.MinimumWidth = 6;
      this.Num.Name = "Num";
      this.Num.ReadOnly = true;
      this.Num.Width = 30;
      this.Nom.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.Nom.HeaderText = "Nom";
      this.Nom.MinimumWidth = 6;
      this.Nom.Name = "Nom";
      this.Nom.ReadOnly = true;
      this.Nom.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.NbPersonne.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.NbPersonne.HeaderText = "Quantité";
      this.NbPersonne.MinimumWidth = 6;
      this.NbPersonne.Name = "NbPersonne";
      this.NbPersonne.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.NbPersonne.Width = 70;
      this.Tags.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.Tags.FlatStyle = FlatStyle.Flat;
      this.Tags.HeaderText = "Tags";
      this.Tags.MinimumWidth = 6;
      this.Tags.Name = "Tags";
      this.Tags.ReadOnly = true;
      this.Tags.Width = 70;
      this.Change.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.Change.FlatStyle = FlatStyle.Flat;
      this.Change.HeaderText = "Changer";
      this.Change.MinimumWidth = 6;
      this.Change.Name = "Change";
      this.Change.ReadOnly = true;
      this.Change.Width = 70;
      this.Remove.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.Remove.FlatStyle = FlatStyle.Flat;
      this.Remove.HeaderText = "Suppr.";
      this.Remove.MinimumWidth = 6;
      this.Remove.Name = "Remove";
      this.Remove.Width = 70;
      this.panel3.Controls.Add((Control) this.footerPanel);
      this.panel3.Controls.Add((Control) this.panel6);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(0, 229);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(823, 249);
      this.panel3.TabIndex = 0;
      this.footerPanel.Controls.Add((Control) this.btnGenerate);
      this.footerPanel.Dock = DockStyle.Bottom;
      this.footerPanel.Location = new Point(0, 220);
      this.footerPanel.Name = "footerPanel";
      this.footerPanel.Size = new Size(823, 29);
      this.footerPanel.TabIndex = 1;
      this.btnGenerate.Dock = DockStyle.Right;
      this.btnGenerate.FlatStyle = FlatStyle.Flat;
      this.btnGenerate.ForeColor = Color.FromArgb(228, 95, 1);
      this.btnGenerate.Location = new Point(707, 0);
      this.btnGenerate.Name = "btnGenerate";
      this.btnGenerate.Size = new Size(116, 29);
      this.btnGenerate.TabIndex = 2;
      this.btnGenerate.Text = "Générer";
      this.btnGenerate.UseVisualStyleBackColor = true;
      this.panel6.Controls.Add((Control) this.contentsGridView);
      this.panel6.Dock = DockStyle.Fill;
      this.panel6.Location = new Point(0, 0);
      this.panel6.Name = "panel6";
      this.panel6.Size = new Size(823, 249);
      this.panel6.TabIndex = 2;
      this.contentsGridView.AllowUserToAddRows = false;
      this.contentsGridView.AllowUserToDeleteRows = false;
      this.contentsGridView.AllowUserToResizeColumns = false;
      this.contentsGridView.AllowUserToResizeRows = false;
      gridViewCellStyle5.BackColor = Color.FromArgb(37, 37, 38);
      gridViewCellStyle5.ForeColor = Color.White;
      gridViewCellStyle5.SelectionBackColor = Color.FromArgb(37, 37, 38);
      gridViewCellStyle5.SelectionForeColor = Color.White;
      this.contentsGridView.AlternatingRowsDefaultCellStyle = gridViewCellStyle5;
      this.contentsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
      this.contentsGridView.BackgroundColor = Color.FromArgb(37, 40, 42);
      this.contentsGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
      gridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle6.BackColor = Color.FromArgb(37, 37, 38);
      gridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle6.ForeColor = Color.White;
      gridViewCellStyle6.SelectionBackColor = Color.FromArgb(37, 37, 38);
      gridViewCellStyle6.SelectionForeColor = Color.White;
      gridViewCellStyle6.WrapMode = DataGridViewTriState.True;
      this.contentsGridView.ColumnHeadersDefaultCellStyle = gridViewCellStyle6;
      this.contentsGridView.ColumnHeadersHeight = 40;
      this.contentsGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
      this.contentsGridView.Columns.AddRange((DataGridViewColumn) this.NumContent, (DataGridViewColumn) this.ContentName, (DataGridViewColumn) this.ContentQuantity, (DataGridViewColumn) this.EnStock, (DataGridViewColumn) this.Total, (DataGridViewColumn) this.Unit, (DataGridViewColumn) this.moins, (DataGridViewColumn) this.Plus);
      gridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle7.BackColor = Color.FromArgb(37, 40, 42);
      gridViewCellStyle7.Font = new Font("Arial", 10.2f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle7.ForeColor = Color.FromArgb(37, 37, 38);
      gridViewCellStyle7.SelectionBackColor = Color.FromArgb(37, 40, 42);
      gridViewCellStyle7.SelectionForeColor = Color.White;
      gridViewCellStyle7.WrapMode = DataGridViewTriState.False;
      this.contentsGridView.DefaultCellStyle = gridViewCellStyle7;
      this.contentsGridView.Dock = DockStyle.Fill;
      this.contentsGridView.EnableHeadersVisualStyles = false;
      this.contentsGridView.GridColor = Color.White;
      this.contentsGridView.Location = new Point(0, 0);
      this.contentsGridView.Margin = new Padding(2, 2, 2, 2);
      this.contentsGridView.Name = "contentsGridView";
      this.contentsGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
      this.contentsGridView.RowHeadersVisible = false;
      this.contentsGridView.RowHeadersWidth = 51;
      gridViewCellStyle8.BackColor = Color.FromArgb(37, 40, 42);
      gridViewCellStyle8.ForeColor = Color.White;
      gridViewCellStyle8.SelectionBackColor = Color.FromArgb(37, 40, 42);
      gridViewCellStyle8.SelectionForeColor = Color.White;
      this.contentsGridView.RowsDefaultCellStyle = gridViewCellStyle8;
      this.contentsGridView.RowTemplate.Height = 24;
      this.contentsGridView.Size = new Size(823, 249);
      this.contentsGridView.TabIndex = 1;
      this.NumContent.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.NumContent.HeaderText = "N°";
      this.NumContent.MinimumWidth = 6;
      this.NumContent.Name = "NumContent";
      this.NumContent.ReadOnly = true;
      this.NumContent.Width = 30;
      this.ContentName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.ContentName.HeaderText = "Nom";
      this.ContentName.MinimumWidth = 6;
      this.ContentName.Name = "ContentName";
      this.ContentName.ReadOnly = true;
      this.ContentName.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.ContentQuantity.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.ContentQuantity.HeaderText = "Quantité";
      this.ContentQuantity.MinimumWidth = 6;
      this.ContentQuantity.Name = "ContentQuantity";
      this.ContentQuantity.ReadOnly = true;
      this.ContentQuantity.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.ContentQuantity.Width = 200;
      this.EnStock.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.EnStock.HeaderText = "En stock";
      this.EnStock.MinimumWidth = 6;
      this.EnStock.Name = "EnStock";
      this.EnStock.Width = 60;
      this.Total.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.Total.HeaderText = "Total";
      this.Total.MinimumWidth = 6;
      this.Total.Name = "Total";
      this.Total.ReadOnly = true;
      this.Total.Width = 60;
      this.Unit.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.Unit.HeaderText = "Unité";
      this.Unit.MinimumWidth = 6;
      this.Unit.Name = "Unit";
      this.Unit.ReadOnly = true;
      this.Unit.SortMode = DataGridViewColumnSortMode.NotSortable;
      this.Unit.Width = 60;
      this.moins.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.moins.FlatStyle = FlatStyle.Flat;
      this.moins.HeaderText = "➖";
      this.moins.MinimumWidth = 6;
      this.moins.Name = "moins";
      this.moins.ReadOnly = true;
      this.moins.Width = 32;
      this.Plus.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
      this.Plus.FlatStyle = FlatStyle.Flat;
      this.Plus.HeaderText = "➕";
      this.Plus.MinimumWidth = 6;
      this.Plus.Name = "Plus";
      this.Plus.ReadOnly = true;
      this.Plus.Width = 32;
      this.panel4.Controls.Add((Control) this.TagsPanel);
      this.panel4.Controls.Add((Control) this.panel7);
      this.panel4.Controls.Add((Control) this.panel2);
      this.panel4.Dock = DockStyle.Top;
      this.panel4.Location = new Point(0, 165);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(823, 64);
      this.panel4.TabIndex = 1;
      this.TagsPanel.Controls.Add((Control) this.panel8);
      this.TagsPanel.Dock = DockStyle.Fill;
      this.TagsPanel.Location = new Point(103, 0);
      this.TagsPanel.Name = "TagsPanel";
      this.TagsPanel.Size = new Size(596, 64);
      this.TagsPanel.TabIndex = 6;
      this.panel8.Controls.Add((Control) this.NotSelectedTags);
      this.panel8.Controls.Add((Control) this.SelectedTags);
      this.panel8.Dock = DockStyle.Fill;
      this.panel8.Location = new Point(0, 0);
      this.panel8.Name = "panel8";
      this.panel8.Size = new Size(596, 64);
      this.panel8.TabIndex = 2;
      this.NotSelectedTags.AutoScroll = true;
      this.NotSelectedTags.Dock = DockStyle.Fill;
      this.NotSelectedTags.Location = new Point(306, 0);
      this.NotSelectedTags.Name = "NotSelectedTags";
      this.NotSelectedTags.Size = new Size(290, 64);
      this.NotSelectedTags.TabIndex = 2;
      this.NotSelectedTags.Visible = false;
      this.SelectedTags.AutoScroll = true;
      this.SelectedTags.Dock = DockStyle.Left;
      this.SelectedTags.Location = new Point(0, 0);
      this.SelectedTags.Name = "SelectedTags";
      this.SelectedTags.Size = new Size(306, 64);
      this.SelectedTags.TabIndex = 1;
      this.panel7.Controls.Add((Control) this.btnValidate);
      this.panel7.Controls.Add((Control) this.btnAddContents);
      this.panel7.Dock = DockStyle.Right;
      this.panel7.Location = new Point(699, 0);
      this.panel7.Name = "panel7";
      this.panel7.Size = new Size(124, 64);
      this.panel7.TabIndex = 5;
      this.btnValidate.FlatStyle = FlatStyle.Flat;
      this.btnValidate.ForeColor = Color.FromArgb(228, 95, 1);
      this.btnValidate.Location = new Point(5, 0);
      this.btnValidate.Name = "btnValidate";
      this.btnValidate.Size = new Size(116, 23);
      this.btnValidate.TabIndex = 3;
      this.btnValidate.Text = "Valider";
      this.btnValidate.UseVisualStyleBackColor = true;
      this.btnAddContents.FlatStyle = FlatStyle.Flat;
      this.btnAddContents.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnAddContents.ForeColor = Color.FromArgb(228, 95, 1);
      this.btnAddContents.Location = new Point(97, 40);
      this.btnAddContents.Name = "btnAddContents";
      this.btnAddContents.Size = new Size(24, 24);
      this.btnAddContents.TabIndex = 1;
      this.btnAddContents.Text = "➕";
      this.btnAddContents.UseVisualStyleBackColor = true;
      this.panel2.Controls.Add((Control) this.btnAddRecipe);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Dock = DockStyle.Left;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(103, 64);
      this.panel2.TabIndex = 4;
      this.btnAddRecipe.FlatStyle = FlatStyle.Flat;
      this.btnAddRecipe.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnAddRecipe.ForeColor = Color.FromArgb(228, 95, 1);
      this.btnAddRecipe.Location = new Point(3, 3);
      this.btnAddRecipe.Name = "btnAddRecipe";
      this.btnAddRecipe.Size = new Size(24, 24);
      this.btnAddRecipe.TabIndex = 2;
      this.btnAddRecipe.Text = "➕";
      this.btnAddRecipe.UseVisualStyleBackColor = true;
      this.btnAddRecipe.Click += new EventHandler(this.btnAddRecipe_Click);
      this.label1.AutoSize = true;
      this.label1.FlatStyle = FlatStyle.Flat;
      this.label1.Font = new Font("Sitka Subheading", 9.749999f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = Color.FromArgb(228, 95, 1);
      this.label1.Location = new Point(3, 42);
      this.label1.Name = "label1";
      this.label1.Size = new Size(100, 19);
      this.label1.TabIndex = 0;
      this.label1.Text = "Liste de courses";
      this.recipeBindDataBindingSource.DataSource = (object) typeof (RecipeBindData);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(37, 37, 38);
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.panel4);
      this.Controls.Add((Control) this.panel1);
      this.ForeColor = Color.FromArgb(37, 37, 38);
      this.Name = nameof (GeneratePanel);
      this.Size = new Size(823, 478);
      this.panel1.ResumeLayout(false);
      ((ISupportInitialize) this.MealsDataGridView).EndInit();
      this.panel3.ResumeLayout(false);
      this.footerPanel.ResumeLayout(false);
      this.panel6.ResumeLayout(false);
      ((ISupportInitialize) this.contentsGridView).EndInit();
      this.panel4.ResumeLayout(false);
      this.TagsPanel.ResumeLayout(false);
      this.panel8.ResumeLayout(false);
      this.panel7.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      ((ISupportInitialize) this.recipeBindDataBindingSource).EndInit();
      this.ResumeLayout(false);
    }
  }
}
