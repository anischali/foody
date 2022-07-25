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
        private SplitContainer splitContainer1;
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.MealsDataGridView = new System.Windows.Forms.DataGridView();
            this.Num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NbPersonne = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tags = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Change = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Remove = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.contentsGridView = new System.Windows.Forms.DataGridView();
            this.NumContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContentQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EnStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moins = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Plus = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.TagsPanel = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.NotSelectedTags = new System.Windows.Forms.FlowLayoutPanel();
            this.SelectedTags = new System.Windows.Forms.FlowLayoutPanel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnAddContents = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAddRecipe = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.recipeBindDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MealsDataGridView)).BeginInit();
            this.panel3.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contentsGridView)).BeginInit();
            this.panel4.SuspendLayout();
            this.TagsPanel.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recipeBindDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MealsDataGridView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(823, 165);
            this.panel1.TabIndex = 0;
            // 
            // MealsDataGridView
            // 
            this.MealsDataGridView.AllowUserToAddRows = false;
            this.MealsDataGridView.AllowUserToDeleteRows = false;
            this.MealsDataGridView.AllowUserToResizeColumns = false;
            this.MealsDataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.MealsDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.MealsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.MealsDataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(40)))), ((int)(((byte)(42)))));
            this.MealsDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MealsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.MealsDataGridView.ColumnHeadersHeight = 40;
            this.MealsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.MealsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Num,
            this.Nom,
            this.NbPersonne,
            this.Tags,
            this.Change,
            this.Remove});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(40)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(40)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.MealsDataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.MealsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MealsDataGridView.EnableHeadersVisualStyles = false;
            this.MealsDataGridView.GridColor = System.Drawing.Color.White;
            this.MealsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.MealsDataGridView.Margin = new System.Windows.Forms.Padding(2);
            this.MealsDataGridView.Name = "MealsDataGridView";
            this.MealsDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.MealsDataGridView.RowHeadersVisible = false;
            this.MealsDataGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(40)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(40)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this.MealsDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.MealsDataGridView.RowTemplate.Height = 24;
            this.MealsDataGridView.Size = new System.Drawing.Size(823, 165);
            this.MealsDataGridView.TabIndex = 1;
            // 
            // Num
            // 
            this.Num.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Num.HeaderText = "N°";
            this.Num.MinimumWidth = 6;
            this.Num.Name = "Num";
            this.Num.ReadOnly = true;
            this.Num.Width = 30;
            // 
            // Nom
            // 
            this.Nom.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Nom.HeaderText = "Nom";
            this.Nom.MinimumWidth = 6;
            this.Nom.Name = "Nom";
            this.Nom.ReadOnly = true;
            this.Nom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // NbPersonne
            // 
            this.NbPersonne.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.NbPersonne.HeaderText = "Quantité";
            this.NbPersonne.MinimumWidth = 6;
            this.NbPersonne.Name = "NbPersonne";
            this.NbPersonne.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.NbPersonne.Width = 70;
            // 
            // Tags
            // 
            this.Tags.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Tags.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Tags.HeaderText = "Tags";
            this.Tags.MinimumWidth = 6;
            this.Tags.Name = "Tags";
            this.Tags.ReadOnly = true;
            this.Tags.Width = 70;
            // 
            // Change
            // 
            this.Change.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Change.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Change.HeaderText = "Changer";
            this.Change.MinimumWidth = 6;
            this.Change.Name = "Change";
            this.Change.ReadOnly = true;
            this.Change.Width = 70;
            // 
            // Remove
            // 
            this.Remove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Remove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Remove.HeaderText = "Suppr.";
            this.Remove.MinimumWidth = 6;
            this.Remove.Name = "Remove";
            this.Remove.Width = 70;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 229);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(823, 249);
            this.panel3.TabIndex = 0;
            // 
            // footerPanel
            // 
            this.footerPanel.Controls.Add(this.btnGenerate);
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.footerPanel.Location = new System.Drawing.Point(0, 0);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Size = new System.Drawing.Size(823, 29);
            this.footerPanel.TabIndex = 1;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(95)))), ((int)(((byte)(1)))));
            this.btnGenerate.Location = new System.Drawing.Point(707, 0);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(116, 29);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "Générer";
            this.btnGenerate.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.splitContainer1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(823, 249);
            this.panel6.TabIndex = 2;
            // 
            // contentsGridView
            // 
            this.contentsGridView.AllowUserToAddRows = false;
            this.contentsGridView.AllowUserToDeleteRows = false;
            this.contentsGridView.AllowUserToResizeColumns = false;
            this.contentsGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            this.contentsGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.contentsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.contentsGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(40)))), ((int)(((byte)(42)))));
            this.contentsGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.contentsGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.contentsGridView.ColumnHeadersHeight = 40;
            this.contentsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.contentsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NumContent,
            this.ContentName,
            this.ContentQuantity,
            this.EnStock,
            this.Total,
            this.Unit,
            this.moins,
            this.Plus});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(40)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(40)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.contentsGridView.DefaultCellStyle = dataGridViewCellStyle7;
            this.contentsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentsGridView.EnableHeadersVisualStyles = false;
            this.contentsGridView.GridColor = System.Drawing.Color.White;
            this.contentsGridView.Location = new System.Drawing.Point(0, 0);
            this.contentsGridView.Margin = new System.Windows.Forms.Padding(2);
            this.contentsGridView.Name = "contentsGridView";
            this.contentsGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.contentsGridView.RowHeadersVisible = false;
            this.contentsGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(40)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(40)))), ((int)(((byte)(42)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White;
            this.contentsGridView.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.contentsGridView.RowTemplate.Height = 24;
            this.contentsGridView.Size = new System.Drawing.Size(823, 216);
            this.contentsGridView.TabIndex = 1;
            // 
            // NumContent
            // 
            this.NumContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.NumContent.HeaderText = "N°";
            this.NumContent.MinimumWidth = 6;
            this.NumContent.Name = "NumContent";
            this.NumContent.ReadOnly = true;
            this.NumContent.Width = 30;
            // 
            // ContentName
            // 
            this.ContentName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ContentName.HeaderText = "Nom";
            this.ContentName.MinimumWidth = 6;
            this.ContentName.Name = "ContentName";
            this.ContentName.ReadOnly = true;
            this.ContentName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ContentQuantity
            // 
            this.ContentQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ContentQuantity.HeaderText = "Quantité";
            this.ContentQuantity.MinimumWidth = 6;
            this.ContentQuantity.Name = "ContentQuantity";
            this.ContentQuantity.ReadOnly = true;
            this.ContentQuantity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ContentQuantity.Width = 200;
            // 
            // EnStock
            // 
            this.EnStock.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.EnStock.HeaderText = "En stock";
            this.EnStock.MinimumWidth = 6;
            this.EnStock.Name = "EnStock";
            this.EnStock.Width = 60;
            // 
            // Total
            // 
            this.Total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Total.HeaderText = "Total";
            this.Total.MinimumWidth = 6;
            this.Total.Name = "Total";
            this.Total.ReadOnly = true;
            this.Total.Width = 60;
            // 
            // Unit
            // 
            this.Unit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Unit.HeaderText = "Unité";
            this.Unit.MinimumWidth = 6;
            this.Unit.Name = "Unit";
            this.Unit.ReadOnly = true;
            this.Unit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Unit.Width = 60;
            // 
            // moins
            // 
            this.moins.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.moins.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.moins.HeaderText = "➖";
            this.moins.MinimumWidth = 6;
            this.moins.Name = "moins";
            this.moins.ReadOnly = true;
            this.moins.Width = 32;
            // 
            // Plus
            // 
            this.Plus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Plus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Plus.HeaderText = "➕";
            this.Plus.MinimumWidth = 6;
            this.Plus.Name = "Plus";
            this.Plus.ReadOnly = true;
            this.Plus.Width = 32;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.TagsPanel);
            this.panel4.Controls.Add(this.panel7);
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 165);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(823, 64);
            this.panel4.TabIndex = 1;
            // 
            // TagsPanel
            // 
            this.TagsPanel.Controls.Add(this.panel8);
            this.TagsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TagsPanel.Location = new System.Drawing.Point(103, 0);
            this.TagsPanel.Name = "TagsPanel";
            this.TagsPanel.Size = new System.Drawing.Size(596, 64);
            this.TagsPanel.TabIndex = 6;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.NotSelectedTags);
            this.panel8.Controls.Add(this.SelectedTags);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(596, 64);
            this.panel8.TabIndex = 2;
            // 
            // NotSelectedTags
            // 
            this.NotSelectedTags.AutoScroll = true;
            this.NotSelectedTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NotSelectedTags.Location = new System.Drawing.Point(306, 0);
            this.NotSelectedTags.Name = "NotSelectedTags";
            this.NotSelectedTags.Size = new System.Drawing.Size(290, 64);
            this.NotSelectedTags.TabIndex = 2;
            this.NotSelectedTags.Visible = false;
            // 
            // SelectedTags
            // 
            this.SelectedTags.AutoScroll = true;
            this.SelectedTags.Dock = System.Windows.Forms.DockStyle.Left;
            this.SelectedTags.Location = new System.Drawing.Point(0, 0);
            this.SelectedTags.Name = "SelectedTags";
            this.SelectedTags.Size = new System.Drawing.Size(306, 64);
            this.SelectedTags.TabIndex = 1;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btnValidate);
            this.panel7.Controls.Add(this.btnAddContents);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(699, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(124, 64);
            this.panel7.TabIndex = 5;
            // 
            // btnValidate
            // 
            this.btnValidate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValidate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(95)))), ((int)(((byte)(1)))));
            this.btnValidate.Location = new System.Drawing.Point(5, 0);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(116, 23);
            this.btnValidate.TabIndex = 3;
            this.btnValidate.Text = "Valider";
            this.btnValidate.UseVisualStyleBackColor = true;
            // 
            // btnAddContents
            // 
            this.btnAddContents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddContents.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddContents.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(95)))), ((int)(((byte)(1)))));
            this.btnAddContents.Location = new System.Drawing.Point(97, 40);
            this.btnAddContents.Name = "btnAddContents";
            this.btnAddContents.Size = new System.Drawing.Size(24, 24);
            this.btnAddContents.TabIndex = 1;
            this.btnAddContents.Text = "➕";
            this.btnAddContents.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnAddRecipe);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(103, 64);
            this.panel2.TabIndex = 4;
            // 
            // btnAddRecipe
            // 
            this.btnAddRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddRecipe.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddRecipe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(95)))), ((int)(((byte)(1)))));
            this.btnAddRecipe.Location = new System.Drawing.Point(3, 3);
            this.btnAddRecipe.Name = "btnAddRecipe";
            this.btnAddRecipe.Size = new System.Drawing.Size(24, 24);
            this.btnAddRecipe.TabIndex = 2;
            this.btnAddRecipe.Text = "➕";
            this.btnAddRecipe.UseVisualStyleBackColor = true;
            this.btnAddRecipe.Click += new System.EventHandler(this.btnAddRecipe_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Sitka Subheading", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(95)))), ((int)(((byte)(1)))));
            this.label1.Location = new System.Drawing.Point(3, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Liste de courses";
            // 
            // recipeBindDataBindingSource
            // 
            this.recipeBindDataBindingSource.DataSource = typeof(Foody.Models.RecipeBindData);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.contentsGridView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.footerPanel);
            this.splitContainer1.Size = new System.Drawing.Size(823, 249);
            this.splitContainer1.SplitterDistance = 216;
            this.splitContainer1.TabIndex = 2;
            // 
            // GeneratePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.Name = "GeneratePanel";
            this.Size = new System.Drawing.Size(823, 478);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MealsDataGridView)).EndInit();
            this.panel3.ResumeLayout(false);
            this.footerPanel.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.contentsGridView)).EndInit();
            this.panel4.ResumeLayout(false);
            this.TagsPanel.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recipeBindDataBindingSource)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

    }
  }
}
