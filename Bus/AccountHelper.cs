using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebAdmin_KingAnttorny.Models;
using Microsoft.EntityFrameworkCore;
using API.KingAttorney.ViewModels;
using WebAdmin_KingAnttorny.Bus;
namespace API.KingAttorney.Bus
{
    public class AccountHelper
    {
        public async Task<List<VMPartner>> getPartner(string keyword, int status)
        {

            if (string.IsNullOrEmpty(keyword)) return new List<VMPartner>();
            var collectionHelper = new B_Collection();
            var collection = await collectionHelper.ReadByName(keyword);
            if (collection == null) return new List<VMPartner>();


            var helper = new B_Account();
            var items = await helper.ReadByCollectionId(collection.Id, status);
            if (items == null || !items.Any())
                return new List<VMPartner>();
            var imgHelper = new B_Image();
            var imgs = await imgHelper.ReadByIds(items.Select(s => s.AvatarId).ToList());
            var telHelper = new B_TelePhone();
            var tels = await telHelper.ReadByIds(items.Select(s => s.TelePhoneId).ToList());
            var addHelper = new B_Address();
            var address = await addHelper.ReadByIds(items.Select(s => s.AddressId).ToList());
            var contentHelper = new B_Content();
            var contents = await contentHelper.ReadByIds(items.Select(s => s.contentId).ToList());

            using (var db = new KingAttornyContext())
            {
                try
                {

                    var dataItem = items
                                        .Select(s => new VMPartner()
                                        {
                                            id = s.Id,
                                            name = s.FirstName + " " + s.LastName,
                                            avatar_id = s.AvatarId,
                                            business = s.BusinessNamne,
                                            content_id = s.contentId,
                                            google_plus_url = s.GooplePlusUrl,
                                            facebook_fanpage_url = s.FacebookFanpageUrl,
                                            twitter_fanpage_url = s.TwitterFanpageUrl,
                                            zalo = s.Zalo,
                                            address_id = s.AddressId,
                                            telephone_id = s.TelePhoneId
                                        })
                                        .ToList();
                    if(dataItem!=null && dataItem.Any())
                    {
                        foreach(var i in dataItem)
                        {
                            var itemAdd = address.Where(s => s.Id == i.address_id).FirstOrDefault();
                            if (itemAdd != null)
                                i.address = itemAdd.AddressText;

                            var itemTele = tels.Where(s => s.Id == i.telephone_id).FirstOrDefault();
                            if (itemTele != null)
                                i.telephone = itemTele.PhoneNumber;

                            var itemImg = imgs.Where(s => s.Id == i.avatar_id).FirstOrDefault();
                            if (itemImg != null)
                                i.avatar_url = itemImg.RelativeUrl +"/" + itemImg.Name;

                            var itemContents = contents.Where(s => s.Id == i.content_id).FirstOrDefault();
                            if (itemContents != null)
                            {
                                i.content_detail = itemContents.Text;
                                i.description = itemContents.Description;
                            }
                        }
                    }
                    return dataItem;
                }
                catch(Exception ex)
                {
                    return new List<VMPartner>();
                }
            }
        }
    }
}
