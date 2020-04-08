using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCafeHoang.DTO
{
    public class TableDTO
    {
        private int id;
        private string name;
        private string status;
        public static int TableWidth = 100;
        public static int TableHeigh = 100;
        public string Status { get => status; set => status = value; }
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }

        public TableDTO(int id , string name , string status)
        {
            this.Id = id;
            this.Name = name;
            this.Status = status;
        } 
        public TableDTO (DataRow dataRow)
        {
            this.Id = (int)dataRow["id"];
            this.Name = dataRow["name"].ToString();
            this.Status = dataRow["status"].ToString();
        } 
    }
}
