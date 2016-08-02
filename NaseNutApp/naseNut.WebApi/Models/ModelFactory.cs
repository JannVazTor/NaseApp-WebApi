﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Web.Http.Routing;
using naseNut.WebApi.Models.Entities;
using naseNut.WebApi.Models.Enum;
using naseNut.WebApi.Models.Business.Services;

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
                Variety = g.Variety.Variety1,
                Producer = g.Producer.ProducerName,
                FieldName = g.FieldName,
                Status = g.Status,
                SampleWeight = g.Samplings.ToList().Count != 0 ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().SampleWeight.ToString() : "",
                HumidityPercent = g.Samplings.ToList().Count != 0 ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().HumidityPercent.ToString(): "",
                WalnutNumber = g.Samplings.ToList().Count != 0 ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().WalnutNumber.ToString():"",
                Performance = g.Samplings.ToList().Count != 0 ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().Performance.ToString() : "",
                TotalWeightOfEdibleNuts = g.Samplings.ToList().Count != 0 ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().TotalWeightOfEdibleNuts.ToString() : ""
            }).ToList();
        }

        public SamplingGrillModel Create(Sampling sampling)
        {
            return new SamplingGrillModel
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

        public List<SamplingGrillModel> CreateSampling(List<Sampling> samplings)
        {
            return samplings.Select(s => new SamplingGrillModel
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
                VarietyName = v.Variety1,
                Small = v.NutSize.Small,
                MediumStart = v.NutSize.MediumStart,
                MediumEnd = v.NutSize.MediumEnd,
                LargeStart = v.NutSize.LargeStart,
                LargeEnd = v.NutSize.LargeEnd
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
                Producer = r.Producer.ProducerName,
                Cylinder = r.Cylinder.CylinderName
            }).ToList();
        }

        public List<SamplingReceptionModel> CreateReception(List<Sampling> samplings)
        {
            return samplings.Select(s => new SamplingReceptionModel
            {
                Id = s.Id,
                DateCapture = s.DateCapture,
                SampleWeight = s.SampleWeight,
                HumidityPercent = s.HumidityPercent,
                WalnutNumber = s.WalnutNumber,
                Performance = s.Performance,
                TotalWeightOfEdibleNuts = s.TotalWeightOfEdibleNuts,
                SacksFirst = s.ReceptionEntry.NutTypes.Any(n => n.NutType1 == 1) ? s.ReceptionEntry.NutTypes.Where(n => n.NutType1 == 1).First().Sacks.ToString() : "",
                KilosFirst = s.ReceptionEntry.NutTypes.Any(n => n.NutType1 == 1) ? s.ReceptionEntry.NutTypes.Where(n => n.NutType1 == 1).First().Kilos.ToString() : "",
                SacksSecond = s.ReceptionEntry.NutTypes.Any(n => n.NutType1 == 2) ? s.ReceptionEntry.NutTypes.Where(n => n.NutType1 == 2).First().Sacks.ToString() : "",
                KilosSecond = s.ReceptionEntry.NutTypes.Any(n => n.NutType1 == 2) ? s.ReceptionEntry.NutTypes.Where(n => n.NutType1 == 2).First().Kilos.ToString() : "",
                SacksThird = s.ReceptionEntry.NutTypes.Any(n => n.NutType1== 3) ? s.ReceptionEntry.NutTypes.Where(n => n.NutType1 == 3).First().Sacks.ToString() : "",
                KilosThird = s.ReceptionEntry.NutTypes.Any(n => n.NutType1 == 3) ? s.ReceptionEntry.NutTypes.Where(n => n.NutType1 == 3).First().Kilos.ToString() : "",
            }).ToList();
        }
        public List<NutTypeModel> Create(List<NutType> nutTypes) {
            return nutTypes.Select(n => new NutTypeModel
            {
                Id = n.Id,
                NutType = n.NutType1,
                Sacks = (int)n.Sacks,
                Kilos = (int)n.Kilos
            }).ToList();
        }
        public List<ReportingProcessModel> CreateReport(List<Variety> varieties)
        {
            var reportService = new ReportService();
            return (from v in varieties
                   let sacksFirstSmall = reportService.GetSacks(v.Grills.ToList(), NutSizes.Small, (int)GrillQuality.First)
                   let sacksFirstMedium = reportService.GetSacks(v.Grills.ToList(), NutSizes.Medium, (int)GrillQuality.Second)
                   let sacksFirstLarge = reportService.GetSacks(v.Grills.ToList(), NutSizes.Large, (int)GrillQuality.Third)
                   let kilogramsFirstSmall = reportService.GetKilograms(v.Grills.ToList(), NutSizes.Small, (int)GrillQuality.First)
                   let kilogramsFirstMedium = reportService.GetKilograms(v.Grills.ToList(), NutSizes.Medium, (int)GrillQuality.First)
                   let kilogramsFirstLarge = reportService.GetKilograms(v.Grills.ToList(), NutSizes.Large, (int)GrillQuality.First)
                   let totalkilogramsSecond = reportService.GetKilograms(v.Grills.ToList(), null, (int)GrillQuality.Second)
                   let totalkilogramsThird = reportService.GetKilograms(v.Grills.ToList(), null, (int)GrillQuality.Third) 
                   let totalKilogramsFirst = reportService.GetKilograms(v.Grills.ToList(), null, (int)GrillQuality.First)
                   let totalKilos = totalKilogramsFirst + totalkilogramsSecond + totalkilogramsThird
                   let variety = v.Variety1
                    select new ReportingProcessModel
                   {
                       SacksFirstSmall = sacksFirstSmall,
                       SacksFirstMedium = sacksFirstMedium,
                       SacksFirstLarge = sacksFirstLarge,
                       KilogramsFirstSmall = kilogramsFirstSmall,
                       KilogramsFirstMedium = kilogramsFirstMedium,
                       KilogramsFirstLarge = kilogramsFirstLarge,
                       TotalKilogramsSecond = totalkilogramsSecond,
                       TotalKilogramsThird = totalkilogramsThird,
                       TotalKilogramsFirst = totalKilogramsFirst,
                       TotalKilos = totalKilos,
                       PercentageFirst = (totalKilogramsFirst == 0 || totalKilos == 0) ? "0%" : ((totalKilogramsFirst / totalKilos) * 100).ToString() + "%",
                       PercentageSecond = (totalkilogramsSecond == 0 || totalKilos == 0) ? "0%" : ((totalkilogramsSecond / totalKilos) * 100).ToString() + "%",
                       PercentageThird = (totalkilogramsThird == 0 || totalKilos == 0) ? "0%" : ((totalkilogramsThird / totalKilos) * 100).ToString() + "%",
                       Variety = variety
                    }).ToList();
        }

        public List<DailyProcessModel> CreateReport(List<Grill> grills, List<Reception> receptions, List<Sampling> samplings, DateTime date)
        {
            var reportService = new ReportService();
            var datedGrills = from grill in grills
                where grill.GrillIssue.DateCapture == date
                select grill;
            return (from r in samplings  where r.ReceptionEntry.DateIssue == date
                    let sacksFirstSmall = reportService.GetSacks(datedGrills.ToList(), NutSizes.Small, (int)GrillQuality.First)
                    let sacksFirstMedium = reportService.GetSacks(datedGrills.ToList(), NutSizes.Medium, (int)GrillQuality.First)
                    let folio = reportService.GetFolio(receptions, r.Grill.ReceptionId)
                    select new DailyProcessModel
                    {
                        Date = date,
                        Producer = r.ReceptionEntry.Producer.ToString(),
                        Folio = folio,
                        Cylinder = r.ReceptionEntry.Cylinder.ToString(),
                        Variety = r.ReceptionEntry.Variety.ToString(),
                        SacksFirstSmall = sacksFirstSmall,
                        SacksFirstMedium = sacksFirstMedium,
                        Total = sacksFirstSmall + sacksFirstMedium,
                        QualityPercent = 50 + '%',
                        Germinated = 100
                    }).ToList();
        }

        public class DailyProcessModel
        {
            public DateTime Date { get; set; }
            public string Producer { get; set; }
            public Reception Folio { get; set; }
            public string Cylinder { get; set; }
            public string Variety { get; set; }
            public int SacksFirstSmall { get; set; }
            public int SacksFirstMedium { get; set; }
            public int Total { get; set; }
            public double QualityPercent { get; set; }
            public int Germinated { get; set; }
        }
        public class ReportingProcessModel
        {
            public string Variety { get; set; }
            public int SacksFirstSmall { get; set; }
            public int SacksFirstMedium { get; set; }
            public int SacksFirstLarge { get; set; }
            public double KilogramsFirstSmall { get; set; }
            public double KilogramsFirstMedium { get; set; }
            public double KilogramsFirstLarge { get; set; }
            public double TotalKilogramsFirst { get; set; }
            public double TotalKilogramsSecond { get; set; }
            public double TotalKilogramsThird { get; set; }
            public double TotalKilos { get; set; }
            public string PercentageFirst { get; set; }
            public string PercentageSecond { get; set; }
            public string PercentageThird { get; set; }
        }
        public class NutTypeModel
        {
            public int Id { get; set; }
            public byte NutType { get; set; }
            public int Sacks { get; set; }
            public double Kilos { get; set; }
        }
        public class SamplingReceptionModel
        {
            public int Id { get; set; }
            public DateTime DateCapture { get; set; }
            public double SampleWeight { get; set; }
            public double HumidityPercent { get; set; }
            public int WalnutNumber { get; set; }
            public double Performance { get; set; }
            public double TotalWeightOfEdibleNuts { get; set; }
            public string SacksFirst{ get; set; }
            public string KilosFirst{ get; set; }
            public string SacksSecond{ get; set; }
            public string KilosSecond { get; set; }
            public string SacksThird { get; set; }
            public string KilosThird { get; set; }
        }
        public class ReceptionEntryModel
        {
            public int Id { get; set; }
            public string Receptions { get; set; }
            public DateTime DateEntry { get; set; }
            public string Variety { get; set; }
            public string Producer { get; set; }
            public string Cylinder { get; set; }
            public List<ReceptionModel> ReceptionList { get; set; }
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

        public class SamplingGrillModel
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
            public int MyProperty { get; set; }
            public string SampleWeight { get; internal set; }
            public string HumidityPercent { get; internal set; }
            public string WalnutNumber { get; internal set; }
            public string Performance { get; internal set; }
            public string TotalWeightOfEdibleNuts { get; internal set; }
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
            public int Small { get; set; }
            public int MediumStart { get; set; }
            public int MediumEnd { get; set; }
            public int LargeStart { get; set; }
            public int LargeEnd { get; set; }
        }

        public List<ReceptionModel> Create(List<Reception> receptions)
        {
            return receptions.Select(r => new ReceptionModel
            {
                Id = r.Id,
                Folio = r.Folio,
                Variety = r.ReceptionEntry.Variety.Variety1,
                ReceivedFromField = r.ReceivedFromField,
                FieldName = r.FieldName,
                CarRegistration = r.CarRegistration,
                EntryDate = r.ReceptionEntry.DateEntry,
                IssueDate = r.ReceptionEntry.DateIssue,
                HeatHoursDrying = r.HeatHoursDtrying,
                HumidityPercent = r.HumidityPercent,
                Observations = r.Observations,
                ProducerName = r.ReceptionEntry.Producer.ProducerName,
                Grills = r.Grills != null && r.Grills.Count != 0 ? string.Join(", ", r.Grills.Select(g => g.Id)) : "",
                Cylinder = r.ReceptionEntry.Cylinder.CylinderName
            }).ToList();
        }

        public HumiditiesModel Create(ReceptionEntry receptionEntry)
        {
            return new HumiditiesModel
            {
                Humidities = Create(receptionEntry.Humidities.ToList()),
                CylinderName = receptionEntry.Cylinder.CylinderName,
                FieldName = string.Join(", ", receptionEntry.Receptions.Select(re => re.FieldName)),
                Tons = receptionEntry.Receptions.Sum(rec => rec.ReceivedFromField),
                EntryDate = receptionEntry.DateEntry,
                Folio = string.Join(", ", receptionEntry.Receptions.Select(re => re.Folio))
            };
        }
        public List<HumidityModel> Create(List<Humidity> humidities) {
            return humidities.Select(h => new HumidityModel
            {
                Id = h.Id,
                DateCapture = h.DateCapture,
                HumidityPercentage = h.HumidityPercent
            }).ToList();
        }
        public List<HumiditiesManageModel> CreateH(List<Humidity> humidities) {
            return humidities.Select(h => new HumiditiesManageModel
            {
                Id = h.Id,
                HumidityPercentage = h.HumidityPercent,
                DateCapture = h.DateCapture,
                CylinderName = h.ReceptionEntry.Cylinder.CylinderName,
                EntryDate = h.ReceptionEntry.DateEntry,
                FieldName = string.Join(", ", h.ReceptionEntry.Receptions.Select(g => g.FieldName)),
                Folio = string.Join(", ", h.ReceptionEntry.Receptions.Select(g => g.Folio)),
                Tons = h.ReceptionEntry.Receptions.Sum(r => r.ReceivedFromField)
            }).ToList();
        }
        public List<ReceptionEntryModel> CreateReceptionId(List<ReceptionEntry> receptionEntries)
        {
            return receptionEntries.Select(r => new ReceptionEntryModel
            {
                Id = r.Id,
                DateEntry = r.DateEntry,
                Producer = r.Producer.ProducerName,
                Variety = r.Variety.Variety1,
                ReceptionList = Create(r.Receptions.ToList())
            }).ToList();
        }
        public ReceptionModel Create(Reception reception)
        {
            return new ReceptionModel
            {
                Id = reception.Id,
                Folio = reception.Folio,
                Variety = reception.ReceptionEntry.Variety.Variety1,
                ReceivedFromField = reception.ReceivedFromField,
                FieldName = reception.FieldName,
                CarRegistration = reception.CarRegistration,
                EntryDate = reception.ReceptionEntry.DateEntry,
                IssueDate = reception.ReceptionEntry.DateIssue,
                HeatHoursDrying = reception.HeatHoursDtrying,
                HumidityPercent = reception.HumidityPercent,
                Observations = reception.Observations,
                ProducerName = reception.ReceptionEntry.Producer.ProducerName,
                Grills = reception.Grills != null && reception.Grills.Count != 0 ? string.Join(", ", reception.Grills.Select(g => g.Id)) : "",
                Cylinder = reception.ReceptionEntry.Cylinder.CylinderName
            };
        }
        public class HumiditiesModel
        {
            public string CylinderName { get; set; }
            public string FieldName { get; set; }
            public double Tons { get; set; }
            public DateTime EntryDate { get; set; }
            public string Folio { get; set; }
            public List<HumidityModel> Humidities { get; set; }
        }
        public class HumiditiesManageModel
        {
            public string CylinderName { get; set; }
            public string FieldName { get; set; }
            public double Tons { get; set; }
            public DateTime EntryDate { get; set; }
            public string Folio { get; set; }
            public int Id { get; set; }
            public DateTime DateCapture { get; set; }
            public double HumidityPercentage { get; set; }
        }
        public class HumidityModel
        {
            public int Id { get; set; }
            public DateTime DateCapture{ get; set; }
            public double HumidityPercentage { get; set; }
        }
        public List<ProducerReportModel> CreateReport(List<ReceptionEntry> receptionEntries)
        {
           Random num = new Random();

           return receptionEntries.Select(r => new ProducerReportModel
            {
               //DateReceptionCapture = r.DateEntry,
               //Variety = r.Variety.Variety1,
               //FieldName = r.Receptions.Count != 0 ? string.Join(", ", r.Receptions.Select(g => g.FieldName)) : "",
               //Cylinder = r.Cylinder.CylinderName,
               //Folio = r.Receptions.ToList().Count != 0 ? string.Join(", ", r.Receptions.Select(c => c.Folio)) : "",
               //DateReceptionCapture = r.Receptions.ToList().Count != 0 ? r.Receptions.First().EntryDate.ToString() : "",
               //ProcessDate = r.Receptions.Select(x => x.EntryDate).First().ToString(),
               //KgsOrigen = r.Receptions.ToList().Count != 0 ? r.Receptions.Select(x => x.ReceivedFromField).Sum() : 0,
               //KilosFirst = r.NutTypes.Count != 0 ? r.Receptions.SelectMany(x => x.ReceptionEntry.NutTypes.Where(n => n.NutType1 == 1).Select(y => y.Sacks * y.Kilos)).Sum() : 0,
               //KilosSecond = r.NutTypes.Count != 0 ? r.Receptions.SelectMany(x => x.ReceptionEntry.NutTypes.Where(n => n.NutType1 == 2).Select(y => y.Sacks * y.Kilos)).Sum() : 0,
               //SacksP = r.NutTypes.Count != 0 ? r.Receptions.SelectMany(x => x.ReceptionEntry.NutTypes.Where(n => n.NutType1 == 1).Select(y => y.Sacks)).Sum() : 0,
               //SacksS = r.NutTypes.Count != 0 ? r.Receptions.SelectMany(x => x.ReceptionEntry.NutTypes.Where(n => n.NutType1 == 2).Select(y => y.Sacks)).Sum() : 0,
               //KilosTotal = r.Receptions.ToList().Count != 0 ? r.Receptions.SelectMany(x => x.ReceptionEntry.NutTypes.Select(g => g.Kilos * g.Sacks)).Sum() : 0,
               //Variety = r.Variety.Variety1,
               //Remission = DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + num.Next(99999).ToString()
           }).ToList();
        }

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
                public List<IEnumerable<Double>> HumidityPercent { get; set; }
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
            public string FieldName { get; set; }
            public DateTime DateReceptionCapture { get; set; }
                public string ProcessDate { get; set; }
                public double KgsOrigen { get; set; }
                public int? SacksP { get; set; } = 0;
                public int? SacksS { get; set; } = 0;
                public double? KilosFirst { get; set; } = 0;
                public double? KilosSecond { get; set; } = 0;
                public double? KilosTotal { get; set; }
            }
        }
    }