﻿using Microsoft.SqlServer.Management.HadrData;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace WPFUsefullThings
{
    /// <summary>
    /// Логика взаимодействия для ItemWindow.xaml
    /// </summary>
    public partial class BaseItemWindow : Window
    {
        public Type Type { get; set; }

        public BaseItemWindow(Type type, IItemViewModel context)
        {
            InitializeComponent();
            
        }

        public void SetContent(UIElement content)
        {
            ContentStackPanel.Children.Add(content);
        }
    }
}