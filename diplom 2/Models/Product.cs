using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace diplom_2.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Filename { get; set; }
        public long Order { get; set; }
    }

    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CategoryId { get; set; }
        public bool Enable { get; set; }
        public string Description { get; set; }
        //Фотографии
        public List<ProductImage> Images { get; set; }
        public List<long> ListPolotnoId { get; set; }
        public List<string> ListPolotno { get; set; }
        //состав сырь
        //цвета


        public static IEnumerable<node> GetProductFromSiteDb()
        {
            
            meotis_newtricEntities db = new meotis_newtricEntities();
            var linksCatalogProducts = from c in db.field_revision_field_catalog 
                                       from p in db.node
                                       where p.nid == c.entity_id
                                       where p.status == 1
                                       where p.language == "ru"
                                       select p;
            return linksCatalogProducts;
        }
    }
}