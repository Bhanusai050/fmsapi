using FmsAPI.Data;
using FmsAPI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FmsAPI.Service
{
    public class FeedPurchaseService: IFeedPurchaseService
    {
    FarmManagementSystemEnities context = new FarmManagementSystemEnities();

        public List<FeedPurchas> GetFeedPurchases()
        {
            return context.FeedPurchases.ToList(); 
        }
    }
}
