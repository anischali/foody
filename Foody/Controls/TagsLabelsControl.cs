// Decompiled with JetBrains decompiler
// Type: Foody.Controls.TagsLabelsControl
// Assembly: Foody, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 04C929E0-6224-48BC-A407-266CE08536F7
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.exe

using Foody.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Foody.Controls
{
  public class TagsLabelsControl : UserControl
  {
    public List<Tag> selectedTagsLabels = new List<Tag>();
    private int tagsToView = -1;
    private TagConverter c = new TagConverter();
    public EventHandler RefreshEvent;
    private IContainer components = (IContainer) null;
    private SplitContainer tagsSplitter;
    private FlowLayoutPanel selectedTags;
    private FlowLayoutPanel notSelectedTags;

    public TagsLabelsControl() => this.InitializeComponent();

    public void SetControlsItems(int mask, Tag[] tags)
    {
      this.tagsToView = mask;
      if (tags != null)
        this.selectedTagsLabels.AddRange((IEnumerable<Tag>) tags);
      this.UpdateLabels();
    }

    private Button[] GetAllLabels(Tag[] lst, bool selected, Action<object, EventArgs> action)
    {
      List<Button> buttonList = new List<Button>();
      for (int index = 0; index < lst.Length; ++index)
      {
        Button button = Components.TagLabel(this.c.GetString(lst[index]), selected);
        button.Click += (EventHandler) ((s, e) =>
        {
          if (action == null)
            return;
          action(s, e);
        });
        buttonList.Add(button);
      }
      return buttonList.ToArray();
    }

    private void ViewLabels(int mask)
    {
      if (mask == 0 || mask == 2)
      {
        this.tagsSplitter.Panel1Collapsed = false;
        this.selectedTags.Visible = true;
        this.PopulateSelectedTags();
      }
      if (mask != 1 && mask != 2)
        return;
      this.tagsSplitter.Panel2Collapsed = false;
      this.notSelectedTags.Visible = true;
      List<string> stringList = new List<string>();
      stringList.AddRange((IEnumerable<string>) Consts.tags);
      List<Tag> tagList = new List<Tag>();
      foreach (string tag in stringList)
      {
        Tag fromString = this.c.GetFromString(tag);
        if (!this.selectedTagsLabels.Contains(fromString))
          tagList.Add(fromString);
      }
      this.PopulateNotSelectedTags(tagList.ToArray());
    }

    private void PopulateSelectedTags() => this.selectedTags.Controls.AddRange((Control[]) this.GetAllLabels(this.selectedTagsLabels.ToArray(), true, (Action<object, EventArgs>) ((s, e) =>
    {
      Button button = (Button) s;
      if ((bool) button.Tag)
        this.selectedTagsLabels.Remove(this.c.GetFromString(button.Text));
      else
        this.selectedTagsLabels.Add(this.c.GetFromString(button.Text));
      this.UpdateLabels();
    })));

    private void PopulateNotSelectedTags(Tag[] tags) => this.notSelectedTags.Controls.AddRange((Control[]) this.GetAllLabels(tags, false, (Action<object, EventArgs>) ((s, e) =>
    {
      Button button = (Button) s;
      if ((bool) button.Tag)
        this.selectedTagsLabels.Remove(this.c.GetFromString(button.Text));
      else
        this.selectedTagsLabels.Add(this.c.GetFromString(button.Text));
      this.UpdateLabels();
    })));

    public void UpdateLabels()
    {
      this.notSelectedTags.Controls.Clear();
      this.selectedTags.Controls.Clear();
      switch (this.tagsToView)
      {
        case -1:
          this.ShowAll(false);
          break;
        case 0:
          this.tagsSplitter.Panel2Collapsed = true;
          this.notSelectedTags.Visible = false;
          this.ViewLabels(0);
          break;
        case 1:
          this.tagsSplitter.Panel1Collapsed = true;
          this.selectedTags.Visible = false;
          this.ViewLabels(1);
          break;
        case 2:
          this.ShowAll(true);
          this.ViewLabels(2);
          break;
      }
      if (this.RefreshEvent == null)
        return;
      this.RefreshEvent((object) null, (EventArgs) null);
    }

    public void SetMask(int mask)
    {
      this.tagsToView = mask;
      this.UpdateLabels();
    }

    private void ShowAll(bool show)
    {
      this.tagsSplitter.SplitterDistance = this.tagsSplitter.Width / 2;
      this.notSelectedTags.Visible = show;
      this.selectedTags.Visible = show;
    }

    public void ResetTags()
    {
      this.selectedTags.Controls.Clear();
      this.selectedTagsLabels.Clear();
    }

    public Tag[] GetTags() => this.selectedTagsLabels.ToArray();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tagsSplitter = new SplitContainer();
      this.selectedTags = new FlowLayoutPanel();
      this.notSelectedTags = new FlowLayoutPanel();
      this.tagsSplitter.BeginInit();
      this.tagsSplitter.Panel1.SuspendLayout();
      this.tagsSplitter.Panel2.SuspendLayout();
      this.tagsSplitter.SuspendLayout();
      this.SuspendLayout();
      this.tagsSplitter.Dock = DockStyle.Fill;
      this.tagsSplitter.Location = new Point(0, 0);
      this.tagsSplitter.Name = "tagsSplitter";
      this.tagsSplitter.Panel1.Controls.Add((Control) this.selectedTags);
      this.tagsSplitter.Panel2.Controls.Add((Control) this.notSelectedTags);
      this.tagsSplitter.Size = new Size(660, 119);
      this.tagsSplitter.SplitterDistance = 315;
      this.tagsSplitter.TabIndex = 0;
      this.selectedTags.AutoScroll = true;
      this.selectedTags.Dock = DockStyle.Fill;
      this.selectedTags.Location = new Point(0, 0);
      this.selectedTags.Name = "selectedTags";
      this.selectedTags.Size = new Size(315, 119);
      this.selectedTags.TabIndex = 0;
      this.notSelectedTags.AutoScroll = true;
      this.notSelectedTags.Dock = DockStyle.Fill;
      this.notSelectedTags.Location = new Point(0, 0);
      this.notSelectedTags.Name = "notSelectedTags";
      this.notSelectedTags.Size = new Size(341, 119);
      this.notSelectedTags.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(37, 40, 42);
      this.Controls.Add((Control) this.tagsSplitter);
      this.Name = nameof (TagsLabelsControl);
      this.Size = new Size(660, 119);
      this.tagsSplitter.Panel1.ResumeLayout(false);
      this.tagsSplitter.Panel2.ResumeLayout(false);
      this.tagsSplitter.EndInit();
      this.tagsSplitter.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
