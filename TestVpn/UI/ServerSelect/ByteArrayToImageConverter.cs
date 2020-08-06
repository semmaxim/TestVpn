using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TestVpn.UI.ServerSelect
{
	public class ByteArrayToImageConverter : IValueConverter
	{
		/// <inheritdoc />
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var result = new BitmapImage();
			using (var stream = new MemoryStream((byte[]) value))
			{
				stream.Position = 0;
				result.BeginInit();
				result.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
				result.CacheOption = BitmapCacheOption.OnLoad;
				result.UriSource = null;
				result.StreamSource = stream;
				result.EndInit();
			}
			result.Freeze();
			return result;
		}

		/// <inheritdoc />
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}