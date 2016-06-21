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
                Variety = r.Reception.Variety
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

        //public ReceptionModel Create(Reception reception)
        //{
        //    return reception = new ReceptionModel
        //    {
        //        Id = reception.Id,
        //        Variety = reception.Variety,
        //        ReceivedFromField = reception.ReceivedFromField,
        //        FieldName = reception.FieldName,
        //        CarRegistration = reception.CarRegistration,
        //        EntryDate = reception.EntryDate,
        //        IssueDate = reception.IssueDate,
        //        HeatHoursDrying = reception.HeatHoursDtrying,
        //        HumidityPercent = reception.HumidityPercent,
        //        Observations = reception.Observations,
        //        ProducerName = reception.Producer != null ? reception.Producer.ProducerName : "",
        //        Grills = reception.Grill?.Id.ToString() ?? "",
        //        Cylinder = reception.Cylinders.Count != 0 ? string.Join(", ", reception.Cylinders.Select(c => c.CylinderName)) : ""
        //    };
        //}

        public class CylinderModel
        {
            public int Id { get; set; }
            public string CylinderName { get; set; }
        }

        public List<ReceptionModel> Create(List<Reception> receptions)
        {
            return receptions.Select(r => new ReceptionModel
            {
                Id =  r.Id,
                Variety = r.Variety,
                ReceivedFromField = r.ReceivedFromField,
                FieldName = r.FieldName,
                CarRegistration = r.CarRegistration,
                EntryDate = r.EntryDate,
                IssueDate = r.IssueDate,
                HeatHoursDrying = r.HeatHoursDtrying,
                HumidityPercent = r.HumidityPercent,
                Observations = r.Observations,
                ProducerName = r.Producer != null? r.Producer.ProducerName: "",
                Grills = r.Grill?.Id.ToString() ?? "",
                Cylinder = r.Cylinders.Count != 0 ? string.Join(", ",r.Cylinders.Select(c => c.CylinderName)):""
            }).ToList();
        }

        public class ReceptionModel
        {
            public int Id { get; set; }
            public string Variety { get; set; }
            public double ReceivedFromField { get; set; }
            public string FieldName { get; set; }
            public string CarRegistration { get; set; }
            public DateTime EntryDate { get; set; }
            public DateTime? IssueDate { get; set; }
            public double? HeatHoursDrying { get; set; }
            public double? HumidityPercent { get; set; }
            public string Observations { get; set; }
            public string ProducerName { get; set; }
            public string Grills { get; set; }
            public string Cylinder { get; set; }
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
    }
}