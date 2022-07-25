// Decompiled with JetBrains decompiler
// Type: Foody.MainWindow
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using Foody.Controls;
using Foody.Generic;
using Foody.IO;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Foody
{
  public class MainWindow : Form
  {
    private Color initialBackColor = Color.FromArgb(35, 171, 242);
    private Color selectedItemBackColor = Color.FromArgb(228, 95, 1);
    public Point MouseLocation;
    private IContainer components = (IContainer) null;
    private Panel pnlLargeSideMenu;
    private Button btnExportLarge;
    private Button btnImportLarge;
    private Button btnViewLarge;
    private Button btnGenerateLarge;
    private Button btnSettings;
    private Button btnCollapse;
    private Panel pnlSmallSideMenu;
    private Button btnUncollapsSmall;
    private Button btnSettingsSmall;
    private Button btnExportSmall;
    private Button btnImportSmall;
    private Button btnViewSmall;
    private Button btnGenerateSmall;
    private Panel pnlFooterLarge;
    private Panel panelMainContainer;
    private Panel pnlMainPage;
    private GenerateHeader headerGenerate;
    private Panel ViewRecipesPanel;
    private RecipeList recipeListView;
    private RecipeControl recipeControl1;
    private GeneratePanel generatePanel1;

    private void InitBorderFormStyleEvents()
    {
      this.MouseDown += new MouseEventHandler(this.OnMouseDownEvent);
      this.MouseMove += new MouseEventHandler(this.OnMouseMoveEvent);
    }

    public MainWindow()
    {
      Database.LoadAllDatabases();
      this.InitializeComponent();
      this.InitEvents();
      this.InitBorderFormStyleEvents();
    }

    private void InitEvents()
    {
      Action<Button, Button, Color> PutColor = (Action<Button, Button, Color>) ((s, l, c) => s.BackColor = l.BackColor = c);
      Action DefaultButtonsTheme = (Action) (() =>
      {
        PutColor(this.btnGenerateSmall, this.btnGenerateLarge, this.initialBackColor);
        PutColor(this.btnViewSmall, this.btnViewLarge, this.initialBackColor);
        PutColor(this.btnExportSmall, this.btnExportLarge, this.initialBackColor);
        PutColor(this.btnImportSmall, this.btnImportLarge, this.initialBackColor);
        this.headerGenerate.Visible = false;
        this.ViewRecipesPanel.Visible = false;
        this.generatePanel1.Visible = false;
      });
      this.btnCollapse.Click += (EventHandler) ((s, e) =>
      {
        this.pnlLargeSideMenu.Hide();
        this.pnlSmallSideMenu.Show();
      });
      this.btnUncollapsSmall.Click += (EventHandler) ((s, e) =>
      {
        this.pnlSmallSideMenu.Hide();
        this.pnlLargeSideMenu.Show();
      });
      Action btnGenerateAction = (Action) (() =>
      {
        DefaultButtonsTheme();
        PutColor(this.btnGenerateSmall, this.btnGenerateLarge, this.selectedItemBackColor);
        this.headerGenerate.Visible = true;
        this.generatePanel1.Visible = false;
      });
      Action btnExportAction = (Action) (() =>
      {
        if (this.recipeListView == null)
          return;
        int[] selectedRecipesIndexs = this.recipeListView.GetSelectedRecipesIndexs;
        if (selectedRecipesIndexs == null)
          return;
        RecipeExporter recipeExporter = new RecipeExporter();
        for (int index = 0; index < selectedRecipesIndexs.Length; ++index)
          recipeExporter.Export(selectedRecipesIndexs[index]);
        using (FileDialog fileDialog = (FileDialog) new SaveFileDialog())
        {
          fileDialog.Filter = "Fichier texte *.txt|*.txt";
          fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
          fileDialog.FileName = "recipes.txt";
          if (fileDialog.ShowDialog() == DialogResult.OK)
            recipeExporter.Save(fileDialog.FileName);
        }
      });
      Action btnViewAction = (Action) (() =>
      {
        DefaultButtonsTheme();
        PutColor(this.btnViewSmall, this.btnViewLarge, this.selectedItemBackColor);
        this.ViewRecipesPanel.Visible = true;
        this.recipeListView.PopulateDataGridViewWithRecipes(Database.AllMenus);
      });
      this.btnGenerateLarge.Click += (EventHandler) ((s, e) => btnGenerateAction());
      this.btnGenerateSmall.Click += (EventHandler) ((s, e) => btnGenerateAction());
      this.btnExportLarge.Click += (EventHandler) ((s, e) => btnExportAction());
      this.btnExportSmall.Click += (EventHandler) ((s, e) => btnExportAction());
      this.btnImportLarge.Click += (EventHandler) ((s, e) => { });
      this.btnImportSmall.Click += (EventHandler) ((s, e) => { });
      this.btnViewLarge.Click += (EventHandler) ((s, e) => btnViewAction());
      this.btnViewSmall.Click += (EventHandler) ((s, e) => btnViewAction());
      this.headerGenerate.generateEvent += (Delegates.GenerateEvent) (n =>
      {
        this.headerGenerate.Visible = false;
        this.generatePanel1.Visible = this.pnlMainPage.Visible = this.ViewRecipesPanel.Visible = true;
        this.generatePanel1.GenerateMealsList(n);
      });
      btnGenerateAction();
    }

    private void OnMouseDownEvent(object s, MouseEventArgs e) => this.MouseLocation = new Point(-e.X, -e.Y);

    private void OnMouseMoveEvent(object s, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;
      Point mousePosition = Control.MousePosition;
      mousePosition.Offset(this.MouseLocation.X, this.MouseLocation.Y);
      this.Location = mousePosition;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainWindow));
      this.pnlLargeSideMenu = new Panel();
      this.pnlFooterLarge = new Panel();
      this.btnSettings = new Button();
      this.btnCollapse = new Button();
      this.btnExportLarge = new Button();
      this.btnImportLarge = new Button();
      this.btnViewLarge = new Button();
      this.btnGenerateLarge = new Button();
      this.pnlSmallSideMenu = new Panel();
      this.btnUncollapsSmall = new Button();
      this.btnSettingsSmall = new Button();
      this.btnExportSmall = new Button();
      this.btnImportSmall = new Button();
      this.btnViewSmall = new Button();
      this.btnGenerateSmall = new Button();
      this.panelMainContainer = new Panel();
      this.pnlMainPage = new Panel();
      this.ViewRecipesPanel = new Panel();
      this.generatePanel1 = new GeneratePanel();
      this.recipeListView = new RecipeList();
      this.headerGenerate = new GenerateHeader();
      this.pnlLargeSideMenu.SuspendLayout();
      this.pnlFooterLarge.SuspendLayout();
      this.pnlSmallSideMenu.SuspendLayout();
      this.panelMainContainer.SuspendLayout();
      this.pnlMainPage.SuspendLayout();
      this.ViewRecipesPanel.SuspendLayout();
      this.SuspendLayout();
      this.pnlLargeSideMenu.BackColor = Color.FromArgb(37, 37, 38);
      this.pnlLargeSideMenu.Controls.Add((Control) this.pnlFooterLarge);
      this.pnlLargeSideMenu.Controls.Add((Control) this.btnExportLarge);
      this.pnlLargeSideMenu.Controls.Add((Control) this.btnImportLarge);
      this.pnlLargeSideMenu.Controls.Add((Control) this.btnViewLarge);
      this.pnlLargeSideMenu.Controls.Add((Control) this.btnGenerateLarge);
      this.pnlLargeSideMenu.Dock = DockStyle.Left;
      this.pnlLargeSideMenu.Location = new Point(48, 0);
      this.pnlLargeSideMenu.Margin = new Padding(2, 2, 2, 2);
      this.pnlLargeSideMenu.Name = "pnlLargeSideMenu";
      this.pnlLargeSideMenu.Size = new Size(156, 609);
      this.pnlLargeSideMenu.TabIndex = 0;
      this.pnlLargeSideMenu.Visible = false;
      this.pnlFooterLarge.Controls.Add((Control) this.btnSettings);
      this.pnlFooterLarge.Controls.Add((Control) this.btnCollapse);
      this.pnlFooterLarge.Dock = DockStyle.Bottom;
      this.pnlFooterLarge.Location = new Point(0, 557);
      this.pnlFooterLarge.Margin = new Padding(2, 2, 2, 2);
      this.pnlFooterLarge.Name = "pnlFooterLarge";
      this.pnlFooterLarge.Size = new Size(156, 52);
      this.pnlFooterLarge.TabIndex = 15;
      this.btnSettings.Dock = DockStyle.Left;
      this.btnSettings.FlatAppearance.BorderSize = 0;
      this.btnSettings.FlatStyle = FlatStyle.Flat;
      this.btnSettings.Font = new Font("Sitka Small", 13.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSettings.ForeColor = Color.White;
      this.btnSettings.Location = new Point(0, 0);
      this.btnSettings.Margin = new Padding(2, 2, 2, 2);
      this.btnSettings.Name = "btnSettings";
      this.btnSettings.Size = new Size(48, 52);
      this.btnSettings.TabIndex = 13;
      this.btnSettings.Text = "⚙";
      this.btnSettings.UseVisualStyleBackColor = true;
      this.btnCollapse.Dock = DockStyle.Right;
      this.btnCollapse.FlatAppearance.BorderSize = 0;
      this.btnCollapse.FlatStyle = FlatStyle.Flat;
      this.btnCollapse.Font = new Font("Sitka Small", 13.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnCollapse.ForeColor = Color.White;
      this.btnCollapse.Location = new Point(108, 0);
      this.btnCollapse.Margin = new Padding(2, 2, 2, 2);
      this.btnCollapse.Name = "btnCollapse";
      this.btnCollapse.Size = new Size(48, 52);
      this.btnCollapse.TabIndex = 14;
      this.btnCollapse.Text = "\uD83D\uDC48";
      this.btnCollapse.UseVisualStyleBackColor = true;
      this.btnExportLarge.BackColor = Color.FromArgb(35, 171, 242);
      this.btnExportLarge.Dock = DockStyle.Top;
      this.btnExportLarge.FlatAppearance.BorderSize = 0;
      this.btnExportLarge.FlatStyle = FlatStyle.Flat;
      this.btnExportLarge.Font = new Font("Times New Roman", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnExportLarge.ForeColor = Color.White;
      this.btnExportLarge.Location = new Point(0, 111);
      this.btnExportLarge.Margin = new Padding(2, 2, 2, 2);
      this.btnExportLarge.Name = "btnExportLarge";
      this.btnExportLarge.Size = new Size(156, 37);
      this.btnExportLarge.TabIndex = 9;
      this.btnExportLarge.Text = "Exporter";
      this.btnExportLarge.UseVisualStyleBackColor = false;
      this.btnImportLarge.BackColor = Color.FromArgb(35, 171, 242);
      this.btnImportLarge.Dock = DockStyle.Top;
      this.btnImportLarge.FlatAppearance.BorderSize = 0;
      this.btnImportLarge.FlatStyle = FlatStyle.Flat;
      this.btnImportLarge.Font = new Font("Times New Roman", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnImportLarge.ForeColor = Color.White;
      this.btnImportLarge.Location = new Point(0, 74);
      this.btnImportLarge.Margin = new Padding(2, 2, 2, 2);
      this.btnImportLarge.Name = "btnImportLarge";
      this.btnImportLarge.Size = new Size(156, 37);
      this.btnImportLarge.TabIndex = 8;
      this.btnImportLarge.Text = "Importer";
      this.btnImportLarge.UseVisualStyleBackColor = false;
      this.btnViewLarge.BackColor = Color.FromArgb(35, 171, 242);
      this.btnViewLarge.Dock = DockStyle.Top;
      this.btnViewLarge.FlatAppearance.BorderSize = 0;
      this.btnViewLarge.FlatStyle = FlatStyle.Flat;
      this.btnViewLarge.Font = new Font("Times New Roman", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnViewLarge.ForeColor = Color.White;
      this.btnViewLarge.Location = new Point(0, 37);
      this.btnViewLarge.Margin = new Padding(2, 2, 2, 2);
      this.btnViewLarge.Name = "btnViewLarge";
      this.btnViewLarge.Size = new Size(156, 37);
      this.btnViewLarge.TabIndex = 7;
      this.btnViewLarge.Text = "Afficher tout";
      this.btnViewLarge.UseVisualStyleBackColor = false;
      this.btnGenerateLarge.BackColor = Color.FromArgb(228, 95, 1);
      this.btnGenerateLarge.Dock = DockStyle.Top;
      this.btnGenerateLarge.FlatAppearance.BorderSize = 0;
      this.btnGenerateLarge.FlatStyle = FlatStyle.Flat;
      this.btnGenerateLarge.Font = new Font("Times New Roman", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnGenerateLarge.ForeColor = Color.White;
      this.btnGenerateLarge.Location = new Point(0, 0);
      this.btnGenerateLarge.Margin = new Padding(2, 2, 2, 2);
      this.btnGenerateLarge.Name = "btnGenerateLarge";
      this.btnGenerateLarge.Size = new Size(156, 37);
      this.btnGenerateLarge.TabIndex = 6;
      this.btnGenerateLarge.Text = "Genérer";
      this.btnGenerateLarge.UseVisualStyleBackColor = false;
      this.pnlSmallSideMenu.BackColor = Color.FromArgb(37, 37, 38);
      this.pnlSmallSideMenu.Controls.Add((Control) this.btnUncollapsSmall);
      this.pnlSmallSideMenu.Controls.Add((Control) this.btnSettingsSmall);
      this.pnlSmallSideMenu.Controls.Add((Control) this.btnExportSmall);
      this.pnlSmallSideMenu.Controls.Add((Control) this.btnImportSmall);
      this.pnlSmallSideMenu.Controls.Add((Control) this.btnViewSmall);
      this.pnlSmallSideMenu.Controls.Add((Control) this.btnGenerateSmall);
      this.pnlSmallSideMenu.Dock = DockStyle.Left;
      this.pnlSmallSideMenu.Location = new Point(0, 0);
      this.pnlSmallSideMenu.Margin = new Padding(2, 2, 2, 2);
      this.pnlSmallSideMenu.Name = "pnlSmallSideMenu";
      this.pnlSmallSideMenu.Size = new Size(48, 609);
      this.pnlSmallSideMenu.TabIndex = 3;
      this.btnUncollapsSmall.Dock = DockStyle.Bottom;
      this.btnUncollapsSmall.FlatAppearance.BorderSize = 0;
      this.btnUncollapsSmall.FlatStyle = FlatStyle.Flat;
      this.btnUncollapsSmall.Font = new Font("Sitka Small", 13.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnUncollapsSmall.ForeColor = Color.White;
      this.btnUncollapsSmall.Location = new Point(0, 505);
      this.btnUncollapsSmall.Margin = new Padding(2, 2, 2, 2);
      this.btnUncollapsSmall.Name = "btnUncollapsSmall";
      this.btnUncollapsSmall.Size = new Size(48, 52);
      this.btnUncollapsSmall.TabIndex = 11;
      this.btnUncollapsSmall.Text = "\uD83D\uDC49";
      this.btnUncollapsSmall.UseVisualStyleBackColor = true;
      this.btnSettingsSmall.Dock = DockStyle.Bottom;
      this.btnSettingsSmall.FlatAppearance.BorderSize = 0;
      this.btnSettingsSmall.FlatStyle = FlatStyle.Flat;
      this.btnSettingsSmall.Font = new Font("Sitka Small", 13.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnSettingsSmall.ForeColor = Color.White;
      this.btnSettingsSmall.Location = new Point(0, 557);
      this.btnSettingsSmall.Margin = new Padding(2, 2, 2, 2);
      this.btnSettingsSmall.Name = "btnSettingsSmall";
      this.btnSettingsSmall.Size = new Size(48, 52);
      this.btnSettingsSmall.TabIndex = 10;
      this.btnSettingsSmall.Text = "⚙";
      this.btnSettingsSmall.UseVisualStyleBackColor = true;
      this.btnExportSmall.Dock = DockStyle.Top;
      this.btnExportSmall.FlatAppearance.BorderSize = 0;
      this.btnExportSmall.FlatStyle = FlatStyle.Flat;
      this.btnExportSmall.Font = new Font("Sitka Small", 13.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnExportSmall.ForeColor = Color.White;
      this.btnExportSmall.Location = new Point(0, 156);
      this.btnExportSmall.Margin = new Padding(2, 2, 2, 2);
      this.btnExportSmall.Name = "btnExportSmall";
      this.btnExportSmall.Size = new Size(48, 52);
      this.btnExportSmall.TabIndex = 9;
      this.btnExportSmall.Text = "\uD83D\uDCE4";
      this.btnExportSmall.UseVisualStyleBackColor = true;
      this.btnImportSmall.Dock = DockStyle.Top;
      this.btnImportSmall.FlatAppearance.BorderSize = 0;
      this.btnImportSmall.FlatStyle = FlatStyle.Flat;
      this.btnImportSmall.Font = new Font("Sitka Small", 13.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnImportSmall.ForeColor = Color.White;
      this.btnImportSmall.Location = new Point(0, 104);
      this.btnImportSmall.Margin = new Padding(2, 2, 2, 2);
      this.btnImportSmall.Name = "btnImportSmall";
      this.btnImportSmall.Size = new Size(48, 52);
      this.btnImportSmall.TabIndex = 8;
      this.btnImportSmall.Text = "\uD83D\uDCE5";
      this.btnImportSmall.UseVisualStyleBackColor = true;
      this.btnViewSmall.Dock = DockStyle.Top;
      this.btnViewSmall.FlatAppearance.BorderSize = 0;
      this.btnViewSmall.FlatStyle = FlatStyle.Flat;
      this.btnViewSmall.Font = new Font("Sitka Small", 13.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnViewSmall.ForeColor = Color.White;
      this.btnViewSmall.Location = new Point(0, 52);
      this.btnViewSmall.Margin = new Padding(2, 2, 2, 2);
      this.btnViewSmall.Name = "btnViewSmall";
      this.btnViewSmall.Size = new Size(48, 52);
      this.btnViewSmall.TabIndex = 7;
      this.btnViewSmall.Text = "\uD83D\uDC41\u200D\uD83D\uDDE8";
      this.btnViewSmall.UseVisualStyleBackColor = true;
      this.btnGenerateSmall.BackColor = Color.FromArgb(228, 95, 1);
      this.btnGenerateSmall.Dock = DockStyle.Top;
      this.btnGenerateSmall.FlatAppearance.BorderSize = 0;
      this.btnGenerateSmall.FlatStyle = FlatStyle.Flat;
      this.btnGenerateSmall.Font = new Font("Sitka Small", 13.8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnGenerateSmall.ForeColor = Color.White;
      this.btnGenerateSmall.Location = new Point(0, 0);
      this.btnGenerateSmall.Margin = new Padding(2, 2, 2, 2);
      this.btnGenerateSmall.Name = "btnGenerateSmall";
      this.btnGenerateSmall.Size = new Size(48, 52);
      this.btnGenerateSmall.TabIndex = 6;
      this.btnGenerateSmall.Text = "\uD83C\uDF86";
      this.btnGenerateSmall.UseVisualStyleBackColor = false;
      this.panelMainContainer.Controls.Add((Control) this.pnlMainPage);
      this.panelMainContainer.Dock = DockStyle.Fill;
      this.panelMainContainer.Location = new Point(204, 0);
      this.panelMainContainer.Margin = new Padding(2, 2, 2, 2);
      this.panelMainContainer.Name = "panelMainContainer";
      this.panelMainContainer.Size = new Size(824, 609);
      this.panelMainContainer.TabIndex = 4;
      this.pnlMainPage.Controls.Add((Control) this.ViewRecipesPanel);
      this.pnlMainPage.Controls.Add((Control) this.headerGenerate);
      this.pnlMainPage.Dock = DockStyle.Fill;
      this.pnlMainPage.Location = new Point(0, 0);
      this.pnlMainPage.Margin = new Padding(2, 2, 2, 2);
      this.pnlMainPage.Name = "pnlMainPage";
      this.pnlMainPage.Size = new Size(824, 609);
      this.pnlMainPage.TabIndex = 3;
      this.ViewRecipesPanel.Controls.Add((Control) this.generatePanel1);
      this.ViewRecipesPanel.Controls.Add((Control) this.recipeListView);
      this.ViewRecipesPanel.Dock = DockStyle.Fill;
      this.ViewRecipesPanel.Location = new Point(0, 41);
      this.ViewRecipesPanel.Margin = new Padding(2, 2, 2, 2);
      this.ViewRecipesPanel.Name = "ViewRecipesPanel";
      this.ViewRecipesPanel.Size = new Size(824, 568);
      this.ViewRecipesPanel.TabIndex = 3;
      this.generatePanel1.BackColor = Color.FromArgb(37, 37, 38);
      this.generatePanel1.Dock = DockStyle.Fill;
      this.generatePanel1.ForeColor = Color.FromArgb(37, 37, 38);
      this.generatePanel1.Location = new Point(0, 0);
      this.generatePanel1.Margin = new Padding(4, 4, 4, 4);
      this.generatePanel1.Name = "generatePanel1";
      this.generatePanel1.Size = new Size(824, 568);
      this.generatePanel1.TabIndex = 4;
      this.generatePanel1.Visible = false;
      this.recipeListView.BackColor = Color.FromArgb(37, 40, 42);
      this.recipeListView.Dock = DockStyle.Fill;
      this.recipeListView.Location = new Point(0, 0);
      this.recipeListView.Margin = new Padding(2, 2, 2, 2);
      this.recipeListView.Name = "recipeListView";
      this.recipeListView.Size = new Size(824, 568);
      this.recipeListView.TabIndex = 2;
      this.headerGenerate.BackColor = Color.FromArgb(37, 37, 38);
      this.headerGenerate.Dock = DockStyle.Top;
      this.headerGenerate.Location = new Point(0, 0);
      this.headerGenerate.Margin = new Padding(2, 2, 2, 2);
      this.headerGenerate.Name = "headerGenerate";
      this.headerGenerate.Size = new Size(824, 41);
      this.headerGenerate.TabIndex = 0;
      this.AllowDrop = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = Color.FromArgb(37, 40, 42);
      this.ClientSize = new Size(1028, 609);
      this.Controls.Add((Control) this.panelMainContainer);
      this.Controls.Add((Control) this.pnlLargeSideMenu);
      this.Controls.Add((Control) this.pnlSmallSideMenu);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(2, 2, 2, 2);
      this.Name = "MainWindow";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Générateur de course";
      this.pnlLargeSideMenu.ResumeLayout(false);
      this.pnlFooterLarge.ResumeLayout(false);
      this.pnlSmallSideMenu.ResumeLayout(false);
      this.panelMainContainer.ResumeLayout(false);
      this.pnlMainPage.ResumeLayout(false);
      this.ViewRecipesPanel.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
