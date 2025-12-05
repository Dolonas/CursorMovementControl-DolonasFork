using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WorkEmulation.Infrastructure.Commands;
using WorkEmulation.ViewModels.Base;



namespace WorkEmulation.ViewModels;

internal class MainWindowViewModel : ViewModel, INotifyPropertyChanged
{
    public int X1
    {
        get;
        set;
    }
    public int X2
    {
        get;
        set;
    }
    public int Y1 { get; set; }
    public int Y2 { get; set; }
    internal Random rndX, rndY;

    //общие переменные, используемые в методах: move_cursor_directly, move_cursor_imitate_behaviour
    internal int x, y, beginCorn, eighth;
    internal double cosA, sinA;
	public new event PropertyChangedEventHandler? PropertyChanged;
	
	#region Commands
	
	#region MoveCursorImitateBehaviourCommand
	public ICommand MoveCursorImitateBehaviourCommand { get; }

	private bool CanMoveCursorImitateBehaviourExecute(object p)
	{
		return true;
	}

	private  void OnMoveCursorImitateBehaviourExecuted(object p)
	{
        void move_cursor_imitate_behaviour(object sender, RoutedEventArgs e)
        {
            if (!pre_quarter_calcucation())
                return;
            else
            {
                eighth = determine_eighth();

                switch (eighth)
                {
                    case 1: case 4: case 5: case 8:
                        beginCorn = GetUpCosDirection(cosA);
                        break;
                    case 2: case 3: case 6: case 7:
                        beginCorn = GetDownCosDirection(cosA);
                        break;
                }
            }

            //передвигаем курсор, теперь мы знаем на какой угол нужно его развернуть
            switch (eighth)
            {
                case 1: case 8:
                    while (X1 < X2)
                    {
                        X1 += 5;
                        if (eighth == 1)
                            Y1 -= rndY.Next(beginCorn, beginCorn + 3);
                        else
                            Y1 += rndY.Next(beginCorn, beginCorn + 3);
                        SetCursorPos(X1, Y1);
                        Thread.Sleep(10);
                    }
                    break;
                case 4: case 5:
                    while (X1 > X2)
                    {
                        X1 -= 5;
                        if (eighth == 4)
                            Y1 -= rndY.Next(beginCorn, beginCorn + 3);
                        else
                            Y1 += rndY.Next(beginCorn, beginCorn + 3);
                        SetCursorPos(X1, Y1);
                        Thread.Sleep(10);
                    }
                    break;
                case 2: case 3:
                    while (Y1 > Y2)
                    {
                        Y1 -= 5;
                        if (eighth == 2)
                            X1 += rndX.Next(beginCorn, beginCorn + 3);
                        else
                            X1 -= rndX.Next(beginCorn, beginCorn + 3);
                        SetCursorPos(X1, Y1);
                        Thread.Sleep(10);
                    }
                    break;
                case 6: case 7:
                    while (Y1 < Y2)
                    {
                        Y1 += 5;
                        if (eighth == 7)
                            X1 += rndX.Next(beginCorn, beginCorn + 3);
                        else
                            X1 -= rndX.Next(beginCorn, beginCorn + 3);
                        SetCursorPos(X1, Y1);
                        Thread.Sleep(10);
                    }
                    break;
            }

            //логика прямоленейного довода курсора, чтобы избавиться от неточности при перемещении крусора на большое расстояние
            switch (eighth)
            {
                case 1: case 4: case 5: case 8:
                    while (Y2 > Y1)
                    {
                        Y1 += 1;
                        SetCursorPos(rndX.Next(X2 - 2, X2 + 2), Y1);
                        Thread.Sleep(4);
                    }
                    while (Y2 < Y1)
                    {
                        Y1 -= 1;
                        SetCursorPos(rndX.Next(X2 - 2, X2 + 2), Y1);
                        Thread.Sleep(4);
                    }
                    SetCursorPos(X2, Y1);
                    break;

                case 2: case 3: case 6: case 7:
                    while (X2 > X1)
                    {
                        X1 += 1;
                        SetCursorPos(X1, rndY.Next(Y2 - 2, Y2 + 2));
                        Thread.Sleep(4);
                    }
                    while (X2 < X1)
                    {
                        X1 -= 1;
                        SetCursorPos(X1, rndY.Next(Y2 - 2, Y2 + 2));
                        Thread.Sleep(4);
                    }
                    SetCursorPos(X1, Y2);
                    break;
            }
        }

        
	}


	#endregion
	
	#endregion

	public MainWindowViewModel()
	{
			
		
		#region Commands

		MoveCursorImitateBehaviourCommand =
			new LambdaCommand(OnMoveCursorImitateBehaviourExecuted, CanMoveCursorImitateBehaviourExecute);
		

		#endregion
	}
    
    private int GetUpCosDirection(double cosAA) //возвращает начальное число для отсчета параметра
    {
        return 0;
    }

    private int GetDownCosDirection(double cosAA)
    {
        return 0;
    }

    private bool pre_quarter_calcucation()
    {
        GetCursorPositions();

        return true;
    }

    private int determine_eighth()
    {
        return 2;
    }
        
    private void GetCursorPositions()
    {
        // x1 = Int32.Parse(x1_text.Text);
        // y1 = Int32.Parse(y1_text.Text);
        //
        // x2 = Int32.Parse(x2_text.Text);
        // y2 = Int32.Parse(y2_text.Text);

        rndX = new Random();
        rndY = new Random();
    }
    
    
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out Point p);
    
}