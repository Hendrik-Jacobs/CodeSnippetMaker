using CodeSnippetMaker.General;
using CodeSnippetMaker.Models;
using System;
using System.ComponentModel;
using System.Windows;

namespace CodeSnippetMaker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _view = new();
            DataContext = _view;
        }

        private ViewModel _view;

        private void OnWinClose(object sender, CancelEventArgs e) => Settings.Save(_view);

        private void ClickOpen(object sender, EventArgs e)
        {
            ViewModel view = Import.Run(_view);
            if (view != null)
            {
                _view = view;
                DataContext = _view;
            }
        }

        private void ClickExport(object sender, EventArgs e) => Export.Run(_view);

        private void ColumnLostFocus(object sender, RoutedEventArgs e) => _view.UpdateTexts();

        private void ClickFolder(object sender, EventArgs e)
        {
            string folder = Picker.FolderPicker();
            if (string.IsNullOrEmpty(folder)) { return; }
            _view.ExportFolder = folder;
        }
    }
}