using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using SLauncher.Classes;

namespace SLauncher.Controls
{
    public sealed partial class AboutSectionControl : UserControl
    {
        public AboutSectionControl()
        {
            this.InitializeComponent();
            this.Loaded += AboutSectionControl_Loaded;
        }

        private void AboutSectionControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLocalizedStrings();
        }

        private void LoadLocalizedStrings()
        {
            try
            {
                SubtitleText.Text = LocalizationManager.GetString("AboutSubtitle");
                VersionLabel.Text = LocalizationManager.GetString("AboutVersion");
                ForkByLabel.Text = LocalizationManager.GetString("AboutForkBy");
                BasedOnLabel.Text = LocalizationManager.GetString("AboutBasedOn");
                GitHubLabel.Text = LocalizationManager.GetString("AboutGitHub");
                LicenseLabel.Text = LocalizationManager.GetString("AboutLicense");
                LicenseLink.Content = LocalizationManager.GetString("AboutViewLicense");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AboutSectionControl] Error loading localized strings: {ex.Message}");
            }
        }

        private void VersionText_Loaded(object sender, RoutedEventArgs e)
        {
            // Update the version string
            string versionString = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            // Do not display the last .0
            VersionText.Text = versionString.Substring(0, versionString.Length - 2);
        }
    }
}


