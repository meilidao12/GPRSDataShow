using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfServers.Notification;
namespace GPRSDataShow.Models
{
    public class HuiZhongModel : NotificationObject
    {
        private string id;
        /// <summary>
        /// 仪表ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; RaisePropertyChanged("ID"); }
        }


        private string collectTime;
        /// <summary>
        /// 采集时间
        /// </summary>
        public string CollectTime
        {
            get { return collectTime; }
            set { collectTime = value; RaisePropertyChanged("CollectTime"); }
        }


        private string instrumentNumber;
        /// <summary>
        /// 仪表编号
        /// </summary>
        public string InstrumentNumber
        {
            get { return instrumentNumber; }
            set { instrumentNumber = value; RaisePropertyChanged("InstrumentNumber"); }
        }


        private string userName;
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged("UserName"); }
        }

        private string organizition;
        public string Organizition
        {
            get{return organizition;}
            set{organizition = value;}
        }

        private string sim;
        /// <summary>
        /// SIM卡号
        /// </summary>
        public string SIM
        {
            get { return sim; }
            set { sim = value; RaisePropertyChanged("SIM"); }
        }


        private string flowRate;
        /// <summary>
        /// 瞬时流量(m3/h)
        /// </summary>
        public string FlowRate
        {
            get { return flowRate; }
            set { flowRate = value; RaisePropertyChanged("FlowRate"); }
        }


        private string accumulateFlow;
        /// <summary>
        /// 累积流量(m3)
        /// </summary>
        public string AccumulateFlow
        {
            get { return accumulateFlow; }
            set { accumulateFlow = value; RaisePropertyChanged("AccumulateFlow"); }
        }


        private string positiveAccumulateFlow;
        /// <summary>
        /// 正累积流量(m3)
        /// </summary>
        public string PositiveAccumulateFlow
        {
            get { return positiveAccumulateFlow; }
            set { positiveAccumulateFlow = value; RaisePropertyChanged("PositiveAccumulateFlow"); }
        }
    }
}
