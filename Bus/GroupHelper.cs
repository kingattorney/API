using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using WebAdmin_KingAnttorny.Models;
using Microsoft.EntityFrameworkCore;
using API.KingAttorney.ViewModels;
namespace API.KingAttorney.Bus
{
    public class GroupHelper
    {
       
        public async Task<VMGroupTitle> Reads(string name, int status)
        {
            using (var db = new KingAttornyContext())
            {
                var datareturn = new VMGroupTitle() { title = "No title", data_items = new List<VMGroup>() };

                int partentId = 0;

                if (string.IsNullOrEmpty(name)) return datareturn;
                var itemgroup = db.Groups.Where(s => s.Name == name).SingleOrDefault();

                if (itemgroup == null) return datareturn;
                datareturn.title = itemgroup.Description;
                partentId = itemgroup.Id;

                if (status < 0)
                {

                    var items = await db.Groups
                                .Where(s => s.ParentId == partentId)
                                .OrderBy(o => o.Order)
                                .Select(s => new VMGroup()
                                {
                                    id = s.Id,
                                    name = s.Description,
                                    order = s.Order

                                })
                                .ToListAsync();
                    if (items == null || !items.Any()) return datareturn;
                    datareturn.data_items = items;
                    return datareturn;
                }
                else
                {
                    var items = await db.Groups
                                    .Where(s => s.Status == status && s.ParentId == partentId)
                                    .OrderBy(o => o.Order)
                                     .Select(s => new VMGroup()
                                     {
                                         id = s.Id,
                                         name = s.Description,
                                         order = s.Order

                                     })
                                    .ToListAsync();
                    if (items == null || !items.Any()) return datareturn;
                    datareturn.data_items = items;
                    return datareturn;
                }
            }
        }

        public async Task<VMGroupTitle> ReadSubItem(int groupId, int status, int level)
        {
            
            using (var db = new KingAttornyContext())
            {
                var dataReturn = new VMGroupTitle() { title = "No title", data_items = new List<VMGroup>() };
                
              
                if (level == 1)
                {
                    var groupItem = db.Groups.Where(s => s.Id == groupId).SingleOrDefault();
                    if (groupItem == null) return dataReturn;

                    dataReturn.title = groupItem.Description;

                    var dataItemCheck = await db.GroupCollections
                                               .Where(s => s.GroupId == groupId && s.Status == status)
                                               .OrderBy(o => o.Order)
                                               .ToListAsync();

                   

                    if (dataItemCheck == null || !dataItemCheck.Any()) return dataReturn;

                    if (dataItemCheck.Any(s => s.Type == "content"))
                    {
                        var dataItems = dataItemCheck
                                            .Join(db.Collections,
                                            a => a.CollectionId,
                                            b => b.Id,
                                            (a, b) => new { a, b })
                                            .Join(db.Contents,
                                            c => c.b.Id,
                                            d => d.CollectionId,
                                            (c, d) => new { c, d })
                                            .Select(s => new VMGroup()
                                            {
                                                id = s.d.Id,
                                                type = s.c.a.Type,
                                                name = s.d.Title,
                                                order = s.d.Order
                                            })
                                            .ToList();

                        dataReturn.data_items = dataItems;

                        return dataReturn;
                    }

                    if (dataItemCheck.Any(s => s.Type == "index"))
                    {

                        var dataItems = dataItemCheck
                                            .Join(db.Collections,
                                            a => a.CollectionId,
                                            b => b.Id,
                                            (a, b) => new { a, b })
                                            .Where(s => s.b.Status == status)
                                            .Select(s => new VMGroup()
                                            {
                                                id = s.b.Id,
                                                type = s.a.Type,
                                                name = s.b.Description,
                                                order = s.a.Order
                                            })
                                            .ToList();
                        dataReturn.data_items = dataItems;
                        return dataReturn;
                    }
                }

                if(level ==2)
                {

                   
                    var dataCollection = db.Collections.Where(s => s.Id == groupId).SingleOrDefault();
                    if (dataCollection == null) return dataReturn;
                    dataReturn.title = dataCollection.Description;

                    var dataItems = db.Contents
                                      .Where(s => s.CollectionId == dataCollection.Id)
                                      .OrderBy(o=>o.Order)
                                      .Select(s => new VMGroup()
                                      {
                                          id = s.Id,
                                          type = "content",
                                          name = s.Title,
                                          order = s.Order
                                      })
                                    .ToList();
                    if(dataItems!=null && dataItems.Any())
                        dataReturn.data_items = dataItems;                
                   
                    return dataReturn;
                }

                return dataReturn;
            }
        }


    }

    
}
