// Decompiled with JetBrains decompiler
// Type: Foody.Controls.RecipeList
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using Foody.Generic;
using Foody.IO;
using Foody.Models;
using Foody.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Foody.Controls
{
    public class RecipeList : UserControl
    {
        private RecipeControl recipeControl;
        private List<int> selectedRecipes = new List<int>();
        private IContainer components = (IContainer)null;
        private Panel panel1;
        private Panel MainListPanel;
        private BindingSource recipeBindDataBindingSource;
        private DataGridView RecipesDataGridView;
        private SplitContainer RecipesCtrlStaffs;
        private SplitContainer RecipesListBody;
        private Button btnAddRecipe;
        private SearchHeader searchHeader1;
        private DataGridViewTextBoxColumn RealIndex;
        private DataGridViewCheckBoxColumn Check;
        private DataGridViewTextBoxColumn Nom;
        private DataGridViewButtonColumn Edit;
        private DataGridViewButtonColumn Delete;

        public RecipeList()
        {
            this.InitializeComponent();
            this.InitEvents();
            this.InitConfigs();
        }

        private void PrepareData()
        {
        }

        private void InitConfigs() => this.searchHeader1.OnReduceSearchBar((object)null, (EventArgs)null);

        private void InitEvents()
        {
            this.searchHeader1.OnExpandSearchBar = (EventHandler)((s, e) =>
           {
               this.RecipesCtrlStaffs.SplitterDistance = this.RecipesCtrlStaffs.SplitterDistance == 50 ? 62 : 290;
               this.RecipesListBody.Panel1Collapsed = true;
           });
            this.searchHeader1.OnReduceSearchBar = (EventHandler)((s, e) =>
           {
               int num = this.RecipesCtrlStaffs.SplitterDistance == 290 ? 62 : 50;
               this.RecipesCtrlStaffs.SplitterDistance = num;
               this.RecipesListBody.Panel1Collapsed = false;
               if (num != 50)
                   return;
               this.searchHeader1.ResetSearch();
           });
            this.RecipesDataGridView.CellClick += (DataGridViewCellEventHandler)((s, e) => this.ListClickEvent(s, e));
            this.RecipesDataGridView.CellDoubleClick += (DataGridViewCellEventHandler)((s, e) => this.ListDblClickEvent(s, e));
            this.RecipesDataGridView.MouseEnter += (EventHandler)((s, e) => this.RecipesDataGridView.Columns[1].Visible = true);
            this.RecipesDataGridView.MouseLeave += (EventHandler)((s, e) => this.RecipesDataGridView.Columns[1].Visible = false);
            this.btnAddRecipe.Click += (EventHandler)((s, e) => this.RecipeEditorForNewCreation());
            this.searchHeader1.Engine.AsyncRefresh += (EventHandler)((s, e) => this.RefreshRecipesList());
            this.RecipesDataGridView.CellPainting += new DataGridViewCellPaintingEventHandler(this.RecipesDataGridView_CellPainting);
        }

        private void RecipesDataGridView_CellPainting(
          object sender,
          DataGridViewCellPaintingEventArgs e)
        {
            Image image = (Image)Resources.unselected;
            if (e.RowIndex != -1 || e.ColumnIndex != 1)
                return;
            if (this.selectedRecipes.Count == 0)
                image = (Image)Resources.unselected;
            else if (this.selectedRecipes.Count >= 0 && this.selectedRecipes.Count < this.RecipesDataGridView.Rows.Count)
                image = (Image)Resources.partialSelect;
            else if (this.selectedRecipes.Count == this.RecipesDataGridView.Rows.Count)
                image = (Image)Resources.selected;
            e.PaintBackground(e.ClipBounds, true);
            e.Graphics.DrawImage(image, this.CalculateCoordinates(e.CellBounds, image));
            e.Handled = true;
        }

        private Rectangle CalculateCoordinates(Rectangle cell, Image img) => new Rectangle()
        {
            Width = img.Width,
            Height = img.Height,
            X = (cell.Width - img.Width) / 2,
            Y = (cell.Height - img.Height) / 2
        };

        private void RefreshRecipesList()
        {
            if (this.searchHeader1.Active)
            {
                if (this.RecipesDataGridView.InvokeRequired)
                    this.RecipesDataGridView.Invoke(new Action(() => this.PopulateDataGridViewWithRecipes(this.searchHeader1.Engine.SearchResult, Database.AllMenus)));
                else
                    this.PopulateDataGridViewWithRecipes(this.searchHeader1.Engine.SearchResult, Database.AllMenus);
            }
            else if (this.RecipesDataGridView.InvokeRequired)
                this.RecipesDataGridView.Invoke(new Action(() => this.PopulateDataGridViewWithRecipes(Database.AllMenus)));
            else
                this.PopulateDataGridViewWithRecipes(Database.AllMenus);
        }

        public void PopulateDataGridViewWithRecipes(List<Recipe> recipes)
        {
            Action<List<Recipe>, DataGridView> action = (Action<List<Recipe>, DataGridView>)((l, v) =>
           {
               for (int index1 = 0; index1 < l.Count; ++index1)
               {
                   int index2 = v.Rows.Add();
                   v.Rows[index2].Cells[0].Value = (object)index1.ToString();
                   v.Rows[index2].Cells[1].Value = (object)false;
                   v.Rows[index2].Cells[2].Value = (object)l[index1].title;
                   v.Rows[index2].Cells[3].Value = (object)"\uD83D\uDDD1";
               }
           });
            this.RecipesDataGridView.Rows.Clear();
            action(recipes, this.RecipesDataGridView);
        }

        public void PopulateDataGridViewWithRecipes(int[] recipesIndexs, List<Recipe> recipes)
        {
            Action<int[], List<Recipe>, DataGridView> action = (Action<int[], List<Recipe>, DataGridView>)((indexs, l, v) =>
           {
               for (int index1 = 0; index1 < indexs.Length; ++index1)
               {
                   int index2 = v.Rows.Add();
                   int index3 = indexs[index1];
                   v.Rows[index2].Cells[0].Value = (object)index3.ToString();
                   v.Rows[index2].Cells[1].Value = (object)false;
                   v.Rows[index2].Cells[2].Value = (object)l[index3].title;
                   v.Rows[index2].Cells[3].Value = (object)"\uD83D\uDDD1";
               }
           });
            this.RecipesDataGridView.Rows.Clear();
            action(recipesIndexs, recipes, this.RecipesDataGridView);
        }


        private void SelectRecipe(int index, bool select)
        {
            bool flag = (bool)this.RecipesDataGridView.Rows[index].Cells[1].Value;
            int num = int.Parse((string)this.RecipesDataGridView.Rows[index].Cells[0].Value);
            if (flag)
            {
                if (this.selectedRecipes.Contains(num))
                    this.selectedRecipes.Remove(num);
            }
            else
                this.selectedRecipes.Add(num);
            this.RecipesDataGridView.Rows[index].Cells[1].Value = (object)select;
        }

        private void SelectAll(bool select)
        {
            if (!select)
                this.selectedRecipes.Clear();
            for (int index = 0; index < this.RecipesDataGridView.Rows.Count; ++index)
            {
                this.RecipesDataGridView.Rows[index].Cells[1].Value = (object)select;
                int num = int.Parse(this.RecipesDataGridView.Rows[index].Cells[0].Value.ToString());
                if (select && !this.selectedRecipes.Contains(num))
                    this.selectedRecipes.Add(num);
            }
        }

        private void SelectAll()
        {
            if (this.selectedRecipes.Count == 0)
                this.SelectAll(true);
            else if (this.selectedRecipes.Count >= 0 && this.selectedRecipes.Count < this.RecipesDataGridView.Rows.Count)
                this.SelectAll(true);
            else if (this.selectedRecipes.Count == this.RecipesDataGridView.Rows.Count)
                this.SelectAll(false);
            this.RecipesDataGridView.Refresh();
        }

        private void ListClickEvent(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                this.SelectAll();
            if (e.RowIndex >= this.RecipesDataGridView.Rows.Count || e.RowIndex < 0 || this.RecipesDataGridView.Rows[e.RowIndex].Cells[0].Value == null)
                return;
            int num = int.Parse((string)this.RecipesDataGridView.Rows[e.RowIndex].Cells[0].Value);
            switch (e.ColumnIndex)
            {
                case 1:
                    this.SelectRecipe(e.RowIndex, !(bool)this.RecipesDataGridView.Rows[e.RowIndex].Cells[1].Value);
                    this.RecipesDataGridView.Refresh();
                    break;
                case 3:
                    if (!Database.ValidateRecipeExistance(num))
                        break;
                    Database.RemoveRecipeFromDatabase(num);
                    this.searchHeader1.RefreshSearchEngine();
                    break;
            }
        }


        private void ListDblClickEvent(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                this.SelectAll();
            if (e.RowIndex >= this.RecipesDataGridView.Rows.Count || e.RowIndex < 0 || this.RecipesDataGridView.Rows[e.RowIndex].Cells[0].Value == null)
                return;
            int num = int.Parse((string)this.RecipesDataGridView.Rows[e.RowIndex].Cells[0].Value);

            this.RecipeEditorByDbIndex(num);
        }


        private void RecipeEditorByDbIndex(int idx)
        {
            Form r = new Form();
            RecipeControl rc = new RecipeControl(idx);
            r.Controls.Add(rc);
            rc.Dock = DockStyle.Fill;
            rc.GoBack += new Delegates.Back(() => r.Close());
            r.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width / 2, Screen.PrimaryScreen.WorkingArea.Height);
            r.Show();
        }

        private void RecipeEditorForNewCreation()
        {
            Form r = new Form();
            RecipeControl rc = new RecipeControl(-1, true);
            r.Controls.Add(rc);
            rc.Dock = DockStyle.Fill;
            rc.Visible = true;
            rc.GoBack += new Delegates.Back(() => r.Close());
            r.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width / 2, Screen.PrimaryScreen.WorkingArea.Height);
            r.Show();
        }

        public int[] GetSelectedRecipesIndexs => this.selectedRecipes.Count > 0 ? this.selectedRecipes.ToArray() : (int[])null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = (IContainer)new Container();
            DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
            this.panel1 = new Panel();
            this.MainListPanel = new Panel();
            this.RecipesCtrlStaffs = new SplitContainer();
            this.RecipesListBody = new SplitContainer();
            this.btnAddRecipe = new Button();
            this.RecipesDataGridView = new DataGridView();
            this.RealIndex = new DataGridViewTextBoxColumn();
            this.Check = new DataGridViewCheckBoxColumn();
            this.Nom = new DataGridViewTextBoxColumn();
            this.Delete = new DataGridViewButtonColumn();
            this.recipeBindDataBindingSource = new BindingSource(this.components);
            this.searchHeader1 = new SearchHeader();
            this.panel1.SuspendLayout();
            this.MainListPanel.SuspendLayout();
            this.RecipesCtrlStaffs.BeginInit();
            this.RecipesCtrlStaffs.Panel1.SuspendLayout();
            this.RecipesCtrlStaffs.Panel2.SuspendLayout();
            this.RecipesCtrlStaffs.SuspendLayout();
            this.RecipesListBody.BeginInit();
            this.RecipesListBody.Panel1.SuspendLayout();
            this.RecipesListBody.Panel2.SuspendLayout();
            this.RecipesListBody.SuspendLayout();
            ((ISupportInitialize)this.RecipesDataGridView).BeginInit();
            ((ISupportInitialize)this.recipeBindDataBindingSource).BeginInit();
            this.SuspendLayout();
            this.panel1.BackColor = Color.FromArgb(37, 40, 42);
            this.panel1.Controls.Add((Control)this.searchHeader1);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Margin = new Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(761, 48);
            this.panel1.TabIndex = 0;
            this.MainListPanel.Controls.Add((Control)this.RecipesCtrlStaffs);
            this.MainListPanel.Dock = DockStyle.Fill;
            this.MainListPanel.Location = new Point(0, 0);
            this.MainListPanel.Margin = new Padding(2);
            this.MainListPanel.Name = "MainListPanel";
            this.MainListPanel.Size = new Size(763, 429);
            this.MainListPanel.TabIndex = 1;
            this.RecipesCtrlStaffs.BorderStyle = BorderStyle.FixedSingle;
            this.RecipesCtrlStaffs.Dock = DockStyle.Fill;
            this.RecipesCtrlStaffs.FixedPanel = FixedPanel.Panel1;
            this.RecipesCtrlStaffs.IsSplitterFixed = true;
            this.RecipesCtrlStaffs.Location = new Point(0, 0);
            this.RecipesCtrlStaffs.Name = "RecipesCtrlStaffs";
            this.RecipesCtrlStaffs.Orientation = Orientation.Horizontal;
            this.RecipesCtrlStaffs.Panel1.Controls.Add((Control)this.panel1);
            this.RecipesCtrlStaffs.Panel2.Controls.Add((Control)this.RecipesListBody);
            this.RecipesCtrlStaffs.Size = new Size(763, 429);
            this.RecipesCtrlStaffs.TabIndex = 1;
            this.RecipesListBody.Dock = DockStyle.Fill;
            this.RecipesListBody.FixedPanel = FixedPanel.Panel1;
            this.RecipesListBody.IsSplitterFixed = true;
            this.RecipesListBody.Location = new Point(0, 0);
            this.RecipesListBody.Name = "RecipesListBody";
            this.RecipesListBody.Orientation = Orientation.Horizontal;
            this.RecipesListBody.Panel1.Controls.Add((Control)this.btnAddRecipe);
            this.RecipesListBody.Panel2.Controls.Add((Control)this.RecipesDataGridView);
            this.RecipesListBody.Size = new Size(761, 373);
            this.RecipesListBody.SplitterDistance = 27;
            this.RecipesListBody.TabIndex = 1;
            this.btnAddRecipe.Dock = DockStyle.Right;
            this.btnAddRecipe.FlatAppearance.BorderColor = Color.FromArgb(100, 100, 100);
            this.btnAddRecipe.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 100, 100);
            this.btnAddRecipe.FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 100, 100);
            this.btnAddRecipe.FlatStyle = FlatStyle.Flat;
            this.btnAddRecipe.Font = new Font("Microsoft Sans Serif", 10.2f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.btnAddRecipe.ForeColor = Color.FromArgb(229, 9, 20);
            this.btnAddRecipe.Location = new Point(713, 0);
            this.btnAddRecipe.Margin = new Padding(2);
            this.btnAddRecipe.Name = "btnAddRecipe";
            this.btnAddRecipe.Size = new Size(48, 27);
            this.btnAddRecipe.TabIndex = 4;
            this.btnAddRecipe.Text = "➕";
            this.btnAddRecipe.UseVisualStyleBackColor = true;
            this.RecipesDataGridView.AllowUserToAddRows = false;
            this.RecipesDataGridView.AllowUserToDeleteRows = false;
            this.RecipesDataGridView.AllowUserToResizeColumns = false;
            this.RecipesDataGridView.AllowUserToResizeRows = false;
            gridViewCellStyle1.BackColor = Color.FromArgb(37, 37, 38);
            gridViewCellStyle1.ForeColor = Color.White;
            gridViewCellStyle1.SelectionBackColor = Color.FromArgb(37, 37, 38);
            gridViewCellStyle1.SelectionForeColor = Color.White;
            this.RecipesDataGridView.AlternatingRowsDefaultCellStyle = gridViewCellStyle1;
            this.RecipesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.RecipesDataGridView.BackgroundColor = Color.FromArgb(37, 40, 42);
            this.RecipesDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridViewCellStyle2.BackColor = Color.FromArgb(37, 37, 38);
            gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            gridViewCellStyle2.ForeColor = Color.White;
            gridViewCellStyle2.SelectionBackColor = Color.FromArgb(37, 37, 38);
            gridViewCellStyle2.SelectionForeColor = Color.White;
            gridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            this.RecipesDataGridView.ColumnHeadersDefaultCellStyle = gridViewCellStyle2;
            this.RecipesDataGridView.ColumnHeadersHeight = 40;
            this.RecipesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.RecipesDataGridView.Columns.AddRange((DataGridViewColumn)this.RealIndex, (DataGridViewColumn)this.Check, (DataGridViewColumn)this.Nom, (DataGridViewColumn)this.Delete);
            this.RecipesDataGridView.Dock = DockStyle.Fill;
            this.RecipesDataGridView.EnableHeadersVisualStyles = false;
            this.RecipesDataGridView.GridColor = Color.White;
            this.RecipesDataGridView.Location = new Point(0, 0);
            this.RecipesDataGridView.Margin = new Padding(2);
            this.RecipesDataGridView.MultiSelect = false;
            this.RecipesDataGridView.Name = "RecipesDataGridView";
            this.RecipesDataGridView.ReadOnly = true;
            this.RecipesDataGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.RecipesDataGridView.RowHeadersVisible = false;
            this.RecipesDataGridView.RowHeadersWidth = 51;
            gridViewCellStyle4.BackColor = Color.FromArgb(37, 40, 42);
            gridViewCellStyle4.ForeColor = Color.White;
            gridViewCellStyle4.SelectionBackColor = Color.FromArgb(37, 40, 42);
            gridViewCellStyle4.SelectionForeColor = Color.White;
            this.RecipesDataGridView.RowsDefaultCellStyle = gridViewCellStyle4;
            this.RecipesDataGridView.RowTemplate.Height = 24;
            this.RecipesDataGridView.Size = new Size(761, 342);
            this.RecipesDataGridView.TabIndex = 0;
            this.RealIndex.HeaderText = "";
            this.RealIndex.Name = "RealIndex";
            this.RealIndex.ReadOnly = true;
            this.RealIndex.Visible = false;
            this.RealIndex.Width = 17;
            this.Check.HeaderText = "";
            this.Check.MinimumWidth = 20;
            this.Check.Name = "Check";
            this.Check.ReadOnly = true;
            this.Check.Visible = false;
            this.Check.Width = 20;
            this.Nom.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Nom.HeaderText = "Nom";
            this.Nom.MinimumWidth = 6;
            this.Nom.Name = "Nom";
            this.Nom.ReadOnly = true;
            this.Nom.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.Delete.FlatStyle = FlatStyle.Flat;
            this.Delete.HeaderText = "Supprimer";
            this.Delete.MinimumWidth = 6;
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Width = 58;
            this.recipeBindDataBindingSource.DataSource = (object)typeof(RecipeBindData);
            this.searchHeader1.BackColor = Color.FromArgb(37, 40, 42);
            this.searchHeader1.Dock = DockStyle.Fill;
            this.searchHeader1.Location = new Point(0, 0);
            this.searchHeader1.Margin = new Padding(2);
            this.searchHeader1.Name = "searchHeader1";
            this.searchHeader1.Size = new Size(761, 48);
            this.searchHeader1.TabIndex = 2;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(37, 40, 42);
            this.Controls.Add((Control)this.MainListPanel);
            this.Margin = new Padding(2);
            this.Name = "RecipeList";
            this.Size = new Size(763, 429);
            this.panel1.ResumeLayout(false);
            this.MainListPanel.ResumeLayout(false);
            this.RecipesCtrlStaffs.Panel1.ResumeLayout(false);
            this.RecipesCtrlStaffs.Panel2.ResumeLayout(false);
            this.RecipesCtrlStaffs.EndInit();
            this.RecipesCtrlStaffs.ResumeLayout(false);
            this.RecipesListBody.Panel1.ResumeLayout(false);
            this.RecipesListBody.Panel2.ResumeLayout(false);
            this.RecipesListBody.EndInit();
            this.RecipesListBody.ResumeLayout(false);
            ((ISupportInitialize)this.RecipesDataGridView).EndInit();
            ((ISupportInitialize)this.recipeBindDataBindingSource).EndInit();
            this.ResumeLayout(false);
        }
    }
}
