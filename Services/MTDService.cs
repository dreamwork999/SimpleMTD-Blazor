using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using SimplyMTD.Data;

namespace SimplyMTD
{
    public partial class MTDService
    {
        MTDContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly MTDContext context;
        private readonly NavigationManager navigationManager;

        public MTDService(MTDContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);


        public async Task ExportPlaningsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/mtd/planings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/mtd/planings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPlaningsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/mtd/planings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/mtd/planings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPlaningsRead(ref IQueryable<SimplyMTD.Models.MTD.Planing> items);

        public async Task<IQueryable<SimplyMTD.Models.MTD.Planing>> GetPlanings(Query query = null)
        {
            var items = Context.Planings.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnPlaningsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPlaningGet(SimplyMTD.Models.MTD.Planing item);

        public async Task<SimplyMTD.Models.MTD.Planing> GetPlaningById(string id)
        {
            var items = Context.Planings
                              .AsNoTracking()
                              .Where(i => i.id == id);

  
            var itemToReturn = items.FirstOrDefault();

            OnPlaningGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnPlaningCreated(SimplyMTD.Models.MTD.Planing item);
        partial void OnAfterPlaningCreated(SimplyMTD.Models.MTD.Planing item);

        public async Task<SimplyMTD.Models.MTD.Planing> CreatePlaning(SimplyMTD.Models.MTD.Planing planing)
        {
            OnPlaningCreated(planing);

            var existingItem = Context.Planings
                              .Where(i => i.id == planing.id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Planings.Add(planing);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(planing).State = EntityState.Detached;
                throw;
            }

            OnAfterPlaningCreated(planing);

            return planing;
        }

        public async Task<SimplyMTD.Models.MTD.Planing> CancelPlaningChanges(SimplyMTD.Models.MTD.Planing item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnPlaningUpdated(SimplyMTD.Models.MTD.Planing item);
        partial void OnAfterPlaningUpdated(SimplyMTD.Models.MTD.Planing item);

        public async Task<SimplyMTD.Models.MTD.Planing> UpdatePlaning(string id, SimplyMTD.Models.MTD.Planing planing)
        {
            OnPlaningUpdated(planing);

            var itemToUpdate = Context.Planings
                              .Where(i => i.id == planing.id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(planing);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterPlaningUpdated(planing);

            return planing;
        }

        partial void OnPlaningDeleted(SimplyMTD.Models.MTD.Planing item);
        partial void OnAfterPlaningDeleted(SimplyMTD.Models.MTD.Planing item);

        public async Task<SimplyMTD.Models.MTD.Planing> DeletePlaning(string id)
        {
            var itemToDelete = Context.Planings
                              .Where(i => i.id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnPlaningDeleted(itemToDelete);


            Context.Planings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPlaningDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}