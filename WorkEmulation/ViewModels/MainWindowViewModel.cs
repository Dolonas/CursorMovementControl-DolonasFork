using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using WorkEmulation.Infrastructure.Commands;
using WorkEmulation.ViewModels.Base;



namespace WorkEmulation.ViewModels;

internal class MainWindowViewModel : ViewModel, INotifyPropertyChanged
{
	public new event PropertyChangedEventHandler? PropertyChanged;

	public MainWindowViewModel()
	{
			
	}
	
}