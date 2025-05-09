﻿namespace CreativeFileBrowser
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        private ToolStrip toolStripMain;
        private ToolStripDropDownButton btnFileMenu;
        private ToolStripButton btnAddFolder;
        private ToolStripButton btnRemoveFolder;
        private ToolStripButton btnRefresh;
        private ToolStripButton btnSaveWorkspace;
        private ToolStripDropDownButton btnFileTypes;
        private CheckedListBox fileTypeChecklist;
        private ToolStripLabel lblSort;
        private ToolStripComboBox sortDropDown;
        private ToolStripLabel lblWorkspace;
        private ToolStripComboBox workspaceDropDown;

        private SplitContainer verticalSplit;
        private SplitContainer horizontalTop;
        private SplitContainer horizontalBottom;



        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            //**********************************************************************//
            //THE WINDOW ON DEFAULT OPEN
            //**********************************************************************//

            // Form Settings
            this.Text = "Creative File Browser";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);
            this.Size = new Size(940, 720);

            //**********************************************************************//
            //NAME THE TOOLS  
            //**********************************************************************//
            this.toolStripMain = new ToolStrip();
            this.quadrantHostPanel = new Panel();
            this.verticalSplit = new SplitContainer();
            this.horizontalTop = new SplitContainer();
            this.horizontalBottom = new SplitContainer();

            //**********************************************************************//
            //THE TOOLSTRIP AT THE TOP
            //**********************************************************************//
            // ToolStrip - the top menu stuff
            this.toolStripMain.AutoSize = true;
            this.toolStripMain.Dock = DockStyle.Top;

            this.btnFileMenu = new ToolStripDropDownButton("File");
            this.btnAddFolder = new ToolStripButton("Add");
            this.btnRemoveFolder = new ToolStripButton("Remove");
            this.btnRefresh = new ToolStripButton("Refresh");

            // Add FILE MENU items
            //file menu is justified to the left
            btnFileMenu.DropDownItems.Cast<ToolStripItem>().ToList().ForEach(item =>
            {
                if (item is ToolStripMenuItem mi)
                {
                    mi.DisplayStyle = ToolStripItemDisplayStyle.Text;
                    mi.Image = null;
                    mi.Padding = new Padding(1);
                }
            });

            var itemSave = new ToolStripMenuItem("Save Current Workspace")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Image = null,
                Padding = new Padding(1), // optional: tighter spacing
            };
            itemSave.Click += (_, _) => SaveCurrentWorkspace();

            var itemRemove = new ToolStripMenuItem("Remove Current Workspace")
            {
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Image = null,
                Padding = new Padding(1),
            };
            itemRemove.Click += (_, _) => RemoveCurrentWorkspace();
            btnFileMenu.DropDownItems.Clear();
            btnFileMenu.DropDownItems.Add(itemSave);
            btnFileMenu.DropDownItems.Add(itemRemove);

            btnFileMenu.DropDownItems.Add(new ToolStripSeparator());

            var resetLayoutItem = new ToolStripMenuItem("Reset Quadrant View");
            resetLayoutItem.Click += (_, _) => ResetQuadrantsToMiddle();

            btnFileMenu.DropDownItems.Add(itemSave);
            btnFileMenu.DropDownItems.Add(itemRemove);
            btnFileMenu.DropDownItems.Add(resetLayoutItem);


            // Filter items - file type and sort
            //container panel
            var fileTypePanel = new Panel
            {
                Size = new Size(150, 220),
                Padding = new Padding(0),
                BackColor = SystemColors.Window
            };
            //toggle all file types
            var toggleLabel = new Label
            {
                Text = "Select All",
                Dock = DockStyle.Top,
                Height = 20,
                TextAlign = ContentAlignment.MiddleLeft,
                Margin = new Padding(6, 2, 6, 2),
                Font = new Font("Segoe UI", 8.25F, FontStyle.Regular),
                Cursor = Cursors.Hand,
                BackColor = SystemColors.ControlLight
            };
            //gives light hover effect
            toggleLabel.MouseEnter += (_, _) => toggleLabel.BackColor = SystemColors.ControlDark;
            toggleLabel.MouseLeave += (_, _) => toggleLabel.BackColor = SystemColors.ControlLight;


            // File Type Checklist
            this.fileTypeChecklist = new CheckedListBox
            {
                CheckOnClick = true,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill
            };

            //file type options
            string[] fileTypes = { ".png", ".jpg", ".jpeg", ".gif", ".webp", ".mp4", ".mov", ".psd", ".tiff", ".bmp", ".raw", ".heic" };
            //temp
            fileTypeChecklist.ItemCheck += (_, __) => DebugSelectedFileTypes();

            fileTypeChecklist.Items.AddRange(fileTypes);
            for (int i = 0; i < fileTypeChecklist.Items.Count; i++)
                fileTypeChecklist.SetItemChecked(i, true); // check all by default

            fileTypePanel.Controls.Add(fileTypeChecklist);
            fileTypePanel.Controls.Add(toggleLabel);

            // Toggle salect all click event
            toggleLabel.Click += (_, _) =>
            {
                bool allSelected = fileTypeChecklist.CheckedItems.Count == fileTypeChecklist.Items.Count;
                for (int i = 0; i < fileTypeChecklist.Items.Count; i++)
                    fileTypeChecklist.SetItemChecked(i, !allSelected);

                toggleLabel.Text = allSelected ? "Select All" : "Deselect All";
            };

            var host = new ToolStripControlHost(fileTypePanel)
            {
                AutoSize = false,
                Margin = Padding.Empty,
                Padding = Padding.Empty,
                Size = fileTypePanel.Size,
            };
            ToolStripDropDown checklistDropdown = new ToolStripDropDown();
            checklistDropdown.Items.Add(host);

            // Create the ToolStripDropDownButton
            btnFileTypes = new ToolStripDropDownButton("File Types");
            btnFileTypes.Click += (_, _) =>
            {
                var location = btnFileTypes.Owner.PointToScreen(new Point(btnFileTypes.Bounds.Left, btnFileTypes.Bounds.Bottom));
                checklistDropdown.Show(location);
            };

            // Filter items - file meta sort by dates x names
            this.lblSort = new ToolStripLabel("Sort by:");
            this.sortDropDown = new ToolStripComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 120
            };
            this.sortDropDown.Items.AddRange(new[]
            {
                "Date (Newest)", "Date (Oldest)",
                "Filename (A-Z)", "Filename (Z-A)"
            });
            this.sortDropDown.SelectedIndex = 0;

            // Workspace otpions dropdown
            this.lblWorkspace = new ToolStripLabel("Workspaces:");
            this.workspaceDropDown = new ToolStripComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 120
            };
            this.workspaceDropDown.Items.AddRange(new[]
            {
                "Default (Blank)", "Downloads", "Pictures"
            });
            this.workspaceDropDown.SelectedIndex = 0;
            this.workspaceDropDown.SelectedIndexChanged += (_, _) =>
            {
                LoadWorkspaceByName(workspaceDropDown.SelectedItem?.ToString());
            };

            //**********************************************************************//
            //END - TOOLSTRIP AT THE TOP
            //**********************************************************************//

            //**********************************************************************//
            //THE QUADRANTS
            //**********************************************************************//
            // 
            // verticalSplit
            // 
            this.verticalSplit.Dock = DockStyle.Fill;
            this.verticalSplit.Orientation = Orientation.Horizontal;
            this.verticalSplit.SplitterDistance = 360;
            this.verticalSplit.Name = "verticalSplit";

            // 
            // horizontalTop
            // 
            this.horizontalTop.Dock = DockStyle.Fill;
            this.horizontalTop.Orientation = Orientation.Vertical;
            this.horizontalTop.SplitterDistance = 360;
            this.horizontalTop.Name = "horizontalTop";

            // 
            // horizontalBottom
            // 
            this.horizontalBottom.Dock = DockStyle.Fill;
            this.horizontalBottom.Orientation = Orientation.Vertical;
            this.horizontalBottom.SplitterDistance = 360;
            this.horizontalBottom.Name = "horizontalBottom";

            // 
            // quadrantHostPanel
            // 
            this.quadrantHostPanel.Dock = DockStyle.Fill;
            this.quadrantHostPanel.BackColor = Color.Transparent;
            this.quadrantHostPanel.Padding = new Padding(0); // Top padding will be added dynamically
            this.quadrantHostPanel.Controls.Add(this.verticalSplit);

            // 
            // Nest split containers
            // 
            this.verticalSplit.Panel1.Controls.Add(this.horizontalTop);
            this.verticalSplit.Panel2.Controls.Add(this.horizontalBottom);
            // Track and sync vertical split ratio
            horizontalTop.SplitterMoved += (_, _) =>
            {
                horizontalRatioTop = (float)horizontalTop.SplitterDistance / ClientSize.Width;
                horizontalBottom.SplitterDistance = (int)(ClientSize.Width * horizontalRatioTop);
                horizontalRatioBottom = horizontalRatioTop;
            };

            horizontalBottom.SplitterMoved += (_, _) =>
            {
                horizontalRatioBottom = (float)horizontalBottom.SplitterDistance / ClientSize.Width;
                horizontalTop.SplitterDistance = (int)(ClientSize.Width * horizontalRatioBottom);
                horizontalRatioTop = horizontalRatioBottom;
            };

            verticalSplit.SplitterMoved += (_, _) =>
            {
                verticalRatio = (float)verticalSplit.SplitterDistance / ClientSize.Height;
            };



            //******************************************************************************//
            // UI - organize the buttons with a separator line
            //requires all buttons in order to show
            //******************************************************************************//
            toolStripMain.Renderer = new NoImageMarginRenderer();

            toolStripMain.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripSeparator(),
                new ToolStripSeparator(),
                btnFileMenu,
                new ToolStripSeparator(),
                btnAddFolder, btnRemoveFolder, btnRefresh,
                new ToolStripSeparator(),
                new ToolStripSeparator(),
                btnFileTypes,
                new ToolStripSeparator(),
                new ToolStripSeparator(),
                lblSort, sortDropDown,
                new ToolStripSeparator(),
                new ToolStripSeparator(),
                lblWorkspace, workspaceDropDown,
                new ToolStripSeparator(),
                new ToolStripSeparator(),
            });

            //**********************************************************************//
            //FORMMAIN
            //**********************************************************************//
            //
            this.AutoScaleMode = AutoScaleMode.Font;
            //default size of the form
            this.ClientSize = new Size(940, 720);
            this.Name = "FormMain";
            this.Text = "Creative File Browser";

            this.Controls.Add(this.quadrantHostPanel);
            this.Controls.Add(this.toolStripMain);
            this.Load += new EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

            // Layout on init
            this.AdjustLayout();
            this.ResetQuadrantsToMiddle();
        }

        //******************************************************************************//
        // Add Controls to Form (after all are initialized)
        //******************************************************************************//

        // ──────────────────────────────────────────────
        // Resize logic (moved into FormMain_Load)
        // ──────────────────────────────────────────────

        // Auto-resize handler // ask to use from the main form event / helper
    }

    //**********************************************************************//
    //TOOLSTRIP MENU IMAGE RENDERER
    //**********************************************************************//
    public class NoImageMarginRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            // Do nothing = no margin rendered
        }
    }
}
