﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Mvvm;
using Prism.Commands;

using Demo.Library.MicroMsg;
using AndroidMemoryAnalyzer.HeapAnalyzer;

namespace Demo.Library.ViewModel
{

    public class MicroMsgViewModel : BindableBase
    {
        string account = "无";
        public string Account
        {
            get { return account; }
            set
            {
                if (account != value)
                {
                    account = value;
                    RaisePropertyChanged("Account");
                }
            }
        }

        List<MicroMsgFriend> friends;
        public List<MicroMsgFriend> Friends
        {
            get { return friends; }
            set
            {
                if (friends != value)
                {
                    friends = value;
                    RaisePropertyChanged("Friends");
                }
            }
        }

        List<MicroMsgMessage> msgs;
        public List<MicroMsgMessage> Msgs
        {
            get { return msgs; }
            set
            {
                if (msgs != value)
                {
                    msgs = value;
                    RaisePropertyChanged("Msgs");
                }
            }
        }
        string state="准备就绪";
        public string State {
            get{return state;}
            set
            {
                if (state != value)
                {
                    state = value;
                    RaisePropertyChanged("State");
                }
            } 
        }
        public string FilePath { get; set; }
        public DelegateCommand AnalyzeFile
        {
            get
            {
                return analyzeFile ?? (analyzeFile = new DelegateCommand(ExcuteAnalyzeFile));
            }
        }

        DelegateCommand analyzeFile;
        public void ExcuteAnalyzeFile()
        {
            if (FilePath == "")
            {
                State = "请输入文件路径";
                return;
            }

            State = "解析中...";
            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        HeapFileAnalyzer x = new HeapFileAnalyzer(FilePath);

                        x.DoWork();

                        Friends = MicroMsgHelper.GetFriends(x);
                        Msgs = MicroMsgHelper.GetMessages(x);
                        Account = MicroMsgHelper.GetAccount(x);
                        State = "解析完成！";
                    }
                    catch
                    {
                        State = "解析出现异常";
                    }
                    
                });

        }
    }
}
