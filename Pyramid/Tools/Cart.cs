using Pyramid.Models.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Tools
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(ProductCartModel product, int quantity)
        {
            CartLine line = lineCollection
                .Where(g => g.Product.Id == product.Id)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void SetQuantity(int productId, int quantity)
        {
            CartLine line = lineCollection
                .Where(g => g.Product.Id == productId)
                .FirstOrDefault();

            if (line != null)
            {
                line.Quantity = quantity;
            }
            
        }

        public void RemoveLine(int productId)
        {
            lineCollection.RemoveAll(l => l.Product.Id == productId);
        }

        public double ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Product.Price * e.Quantity);

        }
        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }

        public Cart() {
        }
        public Cart(Cart obj)
        {
            this.lineCollection = new List<CartLine>();
            this.lineCollection.AddRange(obj.Lines);
        }
        public Cart(List<CartLine> lines)
        {
            lineCollection = lines;
        }
    }

    public class CartLine
    {
        public ProductCartModel  Product { get; set; }
        public int Quantity { get; set; }
    }
}