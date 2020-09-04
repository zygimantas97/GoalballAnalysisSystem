using GoalballAnalysisSystem.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoalballAnalysisSystem.WPF.View
{
    /// <summary>
    /// Interaction logic for RegistrationView.xaml
    /// </summary>
    public partial class RegistrationView : UserControl
    {
        public ICommand RegisterCommand
        {
            get { return (ICommand)GetValue(RegisterCommandProperty); }
            set { SetValue(RegisterCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LoginCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegisterCommandProperty =
            DependencyProperty.Register("RegisterCommand", typeof(ICommand), typeof(RegistrationView), new PropertyMetadata(null));

        public RegistrationView()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if(RegisterCommand != null)
            {
                string[] passInfo = new string[2];
                passInfo[0] = passwordBox.Password;
                passInfo[1] = confirmPasswordBox.Password;
                RegisterCommand.Execute(passInfo);
            }
        }
    }
}
