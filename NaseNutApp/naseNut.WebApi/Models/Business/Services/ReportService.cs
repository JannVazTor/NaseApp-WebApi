using naseNut.WebApi.Models.Entities;
using naseNut.WebApi.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace naseNut.WebApi.Models.Business.Services
{
    public class ReportService
    {
        public int GetSacks(List<ReceptionEntry> receptionEntries, NutSizes type, int quality)
        {
            var sacks = 0;
            switch (type)
            {
                case NutSizes.Small:
                    sacks = receptionEntries.SelectMany(r => r.NutTypes).Any(n => n.NutSizeProcessResults.Any()) ?
                        (int)receptionEntries.SelectMany(r => r.NutTypes.Where(n => n.NutType1 == quality).SelectMany(n => n.NutSizeProcessResults.Where(nu => nu.NutSize == (int)NutSizes.Small))).Sum(r => r.Sacks)
                        : 0;
                    break;
                case NutSizes.Medium:
                    sacks = receptionEntries.SelectMany(r => r.NutTypes).Any(n => n.NutSizeProcessResults.Any()) ?
                        (int)receptionEntries.SelectMany(r => r.NutTypes.Where(n => n.NutType1 == quality).SelectMany(n => n.NutSizeProcessResults.Where(nu => nu.NutSize == (int)NutSizes.Medium))).Sum(r => r.Sacks)
                        : 0;
                    break;
                case NutSizes.Large:
                    sacks = receptionEntries.SelectMany(r => r.NutTypes).Any(n => n.NutSizeProcessResults.Any()) ?
                        (int)receptionEntries.SelectMany(r => r.NutTypes.Where(n => n.NutType1 == quality).SelectMany(n => n.NutSizeProcessResults.Where(nu => nu.NutSize == (int)NutSizes.Large))).Sum(r => r.Sacks)
                        : 0;
                    break;
                default:
                    break;
            }
            return sacks;
        }
        public int GetSacks(ReceptionEntry receptionEntries, NutSizes type, int quality)
        {
            var sacks = 0;
            switch (type)
            {
                case NutSizes.Small:
                    sacks = receptionEntries.NutTypes.Any(n => n.NutType1 == quality && n.NutSizeProcessResults.Any()) ?
                        (int)receptionEntries.NutTypes.Where(n => n.NutType1 == quality).First().NutSizeProcessResults.Where(n => n.NutSize == (int)NutSizes.Small).First().Sacks
                        : 0;
                    break;
                case NutSizes.Medium:
                    sacks = receptionEntries.NutTypes.Any(n => n.NutType1 == quality && n.NutSizeProcessResults.Any()) ?
                        (int)receptionEntries.NutTypes.Where(n => n.NutType1 == quality).First().NutSizeProcessResults.Where(n => n.NutSize == (int)NutSizes.Medium).First().Sacks
                        : 0;
                    break;
                case NutSizes.Large:
                    sacks = receptionEntries.NutTypes.Any(n => n.NutType1 == quality && n.NutSizeProcessResults.Any()) ?
                        (int)receptionEntries.NutTypes.Where(n => n.NutType1 == quality).First().NutSizeProcessResults.Where(n => n.NutSize == (int)NutSizes.Large).First().Sacks
                        : 0;
                    break;
                default:
                    break;
            }
            return sacks;
        }

        public double GetKilograms(List<ReceptionEntry> receptionEntries, NutSizes? type, int quality)
        {
            var kilograms = 0.0;
            if (type == null) return receptionEntries.SelectMany(r => r.NutTypes).Any() 
                                ? (double)receptionEntries.SelectMany(r => r.NutTypes.Where(n => n.NutType1 == quality)).Sum(n => n.Kilos * n.Sacks) : 0;
            switch (type)
            {
                case NutSizes.Small:
                    kilograms = receptionEntries.SelectMany(r => r.NutTypes).Any(n => n.NutSizeProcessResults.Any()) ?
                        (int)receptionEntries.SelectMany(r => r.NutTypes.Where(n => n.NutType1 == (int)NutQuality.First)
                            .SelectMany(n => n.NutSizeProcessResults.Where(nu => nu.NutSize == (int)NutSizes.Small))).Sum(r => r.Sacks * r.NutType.Kilos)
                        : 0;
                    break;
                case NutSizes.Medium:
                    kilograms = receptionEntries.SelectMany(r => r.NutTypes).Any(n => n.NutSizeProcessResults.Any()) ?
                        (int)receptionEntries.SelectMany(r => r.NutTypes.Where(n => n.NutType1 == (int)NutQuality.First)
                            .SelectMany(n => n.NutSizeProcessResults.Where(nu => nu.NutSize == (int)NutSizes.Medium))).Sum(r => r.Sacks * r.NutType.Kilos)
                        : 0;
                    break;
                case NutSizes.Large:
                    kilograms = receptionEntries.SelectMany(r => r.NutTypes).Any(n => n.NutSizeProcessResults.Any()) ?
                        (int)receptionEntries.SelectMany(r => r.NutTypes.Where(n => n.NutType1 == (int)NutQuality.First)
                            .SelectMany(n => n.NutSizeProcessResults.Where(nu => nu.NutSize == (int)NutSizes.Large))).Sum(r => r.Sacks * r.NutType.Kilos)
                        : 0;
                    break;
                default:
                    break;
            }
            return kilograms;
        }
        //public int GetSacks(List<Grill> grills, NutSizes type, int quality)
        //{
        //    var sacks = 0;
        //    switch (type)
        //    {
        //        case NutSizes.Small:
        //            sacks = (grills.Any(s => s.Samplings.Any())) ?
        //                grills.SelectMany(g => g.Samplings
        //                    .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.Small && sa.Grill.Quality == quality).Select(s => s.WalnutNumber)).Sum() : 0;
        //            break;
        //        case NutSizes.Medium:
        //            sacks = (grills.Any(s => s.Samplings.Any())) ?
        //                grills.SelectMany(g => g.Samplings
        //                    .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.MediumStart && sa.WalnutNumber <= g.Variety.NutSize.MediumEnd && sa.Grill.Quality == quality).Select(s => s.WalnutNumber)).Sum() : 0;
        //            break;
        //        case NutSizes.Large:
        //            sacks = (grills.Any(s => s.Samplings.Any())) ?
        //                grills.SelectMany(g => g.Samplings
        //                    .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.LargeStart && sa.WalnutNumber <= g.Variety.NutSize.LargeEnd && sa.Grill.Quality == quality).Select(s => s.WalnutNumber)).Sum() : 0;
        //            break;
        //        default:
        //            break;
        //    }
        //    return sacks;
        //}
        //public double GetKilograms(List<Grill> grills, NutSizes? type, int quality)
        //{
        //    var kilograms = 0.0;
        //    if (type == null) return grills.SelectMany(g => g.Samplings.Where(sa => sa.Grill.Quality == quality).Select(s => s.Grill.Kilos)).Sum();
        //    switch (type)
        //    {
        //        case NutSizes.Small:
        //            kilograms = (grills.Any(s => s.Samplings.Any())) ?
        //                grills.SelectMany(g => g.Samplings
        //                    .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.Small && sa.Grill.Quality == quality).Select(s => s.Grill.Kilos)).Sum() : 0;
        //            break;
        //        case NutSizes.Medium:
        //            kilograms = (grills.Any(s => s.Samplings.Any())) ?
        //                grills.SelectMany(g => g.Samplings
        //                    .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.MediumStart && sa.WalnutNumber <= g.Variety.NutSize.MediumEnd && sa.Grill.Quality == quality).Select(s => s.Grill.Kilos)).Sum() : 0;
        //            break;
        //        case NutSizes.Large:
        //            kilograms = (grills.Any(s => s.Samplings.Any())) ?
        //                grills.SelectMany(g => g.Samplings
        //                    .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.LargeStart && sa.WalnutNumber <= g.Variety.NutSize.LargeEnd && sa.Grill.Quality == quality).Select(s => s.Grill.Kilos)).Sum() : 0;
        //            break;
        //        default:
        //            break;
        //    }
        //    return kilograms;
        //}
    }
}