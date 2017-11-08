﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CookBook_WPF.View
{
    /// <summary>
    /// Логика взаимодействия для ProductCatalogView.xaml
    /// </summary>
    public partial class ProductCatalogView : UserControl
    {
        public ProductCatalogView()
        {
            InitializeComponent();
            Focusable = true;
            Loaded += (s, e) => Keyboard.Focus(this);

        }
    }
}
