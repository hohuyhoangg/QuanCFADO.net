using QuanLiQuanCafeHoang.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCafeHoang.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        { 
            get { if (instance == null) instance = new BillInfoDAO(); return instance; }
            private set { instance = value; }
        }
        public BillInfoDAO()
        {

        }
        public List<BillInfoDTO> GetListBillInfo(int id)
        {
            List<BillInfoDTO> billInfoDTOs = new List<BillInfoDTO>();

            DataTable dataTable = DataProvider.Instance.ExcuteQuery("SELECT * FROM BillInfo WHERE IdBill = "+ id);
            foreach (DataRow item in dataTable.Rows)
            {
                BillInfoDTO billInfoDTO = new BillInfoDTO(item);
                billInfoDTOs.Add(billInfoDTO);
            }
            return billInfoDTOs;
        }
        public void InsertBillInfo(int idBill,int idFood, int count)
        {
            DataProvider.Instance.ExcuteNonQuery("EXEC USP_InsertBillInfo @idBill , @idFood , @count", new object[] { idBill,idFood,count });
        }
        public void DeleteBillInfoByFoodId(int id)
        {
            DataProvider.Instance.ExcuteQuery("DELETE BillInfo WHERE IdFood = " + id);
        }
    }
}
