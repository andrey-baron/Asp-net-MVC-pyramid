using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pyramid.Entity;
using AutoMapper;

namespace Pyramid.BLL
{
    public class ProductBLL
    {
        //public static IEnumerable<Product> AdmidIndex()
        //{
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
        //        .ForMember(d => d.EnumValues, o => o.Ignore())
        //        .ForMember(d => d.Categories, o => o.Ignore())
        //        .ForMember(d => d.ProductValues, o => o.Ignore())
        //        .ForMember(d => d.ThumbnailId, o => o.Ignore())
        //        .ForMember(d => d.ThumbnailImg, o => o.Ignore())
        //        .ForMember(d => d.Images, o => o.Ignore())
        //        ;


        //    });
        //    config.AssertConfigurationIsValid();

        //    var mapper = config.CreateMapper();
        //    var model = mapper.Map<IEnumerable<DBFirstDAL.Products>, List<Entity.Product>>(_productRepository.GetAll().ToList());

        //    return model;
        //}
    }
}
