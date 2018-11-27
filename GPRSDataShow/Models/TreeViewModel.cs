using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfServers.Notification;
namespace GPRSDataShow.Models
{
    public class TreeViewModel : NotificationObject
    {
        private string id;
        public string Id
        {
            get{ return id; }
            set{ id = value ; RaisePropertyChanged("Id"); }
        }

        private string icon;
        public string Icon
        {
            get { return icon; }
            set { icon = value;RaisePropertyChanged("Icon"); }
        }

        public string displayname;
        public string DisplayName
        {
            get{ return displayname; }
            set { displayname = value;RaisePropertyChanged(displayname); }
        }

        public List<TreeViewModel> children;
        public List<TreeViewModel> Children
        {
            get { return children; }
            set { children = value ; RaisePropertyChanged("Children"); }
        }

        public TreeViewModel()
        {
            Children = new List<TreeViewModel>();
        }
    }
}
