using System;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SmartPrintScreen {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			string progGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
			try {
				using (Mutex m = new Mutex(false, "Global\\" + progGuid)) {
					if (!m.WaitOne(0, false)) {
						MessageBox.Show(FormMain.programName + " already running");
						return;
					}
					GC.Collect();
					Application.EnableVisualStyles();
					Application.SetCompatibleTextRenderingDefault(false);
					Application.Run(new FormMain());
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}
}
