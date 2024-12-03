using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253502_Chertok.Domain.Entities;

namespace WEB_253502_Chertok.Domain.Models
{
	public class CartItem
	{
		public Product Item { get; set; }
		public int Amount { get; set; }
	}
}
