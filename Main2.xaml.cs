﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using WechatBakTool.Model;

namespace WechatBakTool
{
    /// <summary>
    /// Main2.xaml 的交互逻辑
    /// </summary>
    public partial class Main2 : Window
    {
        public static UserBakConfig? CurrentUserBakConfig;
        private ObservableCollection<UserBakConfig> userBakConfigs = new ObservableCollection<UserBakConfig>();
        public Main2()
        {
            InitializeComponent();
            lab_version.Content += $" {Application.ResourceAssembly.GetName().Version}";
            // list_workspace.Items.Add(new { Name = "sxcoder", Friends_Number=23, Msg_Number=102302, Decrypt="已解密" });
            LoadWorkspace();
        }

        private void img_btn_close_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void LoadWorkspace()
        {
            userBakConfigs.Clear();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "workspace");
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    string type = file.Substring(file.Length - 5, 5);
                    if (type == ".json")
                    {
                        string jsonString = File.ReadAllText(file);
                        UserBakConfig? userBakConfig = null;
                        try
                        {
                            userBakConfig = JsonConvert.DeserializeObject<UserBakConfig>(jsonString);
                        }
                        catch
                        {
                            MessageBox.Show("读取到异常工作区文件，请确认备份数据是否正常\r\n文件路径：" + file, "错误");
                        }
                        if (userBakConfig != null)
                        {
                            userBakConfigs.Add(userBakConfig);
                        }
                    }
                }
            }
            list_workspace.ItemsSource = userBakConfigs;
        }

        private void list_workspace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserBakConfig? config = list_workspace.SelectedItem as UserBakConfig;
            if(config == null)
            {
                MainFrame.Navigate(new Uri("pack://application:,,,/Pages/Welcome.xaml?datatime=" + DateTime.Now.Ticks));
                return;
            }
            if (!config.Decrypt)
            {
                MessageBox.Show("请先到创建工作区进行解密");
                MainFrame.Navigate(new Uri("pack://application:,,,/Pages/CreateWork.xaml?datatime=" + DateTime.Now.Ticks));
                return;
            }

            CurrentUserBakConfig = config;
            MainFrame.Navigate(new Uri("pack://application:,,,/Pages/Workspace.xaml?datatime=" + DateTime.Now.Ticks));
        }

        private void new_workspace_fill_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new Uri("pack://application:,,,/Pages/CreateWork.xaml?datatime=" + DateTime.Now.Ticks));
        }
    }
}