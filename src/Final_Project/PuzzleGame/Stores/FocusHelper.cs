using System.Windows;


namespace PuzzleGame.Store
{
    public static class FocusHelper
    {
        public static readonly DependencyProperty IsFocusedProperty =      //register attached property
            DependencyProperty.RegisterAttached("IsFocused", typeof(bool), 
                typeof(FocusHelper), new PropertyMetadata(false, OnIsFocusedChanged));

        public static void SetIsFocused(UIElement element, bool value)
        {
            element.SetValue(IsFocusedProperty, value);
        }

        public static bool GetIsFocused(UIElement element)
        {
            return (bool)element.GetValue(IsFocusedProperty);
        }

        private static void OnIsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element && (bool)e.NewValue)
            {
                element.Focus();        //set focus on element
            }
        }
    }
}