using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Web.Http.Routing;
using naseNut.WebApi.Models.BindingModels;
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

        public PieChartModel[] CreateDash(List<Variety> varieties)
        {
            return varieties.Select(v => new PieChartModel
            {
                name = v.Variety1,
                y = v.Grills.Where(g => g.Variety.Id == v.Id).Sum(g => g.Kilos)
            }).ToList().ToArray();
        }
        public class PieChartModel
        {
            public string name { get; set; }
            public double y { get; set; }
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
                Quantity = r.Quantity,
                Butler = r.Butler,
                TransportNumber = r.TransportNumber,
                Driver = r.Driver,
                Elaborate = r.Elaborate,
                DateCapture = r.DateCapture,
                FieldName = r.Field.FieldName,
                Batch = r.Batch.Batch1,
                Box = r.Box.Box1,
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
                CylinderName = c.CylinderName,
                State = c.Active
            }).ToList();
        }

        public List<GrillModel> Create(List<Grill> grills)
        {
            return grills.Select(g => new GrillModel
            {
                Id = g.Id,
                DateCapture = g.DateCapture,
                Receptions = g.Receptions != null && g.Receptions.Count != 0 ? string.Join(", ", g.Receptions.Select(r => r.Folio)) : "",
                Size = g.Size == 1 ? "Grande" : (g.Size == 2 ? "Mediana" : "Chica"),
                Sacks = g.Sacks,
                Kilos = g.Kilos,
                Quality = g.Quality == 1 ? "Primera" : (g.Quality == 2 ? "Segunda" : "Tercera") ,
                Variety = g.Variety.Variety1,
                Producer = g.Producer.ProducerName,
                FieldName = g.Field.FieldName,
                Status = g.Status,
                SampleWeight = g.Samplings.ToList().Count != 0 ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().SampleWeight.ToString() : "",
                HumidityPercent = g.Samplings.ToList().Count != 0 ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().HumidityPercent.ToString(): "",
                WalnutNumber = g.Samplings.ToList().Count != 0 ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().WalnutNumber.ToString():"",
                Performance = g.Samplings.ToList().Count != 0 ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().Performance.ToString() : "",
                TotalWeightOfEdibleNuts = g.Samplings.ToList().Count != 0 ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().TotalWeightOfEdibleNuts.ToString() : ""
            }).ToList();
        }

        public List<DailyProcessModel> CreateReport(List<ReceptionEntry> receptionEntries, string date)
        {
            var reportService = new ReportService();
            return (from s in receptionEntries
                    let grill = s.Variety.Grills.ToList()
                    let sacksFirstMedium = reportService.GetSacks(grill, NutSizes.Medium, (int)GrillQuality.First)
                    let sacksFirstSmall =  reportService.GetSacks(grill, NutSizes.Small, (int)GrillQuality.First)
                    select new DailyProcessModel
                    {
                        Date = date,
                        Producer = s.Producer.ProducerName,
                        Folio = s.Receptions.ToList().Count != 0 ? string.Join(", ", s.Receptions.Select(c => c.Folio)) : "",
                        Cylinder = s.Cylinder.CylinderName,
                        Variety = s.Variety.Variety1,
                        SacksFirstSmall = sacksFirstSmall,
                        SacksFirstMedium = sacksFirstMedium,
                        Total = sacksFirstSmall + sacksFirstMedium,
                        QualityPercent = s.Samplings.Count != 0 ? s.Samplings.OrderBy(x=> x.DateCapture).FirstOrDefault().Performance.ToString(CultureInfo.InvariantCulture) + "%" : "",
                        Germinated = s.Samplings.SelectMany(s => s.NutTypes).Any() ? s.Receptions.SelectMany(x=> x.ReceptionEntry.Samplings.SelectMany(n => n.NutTypes).Where(n=> n.NutType1 == 2).Select(y=> y.Sacks)).Sum() : 0,
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
                GrillId = s.Grill.Id,
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
                Folio = s.ReceptionEntry.Receptions.Any() ? string.Join(", ", s.ReceptionEntry.Receptions.Select(c => c.Folio)) : "",
                DateCapture = s.DateCapture,
                SampleWeight = s.SampleWeight,
                HumidityPercent = s.HumidityPercent,
                WalnutNumber = s.WalnutNumber,
                Performance = s.Performance.RoundTwoDigitsDouble(),
                TotalWeightOfEdibleNuts = s.TotalWeightOfEdibleNuts,
                SacksFirst = s.NutTypes.Any(n => n.NutType1 == 1) 
                            ? s.NutTypes.Where(n => n.NutType1 == 1).First().Sacks.ToString() : "",
                KilosFirst = s.NutTypes.Any(n => n.NutType1 == 1) 
                            ? (s.NutTypes.Where(n => n.NutType1 == 1).First().Kilos 
                                * s.NutTypes.Where(n => n.NutType1 == 1).First().Sacks).ToString() : "",
                SacksSecond = s.NutTypes.Any(n => n.NutType1 == 2) 
                            ? (s.NutTypes.Where(n => n.NutType1 == 2).First().Sacks).ToString() : "",
                KilosSecond = s.NutTypes.Any(n => n.NutType1 == 2)
                            ? (s.NutTypes.Where(n => n.NutType1 == 2).First().Kilos
                                * s.NutTypes.Where(n => n.NutType1 == 2).First().Sacks).ToString() : "",
                SacksThird = s.NutTypes.Any(n => n.NutType1== 3) 
                            ? s.NutTypes.Where(n => n.NutType1 == 3).First().Sacks.ToString() : "",
                KilosThird = s.NutTypes.Any(n => n.NutType1 == 3) 
                            ? (s.NutTypes.Where(n => n.NutType1 == 3).First().Kilos 
                                * s.NutTypes.Where(n => n.NutType1 == 3).First().Sacks).ToString() : "",
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
                    let sacksFirstSmall = reportService.GetSacks(v.ReceptionEntries.ToList(), NutSizes.Small, (int)NutQuality.First)
                    let sacksFirstMedium = reportService.GetSacks(v.ReceptionEntries.ToList(), NutSizes.Medium, (int)NutQuality.First)
                    let sacksFirstLarge = reportService.GetSacks(v.ReceptionEntries.ToList(), NutSizes.Large, (int)NutQuality.First)
                    let kilogramsFirstSmall = reportService.GetKilograms(v.ReceptionEntries.ToList(), NutSizes.Small, (int)NutQuality.First)
                    let kilogramsFirstMedium = reportService.GetKilograms(v.ReceptionEntries.ToList(), NutSizes.Medium, (int)NutQuality.First)
                    let kilogramsFirstLarge = reportService.GetKilograms(v.ReceptionEntries.ToList(), NutSizes.Large, (int)NutQuality.First)
                    let totalkilogramsSecond = reportService.GetKilograms(v.ReceptionEntries.ToList(), null, (int)NutQuality.Second)
                    let totalkilogramsThird = reportService.GetKilograms(v.ReceptionEntries.ToList(), null, (int)NutQuality.Third)
                    let totalKilogramsFirst = reportService.GetKilograms(v.ReceptionEntries.ToList(), null, (int)NutQuality.First)
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
                       PercentageFirst = (totalKilogramsFirst == 0 || totalKilos == 0) ? "0%" : ((totalKilogramsFirst / totalKilos) * 100).RoundTwoDigitsDouble().ToString() + "%",
                       PercentageSecond = (totalkilogramsSecond == 0 || totalKilos == 0) ? "0%" : ((totalkilogramsSecond / totalKilos) * 100).RoundTwoDigitsDouble().ToString() + "%",
                       PercentageThird = (totalkilogramsThird == 0 || totalKilos == 0) ? "0%" : ((totalkilogramsThird / totalKilos) * 100).RoundTwoDigitsDouble().ToString() + "%",
                       Variety = variety
                    }).ToList();
        }
        public List<GrillIssueReportModel> CreateReport(List<Grill> issues)
        {
            return issues.Select(i => new GrillIssueReportModel
            {
                Id = i.Id,
                DateCapture = i.DateCapture,
                Quality = i.Quality,
                Variety = i.Variety.Variety1,
                Size = i.Size,
                Sacks = i.Sacks,
                Kilos = i.Kilos,
                Performance = i.Samplings.Any() ? i.Samplings.OrderBy(d => d.DateCapture).First().Performance.ToString() : "0",
                WalnutNumber = i.Samplings.Any() ? i.Samplings.OrderBy(d => d.DateCapture).First().WalnutNumber.ToString() : "0",
                HumidityPercent = i.Samplings.Any() ? i.Samplings.OrderBy(d => d.DateCapture).First().HumidityPercent.ToString() : "0",
                Producer = i.Producer.ProducerName,
                GrillIssueId = i.GrillIssue.Id,
                IssueDateCapture = i.GrillIssue.DateCapture,
                Truck = i.GrillIssue.Truck,
                Driver = i.GrillIssue.Driver,
                Box = i.GrillIssue.Box,
                Remission = i.GrillIssue.Remission
            }).ToList();
        }

        public List<OriginReportModel> CreateReport(List<Batch> batches, List<Variety> varietiesL, List<Remission> remissions, List<NutType> nutTypes)
        {
            return (from b in batches
                    let batch = b.Batch1
                    let hectares = b.Hectares
                    let varieties = (from v in varietiesL
                                     let cleanWalnut = (double)nutTypes.Where(n => n.Sampling.ReceptionEntry.Variety.Id == v.Id).Sum(n => (n.Kilos * n.Sacks))
                                     let rawWalnutInBatch = remissions.Where(r => r.Reception.ReceptionEntry.Variety.Id == v.Id && r.Batch.Id == b.Id).Sum(t => t.Quantity)
                                     let rawWalnut = remissions.Where(r => r.Reception.ReceptionEntry.Variety.Id == v.Id).Sum(t => t.Quantity)
                                     let cleanWalnutInBatch = rawWalnut != 0 ? ((cleanWalnut * rawWalnutInBatch)/rawWalnut).RoundTwoDigitsDouble() : 0 
                                     select new OriginDataModel {
                                         Variety = v.Variety1,
                                         Total = cleanWalnutInBatch 
                                     }).ToList()
                    let totalProduction = varieties.Sum(g => g.Total).RoundTwoDigitsDouble()
                    let performancePerHa = hectares != 0 ? (totalProduction / hectares).RoundTwoDigitsDouble() : 0
                    select new OriginReportModel
                    {
                        Field = batch,
                        Hectares = hectares,
                        Varieties = varieties,
                        TotalProduction = totalProduction,
                        PerformancePerHa = performancePerHa
                    }).ToList();
        }

        public class OriginDataModel {
            public double Total { get; set; }
            public string Variety { get; set; }
        }

        public List<ReceptionModel> Create(List<Reception> receptions)
        {
            return receptions.Select(r => new ReceptionModel
            {
                Id = r.Id,
                Folio = r.Folio,
                Variety = r.ReceptionEntry.Variety.Variety1,
                ReceivedFromField = r.Remissions.Select(re => re.Quantity).Sum(),
                FieldName = r.Remissions != null && r.Remissions.Count != 0 ? string.Join(", ", r.Remissions.Select(re => re.Batch.Batch1)) : "",
                CarRegistration = r.CarRegistration,
                EntryDate = r.ReceptionEntry.DateEntry,
                IssueDate = r.ReceptionEntry.DateIssue,
                HeatHoursDrying = r.HeatHoursDtrying,
                HumidityPercent = r.ReceptionEntry.Humidities.Any() ? r.ReceptionEntry.Humidities.OrderByDescending(h => h.DateCapture).First().HumidityPercent : 0,
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
                FieldName = receptionEntry.Receptions.Select(r => r.Remissions).FirstOrDefault() != null ? string.Join(", ", receptionEntry.Receptions.SelectMany(r => r.Remissions.Select(re => re.Batch.Batch1))) : "",
                Tons = receptionEntry.Receptions.Sum(r => r.Remissions.Sum(re => re.Quantity)),
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
                FieldName = h.ReceptionEntry.Receptions.Select(r => r.Remissions).FirstOrDefault() != null ? string.Join(", ", h.ReceptionEntry.Receptions.SelectMany(r => r.Remissions.Select(re => re.Batch.Batch1))) : "",
                Folio = string.Join(", ", h.ReceptionEntry.Receptions.Select(g => g.Folio)),
                Tons = h.ReceptionEntry.Receptions.Sum(r => r.Remissions.Sum(re => re.Quantity))
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
                ReceivedFromField = reception.Remissions.Sum(r => r.Quantity),
                FieldName = reception.Remissions != null && reception.Remissions.Count != 0 ? string.Join(", ", reception.Remissions.Select(re => re.Batch.Batch1)) : "",
                CarRegistration = reception.CarRegistration,
                EntryDate = reception.ReceptionEntry.DateEntry,
                IssueDate = reception.ReceptionEntry.DateIssue,
                HeatHoursDrying = reception.HeatHoursDtrying,
                HumidityPercent = reception.ReceptionEntry.Humidities.Any() ? reception.ReceptionEntry.Humidities.OrderByDescending(d => d.DateCapture).First().HumidityPercent : 0,
                Observations = reception.Observations,
                ProducerName = reception.ReceptionEntry.Producer.ProducerName,
                Grills = reception.Grills != null && reception.Grills.Count != 0 ? string.Join(", ", reception.Grills.Select(g => g.Id)) : "",
                Cylinder = reception.ReceptionEntry.Cylinder.CylinderName
            };
        }
        public List<FieldModel> Create(List<Batch> batches, bool isDropdown) {
            if (isDropdown)
            {
                return batches.Select(b => new FieldModel {
                    Id = b.Id,
                    Batch = b.Batch1
                }).ToList();
            }
            return batches.Select(b => new FieldModel
            {
                Id = b.Field.Id,
                FieldName = b.Field.FieldName,
                Hectares = b.Hectares,
                Batch = b.Batch1,
                Box = b.Box != null ? b.Box.Box1 : ""
            }).ToList();
        }
        public List<BoxModel> Create(List<Box> boxes)
        {
            return boxes.Select(b => new BoxModel
            {
                Id = b.Id,
                Box = b.Box1
            }).ToList();
        }
        public List<FieldModel> Create(List<Field> fields) {
            return fields.Select(f => new FieldModel
            {
                Id = f.Id,
                FieldName = f.FieldName
            }).ToList();
        }
        public UpdateRemissionModel Create(Remission remission) {
            return new UpdateRemissionModel
            {
                Id = remission.Id,
                Quantity = remission.Quantity,
                Butler = remission.Butler,
                TransportNumber = remission.TransportNumber,
                Driver = remission.Driver,
                Elaborate = remission.Elaborate,
                DateCapture = remission.DateCapture,
                BatchId = remission.BatchId,
                FieldId = remission.FieldId,
                BoxId = remission.BoxId        
            };
        }
        public class UpdateRemissionModel {
            public int Id { get; set; }
            public double Quantity { get; set; }
            public string Butler { get; set; }
            public int TransportNumber { get; set; }
            public string Driver { get; set; }
            public string Elaborate { get; set; }
            public int ReceptionId { get; set; }
            public System.DateTime DateCapture { get; set; }
            public int BatchId { get; set; }
            public int BoxId { get; set; }
            public int FieldId { get; set; }
        }
        public class BoxModel
        {
            public int Id { get; set; }
            public string Box { get; set; }
        }
        public class FieldModel
        {
            public int Id { get; set; }
            public string FieldName { get; set; }
            public double Hectares { get; set; }
            public string Batch { get; set; }
            public string Box { get; set; }
        }
        public class GrillIssueReportModel
        {
            public int Id { get; set; }
            public DateTime DateCapture { get; set; }
            public string Truck { get; set; }
            public string Driver { get; set; }
            public string Box { get; set; }
            public int Remission { get; set; }
            public int Quality { get; internal set; }
            public string Variety { get; internal set; }
            public int Sacks { get; internal set; }
            public int Size { get; internal set; }
            public double Kilos { get; internal set; }
            public string Performance { get; internal set; }
            public string WalnutNumber { get; internal set; }
            public string HumidityPercent { get; internal set; }
            public string Producer { get; internal set; }
            public int GrillIssueId { get; internal set; }
            public DateTime IssueDateCapture { get; internal set; }
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
            public string Folio { get; set; }
            public DateTime DateCapture { get; set; }
            public double SampleWeight { get; set; }
            public double HumidityPercent { get; set; }
            public int WalnutNumber { get; set; }
            public double Performance { get; set; }
            public double TotalWeightOfEdibleNuts { get; set; }
            public string SacksFirst { get; set; }
            public string KilosFirst { get; set; }
            public string SacksSecond { get; set; }
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
            public int GrillId { get; set; }
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
            public string Size { get; set; }
            public int Sacks { get; set; }
            public double Kilos { get; set; }
            public string Quality { get; set; }
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
            public bool State { get; set; }
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

        public class DailyProcessModel
        {
            public string Date { get; set; }
            public string Producer { get; set; }
            public string Folio { get; set; }
            public string Cylinder { get; set; }
            public string Variety { get; set; }
            public int SacksFirstSmall { get; set; }
            public int SacksFirstMedium { get; set; }
            public int Total { get; set; }
            public string QualityPercent { get; set; }
            public int? Germinated { get; set; }
        }
        public List<ProducerReportModel> CreateReport(List<ReceptionEntry> receptionEntries)
        {
           Random num = new Random();
           return receptionEntries.Select(r => new ProducerReportModel
            {
               DateReceptionCapture = r.DateEntry,
               Variety = r.Variety.Variety1,
               FieldName = r.Receptions.Any(re => re.Remissions != null && re.Remissions.Count != 0) ? string.Join(", ", r.Receptions.Select(re => re.Remissions.Select(rem => rem.Batch.Batch1))) : "",
               Remission = DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + num.Next(99999).ToString(),
               Cylinder = r.Cylinder.CylinderName,
               Folio = r.Receptions.Any() ? string.Join(", ", r.Receptions.Select(c => c.Folio)) : "",
               KgsOrigen = r.Receptions.Any() ? r.Receptions.Sum(x => x.Remissions.Sum(re => re.Quantity)): 0,
               ProcessDate = r.DateIssue != null ? r.DateIssue.ToString() : "",
               SacksP = r.Samplings.SelectMany(n => n.NutTypes).Any() ? (int)r.Samplings.SelectMany(s => s.NutTypes.Where(n => n.NutType1 == (int)NutQuality.First)).Sum(n => n.Sacks): 0,
               KilosFirst = r.Samplings.SelectMany(n => n.NutTypes).Any() ? (int)r.Samplings.SelectMany(s => s.NutTypes.Where(n => n.NutType1 == (int)NutQuality.First)).Sum(n => n.Sacks * n.Kilos) : 0,
               SacksS = r.Samplings.SelectMany(n => n.NutTypes).Any() ? (int)r.Samplings.SelectMany(s => s.NutTypes.Where(n => n.NutType1 == (int)NutQuality.Second)).Sum(s => s.Sacks) : 0,
               KilosSecond = r.Samplings.SelectMany(n => n.NutTypes).Any() ? (int)r.Samplings.SelectMany(s => s.NutTypes.Where(n => n.NutType1 == (int)NutQuality.Second)).Sum(n => n.Sacks * n.Kilos) : 0,
               KilosTotal = r.Samplings.SelectMany(n => n.NutTypes).Any() ? (double)r.Samplings.SelectMany(n => n.NutTypes).Sum(g => g.Kilos * g.Sacks) : 0
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
                public string Box { get; set; }
        }

            public class ProducerModel
            {
                public int Id { get; set; }
                public string ProducerName { get; set; }
            }

            public class OriginReportModel
            {
                public string Field { get; set; }
                public double Hectares { get; set; }
                public List<OriginDataModel> Varieties { get; set; }
                public double TotalProduction { get; set; }
                public double PerformancePerHa { get; set; }
            }
            public class ProducerReportModel
            {
                public string Folio { get; set; }
                public string Variety { get; set; }
                public string Remission { get; set; }
                public string Cylinder { get; set; }
                public string FieldName { get; set; }
                public DateTime DateReceptionCapture { get; set; }
                public string ProcessDate { get; set; }
                public double KgsOrigen { get; set; }
                public int SacksP { get; set; }
                public int SacksS { get; set; }
                public double KilosFirst { get; set; }
                public double KilosSecond { get; set; }
                public double KilosTotal { get; set; }
            }
        }
    }