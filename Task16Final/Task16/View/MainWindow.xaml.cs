﻿using System.Windows;
using Task16.Model;
using Task16.Other;
using EntityToWindow.ViewModels;
using EntityToWindow.WPF;


namespace Task16.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Initialization.Instance.Init();
            new MainProjectWindow(typeof(SqliteContext), this).Show();
        }
    }
}
