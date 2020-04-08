using QuanLiQuanCafeHoang.DAO;
using QuanLiQuanCafeHoang.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiQuanCafeHoang
{
    public partial class fAdmin : Form
    {
        private AccountDTO loginAccountDTO;
        public AccountDTO LoginAccountDTO
        {
            get => loginAccountDTO;
            set
            {
                this.loginAccountDTO = value;
            }
        }
        BindingSource bindingSourceFood = new BindingSource();
        BindingSource bindingSourceCategory = new BindingSource();
        BindingSource bindingSourceTable = new BindingSource();
        BindingSource bindingSourceAccount = new BindingSource();
        public fAdmin( AccountDTO accountDTO)
        {
            
            InitializeComponent();
            this.loginAccountDTO = accountDTO;
            Load();
        }
        #region Test đổ dữ liệu
        //void LoadAccountList()
        //{
        //    string query = "select Username as [Tên tài khoản] from Account";
        //    dataGridViewAccount.DataSource = DataProvider.Instance.ExcuteQuery(query);
        //    dataGridViewAccount.DataSource = test.ExcuteQuery(query);// dùng static test bình thường
        //}
        //void LoadFoodtList()
        //{
        //    string query = "select * from Food";
        //    dataGridViewFood.DataSource = DataProvider.Instance.ExcuteQuery(query);
        //}
        #endregion

        #region Event
        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dateFromBill.Value, dateEndBill.Value);

        }
        private void btSeeFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewFood.SelectedCells.Count > 0)
                {
                    int id = (int)dataGridViewFood.SelectedCells[0].OwningRow.Cells["Column3"].Value;
                    CategoryDTO categoryDTO = CategoryDAO.Instance.GetCategoryById(id);
                    int index = -1;
                    int i = 0;
                    foreach (CategoryDTO item in cbFoodCategory.Items)
                    {
                        if (item.Id == categoryDTO.Id)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch
            {

            }
            
        }
        private void btAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int id = (cbFoodCategory.SelectedItem as CategoryDTO).Id;
            float price = (float)numericUpDownFoodPrice.Value;
            if (FoodDAO.Instance.InsertFood(name, id, price))
            {
                MessageBox.Show("Đã thêm thành công");
                LoadListFood();
                if (insert != null)
                    insert(this, new EventArgs());
            }
            else
                MessageBox.Show("Lỗi");
        }
        private void btEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int id = (cbFoodCategory.SelectedItem as CategoryDTO).Id;
            float price = (float)numericUpDownFoodPrice.Value;
            int idFood =Int32.Parse( txbFoodID.Text);
            if (FoodDAO.Instance.UpdateFood(idFood,name, id, price))
            {
                MessageBox.Show("Đã Update thành công");
                LoadListFood();
                if (update != null)
                    update(this, new EventArgs());
            }
            else
                MessageBox.Show("Lỗi");
        }
        private void btDeleteFood_Click(object sender, EventArgs e)
        {
            int idFood =Int32.Parse(txbFoodID.Text);
            if (FoodDAO.Instance.DeleteFood(idFood))
            {
                MessageBox.Show("Đã xóa thành công");
                LoadListFood();
                if (delete != null)
                    delete(this, new EventArgs());
            }
            else
                MessageBox.Show("Lỗi");
        }
        /// <summary>
        /// Tạo event
        /// </summary>
        /// 
        private event EventHandler insert;
        public event EventHandler Insert {
            add { insert += value; }
            remove { insert = value; }
        }
        private event EventHandler update;
        public event EventHandler Update
        {
            add { update += value; }
            remove { update = value; }
        }
        private event EventHandler delete;
        public event EventHandler Delete
        {
            add { delete += value; }
            remove { delete= value; }
        }
        private void btSearchFood_Click(object sender, EventArgs e)
        {
            bindingSourceFood.DataSource = SearchFood(txbSerchFoodName.Text);
        }
        private void btCategoryShow_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }

        private void btCategoryAdd_Click(object sender, EventArgs e)
        {
            string name = txbCategoryNamee.Text;
            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Đã thêm thành công");
                LoadListCategory();
                LoadCategoryIntoComboBox(cbFoodCategory);
                if (insert != null)
                    insert(this, new EventArgs());
            }
            else
                MessageBox.Show("Lỗi");
        }

        private void btCategoryDelete_Click(object sender, EventArgs e)
        {
            int id= Int32.Parse(txbCategoryId.Text);
            if (CategoryDAO.Instance.DeletetCategory(id))
            {
                MessageBox.Show("Đã xóa thành công");
                LoadListCategory();
                LoadCategoryIntoComboBox(cbFoodCategory);
                if (delete != null)
                    delete(this, new EventArgs());
            }
            else
                MessageBox.Show("Lỗi");
        }

        private void btCategoryEdit_Click(object sender, EventArgs e)
        {
            string name = txbCategoryNamee.Text;
            int id = Int32.Parse(txbCategoryId.Text);
            if (CategoryDAO.Instance.UpdateCategory(name, id))
            {
                MessageBox.Show("Đã Update thành công");
                LoadListCategory();
                LoadCategoryIntoComboBox(cbFoodCategory);
                if (update != null)
                    update(this, new EventArgs());
            }
            else
                MessageBox.Show("Lỗi");
        }
        private void btTableShow_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void btTableAdd_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;
            if (TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Đã thêm thành công");
                LoadListTable();
                if (insert != null)
                    insert(this, new EventArgs());
            }
            else
                MessageBox.Show("Lỗi");
        }

        private void btTableDelete_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txbTableId.Text);
            if (TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Đã xóa thành công");
                LoadListTable();
                if (delete != null)
                    delete(this, new EventArgs());
            }
            else
                MessageBox.Show("Lỗi");
        }

        private void btTableEdit_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(txbTableId.Text);
            string name = txbTableName.Text;
            if (TableDAO.Instance.UpdateTable(id,name))
            {
                MessageBox.Show("Đã sửa thành công");
                LoadListTable();
                if (update != null)
                    update(this, new EventArgs());
            }
            else
                MessageBox.Show("Lỗi");
        }
        private void btAccountShow_Click(object sender, EventArgs e)
        {
            LoadListAccount();
        }

        private void btAccountEdit_Click(object sender, EventArgs e)
        {
            string username = txbAccountUsername.Text;
            string displayname = txbAccountDisplayname.Text;
            int type = (int)numericUpDownType.Value;
            if (AccountDAO.Instance.UpdateAccount(username,displayname, type))
            {
                MessageBox.Show("Đã sửa thành công");
                LoadListAccount();
            }
            else
                MessageBox.Show("Lỗi");
        }

        private void btAccountDelete_Click(object sender, EventArgs e)
        {
            string username = txbAccountUsername.Text;
            if (loginAccountDTO.Username.Equals(username))
            {
                MessageBox.Show("Tài khoản đang Login không thể xóa!");
            }
            else
            {
                if (AccountDAO.Instance.DeleteAccount(username))
                {
                    MessageBox.Show("Đã sửa thành công");
                    LoadListAccount();
                }
                else
                    MessageBox.Show("Lỗi");
            }
        }

        private void btAccountAdd_Click(object sender, EventArgs e)
        {
            string username = txbAccountUsername.Text;
            string displayname = txbAccountDisplayname.Text;
            int type = (int)numericUpDownType.Value;
            if (AccountDAO.Instance.InsertAccount(username, displayname, type))
            {
                MessageBox.Show("Đã thêm thành công");
                LoadListAccount();
            }
            else
                MessageBox.Show("Lỗi");
        }
        private void btResetPasswword_Click(object sender, EventArgs e)
        {
            string username = txbAccountUsername.Text;
            if (AccountDAO.Instance.ResetPass(username))
            {
                MessageBox.Show("Đã sửa thành công");
                LoadListAccount();
            }
            else
                MessageBox.Show("Lỗi");        }
        #endregion
        #region Methods
        void Load()
        {
            LoadListFood();
            LoadListCategory();
            LoadListTable();
            LoadListAccount();
            dataGridViewFood.DataSource = bindingSourceFood;
            dataGridViewCategory.DataSource = bindingSourceCategory;
            dataGridViewTable.DataSource = bindingSourceTable;
            dataGridViewAccount.DataSource = bindingSourceAccount;
            LoadDateTimePicker();
            LoadListBillByDate(dateFromBill.Value, dateEndBill.Value);
            LoadCategoryIntoComboBox(cbFoodCategory);
            AddFoodBindings();
            AddCategoryBindings();
            AddTableBindings();
            AddAccountBindings();
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dataGridViewBill.DataSource = BillDAO.Instance.GetListBillByDate(checkIn, checkOut);
            int  Temp = dataGridViewBill.Rows.Count;
            double result = 0;
            for (int i = 0; i < Temp-1; i++)
            {
                result += double.Parse(dataGridViewBill.Rows[i].Cells[1].Value.ToString());
               // result += double.Parse(dataGridViewBill.Rows[i].Cells["Tổng tiền"].Value.ToString());
            }
            tbResult.Text = result.ToString();
        }
        void LoadDateTimePicker()
        {
            DateTime today = DateTime.Now;
            dateFromBill.Value = new DateTime(today.Year, today.Month, 1);
            dateEndBill.Value = dateFromBill.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListFood()
        {
            bindingSourceFood.DataSource = FoodDAO.Instance.GetListFood();
        }
        void LoadCategoryIntoComboBox(ComboBox comboBox)
        {
            List<CategoryDTO> categoryDTOs = CategoryDAO.Instance.GetListCategory();
            comboBox.DataSource = categoryDTOs;
            comboBox.DisplayMember = "Name";
        }
        void AddFoodBindings()
        {
            txbFoodID.DataBindings.Add(new Binding("Text", dataGridViewFood.DataSource,"Id",true,DataSourceUpdateMode.Never));
            txbFoodName.DataBindings.Add(new Binding("Text", dataGridViewFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            numericUpDownFoodPrice.DataBindings.Add(new Binding("Value", dataGridViewFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        List<FoodDTO> SearchFood(string name)
        {
            List<FoodDTO> foodDTOs = new List<FoodDTO>();
            foodDTOs = FoodDAO.Instance.SearchFoodByName(name);
            return foodDTOs;
        }
        void LoadListCategory()
        {
            bindingSourceCategory.DataSource = CategoryDAO.Instance.GetListCategory();
        }
        void AddCategoryBindings()
        {
            txbCategoryId.DataBindings.Add(new Binding("Text", dataGridViewCategory.DataSource, "Id", true, DataSourceUpdateMode.Never));
            txbCategoryNamee.DataBindings.Add(new Binding("Text", dataGridViewCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }
        void LoadListTable()
        {
            bindingSourceTable.DataSource = TableDAO.Instance.LoadTableList();
        }
        void AddTableBindings()
        {
            txbTableId.DataBindings.Add(new Binding("Text", dataGridViewTable.DataSource, "Id", true, DataSourceUpdateMode.Never));
            txbTableName.DataBindings.Add(new Binding("Text", dataGridViewTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbTableStatus.DataBindings.Add(new Binding("Text", dataGridViewTable.DataSource, "Status", true, DataSourceUpdateMode.Never));
        }
        void LoadListAccount()
        {
            bindingSourceAccount.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void AddAccountBindings()
        {
            txbAccountUsername.DataBindings.Add(new Binding("Text", dataGridViewAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbAccountDisplayname.DataBindings.Add(new Binding("Text", dataGridViewAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            numericUpDownType.DataBindings.Add(new Binding("Value", dataGridViewAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }



        #endregion

    }
}
