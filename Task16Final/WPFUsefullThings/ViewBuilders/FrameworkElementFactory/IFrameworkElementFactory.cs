﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFUsefullThings
{
    public interface IFrameworkElementFactory
    {
        FrameworkElement Create(PropertyInfo property);
        public virtual StackPanel CreatePanel(PropertyInfo property)
        {
            return CreatePanelWithOrientation(property, Orientation.Horizontal);
        }

        public static StackPanel CreatePanelWithOrientation(PropertyInfo property, Orientation orientation)
        {
            var textBlock = ItemWindowConstructor.CreateTextBlock(property);
            var element = FrameworkElementFactory.CreateElement(property);

            var horizontalPanel = new StackPanel { Orientation = orientation };
            horizontalPanel.Children.Add(textBlock);
            horizontalPanel.Children.Add(element);

            return horizontalPanel;
        }
        bool CanHandle(PropertyInfo property);
        int Priority { get; }
    }
}
