using QuanLiQuanCafeHoang.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCafeHoang.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return instance; }
            private set { instance = value; }
        }
        public FoodDAO()
        {

        }
        public List<FoodDTO> GetFoodByCategoryId(int id)
        {
            List<FoodDTO> foodDTOs = new List<FoodDTO>();
            string query = "SELECT * FROM dbo.Food WHERE IdFoodCategory= "+ id;
            DataTable dataTable = DataProvider.Instance.ExcuteQuery(query);
            foreach (DataRow item in dataTable.Rows)
            {
                FoodDTO foodDTO = new FoodDTO(item);
                foodDTOs.Add(foodDTO);
            }
            if(foodDTOs.Count >0)
                return foodDTOs;
            return null;
        }
        public List<FoodDTO> GetListFood()
        {
            List<FoodDTO> foodDTOs = new List<FoodDTO>();
            string query = "SELECT * FROM dbo.Food";
            DataTable dataTable = DataProvider.Instance.ExcuteQuery(query);
            foreach (DataRow item in dataTable.Rows)
            {
                FoodDTO foodDTO = new FoodDTO(item);
                foodDTOs.Add(foodDTO);
            }
            return foodDTOs;
        }
        public bool InsertFood(string name,int id, float price)
        {
            string query =string.Format( "INSERT INTO dbo.Food( Name,IdFoodCategory,Price) VALUES(   N'{0}', {1}, {2} )",name,id,price);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateFood(int idFood,string name, int id, float price)
        {
            string query = string.Format("UPDATE dbo.Food SET Name = N'{0}',IdFoodCategory = {1}, Price = {2} WHERE Id ={3}", name, id, price,idFood);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteFood(int idFood)
        {
            BillInfoDAO.Instance.DeleteBillInfoByFoodId(idFood);
            string query = string.Format("DELETE Food WHERE Id ={0}", idFood);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public List<FoodDTO> SearchFoodByName(string name)
        {
            List<FoodDTO> foodDTOs = new List<FoodDTO>();
            string query = string.Format("SELECT * FROM dbo.Food WHERE dbo.fuConvertToUnsign1(name) " +
                "LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name);
            DataTable dataTable = DataProvider.Instance.ExcuteQuery(query);
            foreach (DataRow item in dataTable.Rows)
            {
                FoodDTO foodDTO = new FoodDTO(item);
                foodDTOs.Add(foodDTO);
            }
            return foodDTOs;
        }
    }
}
