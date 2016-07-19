using naseNut.WebApi.Models.Entities;
using naseNut.WebApi.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                            .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.MediumStart && sa.WalnutNumber <= g.Variety.NutSize.MediumEnd && sa.Grill.Quality == quality).Select(s => s.WalnutNumber)).Sum():0;
                    break;
                case NutSizes.Large:
                    sacks = (grills.Any(s => s.Samplings.Any())) ?
                        grills.SelectMany(g => g.Samplings
                            .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.LargeStart && sa.WalnutNumber <= g.Variety.NutSize.LargeEnd && sa.Grill.Quality == quality).Select(s => s.WalnutNumber)).Sum():0;
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
                            .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.Small && sa.Grill.Quality == quality).Select(s => s.Grill.Kilos)).Sum():0;
                    break;
                case NutSizes.Medium:
                    kilograms = (grills.Any(s => s.Samplings.Any())) ?
                        grills.SelectMany(g => g.Samplings
                            .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.MediumStart && sa.WalnutNumber <= g.Variety.NutSize.MediumEnd && sa.Grill.Quality == quality).Select(s => s.Grill.Kilos)).Sum():0;
                    break;
                case NutSizes.Large:
                    kilograms = (grills.Any(s => s.Samplings.Any())) ?
                        grills.SelectMany(g => g.Samplings
                            .Where(sa => sa.WalnutNumber >= g.Variety.NutSize.LargeStart && sa.WalnutNumber <= g.Variety.NutSize.LargeEnd && sa.Grill.Quality == quality).Select(s => s.Grill.Kilos)).Sum():0;
                    break;
                default:
                    break;
            }
            return kilograms;
        }
    }
}