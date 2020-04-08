using QuanLiQuanCafeHoang.DAO;
using QuanLiQuanCafeHoang.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiQuanCafeHoang
{
    public partial class fTableManager : Form
    {
        private AccountDTO loginAccountDTO;
        public AccountDTO LoginAccountDTO
        {
            get => loginAccountDTO;
            set
            {
                this.loginAccountDTO = value;
                ChangeAccount(loginAccountDTO.Type);
            }
        }
        public fTableManager(AccountDTO accountDTO)
        {
            InitializeComponent();
            this.LoginAccountDTO = accountDTO;
            LoadTable();
            LoadCategory();
            LoadComboBoxTable(cbSwitchTable);
        }
        #region Biến
        public bool IsSelectedTable = false;

        
        #endregion
        #region Events
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccountDTO);
            f.UpdateAccount += F_UpdateAccount;
            f.ShowDialog();
        }

        private void F_UpdateAccount(object sender, AccountEvent e)
        {
            tooltipThongtin.Text = "Thông tin tài khoản (" + e.AccountDTO.Displayname + ")";
            thôngTinCáNhânToolStripMenuItem.Text = " Thông tin cá nhân (" + e.AccountDTO.Displayname + ")";
        }

        private void aDMINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin(loginAccountDTO);
            f.Update += F_Update;
            f.Insert+= F_Insert;
            f.Delete += F_Delete;
            f.ShowDialog();
        }

        private void F_Delete(object sender, EventArgs e)
        {
            LoadTable();
            LoadCategory();
            LoadFoodListByCategoryId((cbCategory.SelectedItem as CategoryDTO).Id);
            if (lwBill.Tag != null)
                ShowBill((lwBill.Tag as TableDTO).Id);
            
        }

        private void F_Insert(object sender, EventArgs e)
        {
            LoadTable();
            LoadCategory();
            LoadFoodListByCategoryId((cbCategory.SelectedItem as CategoryDTO).Id);
            if (lwBill.Tag != null)
                ShowBill((lwBill.Tag as TableDTO).Id);
        }

        private void F_Update(object sender, EventArgs e)
        {
            LoadTable();
            LoadCategory();
            LoadFoodListByCategoryId((cbCategory.SelectedItem as CategoryDTO).Id);
            if(lwBill.Tag!=null)
                ShowBill((lwBill.Tag as TableDTO).Id);

        }

        private void Button_Click(object sender, EventArgs e)
        {
            IsSelectedTable = true;
            int TableID = ((sender as Button).Tag as TableDTO).Id;
            lwBill.Tag = (sender as Button).Tag;
            ShowBill(TableID);
        }
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem == null)
                return;
            CategoryDTO categoryDTO = comboBox.SelectedItem as CategoryDTO;
            id = categoryDTO.Id;
            LoadFoodListByCategoryId(id);
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            if (IsSelectedTable == true)
            {
                TableDTO tableDTO = lwBill.Tag as TableDTO;
                int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(tableDTO.Id);
                int idFood = (cbFood.SelectedItem as FoodDTO).Id;
                int count = (int)nmFoodCount.Value;
                if (idBill == -1)
                {
                    BillDAO.Instance.InsertBill(tableDTO.Id);
                    BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIdBill(), idFood, count);

                }
                else
                {
                    BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count);
                }
                ShowBill(tableDTO.Id);
                LoadTable();
            }
            else
                MessageBox.Show("Bạn chưa chọn bàn!","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (IsSelectedTable == true)
            {
                TableDTO tableDTO = lwBill.Tag as TableDTO;
                int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(tableDTO.Id);
                int discount = (int)nmDiscount.Value;
                double totalPrice = Convert.ToDouble(tbtotalPrice.Text.Split(',')[0]);
                double finalPrice = totalPrice * (100 - discount)/100;
                if (idBill != -1)
                {
                    if (MessageBox.Show(string.Format("Bạn có muốn thanh toán {0} không?\n Tổng tiền: {1} \n Giảm giá: {2}% \n Thành tiền: {3}",
                        tableDTO.Name,totalPrice,discount,finalPrice),
                        "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        BillDAO.Instance.CheckOut(idBill,discount, (float)finalPrice);
                        ShowBill(tableDTO.Id);
                        LoadTable();
                    }
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn bàn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            int id1 = (lwBill.Tag as TableDTO).Id;
            int id2 = (cbSwitchTable.SelectedItem as TableDTO).Id;
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển {0} sang {1}", (lwBill.Tag as TableDTO).Name, 
                (cbSwitchTable.SelectedItem as TableDTO).Name), "Thông báo",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)==DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);
                LoadTable();
            }
        }
        #endregion
        #region Method
        void ChangeAccount(int type)
        {
            if (type == 1)
                tooltipAdmin.Enabled = true;
            else
                tooltipAdmin.Enabled = false;
            tooltipThongtin.Text += " (" + LoginAccountDTO.Displayname + ")";
            thôngTinCáNhânToolStripMenuItem.Text += " (" + LoginAccountDTO.Displayname + ")";
        }
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<TableDTO> tableDTOs = TableDAO.Instance.LoadTableList();
            foreach (TableDTO item in tableDTOs)
            {
                Button button = new Button() { Height = TableDTO.TableHeigh, Width = TableDTO.TableWidth };
                button.Text = item.Name + Environment.NewLine + item.Status;
                button.Click += Button_Click;
                button.Tag = item;
                if (item.Status.Equals("Trống"))
                    button.BackColor = Color.Aqua;
                else
                    button.BackColor = Color.Orange;
                flpTable.Controls.Add(button);
            }
        }
        void ShowBill(int idTable)
        {
            lwBill.Items.Clear();
            List<MenuDTO> menuDTOs = MenuDAO.Instance.GetListMenuByTable(idTable);
            float totalPrice = 0;
            foreach (MenuDTO item in menuDTOs)
            {
                ListViewItem listViewItem = new ListViewItem(item.FoodName.ToString());
                listViewItem.SubItems.Add(item.Price.ToString());
                listViewItem.SubItems.Add(item.Count.ToString());
                listViewItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lwBill.Items.Add(listViewItem);
            }
            // đổi thành tiền tiếng việt
            CultureInfo cultureInfo = new CultureInfo("vi-VN");
            //Thread.CurrentThread.CurrentCulture = cultureInfo;
            //tbtotalPrice.Text = totalPrice.ToString("c", cultureInfo);
            tbtotalPrice.Text = totalPrice.ToString();
        }
        void LoadCategory()
        {
            List<CategoryDTO> categoryDTOs = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = categoryDTOs;
            cbCategory.DisplayMember = "Name";

        }
        void LoadFoodListByCategoryId(int id)
        {
            List<FoodDTO> foodDTOs = FoodDAO.Instance.GetFoodByCategoryId(id);
            cbFood.DataSource = foodDTOs;
            cbFood.DisplayMember = "Name";
        }
        void LoadComboBoxTable( ComboBox comboBox)
        {
            comboBox.DataSource = TableDAO.Instance.LoadTableList();
            comboBox.DisplayMember = "Name";
        }

        #endregion

    }
}
