using System;
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
using CommunicationServers.Pipe;
using GPRSDataShow.Models;
using ProtocolFamily.YanGang;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Services;
using System.Text.RegularExpressions;
using Services.DataBase;
using System.Data;

namespace GPRSDataShow.Views
{
    /// <summary>
    /// YGMainView.xaml 的交互逻辑
    /// </summary>
    public partial class YGMainView : Page
    {

        #region ---变量
        AccessHelper accessHelper;
        PipeServerHelper ps = new PipeServerHelper("tianchen");
        public List<TreeViewModel> ListTreeViewModels = new List<TreeViewModel>();
        public ObservableCollection<HuiZhongModel> HuiZhongModels = new ObservableCollection<HuiZhongModel>();
        public ObservableCollection<HuiZhongModel> HuiZhongModelsBranch = new ObservableCollection<HuiZhongModel>();
        public PagingModel<HuiZhongModel> PagingModel;
        private string TreeViewSelectedDisplayName;
        #endregion

        public YGMainView()
        {
            InitializeComponent();
            this.Loaded += YGMainView_Loaded;
        }

        private void YGMainView_Loaded(object sender, RoutedEventArgs e)
        {
            //---打开通道
            ps.NewMessageEvent += Ps_NewMessageEvent;
            ps.Run();
            //---显示树形结构
            ShowTreeView();
            IniHuiZhongModels();
            //记录
            PagingModel = new PagingModel<HuiZhongModel>(HuiZhongModels, 20);
            PagingModel.GetPageData(JumpOperation.GoHome);
            //绑定
            this.txtCurrentPage.SetBinding(TextBlock.TextProperty, new Binding("CurrentIndex") { Source = PagingModel,Mode= BindingMode.TwoWay});
            this.txtTotalPage.SetBinding(TextBlock.TextProperty, new Binding("PageCount") { Source = PagingModel, Mode = BindingMode.TwoWay });
            this.txtTargetPage.SetBinding(TextBox.TextProperty, new Binding("JumpIndex") { Source = PagingModel, Mode = BindingMode.TwoWay });
            this.Record.SetBinding(DataGrid.ItemsSourceProperty, new Binding("ShowDataSource") { Source = PagingModel, Mode = BindingMode.TwoWay });
        }

        private void ListToObservable(List<HuiZhongModel> lst , ObservableCollection<HuiZhongModel> obc)
        {
            obc.Clear();
            lst.ForEach(p => obc.Add(p));
        }

        private void IniHuiZhongModels()
        {
            accessHelper = new AccessHelper();
            DataTable dataTable =   accessHelper.GetDataTable("select * from ConnectionSet1");
            if (dataTable == null) return;
            List<HuiZhongModel> lst = dataTable.AsEnumerable().Select(
                m => new HuiZhongModel
                {
                    ID = m.Field<string>("序号"),
                    InstrumentNumber = m.Field<string>("仪表编号"),
                    UserName = m.Field<string>("用户名称"),
                    Organizition = m.Field<string>("组织机构"),
                    SIM = m.Field<string>("SIM卡号"),
                }
            ).ToList();
            lst.ForEach(p => HuiZhongModels.Add(p));
        }

        private void Ps_NewMessageEvent(string Message)
        {
            AnalysisDataHelper analysisDataHelper = new AnalysisDataHelper();
            var model = analysisDataHelper.AnalysisData(Message);
            TimeFormatHelper timeHelper = new TimeFormatHelper();
            Dispatcher.Invoke(new Action(delegate
            {
                var model2 = HuiZhongModels.Where(m => m.SIM == model.Data0).FirstOrDefault();
                int index = HuiZhongModels.IndexOf(model2);
                HuiZhongModels[index].CollectTime = timeHelper.HexTimeToDecTime(model.Data3);
                HuiZhongModels[index].FlowRate = model.Data1;
                HuiZhongModels[index].AccumulateFlow = model.Data2;
            }));
            //--- 分页
            ShowData(TreeViewSelectedDisplayName);
        }

        private void ShowTreeView()
        {
            TreeViewModel parent = new TreeViewModel();
            parent.Id = "1";
            parent.DisplayName = "一级显示";
            parent.Children.Add(new TreeViewModel { DisplayName = "动力一厂", Id = "1" });
            parent.Children.Add(new TreeViewModel { DisplayName = "北区制氧" });
            parent.Children.Add(new TreeViewModel { DisplayName = "北区球团" });
            parent.Children.Add(new TreeViewModel { DisplayName = "动力二厂" });
            parent.Children.Add(new TreeViewModel { DisplayName = "烧结二厂" });
            parent.Children.Add(new TreeViewModel { DisplayName = "新区球团" });
            parent.Children.Add(new TreeViewModel { DisplayName = "炼钢二厂" });
            parent.Children.Add(new TreeViewModel { DisplayName = "炼铁二厂" });
            parent.Children.Add(new TreeViewModel { DisplayName = "新区制氧" });
            parent.Children.Add(new TreeViewModel { DisplayName = "煤气" });
            parent.Children.Add(new TreeViewModel { DisplayName = "轧钢一厂" });
            parent.Children.Add(new TreeViewModel { DisplayName = "轧钢二厂" });
            parent.Children.Add(new TreeViewModel { DisplayName = "炼钢一厂" });
            parent.Children.Add(new TreeViewModel { DisplayName = "炼铁一厂" });
            parent.Children.Add(new TreeViewModel { DisplayName = "水源地" });
            parent.Children.Add(new TreeViewModel { DisplayName = "白灰厂" });
            parent.Children.Add(new TreeViewModel { DisplayName = "烧结一厂" });
            parent.Children.Add(new TreeViewModel { DisplayName = "热轧三厂" });
            ListTreeViewModels.Add(parent);
            this.AddressList.ItemsSource = ListTreeViewModels;
        }

        private void AddressList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(e.Source != null)
            {
                TreeView tv = (TreeView)e.Source;
                var b = (TreeViewModel)tv.SelectedItem;
                TreeViewSelectedDisplayName = b.DisplayName;
                ShowData(TreeViewSelectedDisplayName);
                PagingModel.GetPageData(JumpOperation.GoHome);
            }
        }

        private void ShowData(string displayName)
        {
            if ("一级显示" == displayName || displayName == null)
            {
                PagingModel.DataSource = HuiZhongModels;
            }
            else
            {
                List<HuiZhongModel> lst = HuiZhongModels.Where(m => m.Organizition == displayName).ToList();
                ListToObservable(lst, HuiZhongModelsBranch);
                PagingModel.DataSource = HuiZhongModelsBranch;  //记录
            }
            PagingModel.Refresh();          
        }

        private void PageOperationClick(object sender, RoutedEventArgs e)
        {
            
            Button btn = (Button)e.Source;
            switch(btn.Name)
            {
                case "btnHomePage":
                    PagingModel.GetPageData(JumpOperation.GoHome);
                    break;
                case "btnPreviousPage":
                    PagingModel.GetPageData(JumpOperation.GoPrevious);
                    break;
                case "btnNextPage":
                    PagingModel.GetPageData(JumpOperation.GoNext);
                    break;
                case "btnEndPage":
                    PagingModel.GetPageData(JumpOperation.GoEnd);
                    break;
                case "btnJmpPage":
                    PagingModel.JumpPageData(Convert.ToInt32(txtTargetPage.Text));
                    break;
            }
        }

        private void txtTargetPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9.-]+");
            e.Handled = re.IsMatch(e.Text);
        }
    }
}
