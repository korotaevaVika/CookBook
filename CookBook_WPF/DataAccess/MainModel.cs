using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookBook_WPF.Data;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using CookBook_WPF.Helper_Classes.DataWrappers;

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
            try
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
            catch { return null; }
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
        public DataView GetRecipes(int productKey)
        {
            using (CookBookModel dbContext = Context)
            {
                DataTable dt = new DataTable();
                dbContext.Database.Connection.Open();
                var con = (SqlConnection)dbContext.Database.Connection;
                var cmd = new SqlCommand("SELECT * FROM dbo.vwRecipeList where ProductKey = " + productKey, con);
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

        internal string SaveBasket(int nKey, DateTime selectedDate, string description, List<int> plansIndexes, ref bool mSuccess)
        {
            using (CookBookModel dbContext = Context)
            {
                try
                {
                    if (nKey == 0)
                    {
                        if (dbContext.Baskets.ToList().FirstOrDefault(x => x.szDescription == description) != null)
                        {
                            return "Ошибка создания корзины\nКорзина с таким описанием уже существует";
                        }
                        List<Plan> plans = new List<Plan>();
                        plans.AddRange(dbContext.Plans.Where(x => plansIndexes.Contains(x.nKey)));

                        dbContext.Baskets.Add(
                            new Basket
                            {
                                szDescription = description,
                                tDate = selectedDate,
                                Plans = plans
                            });
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        if (dbContext.Baskets.ToList().Exists(x => x.szDescription == description && x.nKey != nKey))
                        {
                            throw new InvalidOperationException(
                                "Ошибка создания корзины\nКорзина с таким описанием уже существует");
                        }
                        Basket b = dbContext.Baskets.FirstOrDefault(x => x.nKey == nKey);
                        b.szDescription = description;
                        b.tDate = selectedDate;

                        List<Plan> plans = new List<Plan>();
                        plans.AddRange(dbContext.Plans.Where(x => plansIndexes.Contains(x.nKey)));

                        b.Plans.Clear();
                        b.Plans = plans; 
                        dbContext.SaveChanges();
                    }
                    mSuccess = true;
                    return "Успешно";
                }
                catch (Exception ex)
                {
                    return "Что-то пошло не так...\n" + ex.ToString();
                }
            }
        }

        internal DataTable GetBuskets()
        {
            using (CookBookModel dbContext = Context)
            {
                DataTable dt = new DataTable();

                dbContext.Database.Connection.Open();
                var con = (SqlConnection)dbContext.Database.Connection;
                var query = "select * from vwBasketList";
                var cmd = new SqlCommand(query, con);
                using (var rdr = cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                }
                return dt;
            }
        }

        internal string SaveMeasureProductRelations(
            List<MeasureProductWrapper> measureProductWrappers,
            int productKey,
            ref bool mSuccess)
        {
            using (CookBookModel dbContext = Context)
            {
                try
                {
                    int mainMeasureKey = measureProductWrappers.FirstOrDefault().MainMeasureKey;

                    //сначала обход существующих отношений и удаление ненужных
                    measureProductWrappers.
                        Where(x => x.IsChanged && !x.IsSaved && (x.MeasureProductKey != 0)).
                        ToList().
                        ForEach(x =>
                        {
                            var item = dbContext.MeasureProductRelations.FirstOrDefault(w => w.nKey == x.MeasureProductKey);
                            dbContext.MeasureProductRelations.Remove(item);
                        });
                    dbContext.SaveChanges();

                    //обход существующих и изменение их количества
                    var existingMeasures = dbContext.MeasureProductRelations.
                        Include(x => x.nProduct).
                        Where(x => x.nProduct.nKey == productKey && x.bIsDefault).ToList();

                    bool isError = false;
                    measureProductWrappers.
                       Where(x => x.IsChanged && x.IsSaved && (x.MeasureProductKey != 0)).
                       ToList().
                       ForEach(x =>
                       {
                           try
                           {
                               existingMeasures.FirstOrDefault(v => v.nKey == x.MeasureProductKey).rQuantity =
                               x.Proportion.HasValue ? x.Proportion.Value : (x.CurrentMeasureQuantity.Value / x.MainMeasureQuantity.Value);
                           }
                           catch
                           {
                               isError = true;
                               //throw;
                           }
                       });
                    if (isError)
                    {
                        mSuccess = false;
                        return "Ошибка записи данных. Не все данные были заполнены..";
                    }
                    else
                    {
                        dbContext.SaveChanges();
                    }

                    var pr = dbContext.Products.FirstOrDefault(m => m.nKey == productKey);
                    measureProductWrappers.
                       Where(x => x.IsChanged && x.IsSaved && (x.MeasureProductKey == 0)).
                       ToList().
                       ForEach(x =>
                       {
                           try
                           {
                               var ms = dbContext.Measures.FirstOrDefault(m => m.nKey == x.MeasureKey);
                               dbContext.MeasureProductRelations.Add(
                                   new MeasureProductRelation
                                   {
                                       bIsDefault = false,
                                       nMeasure = ms,
                                       nProduct = pr,
                                       rQuantity = x.Proportion.HasValue ?
                                                x.Proportion.Value :
                                                (x.CurrentMeasureQuantity.Value / x.MainMeasureQuantity.Value)
                                   });
                               dbContext.SaveChanges();
                           }
                           catch
                           {
                               isError = true;
                           }
                       });
                    if (isError)
                    {
                        mSuccess = false;
                        return "Ошибка записи данных. Не все данные были заполнены..";
                    }
                    else
                    {
                        mSuccess = true;
                        return "Изменения успешно сохранены";
                    }

                }
                catch
                {
                    mSuccess = false;
                    return "Ошибка записи данных. Не все данные были заполнены..";
                }

            }
        }

        internal string DeletePlan(int planKey, ref bool mSuccess)
        {
            using (CookBookModel dbContext = Context)
            {
                try
                {
                    var pln = dbContext.Plans.
                        FirstOrDefault(x => x.nKey == planKey);

                    dbContext.Plans.Remove(pln);
                    dbContext.SaveChanges();
                    mSuccess = true;
                    return "План успешно удален";
                }
                catch (Exception ex)
                {
                    mSuccess = false;
                    return "Что-то пошло не так.." + ex.Message.ToString();
                }
            }
        }

        internal string SavePlan(
            int mPlanKey,
            int mProductKey,
            int mRecipeKey,
            DateTime mDate,
            double mQuantity,
            ref bool mSuccess)
        {
            using (CookBookModel dbContext = Context)
            {
                try
                {
                    if (mPlanKey != 0)
                    {
                        var pln = dbContext.Plans.
                         Include(x => x.nProduct).
                         Include(x => x.nRecipe).
                         FirstOrDefault(
                            x => x.nKey == mPlanKey);
                        pln.nProduct = dbContext.Products.FirstOrDefault(
                            x => x.nKey == mProductKey);
                        pln.nRecipe = dbContext.Recipes.FirstOrDefault(
                            x => x.nKey == mRecipeKey);
                        pln.rQuantity = mQuantity;
                        pln.tDate = mDate;
                    }
                    else
                    {
                        Recipe rc = dbContext.Recipes.FirstOrDefault(x => x.nKey == mRecipeKey);
                        Product prd = dbContext.Products.FirstOrDefault(x => x.nKey == mProductKey);
                        Plan pln = new Plan
                        {
                            nProduct = prd,
                            nRecipe = rc,
                            rQuantity = mQuantity,
                            tDate = mDate
                        };
                        dbContext.Plans.Add(pln);
                    }
                    dbContext.SaveChanges();
                    mSuccess = true;
                    return "План успешно создан/отредактирован";
                }
                catch (Exception ex)
                {
                    mSuccess = false;
                    return "Что-то пошло не так.." + ex.Message.ToString();
                }
            }
        }

        internal DataTable GetPlans(DateTime dateFrom, DateTime dateTill)
        {
            using (CookBookModel dbContext = Context)
            {
                DataTable dt = new DataTable();

                dbContext.Database.Connection.Open();
                var con = (SqlConnection)dbContext.Database.Connection;
                var query = "select * from vwPlanList where  CONVERT(date, tdate) between '" +
                  dateFrom.ToShortDateString() + "' and '" +
                  dateTill.ToShortDateString() + "'";
                var cmd = new SqlCommand(query, con);
                using (var rdr = cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                }
                return dt;
            }
        }

        public string SaveMaterial(
            ref int productKey,
            string productName,
            double protein,
            double fat,
            double carbohydrates,
            double energy,
            int groupKey,
            int? defaultMeasureKey,
            ref bool mSuccess)
        {
            int pKey = productKey;

            using (CookBookModel dbContext = Context)
            {
                try
                {
                    if (pKey == 0)
                    {
                        if (dbContext.Products.ToList().Exists(x => x.szMaterialName == productName))
                        {
                            throw new InvalidOperationException(
                                "Ошибка создания продукта\nПродукт с таким именем уже существует");
                        }

                        var grp = dbContext.MaterialGroups.FirstOrDefault(x => x.nKey == groupKey);
                        var pr = new Product
                        {
                            szMaterialName = productName,
                            rCarbohydrate = carbohydrates,
                            rEnergy = energy,
                            rFat = fat,
                            rProtein = protein,
                            nMaterialGroup = grp
                        };
                        dbContext.Products.Add(pr);
                        dbContext.SaveChanges();

                        if (defaultMeasureKey != null)
                        {
                            dbContext.MeasureProductRelations.Add(
                                new MeasureProductRelation
                                {
                                    nMeasure = dbContext.Measures.FirstOrDefault(m => m.nKey == defaultMeasureKey.Value),
                                    nProduct = pr,
                                    bIsDefault = true
                                });
                        }
                        dbContext.SaveChanges();
                        productKey = pr.nKey;

                    }
                    else
                    {
                        if (dbContext.Products.ToList().Exists(x => x.szMaterialName == productName && x.nKey != pKey))
                        {
                            throw new InvalidOperationException(
                                "Ошибка редактирования продукта\nПродукт с таким именем уже существует");
                        }

                        Product mt = dbContext.Products.FirstOrDefault(x => x.nKey == pKey);
                        var grp = dbContext.MaterialGroups.FirstOrDefault(x => x.nKey == groupKey);

                        mt.rCarbohydrate = carbohydrates;
                        mt.rEnergy = energy;
                        mt.rFat = fat;
                        mt.rProtein = protein;
                        mt.szMaterialName = productName;

                        //Проверка связи материала и ЕИ
                        var mpr = dbContext.MeasureProductRelations.
                            FirstOrDefault(
                            x => x.nProduct.nKey == pKey
                            && x.bIsDefault
                            && x.nMeasure.nKey == defaultMeasureKey);

                        //Если есть такая же, то ничего не менять
                        //Иначе следующий блок
                        if (mpr == null)
                        {
                            mpr = dbContext.MeasureProductRelations.
                           FirstOrDefault(
                           x => x.nProduct.nKey == pKey
                           && x.bIsDefault
                           && x.nMeasure.nKey != defaultMeasureKey);

                            var oldRelations = dbContext.MeasureProductRelations.
                                Where(x => x.nProduct.nKey == pKey
                                && !x.bIsDefault);
                            dbContext.MeasureProductRelations.RemoveRange(oldRelations);

                            if (mpr != null)
                            {
                                mpr.nMeasure = dbContext.Measures.
                                    FirstOrDefault(m => m.nKey == defaultMeasureKey);
                            }
                            else
                            {
                                dbContext.MeasureProductRelations.Add(
                                    new MeasureProductRelation
                                    {
                                        nMeasure = dbContext.Measures.
                                        FirstOrDefault(m => m.nKey == defaultMeasureKey.Value),
                                        nProduct = mt,
                                        bIsDefault = true
                                    });
                            }
                        }

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

        internal string SaveIngredient(
            int recipeKey,
            int ingredientKey,
            int productKey,
            int measureKey,
            double ingredientQuantity,
            ref bool mSuccess)
        {
            using (CookBookModel dbContext = Context)
            {
                try
                {
                    var mpr = dbContext.MeasureProductRelations.
                        Include(x => x.nProduct).
                        Include(x => x.nMeasure).
                        FirstOrDefault(
                        x => x.nProduct.nKey == productKey && x.nMeasure.nKey == measureKey);

                    if (ingredientKey != 0)
                    {
                        var ing = dbContext.Ingredients.
                            FirstOrDefault(x => x.nKey == ingredientKey);
                        ing.nMeasureProduct = mpr;
                        ing.rQuantity = ingredientQuantity;
                    }
                    else
                    {
                        Recipe rc = dbContext.Recipes.FirstOrDefault(x => x.nKey == recipeKey);
                        Ingredient ing = new Ingredient
                        {
                            nMeasureProduct = mpr,
                            rQuantity = ingredientQuantity,
                            nRecipe = rc
                        };
                        dbContext.Ingredients.Add(ing);
                    }
                    dbContext.SaveChanges();
                    mSuccess = true;
                    return "Ингредиент успешно создан/отредактирован";
                }
                catch (Exception ex)
                {
                    mSuccess = false;
                    return "Что-то пошло не так.." + ex.Message.ToString();
                }
            }
        }

        internal string DeleteIngredient(int mIngredientKey, ref bool mSuccess)
        {

            using (CookBookModel dbContext = Context)
            {
                try
                {
                    var ing = dbContext.Ingredients.
                        FirstOrDefault(x => x.nKey == mIngredientKey);

                    dbContext.Ingredients.Remove(ing);
                    dbContext.SaveChanges();
                    mSuccess = true;
                    return "Ингредиент успешно удален";
                }
                catch (Exception ex)
                {
                    mSuccess = false;
                    return "Что-то пошло не так.." + ex.Message.ToString();
                }
            }
        }

        internal string DeleteRecipe(int pRecipeKey, ref bool mSuccess)
        {
            using (CookBookModel dbContext = Context)
            {
                try
                {
                    var ings = dbContext.Ingredients.
                         Include(x => x.nRecipe).
                         Where(x => x.nRecipe.nKey == pRecipeKey).ToList();
                    dbContext.Ingredients.RemoveRange(ings);

                    var rc = dbContext.Recipes.FirstOrDefault(x => x.nKey == pRecipeKey);
                    dbContext.Recipes.Remove(rc);
                    dbContext.SaveChanges();

                    mSuccess = true;
                    return "Рецепт успешно удален";
                }
                catch (Exception ex)
                {
                    mSuccess = false;
                    return "Что-то пошло не так...\n" + ex.ToString();
                }
            }
        }

        internal DataView GetIngredients(int pRecipeKey)
        {
            using (CookBookModel dbContext = Context)
            {
                DataTable dt = new DataTable();

                dbContext.Database.Connection.Open();
                var con = (SqlConnection)dbContext.Database.Connection;
                var cmd = new SqlCommand("exec sp_SelectIngredients " +
                    "  @pRecipeKey = " + pRecipeKey, con);
                using (var rdr = cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                }
                return dt.AsDataView();
            }
        }

        internal DataView GetProducts(int recipeKey, int mIngredientKey)
        {
            using (CookBookModel dbContext = Context)
            {
                DataTable dt = new DataTable();

                dbContext.Database.Connection.Open();
                var con = (SqlConnection)dbContext.Database.Connection;
                var cmd = new SqlCommand("exec sp_SelectProducts " +
                    "  @pRecipeKey = " + recipeKey + ", " +
                    "  @pIngredientKey = " + mIngredientKey, con);
                using (var rdr = cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                }
                return dt.AsDataView();
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

                    var ing = dbContext.Ingredients.Include(m => m.nMeasureProduct)
                        .Include(m => m.nMeasureProduct.nProduct)
                        .Where(m => m.nMeasureProduct.nProduct.nKey == productKey);
                    dbContext.Ingredients.RemoveRange(ing);

                    var mrp = dbContext.MeasureProductRelations.Include(m => m.nProduct)
                        .Where(m => m.nProduct.nKey == productKey);
                    dbContext.MeasureProductRelations.RemoveRange(mrp);

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
            try
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
            catch { return null; }
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

        internal string SaveRecipe(
            int recipeKey,
            string recipeName,
            int nProductKey,
            double portion,
            double quantity,
            string description,
            ref bool mSuccess)
        {
            using (CookBookModel dbContext = Context)
            {
                try
                {
                    if (recipeKey == 0)
                    {
                        if (dbContext.Recipes.ToList().Exists(x => x.szRecipeName == recipeName))
                        {
                            throw new InvalidOperationException(
                                "Ошибка создания рецепта\nРецепт с таким именем уже существует");
                        }

                        var pr = dbContext.Products.FirstOrDefault(x => x.nKey == nProductKey);
                        dbContext.Recipes.Add(
                            new Recipe
                            {
                                nProduct = pr,
                                rPortion = portion,
                                rQuantity = quantity,
                                szDescription = description,
                                szRecipeName = recipeName
                            });
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        if (dbContext.Recipes.ToList().Exists(x => x.szRecipeName == recipeName && x.nKey != recipeKey))
                        {
                            throw new InvalidOperationException(
                                "Ошибка редактирования рецепта\nРецепт с таким именем уже существует");
                        }
                        Recipe rc = dbContext.Recipes.FirstOrDefault(x => x.nKey == recipeKey);

                        var pr = dbContext.Products.FirstOrDefault(x => x.nKey == nProductKey);
                        rc.nProduct = pr;
                        rc.rPortion = portion;
                        rc.rQuantity = quantity;
                        rc.szDescription = description;
                        rc.szRecipeName = recipeName;
                        dbContext.SaveChanges();
                    }
                    mSuccess = true;
                    return "Рецепт успешно создан/отредактирован";
                }
                catch (Exception ex)
                {
                    return "Что-то пошло не так...\n" + ex.ToString();
                }
            }
        }

        internal List<Product> GetOutputProducts()
        {
            using (CookBookModel dbContext = Context)
            {
                try
                {
                    List<Product> pd =
                        dbContext.Products.
                        Include(p => p.nMaterialGroup).
                        Where(p => p.nMaterialGroup.bContainsFinishedProduct).
                        ToList();
                    return pd;
                }
                catch
                {
                    return null;
                }
            }
        }

        internal List<MeasureProductWrapper> GetMeasureRelations(int productKey, ref string mErrorMessage, ref bool mSuccess)
        {
            using (CookBookModel dbContext = Context)
            {
                try
                {
                    List<MeasureProductWrapper> res = new List<MeasureProductWrapper>();

                    var mainMeasure = dbContext.MeasureProductRelations.
                        Include(x => x.nMeasure).
                        Include(x => x.nProduct).
                        FirstOrDefault(x => x.nProduct.nKey == productKey && x.bIsDefault);

                    if (mainMeasure == null)
                    {
                        throw new InvalidOperationException("Введите основные ЕИ");
                    }
                    int mainMeasureKey = mainMeasure.nMeasure.nKey;

                    //Add to result set existing measures 
                    dbContext.MeasureProductRelations.
                        Include(x => x.nMeasure).
                        Include(x => x.nProduct).
                        Where(x => x.nProduct.nKey == productKey && !x.bIsDefault).
                        ToList().ForEach(x =>
                        {
                            res.Add(
                                new MeasureProductWrapper
                                {
                                    IsSaved = true,
                                    CurrentMeasureQuantity = 1,
                                    MainMeasureQuantity = 1 / x.rQuantity,
                                    Proportion = x.rQuantity,
                                    MeasureKey = x.nMeasure.nKey,
                                    MeasureName = x.nMeasure.szMeasureName,
                                    MeasureProductKey = x.nKey,
                                    MainMeasureName = mainMeasure.nMeasure.szMeasureName,
                                    MainMeasureKey = mainMeasureKey,
                                    IsChanged = false

                                });
                        });

                    //Add to result set NOT existing measures 
                    List<int> enteredMeasures = res.Select(x => x.MeasureKey).ToList();
                    dbContext.Measures.
                        Where(x => x.nKey != mainMeasureKey && !enteredMeasures.Contains(x.nKey)).
                        ToList().
                        ForEach(x =>
                        {
                            res.Add(
                                new MeasureProductWrapper
                                {
                                    IsSaved = false,
                                    CurrentMeasureQuantity = null,
                                    MainMeasureQuantity = null,
                                    Proportion = null,
                                    MeasureKey = x.nKey,
                                    MeasureName = x.szMeasureName,
                                    MeasureProductKey = 0,
                                    MainMeasureName = mainMeasure.nMeasure.szMeasureName,
                                    MainMeasureKey = mainMeasureKey,
                                    IsChanged = false
                                });
                        });
                    mSuccess = true;
                    mErrorMessage = null;
                    return res;
                }
                catch (Exception ex)
                {
                    mSuccess = false;
                    mErrorMessage = ex.Message.ToString();
                    return null;
                }

            }
        }
    }

}