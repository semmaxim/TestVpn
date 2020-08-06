namespace TestVpn.Providers
{
	public class Provider
	{
		public Provider(string name, string url, byte[] data)
		{
			Name = name;
			Url = url;
			Data = data;
		}

		public string Name { get; }
		public string Url { get; }
		public byte[] Data { get; }
	}
}