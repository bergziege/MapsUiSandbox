using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SegmentPlaner.Modules.Segments.UI
{
    internal sealed partial class SegmentView : UserControl
    {
        public SegmentView()
        {
            this.InitializeComponent();
        }

        public SegmentViewModel Data
        {
            get { return (SegmentViewModel)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(SegmentViewModel), typeof(SegmentView), new PropertyMetadata(default));

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem != null)
            {
                listView.ScrollIntoView(listView.SelectedItem);
            }
        }
    }
}
