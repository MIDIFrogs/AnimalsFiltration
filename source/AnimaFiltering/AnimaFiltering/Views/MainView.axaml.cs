using System;
using System.Diagnostics;
using System.Linq;
using AnimaFiltering.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
namespace AnimaFiltering.Views;

public partial class MainView : UserControl
{

    public MainView()
    {
        InitializeComponent();
        //AddHandler(DragDrop.DragOverEvent, DragOver);
        AddHandler(DragDrop.DropEvent, Drop);
    }

    private void Drop(object? sender, DragEventArgs e)
    {
        var files = e.Data.GetFiles();
        if (files!= null)
        {
            foreach (var file in files)
            {
                Debugger.Break();
            }
        }
    }


}
