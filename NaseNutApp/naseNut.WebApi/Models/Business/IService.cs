using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace naseNut.WebApi.Models.Business
{
    internal interface IService<T>
    {
        bool Save(T obj);
        bool Delete(T obj);
    }
}