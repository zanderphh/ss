﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using 销售管理.DAL.DataSetUsersTableAdapters;
using System.Data.SqlClient;

namespace 销售管理.日常业务
{
    public partial class 费用分配审核_商务 : 销售管理.UserControlBase
    {
        public 费用分配审核_商务()
        {
            InitializeComponent();
        }

        private void 费用分配审核_商务_Load(object sender, EventArgs e)
        {
            dgvExAllocation.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(dgvExAllocation, true, null);
            cmbUserName.DataSource = new T_UsersTableAdapter().GetSalers();
            cmbUserName.DisplayMember = "UserName";
            cmbUserName.SelectedIndex = -1;
            cmbMoney.SelectedIndex = 0;
            cmbHasAudit.SelectedIndex = 1;
            cmbStatus.SelectedIndex = 0;

        }

        private void btnSerch_Click(object sender, EventArgs e)
        {
            string mSql;
            SqlConnection conn = new SqlConnection(global::Common.CommonClass.SqlConnStr);
            mSql = @"SELECT a.Id, a.Month, b.UserName, a.TableNo, d.CompanyName as CustomerName, a.ProjectName, p.name as ProductName, pt.name ProductType, a.Amount, a.DeliverPrice, a.DeliverSum, a.SalePrice, a.SaleSum, a.DepartSum, a.SaleWages, a.CommissionPrice, a.CommissionSum, a.SaleComission, a.AgentPrice, a.AgentSum, a.AgentCommission, a.IsPaid, a.PaidDate, a.Status, c.UserName as businessAudit, a.BusinessDate, a.BusinessRemark, a.FinanceAudit, a.FinanceDate, a.FinanceRemark, a.LeaderAudit, a.LeaderDate, a.LeaderRemark, a.RecDate, a.SaleDetailsId,cs.username citysaler,a.citysum,a.citywages,a.citysaleprice,a.citysalesum,a.citysalecommission FROM T_ExpenseAllocation a left join t_users b on a.username = b.id left join t_users c on a.businessAudit = c.id left join t_customers d on a.customername = d.id left join t_products p on a.productname = p.id left join t_products pt on p.parentid= pt.id left join t_users cs on cs.id = a.CitySaler where 1=1 ";


            if (cmbHasAudit.Text == "未审核")
            {
                mSql += " and a.status = '已提交等待商务审核'";
                //dgvExAllocation.Columns["ColAudit"].Visible = true;
                //dgvExAllocation.Columns["ColAudit1"].Visible = true;
            }
            if (cmbHasAudit.Text == "已审核")
            {
                mSql += " and a.businessAudit is not null";
                //dgvExAllocation.Columns["ColAudit"].Visible = false;
                //dgvExAllocation.Columns["ColAudit1"].Visible = false;
            }

            if (cmbMoney.Text == "已付款")
            {
                mSql += " and a.IsPaid = '已付款'";
            }
            if (cmbMoney.Text == "未付款")
            {
                mSql += " and a.IsPaid = '未付款'";

            }

            if (cmbStatus.Text == "已通过")
            {
                mSql += " and a.status ='领导审核通过'";
            }
            if (cmbStatus.Text == "未通过")
            {
                mSql += " and a.status <> '领导审核通过'";
            }

            if (cmbUserName.Text != "")
            {
                mSql += " and b.username like '%" + cmbUserName.Text + "%'";
            }
            if (txtTableNo.Text != "")
            {
                mSql += " and tableno like '%" + txtTableNo.Text + "%'";
            }
            if (txtCustomerName.Text != "")
            {
                mSql += " and d.CompanyName like '%" + txtCustomerName.Text + "%'";
            }
            if (txtProductName.Text != "")
            {
                mSql += " and p.name like '%" + txtProductName.Text + "%'";
            }

            SqlDataAdapter adapter = new SqlDataAdapter(mSql, conn);
            DataTable mTable = new DataTable();

            try
            {
                adapter.Fill(mTable);
                dgvExAllocation.DataSource = mTable;
                if (mTable.Rows.Count < 1)
                {
                    MessageBox.Show("没有记录");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvExAllocation_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvExAllocation.Columns["ColAudit"].Index)
                {
                    //判读是否是需要审核的记录
                    if (dgvExAllocation.Rows[e.RowIndex].Cells["statusDataGridViewTextBoxColumn"].Value.ToString().Contains("等待商务审核") == false)
                        return;
                    if (DialogResult.Yes == MessageBox.Show("确认审核通过？", "提示", MessageBoxButtons.YesNo))
                    {
                        int ret = t_ExpenseAllocationTableAdapter.UpdateBusinessAudit("商务已审核等待财务审核", Classes.PubClass.UserId, "", Convert.ToInt64(dgvExAllocation.Rows[e.RowIndex].Cells["idDataGridViewTextBoxColumn"].Value));
                        if (ret > 0)
                        {
                            MessageBox.Show("提交成功");
                            dgvExAllocation.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                        {
                            MessageBox.Show("提交失败");
                        }
                    }
                }
                if (e.ColumnIndex == dgvExAllocation.Columns["ColAudit1"].Index)
                {
                    //判读是否是需要审核的记录
                    if (dgvExAllocation.Rows[e.RowIndex].Cells["statusDataGridViewTextBoxColumn"].Value.ToString().Contains("等待商务审核") == false)
                        return;
                    if (DialogResult.Yes == MessageBox.Show("确认审核不通过？", "提示", MessageBoxButtons.YesNo))
                    {
                        //using (审核备注 mForm = new 审核备注())
                        //{
                        //    if (DialogResult.OK == mForm.ShowDialog())
                        //    {
                        int ret = t_ExpenseAllocationTableAdapter.UpdateBusinessAudit("商务审核不通过", Classes.PubClass.UserId, "", Convert.ToInt64(dgvExAllocation.Rows[e.RowIndex].Cells["idDataGridViewTextBoxColumn"].Value));
                        if (ret > 0)
                        {
                            MessageBox.Show("提交成功");
                            dgvExAllocation.Rows.RemoveAt(e.RowIndex);
                        }
                        else
                        {
                            MessageBox.Show("提交失败");
                        }
                        //    }

                        //}
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (费用分配详细 mForm = new 费用分配详细())
            {
                mForm.ShowDialog();
            }
        }

        private void dgvExAllocation_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                using (申请费用分配 mForm = new 申请费用分配())
                {
                    mForm.mRow = dgvExAllocation.Rows[e.RowIndex];
                    mForm.ExId = Convert.ToInt64(dgvExAllocation.Rows[e.RowIndex].Cells["idDataGridViewTextBoxColumn"].Value);
                    mForm.ShowDialog();
                }
            }
        }
    }
}
