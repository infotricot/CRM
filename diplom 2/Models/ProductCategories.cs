using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace diplom_2.Models
{
    public enum TypeCategory { Категории = 2, Направления = 3 }

    public class ProductCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public TypeCategory TypeCategory { get; set; }

        public static List<ProductCategory> GetCategories(TypeCategory Type)
        {
            //Объявления объекта класса контекста, через который получаем доступ к базе данных
            meotis_newtricEntities db = new meotis_newtricEntities();
            //Получаем содержимое таблицы taxonomy_term_data, преобразуем её в список объектов и сохраняем в DbCategories
            var DbCategories = db.taxonomy_term_data.Where(a=>a.vid == (int)Type && a.language == "ru").ToList();
            
            //Создаём список категорий своего класса ProductCategories для удобства работы с данными
            List<ProductCategory> Categories = new List<ProductCategory>();
            //перебираем полученную таблицу, которая теперь имеет тип списка 
            foreach (var i in DbCategories)
            {
                //Добавляем в свой список элементы из списка полученного из базы
                Categories.Add(new ProductCategory() { Id = i.tid, Name = i.name, TypeCategory = Type});
            }
            //Возвращает результат
            return Categories;
        }
    }
}