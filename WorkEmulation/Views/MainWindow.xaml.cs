using System.Windows;
using System.Windows.Threading;
using WorkEmulation.ViewModels;

namespace WorkEmulation.Views;

public partial class MainWindow
{
	public MainWindow()
	{
		InitializeComponent();
		
		DataContext = new MainWindowViewModel();
		Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(Exception1);
	}

	private static void Exception1(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		MessageBox.Show(e.Exception.ToString());
	}
}