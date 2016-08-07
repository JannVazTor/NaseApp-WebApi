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
        public int GetSacks(List<Grill> grills, NutSizes type, int quality)
        {
            var sacks = 0;
            switch (type)
            {
                case NutSizes.Small:
                    sacks = (grills.Any(s => s.Samplings.Any())) ?
                        grills.SelectMany(g => g.Samplings
                            .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.Small && sa.Grill.Quality == quality).Select(s => s.WalnutNumber)).Sum() : 0;
                    break;
                case NutSizes.Medium:
                    sacks = (grills.Any(s => s.Samplings.Any())) ?
                        grills.SelectMany(g => g.Samplings
                            .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.MediumStart && sa.WalnutNumber <= g.Variety.NutSize.MediumEnd && sa.Grill.Quality == quality).Select(s => s.WalnutNumber)).Sum() : 0;
                    break;
                case NutSizes.Large:
                    sacks = (grills.Any(s => s.Samplings.Any())) ?
                        grills.SelectMany(g => g.Samplings
                            .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.LargeStart && sa.WalnutNumber <= g.Variety.NutSize.LargeEnd && sa.Grill.Quality == quality).Select(s => s.WalnutNumber)).Sum() : 0;
                    break;
                default:
                    break;
            }
            return sacks;
        }
        public int GetSacks(List<ReceptionEntry> receptionEntries, NutSizes type, int quality)
        {
            var sacks = 0;
            switch (type)
            {
                case NutSizes.Small:
                    sacks = receptionEntries.SelectMany(r => r.Samplings.SelectMany(s => s.NutTypes)).Any()
                        ? (int)receptionEntries.SelectMany(r => r.Samplings.Where(sa => sa.WalnutNumber >= sa.ReceptionEntry.Variety.NutSize.Small)
                            .SelectMany(n => n.NutTypes.Where(nu => nu.NutType1 == quality))).Sum(n => n.Sacks) : 0;
                    break;
                case NutSizes.Medium:
                    sacks = receptionEntries.SelectMany(r => r.Samplings.SelectMany(s => s.NutTypes)).Any()
                        ? (int)receptionEntries.SelectMany(r => r.Samplings.Where(sa => sa.WalnutNumber >= sa.ReceptionEntry.Variety.NutSize.MediumStart && sa.WalnutNumber <= sa.ReceptionEntry.Variety.NutSize.MediumEnd)
                            .SelectMany(n => n.NutTypes.Where(nu => nu.NutType1 == (int)NutQuality.First))).Sum(n => n.Sacks) : 0;
                    break;
                case NutSizes.Large:
                    sacks = receptionEntries.SelectMany(r => r.Samplings.SelectMany(s => s.NutTypes)).Any()
                        ? (int)receptionEntries.SelectMany(r => r.Samplings.Where(sa => sa.WalnutNumber >= sa.ReceptionEntry.Variety.NutSize.LargeStart && sa.WalnutNumber <= sa.ReceptionEntry.Variety.NutSize.LargeEnd)
                            .SelectMany(n => n.NutTypes.Where(nu => nu.NutType1 == (int)NutQuality.First))).Sum(n => n.Sacks) : 0;
                    break;
                default:
                    break;
            }
            return sacks;
        }
        public double GetKilograms(List<Grill> grills, NutSizes? type, int quality)
        {
            var kilograms = 0.0;
            if (type == null) return grills.SelectMany(g => g.Samplings.Where(sa => sa.Grill.Quality == quality).Select(s => s.Grill.Kilos)).Sum();
            switch (type)
            {
                case NutSizes.Small:
                    kilograms = (grills.Any(s => s.Samplings.Any())) ?
                        grills.SelectMany(g => g.Samplings
                            .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.Small && sa.Grill.Quality == quality).Select(s => s.Grill.Kilos)).Sum() : 0;
                    break;
                case NutSizes.Medium:
                    kilograms = (grills.Any(s => s.Samplings.Any())) ?
                        grills.SelectMany(g => g.Samplings
                            .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.MediumStart && sa.WalnutNumber <= g.Variety.NutSize.MediumEnd && sa.Grill.Quality == quality).Select(s => s.Grill.Kilos)).Sum() : 0;
                    break;
                case NutSizes.Large:
                    kilograms = (grills.Any(s => s.Samplings.Any())) ?
                        grills.SelectMany(g => g.Samplings
                            .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.LargeStart && sa.WalnutNumber <= g.Variety.NutSize.LargeEnd && sa.Grill.Quality == quality).Select(s => s.Grill.Kilos)).Sum() : 0;
                    break;
                default:
                    break;
            }
            return kilograms;
        }
        public double GetKilograms(List<ReceptionEntry> receptionEntries, NutSizes? type, int quality)
        {
            var kilograms = 0.0;
            if (type == null) return receptionEntries.SelectMany(r => r.Samplings.SelectMany(s => s.NutTypes)).Any() 
                                ? (double)receptionEntries.SelectMany(g => g.Samplings.SelectMany(s => s.NutTypes.Where(n => n.NutType1 == quality))).Sum(n => n.Kilos * n.Sacks) : 0;
            switch (type)
            {
                case NutSizes.Small:
                    kilograms = receptionEntries.SelectMany(r => r.Samplings.SelectMany(s => s.NutTypes)).Any()
                        ? (int)receptionEntries.SelectMany(r => r.Samplings.Where(sa => sa.WalnutNumber >= sa.ReceptionEntry.Variety.NutSize.Small)
                            .SelectMany(n => n.NutTypes.Where(nu => nu.NutType1 == quality))).Sum(n => n.Kilos * n.Sacks) : 0;
                    break;
                case NutSizes.Medium:
                    kilograms = receptionEntries.SelectMany(r => r.Samplings.SelectMany(s => s.NutTypes)).Any()
                        ? (int)receptionEntries.SelectMany(r => r.Samplings.Where(sa => sa.WalnutNumber >= sa.ReceptionEntry.Variety.NutSize.MediumStart && sa.WalnutNumber <= sa.ReceptionEntry.Variety.NutSize.MediumEnd)
                            .SelectMany(n => n.NutTypes.Where(nu => nu.NutType1 == (int)NutQuality.First))).Sum(n => n.Kilos * n.Sacks) : 0;
                    break;
                case NutSizes.Large:
                    kilograms = receptionEntries.SelectMany(r => r.Samplings.SelectMany(s => s.NutTypes)).Any()
                        ? (int)receptionEntries.SelectMany(r => r.Samplings.Where(sa => sa.WalnutNumber >= sa.ReceptionEntry.Variety.NutSize.LargeStart && sa.WalnutNumber <= sa.ReceptionEntry.Variety.NutSize.LargeEnd)
                            .SelectMany(n => n.NutTypes.Where(nu => nu.NutType1 == (int)NutQuality.First))).Sum(n => n.Kilos * n.Sacks) : 0;
                    break;
                default:
                    break;
            }
            return kilograms;
        }
    }
}