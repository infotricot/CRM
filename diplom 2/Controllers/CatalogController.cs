
using diplom_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace diplom_2.Controllers
{
    [Authorize]
    public class CatalogController : Controller
    {
        public ActionResult ShowProduct(int id)
        {
            meotis_newtricEntities db = new meotis_newtricEntities();

            node mySqlProduct = db.node.Where(a => a.nid == id).FirstOrDefault();
            Product product = new Product();

            product.Name = mySqlProduct.title;     

            //Получаем строки из таблицы связей между фотографиями и товарами
            var ImageLinks = db.field_data_field_image.Where(a => a.entity_id == mySqlProduct.nid).OrderBy(a => a.delta).ToList();
            product.Images = new List<ProductImage>();
            //Перебираем строки со связями
            foreach (var photo in ImageLinks)
            {
                //Находим в таблице с изображениями строку с текущей связью
                var FindedPhoto = db.file_managed.Where(a => a.fid == photo.field_image_fid).FirstOrDefault();
                product.Images.Add(new ProductImage() { Filename = FindedPhoto.uri.Replace("public://products/", ""), Order = photo.delta });
            }
            try
            {
                product.Description = db.field_revision_body.Where(a => a.entity_id == mySqlProduct.nid).FirstOrDefault().body_value;

                List<field_revision_field_polotno> OldPolotnoList = db.field_revision_field_polotno.Where(a => a.entity_id == mySqlProduct.nid).ToList();
                product.ListPolotnoId = new List<long>();
                product.ListPolotno = new List<string>();
                foreach (var p in OldPolotnoList)
                {
                    var PolotnoInfo = db.taxonomy_term_data.Where(a => a.tid == p.field_polotno_tid).FirstOrDefault();
                    product.ListPolotnoId.Add(PolotnoInfo.tid);
                    product.ListPolotno.Add(PolotnoInfo.name);
                }
            }
            catch
            {
                product.ListPolotnoId = new List<long>();
                product.ListPolotno = new List<string>();
                product.Description = "";
            }
            return View(product);
        }

        // GET: Catalog
        public ActionResult GetProductsInCollection(int id)
        {
            //Нужно получить список товаров категории идентификатор которой храниться в параметре id

            //Создаём объект для доступа к базе данных
            meotis_newtricEntities db = new meotis_newtricEntities();

            //В таблице node храняться все товары. По этой таблице нельзя определить - в какой категории тот или иной товар
            //Связь между товарами и категориями находится в таблице field_revision_field_catalog

            //Сначало нужно получить строки из таблице field_revision_field_catalog, в которых
            //id-категорий совпадают с параметром метода id
            //То есть получить строки из таблицы field_revision_field_catalog, где значения в столбце
            //field_catalog_tid совпадают с параметром id
            var LinkProductAndCollection = db.field_revision_field_collection.Where(a => a.field_collection_tid == id).ToList();


            //Создаём список товаров в более удобнов виде.
            List<Product> Products = new List<Product>();

            //Перебираем полученные строки            
            foreach (var i in LinkProductAndCollection)
            {
                //Получаем строки из таблицы node, в которой значения столбца nid таблицы node совпадают с entity_id таблицы field_revision_field_catalog
                //Метод Where в результате всегда возвращает коллекцию (т.е. объект, который может зранить несколько объектов),
                //Но мы знаем - что в результате будет получена одна строка. Поэтому нужно применить к результату метод FirstOrDefault()
                node mySqlProduct = db.node.Where(a => a.nid == i.entity_id).FirstOrDefault();
                Product product = new Product();
                product.Id = mySqlProduct.nid;
                product.Name = mySqlProduct.title;
                product.CategoryId = i.field_collection_tid.GetValueOrDefault();
                product.Enable = Convert.ToBoolean(mySqlProduct.status);

                if (!product.Enable) continue;

                //Получаем строки из таблицы связей между фотографиями и товарами
                var ImageLinks = db.field_data_field_image.Where(a => a.entity_id == mySqlProduct.nid).OrderBy(a => a.delta).ToList();
                product.Images = new List<ProductImage>();
                //Перебираем строки со связями
                foreach (var photo in ImageLinks)
                {
                    //Находим в таблице с изображениями строку с текущей связью
                    var FindedPhoto = db.file_managed.Where(a => a.fid == photo.field_image_fid).FirstOrDefault();
                    product.Images.Add(new ProductImage() { Filename = FindedPhoto.uri.Replace("public://products/", ""), Order = photo.delta });
                }
                try
                {
                    product.Description = db.field_revision_body.Where(a => a.entity_id == mySqlProduct.nid).FirstOrDefault().body_value;

                    List<field_revision_field_polotno> OldPolotnoList = db.field_revision_field_polotno.Where(a => a.entity_id == mySqlProduct.nid).ToList();
                    product.ListPolotnoId = new List<long>();
                    product.ListPolotno = new List<string>();
                    foreach (var p in OldPolotnoList)
                    {
                        var PolotnoInfo = db.taxonomy_term_data.Where(a => a.tid == p.field_polotno_tid).FirstOrDefault();
                        product.ListPolotnoId.Add(PolotnoInfo.tid);
                        product.ListPolotno.Add(PolotnoInfo.name);
                    }
                }
                catch
                {
                    product.ListPolotnoId = new List<long>();
                    product.ListPolotno = new List<string>();
                    product.Description = "";
                }
                Products.Add(product);
            }

            //Возвращает представление с таким же именем, как и у текущего Action-метода (index),
            //и передаём ему список товаров.
            return View("GetProductsInCategory", Products);
        }

        // GET: Catalog
        public ActionResult GetProductsInCategory(int id)
        {
            //Нужно получить список товаров категории идентификатор которой храниться в параметре id

            //Создаём объект для доступа к базе данных
            meotis_newtricEntities db = new meotis_newtricEntities();

            //В таблице node храняться все товары. По этой таблице нельзя определить - в какой категории тот или иной товар
            //Связь между товарами и категориями находится в таблице field_revision_field_catalog

            //Сначало нужно получить строки из таблице field_revision_field_catalog, в которых
            //id-категорий совпадают с параметром метода id
            //То есть получить строки из таблицы field_revision_field_catalog, где значения в столбце
            //field_catalog_tid совпадают с параметром id
            var LinkProductAndCategory = db.field_revision_field_catalog.Where(a => a.field_catalog_tid == id).ToList();
            

            //Создаём список товаров в более удобнов виде.
            List<Product> Products = new List<Product>();

            //Перебираем полученные строки            
            foreach (var i in LinkProductAndCategory)
            {
                //Получаем строки из таблицы node, в которой значения столбца nid таблицы node совпадают с entity_id таблицы field_revision_field_catalog
                //Метод Where в результате всегда возвращает коллекцию (т.е. объект, который может зранить несколько объектов),
                //Но мы знаем - что в результате будет получена одна строка. Поэтому нужно применить к результату метод FirstOrDefault()
                node mySqlProduct = db.node.Where(a => a.nid == i.entity_id).FirstOrDefault();
                Product product = new Product();
                product.Id = mySqlProduct.nid;
                product.Name = mySqlProduct.title;
                product.CategoryId = i.field_catalog_tid.GetValueOrDefault();
                product.Enable = Convert.ToBoolean(mySqlProduct.status);

                if (!product.Enable) continue;

                //Получаем строки из таблицы связей между фотографиями и товарами
                var ImageLinks = db.field_data_field_image.Where(a => a.entity_id == mySqlProduct.nid).OrderBy(a=>a.delta).ToList();
                product.Images = new List<ProductImage>();
                //Перебираем строки со связями
                foreach (var photo in ImageLinks)
                {
                    //Находим в таблице с изображениями строку с текущей связью
                    var FindedPhoto = db.file_managed.Where(a => a.fid == photo.field_image_fid).FirstOrDefault();
                    product.Images.Add(new ProductImage() { Filename = FindedPhoto.uri.Replace("public://products/",""), Order = photo.delta });
                }
                try
                {
                    product.Description = db.field_revision_body.Where(a => a.entity_id == mySqlProduct.nid).FirstOrDefault().body_value;

                    List<field_revision_field_polotno> OldPolotnoList = db.field_revision_field_polotno.Where(a => a.entity_id == mySqlProduct.nid).ToList();
                    product.ListPolotnoId = new List<long>();
                    product.ListPolotno = new List<string>();
                    foreach (var p in OldPolotnoList)
                    {
                        var PolotnoInfo = db.taxonomy_term_data.Where(a => a.tid == p.field_polotno_tid).FirstOrDefault();
                        product.ListPolotnoId.Add(PolotnoInfo.tid);
                        product.ListPolotno.Add(PolotnoInfo.name);
                    }
                } catch
                {
                    product.ListPolotnoId = new List<long>();
                    product.ListPolotno = new List<string>();
                    product.Description = "";
                }
                Products.Add(product);
            }

            //Возвращает представление с таким же именем, как и у текущего Action-метода (index),
            //и передаём ему список товаров.
            return View(Products);
        }
    }
}