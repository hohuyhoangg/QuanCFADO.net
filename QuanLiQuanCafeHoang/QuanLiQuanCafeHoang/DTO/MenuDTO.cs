using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCafeHoang.DTO
{
    public class MenuDTO
    {
        private string foodName;
        private int count;
        private float price;
        private float totalPrice;

        public string FoodName { get => foodName; set => foodName = value; }
        public int Count { get => count; set => count = value; }
        public float Price { get => price; set => price = value; }
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }
        public MenuDTO() { }

        public MenuDTO(string foodName, int count, float price, float totalPrice = 0)
        {
            this.FoodName = foodName;
            this.Count = count;
            this.Price = price;
            this.TotalPrice = totalPrice;
        }

        public MenuDTO(DataRow dataRow)
        {
            this.FoodName = dataRow["Name"].ToString();
            this.Price = (float)Convert.ToDouble(dataRow["Price"].ToString());
            this.Count = (int)dataRow["Count"];
            this.TotalPrice = (float)Convert.ToDouble(dataRow["totalPrice"].ToString());
        }
    }
}
