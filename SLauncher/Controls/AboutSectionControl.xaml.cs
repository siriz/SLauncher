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
using Windows.ApplicationModel.Resources;

namespace SLauncher.Controls
{
    public sealed partial class AboutSectionControl : UserControl
    {
        private ResourceLoader _resourceLoader;

        public AboutSectionControl()
        {
            this.InitializeComponent();
            _resourceLoader = ResourceLoader.GetForViewIndependentUse();
            LoadLocalizedStrings();
        }

        private void LoadLocalizedStrings()
        {
            try
            {
                SubtitleText.Text = _resourceLoader.GetString("AboutSubtitle");
                VersionLabel.Text = _resourceLoader.GetString("AboutVersion");
                ForkByLabel.Text = _resourceLoader.GetString("AboutForkBy");
                BasedOnLabel.Text = _resourceLoader.GetString("AboutBasedOn");
                GitHubLabel.Text = _resourceLoader.GetString("AboutGitHub");
                LicenseLabel.Text = _resourceLoader.GetString("AboutLicense");
                LicenseLink.Content = _resourceLoader.GetString("AboutViewLicense");
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


