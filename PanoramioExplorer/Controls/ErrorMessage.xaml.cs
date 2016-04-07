using System;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace PanoramioExplorer.Controls
{
    public sealed partial class ErrorMessage
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ErrorMessage),
                                        new PropertyMetadata(default(string), OnTextChanged));

        public static readonly DependencyProperty RetryCommandProperty =
            DependencyProperty.Register("RetryCommand", typeof(ICommand), typeof(ErrorMessage),
                new PropertyMetadata(default(ICommand), OnRetryCommandChanged));

        private static void OnTextChanged(DependencyObject target,
                                          DependencyPropertyChangedEventArgs e)
        {
            (target as ErrorMessage).SetErrorText(e.NewValue as string);
        }

        private static void OnRetryCommandChanged(DependencyObject target,
                                                 DependencyPropertyChangedEventArgs e)
        {
            (target as ErrorMessage).SetRetryCommand(e.NewValue as ICommand);
        }

        public ErrorMessage()
        {
            InitializeComponent();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public ICommand RetryCommand
        {
            get { return (ICommand)GetValue(RetryCommandProperty); }
            set { SetValue(RetryCommandProperty, value); }
        }

        private void SetErrorText(string newText)
        {
            ErrorDetails.Text = newText ?? string.Empty;
        }

        private void SetRetryCommand(ICommand command)
        {
            RetryButton.Command = command;
        }
    }
}