using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253502_Chertok.Domain.Entities;

namespace WEB_253502_Chertok.Domain.Models
{
	public class Cart
	{
		public Dictionary<int, CartItem> Items { get; set; } = new();
		public virtual void AddToCart(Product product)
		{
			if (Items.ContainsKey(product.Id))
				++Items[product.Id].Amount;
			else
				Items.Add(product.Id, new CartItem { Item = product, Amount = 1 });
		}
		public virtual void RemoveItems(int id)
		{
			if (Items.ContainsKey(id) && --Items[id].Amount <= 0)
			{
				Items.Remove(id);
			}
		}
		public virtual void ClearAll()
		{
			Items.Clear();
		}
		public int Count { get => Items.Sum(item => item.Value.Amount); }
		public decimal CountPrice { get => Items.Sum(item => item.Value.Item.Price * item.Value.Amount); }
	}
}
