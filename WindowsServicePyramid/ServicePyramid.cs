using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using DBFirstDAL.Repositories;
using DBFirstDAL.DataModels._1C;

namespace WindowsServicePyramid
{
    public partial class ServicePyramid : ServiceBase
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly System.Timers.Timer _timer;
        private readonly object syncRoot = new object();
        private CategoryRepository _categoryRepository;
        private ProductRepository _productRepository;
        static object locker = new object();

        public ServicePyramid()
        {
            InitializeComponent();
            _timer = new System.Timers.Timer(double.Parse(ConfigurationManager.AppSettings["UpdateInterval"]) * 1000)
            {
                AutoReset = bool.Parse(ConfigurationManager.AppSettings["UseTimer"])
            };
            _timer.Elapsed += (sender, args) => ExecuteLogic();
        }

        protected override void OnStart(string[] args)
        {
            _categoryRepository = new CategoryRepository();
            _productRepository = new ProductRepository();
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer.Stop();
        }

        private void ExecuteLogic()
        {
            lock (locker)
            {

                bool flagErr = false;
                var xmlModel = Load1CDataFromXml.GetXmlModel(out flagErr);
                try
                {
                    var efCats = xmlModel.Categories.Select(s => new DBFirstDAL.Categories()
                    {
                        Title = s.Title,
                        OneCId = s.Id,

                    });
                    foreach (var item in efCats)
                    {
                        _categoryRepository.AddOrUpdateFromOneC(item);
                    }


                    var efCatWithParent = xmlModel.Categories.Select(s => new Category1CIdWithParent1CId() { Id = s.Id, ParentId = s.ParentId }).ToList();


                    _categoryRepository.UpdateParentCategory(efCatWithParent);


                    var efProducts = xmlModel.Products.Select(s => new DBFirstDAL.Products()
                    {
                        Title = s.Title,
                        Price = s.Price,
                        DateChange = DateTime.Now,
                        DateCreation = DateTime.Now,
                        OneCId = s.Id,
                        TypePrice = (int)s.TypePrice,
                        IsPriority = s.Priority,
                        IsFilled = false,
                        EnumValues = new List<DBFirstDAL.EnumValues>(new DBFirstDAL.EnumValues[]{ new DBFirstDAL.EnumValues() {
                    Key =s.Brand,
                    TypeValue =(int)Common.TypeFromEnumValue.Brand} }),
                       
                        TypeStatusProduct = (int)s.TypeStatusProduct,
                        Categories = s.CategoryTextIds.Select(i => new DBFirstDAL.Categories() { OneCId = i }).ToList()
                    });

                    _categoryRepository.DeleteAllFilterBrands();
                    foreach (var item in efProducts)
                    {
                        _productRepository.AddOrUpdateFromOneC(item);
                    }


                    _categoryRepository.AddOrUpdateFilterBrand();

                }
                catch (Exception ex)
                {
                    Logger.Log(LogLevel.Error, ex, $"Ошибка при выгрузке информации на публичный сайт: {ex}");

                    flagErr = true; ;
                }

            }
        }


        internal void RunAsConsole()
        {
            OnStart(null);
            Console.OpenStandardInput();
           //Console.ReadLine();
            ExecuteLogic();
            OnStop();
        }
    }
}
