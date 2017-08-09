using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using WindowsServicePyramid._1CToolModels;

namespace WindowsServicePyramid
{
    public class Load1CDataFromXml
    {
        public static string pathDirectoryFiles =  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["FilesFolder"]);

            /*Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content\\UserUploadXml\\");*/
        public static AllEntity1CXMLModel GetXmlModel(out bool flagError,ref string pathFile)
        {
            List<string> stoplistTitleCategories = new List<string>(new string[] {
                "Сайдинг и водосточная система",
                "Металлоизделия",
                "Плитка",
                "Декоративные материалы",
                "Декоративные покрытия"
            });
            List<CategoryXMLModel> stoplistCategories = new List<CategoryXMLModel>();

            AllEntity1CXMLModel outModel = new AllEntity1CXMLModel();
            try
            {
                XmlDocument xDoc = new XmlDocument();
                var path = GetLastFilePath(pathDirectoryFiles);
                if (path==null)
                {
                    flagError = true;

                    return outModel;
                }
                pathFile = path.ToString();
                xDoc.Load(path);

                var XmlElement = xDoc.DocumentElement;
                XmlNode xProducts = XmlElement.SelectSingleNode("ВсеТовары");
                XmlNode xCategories = XmlElement.SelectSingleNode("ВсеКатегории");

                #region categories
                XmlNodeList categories = xCategories.SelectNodes("Группа");
                CategoryXMLModel categoryModel = new CategoryXMLModel();

                List<CategoryXMLModel> outCategoryModels = GetInnerCategories(categories, null);

                //var stoplistRootCategories=outCategoryModels.Where(i => stoplistTitleCategories.Any(a => a == i.Title)).ToList();

                //stoplistCategories = GetListCategoriesWithChildCategories(stoplistRootCategories, ref outCategoryModels);
                #endregion

                #region products

                List<ProductXMLModel> listProducts = new List<ProductXMLModel>();

                XmlNodeList Products = xProducts.SelectNodes("Товар");
                int testIndx = 0;
                foreach (var product in Products)
                {
                    bool flagAdd = true;
                    XmlNodeList Inners = ((XmlNode)product).SelectNodes("*");
                    ProductXMLModel prodModel = new ProductXMLModel();

                    
                    
                    XmlNode IdNode = ((XmlNode)product).SelectSingleNode("Ид");
                    prodModel.Id = IdNode.InnerText;
                    XmlNode TitleNode = ((XmlNode)product).SelectSingleNode("ПолноеНаименование");
                    prodModel.Title = TitleNode.InnerText;

                    XmlNode Amount = ((XmlNode)product).SelectSingleNode("Количество");
                    
                    var notDisplayed = ((XmlNode)product).SelectSingleNode("ОтображатьНаСайте");
                    if (notDisplayed.InnerText == "Нет")
                    {
                        prodModel.TypeStatusProduct = Common.TypeStatusProduct.Hide;
                    }
                    else
                    {
                        if (Amount.InnerText != "0")
                        {
                            prodModel.TypeStatusProduct = Common.TypeStatusProduct.Normal;
                        }
                        else
                        {
                            prodModel.TypeStatusProduct = Common.TypeStatusProduct.WillBeAdded;

                        }

                    }

                    XmlNode GroupsNode = ((XmlNode)product).SelectSingleNode("Группы");
                    if (GroupsNode==null)
                    {
                        /*continue;*/
                        foreach (var group in GroupsNode)
                        {
                            var idgroup = ((XmlNode)group).InnerText;
                            if (stoplistCategories.Any(a => a.Id == idgroup))
                            {
                                flagAdd = false;
                            }
                            prodModel.CategoryTextIds.Add(((XmlNode)group).InnerText);
                        }
                    }
                    

                    XmlNode Brand = ((XmlNode)product).SelectSingleNode("Бренд");
                    if (Brand!=null)
                    {
                        prodModel.Brand = Brand.InnerText;
                    }
                   

                    XmlNode Priority = ((XmlNode)product).SelectSingleNode("Приоритетный");
                    bool priorityFlag = false;
                    if (Priority.InnerText=="Да")
                    {
                        priorityFlag = true;
                    }
                    //bool.TryParse(Priority.InnerText, out priorityFlag);
                    prodModel.Priority = priorityFlag;

                    XmlNode PriceNode = ((XmlNode)product).SelectSingleNode("Цена");
                    XmlNode PriceForOneNode = ((XmlNode)PriceNode).SelectSingleNode("ЦенаЗаЕдиницу");
                    double price = 0;

                    double.TryParse(PriceForOneNode.InnerText.Replace(" ",""), out price);

                    prodModel.Price = price;
                    prodModel.TypePrice = Common.TypeProductPrice.SimplePrice;
                    if (flagAdd)
                    {
                        listProducts.Add(prodModel);
                        //testIndx++;
                        //if (testIndx>10)
                        //{
                        //    break;
                        //}
                    }

                }
                #endregion

                
                outModel.Products = listProducts;
                outModel.Categories = outCategoryModels;

                
                flagError = false;

                
                return outModel;
            }
            catch (Exception ex)
            {

                flagError = true;
                return outModel;
            }
            
        }

        private static string GetLastFilePath(string dirName)
        {
            string[] files = Directory.GetFiles(dirName);
           var file= files.FirstOrDefault(i => !i.Contains("done"));
            if (file==null)
            {
                return null;
            }
            //var tmp = Path.Combine(dirName, file);
            return Path.Combine(dirName, file);
        }

        public static void ChangePathFile(string pathFile)
        {
            if (File.Exists(pathFile))
            {
                var filename=Path.GetFileName(pathFile);
               
                filename = "done" + DateTime.Now.ToString("g").Replace(" ","").Replace(".", "").Replace(":", "") + filename;
                var newFilePath = Path.Combine(pathDirectoryFiles, filename);
                File.Move(pathFile, newFilePath);
            }
        }

        static List<CategoryXMLModel> GetInnerCategories(XmlNodeList groupsNode,string ParentId)
        {
            List<CategoryXMLModel> listModels = new List<CategoryXMLModel>();
            foreach (var group in groupsNode)
            {
                CategoryXMLModel categoryModel = new CategoryXMLModel();

                XmlNode categoryIdNode = ((XmlNode)group).SelectSingleNode("Ид");
                if (categoryIdNode != null)
                {
                    categoryModel.Id = categoryIdNode.InnerText;
                }
                XmlNode categoryTitleNode = ((XmlNode)group).SelectSingleNode("Наименование");
                if (categoryTitleNode != null)
                {
                    categoryModel.Title = categoryTitleNode.InnerText;
                }

                categoryModel.ParentId = ParentId;

                listModels.Add(categoryModel);

                XmlNode categoriesNode = ((XmlNode)group).SelectSingleNode("Группы");
                if (categoriesNode != null)
                {
                    XmlNodeList innerGroup = ((XmlNode)categoriesNode).SelectNodes("Группа");
                    listModels.AddRange(GetInnerCategories(innerGroup, categoryModel.Id));
                }

                

            }
            return listModels;
        }

        static List<CategoryXMLModel> GetListCategoriesWithChildCategories(List<CategoryXMLModel> groupsList, ref List<CategoryXMLModel> allstock )
        {
            List<CategoryXMLModel> listModels = new List<CategoryXMLModel>();
            foreach (var group in groupsList)
            {
                CategoryXMLModel categoryModel = group;

                listModels.Add(categoryModel);
                //allstock.Remove(categoryModel);
                listModels.AddRange(allstock.Where(w => listModels.Any(a=>a.Id==w.ParentId)));

                allstock.RemoveAll(i=>listModels.Any(a=>a.Id==i.Id));
                //while (categoryModel.ParentId!=null)
                //{
                //    categoryModel = allstock.FirstOrDefault(f => f.Id == categoryModel.ParentId);
                //    if (categoryModel!=null)
                //    {
                //        listModels.Add(categoryModel);
                //        allstock.Remove(categoryModel);
                //    }
                //    else
                //    {
                //        break;
                //    }
                //}
            }

            return listModels;
        }

    }
}