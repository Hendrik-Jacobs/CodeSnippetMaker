using CodeSnippetMaker.General;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;

namespace CodeSnippetMaker.Models
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Constructors
        public ViewModel()
        {
            Literals = new();

            if (File.Exists(Helper.JsonPath))
            {
                string[] settings = Settings.Load();

                Author = settings[0];
                Language = settings[1];
                ExportFolder = settings[2];
            }

            Literals.CollectionChanged += new NotifyCollectionChangedEventHandler(LiteralsChanged);
        }

        public ViewModel(ViewModel other)
        {
            Literals = new();
            Author = other.Author;
            Language = other.Language;
            ExportFolder = other.ExportFolder;

            Literals.CollectionChanged += new NotifyCollectionChangedEventHandler(LiteralsChanged);
        }
        #endregion Constructors


        #region Fields
        private string _author;
        private string _language;
        private string _exportPath;
        private string _fileName;
        private string _code;
        private string _codeWhite;
        private string _codeViolet;
        private string _title;
        private string _description;
        private string _shortCut;
        #endregion Fields


        #region Properties#
        public ObservableCollection<LiteralModel> Literals { get; set; }

        public string Author
        {
            get
            {
                if (string.IsNullOrEmpty(_author))
                {
                    return "";
                }
                return _author;
            }
            set
            {
                if (_author != value)
                {
                    _author = value;
                    OnPropertyChanged("Author");
                }
            }
        }

        public string Language
        {
            get
            {
                if (string.IsNullOrEmpty(_language))
                {
                    return "";
                }
                return _language;
            }
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged("Language");
                }
            }
        }

        public string ExportFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_exportPath))
                {
                    return "";
                }
                return _exportPath;
            }
            set
            {
                if (_exportPath != value)
                {
                    _exportPath = value;
                    OnPropertyChanged("ExportFolder");
                }
            }
        }

        public string Code
        {
            get
            {
                if (string.IsNullOrEmpty(_code))
                {
                    return "";
                }
                return _code;
            }
            set
            {
                if (_code != value)
                {
                    _code = value;
                    UpdateTexts();
                    OnPropertyChanged("Code");
                }
            }
        }

        public string CodeWhite
        {
            get
            {
                if (string.IsNullOrEmpty(_codeWhite))
                {
                    return "";
                }
                return _codeWhite;
            }
            set
            {
                if (_codeWhite != value)
                {
                    _codeWhite = value;
                    OnPropertyChanged("CodeWhite");
                }
            }
        }

        public string CodeViolet
        {
            get
            {
                if (string.IsNullOrEmpty(_codeViolet))
                {
                    return "";
                }
                return _codeViolet;
            }
            set
            {
                if (_codeViolet != value)
                {
                    _codeViolet = value;
                    OnPropertyChanged("CodeViolet");
                }
            }
        }

        public string Title
        {
            get
            {
                if (string.IsNullOrEmpty(_title))
                {
                    return "";
                }
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(_description))
                {
                    return "";
                }
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public string ShortCut
        {
            get
            {
                if (string.IsNullOrEmpty(_shortCut))
                {
                    return "";
                }
                return _shortCut;
            }
            set
            {
                if (_shortCut != value)
                {
                    _shortCut = value;
                    OnPropertyChanged("ShortCut");
                }
            }
        }

        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(_fileName))
                {
                    return "";
                }
                return _fileName;
            }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged("FileName");
                }
            }
        }
        #endregion Properties

        private void LiteralsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateTexts();
        }

        public void UpdateTexts()
        {
            Dictionary<string, string> texts = Texts.SplitColors(this);
            CodeWhite = texts["White"];
            CodeViolet = texts["Violet"];
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChanged Members}}
    }
}