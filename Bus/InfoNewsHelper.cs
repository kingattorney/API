using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using API.KingAttorney.ViewModels;
using System.Collections.Generic;
using WebAdmin_KingAnttorny.Bus;
using System.Net.Http;
using System.IO;
namespace API.KingAttorney.Bus
{
    public class InfoNewsHelper
    {
        public async Task<List<VMInfoNews>> GetNews(string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return new List<VMInfoNews>();
            var collectionHelper = new B_Collection();
            var collection  = await collectionHelper.ReadByName(keyword);
            if (collection == null) return new List<VMInfoNews>();
            var contentHelper = new B_Content();
            var contents = await contentHelper.Reads(collection.Id, 1);
            if (contents == null || !contents.Any()) return new List<VMInfoNews>();
            var imageHelper = new B_Image();
            var Images = await imageHelper.ReadByIds(contents.Select(s => s.ImageId).ToList());
            var dataItems = contents.Join(Images,
                                    a => a.ImageId,
                                    b => b.Id,
                                    (a, b) => new { a, b })
                                    .Select(s => new VMInfoNews()
                                    {
                                        id = s.a.Id,
                                        image_url = s.b.RelativeUrl + "/" + s.b.Name,
                                        name = s.a.Title,
                                        description = s.a.Description,
                                        content = s.a.Text,
                                        date = s.a.CreatedAt.Date.ToString("dd/MM/yyyy")
                                    })
                                    .ToList();
            if (dataItems == null || !dataItems.Any()) return new List<VMInfoNews>();
            return dataItems;


        }

        public async Task<List<VMInfoNews>> GetNews(string keyword, int numberRecords)
        {
            if (string.IsNullOrEmpty(keyword)) return new List<VMInfoNews>();
            var collectionHelper = new B_Collection();
            var collection = await collectionHelper.ReadByName(keyword);
            if (collection == null) return new List<VMInfoNews>();
            var contentHelper = new B_Content();
            var contents = await contentHelper.Reads(collection.Id, 1);
            if (contents == null || !contents.Any()) return new List<VMInfoNews>();
            var imageHelper = new B_Image();
            var Images = await imageHelper.ReadByIds(contents.Select(s => s.ImageId).ToList());
            var dataItems = contents.Join(Images,
                                    a => a.ImageId,
                                    b => b.Id,
                                    (a, b) => new { a, b })
                                    .Select(s => new VMInfoNews()
                                    {
                                        id = s.a.Id,
                                        image_url = s.b.RelativeUrl +"/" + s.b.Name,
                                        name = s.a.Title,
                                        description = s.a.Description,
                                        content = s.a.Text,
                                        date = s.a.CreatedAt.Date.ToString("dd/MM/yyyy"),
                                        
                                    })
                                    .Take(numberRecords)
                                    .ToList();

            if (dataItems == null || !dataItems.Any()) return new List<VMInfoNews>();
            return dataItems;


        }


        public async Task<List<VMInfoNews>> GetNews(string keyword, int size, int pageNumber)
        {
            if (string.IsNullOrEmpty(keyword)) return new List<VMInfoNews>();
            var collectionHelper = new B_Collection();
            var collection = await collectionHelper.ReadByName(keyword);
            if (collection == null) return new List<VMInfoNews>();
            var contentHelper = new B_Content();
            var contents = await contentHelper.Reads(collection.Id, 1, size, pageNumber);
            if (contents == null || !contents.Any()) return new List<VMInfoNews>();
            var imageHelper = new B_Image();
            var Images = await imageHelper.ReadByIds(contents.Select(s => s.ImageId).ToList());
            var dataItems = contents.Join(Images,
                                    a => a.ImageId,
                                    b => b.Id,
                                    (a, b) => new { a, b })
                                    .Select(s => new VMInfoNews()
                                    {
                                        id = s.a.Id,
                                        image_url = s.b.RelativeUrl +"/" + s.b.Name,
                                        name = s.a.Title,
                                        description = s.a.Description,
                                        content = s.a.Text,
                                        date = s.a.CreatedAt.Date.ToString("dd/MM/yyyy")
                                    })
                                    .ToList();
            if (dataItems == null || !dataItems.Any()) return new List<VMInfoNews>();
            return dataItems;


        }
    }
}
