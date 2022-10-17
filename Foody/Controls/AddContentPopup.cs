// Decompiled with JetBrains decompiler
// Type: Foody.Controls.AddContentPopup
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using Foody.Generic;
using Foody.IO;
using Foody.Models;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Foody.Controls
{
    public class AddContentPopup : UserControl
    {
        public Action<Content> action;
        private readonly Form form = new Form();
        private IContainer components = (IContainer)null;
        private ComboBox cbContentType;
        private Button cancelButton;
        private Label lblContentType;
        private Label lblContentName;
        private TextBox tbContentName;
        private Button btnAddContent;
        private ContentTagConverter ctc = new ContentTagConverter();

        public AddContentPopup()
        {
            this.InitializeComponent();
            this.PopulateData();
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

        private void PopulateData()
        {
            this.cbContentType.Items.AddRange(Consts.contentTags);
        }
        public void InitEvents()
        {
            this.btnAddContent.Click += (EventHandler)((s, e) =>
           {
               if (cbContentType.SelectedIndex > 0)
                   return;

               string tag = Consts.contentTags[cbContentType.SelectedIndex];
               var c = new Content(tbContentName.Text, ctc.GetFromString(tag));
               Database.AddContentToDatabase(c);
              
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
            this.cbContentType = new System.Windows.Forms.ComboBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.btnAddContent = new System.Windows.Forms.Button();
            this.lblContentType = new System.Windows.Forms.Label();
            this.lblContentName = new System.Windows.Forms.Label();
            this.tbContentName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cbContentType
            // 
            this.cbContentType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(40)))), ((int)(((byte)(42)))));
            this.cbContentType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbContentType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbContentType.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.cbContentType.Location = new System.Drawing.Point(185, 51);
            this.cbContentType.Margin = new System.Windows.Forms.Padding(2);
            this.cbContentType.Name = "cbContentType";
            this.cbContentType.Size = new System.Drawing.Size(296, 25);
            this.cbContentType.TabIndex = 0;
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(45)))), ((int)(((byte)(32)))));
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.ForeColor = System.Drawing.SystemColors.WindowText;
            this.cancelButton.Location = new System.Drawing.Point(336, 92);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(56, 25);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Annuler";
            this.cancelButton.UseVisualStyleBackColor = false;
            // 
            // btnAddContent
            // 
            this.btnAddContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(210)))), ((int)(((byte)(138)))));
            this.btnAddContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddContent.ForeColor = System.Drawing.SystemColors.MenuText;
            this.btnAddContent.Location = new System.Drawing.Point(422, 92);
            this.btnAddContent.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddContent.Name = "btnAddContent";
            this.btnAddContent.Size = new System.Drawing.Size(56, 25);
            this.btnAddContent.TabIndex = 2;
            this.btnAddContent.Text = "Ajouter";
            this.btnAddContent.UseVisualStyleBackColor = false;
            // 
            // lblContentType
            // 
            this.lblContentType.AutoSize = true;
            this.lblContentType.Location = new System.Drawing.Point(26, 57);
            this.lblContentType.Name = "lblContentType";
            this.lblContentType.Size = new System.Drawing.Size(88, 13);
            this.lblContentType.TabIndex = 3;
            this.lblContentType.Text = "Type d\'ingredient";
            // 
            // lblContentName
            // 
            this.lblContentName.AutoSize = true;
            this.lblContentName.Location = new System.Drawing.Point(26, 16);
            this.lblContentName.Name = "lblContentName";
            this.lblContentName.Size = new System.Drawing.Size(86, 13);
            this.lblContentName.TabIndex = 4;
            this.lblContentName.Text = "Nom d\'ingredient";
            // 
            // tbContentName
            // 
            this.tbContentName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.tbContentName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbContentName.ForeColor = System.Drawing.SystemColors.Window;
            this.tbContentName.Location = new System.Drawing.Point(185, 16);
            this.tbContentName.Name = "tbContentName";
            this.tbContentName.Size = new System.Drawing.Size(296, 20);
            this.tbContentName.TabIndex = 5;
            // 
            // AddContentPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.tbContentName);
            this.Controls.Add(this.lblContentName);
            this.Controls.Add(this.lblContentType);
            this.Controls.Add(this.btnAddContent);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.cbContentType);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AddContentPopup";
            this.Size = new System.Drawing.Size(489, 133);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
