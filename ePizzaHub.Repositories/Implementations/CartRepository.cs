﻿using ePizzaHub.Entities;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Repositories.Implementations
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private AppDbContext appContext
        {
            get
            {
                return _dbContext as AppDbContext;
            }
        }
        public CartRepository(DbContext dbContext):base(dbContext)
        {

        }
        public Cart GetCart(Guid cartId)
        {
            return appContext.Carts.Include(c=>c.Items).Where(r=>r.Id == cartId && r.IsActive==true).FirstOrDefault();
        }

        public int DeleteItem(Guid cartId, int itemId)
        {
            var item = appContext.CartItems.Where(c => c.CartId == cartId && c.Id == itemId).FirstOrDefault();

            if (item != null)
            {
                appContext.CartItems.Remove(item);
                return appContext.SaveChanges();
            }
            else
            {
                return 0;
            }
        }

        public int UpdateQuantity(Guid cartId, int itemId, int quantity)
        {
            bool flag = false;
            var cart = GetCart(cartId);
            if(cart !=null)
            {
                for (int i = 0; i < cart.Items.Count; i++)
                {
                    flag = true;
                    if (quantity < 0 && cart.Items[i].Quantity > 1)
                        cart.Items[i].Quantity += (quantity);
                    else if(quantity > 0)
                        cart.Items[i].Quantity += quantity;
                    break;
                }
                if(flag)
                {
                    return appContext.SaveChanges();
                }
            }
            return 0;
        }

        public int UpdateCart(Guid cartId, int userId)
        {
            Cart cart = GetCart(cartId);
            cart.UserId = userId;
            return appContext.SaveChanges();
        }

        public CartModel GetCartDetails(Guid cartId)
        {
            var model = (from cart in appContext.Carts
                         where cart.Id == cartId && cart.IsActive == true
                         select new CartModel
                         {
                             Id = cart.Id,
                             UserId = cart.UserId,
                             CreatedDate = cart.CreatedDate,
                             Items = (from cartItem in appContext.CartItems
                                      join item in appContext.Items
                                      on cartItem.ItemId equals item.Id
                                      where cartItem.CartId == cartId
                                      select new ItemModel
                                      {
                                          Id = cartItem.Id,
                                          Name = item.Name,
                                          Description = item.Description,
                                          ImageUrl = item.ImageUrl,
                                          Quantity = cartItem.Quantity,
                                          ItemId = item.Id,
                                          UnitPrice = cartItem.UnitPrice,
                                      }).ToList()
                         }).FirstOrDefault();
            return model;
        }
    }
}
