﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ViewModel.Converters
{
    public class BooleanReversedToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value == null || ((bool)value) == false) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}