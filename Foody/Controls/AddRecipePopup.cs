// Decompiled with JetBrains decompiler
// Type: Foody.Controls.AddRecipePopup
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Foody.Controls
{
    public class AddRecipePopup : UserControl
    {
        private int idx = -1;
        public Action<int> action;
        private readonly Form form = new Form();
        private IContainer components = (IContainer)null;
        private ComboBox Data;
        private Button cancelButton;
        private Button addRecipe;

        public AddRecipePopup(string[] toView)
        {
            this.InitializeComponent();
            this.PopulateData(toView);

        }

        private void InitForm()
        {
            this.form.Size = this.Size;
            this.form.ShowIcon = false;
            this.form.FormBorderStyle = FormBorderStyle.None;
            this.form.Controls.Add((Control)this);
            this.form.StartPosition = FormStartPosition.CenterScreen;
            int num = (int)this.form.ShowDialog();
        }

        private void PopulateData(string[] toView) => this.Data.Items.AddRange((object[])toView);

        public void InitEvents()
        {
            this.addRecipe.Click += (EventHandler)((s, e) =>
           {
               if (this.action != null)
                   this.action(this.Data.SelectedIndex);
               this.Dispose();
               this.form.Close();
               this.form.Dispose();
           });
            this.cancelButton.Click += (EventHandler)((s, e) =>
           {
               this.Dispose();
               this.form.Close();
               this.form.Dispose();
           });
        }

        public void ShowPopup()
        {
            this.InitEvents();
            this.InitForm();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Data = new System.Windows.Forms.ComboBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.addRecipe = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Data
            // 
            this.Data.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(40)))), ((int)(((byte)(42)))));
            this.Data.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Data.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Data.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Data.Location = new System.Drawing.Point(13, 10);
            this.Data.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Data.Name = "Data";
            this.Data.Size = new System.Drawing.Size(468, 25);
            this.Data.TabIndex = 0;
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(45)))), ((int)(((byte)(32)))));
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cancelButton.Location = new System.Drawing.Point(336, 89);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(56, 25);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Annuler";
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // addRecipe
            // 
            this.addRecipe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(210)))), ((int)(((byte)(138)))));
            this.addRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addRecipe.ForeColor = System.Drawing.SystemColors.MenuText;
            this.addRecipe.Location = new System.Drawing.Point(397, 89);
            this.addRecipe.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.addRecipe.Name = "addRecipe";
            this.addRecipe.Size = new System.Drawing.Size(56, 25);
            this.addRecipe.TabIndex = 2;
            this.addRecipe.Text = "Ajouter";
            this.addRecipe.UseVisualStyleBackColor = false;
            // 
            // AddRecipePopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.addRecipe);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.Data);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "AddRecipePopup";
            this.Size = new System.Drawing.Size(489, 123);
            this.ResumeLayout(false);

        }
    }
}
