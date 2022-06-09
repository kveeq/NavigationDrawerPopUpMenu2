using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using NavigationDrawerPopUpMenu2.db;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NavigationDrawerPopUpMenu2
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static HomeRepairEntities db = new HomeRepairEntities();
        public static Auth user;
        public static Users user1;
    }
}
