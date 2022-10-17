// Decompiled with JetBrains decompiler
// Type: Foody.Controls.RecipeControl
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using Foody.Generic;
using Foody.IO;
using Foody.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Foody.Controls
{
    public class RecipeControl : UserControl
    {
        private bool IsSaving = false;
        private bool IsCommitted = true;
        private int recipeIndex = -1;
        private bool editMode = false;
        private Recipe currentRecipe;
        private bool IsNew = false;
        private TagConverter tagConverter = new TagConverter();
        private UnitConverter UnitConverter = new UnitConverter();
        public Delegates.Back GoBack;
        private IContainer components = (IContainer)null;
        private Label lblName;
        private Button btnEdit;
        private Panel header;
        private Label lblPepeole;
        private Label lblTags;
        private Label lblContents;
        private TextBox tbPepoleNumber;
        private TextBox tbName;
        private Button btnEditTags;
        private Panel RecipeMainPanelControl;
        private Button btnAddContent;
        private Button btnBack;
        private DataGridView dgvContents;
        private Panel panel1;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
        private SplitContainer spltContTagsControl;
        private FlowLayoutPanel NotSelected;
        private SplitContainer splitContainer4;
        private SplitContainer splitContainer5;
        private SplitContainer splitContainer6;
        private SplitContainer splitContainer7;
        private SplitContainer splitContainer8;
        private SplitContainer splitContainer9;
        private SplitContainer splitContainer10;
        private TagsLabelsControl tagsLabelsController;
        private Button btnAddNewContent;
        private SplitContainer splitContainer3;
        private RichTextBox rtbDescription;
        private DataGridViewTextBoxColumn Nom;
        private DataGridViewTextBoxColumn quantity;
        private DataGridViewTextBoxColumn UnitContent;
        private DataGridViewButtonColumn edit;
        private DataGridViewButtonColumn remove;
        private DataGridViewTextBoxColumn contentIndex;
        private DataGridViewCheckBoxColumn UnderEdit;

        public RecipeControl(int idx, bool isNew = false)
        {
            this.InitializeComponent();
            this.InitEvents();

            if (!isNew && idx >= 0 && idx < Database.AllMenus.Count)
            {
                this.recipeIndex = idx;
                this.currentRecipe = (Recipe)Database.AllMenus[idx].Clone();
            }
            else
            {
                currentRecipe = new Recipe();
                currentRecipe.PeopleNumber = 2;
            }

            this.IsNew = isNew;
            this.PopulateWithRecipe();
            this.EnterEditMode(true);
        }

        private void PopulateWithRecipe()
        {
            this.lblName.Text = this.currentRecipe.title;
            this.lblPepeole.Text = string.Format("{0} Personnes", (object)this.currentRecipe.PeopleNumber);
            this.rtbDescription.Text = this.currentRecipe.Description;
            this.PopulateTags();
            this.PopulateContents((RecipeContent[])this.currentRecipe.GetContents().ToArray().Clone());
        }

        private void RefreshRecipe()
        {
            this.dgvContents.Rows.Clear();
            this.PopulateContents((RecipeContent[])this.currentRecipe.GetContents().ToArray().Clone());
        }

        private void PopulateContents(RecipeContent[] contents)
        {
            if (contents == null || contents.Length == 0)
                return;
            this.dgvContents.Columns[3].Visible = this.editMode;
            this.dgvContents.Columns[4].Visible = this.editMode;
            for (int index = 0; index < contents.Length; ++index)
            {
                string name = Tools.FindContentByUid(contents[index].uid).Name;
                this.dgvContents.Rows.Add();
                this.dgvContents.Rows[index].Cells[0].Value = (object)name;
                this.dgvContents.Rows[index].Cells[1].Value = (object)contents[index].Quantity.ToString();
                this.dgvContents.Rows[index].Cells[2].Value = (object)this.UnitConverter.GetString(contents[index].QuantityUnit);
                this.dgvContents.Rows[index].Cells[6].Value = (object)false;
                this.dgvContents.Rows[index].Cells[5].Value = (object)index;
                this.dgvContents.Rows[index].Cells[3].Value = (bool)this.dgvContents.Rows[index].Cells[6].Value ? (object)"\uD83D\uDCBE" : (object)"✏";
                this.dgvContents.Rows[index].Cells[4].Value = (bool)this.dgvContents.Rows[index].Cells[6].Value ? (object)"❌" : (object)"\uD83D\uDDD1";
            }
        }

        private void PopulateTags() => this.tagsLabelsController.SetControlsItems(0, this.currentRecipe.Tags.ToArray());

        private void EnterEditMode(bool edit)
        {
            if (!edit)
            {
                this.currentRecipe.title = this.tbName.Text;
                this.currentRecipe.PeopleNumber = int.Parse(this.tbPepoleNumber.Text);
                this.currentRecipe.Description = this.rtbDescription.Text;
                this.Save(this.IsNew);
            }
            this.tbPepoleNumber.Text = edit ? this.lblPepeole.Text.Replace("Personnes", "") : this.tbPepoleNumber.Text + " Personnes";
            this.lblPepeole.Text = "Personnes";
            this.tbName.Text = this.lblName.Text;
            this.lblName.Visible = !edit;
            this.lblName.Text = this.currentRecipe.title;
            this.tbName.Text = this.currentRecipe.title;
            this.IsSaving = this.btnAddNewContent.Visible = this.btnAddContent.Visible = this.tbPepoleNumber.Visible = this.tbName.Visible = this.btnEditTags.Visible = edit;
            this.btnEdit.Text = edit ? "\uD83D\uDCBE" : "✏";
            this.rtbDescription.ReadOnly = !edit;
            this.rtbDescription.Text = this.currentRecipe.Description;
            this.rtbDescription.Enabled = edit;
            this.editMode = edit;

            if (this.dgvContents.Columns.Count > 4)
            {
                this.dgvContents.Columns[3].Visible = this.editMode;
                this.dgvContents.Columns[4].Visible = this.editMode;
            }
        }

        private void InitEvents()
        {

            this.btnAddNewContent.Click += ((s, e) =>
            {
                AddContentPopup popup = new AddContentPopup();
                popup.ShowPopup();

            });

            this.btnEdit.Click += (EventHandler)((s, e) =>
           {
               if (!this.IsSaving)
               {
                   this.EnterEditMode(true);
               }
               else
               {
                   this.EnterEditMode(false);
                   this.ShowTagsControls(false);
               }
           });
            this.btnEditTags.Click += (EventHandler)((s, e) =>
           {
               if (int.Parse((string)this.btnEditTags.Tag) == 0)
               {
                   this.ShowTagsControls(true);
                   this.btnEditTags.Tag = (object)"1";
                   this.btnEditTags.Text = "\uD83D\uDCBE";
               }
               else
               {
                   this.ShowTagsControls(false);
                   this.btnEditTags.Tag = (object)"0";
                   this.btnEditTags.Text = "✏";
                   this.UpdateTags();
               }
           });
            this.btnAddContent.Click += (EventHandler)((s, e) =>
           {
               if (!this.IsCommitted)
               {
                   this.btnAddContent.Text = "➕";
                   this.RefreshRecipe();
                   this.dgvContents.CellMouseClick += new DataGridViewCellMouseEventHandler(this.DgvContents_CellMouseClick);
                   this.IsCommitted = true;
               }
               else
               {
                   this.dgvContents.CellMouseClick -= new DataGridViewCellMouseEventHandler(this.DgvContents_CellMouseClick);
                   int contentId = this.dgvContents.Rows.Add();
                   string[] array = Database.contents.Select<Content, string>((Func<Content, string>)(c => c.Name)).ToArray<string>();
                   this.btnAddContent.Text = "❌";
                   this.EnterCellEditMode(contentId, array);
                   this.dgvContents.CellMouseClick += (DataGridViewCellMouseEventHandler)((sender, evt) =>
                   {
                       if (evt.RowIndex == contentId && evt.ColumnIndex == 4)
                       {
                           if (this.IsCommitted)
                               return;
                           this.btnAddContent.Text = "➕";
                           this.RefreshRecipe();
                           this.dgvContents.CellMouseClick += new DataGridViewCellMouseEventHandler(this.DgvContents_CellMouseClick);
                           this.IsCommitted = true;
                       }
                       else
                       {
                           if (evt.RowIndex != contentId || evt.ColumnIndex != 3)
                               return;
                           this.IsCommitted = true;
                           RecipeContent recipeContent = new RecipeContent();
                           string uid = Database.contents.FirstOrDefault<Content>((Func<Content, bool>)(x => x.Name == this.dgvContents.Rows[contentId].Cells[0].Value.ToString())).uid;
                           Unit fromString = this.UnitConverter.GetFromString(this.dgvContents.Rows[contentId].Cells[2].Value?.ToString());
                           double num = double.Parse(this.dgvContents.Rows[contentId].Cells[1].Value.ToString());
                           if (string.IsNullOrEmpty(uid))
                               return;
                           recipeContent.uid = uid;
                           recipeContent.QuantityUnit = fromString;
                           recipeContent.Quantity = num;
                           this.currentRecipe.Contents.Add(recipeContent);
                           this.RefreshRecipe();
                           this.dgvContents.CellMouseClick += new DataGridViewCellMouseEventHandler(this.DgvContents_CellMouseClick);
                       }
                   });
                   this.IsCommitted = false;
               }
           });
            this.dgvContents.CellMouseClick += new DataGridViewCellMouseEventHandler(this.DgvContents_CellMouseClick);
            this.btnBack.Click += (EventHandler)((s, e) =>
            {
                this.currentRecipe = (Recipe)null;
                if (this.GoBack == null)
                    return;
                this.GoBack();
            });
        }

        private void EnterCellEditMode(int erow, string[] names, int index = -1)
        {
            this.dgvContents.Rows[erow].Cells[0].ReadOnly = false;
            this.dgvContents.Rows[erow].Cells[1].ReadOnly = false;
            this.dgvContents.Rows[erow].Cells[2].ReadOnly = false;
            this.dgvContents.Rows[erow].Cells[0] = (DataGridViewCell)ControlHelpers.CreateStyledComboBox(names);
            this.dgvContents.Rows[erow].Cells[1] = (DataGridViewCell)ControlHelpers.CreateStyledTextBoxCell();
            this.dgvContents.Rows[erow].Cells[2] = (DataGridViewCell)ControlHelpers.CreateStyledComboBox(Consts.units);
            if (index > -1 && index < names.Length)
            {
                this.dgvContents.Rows[erow].Cells[0].Value = (object)names[index];
                this.dgvContents.Rows[erow].Cells[1].Value = (object)this.currentRecipe.Contents[erow].Quantity;
                this.dgvContents.Rows[erow].Cells[2].Value = (object)Consts.units[(int)this.currentRecipe.Contents[erow].QuantityUnit];
            }
            this.dgvContents.Rows[erow].Cells[3] = (DataGridViewCell)ControlHelpers.CreateStyledButtonCell("\uD83D\uDCBE");
            this.dgvContents.Rows[erow].Cells[4] = (DataGridViewCell)ControlHelpers.CreateStyledButtonCell("❌");
        }

        private void DgvContents_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.currentRecipe.Contents.Count <= e.RowIndex || e.RowIndex < 0)
                return;
            int index = int.Parse(this.dgvContents.Rows[e.RowIndex].Cells[5].Value.ToString());
            bool isCancel = (bool)this.dgvContents.Rows[e.RowIndex].Cells[6].Value;
            switch (e.ColumnIndex)
            {
                case 3:
                    this.EditOrSave(index);
                    break;
                case 4:
                    this.RemoveOrCancel(index, isCancel);
                    this.RefreshRecipe();
                    break;
            }
        }

        private void EditOrSave(int index)
        {
            if (!(bool)this.dgvContents.Rows[index].Cells[6].Value)
            {
                string[] array = Database.contents.Select<Content, string>((Func<Content, string>)(c => c.Name)).ToArray<string>();
                int contentIndexByUid = Tools.FindContentIndexByUid(this.currentRecipe.Contents[index].uid);
                this.EnterCellEditMode(index, array, contentIndexByUid);
                this.dgvContents.Rows[index].Cells[6].Value = (object)true;
            }
            else
            {
                string uid = Database.contents.FirstOrDefault<Content>((Func<Content, bool>)(x => x.Name == this.dgvContents.Rows[index].Cells[0].Value.ToString())).uid;
                Unit fromString = this.UnitConverter.GetFromString(this.dgvContents.Rows[index].Cells[2].Value.ToString());
                double num = double.Parse(this.dgvContents.Rows[index].Cells[1].Value.ToString());
                if (!string.IsNullOrEmpty(uid))
                {
                    this.currentRecipe.Contents[index].uid = uid;
                    this.currentRecipe.Contents[index].Quantity = num;
                    this.currentRecipe.Contents[index].QuantityUnit = fromString;
                }
                this.dgvContents.Rows[index].Cells[6].Value = (object)false;
                this.RefreshRecipe();
            }
        }

        private void RemoveOrCancel(int index, bool isCancel)
        {
            if (isCancel)
                this.dgvContents.Rows[index].Cells[6].Value = (object)false;
            else
                this.currentRecipe.Contents.RemoveAt(index);
        }

        private void Save(bool isNew)
        {
            if (isNew)
            {
                Database.AddRecipeToDatabase(this.currentRecipe);
                if (this.GoBack == null)
                    return;
                this.GoBack();
            }
            else
            {
                Database.AddRecipeToDatabase(this.currentRecipe, true);
                if (this.GoBack == null)
                    return;
                this.GoBack();
            }
        }

        private void UpdateTags()
        {
            this.currentRecipe.Tags.Clear();
            this.currentRecipe.Tags.AddRange((IEnumerable<Tag>)this.tagsLabelsController.GetTags());
        }

        private void ShowTagsControls(bool show) => this.tagsLabelsController.SetMask(show ? 2 : 0);

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
            this.btnAddContent = new Button();
            this.dgvContents = new DataGridView();
            this.lblContents = new Label();
            this.lblTags = new Label();
            this.lblPepeole = new Label();
            this.lblName = new Label();
            this.btnEditTags = new Button();
            this.tbPepoleNumber = new TextBox();
            this.tbName = new TextBox();
            this.header = new Panel();
            this.btnBack = new Button();
            this.btnEdit = new Button();
            this.RecipeMainPanelControl = new Panel();
            this.splitContainer10 = new SplitContainer();
            this.splitContainer4 = new SplitContainer();
            this.splitContainer5 = new SplitContainer();
            this.panel1 = new Panel();
            this.splitContainer1 = new SplitContainer();
            this.splitContainer2 = new SplitContainer();
            this.spltContTagsControl = new SplitContainer();
            this.NotSelected = new FlowLayoutPanel();
            this.splitContainer6 = new SplitContainer();
            this.btnAddNewContent = new Button();
            this.splitContainer7 = new SplitContainer();
            this.splitContainer8 = new SplitContainer();
            this.splitContainer9 = new SplitContainer();
            this.splitContainer3 = new SplitContainer();
            this.rtbDescription = new RichTextBox();
            this.Nom = new DataGridViewTextBoxColumn();
            this.quantity = new DataGridViewTextBoxColumn();
            this.UnitContent = new DataGridViewTextBoxColumn();
            this.edit = new DataGridViewButtonColumn();
            this.remove = new DataGridViewButtonColumn();
            this.contentIndex = new DataGridViewTextBoxColumn();
            this.UnderEdit = new DataGridViewCheckBoxColumn();
            this.tagsLabelsController = new TagsLabelsControl();
            ((ISupportInitialize)this.dgvContents).BeginInit();
            this.header.SuspendLayout();
            this.RecipeMainPanelControl.SuspendLayout();
            this.splitContainer10.BeginInit();
            this.splitContainer10.Panel1.SuspendLayout();
            this.splitContainer10.Panel2.SuspendLayout();
            this.splitContainer10.SuspendLayout();
            this.splitContainer4.BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.splitContainer5.BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.splitContainer1.BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.spltContTagsControl.BeginInit();
            this.spltContTagsControl.Panel1.SuspendLayout();
            this.spltContTagsControl.Panel2.SuspendLayout();
            this.spltContTagsControl.SuspendLayout();
            this.splitContainer6.BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.splitContainer7.BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.splitContainer8.BeginInit();
            this.splitContainer8.Panel2.SuspendLayout();
            this.splitContainer8.SuspendLayout();
            this.splitContainer9.BeginInit();
            this.splitContainer9.Panel1.SuspendLayout();
            this.splitContainer9.SuspendLayout();
            this.splitContainer3.BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            this.btnAddContent.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            this.btnAddContent.FlatStyle = FlatStyle.Flat;
            this.btnAddContent.ForeColor = Color.FromArgb(228, 95, 1);
            this.btnAddContent.Location = new Point(65, 3);
            this.btnAddContent.Margin = new Padding(2);
            this.btnAddContent.Name = "btnAddContent";
            this.btnAddContent.Size = new Size(22, 26);
            this.btnAddContent.TabIndex = 15;
            this.btnAddContent.Text = "➕";
            this.btnAddContent.UseVisualStyleBackColor = true;
            this.btnAddContent.Visible = false;
            this.dgvContents.AllowUserToResizeColumns = false;
            this.dgvContents.AllowUserToResizeRows = false;
            this.dgvContents.BackgroundColor = Color.FromArgb(37, 40, 42);
            this.dgvContents.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridViewCellStyle1.BackColor = Color.FromArgb(37, 40, 42);
            gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            gridViewCellStyle1.ForeColor = Color.White;
            gridViewCellStyle1.SelectionBackColor = Color.FromArgb(37, 40, 42);
            gridViewCellStyle1.SelectionForeColor = Color.White;
            gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.dgvContents.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
            this.dgvContents.ColumnHeadersHeight = 32;
            this.dgvContents.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvContents.Columns.AddRange((DataGridViewColumn)this.Nom, (DataGridViewColumn)this.quantity, (DataGridViewColumn)this.UnitContent, (DataGridViewColumn)this.edit, (DataGridViewColumn)this.remove, (DataGridViewColumn)this.contentIndex, (DataGridViewColumn)this.UnderEdit);
            gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            gridViewCellStyle2.BackColor = Color.FromArgb(37, 40, 42);
            gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            gridViewCellStyle2.ForeColor = Color.White;
            gridViewCellStyle2.SelectionBackColor = Color.FromArgb(37, 40, 42);
            gridViewCellStyle2.SelectionForeColor = Color.White;
            gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.dgvContents.DefaultCellStyle = gridViewCellStyle2;
            this.dgvContents.Dock = DockStyle.Fill;
            this.dgvContents.EnableHeadersVisualStyles = false;
            this.dgvContents.GridColor = Color.White;
            this.dgvContents.Location = new Point(0, 0);
            this.dgvContents.Margin = new Padding(15, 16, 15, 16);
            this.dgvContents.Name = "dgvContents";
            this.dgvContents.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            this.dgvContents.RowHeadersVisible = false;
            this.dgvContents.RowHeadersWidth = 51;
            gridViewCellStyle3.BackColor = Color.FromArgb(37, 37, 38);
            gridViewCellStyle3.ForeColor = Color.White;
            gridViewCellStyle3.SelectionBackColor = Color.FromArgb(37, 37, 38);
            gridViewCellStyle3.SelectionForeColor = Color.White;
            this.dgvContents.RowsDefaultCellStyle = gridViewCellStyle3;
            this.dgvContents.Size = new Size(701, 63);
            this.dgvContents.TabIndex = 0;
            this.lblContents.AutoSize = true;
            this.lblContents.ForeColor = Color.White;
            this.lblContents.Location = new Point(2, 10);
            this.lblContents.Margin = new Padding(2, 0, 2, 0);
            this.lblContents.Name = "lblContents";
            this.lblContents.Size = new Size(59, 13);
            this.lblContents.TabIndex = 7;
            this.lblContents.Text = "Ingrédients";
            this.lblTags.AutoSize = true;
            this.lblTags.ForeColor = Color.White;
            this.lblTags.Location = new Point(2, 12);
            this.lblTags.Margin = new Padding(2, 0, 2, 0);
            this.lblTags.Name = "lblTags";
            this.lblTags.Size = new Size(31, 13);
            this.lblTags.TabIndex = 6;
            this.lblTags.Text = "Tags";
            this.lblPepeole.AutoSize = true;
            this.lblPepeole.ForeColor = Color.White;
            this.lblPepeole.Location = new Point(476, 13);
            this.lblPepeole.Margin = new Padding(2, 0, 2, 0);
            this.lblPepeole.Name = "lblPepeole";
            this.lblPepeole.Size = new Size(57, 13);
            this.lblPepeole.TabIndex = 4;
            this.lblPepeole.Text = "Personnes";
            this.lblName.AutoSize = true;
            this.lblName.ForeColor = Color.White;
            this.lblName.Location = new Point(16, 13);
            this.lblName.Margin = new Padding(2, 0, 2, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new Size(35, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            this.btnEditTags.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            this.btnEditTags.FlatStyle = FlatStyle.Flat;
            this.btnEditTags.ForeColor = Color.FromArgb(228, 95, 1);
            this.btnEditTags.Location = new Point(68, 6);
            this.btnEditTags.Margin = new Padding(2);
            this.btnEditTags.Name = "btnEditTags";
            this.btnEditTags.Size = new Size(28, 26);
            this.btnEditTags.TabIndex = 14;
            this.btnEditTags.Tag = (object)"0";
            this.btnEditTags.Text = "✏";
            this.btnEditTags.UseVisualStyleBackColor = true;
            this.btnEditTags.Visible = false;
            this.tbPepoleNumber.BackColor = Color.FromArgb(37, 37, 38);
            this.tbPepoleNumber.ForeColor = Color.White;
            this.tbPepoleNumber.Location = new Point(444, 11);
            this.tbPepoleNumber.Margin = new Padding(2);
            this.tbPepoleNumber.Name = "tbPepoleNumber";
            this.tbPepoleNumber.Size = new Size(29, 20);
            this.tbPepoleNumber.TabIndex = 13;
            this.tbPepoleNumber.TextAlign = HorizontalAlignment.Center;
            this.tbPepoleNumber.Visible = false;
            this.tbName.BackColor = Color.FromArgb(37, 37, 38);
            this.tbName.ForeColor = Color.White;
            this.tbName.Location = new Point(19, 11);
            this.tbName.Margin = new Padding(2);
            this.tbName.Name = "tbName";
            this.tbName.Size = new Size(422, 20);
            this.tbName.TabIndex = 12;
            this.tbName.Visible = false;
            this.header.BorderStyle = BorderStyle.FixedSingle;
            this.header.Controls.Add((Control)this.btnBack);
            this.header.Controls.Add((Control)this.btnEdit);
            this.header.Dock = DockStyle.Top;
            this.header.Location = new Point(0, 0);
            this.header.Margin = new Padding(2);
            this.header.Name = "header";
            this.header.Size = new Size(760, 45);
            this.header.TabIndex = 3;
            this.btnBack.Dock = DockStyle.Right;
            this.btnBack.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            this.btnBack.FlatStyle = FlatStyle.Flat;
            this.btnBack.ForeColor = Color.FromArgb(228, 95, 1);
            this.btnBack.Location = new Point(702, 0);
            this.btnBack.Margin = new Padding(2);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new Size(28, 43);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "\uD83D\uDC48";
            this.btnBack.TextAlign = ContentAlignment.MiddleRight;
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnEdit.Dock = DockStyle.Right;
            this.btnEdit.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            this.btnEdit.FlatStyle = FlatStyle.Flat;
            this.btnEdit.ForeColor = Color.FromArgb(228, 95, 1);
            this.btnEdit.Location = new Point(730, 0);
            this.btnEdit.Margin = new Padding(2);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(28, 43);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "✏";
            this.btnEdit.TextAlign = ContentAlignment.MiddleRight;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.RecipeMainPanelControl.Controls.Add((Control)this.splitContainer10);
            this.RecipeMainPanelControl.Dock = DockStyle.Fill;
            this.RecipeMainPanelControl.Location = new Point(0, 45);
            this.RecipeMainPanelControl.Margin = new Padding(2);
            this.RecipeMainPanelControl.Name = "RecipeMainPanelControl";
            this.RecipeMainPanelControl.Size = new Size(760, 381);
            this.RecipeMainPanelControl.TabIndex = 15;
            this.splitContainer10.Dock = DockStyle.Fill;
            this.splitContainer10.FixedPanel = FixedPanel.Panel1;
            this.splitContainer10.Location = new Point(0, 0);
            this.splitContainer10.Margin = new Padding(2);
            this.splitContainer10.Name = "splitContainer10";
            this.splitContainer10.Orientation = Orientation.Horizontal;
            this.splitContainer10.Panel1.Controls.Add((Control)this.splitContainer4);
            this.splitContainer10.Panel2.Controls.Add((Control)this.splitContainer6);
            this.splitContainer10.Size = new Size(760, 381);
            this.splitContainer10.SplitterDistance = 180;
            this.splitContainer10.SplitterWidth = 3;
            this.splitContainer10.TabIndex = 17;
            this.splitContainer4.Dock = DockStyle.Fill;
            this.splitContainer4.FixedPanel = FixedPanel.Panel1;
            this.splitContainer4.Location = new Point(0, 0);
            this.splitContainer4.Margin = new Padding(2);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = Orientation.Horizontal;
            this.splitContainer4.Panel1.Controls.Add((Control)this.tbName);
            this.splitContainer4.Panel1.Controls.Add((Control)this.lblName);
            this.splitContainer4.Panel1.Controls.Add((Control)this.tbPepoleNumber);
            this.splitContainer4.Panel1.Controls.Add((Control)this.lblPepeole);
            this.splitContainer4.Panel2.Controls.Add((Control)this.splitContainer5);
            this.splitContainer4.Size = new Size(760, 180);
            this.splitContainer4.SplitterDistance = 41;
            this.splitContainer4.SplitterWidth = 3;
            this.splitContainer4.TabIndex = 16;
            this.splitContainer5.Dock = DockStyle.Fill;
            this.splitContainer5.FixedPanel = FixedPanel.Panel1;
            this.splitContainer5.Location = new Point(0, 0);
            this.splitContainer5.Margin = new Padding(2);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = Orientation.Horizontal;
            this.splitContainer5.Panel1.Controls.Add((Control)this.lblTags);
            this.splitContainer5.Panel1.Controls.Add((Control)this.btnEditTags);
            this.splitContainer5.Panel2.Controls.Add((Control)this.panel1);
            this.splitContainer5.Size = new Size(760, 136);
            this.splitContainer5.SplitterDistance = 48;
            this.splitContainer5.SplitterWidth = 3;
            this.splitContainer5.TabIndex = 0;
            this.panel1.Controls.Add((Control)this.splitContainer1);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Margin = new Padding(2);
            this.panel1.MaximumSize = new Size(0, 122);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0, 85);
            this.panel1.TabIndex = 15;
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.FixedPanel = FixedPanel.Panel1;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Margin = new Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel2.Controls.Add((Control)this.splitContainer2);
            this.splitContainer1.Size = new Size(760, 85);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer2.Dock = DockStyle.Fill;
            this.splitContainer2.FixedPanel = FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new Point(0, 0);
            this.splitContainer2.Margin = new Padding(2);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Panel1.Controls.Add((Control)this.spltContTagsControl);
            this.splitContainer2.Size = new Size(732, 85);
            this.splitContainer2.SplitterDistance = 704;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 0;
            this.spltContTagsControl.Dock = DockStyle.Fill;
            this.spltContTagsControl.Location = new Point(0, 0);
            this.spltContTagsControl.Margin = new Padding(2);
            this.spltContTagsControl.Name = "spltContTagsControl";
            this.spltContTagsControl.Panel1.Controls.Add((Control)this.tagsLabelsController);
            this.spltContTagsControl.Panel2.Controls.Add((Control)this.NotSelected);
            this.spltContTagsControl.Panel2Collapsed = true;
            this.spltContTagsControl.Size = new Size(704, 85);
            this.spltContTagsControl.SplitterDistance = 25;
            this.spltContTagsControl.SplitterWidth = 3;
            this.spltContTagsControl.TabIndex = 0;
            this.NotSelected.AutoScroll = true;
            this.NotSelected.BorderStyle = BorderStyle.Fixed3D;
            this.NotSelected.Dock = DockStyle.Fill;
            this.NotSelected.ForeColor = Color.Coral;
            this.NotSelected.Location = new Point(0, 0);
            this.NotSelected.Margin = new Padding(2);
            this.NotSelected.Name = "NotSelected";
            this.NotSelected.Size = new Size(96, 100);
            this.NotSelected.TabIndex = 6;
            this.NotSelected.Visible = false;
            this.splitContainer6.Dock = DockStyle.Fill;
            this.splitContainer6.FixedPanel = FixedPanel.Panel1;
            this.splitContainer6.Location = new Point(0, 0);
            this.splitContainer6.Margin = new Padding(2);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = Orientation.Horizontal;
            this.splitContainer6.Panel1.Controls.Add((Control)this.btnAddNewContent);
            this.splitContainer6.Panel1.Controls.Add((Control)this.btnAddContent);
            this.splitContainer6.Panel1.Controls.Add((Control)this.lblContents);
            this.splitContainer6.Panel2.Controls.Add((Control)this.splitContainer7);
            this.splitContainer6.Size = new Size(760, 198);
            this.splitContainer6.SplitterDistance = 39;
            this.splitContainer6.SplitterWidth = 3;
            this.splitContainer6.TabIndex = 16;
            this.btnAddNewContent.FlatAppearance.BorderColor = Color.FromArgb(224, 224, 224);
            this.btnAddNewContent.FlatStyle = FlatStyle.Flat;
            this.btnAddNewContent.ForeColor = Color.FromArgb(228, 95, 1);
            this.btnAddNewContent.Location = new Point(91, 3);
            this.btnAddNewContent.Margin = new Padding(2);
            this.btnAddNewContent.Name = "btnAddNewContent";
            this.btnAddNewContent.Size = new Size(22, 26);
            this.btnAddNewContent.TabIndex = 17;
            this.btnAddNewContent.Text = "\uD83C\uDF1F";
            this.btnAddNewContent.UseVisualStyleBackColor = true;
            this.btnAddNewContent.Visible = false;
            this.splitContainer7.Dock = DockStyle.Fill;
            this.splitContainer7.FixedPanel = FixedPanel.Panel2;
            this.splitContainer7.Location = new Point(0, 0);
            this.splitContainer7.Margin = new Padding(2);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = Orientation.Horizontal;
            this.splitContainer7.Panel1.Controls.Add((Control)this.splitContainer8);
            this.splitContainer7.Size = new Size(760, 156);
            this.splitContainer7.SplitterDistance = 128;
            this.splitContainer7.SplitterWidth = 3;
            this.splitContainer7.TabIndex = 0;
            this.splitContainer8.Dock = DockStyle.Fill;
            this.splitContainer8.FixedPanel = FixedPanel.Panel1;
            this.splitContainer8.Location = new Point(0, 0);
            this.splitContainer8.Margin = new Padding(2);
            this.splitContainer8.Name = "splitContainer8";
            this.splitContainer8.Panel2.Controls.Add((Control)this.splitContainer9);
            this.splitContainer8.Size = new Size(760, 128);
            this.splitContainer8.SplitterDistance = 28;
            this.splitContainer8.SplitterWidth = 3;
            this.splitContainer8.TabIndex = 0;
            this.splitContainer9.Dock = DockStyle.Fill;
            this.splitContainer9.FixedPanel = FixedPanel.Panel2;
            this.splitContainer9.Location = new Point(0, 0);
            this.splitContainer9.Margin = new Padding(2);
            this.splitContainer9.Name = "splitContainer9";
            this.splitContainer9.Panel1.Controls.Add((Control)this.splitContainer3);
            this.splitContainer9.Size = new Size(729, 128);
            this.splitContainer9.SplitterDistance = 701;
            this.splitContainer9.SplitterWidth = 3;
            this.splitContainer9.TabIndex = 0;
            this.splitContainer3.Dock = DockStyle.Fill;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = Orientation.Horizontal;
            this.splitContainer3.Panel1.Controls.Add((Control)this.dgvContents);
            this.splitContainer3.Panel2.Controls.Add((Control)this.rtbDescription);
            this.splitContainer3.Size = new Size(701, 128);
            this.splitContainer3.SplitterDistance = 63;
            this.splitContainer3.TabIndex = 1;
            this.rtbDescription.BackColor = Color.FromArgb(255, 255, 255);
            this.rtbDescription.Dock = DockStyle.Fill;
            this.rtbDescription.ForeColor = Color.Black;
            this.rtbDescription.Font = new Font("Times New Roman", 12.0f);
            this.rtbDescription.Location = new Point(0, 0);
            this.rtbDescription.Name = "rtbDescription";
            this.rtbDescription.ReadOnly = true;
            this.rtbDescription.Size = new Size(701, 61);
            this.rtbDescription.TabIndex = 0;
            this.rtbDescription.Text = "";
            this.Nom.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Nom.HeaderText = "Nom";
            this.Nom.MinimumWidth = 6;
            this.Nom.Name = "Nom";
            this.Nom.ReadOnly = true;
            this.Nom.Resizable = DataGridViewTriState.False;
            this.Nom.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.quantity.HeaderText = "Quantité";
            this.quantity.MinimumWidth = 6;
            this.quantity.Name = "quantity";
            this.quantity.ReadOnly = true;
            this.quantity.Resizable = DataGridViewTriState.False;
            this.quantity.SortMode = DataGridViewColumnSortMode.NotSortable;
            this.quantity.Width = 60;
            this.UnitContent.HeaderText = "Unité";
            this.UnitContent.MinimumWidth = 6;
            this.UnitContent.Name = "UnitContent";
            this.UnitContent.ReadOnly = true;
            this.UnitContent.Resizable = DataGridViewTriState.False;
            this.UnitContent.Width = 80;
            this.edit.FlatStyle = FlatStyle.Flat;
            this.edit.HeaderText = "";
            this.edit.MinimumWidth = 10;
            this.edit.Name = "edit";
            this.edit.ReadOnly = true;
            this.edit.Resizable = DataGridViewTriState.False;
            this.edit.Width = 46;
            this.remove.FlatStyle = FlatStyle.Flat;
            this.remove.HeaderText = "";
            this.remove.MinimumWidth = 10;
            this.remove.Name = "remove";
            this.remove.ReadOnly = true;
            this.remove.Resizable = DataGridViewTriState.False;
            this.remove.Width = 46;
            this.contentIndex.HeaderText = "";
            this.contentIndex.Name = "contentIndex";
            this.contentIndex.ReadOnly = true;
            this.contentIndex.Visible = false;
            this.UnderEdit.HeaderText = "";
            this.UnderEdit.Name = "UnderEdit";
            this.UnderEdit.ReadOnly = true;
            this.UnderEdit.Visible = false;
            this.tagsLabelsController.BackColor = Color.FromArgb(37, 40, 42);
            this.tagsLabelsController.Dock = DockStyle.Fill;
            this.tagsLabelsController.Location = new Point(0, 0);
            this.tagsLabelsController.Margin = new Padding(2, 2, 2, 2);
            this.tagsLabelsController.Name = "tagsLabelsController";
            this.tagsLabelsController.Size = new Size(704, 85);
            this.tagsLabelsController.TabIndex = 0;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(37, 37, 38);
            this.BorderStyle = BorderStyle.Fixed3D;
            this.Controls.Add((Control)this.RecipeMainPanelControl);
            this.Controls.Add((Control)this.header);
            this.Margin = new Padding(22, 24, 22, 24);
            this.Name = "RecipeControl";
            this.Size = new Size(760, 426);
            ((ISupportInitialize)this.dgvContents).EndInit();
            this.header.ResumeLayout(false);
            this.RecipeMainPanelControl.ResumeLayout(false);
            this.splitContainer10.Panel1.ResumeLayout(false);
            this.splitContainer10.Panel2.ResumeLayout(false);
            this.splitContainer10.EndInit();
            this.splitContainer10.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel1.PerformLayout();
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.spltContTagsControl.Panel1.ResumeLayout(false);
            this.spltContTagsControl.Panel2.ResumeLayout(false);
            this.spltContTagsControl.EndInit();
            this.spltContTagsControl.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel1.PerformLayout();
            this.splitContainer6.Panel2.ResumeLayout(false);
            this.splitContainer6.EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.splitContainer8.Panel2.ResumeLayout(false);
            this.splitContainer8.EndInit();
            this.splitContainer8.ResumeLayout(false);
            this.splitContainer9.Panel1.ResumeLayout(false);
            this.splitContainer9.EndInit();
            this.splitContainer9.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
