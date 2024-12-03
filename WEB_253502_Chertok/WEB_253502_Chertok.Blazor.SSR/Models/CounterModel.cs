using System.ComponentModel.DataAnnotations;

namespace WEB_253502_Chertok.Blazor.SSR.Models
{
	public class CounterModel
	{
		[Range(1, 10)]
		public int Value { get; set; }
	}
}
