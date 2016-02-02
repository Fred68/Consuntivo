//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace StringExtension
	{
	/// <summary>
	/// bool found = s1.ContainsWithWildcards(s2);
	/// </summary>
	static class StringExtension
		{
		public static bool ContainsWithWildcards(this string value, string search)
			{
			return new Regex("^" + Regex.Escape(search).Replace(@"\*", ".*").Replace(@"\?", ".") + "$", RegexOptions.IgnoreCase | RegexOptions.Singleline).IsMatch(value);
			}
		}
	}
