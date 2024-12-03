﻿namespace WEB_253502_Chertok.Extensions
{
	public static class HttpRequestExtensions
	{
		public static bool IsAjaxRequest(this HttpRequest request)
		{
			return request.Headers["X-Requested-With"] == "XMLHttpRequest";
		}
	}
}
