using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NavigationDrawerPopUpMenu2.db;
using Microsoft.Win32;
using System.IO;
using System.Drawing;

namespace NavigationDrawerPopUpMenu2
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    /// 
    public partial class MainWindow : Window
    { 
        private bool isDesign = false;
        private bool isEng = false;
        private bool isRepair = false;
        private bool islstView = false;
        private List<Zayavki> cart = new List<Zayavki>();

        public MainWindow()
        {
            InitializeComponent();

        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VisibleHidden();
            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "Home":
                    User_main.Visibility = Visibility.Visible;
                    break;
                case "ItemHome":
                    DataContext = App.user;
                    Profile.Visibility = Visibility.Visible;
                    break; 
                case "ItemCreate":
                    User_cart.Visibility = Visibility.Visible;
                    CartsLstView.ItemsSource = App.db.Zayavki.ToList().Where(x => x.isCart == true && x.IdUser == App.user.Users.Id);
                    break;
                case "zayavki":
                    if (App.user.IdRole == 2)
                    {
                        Brigadir_zayavki.Visibility = Visibility.Visible;
                        Zayavki_for_brigadirs_LstView.ItemsSource = App.db.Zayavki.ToList().Where(x => x.isCart == false && x.isAccept == true);
                    }
                    else if (App.user.IdRole == 1)
                    {
                        User_zayavki.Visibility = Visibility.Visible; 
                        ZayavkiLstView.ItemsSource = App.db.Zayavki.ToList().Where(x => x.isCart == false && x.IdUser == App.user.Users.Id).Select(x => x.Service);
                    }
                    break;
                case "clients":
                    Admin_Clients.Visibility = Visibility.Visible;
                    ClientsLstView.ItemsSource = App.db.Users.ToList().Where(x => x.Auth.IdRole == 1);
                    break;
                case "Brigadirs":
                    Admin_Brigadirs.Visibility = Visibility.Visible;
                    BrigadirsLstView.ItemsSource = App.db.Users.ToList().Where(x => x.Auth.IdRole == 2);
                    break;
                case "zayvkiForBrigadir":
                    Brigadir_accepted_zayavki.Visibility = Visibility.Visible;
                    Brigadir_accepted_zayavki_LstView.ItemsSource = App.db.Kontrakt.Select(x => x.Zayavki).ToList();
                    break;
                default:
                    break;
            }
        }

        private void VisibleHidden()
        {
            // профиль не трогать 
            // все остальное сделать невидимым
            Profile.Visibility = Visibility.Hidden;
            User_main.Visibility = Visibility.Hidden;
            Services.Visibility = Visibility.Hidden;
            About_Uslugi.Visibility = Visibility.Hidden;
            User_cart.Visibility = Visibility.Hidden;
            User_zayavki.Visibility = Visibility.Hidden;
            Brigadir_zayavki.Visibility = Visibility.Hidden;
            Brigadir_accepted_zayavki.Visibility = Visibility.Hidden;
            Admin_Brigadirs.Visibility = Visibility.Hidden;
            Admin_Clients.Visibility = Visibility.Hidden;
            Services.Visibility = Visibility.Hidden;
            Description_uslugi.Visibility = Visibility.Hidden;
            About_Uslugi.Visibility = Visibility.Hidden;
            islstView = false;
        }

        private void Delete_accaunt_Click(object sender, RoutedEventArgs e)
        {
            var user = App.db.Users.Where(x => x.Id == App.user.IdUser).FirstOrDefault();
            App.db.Users.Remove(user);
            App.db.SaveChanges();
            Exit_account_Click(null,null);
        }

        private void Redact_account_Click(object sender, RoutedEventArgs e)
        {
            var user = App.db.Users.Where(x => x.Id == App.user.IdUser).FirstOrDefault();
            user = App.user.Users;
            App.db.SaveChanges();
        }

        private void Exit_account_Click(object sender, RoutedEventArgs e)
        {
            App.user = null;
            Home.Visibility = Visibility.Collapsed;
            ItemHome.Visibility = Visibility.Collapsed;
            ItemCreate.Visibility = Visibility.Collapsed;
            zayavki.Visibility = Visibility.Collapsed;
            Brigadirs.Visibility = Visibility.Collapsed;
            clients.Visibility = Visibility.Collapsed;
            zayvkiForBrigadir.Visibility = Visibility.Collapsed;
            Account_information.Visibility = Visibility.Hidden;
            Account_input.Visibility = Visibility.Visible;
            Clear_account_registrate_Click(null, null);
        }

        private void Input_user_account_Click(object sender, RoutedEventArgs e)
        {
            //вход по базе
            App.user = App.db.Auth.ToList().Where(x => x.Users.PhoneNumber == Account_phone_input.Text && x.Password == Account_password_input.Password).FirstOrDefault();

            if(App.user != null)
            {
                Account_input.Visibility = Visibility.Hidden;
                Account_registrate.Visibility = Visibility.Hidden;
                ListViewMenu.Visibility = Visibility.Visible;
                Account_information.Visibility = Visibility.Visible;
                Account_password_information.Password = App.user.Password;
                DataContext = App.user;
                using (Bitmap bmp = new Bitmap(new MemoryStream(App.user.Users.Photo)))
                    img_profile.Source = bmp.BitmapToImageSource();
                if (App.user.IdRole == 3)
                    GetAdmin();
                else if (App.user.IdRole == 2)
                    GetBrigadir();
                else
                    GetUser();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль!");
            }
        }

        private void Registrate_new_User_Click(object sender, RoutedEventArgs e)
        {
            // переход на страницу регистрации
            Account_input.Visibility = Visibility.Hidden;
            Account_registrate.Visibility = Visibility.Visible;
        }

        private void Registrate_accaunt_button_Click(object sender, RoutedEventArgs e)
        {
            //регистрация в базе
            try
            {
                if (Account_password_registrate.Password != Account_double_password_registrate.Password)
                    throw new Exception("Пароль не совпадает");
                Auth auth = new Auth();
                Users user = new Users();
                user.Name = Account_name_registrate.Text;
                user.Surname = Account_surname_registrate.Text;
                user.Lastname = Account_lastname_registrate.Text;
                user.Adres = Account_adress_registrate.Text;
                user.PhoneNumber = Account_phone_num_registrate.Text;
                if (App.db.Users.Where(x => x.PhoneNumber == Account_phone_num_registrate.Text).FirstOrDefault() != null)
                    throw new Exception("Такой клиент уже существует");
                App.db.Users.Add(user);
                App.db.SaveChanges();
                auth.Password = Account_password_registrate.Password;
                auth.IdRole = 1;
                auth.IdUser = user.Id;
                App.db.Auth.Add(auth);
                App.db.SaveChanges();
                MessageBox.Show("Вы успешно зарегистрировались");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Регистрация не успешная!" + $"\n{ex.Message}");
            }
        }

        private void Clear_account_registrate_Click(object sender, RoutedEventArgs e)
        {
            //очистка полей регистрации
            Account_surname_registrate.Clear();
            Account_name_registrate.Clear();
            Account_lastname_registrate.Clear();
            Account_adress_registrate.Clear();
            Account_phone_num_registrate.Clear();
            Account_password_registrate.Clear();
            Account_phone_input.Clear();
            Account_password_input.Clear();
            Account_double_password_registrate.Clear();
        }

        private void question_about_account_Click(object sender, RoutedEventArgs e)
        {
            // переход в войти
            Account_registrate.Visibility = Visibility.Hidden;
            Account_input.Visibility = Visibility.Visible;
        }

        private void img_change_MouseMove(object sender, MouseEventArgs e)
        {
            //var grid = (Grid)sender;
            //if (grid.Name == "img_profile_change")
                img_profile_change.Opacity = 0.5;
            //if (grid.Name == "img_uslugi_change")
            //    img_uslugi_change.Opacity = 0.5;
        }

        private void img_change_MouseLeave(object sender, MouseEventArgs e)
        {
            img_profile_change.Opacity = 0;
        }

        // сделать добавление в корзину с помощью кнопки в списке услуг и сделать заказ через корзину

        private void prinyat_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;

            var result = MessageBox.Show("Подтвердите свое действие", "Подтверждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) 
            {
                btn.IsEnabled = false;
                var zayavka = App.db.Zayavki.ToList().Where(x => x.Id == int.Parse(btn.CommandParameter.ToString())).FirstOrDefault();
                zayavka.isAccept = false;
                Kontrakt kontrakt = new Kontrakt();
                kontrakt.Zayavki = zayavka;
                kontrakt.IdBrigadir = App.user.Users.Id;
                kontrakt.Date = DateTime.Now;
                App.db.Kontrakt.Add(kontrakt);
                App.db.SaveChanges();

            }
        }
        private void Ingeneering_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var txt = ((TextBlock)sender).Name;
            Services.Visibility = Visibility.Visible;
            About_Uslugi.Visibility = Visibility.Hidden;
            switch(txt)
            {
                case "Ingeneering":
                    IngeeniringChoose();
                    break;
                case "Designq":
                    DesignChoose();
                    break;
                case "Repair":
                    RepairChoose();
                    break;
            }
            islstView = true;
        }
        int category = 0;

        private void IngeeniringChoose()
        {
            ServicesLstView.ItemsSource = App.db.Service.ToList().Where(x => x.IdCategory == 1);
            category = 1;
        }
        
        private void DesignChoose()
        {
            ServicesLstView.ItemsSource = App.db.Service.ToList().Where(x => x.IdCategory == 2);
            category = 2;
        }

        private void RepairChoose()
        {
            ServicesLstView.ItemsSource = App.db.Service.ToList().Where(x => x.IdCategory == 3);
            category = 3;
        }

        private void GetAdmin()
        {
            Home.Visibility = Visibility.Visible;
            ItemHome.Visibility = Visibility.Visible;
            ItemCreate.Visibility = Visibility.Collapsed;
            zayavki.Visibility = Visibility.Collapsed;
            Brigadirs.Visibility = Visibility.Visible;
            clients.Visibility = Visibility.Visible;
            zayvkiForBrigadir.Visibility = Visibility.Collapsed;
            ServicechangeCommitBtn.Visibility = Visibility.Visible;
            DeleteServicePhotoBtn.Visibility = Visibility.Visible;
            AddServicePhotoBtn.Visibility = Visibility.Visible; 
            AddServicePhotoBtn.Visibility = Visibility.Visible;
            BrigadirsLstView.ItemsSource = App.db.Foreman.ToList();
            ClientsLstView.ItemsSource = App.db.Users.ToList();

        }

        private void GetUser()
        {
            Home.Visibility = Visibility.Visible;
            ItemHome.Visibility = Visibility.Visible;
            ItemCreate.Visibility = Visibility.Visible;
            zayavki.Visibility = Visibility.Visible;
            Brigadirs.Visibility = Visibility.Visible;
            clients.Visibility = Visibility.Collapsed;
            DeleteServicePhotoBtn.Visibility = Visibility.Collapsed;
            AddServicePhotoBtn.Visibility = Visibility.Collapsed;
            AddServicePhotoBtn.Visibility = Visibility.Collapsed;
            ServicechangeCommitBtn.Visibility = Visibility.Collapsed;
            zayvkiForBrigadir.Visibility = Visibility.Collapsed;
            
            ZayavkiLstView.ItemsSource = App.db.Zayavki.ToList().Where(x => x.isCart == false && x.IdUser == App.user.Users.Id).Select(x => x.Service);
            CartsLstView.ItemsSource = App.db.Zayavki.ToList().Where(x => x.isCart == true && x.IdUser == App.user.Users.Id);
        }

        private void GetBrigadir()
        {
            Home.Visibility = Visibility.Collapsed;
            ItemCreate.Visibility = Visibility.Collapsed;
            zayavki.Visibility = Visibility.Visible;
            Brigadirs.Visibility = Visibility.Collapsed;
            AddServicePhotoBtn.Visibility = Visibility.Collapsed;
            Brigadir_zayavki.Visibility = Visibility.Visible;
            DeleteServicePhotoBtn.Visibility = Visibility.Collapsed;
            AddServicePhotoBtn.Visibility = Visibility.Collapsed;
            ServicechangeCommitBtn.Visibility = Visibility.Collapsed;
            zayvkiForBrigadir.Visibility = Visibility.Visible;
            clients.Visibility = Visibility.Visible;
            ItemHome.Visibility = Visibility.Visible;
            Zayavki_for_brigadirs_LstView.ItemsSource = App.db.Zayavki.ToList().Where(x => x.isCart == false);
        }
        Service a = new Service();

        private void ServicesLstView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                NavigateToCartBtn.Visibility = Visibility.Visible;
                AddCartBtn.Visibility = Visibility.Visible;
                if (App.user.IdRole == 2 || App.user.IdRole == 3)
                {
                    NavigateToCartBtn.Visibility = Visibility.Hidden;
                    AddCartBtn.Visibility = Visibility.Hidden;
                }
                if (islstView)
                {
                    Services.Visibility = Visibility.Hidden;
                    About_Uslugi.Visibility = Visibility.Visible;
                    a = (db.Service)ServicesLstView.SelectedItem;
                    if (App.db.Zayavki.Where(x => x.Id == a.Id && x.isCart == true && x.IdUser == App.user.Users.Id).FirstOrDefault() != null)
                    {
                        AddCartBtn.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        NavigateToCartBtn.Visibility = Visibility.Collapsed;
                    }
                    DataContext = a;
                }

                DeleteServicePhotoBtn.Visibility = Visibility.Visible;
                isReduct = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Произошла ошибка\n" + ex.Message);
            }
        }

        private void AboutServiceBackBtn_Click(object sender, RoutedEventArgs e)
        {
            About_Uslugi.Visibility = Visibility.Hidden;
            Services.Visibility = Visibility.Visible;
        }

        private void AddCartBtn_Click(object sender, RoutedEventArgs e)
        {
            var id = int.Parse(AddCartBtn.CommandParameter.ToString());
            var serv = App.db.Service.ToList().Where(x => x.Id == id).FirstOrDefault();
            db.Zayavki zayavki = new db.Zayavki();
            zayavki.Users = App.user.Users;
            zayavki.Service = serv;
            zayavki.isCart = true;
            zayavki.isAccept = true;
            zayavki.Date = DateTime.Now;
            App.db.Zayavki.Add(zayavki);
            App.db.SaveChanges();
            NavigateToCartBtn.Visibility = Visibility.Visible;
            AddCartBtn.Visibility = Visibility.Collapsed;
        }

        private void NavigateToCartBtn_Click(object sender, RoutedEventArgs e)
        {
            About_Uslugi.Visibility = Visibility.Hidden;
            User_cart.Visibility = Visibility.Visible;
            CartsLstView.ItemsSource = App.db.Zayavki.ToList().Where(x => x.isCart == true && x.IdUser == App.user.Users.Id);
        }

        private void DeleteServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            if(App.user.IdRole == 3)
            {
                var id = int.Parse(((Button)sender).CommandParameter.ToString());
                var serv = App.db.Service.Where(x => x.Id == id).FirstOrDefault();
                App.db.Service.Remove(serv);
                App.db.SaveChanges();
                MessageBox.Show("Элемент успешно удален!");
                ServicesLstView.ItemsSource = App.db.Service.ToList().Where(x => x.IdCategory == serv.IdCategory);
            }
            else
            {
                MessageBox.Show("Удалять могут только админы!");
            }
        }

        private void cartCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var id = int.Parse(((CheckBox)sender).CommandParameter.ToString());
            var zayavka = App.db.Zayavki.Where(x => x.Id == id).FirstOrDefault();
            CartAmountSumTxt.Text = (decimal.Parse(CartAmountSumTxt.Text) + zayavka.Service.Price).ToString();
            cart.Add(zayavka);
        }

        private void cartCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var id = int.Parse(((CheckBox)sender).CommandParameter.ToString());
            var zayavka = App.db.Zayavki.Where(x => x.Id == id).FirstOrDefault();
            CartAmountSumTxt.Text = (decimal.Parse(CartAmountSumTxt.Text) - zayavka.Service.Price).ToString();
            cart.Remove(zayavka);
        }

        private void DeleteServiceOfCart_Click(object sender, RoutedEventArgs e)
        {
            var id = int.Parse(((Button)sender).CommandParameter.ToString());
            var zayavka = App.db.Zayavki.Where(x => x.Id == id).FirstOrDefault();
            App.db.Zayavki.Remove(zayavka);
            App.db.SaveChanges();
            CartsLstView.ItemsSource = App.db.Zayavki.ToList().Where(x => x.isCart == true && x.IdUser == App.user.Users.Id);   
        }

        private void ServicechangeCommitBtn_Click(object sender, RoutedEventArgs e)
        {
            // коммит всех изменений услуги сделанных админом
        }

        bool isReduct = false;
        private void Add_uslugi_img_Click(object sender, RoutedEventArgs e) 
        {
            if (isReduct)
            {
                var id = int.Parse(((Button)sender).CommandParameter.ToString());
                var OFD = new OpenFileDialog();

                if (OFD.ShowDialog() == true)
                {
                    string link = OFD.FileName;
                    try
                    {
                        var stream = File.ReadAllBytes(link);
                        var user = App.db.Service.Where(x => x.Id == id).FirstOrDefault();
                        user.Photo = stream;
                        DataContext = user;
                        App.db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка\n" + ex.Message);
                    }
                }
            }
            else
            {
                var OFD = new OpenFileDialog();

                if (OFD.ShowDialog() == true)
                {
                    linked = OFD.FileName;
                    try
                    {
                        var stream = File.ReadAllBytes(linked);
                        using (Bitmap bmp = new Bitmap(linked))
                        {
                            Main_img.Source = bmp.BitmapToImageSource();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка\n" + ex.Message);
                    }
                }
            }
        }

        string linked = "";

        private void DeleteServicePhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var id = int.Parse(((Button)sender).CommandParameter.ToString());
                var user = App.db.Service.Where(x => x.Id == id).FirstOrDefault();
                user.Photo1 = null;
                App.db.SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            // добавление услуги в лист
            isReduct = false;
            About_Uslugi.Visibility = Visibility.Visible;
            Description_uslugi.Clear();
            Main_img.Source = null;
            PriceUsluginTxt.Clear();
            NameUslugiTxt.Clear();
            DeleteServicePhotoBtn.Visibility = Visibility.Hidden;
        }

        private void img_profile_change_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var OFD = new OpenFileDialog();

            if (OFD.ShowDialog() == true)
            {
                string link = OFD.FileName;
                try
                {
                    var stream = File.ReadAllBytes(link);
                    App.user.Users.Photo = stream;
                    using(Bitmap bmp = new Bitmap(link))
                        img_profile.Source = bmp.BitmapToImageSource();
                    var user = App.db.Users.Where(x => x.Id == App.user.IdUser).FirstOrDefault();
                    user.Photo = stream;
                    App.db.SaveChanges();

                }
                catch(Exception ex)
                {
                    MessageBox.Show("Произошла ошибка\n" + ex.Message);
                } 
            }
        }

        private void AddUsluguBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isReduct == false)
                {
                    db.Service service = new Service();
                    service.Price = decimal.Parse(PriceUsluginTxt.Text);
                    service.Name = NameUslugiTxt.Text;
                    service.Description = Description_uslugi.Text;
                    service.Photo = File.ReadAllBytes(linked);
                    service.IdCategory = category;
                    App.db.Service.Add(service);
                    App.db.SaveChanges();
                    ServicesLstView.ItemsSource = App.db.Service.ToList().Where(x => x.IdCategory == category);
                    MessageBox.Show("Успешное добавление");
                    AddServiceBtn_Click(null, null);
                }
                else
                {
                    var service = App.db.Service.Where(x => x.Id == a.Id).FirstOrDefault();
                    service.Price = decimal.Parse(PriceUsluginTxt.Text.Replace('.', ','));
                    service.Name = NameUslugiTxt.Text;
                    service.Description = Description_uslugi.Text;
                    service.IdCategory = category;
                    App.db.SaveChanges();
                    ServicesLstView.ItemsSource = App.db.Service.ToList().Where(x => x.IdCategory == category);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var id = int.Parse(((Button)sender).CommandParameter.ToString());
            var item = App.db.Zayavki.Where(x => x.Id == id).FirstOrDefault();
            var kontrakt = App.db.Kontrakt.Where(x => x.IdZayavki == item.Id).FirstOrDefault();
            if(kontrakt != null)
                App.db.Kontrakt.Remove(kontrakt);
            App.db.Zayavki.Remove(item);
            App.db.SaveChanges();
            ZayavkiLstView.ItemsSource = App.db.Zayavki.ToList().Where(x => x.isCart == false && x.IdUser == App.user.Users.Id).Select(x => x.Service);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var item in cart)
            {
                var it = App.db.Zayavki.Where(x => x.Id == item.Id).FirstOrDefault();
                it.isCart = false;
                it.isAccept = true;
                App.db.SaveChanges();
            }
            ZayavkiLstView.ItemsSource = App.db.Zayavki.ToList().Where(x => x.isCart == false && x.IdUser == App.user.Users.Id).Select(x => x.Service);
            CartsLstView.ItemsSource = App.db.Zayavki.ToList().Where(x => x.isCart == true && x.IdUser == App.user.Users.Id);
            cart = new List<Zayavki>();
        }
    }

    public static class Extensions
    {
        public static BitmapImage BitmapToImageSource(this Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }
    }
}
