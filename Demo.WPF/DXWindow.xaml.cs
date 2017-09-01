﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;

using Demo.Library.ViewModel;
namespace Demo.WPF
{
    /// <summary>
    /// Interaction logic for DXWindow1.xaml
    /// </summary>
    public partial class DXWindow1 : DXWindow
    {
        public DXWindow1()
        {
            InitializeComponent();

            this.DataContext = MainViewModel.GetInstance();
            var type=MainViewModel.GetInstance().startType;
            switch(type)
            {
                case "com.tencent.mobileqq":  
                    tabControl.SelectTabItem(0);
                    break;
                case "com.tencent.mm":
                    tabControl.SelectTabItem(2);
                    break;
                default:
                    break;
            }
        }
    }
}