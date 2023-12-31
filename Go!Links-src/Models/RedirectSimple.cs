/***

Copyright 2023, SV Foster. All rights reserved.

License:
    This program is free for personal, educational and/or non-profit usage    

Revision History:

***/

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Go_Links.Models
{
	public class RedirectSimple
	{
		public string DisplayName { get; }
		public string URL { get; }

		public RedirectSimple(string? d, string u)
		{
			if (String.IsNullOrEmpty(d))
				DisplayName = u;
			else
				DisplayName = d;

			URL = u;
		}
	}
}
