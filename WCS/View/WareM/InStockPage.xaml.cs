﻿using System;
using System.Collections.Generic;
using System.Data;
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
using WCS.ViewBase.WareM;
using WCS.ViewModel.WareM;
using WCS_Bll;
using WCS_Model.DB;

namespace WCS.View.WareM
{
    /// <summary>
    /// InStockPage.xaml 的交互逻辑
    /// </summary>
    public partial class InStockPage : Page
    {
        WMS_PutIn_Model wms_PutIn_Model = new WMS_PutIn_Model();
        InStockBase stockBase = new InStockBase();
        int editFlag = 0;//可编辑标签
        string snName = string.Empty;
        List<string> selectSnName = new List<string>();
        public InStockPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DMButton_Add_Click(object sender, RoutedEventArgs e)
        {
            editFlag = 1;
            PutInDataGrid.IsReadOnly = false;
            PutInDataGrid.CanUserAddRows = true;
        }

        /// <summary>
        /// 编辑按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DMButton_Edit_Click(object sender, RoutedEventArgs e)
        {
            editFlag = 2;
            PutInDataGrid.IsReadOnly = false;
            PutInDataGrid.CanUserAddRows = false;
        }

        /// <summary>
        /// 删除按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DMButton_Delete_Click(object sender, RoutedEventArgs e)
        {
            foreach (var no in selectSnName)
            {
                WMS_PutIn_Bll.Delete_PutIn(" SN = '" + no + "'");
            }
            Page_Frush();
        }

        /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DMButton_Save_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == 1)
            {
                WMS_PutIn_Bll.Insert_PutIn(wms_PutIn_Model);
            }
            else if (editFlag == 2)
            {
                WMS_PutIn_Bll.Update_PutIn(wms_PutIn_Model, " SN = '" + snName + "'");
            }
            editFlag = 0;
            Page_Frush();
            PutInDataGrid.CanUserAddRows = false;
            PutInDataGrid.IsReadOnly = true;
        }

        /// <summary>
        /// 搜索按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DMButton_Search_Click(object sender, RoutedEventArgs e)
        {
            stockBase.PutInList.Clear();
            List<WMS_PutIn_Model> list = new List<WMS_PutIn_Model>();
            DataTable dt = WMS_PutIn_Bll.Select_PutIn(" SN = '" + SearchText.Text.Trim() + "'");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                WMS_PutIn_Model wMS_PutIn_Model = new WMS_PutIn_Model
                {
                    ShelfNo = dt.Rows[i]["ShelfNo"].ToString(),
                    PutInNo = dt.Rows[i]["PutInNo"].ToString(),
                    SN = dt.Rows[i]["SN"].ToString(),
                    OrderNo = dt.Rows[i]["OrderNo"].ToString(),
                    PutInType = dt.Rows[i]["PutInType"].ToString(),
                    Status = dt.Rows[i]["Status"].ToString(),
                    PutInTime = dt.Rows[i]["PutInTime"].ToString()
                };
                list.Add(wMS_PutIn_Model);
            }
            foreach (var model in list)
            {
                stockBase.PutInList.Add(model);
            }
            this.PutInDataGrid.ItemsSource = stockBase.PutInList;
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StockDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            wms_PutIn_Model = e.Row.Item as WMS_PutIn_Model;
            snName = wms_PutIn_Model.SN;
        }

        /// <summary>
        /// 选中框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox dg = sender as CheckBox;
            string sn = dg.Tag.ToString();
            var bl = dg.IsChecked;
            if (bl == true)
            {
                selectSnName.Add(sn);
            }
            else
            {
                selectSnName.Remove(sn);
            }
        }

        /// <summary>
        /// 实时界面数据刷新
        /// </summary>
        private void Page_Frush()
        {
            stockBase.PutInList.Clear();
            foreach (var model in InStockViewModel.GetPutInList())
            {
                stockBase.PutInList.Add(model);
            }
            this.PutInDataGrid.ItemsSource = stockBase.PutInList;
        }
    }
}
