using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Point = System.Windows.Point;

namespace rori
{
    public partial class SelectionWindow : Window, INotifyPropertyChanged
    {
        private Point _firstPoint;

        private bool _isMouseDown;

        private Point _secondPoint;

        public SelectionWindow(ImageBrush background)
        {
            InitializeComponent();
            ScreenBackground = background;
            DataContext = this;
            Left = SystemParameters.VirtualScreenLeft;
            Top = SystemParameters.VirtualScreenTop;
            Width = SystemParameters.VirtualScreenWidth;
            Height = SystemParameters.VirtualScreenHeight;
        }

        public Rect ScreenRect
        {
            get { return new Rect(0, 0, SystemParameters.VirtualScreenWidth, SystemParameters.VirtualScreenHeight); }
        }

        public Rect SelectionRect
        {
            get { return new Rect(FirstPoint, SecondPoint); }
        }

        protected Point FirstPoint
        {
            get { return _firstPoint; }
            set
            {
                _firstPoint = value;
                OnPropertyChanged("SelectionRect");
            }
        }

        protected Point SecondPoint
        {
            get { return _secondPoint; }
            set
            {
                _secondPoint = value;
                OnPropertyChanged("SelectionRect");
            }
        }

        public new static bool IsActive { get; private set; }

        public ImageBrush ScreenBackground { get; private set; }

        public Rectangle Result { get; private set; }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged members

        #region Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Background = ScreenBackground;
            IsActive = true;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            _isMouseDown = true;
            FirstPoint = e.GetPosition(this);
            SecondPoint = FirstPoint;
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Released || !_isMouseDown) return;
            if (!(SelectionRect.Height > 0) && !(SelectionRect.Width > 0)) return;
            DialogResult = true;
            var resultRectangle = new Rectangle()
            {
                X = (int)SelectionRect.X,
                Y = (int)SelectionRect.Y,
                Width = (int)SelectionRect.Width,
                Height = (int)SelectionRect.Height
            };
            Result = resultRectangle;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed || !_isMouseDown) return;
            SecondPoint = e.GetPosition(this);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            IsActive = false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape || !_isMouseDown)
            {
                if (e.Key != Key.Escape) return;
                DialogResult = false;
                Close();
            }
            else
            {
                _isMouseDown = false;
                FirstPoint = new Point(0, 0);
                SecondPoint = new Point(0, 0);
            }
        }

        #endregion Events
    }
}