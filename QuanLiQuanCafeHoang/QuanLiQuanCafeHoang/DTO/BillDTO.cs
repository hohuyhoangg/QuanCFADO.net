using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCafeHoang.DTO
{
    public class BillDTO
    {
        private int id;
        private DateTime? dateCheckOut;
        private DateTime? dateCheckIn;
        private int status;
        private int discount;
        private float totalPrice;
        public int Id { get => id; set => id = value; }
        public DateTime? DateCheckOut { get => dateCheckOut; set => dateCheckOut = value; }
        public DateTime? DateCheckIn { get => dateCheckIn; set => dateCheckIn = value; }
        public int Status { get => status; set => status = value; }
        public int Discount { get => discount; set => discount = value; }
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }

        public BillDTO(DataRow dataRow) 
        {
            this.Id = (int)dataRow["id"];
            var dateCheckOutTemp = dataRow["dateCheckOut"];
            if (dateCheckOutTemp.ToString() != "")
                this.DateCheckOut = (DateTime?)dateCheckOutTemp;
            this.DateCheckIn = (DateTime?)dataRow["dateCheckIn"];
            this.Status = (int)dataRow["status"];
            if(dataRow["discount"].ToString()!="")
                this.Discount = (int)dataRow["discount"];
            if (dataRow["TotalPrice"].ToString() != "")
                this.TotalPrice = (float)Convert.ToDouble(dataRow["TotalPrice"]);
        }

        public BillDTO(int id, DateTime? dateCheckOut, DateTime? dateCheckIn, int status,int discount=0,float totalPrice=0)
        {
            this.Id = id;
            this.DateCheckOut = dateCheckOut;
            this.DateCheckIn = dateCheckIn;
            this.Status = status;
            this.Discount = discount;
            this.TotalPrice = totalPrice;
        }

        public BillDTO()
        {

        }
    }
}
