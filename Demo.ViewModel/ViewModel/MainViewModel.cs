﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.ViewModel
{
    public class MainViewModel
    {
        public QQViewModel QQViewModel { get; set; }
        public MicroMsgViewModel MicroMsgViewModel { get; set; }
        public Mail163ViewModel Mail163ViewModel { get; set; }
        public Mail189ViewModel Mail189ViewModel { get; set; }
        public ECloudViewModel ECloudViewModel { get; set; }
        public Browser360ViewModel Browser360ViewModel { get; set; }

        private static readonly MainViewModel instance = new MainViewModel();
        public static MainViewModel GetInstance() { return instance; }
        private MainViewModel()
        {
            MicroMsgViewModel = new MicroMsgViewModel();
            QQViewModel = new QQViewModel();
            Mail163ViewModel = new Mail163ViewModel();
            Mail189ViewModel = new Mail189ViewModel();
            ECloudViewModel = new ECloudViewModel();
            Browser360ViewModel = new Browser360ViewModel();
        }

        public static void InitializeWithParams(string type,string path)
        {
            switch (type)
            {
                case "com.tencent.mobileqq":                    
                    instance.QQViewModel.FilePath = path;
                    instance.QQViewModel.ExcuteAnalyzeFile();
                    instance.startType = type;
                    break;
                case "com.tencent.mm":
                    instance.MicroMsgViewModel.FilePath = path;
                    instance.MicroMsgViewModel.ExcuteAnalyzeFile();
                    instance.startType = type;
                    break;
                case "com.netease.mobimail":
                    instance.Mail163ViewModel.FilePath = path;
                    instance.Mail163ViewModel.ExcuteAnalyzeFile();
                    instance.startType = type;
                    break;
                case "com.corp21cn.mail189":
                    instance.Mail189ViewModel.FilePath = path;
                    instance.Mail189ViewModel.ExcuteAnalyzeFile();
                    instance.startType = type;
                    break;
                case "com.cn21.ecloud":
                    instance.ECloudViewModel.FilePath = path;
                    instance.ECloudViewModel.ExcuteAnalyzeFile();
                    instance.startType = type;
                    break;
                case "com.qihoo.browser":
                    instance.Browser360ViewModel.FilePath = path;
                    instance.Browser360ViewModel.ExcuteAnalyzeFile();
                    instance.startType = type;
                    break; 
                default:
                    break;
            }
        }

        public string startType=null;
    }
}
