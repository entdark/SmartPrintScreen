namespace SmartPrintScreen {
	partial class FormMain {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.labelShotURLs = new System.Windows.Forms.Label();
			this.groupBoxModifiers = new System.Windows.Forms.GroupBox();
			this.checkBoxModifierWin = new System.Windows.Forms.CheckBox();
			this.checkBoxModifierShift = new System.Windows.Forms.CheckBox();
			this.checkBoxModifierCtrl = new System.Windows.Forms.CheckBox();
			this.groupBoxClipboard = new System.Windows.Forms.GroupBox();
			this.radioButtonClipboardURL = new System.Windows.Forms.RadioButton();
			this.radioButtonClipboardScreenshot = new System.Windows.Forms.RadioButton();
			this.notifyIconTray = new System.Windows.Forms.NotifyIcon(this.components);
			this.contextMenuStripTray = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openSmartPrintScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkBoxNotifications = new System.Windows.Forms.CheckBox();
			this.listBoxShotURLs = new System.Windows.Forms.ListBox();
			this.contextMenuStripShotURLs = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openInBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkBoxUpload = new System.Windows.Forms.CheckBox();
			this.checkBoxSaveURLsList = new System.Windows.Forms.CheckBox();
			this.checkBoxHideOnStartup = new System.Windows.Forms.CheckBox();
			this.groupBoxModifiers.SuspendLayout();
			this.groupBoxClipboard.SuspendLayout();
			this.contextMenuStripTray.SuspendLayout();
			this.contextMenuStripShotURLs.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelShotURLs
			// 
			this.labelShotURLs.AutoSize = true;
			this.labelShotURLs.Location = new System.Drawing.Point(116, 9);
			this.labelShotURLs.Name = "labelShotURLs";
			this.labelShotURLs.Size = new System.Drawing.Size(94, 13);
			this.labelShotURLs.TabIndex = 4;
			this.labelShotURLs.Text = "Screenshot URLs:";
			// 
			// groupBoxModifiers
			// 
			this.groupBoxModifiers.Controls.Add(this.checkBoxModifierWin);
			this.groupBoxModifiers.Controls.Add(this.checkBoxModifierShift);
			this.groupBoxModifiers.Controls.Add(this.checkBoxModifierCtrl);
			this.groupBoxModifiers.Location = new System.Drawing.Point(10, 70);
			this.groupBoxModifiers.Name = "groupBoxModifiers";
			this.groupBoxModifiers.Size = new System.Drawing.Size(100, 63);
			this.groupBoxModifiers.TabIndex = 5;
			this.groupBoxModifiers.TabStop = false;
			this.groupBoxModifiers.Text = "Cut Modifiers";
			// 
			// checkBoxModifierWin
			// 
			this.checkBoxModifierWin.AutoSize = true;
			this.checkBoxModifierWin.Checked = true;
			this.checkBoxModifierWin.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxModifierWin.Location = new System.Drawing.Point(53, 17);
			this.checkBoxModifierWin.Name = "checkBoxModifierWin";
			this.checkBoxModifierWin.Size = new System.Drawing.Size(45, 17);
			this.checkBoxModifierWin.TabIndex = 3;
			this.checkBoxModifierWin.Text = "Win";
			this.checkBoxModifierWin.UseVisualStyleBackColor = true;
			this.checkBoxModifierWin.CheckedChanged += new System.EventHandler(this.checkBoxModifierWin_CheckedChanged);
			this.checkBoxModifierWin.CheckedChanged += new System.EventHandler(this.saveOnOptionChanged);
			// 
			// checkBoxModifierShift
			// 
			this.checkBoxModifierShift.AutoSize = true;
			this.checkBoxModifierShift.Location = new System.Drawing.Point(6, 27);
			this.checkBoxModifierShift.Name = "checkBoxModifierShift";
			this.checkBoxModifierShift.Size = new System.Drawing.Size(47, 17);
			this.checkBoxModifierShift.TabIndex = 2;
			this.checkBoxModifierShift.Text = "Shift";
			this.checkBoxModifierShift.UseVisualStyleBackColor = true;
			this.checkBoxModifierShift.CheckedChanged += new System.EventHandler(this.checkBoxModifierShift_CheckedChanged);
			this.checkBoxModifierShift.CheckedChanged += new System.EventHandler(this.saveOnOptionChanged);
			// 
			// checkBoxModifierCtrl
			// 
			this.checkBoxModifierCtrl.AutoSize = true;
			this.checkBoxModifierCtrl.Location = new System.Drawing.Point(53, 40);
			this.checkBoxModifierCtrl.Name = "checkBoxModifierCtrl";
			this.checkBoxModifierCtrl.Size = new System.Drawing.Size(41, 17);
			this.checkBoxModifierCtrl.TabIndex = 0;
			this.checkBoxModifierCtrl.Text = "Ctrl";
			this.checkBoxModifierCtrl.UseVisualStyleBackColor = true;
			this.checkBoxModifierCtrl.CheckedChanged += new System.EventHandler(this.checkBoxModifierCtrl_CheckedChanged);
			this.checkBoxModifierCtrl.CheckedChanged += new System.EventHandler(this.saveOnOptionChanged);
			// 
			// groupBoxClipboard
			// 
			this.groupBoxClipboard.Controls.Add(this.radioButtonClipboardURL);
			this.groupBoxClipboard.Controls.Add(this.radioButtonClipboardScreenshot);
			this.groupBoxClipboard.Location = new System.Drawing.Point(10, 9);
			this.groupBoxClipboard.Name = "groupBoxClipboard";
			this.groupBoxClipboard.Size = new System.Drawing.Size(100, 59);
			this.groupBoxClipboard.TabIndex = 4;
			this.groupBoxClipboard.TabStop = false;
			this.groupBoxClipboard.Text = "Clipboard";
			// 
			// radioButtonClipboardURL
			// 
			this.radioButtonClipboardURL.AutoSize = true;
			this.radioButtonClipboardURL.Checked = true;
			this.radioButtonClipboardURL.Location = new System.Drawing.Point(6, 36);
			this.radioButtonClipboardURL.Name = "radioButtonClipboardURL";
			this.radioButtonClipboardURL.Size = new System.Drawing.Size(47, 17);
			this.radioButtonClipboardURL.TabIndex = 1;
			this.radioButtonClipboardURL.TabStop = true;
			this.radioButtonClipboardURL.Text = "URL";
			this.radioButtonClipboardURL.UseVisualStyleBackColor = true;
			this.radioButtonClipboardURL.CheckedChanged += new System.EventHandler(this.saveOnOptionChanged);
			// 
			// radioButtonClipboardScreenshot
			// 
			this.radioButtonClipboardScreenshot.AutoSize = true;
			this.radioButtonClipboardScreenshot.Location = new System.Drawing.Point(6, 15);
			this.radioButtonClipboardScreenshot.Name = "radioButtonClipboardScreenshot";
			this.radioButtonClipboardScreenshot.Size = new System.Drawing.Size(79, 17);
			this.radioButtonClipboardScreenshot.TabIndex = 0;
			this.radioButtonClipboardScreenshot.Text = "Screenshot";
			this.radioButtonClipboardScreenshot.UseVisualStyleBackColor = true;
			this.radioButtonClipboardScreenshot.CheckedChanged += new System.EventHandler(this.saveOnOptionChanged);
			// 
			// notifyIconTray
			// 
			this.notifyIconTray.ContextMenuStrip = this.contextMenuStripTray;
			this.notifyIconTray.Icon = new System.Drawing.Icon((System.Drawing.Icon)(resources.GetObject("notifyIconTray.Icon")), 16, 16);
			this.notifyIconTray.Text = "SmartPrintScreen";
			this.notifyIconTray.Visible = true;
			this.notifyIconTray.BalloonTipClicked += new System.EventHandler(this.notifyIconTray_BalloonTipClicked);
			this.notifyIconTray.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIconTray_MouseClick);
			// 
			// contextMenuStripTray
			// 
			this.contextMenuStripTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openSmartPrintScreenToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.contextMenuStripTray.Name = "contextMenuStripTray";
			this.contextMenuStripTray.Size = new System.Drawing.Size(198, 54);
			// 
			// openSmartPrintScreenToolStripMenuItem
			// 
			this.openSmartPrintScreenToolStripMenuItem.Name = "openSmartPrintScreenToolStripMenuItem";
			this.openSmartPrintScreenToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
			this.openSmartPrintScreenToolStripMenuItem.Text = "Open SmartPrintScreen";
			this.openSmartPrintScreenToolStripMenuItem.Click += new System.EventHandler(this.openSmartPrintScreenToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(194, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// checkBoxNotifications
			// 
			this.checkBoxNotifications.AutoSize = true;
			this.checkBoxNotifications.Checked = true;
			this.checkBoxNotifications.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxNotifications.Location = new System.Drawing.Point(119, 116);
			this.checkBoxNotifications.Name = "checkBoxNotifications";
			this.checkBoxNotifications.Size = new System.Drawing.Size(84, 17);
			this.checkBoxNotifications.TabIndex = 6;
			this.checkBoxNotifications.Text = "Notifications";
			this.checkBoxNotifications.UseVisualStyleBackColor = true;
			this.checkBoxNotifications.CheckedChanged += new System.EventHandler(this.saveOnOptionChanged);
			// 
			// listBoxShotURLs
			// 
			this.listBoxShotURLs.ContextMenuStrip = this.contextMenuStripShotURLs;
			this.listBoxShotURLs.FormattingEnabled = true;
			this.listBoxShotURLs.Location = new System.Drawing.Point(119, 25);
			this.listBoxShotURLs.Name = "listBoxShotURLs";
			this.listBoxShotURLs.Size = new System.Drawing.Size(192, 69);
			this.listBoxShotURLs.TabIndex = 7;
			this.listBoxShotURLs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxShotURLs_KeyDown);
			this.listBoxShotURLs.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBoxShotURLs_MouseDoubleClick);
			this.listBoxShotURLs.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listBoxShotURLs_MouseClick);
			// 
			// contextMenuStripShotURLs
			// 
			this.contextMenuStripShotURLs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openInBrowserToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.clearListToolStripMenuItem});
			this.contextMenuStripShotURLs.Name = "contextMenuStripShotURLs";
			this.contextMenuStripShotURLs.Size = new System.Drawing.Size(162, 92);
			// 
			// openInBrowserToolStripMenuItem
			// 
			this.openInBrowserToolStripMenuItem.Name = "openInBrowserToolStripMenuItem";
			this.openInBrowserToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.openInBrowserToolStripMenuItem.Text = "Open in browser";
			this.openInBrowserToolStripMenuItem.Click += new System.EventHandler(this.openInBrowserToolStripMenuItem_Click);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			// 
			// clearListToolStripMenuItem
			// 
			this.clearListToolStripMenuItem.Name = "clearListToolStripMenuItem";
			this.clearListToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.clearListToolStripMenuItem.Text = "Clear list";
			this.clearListToolStripMenuItem.Click += new System.EventHandler(this.clearListToolStripMenuItem_Click);
			// 
			// checkBoxUpload
			// 
			this.checkBoxUpload.AutoSize = true;
			this.checkBoxUpload.Checked = true;
			this.checkBoxUpload.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxUpload.Location = new System.Drawing.Point(119, 100);
			this.checkBoxUpload.Name = "checkBoxUpload";
			this.checkBoxUpload.Size = new System.Drawing.Size(88, 17);
			this.checkBoxUpload.TabIndex = 8;
			this.checkBoxUpload.Text = "Upload shots";
			this.checkBoxUpload.UseVisualStyleBackColor = true;
			this.checkBoxUpload.CheckedChanged += new System.EventHandler(this.saveOnOptionChanged);
			// 
			// checkBoxSaveURLsList
			// 
			this.checkBoxSaveURLsList.AutoSize = true;
			this.checkBoxSaveURLsList.Checked = true;
			this.checkBoxSaveURLsList.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxSaveURLsList.Location = new System.Drawing.Point(213, 100);
			this.checkBoxSaveURLsList.Name = "checkBoxSaveURLsList";
			this.checkBoxSaveURLsList.Size = new System.Drawing.Size(96, 17);
			this.checkBoxSaveURLsList.TabIndex = 9;
			this.checkBoxSaveURLsList.Text = "Save URLs list";
			this.checkBoxSaveURLsList.UseVisualStyleBackColor = true;
			this.checkBoxSaveURLsList.CheckedChanged += new System.EventHandler(this.saveOnOptionChanged);
			// 
			// checkBoxHideOnStartup
			// 
			this.checkBoxHideOnStartup.AutoSize = true;
			this.checkBoxHideOnStartup.Location = new System.Drawing.Point(213, 116);
			this.checkBoxHideOnStartup.Name = "checkBoxHideOnStartup";
			this.checkBoxHideOnStartup.Size = new System.Drawing.Size(98, 17);
			this.checkBoxHideOnStartup.TabIndex = 10;
			this.checkBoxHideOnStartup.Text = "Hide on startup";
			this.checkBoxHideOnStartup.UseVisualStyleBackColor = true;
			this.checkBoxHideOnStartup.CheckedChanged += new System.EventHandler(this.saveOnOptionChanged);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(321, 142);
			this.Controls.Add(this.checkBoxHideOnStartup);
			this.Controls.Add(this.checkBoxSaveURLsList);
			this.Controls.Add(this.checkBoxUpload);
			this.Controls.Add(this.listBoxShotURLs);
			this.Controls.Add(this.checkBoxNotifications);
			this.Controls.Add(this.groupBoxClipboard);
			this.Controls.Add(this.groupBoxModifiers);
			this.Controls.Add(this.labelShotURLs);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SmartPrintScreen";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_Closing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.Resize += new System.EventHandler(this.FormMain_Resize);
			this.groupBoxModifiers.ResumeLayout(false);
			this.groupBoxModifiers.PerformLayout();
			this.groupBoxClipboard.ResumeLayout(false);
			this.groupBoxClipboard.PerformLayout();
			this.contextMenuStripTray.ResumeLayout(false);
			this.contextMenuStripShotURLs.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label labelShotURLs;
		private System.Windows.Forms.GroupBox groupBoxModifiers;
		private System.Windows.Forms.CheckBox checkBoxModifierWin;
		private System.Windows.Forms.CheckBox checkBoxModifierShift;
		private System.Windows.Forms.CheckBox checkBoxModifierCtrl;
		private System.Windows.Forms.GroupBox groupBoxClipboard;
		private System.Windows.Forms.RadioButton radioButtonClipboardURL;
		private System.Windows.Forms.RadioButton radioButtonClipboardScreenshot;
		private System.Windows.Forms.NotifyIcon notifyIconTray;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripTray;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.CheckBox checkBoxNotifications;
		private System.Windows.Forms.ListBox listBoxShotURLs;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripShotURLs;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.CheckBox checkBoxUpload;
		private System.Windows.Forms.ToolStripMenuItem openInBrowserToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearListToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openSmartPrintScreenToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.CheckBox checkBoxSaveURLsList;
		private System.Windows.Forms.CheckBox checkBoxHideOnStartup;
	}
}

