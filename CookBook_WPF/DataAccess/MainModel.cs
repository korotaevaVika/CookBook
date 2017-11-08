using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookBook_WPF.Data;
using System.Data;
using System.Data.SqlClient;

namespace CookBook_WPF.DataAccess
{
    public class MainModel
    {
        ContextProvider _contextProvider;
        private CookBookModel Context
        {
            get { return _contextProvider.CreateNew(); }
        }
        public MainModel()
        {
            _contextProvider = new ContextProvider();
        }
        public DataView GetMaterialGroups()
        {
            using (CookBookModel dbContext = Context)
            {
                DataTable dt = new DataTable();

                dbContext.Database.Connection.Open();
                var con = (SqlConnection)dbContext.Database.Connection;
                var cmd = new SqlCommand("SELECT * FROM dbo.vwMaterialGroupList", con);
                using (var rdr = cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                }
                return dt.AsDataView();
            }
        }

        public DataView GetProducts(int pGroupKey)
        {
            using (CookBookModel dbContext = Context)
            {
                DataTable dt = new DataTable();

                dbContext.Database.Connection.Open();
                var con = (SqlConnection)dbContext.Database.Connection;
                var cmd = new SqlCommand("exec sp_SelectProducts " +
                    "  @pGroupKey = " + pGroupKey, con);
                using (var rdr = cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                }
                return dt.AsDataView();
            }
        }

        public DataView GetRecipes()
        {
            using (CookBookModel dbContext = Context)
            {

                Recipe rcp = new Recipe
                {
                    nProduct = dbContext.Products.FirstOrDefault(),
                    rQuantity = 100,
                    szRecipeName = "Тестовый рецепт"
                };

                dbContext.Recipes.Add(rcp);
                Ingredient ing = new Ingredient
                {
                    nProduct = dbContext.MeasureProductRelations.FirstOrDefault(),
                    nRecipe = rcp
                };
                dbContext.SaveChanges();


                DataTable dt = new DataTable();

                dbContext.Database.Connection.Open();
                var con = (SqlConnection)dbContext.Database.Connection;
                var cmd = new SqlCommand("SELECT * FROM dbo.vwRecipeList", con);
                using (var rdr = cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                }
                return dt.AsDataView();
            }
        }

        public string SaveMaterialGroup(
            int pGroupKey,
            string pGroupName,
            bool pIsContainsFinishedProducts,
            ref bool pSuccess
            )
        {
            using (CookBookModel dbContext = Context)
            {
                try
                {
                    if (pGroupKey == 0)
                    {
                        if (dbContext.MaterialGroups.ToList().Exists(x => x.szGroupName == pGroupName))
                        {
                            //throw new InvalidOperationException(
                            //    "Ошибка создания группы материалов\nГруппа с таким именем уже существует");
                            return "Ошибка создания группы материалов\nГруппа с таким именем уже существует";
                        }
                        dbContext.MaterialGroups.Add(
                            new MaterialGroup
                            {
                                szGroupName = pGroupName,
                                bContainsFinishedProduct = pIsContainsFinishedProducts
                            });
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        if (dbContext.MaterialGroups.ToList().Exists(x => x.szGroupName == pGroupName && x.nKey != pGroupKey))
                        {
                            throw new InvalidOperationException(
                                "Ошибка редактирования группы материалов\nГруппа с таким именем уже существует");
                        }
                        MaterialGroup mg = dbContext.MaterialGroups.FirstOrDefault(x => x.nKey == pGroupKey);
                        mg.szGroupName = pGroupName;
                        mg.bContainsFinishedProduct = pIsContainsFinishedProducts;
                        dbContext.SaveChanges();
                    }
                    pSuccess = true;
                    return "Успешно";
                }
                catch (Exception ex)
                {
                    return "Что-то пошло не так...\n" + ex.ToString();
                }
            }
        }
        public string SaveMaterial(
            int productKey,
            string productName,
            double protein,
            double fat,
            double carbohydrates,
            double energy,
            int groupKey,
            ref bool mSuccess)
        {
            using (CookBookModel dbContext = Context)
            {
                try
                {
                    if (productKey == 0)
                    {
                        if (dbContext.Products.ToList().Exists(x => x.szMaterialName == productName))
                        {
                            throw new InvalidOperationException(
                                "Ошибка создания продукта\nПродукт с таким именем уже существует");
                        }

                        var grp = dbContext.MaterialGroups.FirstOrDefault(x => x.nKey == groupKey);
                        dbContext.Products.Add(
                            new Product
                            {
                                szMaterialName = productName,
                                rCarbohydrate = carbohydrates,
                                rEnergy = energy,
                                rFat = fat,
                                rProtein = protein,
                                nMaterialGroup = grp
                            });
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        if (dbContext.Products.ToList().Exists(x => x.szMaterialName == productName && x.nKey != productKey))
                        {
                            throw new InvalidOperationException(
                                "Ошибка редактирования продукта\nПродукт с таким именем уже существует");
                        }
                        Product mt = dbContext.Products.FirstOrDefault(x => x.nKey == productKey);
                        var grp = dbContext.MaterialGroups.FirstOrDefault(x => x.nKey == groupKey);

                        mt.rCarbohydrate = carbohydrates;
                        mt.rEnergy = energy;
                        mt.rFat = fat;
                        mt.rProtein = protein;
                        mt.szMaterialName = productName;

                        dbContext.SaveChanges();
                    }
                    mSuccess = true;
                    return "Продукт успешно создан/отредактирован";
                }
                catch (Exception ex)
                {
                    return "Что-то пошло не так...\n" + ex.ToString();
                }
            }
        }

        internal double CountEnergyValue(double pProtein, double pFat, double pCarbohydrates)
        {
            using (CookBookModel dbContext = Context)
            {
                return dbContext.Database.SqlQuery<double>(
                    "SELECT dbo.fnCountEnergyValue({0}, {1}, {2})",
                    pProtein, pFat, pCarbohydrates).First();
            }
        }

        internal string DeleteMaterial(int productKey, ref bool mSuccess)
        {
            using (CookBookModel dbContext = Context)
            {
                try
                {
                    Product pr = dbContext.Products.FirstOrDefault(x => x.nKey == productKey);
                    dbContext.Products.Remove(pr);
                    dbContext.SaveChanges();
                    mSuccess = true;
                    return "Продукт успешно удален";
                }
                catch (Exception ex)
                {
                    return "Что-то пошло не так...\n" + ex.ToString();
                }

            }
        }

        internal DataView GetMeasures(int pProductKey, int pFindDefault)
        {
            using (CookBookModel dbContext = Context)
            {
                DataTable dt = new DataTable();

                dbContext.Database.Connection.Open();
                var con = (SqlConnection)dbContext.Database.Connection;
                var cmd = new SqlCommand("exec sp_SelectMeasureValues " +
                    "  @pProductKey = " + pProductKey + ", @pFindDefault = " + pFindDefault, con);
                using (var rdr = cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                }
                return dt.AsDataView();
            }
        }

        internal string DeleteMaterialGroup(int groupKey, ref bool mSuccess)
        {
            using (CookBookModel dbContext = Context)
            {
                try
                {
                    MaterialGroup mg = dbContext.MaterialGroups.FirstOrDefault(x => x.nKey == groupKey);
                    dbContext.MaterialGroups.Remove(mg);
                    dbContext.SaveChanges();
                    mSuccess = true;
                    return "Группа материалов успешно удалена";
                }
                catch (Exception ex)
                {
                    return "Что-то пошло не так...\n" + ex.ToString();
                }
            }
        }


    }
}
