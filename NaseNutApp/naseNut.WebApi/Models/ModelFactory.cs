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
        private NaseNEntities _db = new NaseNEntities();
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
                y = v.Grills.Any(g => g.Variety.Id == v.Id) ? v.Grills.Where(g => g.Variety.Id == v.Id).Sum(g => g.Kilos) : 0
            }).ToList().ToArray();
        }

        public BarChartModel[] CreateDash(List<Producer> producers)
        {
            return producers.Select(p => new BarChartModel
            {
                name = p.ProducerName,
                data = new double[] { p.Grills.Any() ? p.Grills.Sum(g => g.Kilos) : 0 }
            }).ToList().ToArray();
        }

        public BarChartModel[] CreateDash(List<Cylinder> cylinders)
        {
            return cylinders.Select(c => new BarChartModel
            {
                name = c.CylinderName,
                data = new double[] { (DateTime.Now - c.ReceptionEntries.OrderByDescending(d => d.EntryDate).First().EntryDate).TotalHours }
            }).ToList().ToArray();
        }

        public BarChartModel[] CreateDash(List<Grill> grills)
        {
            return new List<BarChartModel>() {
                new BarChartModel {
                    name = "Inventario",
                    data = new double[] {
                        grills.Where(g => g.Status && g.Quality == (int)GrillQuality.First).Count(),
                        grills.Where(g => g.Status && g.Quality == (int)GrillQuality.Second).Count()
                    }
                },
                new BarChartModel {
                    name = "Salidas",
                    data = new double[] {
                        grills.Where(g => !g.Status && g.Quality == (int)GrillQuality.First).Count(),
                        grills.Where(g => !g.Status && g.Quality == (int)GrillQuality.Second).Count()
                    }
                }
            }.ToArray();
        }
        public BarChartWithNumbersModel[] CreateDashBarWithNumber(List<Variety> varieties)
        {
            return (from v in varieties
                    let name = v.Variety1
                    let y = v.Grills.Where(s => s.Samplings.Any()).Any() ? v.Grills.Where(s => s.Samplings.Any())
                        .Sum(g => g.Samplings.Sum(s => ((s.WalnutNumber * 1000) / s.SampleWeight)) / g.Samplings.Count) / v.Grills.Where(s => s.Samplings.Any()).Count() : 0
                    select new BarChartWithNumbersModel
                    {
                        name = name,
                        y = y,
                        drilldown = null
                    }).ToList().ToArray();
        }
        public BarChartWithCategories[] CreateDashBarWithNumberPercentage(List<Variety> varieties)
        {
            return new List<BarChartWithCategories>()
            {
                new BarChartWithCategories {
                    name = "Primeras",
                    data = varieties.OrderByDescending(v => v.Variety1)
                        .Select(v => v.Grills.Any(g => g.Quality == (int)NutQuality.First)
                        ? v.Grills.Where(g => g.Quality == (int)NutQuality.First).Sum(s => s.Kilos) : 0).ToList().ToArray(),
                    categories = varieties.OrderByDescending(v => v.Variety1).Select(v => v.Variety1).ToList().ToArray()
                },
                new BarChartWithCategories {
                    name = "Segundas",
                    data = varieties.Select(v => v.Grills.Any(g => g.Quality == (int)NutQuality.Second)
                        ? v.Grills.Where(g => g.Quality == (int)NutQuality.Second).Sum(s => s.Kilos) : 0).ToList().ToArray(),
                    categories = varieties.OrderByDescending(v => v.Variety1).Select(v => v.Variety1).ToList().ToArray()
                }
            }.ToArray();
        }
        public class BarChartWithCategories
        {
            public string name { get; set; }
            public double[] data { get; set; }
            public string[] categories { get; set; }
        }
        public class BarChartWithNumbersModel
        {
            public string name { get; set; }
            public double y { get; set; }
            public object drilldown { get; set; }
        }
        public class BarChartModel
        {
            public string name { get; set; }
            public double[] data { get; set; }
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
                Variety = r.Reception.ReceptionEntry.Variety.Variety1,
                RemissionFolio = r.Folio
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
            return (from c in cylinders
                    let id = c.Id
                    let cylinderName = c.CylinderName
                    let state = c.Active
                    let lastHumidity = c.ReceptionEntries.Any() ?
                                            c.ReceptionEntries.OrderByDescending(r => r.EntryDate).First().Humidities.OrderByDescending(h => h.DateCapture).FirstOrDefault() != null ?
                                        c.ReceptionEntries.OrderByDescending(r => r.EntryDate).First().Humidities.OrderByDescending(h => h.DateCapture).First().HumidityPercent.ToString() : "" : ""
                    let folios = c.ReceptionEntries.Any(r => r.CylinderId == c.Id) ?
                                        string.Join(", ", c.ReceptionEntries.OrderByDescending(d => d.EntryDate)
                                            .FirstOrDefault(r => r.CylinderId == c.Id).Receptions.Select(f => f.Folio)) : ""
                    select new CylinderModel
                    {
                        Id = id,
                        CylinderName = cylinderName,
                        State = state,
                        LastHumidity = lastHumidity,
                        Folios = folios
                    }).ToList();
        }

        public List<GrillModel> Create(List<Grill> grills)
        {
            return grills.Select(g => new GrillModel
            {
                Id = g.Id,
                DateCapture = g.DateCapture,
                Folio = g.Folio,
                Receptions = g.Receptions.Any() ? string.Join(", ", g.Receptions.Select(r => r.Folio)) : "",
                Size = g.Size == 1 ? "Grande" : (g.Size == 2 ? "Mediana" : "Chica"),
                Sacks = g.Sacks,
                Kilos = g.Kilos,
                Quality = g.Quality == 1 ? "Primera" : (g.Quality == 2 ? "Segunda" : "Tercera"),
                Variety = g.Variety.Variety1,
                Producer = g.Producer.ProducerName,
                Batch = g.Receptions.Any(r => r.Remissions.Any()) ? string.Join(", ", g.Receptions.SelectMany(r => r.Remissions.Select(re => re.Batch.Batch1))) : "",
                Status = g.Status,
                SampleWeight = g.Samplings.Any() ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().SampleWeight.ToString() : "",
                HumidityPercent = g.Samplings.Any() ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().HumidityPercent.ToString() : "",
                WalnutNumber = g.Samplings.Any() ? ((g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().WalnutNumber * 1000) / g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().SampleWeight).RoundTwoDigitsDouble().ToString() : "",
                Performance = g.Samplings.Any() ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().Performance.RoundTwoDigitsDouble() : 0,
                TotalWeightOfEdibleNuts = g.Samplings.Any() ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().TotalWeightOfEdibleNuts.ToString() : ""
            }).ToList();
        }
        public GrillModel Create(Grill g)
        {
            return new GrillModel
            {
                Id = g.Id,
                DateCapture = g.DateCapture,
                Folio = g.Folio,
                Receptions = g.Receptions.Any() ? string.Join(", ", g.Receptions.Select(r => r.Folio)) : "",
                Size = g.Size == 1 ? "Grande" : (g.Size == 2 ? "Mediana" : "Chica"),
                Sacks = g.Sacks,
                Kilos = g.Kilos,
                Quality = g.Quality == 1 ? "Primera" : (g.Quality == 2 ? "Segunda" : "Tercera"),
                Variety = g.Variety.Variety1,
                Producer = g.Producer.ProducerName,
                Batch = g.Receptions.Any(r => r.Remissions.Any()) ? string.Join(", ", g.Receptions.SelectMany(r => r.Remissions.Select(re => re.Batch.Batch1))) : "",
                Status = g.Status,
                SampleWeight = g.Samplings.Any() ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().SampleWeight.ToString() : "",
                HumidityPercent = g.Samplings.Any() ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().HumidityPercent.ToString() : "",
                WalnutNumber = g.Samplings.Any() ? ((g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().WalnutNumber * 1000) / g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().SampleWeight).RoundTwoDigitsDouble().ToString() : "",
                Performance = g.Samplings.Any() ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().Performance.RoundTwoDigitsDouble() : 0,
                TotalWeightOfEdibleNuts = g.Samplings.Any() ? g.Samplings.OrderBy(s => s.DateCapture).FirstOrDefault().TotalWeightOfEdibleNuts.ToString() : ""
            };
        }
        public List<DailyProcessModel> CreateReport(List<ReceptionEntry> receptionEntries, string date)
        {
            var reportService = new ReportService();
            return (from s in receptionEntries
                    let sacksFirstLarge = reportService.GetSacks(receptionEntries, NutSizes.Large, (int)GrillQuality.First)
                    let sacksFirstMedium = reportService.GetSacks(receptionEntries, NutSizes.Medium, (int)GrillQuality.First)
                    let sacksFirstSmall = reportService.GetSacks(receptionEntries, NutSizes.Small, (int)GrillQuality.First)
                    select new DailyProcessModel
                    {
                        Date = s.IssueDate.Value.ToString("dd/MM/yyyy"),
                        Producer = s.Producer.ProducerName,
                        Folio = s.Receptions.ToList().Count != 0 ? string.Join(", ", s.Receptions.Select(c => c.Folio)) : "",
                        Cylinder = s.Cylinder.CylinderName,
                        Variety = s.Variety.Variety1,
                        SacksFirstLarge = sacksFirstLarge,
                        SacksFirstSmall = sacksFirstSmall,
                        SacksFirstMedium = sacksFirstMedium,
                        Total = sacksFirstSmall + sacksFirstMedium + sacksFirstLarge,
                        QualityPercent = s.Samplings.Count != 0 ? s.Samplings.OrderBy(x => x.DateCapture).FirstOrDefault().Performance.ToString(CultureInfo.InvariantCulture) + "%" : "",
                        Germinated = s.Samplings.SelectMany(x => x.NutTypes).Any() ? s.Receptions.SelectMany(x => x.ReceptionEntry.Samplings.SelectMany(n => n.NutTypes).Where(n => n.NutType1 == 2).Select(y => y.Sacks)).Sum() : 0,
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
                Folio = s.Grill.Folio,
                GrillId = s.Grill.Id,
                DateCapture = s.DateCapture,
                SampleWeight = s.SampleWeight,
                HumidityPercent = s.HumidityPercent,
                WalnutNumber = s.WalnutNumber,
                Performance = s.Performance.RoundTwoDigitsDouble(),
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
                EntryDate = r.EntryDate,
                Variety = r.Variety.Variety1,
                Producer = r.Producer.ProducerName,
                Cylinder = r.Cylinder.CylinderName,
                Sampling = r.Samplings.Any(),
                ProcessResult = r.NutTypes.Any()
            }).ToList();
        }

        public List<SamplingReceptionModel> CreateReception(List<ReceptionEntry> receptionEntries)
        {
            return receptionEntries.Select(r => new SamplingReceptionModel
            {
                Id = r.Id,
                Folio = r.Receptions.Any() ? string.Join(", ", r.Receptions.Select(c => c.Folio)) : "",
                DateCapture = r.Samplings.Any() ? r.Samplings.OrderByDescending(d => d.DateCapture).First().DateCapture : (DateTime?)null,
                SampleWeight = r.Samplings.Any() ? r.Samplings.OrderByDescending(d => d.DateCapture).First().SampleWeight : 0,
                HumidityPercent = r.Samplings.Any() ? r.Samplings.OrderByDescending(d => d.DateCapture).First().HumidityPercent : 0,
                WalnutNumber = r.Samplings.Any() ? r.Samplings.OrderByDescending(d => d.DateCapture).First().WalnutNumber : 0,
                Variety = r.Variety.Variety1,
                Performance = r.Samplings.Any() ? r.Samplings.OrderByDescending(d => d.DateCapture).First().Performance.RoundTwoDigitsDouble() : 0,
                TotalWeightOfEdibleNuts = r.Samplings.Any() ? r.Samplings.OrderByDescending(d => d.DateCapture).First().TotalWeightOfEdibleNuts : 0,
                SacksFirst = r.NutTypes.Any(n => n.NutType1 == 1)
                            ? r.NutTypes.Where(n => n.NutType1 == 1).First().Sacks.ToString() : "",
                KilosFirst = r.NutTypes.Any(n => n.NutType1 == 1)
                            ? (r.NutTypes.Where(n => n.NutType1 == 1).First().Kilos
                                * r.NutTypes.Where(n => n.NutType1 == 1).First().Sacks).ToString() : "",
                SacksSecond = r.NutTypes.Any(n => n.NutType1 == 2)
                            ? (r.NutTypes.Where(n => n.NutType1 == 2).First().Sacks).ToString() : "",
                KilosSecond = r.NutTypes.Any(n => n.NutType1 == 2)
                            ? (r.NutTypes.Where(n => n.NutType1 == 2).First().Kilos
                                * r.NutTypes.Where(n => n.NutType1 == 2).First().Sacks).ToString() : "",
                SacksThird = r.NutTypes.Any(n => n.NutType1 == 3)
                            ? r.NutTypes.Where(n => n.NutType1 == 3).First().Sacks.ToString() : "",
                KilosThird = r.NutTypes.Any(n => n.NutType1 == 3)
                            ? (r.NutTypes.Where(n => n.NutType1 == 3).First().Kilos
                                * r.NutTypes.Where(n => n.NutType1 == 3).First().Sacks).ToString() : "",
            }).ToList();
        }
        public List<NutTypeModel> Create(List<NutType> nutTypes)
        {
            return nutTypes.Select(n => new NutTypeModel
            {
                Id = n.Id,
                NutType = n.NutType1,
                Sacks = (int)n.Sacks,
                Kilos = (int)n.Kilos
            }).ToList();
        }
        public List<ProducerReportModel> CreateReport(List<ReceptionEntry> receptionEntries)
        {
            var reportService = new ReportService();
            return (from r in receptionEntries
                    let variety = r.Variety.Variety1
                    let batch = r.Receptions.Select(x => x.Remissions).Any() ? string.Join(", ", r.Receptions.SelectMany(y => y.Remissions.Select(re => re.Batch.Batch1))) : ""
                    let cylinder = r.Cylinder.CylinderName
                    let folio = r.Receptions.Any() ? string.Join(", ", r.Receptions.Select(c => c.Folio)) : ""
                    let kgsOrigin = r.Receptions.Any() ? r.Receptions.Sum(x => x.Remissions.Sum(re => re.Quantity)) : 0
                    let processDate = r.IssueDate != null ? r.IssueDate.ToString() : ""
                    let sacksP = r.NutTypes.Any() ? (int)r.NutTypes.Where(n => n.NutType1 == (int)NutQuality.First).Sum(n => n.Sacks) : 0
                    let kilosFirst = r.NutTypes.Any() ? (int)r.NutTypes.Where(n => n.NutType1 == (int)NutQuality.First).Sum(n => n.Sacks * n.Kilos) : 0
                    let sacksS = r.NutTypes.Any() ? (int)r.NutTypes.Where(n => n.NutType1 == (int)NutQuality.Second).Sum(n => n.Sacks) : 0
                    let kilosSecond = r.NutTypes.Any() ? (int)r.NutTypes.Where(n => n.NutType1 == (int)NutQuality.Second).Sum(n => n.Sacks * n.Kilos) : 0
                    let kilosTotal = r.NutTypes.Any() ? (double)r.NutTypes.Sum(g => g.Kilos * g.Sacks) : 0
                    let sacksFirstSmall = reportService.GetSacks(r, NutSizes.Small, (int)NutQuality.First)
                    let sacksFirstMedium = reportService.GetSacks(r, NutSizes.Medium, (int)NutQuality.First)
                    let sacksFirstLarge = reportService.GetSacks(r, NutSizes.Large, (int)NutQuality.First)
                    let sacksFirstTotal = sacksFirstSmall + sacksFirstMedium + sacksFirstLarge
                    select new ProducerReportModel
                    {
                        Variety = variety,
                        Batch = batch,
                        Cylinder = cylinder,
                        Folio = folio,
                        KgsOrigen = kgsOrigin,
                        ProcessDate = processDate,
                        SacksP = sacksP,
                        KilosFirst = kilosFirst,
                        SacksS = sacksS,
                        KilosSecond = kilosSecond,
                        KilosTotal = kilosTotal,
                        SacksFirstSmall = sacksFirstSmall,
                        SacksFirstMedium = sacksFirstMedium,
                        SacksFirstLarge = sacksFirstLarge,
                        SacksFirstTotal = sacksFirstTotal
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

        public List<OriginReportModel> CreateReport(List<Batch> batches, List<Variety> varietiesL, List<Remission> remissions, List<NutType> nutTypes)
        {
            return (from b in batches
                    let field = b.Field.FieldName
                    let batch = b.Batch1
                    let hectares = b.Hectares
                    let varieties = (from v in varietiesL
                                     let cleanWalnut = (double)nutTypes.Where(n => n.ReceptionEntry.Variety.Id == v.Id).Sum(n => (n.Kilos * n.Sacks))
                                     let rawWalnutInBatch = remissions.Where(r => r.Reception.ReceptionEntry.Variety.Id == v.Id && r.Batch.Id == b.Id).Sum(t => t.Quantity)
                                     let rawWalnut = remissions.Where(r => r.Reception.ReceptionEntry.Variety.Id == v.Id).Sum(t => t.Quantity)
                                     let cleanWalnutInBatch = rawWalnut != 0 ? ((cleanWalnut * rawWalnutInBatch) / rawWalnut).RoundTwoDigitsDouble() : 0
                                     select new OriginDataModel
                                     {
                                         Variety = v.Variety1,
                                         Total = cleanWalnutInBatch,
                                         Performance = v.NutInBatches.Any(vn => vn.BatchId == b.Id && vn.VarietyId == v.Id) ?
                                            ((cleanWalnutInBatch / hectares) * v.NutInBatches.Where(vn => vn.BatchId == b.Id
                                                && vn.VarietyId == v.Id).First().NutPercentage).RoundTwoDigitsDouble() : 0
                                     }).ToList()
                    let totalProduction = varieties.Sum(g => g.Total).RoundTwoDigitsDouble()
                    let performancePerHa = hectares != 0 ? (totalProduction / hectares).RoundTwoDigitsDouble() : 0
                    select new OriginReportModel
                    {
                        Field = field,
                        Batch = batch,
                        Hectares = hectares,
                        Varieties = varieties,
                        TotalProduction = totalProduction,
                        PerformancePerHa = performancePerHa
                    }).ToList();
        }

        public class OriginDataModel
        {
            public double Total { get; set; }
            public string Variety { get; set; }
            public double Performance { get; set; }
        }

        public List<ReceptionModel> Create(List<Reception> receptions)
        {
            return receptions.Select(r => new ReceptionModel
            {
                Id = r.Id,
                Folio = r.Folio,
                Remissions = r.Remissions.Any() ? string.Join(", ", r.Remissions.Select(re => re.Folio)) : "",
                Variety = r.ReceptionEntry.Variety.Variety1,
                ReceivedFromField = r.Remissions.Select(re => re.Quantity).Sum(),
                FieldName = r.Remissions.Any() ? string.Join(", ", r.Remissions.Select(re => re.Batch.Batch1)) : "",
                CarRegistration = r.CarRegistration,
                EntryDate = r.ReceptionEntry.EntryDate,
                IssueDate = r.ReceptionEntry.IssueDate,
                HeatHoursDrying = r.HeatHoursDtrying,
                HumidityPercent = r.ReceptionEntry.Humidities.Any() ? r.ReceptionEntry.Humidities.OrderByDescending(h => h.DateCapture).First().HumidityPercent : 0,
                Observations = r.Observations,
                ProducerName = r.ReceptionEntry.Producer.ProducerName,
                Grills = r.Grills != null && r.Grills.Count != 0 ? string.Join(", ", r.Grills.Select(g => g.Id)) : "",
                Cylinder = r.ReceptionEntry.Cylinder.CylinderName,
                Active = r.ReceptionEntry.Active ? "ACTIVO" : "SALIO"
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
                EntryDate = receptionEntry.EntryDate,
                Folio = string.Join(", ", receptionEntry.Receptions.Select(re => re.Folio))
            };
        }
        public List<HumidityModel> Create(List<Humidity> humidities)
        {
            return humidities.Select(h => new HumidityModel
            {
                Id = h.Id,
                DateCapture = h.DateCapture,
                HumidityPercentage = h.HumidityPercent
            }).ToList();
        }
        public List<HumiditiesManageModel> CreateH(List<Humidity> humidities)
        {
            return humidities.Select(h => new HumiditiesManageModel
            {
                Id = h.Id,
                HumidityPercentage = h.HumidityPercent,
                DateCapture = h.DateCapture,
                CylinderName = h.ReceptionEntry.Cylinder.CylinderName,
                EntryDate = h.ReceptionEntry.EntryDate,
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
                EntryDate = r.EntryDate,
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
                EntryDate = reception.ReceptionEntry.EntryDate,
                IssueDate = reception.ReceptionEntry.IssueDate,
                HeatHoursDrying = reception.HeatHoursDtrying,
                HumidityPercent = reception.ReceptionEntry.Humidities.Any() ? reception.ReceptionEntry.Humidities.OrderByDescending(d => d.DateCapture).First().HumidityPercent : 0,
                Observations = reception.Observations,
                ProducerName = reception.ReceptionEntry.Producer.ProducerName,
                Grills = reception.Grills != null && reception.Grills.Count != 0 ? string.Join(", ", reception.Grills.Select(g => g.Id)) : "",
                Cylinder = reception.ReceptionEntry.Cylinder.CylinderName
            };
        }
        public List<FieldModel> Create(List<Batch> batches, bool isDropdown)
        {
            if (isDropdown)
            {
                return batches.Select(b => new FieldModel
                {
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
                Box = b.Box != null ? b.Box.Box1 : "",
                NutPercentages = b.NutInBatches.Any() ? string.Join("", b.NutInBatches.Select(n => n.Variety.Variety1 + " " + n.NutPercentage.ToString() + "%. ")) : ""
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
        public List<FieldModel> Create(List<Field> fields)
        {
            return fields.Select(f => new FieldModel
            {
                Id = f.Id,
                FieldName = f.FieldName
            }).ToList();
        }
        public UpdateRemissionModel Create(Remission remission)
        {
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
        public List<GrillIssueModel> CreateReport(List<GrillIssue> issues)
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

        public List<GrillIssueModel> CreateReport(List<Grill> grill, List<GrillIssue> issues)
        {
            return issues.Select(i => new GrillIssueModel
            {
                Id = i.Id,
                DateCapture = i.DateCapture,
                Truck = i.Truck,
                Driver = i.Driver,
                Box = i.Box,
                Remission = i.Remission,
                Grills = Create(i.Grills.Where(g => g.GrillIssuesId == i.Id && g.Quality == 2).ToList())
            }).ToList();
        }
        public List<HarvestSeasonModel> Create(List<HarvestSeason> harvestSeasons)
        {
            return harvestSeasons.Select(h => new HarvestSeasonModel
            {
                Id = h.Id,
                Active = h.Active,
                EntryDate = h.EntryDate,
                IssueDate = h.IssueDate,
                Description = h.Description,
                Name = h.Name,
                UserName = _db.AspNetUsers.First(u => u.Id == h.UserId).UserName
            }).ToList();
        }

        public class HarvestSeasonModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime EntryDate { get; set; }
            public DateTime? IssueDate { get; set; }
            public bool Active { get; set; }
            public string UserName { get; set; }
        }

        public class UpdateRemissionModel
        {
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
            public string NutPercentages { get; set; }
        }
        public class GrillIssueReportModel
        {
            public string Truck { get; set; }
            public string Driver { get; set; }
            public string Box { get; set; }
            public int Remission { get; set; }
            public int Id { get; internal set; }
            public DateTime DateCapture { get; internal set; }
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
            public string Variety { get; set; }
            public DateTime? DateCapture { get; set; }
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
            public DateTime EntryDate { get; set; }
            public string Variety { get; set; }
            public string Producer { get; set; }
            public string Cylinder { get; set; }
            public bool Sampling { get; set; }
            public bool ProcessResult { get; set; }
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
            public int Folio { get; set; }
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
            public int Folio { get; set; }
            public DateTime DateCapture { get; set; }
            public string Receptions { get; set; }
            public string Size { get; set; }
            public int Sacks { get; set; }
            public double Kilos { get; set; }
            public string Quality { get; set; }
            public string Variety { get; set; }
            public string Producer { get; set; }
            public string Batch { get; set; }
            public string Field { get; set; }
            public bool Status { get; set; }
            public string SampleWeight { get; set; }
            public string HumidityPercent { get; set; }
            public string WalnutNumber { get; set; }
            public double Performance { get; set; }
            public string TotalWeightOfEdibleNuts { get; set; }
        }

        public class CylinderModel
        {
            public int Id { get; set; }
            public string CylinderName { get; set; }
            public bool State { get; set; }
            public string LastHumidity { get; set; }
            public string Folios { get; set; }
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
            public DateTime DateCapture { get; set; }
            public double HumidityPercentage { get; set; }
        }

        public class DailyProcessModel
        {
            public string Date { get; set; }
            public string Producer { get; set; }
            public string Folio { get; set; }
            public string Cylinder { get; set; }
            public string Variety { get; set; }
            public int SacksFirstLarge { get; set; }
            public int SacksFirstMedium { get; set; }
            public int SacksFirstSmall { get; set; }
            public int Total { get; set; }
            public string QualityPercent { get; set; }
            public int? Germinated { get; set; }
        }

        public class ReceptionModel
        {
            public int Id { get; set; }
            public int Folio { get; set; }
            public string CylinderStatus { get; set; }
            public string Remissions { get; set; }
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
            public string Active { get; set; }
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
            public int RemissionFolio { get; set; }
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
            public string Batch { get; set; }
            public double Hectares { get; set; }
            public List<OriginDataModel> Varieties { get; set; }
            public double TotalProduction { get; set; }
            public double PerformancePerHa { get; set; }
        }
        public class ProducerReportModel
        {
            public string Folio { get; set; }
            public string Variety { get; set; }
            public string Cylinder { get; set; }
            public string Batch { get; set; }
            public string ProcessDate { get; set; }
            public double KgsOrigen { get; set; }
            public int SacksP { get; set; }
            public int SacksS { get; set; }
            public double KilosFirst { get; set; }
            public double KilosSecond { get; set; }
            public double KilosTotal { get; set; }
            public int SacksFirstSmall { get; internal set; }
            public int SacksFirstMedium { get; internal set; }
            public int SacksFirstLarge { get; internal set; }
            public int SacksFirstTotal { get; internal set; }
        }
    }
}