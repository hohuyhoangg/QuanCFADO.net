using QuanLiQuanCafeHoang.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCafeHoang.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new MenuDAO();
                return MenuDAO.instance;
            }
            private set
            {
                MenuDAO.instance = value;
            }
        }
        public MenuDAO()
        {

        }
        public List<MenuDTO> GetListMenuByTable(int id)
        {
            List<MenuDTO> menuDTOs = new List<MenuDTO>();
            string query = "SELECT f.Name,f.Price,bi.Count,f.Price*bi.Count as totalPrice FROM dbo.BillInfo AS bi, dbo.Bill AS b, dbo.Food AS f " +
                "WHERE bi.IdBill = b.Id AND bi.IdFood = f.Id AND b.IdTable =" + id+ "AND Status = 0";
            DataTable dataTable = DataProvider.Instance.ExcuteQuery(query);
            foreach (DataRow item in dataTable.Rows)
            {
                MenuDTO menuDTO = new MenuDTO(item);
                menuDTOs.Add(menuDTO);
            }
            return menuDTOs;
        }
    }
}
