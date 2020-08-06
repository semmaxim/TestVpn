using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace TestVpn.UI.Connection
{
	public class PasswordBoxHelper
	{
		public static readonly DependencyProperty PasswordProperty =
			DependencyProperty.RegisterAttached("Password",
				typeof(SecureString), typeof(PasswordBoxHelper),
				new FrameworkPropertyMetadata(new SecureString(), OnPasswordPropertyChanged));

		public static readonly DependencyProperty AttachProperty =
			DependencyProperty.RegisterAttached("Attach",
				typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false, Attach));

		private static readonly DependencyProperty IsUpdatingProperty =
			DependencyProperty.RegisterAttached("IsUpdating", typeof(bool),
				typeof(PasswordBoxHelper));


		public static void SetAttach(DependencyObject dp, bool value)
		{
			dp.SetValue(AttachProperty, value);
		}

		public static bool GetAttach(DependencyObject dp)
		{
			return (bool)dp.GetValue(AttachProperty);
		}

		public static SecureString GetPassword(DependencyObject dp)
		{
			return (SecureString)dp.GetValue(PasswordProperty);
		}

		public static void SetPassword(DependencyObject dp, SecureString value)
		{
			dp.SetValue(PasswordProperty, value);
		}

		private static bool GetIsUpdating(DependencyObject dp)
		{
			return (bool)dp.GetValue(IsUpdatingProperty);
		}

		private static void SetIsUpdating(DependencyObject dp, bool value)
		{
			dp.SetValue(IsUpdatingProperty, value);
		}

		private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var passwordBox = (PasswordBox)sender;
			passwordBox.PasswordChanged -= PasswordChanged;

			if (!(bool)GetIsUpdating(passwordBox))
			{
				passwordBox.Password = (string)e.NewValue;
			}

			passwordBox.PasswordChanged += PasswordChanged;
		}

		private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if (!(sender is PasswordBox passwordBox))
				return;

			if ((bool)e.OldValue)
			{
				passwordBox.PasswordChanged -= PasswordChanged;
			}

			if ((bool)e.NewValue)
			{
				passwordBox.PasswordChanged += PasswordChanged;
			}
		}

		private static void PasswordChanged(object sender, RoutedEventArgs e)
		{
			var passwordBox = (PasswordBox)sender;
			SetIsUpdating(passwordBox, true);
			SetPassword(passwordBox, passwordBox.SecurePassword);
			SetIsUpdating(passwordBox, false);
		}
	}
}