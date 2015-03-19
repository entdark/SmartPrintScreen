using System;
using System.Drawing;

using System.IO;
using System.Windows.Forms;


namespace SmartPrintScreen {
	public partial class FormMain : Form {
		public FormMain() {
			InitializeComponent();
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
			hookPrintScreen.KeyPressed += new EventHandler<SmartPrintScreen.KeyPressedEventArgs>(CaptureMain);
			hookPrintScreen.HookedKeys.Add(captureKey);
			if (checkBoxHideOnStartup.Checked) {
				this.WindowState = FormWindowState.Minimized;
			}
		}
		
		private void FormMain_Load(object sender, EventArgs e) {
			if (checkBoxHideOnStartup.Checked) {
				this.Hide();
			}
		}
		
		private bool closingFromTray = false;
		private void FormMain_Closing(object sender, FormClosingEventArgs e) {
			if (closingFromTray) {
				// do nothing so we just closing
			} else if (e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				this.Hide();
			}
		}

		private void FormMain_Resize(object sender, EventArgs e) {
			if (this.WindowState == FormWindowState.Minimized) {
				this.WindowState = FormWindowState.Normal;
				this.Hide();
			}
		}
		
		private void fMouseDown_Click(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				cutBorder[0] = e.Location;
			}
		}
		private void fMouseUp_Click(object sender, MouseEventArgs e) {
			if (cutBorder[0] == e.Location) {
				cancelCapture = true;
				capturingCut = false;
			} else if (e.Button == MouseButtons.Left) {
				CutUpdate();
				capturingCut = false;
			}
		}
		
		private void fMouseMove_MouseClick(object sender, MouseEventArgs e) {
			Form f = (Form)sender;
			f.Refresh();
			IntPtr desktopPtr = GetDC(IntPtr.Zero);
			Graphics g = Graphics.FromHdc(desktopPtr);
			g.Dispose();
			ReleaseDC(IntPtr.Zero, desktopPtr);
			if (e.Button == MouseButtons.Left) {
				cutBorder[1] = new Point(cutBorder[0].X, e.Y);
				cutBorder[2] = e.Location;
				cutBorder[3] = new Point(e.X, cutBorder[0].Y);
				DrawLines(cutBorder, Color.Green);
			}
		}

		private void fKeyDown_KeyPress(object sender, KeyEventArgs e) {
			e.Handled = true;
			cancelCapture = true;
			capturingCut = false;
		}

		void notifyIconTray_BalloonTipClicked(object sender, System.EventArgs e) {
			if (Clipboard.ContainsText())
				System.Diagnostics.Process.Start(Clipboard.GetText());
		}

		private void notifyIconTray_MouseClick(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				if (!this.Visible) {
					this.Show();
					this.WindowState = FormWindowState.Normal;
					//mighty hack..... although, is there a better way?
					this.TopMost = true;
					this.TopMost = false;
				} else {
					this.Hide();
				}
			} else if (e.Button == MouseButtons.Right) {
				if (Screen.PrimaryScreen.Bounds.Height - Cursor.Position.Y <= 38) {
					contextMenuStripTray.Show(Cursor.Position, ToolStripDropDownDirection.AboveRight);
				} else {
					contextMenuStripTray.Show(Cursor.Position, ToolStripDropDownDirection.Default);
				}
			}
		}
		
		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			closingFromTray = true;
			this.Close();
		}

		private void checkBoxModifierWin_CheckedChanged(object sender, EventArgs e) {
			if (checkBoxModifierWin.Checked) {
				modifierKeys |= SmartPrintScreen.ModifierKeys.Win;
			} else {
				modifierKeys &= ~(SmartPrintScreen.ModifierKeys.Win);
			}
			if (modifierKeys == SmartPrintScreen.ModifierKeys.None) {
				System.Media.SystemSounds.Beep.Play();
				checkBoxModifierWin.Checked = true;
				modifierKeys |= SmartPrintScreen.ModifierKeys.Win;
			}
		}

		private void checkBoxModifierCtrl_CheckedChanged(object sender, EventArgs e) {
			if (checkBoxModifierCtrl.Checked) {
				modifierKeys |= SmartPrintScreen.ModifierKeys.Ctrl;
			} else {
				modifierKeys &= ~(SmartPrintScreen.ModifierKeys.Ctrl);
			}
			if (modifierKeys == SmartPrintScreen.ModifierKeys.None) {
				System.Media.SystemSounds.Beep.Play();
				checkBoxModifierCtrl.Checked = true;
				modifierKeys |= SmartPrintScreen.ModifierKeys.Ctrl;
			}
		}

		private void checkBoxModifierShift_CheckedChanged(object sender, EventArgs e) {
			if (checkBoxModifierShift.Checked) {
				modifierKeys |= SmartPrintScreen.ModifierKeys.Shift;
			} else {
				modifierKeys &= ~(SmartPrintScreen.ModifierKeys.Shift);
			}
			if (modifierKeys == SmartPrintScreen.ModifierKeys.None) {
				System.Media.SystemSounds.Beep.Play();
				checkBoxModifierShift.Checked = true;
				modifierKeys |= SmartPrintScreen.ModifierKeys.Shift;
			}
		}

		//Alt+PrintScreen is used to capture focused window in Windows, let's leave this behavior
/*		private void checkBoxModifierAlt_CheckedChanged(object sender, EventArgs e) {
			if (checkBoxModifierAlt.Checked) {
				modifierKeys |= SmartPrintScreen.ModifierKeys.Alt;
			} else {
				modifierKeys &= ~(SmartPrintScreen.ModifierKeys.Alt);
			}
			if (modifierKeys == SmartPrintScreen.ModifierKeys.None) {
				System.Media.SystemSounds.Beep.Play();
				checkBoxModifierAlt.Checked = true;
				modifierKeys |= SmartPrintScreen.ModifierKeys.Alt;
			}
		}*/

		private void listBoxShotURLs_MouseClick(object sender, MouseEventArgs e) {
			listBoxShotURLs.SelectedIndex = listBoxShotURLs.IndexFromPoint(e.X, e.Y);
			if (e.Button == MouseButtons.Right) {
				if (listBoxShotURLs.Items.Count <= 0) {
					clearListToolStripMenuItem.Enabled = false;
				} else {
					clearListToolStripMenuItem.Enabled = true;
				}
				if (listBoxShotURLs.SelectedIndex < 0) {
					openInBrowserToolStripMenuItem.Enabled = false;
					copyToolStripMenuItem.Enabled = false;
				} else {
					openInBrowserToolStripMenuItem.Enabled = true;
					copyToolStripMenuItem.Enabled = true;
				}
				contextMenuStripShotURLs.Show(Cursor.Position);
			}
		}

		private void listBoxShotURLs_MouseDoubleClick(object sender, MouseEventArgs e) {
			OpenURLinBrowser();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
			CopyURL();
		}

		private void openInBrowserToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenURLinBrowser();
		}

		private void clearListToolStripMenuItem_Click(object sender, EventArgs e) {
			listBoxShotURLs.Items.Clear();
		}

		private void listBoxShotURLs_KeyDown(object sender, KeyEventArgs e) {
			if (e.Control && e.KeyCode == Keys.C) {
				e.Handled = true;
				CopyURL();
			}
		}

		private void openSmartPrintScreenToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Show();
		}
	}
}
