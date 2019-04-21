using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Invoices.Views;

namespace Invoices
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region FIELDS

        public const int MenuButtonHeight = 50;
        public const int ViewbarButtonWidth = 120;

        private static CustomButton _btnInvoices;
        private static CustomButton _btnData;

        private static UserControl _selectedView;
        private static UserControl _previousView;
        private List<UserControl> _views;

        private static ItemsControl _icInvoicesButtons;
        private static CustomButton _btnCreatorView;
        private static CustomButton _btnInvoicesView;

        private static ItemsControl _icDataButtons;
        private static CustomButton _btnVendorsView;
        private static CustomButton _btnCustomersView;
        private static CustomButton _btnConsumersView;
        #endregion

        #region CONSTRUCTORS

        public MainWindow()
        {
            InitializeComponent();

            ViewManager.SetMainWindow(this);
        }

        #endregion

        #region EVENTS

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _views = new List<UserControl>();
                SecondMenu.Visibility = Visibility.Hidden;

                var menuButtons = new List<CustomButton>();
                _btnInvoices = CreateButton("btnInvoices", Properties.strings.btnInvoices, MenuButtonHeight,
                    btnInvoices_Click);
                menuButtons.Add(_btnInvoices);
                _btnData = CreateButton("btnData", Properties.strings.btnData, MenuButtonHeight,
                    btnData_Click);
                menuButtons.Add(_btnData);

                _firstPanel.ItemsSource = menuButtons;

                _icInvoicesButtons = XamlReader.Parse(XamlWriter.Save(_secondPanel)) as ItemsControl;
                var b1Buttons = new List<CustomButton>();
                _btnCreatorView = CreateButton("btnCreator", Properties.strings.btnCreator, MenuButtonHeight,
                    BtnCreatorView_Click);
                b1Buttons.Add(_btnCreatorView);

                _btnInvoicesView = CreateButton("btnInvoices", Properties.strings.btnInvoices, MenuButtonHeight,
                    BtnInvoicesView_Click);
                b1Buttons.Add(_btnInvoicesView);

                if (_icInvoicesButtons != null) _icInvoicesButtons.ItemsSource = b1Buttons;

                _icDataButtons = XamlReader.Parse(XamlWriter.Save(_secondPanel)) as ItemsControl;
                var b2Buttons = new List<CustomButton>();
                _btnVendorsView = CreateButton("btnVendors", Properties.strings.btnVendors, MenuButtonHeight,
                    BtnVendorsView_Click);
                b2Buttons.Add(_btnVendorsView);

                _btnConsumersView = CreateButton("btnConsumers", Properties.strings.btnConsumers, MenuButtonHeight,
                    BtnConsumersView_Click);
                b2Buttons.Add(_btnConsumersView);

                _btnCustomersView = CreateButton("btnCustomers", Properties.strings.btnCustomers, MenuButtonHeight,
                    BtnCustomersView_Click);
                b2Buttons.Add(_btnCustomersView);

                if (_icDataButtons != null) _icDataButtons.ItemsSource = b2Buttons;
            }
            catch (Exception ex)
            {
                var dialog = new MessageBox(Properties.strings.messageBoxStatement, ex.Message);
                dialog.ShowDialog();
            }
        }

        #region menuItems

        private void btnInvoices_Click(object sender, RoutedEventArgs e)
        {
            ShowOrHideSecondMenu(_icInvoicesButtons);
            SetSecondMenu(_icInvoicesButtons);
        }

        private void btnData_Click(object sender, RoutedEventArgs e)
        {
            ShowOrHideSecondMenu(_icDataButtons);
            SetSecondMenu(_icDataButtons);
        }

        #endregion

        #region InvoicesItems
        private void BtnCreatorView_Click(object sender, RoutedEventArgs e)
        {
            CreateAndOpenNewView(new CreatorView());
        }

        private void BtnInvoicesView_Click(object sender, RoutedEventArgs e)
        {
            CreateAndOpenNewView(new ShowInvoicesView());
        }
        #endregion

        #region DataItems
        private void BtnCustomersView_Click(object sender, RoutedEventArgs e)
        {
            CreateAndOpenNewView(new ShowCustomersView());
        }

        private void BtnConsumersView_Click(object sender, RoutedEventArgs e)
        {
            CreateAndOpenNewView(new ShowConsumersView());
        }

        private void BtnVendorsView_Click(object sender, RoutedEventArgs e)
        {
            CreateAndOpenNewView(new ShowVendorsView());
        }
        #endregion

        #region TITLE_BAR

        /// <summary>
        /// Handles the MouseDown event of the TitleBar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;
            if (e.ClickCount == 2)
            {
                AdjustWindowSize();
            }
            else
            {
                if (Application.Current.MainWindow != null) Application.Current.MainWindow.DragMove();
            }
        }

        /// <summary>
        /// Adjusts the size of the window.
        /// </summary>
        private void AdjustWindowSize()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                var resourceUri = new Uri("img/max.png", UriKind.Relative);
                var streamInfo = Application.GetResourceStream(resourceUri);

                if (streamInfo == null) return;
                var temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush
                {
                    ImageSource = temp
                };
                MaxButton.Background = brush;
            }
            else
            {
                WindowState = WindowState.Maximized;
                var resourceUri = new Uri("img/min.png", UriKind.Relative);
                var streamInfo = Application.GetResourceStream(resourceUri);

                if (streamInfo == null) return;
                var temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush
                {
                    ImageSource = temp
                };
                MaxButton.Background = brush;
            }
        }

        /// <summary>
        /// Handles the Click event of the MinButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Handles the Click event of the MaxButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MaxButton_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }

        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #endregion

        #region private

        private void SetSecondMenu(ItemsControl itemsControl)
        {
            _secondStockPanel.Children.Clear();
            _secondStockPanel.Children.Add(itemsControl);
        }

        private void ShowOrHideSecondMenu(ItemsControl itemsControl)
        {
            if (_secondStockPanel.Children.Contains(itemsControl))
            {
                SecondMenu.Visibility =
                    SecondMenu.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
            }
            else
            {
                SecondMenu.Visibility = Visibility.Visible;
            }
        }

        private void CreateAndOpenNewView(UserControl view)
        {
            if (SecondMenu.IsVisible)
            {
                SecondMenu.Visibility = Visibility.Hidden;
            }

            AddUserControl(view);
            OpenUserControl(view);
        }

        private CustomButton CreateButton(string name, string content, int height, RoutedEventHandler operation)
        {
            var button = new CustomButton
            {
                Name = name,
                Content = content,
            };
            button.Click += operation;
            button.Height = height;
            button.Style = (Style) FindResource("MyButton");

            return button;
        }

        private ButtonWithObject CreateViewBarButton(string name, string content, int width, object insideObject,
            bool isSelected, RoutedEventHandler operation)
        {
            var brush = (Brush) FindResource("MyAzure");
            var style = (Style) FindResource("MyButton");
            if (isSelected)
            {
                brush = (Brush) FindResource("MyWhite");
                style = (Style) FindResource("MyButtonBarView");
            }

            var columnDefinition1 = new ColumnDefinition()
            {
                Width = new GridLength(ViewbarButtonWidth - 30),
            };
            var columnDefinition2 = new ColumnDefinition()
            {
                Width = new GridLength(20),
            };
            var grid = new Grid
            {
                Width = ViewbarButtonWidth - 10,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = (Brush) FindResource("Transparent"),
                ColumnDefinitions =
                {
                    columnDefinition1,
                    columnDefinition2
                },
            };

            var label = new Label
            {
                Content = content,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = (Brush) FindResource("Transparent"),
                Foreground = brush,
            };
            var closeButton = CreateCloseButton(insideObject);
            closeButton.HorizontalAlignment = HorizontalAlignment.Right;
            label.SetValue(Grid.ColumnProperty, 0);
            closeButton.SetValue(Grid.ColumnProperty, 1);
            grid.Children.Add(label);
            grid.Children.Add(closeButton);

            var button = new ButtonWithObject
            {
                Name = name,
                Content = grid,
                Object = insideObject,
                Context = label,
            };
            button.Click += operation;
            if (!isSelected)
            {
                button.MouseEnter += MouseEnter_Event;
                button.MouseLeave += MouseLeave_Event;
            }

            button.Width = width;
            button.Style = style;

            return button;
        }

        private void MouseLeave_Event(object sender, MouseEventArgs e)
        {
            var button = (ButtonWithObject) sender;
            ((Label) button.Context).Foreground = (Brush) FindResource("MyAzure");
        }

        private void MouseEnter_Event(object sender, MouseEventArgs e)
        {
            var button = (ButtonWithObject) sender;
            ((Label) button.Context).Foreground = (Brush) FindResource("MyDarkGrey");
        }

        public void AddUserControl(UserControl uc)
        {
            if (_views.Any(u =>
                (u as IRepresentative)?.RepresentativeName == (uc as IRepresentative)?.RepresentativeName))
            {
                return;
            }

            _views.Add(uc);
        }

        public void OpenUserControl(UserControl uc)
        {
            _previousView = _selectedView;
            _selectedView = _views.FirstOrDefault(u =>
                (u as IRepresentative)?.RepresentativeName == (uc as IRepresentative)?.RepresentativeName);
            brdMain.Child = _selectedView;
            UpdateViewBar(_selectedView);
        }

        public void RemoveUserControl(UserControl uc)
        {
            var userControl = _views.First(u =>
                (u as IRepresentative)?.RepresentativeName == (uc as IRepresentative)?.RepresentativeName);
            _views.Remove(userControl);
        }

        private void UpdateViewBar(UserControl selected)
        {
            _viewBarStockPanel.Children.Clear();

            foreach (var item in _views)
            {
                var tmp = CreateViewBarButton($"btn{item}", (item as IRepresentative)?.RepresentativeName,
                    ViewbarButtonWidth, item,
                    (item as IRepresentative)?.RepresentativeName == (selected as IRepresentative)?.RepresentativeName,
                    BtnView_Click);
                _viewBarStockPanel.Children.Add(tmp);
            }
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            var uc = (sender as ButtonWithObject)?.Object as UserControl;
            OpenUserControl(uc);
        }

        #endregion

        private ButtonWithObject CreateCloseButton(object uc)
        {
            var newButton = new ButtonWithObject
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri(@"..\img\x-icon.png", UriKind.Relative)),
                    VerticalAlignment = VerticalAlignment.Center
                },
                Object = uc,
                Width = 20,
                Height = 20,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Style = (Style) FindResource("MyButton"),
            };
            newButton.Click += RemoveFromBarView_Click;

            return newButton;
        }

        private void RemoveFromBarView_Click(object sender, RoutedEventArgs e)
        {
            var uc = (sender as ButtonWithObject)?.Object as UserControl;
            RemoveFromBarView(uc);

            e.Handled = true;
        }

        public void RemoveFromBarView(UserControl uc)
        {
            RemoveUserControl(uc);
            if ((_selectedView as IRepresentative)?.RepresentativeName == (uc as IRepresentative)?.RepresentativeName)
            {
                OpenUserControl(_previousView);
            }

            if ((_previousView as IRepresentative)?.RepresentativeName == (uc as IRepresentative)?.RepresentativeName)
            {
                _previousView = _views.Any() ? _views[0] : null;
            }

            UpdateViewBar(_selectedView);
        }

        private void SecondMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            SecondMenu.Visibility = Visibility.Hidden;
        }
    }
}
