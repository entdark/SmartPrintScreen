using System;
using System.Drawing;

using System.IO;
using System.Windows.Forms;

using IniParser;
using IniParser.Model;

namespace SmartPrintScreen {
	public partial class FormMain : Form {
		public const string programName = "SmartPrintScreen";
		readonly string iniPath;
		public FormMain() {
			InitializeComponent();
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);

			hookPrintScreen.KeyPressed += new EventHandler<SmartPrintScreen.KeyPressedEventArgs>(CaptureMain);
			hookPrintScreen.HookedKeys.Add(captureKey);

			iniPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), programName);

			if (File.Exists(Path.Combine(iniPath, programName + ".ini"))) {
				try {
					LoadIniData(Path.Combine(iniPath, programName + ".ini"));
				} catch (Exception ex) {
					MessageBox.Show(ex.Message);
				}
			}
			if (checkBoxHideOnStartup.Checked) {
				this.WindowState = FormWindowState.Minimized;
			}
		}
		
		private void FormMain_Load(object sender, EventArgs e) {
			if (checkBoxHideOnStartup.Checked) {
				this.Hide();
			}
		}
		
		private void LoadIniData(string path) {
			FileIniDataParser ini = new FileIniDataParser();
			IniData data = ini.ReadFile(path);
			//General
			if (data["General"]["Clipboard"].ToLower() == "screenshot") {
				radioButtonClipboardScreenshot.Checked = true;
			} else {
				radioButtonClipboardURL.Checked = true;
			}
			modifierKeys = SmartPrintScreen.ModifierKeys.None;
			if (checkBoxModifierShift.Checked = data["General"]["Modifiers"].ToLower().Contains("shift")) {
				modifierKeys |= SmartPrintScreen.ModifierKeys.Shift;
			}
			if (checkBoxModifierCtrl.Checked = data["General"]["Modifiers"].ToLower().Contains("ctrl")) {
				modifierKeys |= SmartPrintScreen.ModifierKeys.Ctrl;
			}
			if (checkBoxModifierWin.Checked = data["General"]["Modifiers"].ToLower().Contains("win")) {
				modifierKeys |= SmartPrintScreen.ModifierKeys.Win;
			}
			checkBoxUpload.Checked = bool.Parse(data["General"]["Upload"]);
			checkBoxNotifications.Checked = bool.Parse(data["General"]["Notifications"]);
			checkBoxHideOnStartup.Checked = bool.Parse(data["General"]["HideOnStartup"]);
			checkBoxSaveURLsList.Checked = bool.Parse(data["General"]["SaveURLsList"]);
			//Screenshot URLs
			int count = int.Parse(data["ScreenshotURLs"]["Count"]);
			if (checkBoxSaveURLsList.Checked) {
				if (count > 0) {
					for (int i = count-1; i >= 0; i--) {
						string url = data["ScreenshotURLs"]["URL" + i.ToString()];
						//TODO: add more conditions to check URLs
						if (url.Contains("http"))
							InsertShotURLtoListBox(url);
					}
				}
			}
		}

		private void SaveIniData(string path) {
			FileIniDataParser ini = new FileIniDataParser();
			IniData data = new IniData();
			//General
			data.Sections.AddSection("General");
			data["General"].AddKey("Clipboard", radioButtonClipboardScreenshot.Checked ? "Screenshot" : "URL");
			string modifiers = "";
			if ((modifierKeys & SmartPrintScreen.ModifierKeys.Win) != 0) {
				modifiers = "Win";
				if (modifierKeys == SmartPrintScreen.ModifierKeys.Win)
					goto skipRestModifiers;
				else
					modifiers += "+";
			}
			if ((modifierKeys & SmartPrintScreen.ModifierKeys.Ctrl) != 0) {
				modifiers += "Ctrl";
				if (modifierKeys == SmartPrintScreen.ModifierKeys.Ctrl)
					goto skipRestModifiers;
				else
					modifiers += "+";
			}
			if ((modifierKeys & SmartPrintScreen.ModifierKeys.Shift) != 0)
				modifiers += "Shift";
skipRestModifiers:
			data["General"].AddKey("Modifiers", modifiers);
			data["General"].AddKey("Upload", checkBoxUpload.Checked ? "True" : "False");
			data["General"].AddKey("Notifications", checkBoxNotifications.Checked ? "True" : "False");
			data["General"].AddKey("HideOnStartup", checkBoxHideOnStartup.Checked ? "True" : "False");
			data["General"].AddKey("SaveURLsList", checkBoxSaveURLsList.Checked ? "True" : "False");
			//Screenshot URLs
			data.Sections.AddSection("ScreenshotURLs");
			int count = listBoxShotURLs.Items.Count;
			data["ScreenshotURLs"].AddKey("Count", count.ToString());
			if (checkBoxSaveURLsList.Checked) {
				if (count > 0) {
					for (int i = 0; i < count; i++) {
						data["ScreenshotURLs"].AddKey("URL" + i.ToString(), listBoxShotURLs.Items[i].ToString());
					}
				}
			}
			Directory.CreateDirectory(path);
			ini.WriteFile(Path.Combine(path, programName + ".ini"), data);
		}

		private bool closingFromTray = false;
		private void FormMain_Closing(object sender, FormClosingEventArgs e) {
			if (closingFromTray) {
				// do nothing so we just closing
			} else if (e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				this.Hide();
				return;
			}
			try {
				SaveIniData(iniPath);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
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
					deleteToolStripMenuItem.Enabled = false;
				} else {
					openInBrowserToolStripMenuItem.Enabled = true;
					copyToolStripMenuItem.Enabled = true;
					deleteToolStripMenuItem.Enabled = true;
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

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
			int i = listBoxShotURLs.SelectedIndex;
			if (i >= 0 && i < listBoxShotURLs.Items.Count)
				listBoxShotURLs.Items.RemoveAt(i);
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
