using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Shapes;

using System.Runtime.InteropServices;
using System.Drawing.Imaging;

using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace SmartPrintScreen {
	public partial class FormMain : Form {
		//hardcoded, do not change
		private const string ClientId = "0e5974b59887a08";
		
		[StructLayout(LayoutKind.Sequential)]
		public struct RECT {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}
		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect); 

		[DllImport("User32.dll")]
		public static extern IntPtr GetDC(IntPtr hwnd);
		[DllImport("User32.dll")]
		public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
		
        GlobalKeyboardHook hookPrintScreen = new GlobalKeyboardHook();

		public FormMain() {
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
			InitializeComponent();
			hookPrintScreen.KeyPressed += new EventHandler<SmartPrintScreen.KeyPressedEventArgs>(CaptureMain);
			hookPrintScreen.HookedKeys.Add(captureKey);
			this.Hide();

		}
		
		private Point[] cutBorder = new Point[4];
		public Rectangle cutScreenshot { get; set; }
		private void CutUpdate() {
			int fpX = cutBorder[0].X, spX = cutBorder[2].X, fpY = cutBorder[0].Y, spY = cutBorder[2].Y;
			// now let's set the selection rectangle corners
			if (fpX >= spX && fpY >= spY) {
				cutScreenshot = new Rectangle(spX, spY, fpX-spX+1, fpY-spY+1);
			} else if (fpX >= spX && fpY <= spY) {
				cutScreenshot = new Rectangle(spX, fpY, fpX-spX+1, spY-fpY+1);
			} else if (fpX <= spX && fpY <= spY) {
				cutScreenshot = new Rectangle(fpX, fpY, spX-fpX+1, spY-fpY+1);
			} else if (fpX <= spX && fpY >= spY) {
				cutScreenshot = new Rectangle(fpX, spY, spX-fpX+1, fpY-spY+1);
			} else {
				throw new Exception(System.String.Format("something wrong with selection coordinates: {0} and {1}", cutBorder[0], cutBorder[2]));
			}
		}
		
		private const Keys captureKey = Keys.PrintScreen;
		private SmartPrintScreen.ModifierKeys modifierKeys = SmartPrintScreen.ModifierKeys.Win;

		private void FormMain_Load(object sender, EventArgs e) {
			this.Hide();
		}
		
		private bool ClosingFromTray = false;
		private void FormMain_Closing(object sender, FormClosingEventArgs e) {
			if (ClosingFromTray) {
				// do nothing so we just closing
			} else if (e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				this.Hide();
			}
		}

		private void FormMain_Resize(object sender, EventArgs e) {
			if (this.WindowState == FormWindowState.Minimized) {
				this.Hide();
			}
		}
		
		private bool capturingCut = false;
		private async void CaptureMain(object sender, KeyPressedEventArgs e) {
			if (e.Modifier == SmartPrintScreen.ModifierKeys.Alt) {
				await CaptureWindow(sender, e);
			} else if (e.Modifier == modifierKeys) {
				//do not let to make several instances of capturing cut screenshot
				if (!capturingCut)
					await CaptureCut(sender, e);
			} else {
				await CaptureShot(new Point(0, 0), Screen.PrimaryScreen.Bounds.Size);
			}
			GC.Collect();
		}
		
		private async Task CaptureWindow(object sender, KeyPressedEventArgs e) {
			RECT focusedWindow;
			GetWindowRect(GetForegroundWindow(), out focusedWindow);
			Rectangle r = new Rectangle(focusedWindow.Left, focusedWindow.Top, focusedWindow.Right-focusedWindow.Left, focusedWindow.Bottom-focusedWindow.Top);
			await CaptureShot(r.Location, r.Size);
		}
		
		private bool cancelCapture = false;
		private async Task CaptureCut(object sender, KeyPressedEventArgs e) {
			using (Form f = new Form()) {
				f.StartPosition = FormStartPosition.Manual;
				f.Location = new Point(0, 0);
				f.Size = Screen.PrimaryScreen.Bounds.Size;
				f.FormBorderStyle = FormBorderStyle.None;
				f.Opacity = 0.004;
				f.BackColor = Color.Black;
				f.TopMost = true;
				f.Cursor = Cursors.Cross;
				f.MouseDown += new System.Windows.Forms.MouseEventHandler(fMouseDown_Click);
				f.MouseUp += new System.Windows.Forms.MouseEventHandler(fMouseUp_Click);
				f.MouseMove += new System.Windows.Forms.MouseEventHandler(fMouseMove_MouseClick);
				f.KeyPress += new System.Windows.Forms.KeyPressEventHandler(fKeyPress_Press);
				f.ShowInTaskbar = false;
				f.ShowIcon = false;
				f.Show();
				capturingCut = true;
				//another mighty hack
				await Task.Run(() => {
					while (true) {
						if (!capturingCut)
							break;
					}
				});
				f.Close();
			}
			capturingCut = false;
			if (!cancelCapture)
				await CaptureShot(cutScreenshot.Location, cutScreenshot.Size);
			cancelCapture = false;
		}
		
		private async Task CaptureShot(Point location, Size size) {
			try {
				using (Bitmap screenShot = new Bitmap(size.Width, size.Height)) {
					using (Graphics g = Graphics.FromImage(screenShot)) {
						g.CopyFromScreen(location, new Point(0, 0), size);
					}
					string url = "";
				
					// copy Image to clipboard
					using (MemoryStream ms = new MemoryStream()) {
						screenShot.Save(ms, ImageFormat.Bmp);
						Clipboard.SetImage(Image.FromStream(ms));
					}
				
					if (checkBoxUpload.Checked) {
						//since uploading takes some time let's just keep Screenshot in the clipboard until we upload
						ShowBalloonTip("Uploading to imgur.com", "Screenshot copied to clipboard");
						url = await GetUploadedShotURL(screenShot);
						if (url == "failed") {
							screenShot.Dispose();
							return;
						}
						InsertShotURLtoListBox(url);
					}
					screenShot.Dispose();
					if (radioButtonClipboardURL.Checked && checkBoxUpload.Checked) {
						Clipboard.SetText(url);
						ShowBalloonTip("Screenshot uploaded", String.Format("{0} copied to clipboard", url));
					} else if (checkBoxUpload.Checked) {
						ShowBalloonTip("Screenshot uploaded", "Screenshot copied to clipboard");
					} else {
						ShowBalloonTip("Screenshot captured", "Screenshot copied to clipboard");
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
		
		private void fMouseDown_Click(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				cutBorder[0] = e.Location;
			}
		}
		private void fMouseUp_Click(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				capturingCut = false;
				CutUpdate();
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

		private void DrawLines(Point[] points, Color colour) {
			IntPtr desktopPtr = GetDC(IntPtr.Zero);
			using (Graphics g = Graphics.FromHdc(desktopPtr)) {
				using (Pen p = new Pen(colour, 2)) {
					g.DrawLine(p, points[0], points[1]);
					g.DrawLine(p, points[1], points[2]);
					g.DrawLine(p, points[2], points[3]);
					g.DrawLine(p, points[3], points[0]);
				}
			}
			ReleaseDC(IntPtr.Zero, desktopPtr);
		}

		private void fKeyPress_Press(object sender, KeyPressEventArgs e) {
			cancelCapture = true;
			capturingCut = false;
		}

		private void ShowBalloonTip(string title, string text, int timeout = 1337) {
			if (!checkBoxNotifications.Checked)
				return;
			notifyIconTray.BalloonTipTitle = title;
			notifyIconTray.BalloonTipText = text;
			notifyIconTray.ShowBalloonTip(timeout);
		}

		//based on http://pc-tips.net/imgur-api-vb-net/
		private async Task<string> GetUploadedShotURL(Bitmap image) {
			return await Task.Run(() => {
			try {
				using (WebClient w = new WebClient()) {
					w.Headers.Add("Authorization", "Client-ID " + ClientId);
					System.Collections.Specialized.NameValueCollection Keys = new System.Collections.Specialized.NameValueCollection();
					using (MemoryStream ms = new MemoryStream()) {
						image.Save(ms, ImageFormat.Bmp);
						byte[] byteStream = ms.ToArray();
						Keys.Add("image", Convert.ToBase64String(byteStream));
					}
					byte[] responseArray = w.UploadValues("https://api.imgur.com/3/image", Keys);
					dynamic result = Encoding.ASCII.GetString(responseArray);
					Regex reg = new Regex("link\":\"(.*?)\"");
					Match match = reg.Match(result);
					string url = match.ToString().Replace("link\":\"", "").Replace("\"", "").Replace("\\/", "/");
					return url;
				}
			} catch (Exception ex) {
				MessageBox.Show("Failed to upload to imgur.com\n" + ex.Message);
				return "failed";
			}
			});
		}

		private void InsertShotURLtoListBox(string url) {
			listBoxShotURLs.Items.Insert(0, url);
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
				contextMenuStripTray.Show(Cursor.Position);
			}
		}
		
		private void notifyIconTrayMenuItem_Click(object sender, EventArgs e) {
			ClosingFromTray = true;
			this.Close();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			ClosingFromTray = true;
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
				if (listBoxShotURLs.SelectedIndex < 0) {
					contextMenuStripShotURLs.Enabled = false;
				} else {
					contextMenuStripShotURLs.Enabled = true;
				}
				contextMenuStripShotURLs.Show(Cursor.Position);
			}
		}

		private void listBoxShotURLs_MouseDoubleClick(object sender, MouseEventArgs e) {
			OpenURLinBrowser();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
			if (listBoxShotURLs.SelectedIndex < 0)
				return;
			string url = listBoxShotURLs.Items[listBoxShotURLs.SelectedIndex].ToString();
			Clipboard.SetText(url);
			ShowBalloonTip("URL copied", String.Format("{0} copied to clipboard", url));
		}

		private void openInBrowserToolStripMenuItem_Click(object sender, EventArgs e) {
			OpenURLinBrowser();
		}

		private void OpenURLinBrowser() {
			if (listBoxShotURLs.SelectedIndex < 0)
				return;
			string url = listBoxShotURLs.Items[listBoxShotURLs.SelectedIndex].ToString();
			System.Diagnostics.Process.Start(url);
		}
	}
}
