using ePizzaHub.Entities;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Repositories.Models;
using ePizzaHub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IRepository<CartItem> _cartItem;

        public CartService(ICartRepository cartRepository, IRepository<CartItem> cartItem)
        {
            _cartRepository = cartRepository;
            _cartItem = cartItem;
        }
        public Cart AddItem(int userId, Guid cartId, int itemId, decimal unitPrice, int quantity)
        {
            try
            {
                Cart cart = _cartRepository.GetCart(cartId);
                if (cart == null)
                {
                    cart = new Cart();
                    CartItem cartItem = new CartItem(itemId, quantity, unitPrice);
                    cart.Id = cartId;
                    cart.UserId = userId;
                    cart.CreatedDate = DateTime.Now;
                    cartItem.CartId = cart.Id;
                    cart.Items.Add(cartItem);
                    _cartRepository.Add(cart);
                    _cartRepository.SaveChanges();
                }
                else
                {
                    CartItem cartItem = cart.Items.Where(p=>p.ItemId==itemId).FirstOrDefault();
                    if(cartItem != null)
                    {
                        cartItem.Quantity += quantity;
                        _cartItem.Update(cartItem);
                        _cartItem.SaveChanges();

                    }
                    else
                    {
                        cartItem = new CartItem(itemId, quantity, unitPrice);
                        cartItem.CartId = cart.Id;
                        cart.Items.Add(cartItem);

                        _cartItem.Update(cartItem);
                        _cartItem.SaveChanges();

                    }
                }
                return cart;
            }
            catch (Exception)
            {

               return null;
            }
        }

        public int DeleteItem(Guid cartId, int itemId)
        {
            return _cartRepository.DeleteItem(cartId, itemId);
        }

        public int GetCartCount(Guid cartId)
        {
            var cart=_cartRepository.GetCart(cartId);
            return cart!=null? cart.Items.Count() : 0;
        }

        public CartModel GetCartDetails(Guid cartId)
        {
            var model = _cartRepository.GetCartDetails(cartId);
            if (model != null && model.Items.Count > 0)
            {
                decimal subTotal = 0;
                foreach (var item in model.Items)
                {
                    item.Total = item.UnitPrice * item.Quantity;
                    subTotal += item.Total;
                }
                model.Total = subTotal;
                //5% tax
                model.Tax = Math.Round((model.Total * 5) / 100, 2);
                model.GrandTotal = model.Tax + model.Total;
            }
            return model;
        }

        public int UpdateCart(Guid cartId, int userId)
        {
           return _cartRepository.UpdateCart(cartId, userId);
        }

        public int UpdateQuantity(Guid cartId, int id, int quantity)
        {
            return _cartRepository.UpdateQuantity(cartId, id, quantity);
        }
    }
}
