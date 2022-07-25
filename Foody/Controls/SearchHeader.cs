// Decompiled with JetBrains decompiler
// Type: Foody.Controls.SearchHeader
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using Foody.Generic;
using Foody.IO;
using Foody.Lib.Generic;
using Foody.Models;
using Foody.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Foody.Controls
{
  public class SearchHeader : UserControl
  {
    private readonly Timer ReinitDisplay = new Timer();
    internal EventHandler OnExpandSearchBar;
    internal EventHandler OnReduceSearchBar;
    private SearchEngine engine = new SearchEngine();
    private List<int> selectedIngredients = new List<int>();
    private bool active = false;
    private IContainer components = (IContainer) null;
    private Panel pnlTextBoxAndSearch;
    private Panel SearchButtonPanel;
    private Panel SearchHeaderMainPanel;
    private Panel AdvancedSearchButtonPanel;
    private Panel searchBarPanel;
    private Button btnSearch;
    private TextBox tbSearchText;
    private Button btnValidateTextSearch;
    private Button button1;
    private Panel pnlAdvancedSearchTags;
    private Label lblContents;
    private Label lblTags;
    private CheckBox chbAndOr;
    private ComboBox cbContents;
    private TagsLabelsControl tagsToSearch;
    private SplitContainer splitContainer1;
    private SplitContainer splitContainer2;
    private SplitContainer splitContainer3;
    private SplitContainer splitContainer4;
    private FlowLayoutPanel ingredientsPanel;
    private CheckBox chbAndOrContent;

    public SearchHeader()
    {
      this.InitializeComponent();
      this.InitEvents();
      this.ReinitDisplay.Interval = 5000;
      this.ReinitDisplay.Start();
      this.Size = new Size(this.Size.Width, 50);
      this.PopulateTags();
      this.PopulateWithIngredients();
    }

    private void InitEvents()
    {
      this.ReinitDisplay.Tick += (EventHandler) ((s, e) =>
      {
        if (!string.IsNullOrEmpty(this.tbSearchText.Text) || this.SearchButtonPanel.Visible || this.pnlAdvancedSearchTags.Visible)
          return;
        this.ReduceSearchBar();
      });
      this.btnSearch.Click += (EventHandler) ((s, e) =>
      {
        this.SearchButtonPanel.Hide();
        this.AdvancedSearchButtonPanel.Show();
        this.searchBarPanel.Show();
        this.ReinitDisplay.Start();
        this.active = true;
        if (this.OnExpandSearchBar == null)
          return;
        this.tbSearchText.Select();
        this.OnExpandSearchBar((object) null, (EventArgs) null);
      });
      this.button1.Click += (EventHandler) ((s, e) =>
      {
        if (this.pnlAdvancedSearchTags.Visible)
        {
          this.button1.Text = "\uD83D\uDD3B";
          this.pnlAdvancedSearchTags.Visible = false;
          this.Size = new Size(this.Size.Width, 76);
          if (this.OnReduceSearchBar == null)
            return;
          this.OnReduceSearchBar((object) null, (EventArgs) null);
        }
        else
        {
          this.button1.Text = "\uD83D\uDD3A";
          this.pnlAdvancedSearchTags.Visible = true;
          this.Size = new Size(this.Size.Width, 290);
          if (this.OnExpandSearchBar != null)
            this.OnExpandSearchBar((object) null, (EventArgs) null);
        }
      });
      this.cbContents.SelectedIndexChanged += (EventHandler) ((s, e) =>
      {
        int selectedIndex = this.cbContents.SelectedIndex;
        if (this.selectedIngredients.Contains(selectedIndex))
          return;
        this.selectedIngredients.Add(selectedIndex);
        this.AddToIngredientsPanel(selectedIndex);
      });
      this.tbSearchText.TextChanged += (EventHandler) ((s, e) => this.RefreshSearchEngine());
      this.tagsToSearch.RefreshEvent += (EventHandler) ((s, e) => this.RefreshSearchEngine());
      this.chbAndOr.CheckStateChanged += (EventHandler) ((s, e) => this.RefreshSearchEngine());
      this.chbAndOrContent.CheckStateChanged += (EventHandler) ((s, e) => this.RefreshSearchEngine());
    }

    private void AddToIngredientsPanel(int index)
    {
      this.ingredientsPanel.Controls.Add((Control) this.GetIngredientLabel(Database.contents[index].Name, index, true, (Action<object, EventArgs>) ((s, e) =>
      {
        Button button = s as Button;
        if (!this.selectedIngredients.Contains((int) button.Tag))
          return;
        this.selectedIngredients.Remove((int) button.Tag);
        this.ingredientsPanel.Controls.Remove((Control) button);
        this.RefreshSearchEngine();
      })));
      this.RefreshSearchEngine();
    }

    public void ResetSearch()
    {
      this.active = false;
      this.engine.ResetEngine();
      this.ingredientsPanel.Controls.Clear();
      this.selectedIngredients.Clear();
      this.tagsToSearch.ResetTags();
    }

    private Button GetIngredientLabel(
      string name,
      int tag,
      bool selected,
      Action<object, EventArgs> action)
    {
      Button ingredientLabel = Components.TagLabel(name, selected);
      ingredientLabel.Tag = (object) tag;
      ingredientLabel.Click += (EventHandler) ((s, e) =>
      {
        if (action == null)
          return;
        action(s, e);
      });
      return ingredientLabel;
    }

    internal void ReduceSearchBar()
    {
      if (this.ReinitDisplay.Enabled)
        this.ReinitDisplay.Stop();
      this.AdvancedSearchButtonPanel.Hide();
      this.searchBarPanel.Hide();
      this.SearchButtonPanel.Show();
      this.AdvancedSearchButtonPanel.Visible = false;
      this.Size = new Size(this.Size.Width, 64);
      if (this.OnReduceSearchBar == null)
        return;
      this.OnReduceSearchBar((object) null, (EventArgs) null);
    }

    private void PopulateTags() => this.tagsToSearch.SetControlsItems(2, (Tag[]) null);

    private void PopulateWithIngredients()
    {
      if (Database.contents == null)
        return;
      string[] array = Database.contents.Select<Content, string>((Func<Content, string>) (i => i.Name)).ToArray<string>();
      if (array != null)
        this.cbContents.Items.AddRange((object[]) array);
    }

    private void PopulateTagsComboBox()
    {
      List<CheckComboBoxItem> checkComboBoxItemList = new List<CheckComboBoxItem>();
      for (int tag = 0; tag < Consts.tags.Length; ++tag)
      {
        CheckComboBoxItem checkComboBoxItem = new CheckComboBoxItem(Consts.tags[tag], false, (object) tag);
        checkComboBoxItemList.Add(checkComboBoxItem);
      }
    }

    public void RefreshSearchEngine() => this.engine.Search(this.tbSearchText.Text, this.tagsToSearch.GetTags(), !this.chbAndOr.Checked, this.selectedIngredients.ToArray(), this.chbAndOrContent.Checked);

    public SearchEngine Engine => this.engine;

    public bool Active => this.active;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlTextBoxAndSearch = new Panel();
      this.searchBarPanel = new Panel();
      this.btnValidateTextSearch = new Button();
      this.tbSearchText = new TextBox();
      this.SearchButtonPanel = new Panel();
      this.btnSearch = new Button();
      this.SearchHeaderMainPanel = new Panel();
      this.AdvancedSearchButtonPanel = new Panel();
      this.button1 = new Button();
      this.pnlAdvancedSearchTags = new Panel();
      this.splitContainer1 = new SplitContainer();
      this.splitContainer2 = new SplitContainer();
      this.chbAndOrContent = new CheckBox();
      this.lblContents = new Label();
      this.lblTags = new Label();
      this.chbAndOr = new CheckBox();
      this.splitContainer3 = new SplitContainer();
      this.splitContainer4 = new SplitContainer();
      this.cbContents = new ComboBox();
      this.ingredientsPanel = new FlowLayoutPanel();
      this.tagsToSearch = new TagsLabelsControl();
      this.pnlTextBoxAndSearch.SuspendLayout();
      this.searchBarPanel.SuspendLayout();
      this.SearchButtonPanel.SuspendLayout();
      this.SearchHeaderMainPanel.SuspendLayout();
      this.AdvancedSearchButtonPanel.SuspendLayout();
      this.pnlAdvancedSearchTags.SuspendLayout();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.splitContainer2.BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.splitContainer3.BeginInit();
      this.splitContainer3.Panel1.SuspendLayout();
      this.splitContainer3.Panel2.SuspendLayout();
      this.splitContainer3.SuspendLayout();
      this.splitContainer4.BeginInit();
      this.splitContainer4.Panel1.SuspendLayout();
      this.splitContainer4.Panel2.SuspendLayout();
      this.splitContainer4.SuspendLayout();
      this.SuspendLayout();
      this.pnlTextBoxAndSearch.BackColor = Color.FromArgb(37, 40, 42);
      this.pnlTextBoxAndSearch.Controls.Add((Control) this.searchBarPanel);
      this.pnlTextBoxAndSearch.Controls.Add((Control) this.SearchButtonPanel);
      this.pnlTextBoxAndSearch.Dock = DockStyle.Top;
      this.pnlTextBoxAndSearch.Location = new Point(0, 0);
      this.pnlTextBoxAndSearch.Margin = new Padding(2);
      this.pnlTextBoxAndSearch.Name = "pnlTextBoxAndSearch";
      this.pnlTextBoxAndSearch.Size = new Size(669, 45);
      this.pnlTextBoxAndSearch.TabIndex = 0;
      this.searchBarPanel.BackColor = Color.FromArgb(37, 37, 38);
      this.searchBarPanel.Controls.Add((Control) this.btnValidateTextSearch);
      this.searchBarPanel.Controls.Add((Control) this.tbSearchText);
      this.searchBarPanel.Dock = DockStyle.Fill;
      this.searchBarPanel.Location = new Point(48, 0);
      this.searchBarPanel.Margin = new Padding(2);
      this.searchBarPanel.Name = "searchBarPanel";
      this.searchBarPanel.Size = new Size(621, 45);
      this.searchBarPanel.TabIndex = 2;
      this.searchBarPanel.Visible = false;
      this.btnValidateTextSearch.Dock = DockStyle.Right;
      this.btnValidateTextSearch.FlatAppearance.BorderColor = Color.FromArgb(100, 100, 100);
      this.btnValidateTextSearch.FlatAppearance.BorderSize = 0;
      this.btnValidateTextSearch.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 100, 100);
      this.btnValidateTextSearch.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 100, 100);
      this.btnValidateTextSearch.FlatStyle = FlatStyle.Flat;
      this.btnValidateTextSearch.Font = new Font("Microsoft Sans Serif", 10.2f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnValidateTextSearch.ForeColor = Color.FromArgb(229, 9, 20);
      this.btnValidateTextSearch.Location = new Point(573, 0);
      this.btnValidateTextSearch.Margin = new Padding(2);
      this.btnValidateTextSearch.Name = "btnValidateTextSearch";
      this.btnValidateTextSearch.Size = new Size(48, 45);
      this.btnValidateTextSearch.TabIndex = 3;
      this.btnValidateTextSearch.Text = "\uD83D\uDD0E";
      this.btnValidateTextSearch.UseVisualStyleBackColor = true;
      this.tbSearchText.BackColor = Color.FromArgb(37, 37, 38);
      this.tbSearchText.BorderStyle = BorderStyle.None;
      this.tbSearchText.Dock = DockStyle.Fill;
      this.tbSearchText.Font = new Font("Microsoft Sans Serif", 24f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tbSearchText.ForeColor = Color.FromArgb(228, 95, 1);
      this.tbSearchText.Location = new Point(0, 0);
      this.tbSearchText.Margin = new Padding(2);
      this.tbSearchText.Name = "tbSearchText";
      this.tbSearchText.Size = new Size(621, 37);
      this.tbSearchText.TabIndex = 2;
      this.tbSearchText.TextAlign = HorizontalAlignment.Center;
      this.SearchButtonPanel.BackColor = Color.FromArgb(37, 37, 38);
      this.SearchButtonPanel.Controls.Add((Control) this.btnSearch);
      this.SearchButtonPanel.Dock = DockStyle.Left;
      this.SearchButtonPanel.Location = new Point(0, 0);
      this.SearchButtonPanel.Margin = new Padding(2);
      this.SearchButtonPanel.Name = "SearchButtonPanel";
      this.SearchButtonPanel.Size = new Size(48, 45);
      this.SearchButtonPanel.TabIndex = 0;
      this.btnSearch.Dock = DockStyle.Fill;
      this.btnSearch.FlatAppearance.BorderColor = Color.FromArgb(100, 100, 100);
      this.btnSearch.FlatAppearance.BorderSize = 0;
      this.btnSearch.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 100, 100);
      this.btnSearch.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 100, 100);
      this.btnSearch.FlatStyle = FlatStyle.Flat;
      this.btnSearch.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSearch.ForeColor = Color.FromArgb(229, 9, 20);
      this.btnSearch.Location = new Point(0, 0);
      this.btnSearch.Margin = new Padding(2);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(48, 45);
      this.btnSearch.TabIndex = 0;
      this.btnSearch.Text = "\uD83D\uDD0E";
      this.btnSearch.UseVisualStyleBackColor = true;
      this.SearchHeaderMainPanel.BackColor = Color.FromArgb(37, 40, 42);
      this.SearchHeaderMainPanel.Controls.Add((Control) this.AdvancedSearchButtonPanel);
      this.SearchHeaderMainPanel.Controls.Add((Control) this.pnlTextBoxAndSearch);
      this.SearchHeaderMainPanel.Dock = DockStyle.Top;
      this.SearchHeaderMainPanel.Location = new Point(0, 0);
      this.SearchHeaderMainPanel.Margin = new Padding(2);
      this.SearchHeaderMainPanel.Name = "SearchHeaderMainPanel";
      this.SearchHeaderMainPanel.Size = new Size(669, 63);
      this.SearchHeaderMainPanel.TabIndex = 1;
      this.AdvancedSearchButtonPanel.BackColor = Color.FromArgb(37, 37, 38);
      this.AdvancedSearchButtonPanel.Controls.Add((Control) this.button1);
      this.AdvancedSearchButtonPanel.Dock = DockStyle.Fill;
      this.AdvancedSearchButtonPanel.Location = new Point(0, 45);
      this.AdvancedSearchButtonPanel.Margin = new Padding(2);
      this.AdvancedSearchButtonPanel.Name = "AdvancedSearchButtonPanel";
      this.AdvancedSearchButtonPanel.Size = new Size(669, 18);
      this.AdvancedSearchButtonPanel.TabIndex = 1;
      this.AdvancedSearchButtonPanel.Visible = false;
      this.button1.BackColor = Color.FromArgb(37, 37, 38);
      this.button1.Dock = DockStyle.Fill;
      this.button1.FlatAppearance.BorderSize = 0;
      this.button1.FlatStyle = FlatStyle.Flat;
      this.button1.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.button1.ForeColor = Color.FromArgb(229, 9, 20);
      this.button1.Location = new Point(0, 0);
      this.button1.Margin = new Padding(2);
      this.button1.Name = "button1";
      this.button1.Size = new Size(669, 18);
      this.button1.TabIndex = 4;
      this.button1.Text = "\uD83D\uDD3B";
      this.button1.TextAlign = ContentAlignment.TopCenter;
      this.button1.UseVisualStyleBackColor = false;
      this.pnlAdvancedSearchTags.BackColor = Color.FromArgb(37, 37, 38);
      this.pnlAdvancedSearchTags.Controls.Add((Control) this.splitContainer1);
      this.pnlAdvancedSearchTags.Dock = DockStyle.Fill;
      this.pnlAdvancedSearchTags.Location = new Point(0, 63);
      this.pnlAdvancedSearchTags.Margin = new Padding(2);
      this.pnlAdvancedSearchTags.Name = "pnlAdvancedSearchTags";
      this.pnlAdvancedSearchTags.Size = new Size(669, 221);
      this.pnlAdvancedSearchTags.TabIndex = 2;
      this.pnlAdvancedSearchTags.Visible = false;
      this.splitContainer1.Dock = DockStyle.Fill;
      this.splitContainer1.FixedPanel = FixedPanel.Panel1;
      this.splitContainer1.IsSplitterFixed = true;
      this.splitContainer1.Location = new Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.Controls.Add((Control) this.splitContainer2);
      this.splitContainer1.Panel2.Controls.Add((Control) this.splitContainer3);
      this.splitContainer1.Size = new Size(669, 221);
      this.splitContainer1.SplitterDistance = 106;
      this.splitContainer1.TabIndex = 5;
      this.splitContainer2.Dock = DockStyle.Fill;
      this.splitContainer2.IsSplitterFixed = true;
      this.splitContainer2.Location = new Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = Orientation.Horizontal;
      this.splitContainer2.Panel1.Controls.Add((Control) this.chbAndOrContent);
      this.splitContainer2.Panel1.Controls.Add((Control) this.lblContents);
      this.splitContainer2.Panel2.Controls.Add((Control) this.lblTags);
      this.splitContainer2.Panel2.Controls.Add((Control) this.chbAndOr);
      this.splitContainer2.Size = new Size(106, 221);
      this.splitContainer2.SplitterDistance = 112;
      this.splitContainer2.TabIndex = 0;
      this.chbAndOrContent.AutoSize = true;
      this.chbAndOrContent.ForeColor = Color.FromArgb(228, 95, 1);
      this.chbAndOrContent.Location = new Point(23, 47);
      this.chbAndOrContent.Margin = new Padding(2);
      this.chbAndOrContent.Name = "chbAndOrContent";
      this.chbAndOrContent.Size = new Size(40, 17);
      this.chbAndOrContent.TabIndex = 4;
      this.chbAndOrContent.Text = "Ou";
      this.chbAndOrContent.UseVisualStyleBackColor = true;
      this.lblContents.AutoSize = true;
      this.lblContents.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblContents.ForeColor = Color.FromArgb(228, 95, 1);
      this.lblContents.Location = new Point(2, 0);
      this.lblContents.Margin = new Padding(2, 0, 2, 0);
      this.lblContents.Name = "lblContents";
      this.lblContents.Size = new Size(89, 20);
      this.lblContents.TabIndex = 1;
      this.lblContents.Text = "Ingrédients";
      this.lblTags.AutoSize = true;
      this.lblTags.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblTags.ForeColor = Color.FromArgb(228, 95, 1);
      this.lblTags.Location = new Point(2, 9);
      this.lblTags.Margin = new Padding(2, 0, 2, 0);
      this.lblTags.Name = "lblTags";
      this.lblTags.Size = new Size(44, 20);
      this.lblTags.TabIndex = 2;
      this.lblTags.Text = "Tags";
      this.chbAndOr.AutoSize = true;
      this.chbAndOr.ForeColor = Color.FromArgb(228, 95, 1);
      this.chbAndOr.Location = new Point(23, 51);
      this.chbAndOr.Margin = new Padding(2);
      this.chbAndOr.Name = "chbAndOr";
      this.chbAndOr.Size = new Size(40, 17);
      this.chbAndOr.TabIndex = 3;
      this.chbAndOr.Text = "Ou";
      this.chbAndOr.UseVisualStyleBackColor = true;
      this.splitContainer3.Dock = DockStyle.Fill;
      this.splitContainer3.IsSplitterFixed = true;
      this.splitContainer3.Location = new Point(0, 0);
      this.splitContainer3.Name = "splitContainer3";
      this.splitContainer3.Orientation = Orientation.Horizontal;
      this.splitContainer3.Panel1.Controls.Add((Control) this.splitContainer4);
      this.splitContainer3.Panel2.Controls.Add((Control) this.tagsToSearch);
      this.splitContainer3.Size = new Size(559, 221);
      this.splitContainer3.SplitterDistance = 119;
      this.splitContainer3.TabIndex = 0;
      this.splitContainer4.Dock = DockStyle.Fill;
      this.splitContainer4.FixedPanel = FixedPanel.Panel1;
      this.splitContainer4.IsSplitterFixed = true;
      this.splitContainer4.Location = new Point(0, 0);
      this.splitContainer4.Name = "splitContainer4";
      this.splitContainer4.Orientation = Orientation.Horizontal;
      this.splitContainer4.Panel1.Controls.Add((Control) this.cbContents);
      this.splitContainer4.Panel2.Controls.Add((Control) this.ingredientsPanel);
      this.splitContainer4.Size = new Size(559, 119);
      this.splitContainer4.SplitterDistance = 30;
      this.splitContainer4.TabIndex = 1;
      this.cbContents.BackColor = Color.FromArgb(37, 37, 38);
      this.cbContents.Dock = DockStyle.Fill;
      this.cbContents.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cbContents.FlatStyle = FlatStyle.Flat;
      this.cbContents.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cbContents.ForeColor = Color.White;
      this.cbContents.FormattingEnabled = true;
      this.cbContents.Location = new Point(0, 0);
      this.cbContents.Margin = new Padding(2);
      this.cbContents.Name = "cbContents";
      this.cbContents.Size = new Size(559, 28);
      this.cbContents.TabIndex = 0;
      this.ingredientsPanel.AutoScroll = true;
      this.ingredientsPanel.BackColor = Color.FromArgb(37, 40, 42);
      this.ingredientsPanel.BorderStyle = BorderStyle.FixedSingle;
      this.ingredientsPanel.Dock = DockStyle.Fill;
      this.ingredientsPanel.Location = new Point(0, 0);
      this.ingredientsPanel.Name = "ingredientsPanel";
      this.ingredientsPanel.Size = new Size(559, 85);
      this.ingredientsPanel.TabIndex = 0;
      this.tagsToSearch.BackColor = Color.FromArgb(37, 40, 42);
      this.tagsToSearch.BorderStyle = BorderStyle.FixedSingle;
      this.tagsToSearch.Dock = DockStyle.Fill;
      this.tagsToSearch.Location = new Point(0, 0);
      this.tagsToSearch.Margin = new Padding(2, 2, 2, 2);
      this.tagsToSearch.Name = "tagsToSearch";
      this.tagsToSearch.Size = new Size(559, 98);
      this.tagsToSearch.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(37, 40, 42);
      this.Controls.Add((Control) this.pnlAdvancedSearchTags);
      this.Controls.Add((Control) this.SearchHeaderMainPanel);
      this.Margin = new Padding(2);
      this.Name = "SearchHeader";
      this.Size = new Size(669, 284);
      this.pnlTextBoxAndSearch.ResumeLayout(false);
      this.searchBarPanel.ResumeLayout(false);
      this.searchBarPanel.PerformLayout();
      this.SearchButtonPanel.ResumeLayout(false);
      this.SearchHeaderMainPanel.ResumeLayout(false);
      this.AdvancedSearchButtonPanel.ResumeLayout(false);
      this.pnlAdvancedSearchTags.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel1.PerformLayout();
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.Panel2.PerformLayout();
      this.splitContainer2.EndInit();
      this.splitContainer2.ResumeLayout(false);
      this.splitContainer3.Panel1.ResumeLayout(false);
      this.splitContainer3.Panel2.ResumeLayout(false);
      this.splitContainer3.EndInit();
      this.splitContainer3.ResumeLayout(false);
      this.splitContainer4.Panel1.ResumeLayout(false);
      this.splitContainer4.Panel2.ResumeLayout(false);
      this.splitContainer4.EndInit();
      this.splitContainer4.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
