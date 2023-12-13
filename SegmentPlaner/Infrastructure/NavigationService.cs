using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using SegmentPlaner.UI.Windows.Shell;
using System;

namespace SegmentPlaner.Infrastructure
{
    class NavigationService
    {
        private Frame rootFrame;

        public void NavigateTo<T>()
        {
            if (rootFrame == null)
            {
                rootFrame = App.Services.GetRequiredService<MainWindow>().RootFrame;
            }

            Type preNavPageType = rootFrame.CurrentSourcePageType;

            if (typeof(T) is not null && !Type.Equals(preNavPageType, typeof(T)))
            {

                rootFrame.Navigate(typeof(T));
            }
        }
    }
}
