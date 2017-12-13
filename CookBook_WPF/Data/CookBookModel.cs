namespace CookBook_WPF.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class CookBookModel : DbContext
    {
        // Контекст настроен для использования строки подключения "CookBookModel" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "dbCookBook" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "CookBookModel" 
        // в файле конфигурации приложения.
        public CookBookModel()
            : base("name=CookBookModel")
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }
        public virtual DbSet<Basket> Baskets { get; set; }
        public virtual DbSet<BasketDetail> BasketDetails { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Measure> Measures { get; set; }
        public virtual DbSet<MeasureProductRelation> MeasureProductRelations { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<MaterialGroup> MaterialGroups { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>().HasIndex(x => x.szRecipeName);
            modelBuilder.Entity<Basket>().HasIndex(x => x.szDescription);
            
            base.OnModelCreating(modelBuilder);
        }
    }

}