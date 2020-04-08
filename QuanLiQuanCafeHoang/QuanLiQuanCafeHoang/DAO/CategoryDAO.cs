using QuanLiQuanCafeHoang.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCafeHoang.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance
        {
            get { if (instance == null) instance = new CategoryDAO(); return instance; }
            private set { instance = value; }
        }
        public CategoryDAO()
        {

        }
        public List<CategoryDTO> GetListCategory()
        {
            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();
            string query = "SELECT * FROM dbo.FoodCategory";
            DataTable dataTable = DataProvider.Instance.ExcuteQuery(query);
            foreach (DataRow item in dataTable.Rows)
            {
                CategoryDTO categoryDTO = new CategoryDTO(item);
                categoryDTOs.Add(categoryDTO);
            }
            return categoryDTOs;
        }
        public CategoryDTO GetCategoryById(int id)
        {
            CategoryDTO categoryDTO = new CategoryDTO();
            string query = "SELECT * FROM dbo.FoodCategory where id= "+ id ;
            DataTable dataTable = DataProvider.Instance.ExcuteQuery(query);
            foreach (DataRow item in dataTable.Rows)
            {
                categoryDTO = new CategoryDTO(item);
                return categoryDTO;
            }
            return categoryDTO;
        }
        public bool InsertCategory(string name)
        {
            string query = string.Format("INSERT INTO dbo.FoodCategory( Name) VALUES(   N'{0}' )", name);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateCategory(string name,int id)
        {
            string query = string.Format("UPDATE dbo.FoodCategory SET Name = N'{0}' WHERE Id ={1}", name, id);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool DeletetCategory(int id)
        {
            List<FoodDTO> foodDTOs = FoodDAO.Instance.GetFoodByCategoryId(id);
            string query = "";
            int result = 0;
            if (foodDTOs == null)
            {

                query = string.Format("DELETE dbo.FoodCategory WHERE Id ={0}", id);
                result = DataProvider.Instance.ExcuteNonQuery(query);
                return result > 0;
            }
            if (foodDTOs.Count > 0)
            {
                for (int i = 0; i < foodDTOs.Count; i++)
                {
                    bool flag = FoodDAO.Instance.DeleteFood(foodDTOs[i].Id);
                    if (flag == false)
                        return false;
                }
            }
            query = string.Format("DELETE dbo.FoodCategory WHERE Id ={0}", id);
            result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }

    }
}
