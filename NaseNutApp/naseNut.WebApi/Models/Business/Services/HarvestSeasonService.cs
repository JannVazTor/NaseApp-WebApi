using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Business.Services;
using naseNut.WebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.Business.Services
{
    public class HarvestSeasonService : IService<HarvestSeason>
    {
        public bool Save(HarvestSeason harvestSeason)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var harvestSeasonRepository = new HarvestSeasonRepository(db);
                    harvestSeasonRepository.Insert(harvestSeason);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Delete(HarvestSeason harvestSeason)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var harvestSeasonRepository = new HarvestSeasonRepository(db);
                    harvestSeasonRepository.Delete(harvestSeason);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public HarvestSeason GetByName(string harvestSeasonName)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var harvestSeasonRepository = new HarvestSeasonRepository(db);
                    return harvestSeasonRepository.SearchOne(h => h.Name.ToLower() == harvestSeasonName.ToLower());
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<HarvestSeason> GetAll()
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var harvestSeasonRepository = new HarvestSeasonRepository(db);
                    var HarvestSeasons = harvestSeasonRepository.GetAll();
                    return HarvestSeasons;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public HarvestSeason GetById(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var harvestSeasonRepository = new HarvestSeasonRepository(db);
                    return harvestSeasonRepository.SearchOne(h => h.Id == id);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool ChangeState(int id, bool state)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var harvestSeasonRepository = new HarvestSeasonRepository(db);
                    var harvestSeasonActive = harvestSeasonRepository.Search(h => h.Active).ToList();
                        if (harvestSeasonActive.First().Id == id)
                        {
                            var harvestSeason = harvestSeasonRepository.GetById(id);
                            harvestSeason.Active = state;
                            db.HarvestSeasons.Attach(harvestSeason);
                            db.Entry(harvestSeason).Property(p => p.Active).IsModified = true;
                            return db.SaveChanges() >= 1;
                        }
                        else {
                            foreach (var h in harvestSeasonActive)
                            {
                                h.Active = false;
                                db.HarvestSeasons.Attach(h);
                                db.Entry(h).Property(p => p.Active).IsModified = true;
                            }
                            var modified = db.SaveChanges() >= 1;
                            if (!modified) return false;
                            var harvestSeason = harvestSeasonRepository.GetById(id);
                            harvestSeason.Active = state;
                            db.HarvestSeasons.Attach(harvestSeason);
                            db.Entry(harvestSeason).Property(p => p.Active).IsModified = true;
                            return db.SaveChanges() >= 1;
                        }
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}