using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCafeHoang.DTO
{
    public class BillInfoDTO
    {
        private int id;
        private int billId;
        private int foodId;
        private int count;

        public int Id { get => id; set => id = value; }
        public int BillId { get => billId; set => billId = value; }
        public int FoodId { get => foodId; set => foodId = value; }
        public int Count { get => count; set => count = value; }

        public BillInfoDTO(int id, int billId, int foodId, int count)
        {
            this.Id = id;
            this.BillId = billId;
            this.FoodId = foodId;
            this.Count = count;
        }
        public BillInfoDTO()
        {

        }
        public BillInfoDTO(DataRow dataRow)
        {
            this.Id = (int)dataRow["Id"];
            this.BillId = (int)dataRow["IdBill"];
            this.FoodId = (int)dataRow["IdFood"];
            this.Count = (int)dataRow["Count"];
        }
    }
}
