using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Reflection;
using System.Windows.Media.Imaging;
using Catel.Windows;
using System;
using System.IO;

namespace GoldenEagle.Desktop.Framwork.Views
{
    /// <summary>
    /// SplashScreen1.xaml 的交互逻辑
    /// </summary>
    public partial class SplashScreen : DataWindow
    {
        private const string SplashScreenLocation = "Resources\\Images\\SplashScreen.png";
        private const string SplashScreenFallbackLocation = "/GoldenEagle.Desktop.Framwork;component/Resources/Images/SplashScreen.png";


         /// <summary>
        /// Initializes a new instance of the <see cref="SplashScreen" /> class.
        /// </summary>
        public SplashScreen()
            : base(DataWindowMode.Custom)
        {
            InitializeComponent();

            InitializeSplashScreen();
        }

        /// <summary>
        /// Initializes the splash screen.
        /// </summary>
        private void InitializeSplashScreen()
        {
            var directory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

            try
            {
                string firstAttemptFile = Path.Combine(directory, SplashScreenLocation);
                if (File.Exists(firstAttemptFile))
                {
                    splashScreenImage.Source = new BitmapImage(new Uri(firstAttemptFile, UriKind.Absolute));
                    return;
                }
            }
            catch (Exception)
            {
                // Swallow exception
            }

            splashScreenImage.Source = new BitmapImage(new Uri(SplashScreenFallbackLocation, UriKind.RelativeOrAbsolute));
        }
    }
}
