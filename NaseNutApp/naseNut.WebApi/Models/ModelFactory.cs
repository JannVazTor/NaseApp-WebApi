using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Web.Http.Routing;
using naseNut.WebApi.Models.Entities;

namespace naseNut.WebApi.Models
{
    public class ModelFactory
    {
        private UrlHelper _urlHelper;
        private ApplicationUserManager _appUserManager;

        public ModelFactory(HttpRequestMessage request, ApplicationUserManager appUserManager)
        {
            _urlHelper = new UrlHelper(request);
            _appUserManager = appUserManager;
        }

        public List<ProducerModel> Create(List<Producer> producers)
        {
            return producers.Select(p => new ProducerModel
            {
                Id = p.Id,
                ProducerName = p.ProducerName
            }).ToList();
        }

        public List<RemissionModel> Create(List<Remission> remissions)
        {
            return remissions.Select(r => new RemissionModel
            {
                Id = r.Id,
                Cultivation = r.Cultivation,
                Batch = r.Batch,
                Quantity = r.Quantity,
                Butler = r.Butler,
                TransportNumber = r.TransportNumber,
                Driver = r.Driver,
                Elaborate = r.Elaborate,
                DateCapture = r.DateCapture,
                FieldName = r.Reception.FieldName,
                Variety = r.Reception.ReceptionEntry.Variety.Variety1
            }).ToList();
        }

        public List<HumidityModel> Create(List<Humidity> humidity)
        {
            return humidity.Select(h => new HumidityModel
            {
                Id = h.Id,
                HumidityPercent = h.HumidityPercent,
                DateCapture = h.DateCapture
            }).ToList();
        }

        public List<RoleModel> Create(List<AspNetRole> roles)
        {
            return roles.Select(r => new RoleModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();
        }

        public List<UserModel> Create(List<AspNetUser> users)
        {
            return users.Select(u => new UserModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Role = u.AspNetRoles.Count == 0 ? "" : u.AspNetRoles.First().Name
            }).ToList();
        }

        public List<CylinderModel> Create(List<Cylinder> cylinders)
        {
            return cylinders.Select(c => new CylinderModel
            {
                Id = c.Id,
                CylinderName = c.CylinderName
            }).ToList();
        }

        public List<GrillModel> Create(List<Grill> grills)
        {
            return grills.Select(g => new GrillModel
            {
                Id = g.Id,
                DateCapture = g.DateCapture,
                Receptions = g.Receptions != null && g.Receptions.Count != 0 ? string.Join(", ", g.Receptions.Select(r => r.Folio)) : "",
                Size = g.Size,
                Sacks = g.Sacks,
                Kilos = g.Kilos,
                Quality = g.Quality,
                Variety = g.Variety,
                Producer = g.Producer,
                FieldName = g.FieldName,
                Status = g.Status,
                Sampling = g.Samplings != null ? Create(g.Samplings.OrderBy(d => d.DateCapture).FirstOrDefault()) : null
            }).ToList();
        }

        public SamplingModel Create(Sampling sampling)
        {
            return new SamplingModel
            {
                Id = sampling.Id,
                DateCapture = sampling.DateCapture,
                SampleWeight = sampling.SampleWeight,
                HumidityPercent = sampling.HumidityPercent,
                WalnutNumber = sampling.WalnutNumber,
                Performance = sampling.Performance,
                TotalWeightOfEdibleNuts = sampling.TotalWeightOfEdibleNuts
            };
        }

        public List<SamplingModel> Create(List<Sampling> samplings)
        {
            return samplings.Select(s => new SamplingModel
            {
                Id = s.Id,
                DateCapture = s.DateCapture,
                SampleWeight = s.SampleWeight,
                HumidityPercent = s.HumidityPercent,
                WalnutNumber = s.WalnutNumber,
                Performance = s.Performance,
                TotalWeightOfEdibleNuts = s.TotalWeightOfEdibleNuts
            }).ToList();
        }

        public List<GrillIssueModel> Create(List<GrillIssue> issues)
        {
            return issues.Select(i => new GrillIssueModel
            {
                Id = i.Id,
                DateCapture = i.DateCapture,
                Truck = i.Truck,
                Driver = i.Driver,
                Box = i.Box,
                Remission = i.Remission,
                Grills = Create(i.Grills.Where(g => g.GrillIssuesId == i.Id).ToList())
            }).ToList();
        }

        public List<VarietyModel> Create(List<Variety> varieties)
        {
            return varieties.Select(v => new VarietyModel
            {
                Id = v.Id,
                VarietyName = v.Variety1
            }).ToList();
        }

        public List<ReceptionEntryModel> Create(List<ReceptionEntry> receptionEntries)
        {
            return receptionEntries.Select(r => new ReceptionEntryModel
            {
                Id = r.Id,
                Receptions = string.Join(", ", r.Receptions.Select(f => f.Folio)),
                DateEntry = r.DateEntry,
                Variety = r.Variety.Variety1,
                Producer = r.Producer.ProducerName
            }).ToList();
        }
        public class ReceptionEntryModel {
            public int Id { get; set; }
            public string Receptions { get; set; }
            public DateTime DateEntry { get; set; }
            public string Variety { get; set; }
            public string Producer { get; set; }
        }
        public class SelectionModel
        {
            public int Id { get; set; }
            public System.DateTime Date { get; set; }
            public int First { get; set; }
            public int Second { get; set; }
            public int Third { get; set; }
            public int Broken { get; set; }
            public int Germinated { get; set; }
            public int Vanas { get; set; }
            public int WithNut { get; set; }
            public double NutColor { get; set; }
            public double NutPerformance { get; set; }
            public double GerminationStart { get; set; }
            public double SampleWeight { get; set; }
            public double NutsNumber { get; set; }
            public double Humidity { get; set; } 
        }

        public class GrillIssueModel
        {
            public int Id { get; set; }
            public DateTime DateCapture { get; set; }
            public string Truck { get; set; }
            public string Driver { get; set; }
            public string Box { get; set; }
            public int Remission { get; set; }
            public List<GrillModel> Grills { get; set; }
        }

        public class SamplingModel
        {
            public int Id { get; set; }
            public DateTime DateCapture { get; set; }
            public double SampleWeight { get; set; }
            public double HumidityPercent { get; set; }
            public int WalnutNumber { get; set; }
            public double Performance { get; set; }
            public double TotalWeightOfEdibleNuts { get; set; }
        }

        public class GrillModel
        {
            public int Id { get; set; }
            public DateTime DateCapture { get; set; }
            public string Receptions { get; set; }
            public int Size { get; set; }
            public int Sacks { get; set; }
            public double Kilos { get; set; }
            public int Quality { get; set; }
            public string Variety { get; set; }
            public string Producer { get; set; }
            public string FieldName { get; set; }
            public bool Status { get; set; }
            public SamplingModel Sampling { get; set; }
        }

        public class CylinderModel
        {
            public int Id { get; set; }
            public string CylinderName { get; set; }
        }

        public class VarietyModel
        {
            public int Id { get; set; }
            public string VarietyName { get; set; }
        }

        public List<ReceptionModel> Create(List<Reception> receptions)
        {
            return receptions.Select(r => new ReceptionModel
            {
                Id =  r.Id,
                Folio = r.Folio,
                Variety = r.ReceptionEntry.Variety.Variety1,
                ReceivedFromField = r.ReceivedFromField,
                FieldName = r.FieldName,
                CarRegistration = r.CarRegistration,
                EntryDate = r.EntryDate,
                IssueDate = r.IssueDate,
                HeatHoursDrying = r.HeatHoursDtrying,
                HumidityPercent = r.HumidityPercent,
                Observations = r.Observations,
                ProducerName = r.ReceptionEntry.Producer.ProducerName,
                Grills = r.Grills != null && r.Grills.Count != 0 ? string.Join(", ", r.Grills.Select(g => g.Id)) : "",
                Cylinder = r.ReceptionEntry.Cylinder.CylinderName,
            }).ToList();
        }

        public List<HumiditiesModel> CreateC(List<Humidity> humidities)
        {
            return humidities.Select(r => new HumiditiesModel
            {
                Id = r.Id,
                HumidityPercentage = r.HumidityPercent,
                DateCapture = r.DateCapture,
                CylinderName = r.ReceptionEntry.Cylinder.CylinderName,
                Variety = r.ReceptionEntry.Variety.Variety1,
                ProducerName = r.ReceptionEntry.Producer.ProducerName,
                Tons = r.ReceptionEntry.Receptions.Sum(rec => rec.ReceivedFromField),
                EntryDate = r.ReceptionEntry.DateEntry,
                Folio = string.Join(", ", r.ReceptionEntry.Receptions.Select(re => re.Folio))  
            }).ToList();
        }

        public List<ReceptionIdModel> CreateReceptionId(List<Reception> receptions)
        {
            return receptions.Select(r => new ReceptionIdModel
            {
                ReceptionId = r.Id
            }).ToList();
        }

        public List<ProducerReportModel> CreateReport(List<Producer> producers) {
            return producers.Select(p => new ProducerReportModel
            {
                Id = p.Id,
                Folio = p.Receptions.Count != 0 ? string.Join(", ", p.Receptions.Select(c => c.Folio)) : "",
                Cylinder = p.Receptions.Count != 0 ? string.Join(", ", p.Receptions.Select(c => c.Cylinders.Select(x => x.CylinderName)).Distinct()) : "",
                DateReceptionCapture = p.Receptions.Count != 0 ? p.Receptions.First().EntryDate.ToString() : "",
                DateGrillCapture = p.Receptions.Select(r => r.Grills.First()).First().DateCapture.ToString(),
                KgsOrigen = p.Receptions.Count > 0 ? p.Receptions.SelectMany(x => x.Remissions.Select(g => g.Quantity)).Sum() : 0,
                KilosFirst = p.Receptions != null && p.Receptions.Count > 0 ? p.Receptions.SelectMany(x => x.Grills.Where(h => h.Size == 1).Select(g => g.Kilos)).Sum() : 0,
                KilosSecond = p.Receptions != null && p.Receptions.Count > 0 ? p.Receptions.SelectMany(x => x.Grills.Where(h => h.Size == 2).Select(g => g.Kilos)).Sum() : 0,
                SacksP = p.Receptions != null && p.Receptions.Count > 0 ? p.Receptions.SelectMany(x => x.Grills.Where(h => h.Size == 1).Select(g => g.Sacks)).Sum() : 0,
                SacksS = p.Receptions != null && p.Receptions.Count > 0 ? p.Receptions.SelectMany(x => x.Grills.Where(h => h.Size == 2).Select(g => g.Sacks)).Sum() : 0,
                KilosTotal = p.Receptions != null && p.Receptions.Count > 0 ? p.Receptions.SelectMany(x => x.Grills.Select(g => g.Kilos)).Sum() : 0,
                Variety = p.Receptions.Count != 0 ? string.Join(", ", p.Receptions.Select(c => c.Variety).Distinct()) : ""

            }).ToList();
        }

        //public List<reportProducerModel> CreateReport(List<Grill> grills)
        //{
           
        //    return grills.Select(r =>
        //    new reportProducerModel
        //    {
        //        Cilindro = r.Receptions.Count != 0 ? string.Join(", ", r.Receptions.Select(c => c.Cylinders.Select(x=>x.CylinderName)).Distinct()) : "",
        //        Folio = r.Receptions.Count != 0 ? string.Join(", ", r.Receptions.Select(c => c.Folio).Distinct()) : "",
        //        Variedad = r.Receptions.Count != 0 ? string.Join(", ", r.Receptions.Select(c => c.Variety).Distinct()) : "",
        //        Fecha = r.Receptions.Count != 0 ? (from u in r.Receptions select u.EntryDate).ToString() : "" ,
        //        FechaProceso = r.DateCapture.ToString() != "" ? r.DateCapture.ToString() : "" ,
        //        Remisión = r.Receptions != null && r.Receptions.Count > 0 ? (from u in r.Receptions select u.Remissions.First().Id).ToString() : "",
        //        SacosPrimera = r.Receptions != null && r.Receptions.Count > 0 ? r.Receptions.SelectMany(x => x.Grills.Where(h=> h.Size == 1).Select(g => g.Sacks)).Sum() : 0,
        //        SacosSegunda = r.Receptions != null && r.Receptions.Count > 0 ? r.Receptions.SelectMany(x => x.Grills.Where(h => h.Size == 2).Select(g => g.Sacks)).Sum() : 0,
        //        KgsOrigen = r.Receptions.Count > 0 ? r.Receptions.SelectMany(x => x.Remissions.Select(g => g.Quantity)).Sum() : 0,
        //        KilosPrimera =  r.Receptions != null && r.Receptions.Count > 0 ? r.Receptions.SelectMany(x => x.Grills.Where(h => h.Size == 1).Select(g => g.Kilos)).Sum() : 0,
        //        KilosSegunda =  r.Receptions != null && r.Receptions.Count > 0 ? r.Receptions.SelectMany(x => x.Grills.Where(h => h.Size == 2).Select(g => g.Kilos)).Sum() : 0,
        //        KilosT  = r.Receptions != null && r.Receptions.Count > 0 ? r.Receptions.SelectMany(x => x.Grills.Where(h => h.Size < 3).Select(g => g.Kilos)).Sum() : 0

        //    }).ToList();
        //}

        public class ReceptionModel
        {
            public int Id { get; set; }
            public int Folio { get; set; }
            public string Variety { get; set; }
            public double ReceivedFromField { get; set; }
            public string FieldName { get; set; }
            public string CarRegistration { get; set; }
            public DateTime? EntryDate { get; set; }
            public DateTime? IssueDate { get; set; }
            public double? HeatHoursDrying { get; set; }
            public double? HumidityPercent { get; set; }
            public string Observations { get; set; }
            public string ProducerName { get; set; }
            public string Grills { get; set; }
            public string Cylinder { get; set; }
        }

        public class CompleteHumidityModel
        {
            public IEnumerable<int> HumidityId { get; set; }
            public string CylinderName { get; set; }
            public string Variety { get; set; }
            public string ProducerName { get; set; }
            public double Tons { get; set; }
            public DateTime EntryDate { get; set; }
            public int ReceptionId { get; set; }
            public List<IEnumerable<Double>>HumidityPercent { get; set; }
        }

        public class ReceptionIdModel
        {
            public int ReceptionId { get; set; }
        }

        public class UserModel
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Role { get; set; }
        }

        public class RoleModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class HumidityModel
        {
            public int Id { get; set; }
            public double HumidityPercent { get; set; }
            public DateTime DateCapture { get; set; }
        }

        public class RemissionModel
        {
            public int Id { get; set; }
            public string Cultivation { get; set; }
            public string Batch { get; set; }
            public double Quantity { get; set; }
            public string Butler { get; set; }
            public int TransportNumber { get; set; }
            public string Driver { get; set; }
            public string Elaborate { get; set; }
            public DateTime DateCapture { get; set; }
            public string FieldName { get; set; }
            public string Variety { get; set; }
        }

        public class ProducerModel 
        {
            public int Id { get; set; }
            public string ProducerName { get; set; }
        }

        public class ProducerReportModel
        {
            
            public int Id { get; set; }
            public string Folio { get; set; }
            public string Variety { get; set; }
            public string Remission { get; set; }
            public string Cylinder { get; set; }
            public string DateReceptionCapture { get; set; }
            public string DateGrillCapture { get; set; }
            public double KgsOrigen { get; set; }
            public int SacksP { get; set; } = 0;
            public int SacksS { get; set; } = 0;
            public double KilosFirst { get; set; } = 0;
            public double KilosSecond { get; set; } = 0;
            public double KilosTotal { get; set; }
        }
    }
}