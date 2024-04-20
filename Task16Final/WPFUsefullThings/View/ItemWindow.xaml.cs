﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFUsefullThings.ViewModels;

namespace WPFUsefullThings.View
{
    /// <summary>
    /// Логика взаимодействия для ItemWindow.xaml
    /// </summary>
    public partial class ItemWindow : Window
    {
        public Type Type { get; set; }

        public ItemWindow(Type type, object context)
        {
            InitializeComponent();
            DataContext = context;
            var stackPanel = WindowConstructor.InitializeItemWindow(type);
            baseGrid.Children.Add(stackPanel);
        }

    }
}
