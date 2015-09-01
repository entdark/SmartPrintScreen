using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing;
using System.Drawing.Imaging;

using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace SmartPrintScreen {
	public partial class FormMain : Form {
		//hardcoded, do not change
		private const string ClientId = "0e5974b59887a08";
		
		[StructLayout(LayoutKind.Sequential)]
		private struct RECT {
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}
		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();
		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect); 

		[DllImport("User32.dll")]
		private static extern IntPtr GetDC(IntPtr hwnd);
		[DllImport("User32.dll")]
		private static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
		
        GlobalKeyboardHook hookPrintScreen = new GlobalKeyboardHook();
		
		private const Keys captureKey = Keys.PrintScreen;
		private SmartPrintScreen.ModifierKeys modifierKeys = SmartPrintScreen.ModifierKeys.Win;
		
		private Point[] cutBorder = new Point[4];
		private Rectangle cutScreenshot;
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
				throw new Exception(System.String.Format("Something wrong with selection coordinates: {0} and {1}", cutBorder[0], cutBorder[2]));
			}
		}
		
		private static bool capturingCut = false;
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
			//fullscreen windows have out of screen borders and the window screenshot can have white borders around, so we have to clamp
			if (focusedWindow.Left < 0)
				focusedWindow.Left = 0;
			if (focusedWindow.Top < 0)
				focusedWindow.Top = 0;
			if (focusedWindow.Right > Screen.PrimaryScreen.Bounds.Width)
				focusedWindow.Right = Screen.PrimaryScreen.Bounds.Width;
			if (focusedWindow.Bottom > Screen.PrimaryScreen.Bounds.Height)
				focusedWindow.Bottom = Screen.PrimaryScreen.Bounds.Height;
			Rectangle r = new Rectangle(focusedWindow.Left, focusedWindow.Top, focusedWindow.Right-focusedWindow.Left, focusedWindow.Bottom-focusedWindow.Top);
			await CaptureShot(r.Location, r.Size, "Window screenshot");
		}
		
		private static bool cancelCapture = false;
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
				f.KeyDown += new System.Windows.Forms.KeyEventHandler(fKeyDown_KeyPress);
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
				await CaptureShot(cutScreenshot.Location, cutScreenshot.Size, "Cut screenshot");
			cancelCapture = false;
		}
		
		private async Task CaptureShot(Point location, Size size, string typeOfShot = "Screenshot") {
			try {
				using (Bitmap screenShot = new Bitmap(size.Width, size.Height)) {
					using (Graphics g = Graphics.FromImage(screenShot)) {
						g.CopyFromScreen(location, new Point(0, 0), size);
					}
					string url = "";
				
					// copy Image to clipboard
					Clipboard.SetImage((Image)screenShot);
				
					if (checkBoxUpload.Checked) {
						if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
							screenShot.Dispose();
							return;
						}
						//since uploading takes some time let's just keep Screenshot in the clipboard until we upload
						ShowBalloonTip("Uploading to imgur.com", String.Format("{0} copied to clipboard", typeOfShot));
						url = await GetUploadedShotURL(screenShot);
						if (url == null) {
							screenShot.Dispose();
							return;
						}
						InsertShotURLtoListBox(url);
					}
					screenShot.Dispose();
					if (radioButtonClipboardURL.Checked && checkBoxUpload.Checked) {
						Clipboard.SetText(url);
						ShowBalloonTip(String.Format("{0} uploaded", typeOfShot), String.Format("{0} copied to clipboard", url));
					} else if (checkBoxUpload.Checked) {
						ShowBalloonTip(String.Format("{0} uploaded", typeOfShot), "Screenshot copied to clipboard");
					} else {
						ShowBalloonTip(String.Format("{0} captured", typeOfShot), "Screenshot copied to clipboard");
					}
				}
			} catch (Exception e) {
				MessageBox.Show(e.Message);
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
						image.Save(ms, ImageFormat.Png);
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
			} catch (Exception e) {
				MessageBox.Show("Failed to upload to imgur.com\n" + e.Message);
				return null;
			}
			});
		}

		private void InsertShotURLtoListBox(string url) {
			listBoxShotURLs.Items.Insert(0, url);
		}
		
		private void OpenURLinBrowser() {
			if (listBoxShotURLs.SelectedIndex < 0)
				return;
			string url = listBoxShotURLs.Items[listBoxShotURLs.SelectedIndex].ToString();
			System.Diagnostics.Process.Start(url);
		}
		
		private void CopyURL() {
			if (listBoxShotURLs.SelectedIndex < 0)
				return;
			string url = listBoxShotURLs.Items[listBoxShotURLs.SelectedIndex].ToString();
			Clipboard.SetText(url);
			ShowBalloonTip("URL copied", String.Format("{0} copied to clipboard", url));
		}
	}
}