using Pyramid.Tools._1CToolModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace Pyramid.Tools
{
    public class Load1CDataFromXml
    {
        public static string pathDirectoryFiles = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content\\UserUploadXml\\");
        public static AllEntity1CXMLModel GetXmlModel(out bool flagError)
        {
            AllEntity1CXMLModel outModel = new AllEntity1CXMLModel();
            try
            {
                XmlDocument xDoc = new XmlDocument();
                var path = GetLastFilePath(pathDirectoryFiles);
                xDoc.Load(path);
                var XmlElement = xDoc.DocumentElement;
                XmlNode xProducts = XmlElement.SelectSingleNode("Товары");
                XmlNode xCategories = XmlElement.SelectSingleNode("Группы");
                #region products

                List<ProductXMLModel> listProducts = new List<ProductXMLModel>();

                XmlNodeList Products = xProducts.SelectNodes("Товар");
                foreach (var product in Products)
                {
                    XmlNodeList Inners = ((XmlNode)product).SelectNodes("*");
                    ProductXMLModel prodModel = new ProductXMLModel();

                    foreach (var productItem in Inners)
                    {
                        XmlNode innerItem = ((XmlNode)productItem);
                        switch (innerItem.Name)
                        {

                            case "Ид":
                                prodModel.Id = innerItem.InnerText;
                                break;
                            case "Наименование":
                                prodModel.Title = innerItem.InnerText;
                                break;
                            case "Группы":
                                XmlNodeList groups = ((XmlNode)innerItem).SelectNodes("Ид");
                                foreach (var group in groups)
                                {
                                    prodModel.CategoryTextIds.Add(((XmlNode)group).InnerText);
                                }
                                break;
                            case "Бренд":
                                prodModel.Brand = innerItem.InnerText;
                                break;
                            case "Цена":
                                XmlNode price = ((XmlNode)innerItem).SelectSingleNode("ЦенаЗаЕдиницу");

                                prodModel.Price = price.InnerText;
                                break;
                            default:
                                break;
                        }
                    }
                    listProducts.Add(prodModel);

                }
                #endregion

                #region categories
                XmlNodeList categories = xCategories.SelectNodes("Группа");
                CategoryXMLModel categoryModel = new CategoryXMLModel();

                List<CategoryXMLModel> outCategoryModels = GetInnerCategories(categories,null);


                #endregion

                outModel.Products = listProducts;
                outModel.Categories = outCategoryModels;
                flagError = false;

                
                return outModel;
            }
            catch (Exception)
            {
                flagError = true;
                return outModel;
            }
            
        }

        private static string GetLastFilePath(string dirName)
        {
            string[] files = Directory.GetFiles(dirName);
           var file= files.FirstOrDefault(i => !i.Contains("done"));
            var tmp = Path.Combine(dirName, file);
            return Path.Combine(dirName, file);
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

    }
}