using QuanLiQuanCafeHoang.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCafeHoang.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance 
        { 
            get { if (instance == null) instance = new TableDAO(); return instance; }
            private set { instance = value; } 
        }
        private TableDAO() { }
        public List<TableDTO> LoadTableList()
        {
            List<TableDTO> tableDTOs = new List<TableDTO>();

            DataTable dataTable = DataProvider.Instance.ExcuteQuery("USP_GetTableList");
            foreach (DataRow item in dataTable.Rows)
            {
                TableDTO tableDTO = new TableDTO(item);
                tableDTOs.Add(tableDTO);
            }

            return tableDTOs;
        }
        public void SwitchTable(int id1, int id2)
        {
            string query = "USP_SwitchTable @idTable1 , @idTable2 ";
            DataProvider.Instance.ExcuteQuery(query,new object[]{id1,id2});
        }
        public bool InsertTable(string name)
        {
            string query = string.Format("INSERT INTO dbo.TableFood (Name, Status) VALUES (N'{0}', N'{1}')", name, "Trống");
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateTable(int id,string name)
        {
            string query = string.Format("UPDATE dbo.TableFood SET Name = N'{0}' WHERE id = {1}",name,id );
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteTable(int id)
        {
            string query = string.Format("DELETE dbo.TableFood  WHERE id = {0}", id);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
    }
}
