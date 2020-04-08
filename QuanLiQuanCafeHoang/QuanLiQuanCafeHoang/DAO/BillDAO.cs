using QuanLiQuanCafeHoang.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCafeHoang.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance 
        { 
            get
            {
                if (instance == null)
                    instance = new BillDAO();
                return BillDAO.instance;
            }
            private set
            {
                BillDAO.instance = value;
            } 
        }
        public BillDAO()
        {

        }
        /// <summary>
        /// Thành công ID Bill
        /// Thất bại -1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetUnCheckBillIDByTableID(int id)
        {
            DataTable  dataTable = DataProvider.Instance.ExcuteQuery("SELECT * FROM Bill WHERE IdTable = "+ id + " AND Status = 0");
            if (dataTable.Rows.Count > 0)
            {
                BillDTO billDTO = new BillDTO(dataTable.Rows[0]);
                return billDTO.Id;
            }
            return -1;
        }
        public void InsertBill(int id)
        {
            DataProvider.Instance.ExcuteNonQuery("EXEC USP_InsertBill @idTable", new object[] { id });
        }
        public int GetMaxIdBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExcuteScalar("SELECT MAX(id) FROM dbo.Bill");
            }
            catch
            {
                return 1;
            }
            

        }
        public void CheckOut(int id,int discount,float totalPrice)
        {
            string query = "UPDATE dbo.Bill SET Status = 1, TotalPrice = "+ totalPrice +" , " + "discount = "+ discount +" WHERE id = " + id;
            DataProvider.Instance.ExcuteNonQuery(query);
        }
        public DataTable GetListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExcuteQuery("EXEC USP_GetListBillByDate @dateCheckIn , @dateCheckOut",new object[]{checkIn,checkOut});
        }
        
    }
}
