using naseNut.WebApi.Models.Business.Repositories;
using naseNut.WebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.Business.Services
{
    public class GrillIssueService
    {
        public bool Save(GrillIssue grillIssue, List<int> grillsId)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var grillService = new GrillService();
                    var grillIssueRepository = new GrillIssueRepository(db);
                    grillIssueRepository.Insert(grillIssue);
                    db.SaveChanges();
                    var grills = db.Grills.Where(g => grillsId.Any(id => id == g.Id)).ToList();
                    grillIssueRepository.Update(grillIssue);
                    grills.ForEach(g => grillIssue.Grills.Add(g));
                    db.SaveChanges();
                    grills.ForEach(g => grillService.UpdateStatus(g.Id, false));
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var grillService = new GrillService();
                    var grillIssueRepository = new GrillIssueRepository(db);
                    var grillIssue = db.GrillIssues.First(g => g.Id == id);
                    grillIssue.Grills.Where(g => g.GrillIssue.Id == grillIssue.Id).ToList().ForEach(gr => grillService.UpdateStatus(gr.Id, true));
                    grillIssue.Grills.Where(g => g.GrillIssue.Id == grillIssue.Id).ToList().ForEach(gr => grillIssue.Grills.Remove(gr));
                    grillIssueRepository.Delete(grillIssue);
                    return db.SaveChanges() >= 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GrillIssue GetById(int id) {
            try
            {
                using (var db = new NaseNEntities())
                {
                    var grillIssueRepository = new GrillIssueRepository(db);
                    return grillIssueRepository.GetById(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}