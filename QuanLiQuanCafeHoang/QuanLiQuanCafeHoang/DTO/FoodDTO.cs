using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCafeHoang.DTO
{
    public class FoodDTO
    {
        private int id;
        private string name;
        private int categoryId;
        private float price;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int CategoryId { get => categoryId; set => categoryId = value; }
        public float Price { get => price; set => price = value; }
        public FoodDTO()
        {

        }

        public FoodDTO(int id, string name, int categoryId, float price)
        {
            Id = id;
            Name = name;
            CategoryId = categoryId;
            Price = price;
        }
        public FoodDTO(DataRow dataRow)
        {
            Id = (int)dataRow["id"];
            Name = dataRow["name"].ToString();
            CategoryId = (int)dataRow["IdFoodCategory"];
            Price = (float)Convert.ToDouble( dataRow["price"].ToString());
        }
    }
}
