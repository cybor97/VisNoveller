using System;

namespace VisNoveller.Windows.Reader
{
	public static class Extensions
	{
		static Random Magic = new Random();

		public static string GetRandomString(int length, bool cyrillic = false)
		{
			string result = string.Empty;
			for (int i = 0; i < length; i++)
				if (cyrillic)
					result += (char)Magic.Next('А'-5, 'я'+5);
				else
					result += (char)Magic.Next('A'-5, 'z'+5);
			return result;
		}
	}
}

