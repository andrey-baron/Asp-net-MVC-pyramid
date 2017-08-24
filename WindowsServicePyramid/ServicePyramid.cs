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
                string pathToChange = "";
                var xmlModel = Load1CDataFromXml.GetXmlModel(out flagErr, ref pathToChange);
                if (xmlModel.Categories.Count() == 0 && xmlModel.Products.Count() == 0)
                {
                    return;
                }
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


                    _categoryRepository.HideCategoryIfNotExistInCurrentUpdateFrom1C(xmlModel.Categories.Select(s => s.Id));
                    var efCatWithParent = xmlModel.Categories.Select(s => new Category1CIdWithParent1CId() { Id = s.Id, ParentId = s.ParentId }).ToList();

                    // var rootCategories = xmlModel.Categories.Where(w => w.ParentId == null);


                    _categoryRepository.UpdateParentCategory(efCatWithParent);


                    var xmlProducts = xmlModel.Products.Select(s => new DBFirstDAL.Products()
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
                    }).ToList();

                    _categoryRepository.DeleteAllFilterBrands();
                    foreach (var item in xmlProducts)
                    {
                            _productRepository.AddOrUpdateFromOneC(item);
                    }


                    _categoryRepository.AddOrUpdateFilterBrand();

                    if (pathToChange != "")
                    {
                        Load1CDataFromXml.ChangePathFile(pathToChange);
                    }

                    _productRepository.SetIsNotUnloading(xmlProducts);
                }
                catch (Exception ex)
                {
                    Logger.Log(LogLevel.Error, ex, $"Ошибка при выгрузке информации на публичный сайт: {ex}");

                    flagErr = true; ;
                }

            }
        }

        //private bool CheckTreeForCyclicity(List<_1CToolModels.CategoryXMLModel> xmlModelGraph)
        //{
        //    //var a = new int[xmlModelGraph.Count];
        //    List<KeyValueSimple<string, int>> cl = xmlModelGraph.Select(i => new KeyValueSimple<string, int>(i.Id, 0) {
        //    }).ToList();

        //    List<KeyValueSimple<string, int>> p = xmlModelGraph.Select(i => new KeyValueSimple<string, int>(i.Id, 0)
        //    {
        //    }).ToList();
        //    string cycle_st;
        //    string cycle_end;
        //}
        //bool dfs(string OneC_id,
        //    ref List<KeyValueSimple<string, int>> cl,
        //    ref int cycle_st,
        //    ref int cycle_end,
        //    ref List<_1CToolModels.CategoryXMLModel> xmlModelGraph)
        //{
        //    var r = cl.First(i => i.Key == OneC_id);
        //    r.Value = 1;
            
        //    for (var i = 0; i < xmlModelGraph.Where(w=>w.); ++i)
        //    {
        //        int to = g[v][i];
        //        if (cl[to] == 0)
        //        {
        //            p[to] = v;
        //            if (dfs(to)) return true;
        //        }
        //        else if (cl[to] == 1)
        //        {
        //            cycle_end = v;
        //            cycle_st = to;
        //            return true;
        //        }
        //    }
        //    cl[v] = 2;
        //    return false;
        //}



        internal void RunAsConsole()
        {
            OnStart(null);
            Console.OpenStandardInput();
            //Console.ReadLine();
            ExecuteLogic();
            OnStop();
        }
    }

    public struct KeyValueSimple<TKey, TValue>
    {

        public KeyValueSimple(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public TKey Key { get; }

        public TValue Value { get; set; }

    }
}
