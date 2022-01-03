﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using APSDAC;
using APSVO;

namespace APSWinForm
{
    public partial class SETUP_TIME : Form
    {
        List<SetupVO> SetupList;
        ServiceHelp srv = new ServiceHelp("");
        List<STD_STEP_VO> eqpgroup;
        List<LineVO> Lineinfo;

        //public List<SetupVO> GetSetup_time()
        //{
        //    EQUIPDAC dac = new EQUIPDAC();
        //    return dac.GetSetup_time();
        //}

        public SETUP_TIME()
        {
            InitializeComponent();
        }

        private void SETUP_TIME_Load(object sender, EventArgs e)
        {
            combobinding();
            DataLoad();
        }

       

        private async void combobinding()
        {
            eqpgroup = await srv.GetListAsync("api/Step/getStepInfoList", eqpgroup);
            Lineinfo = await srv.GetListAsync("api/EQUIPMENT/Linelist", Lineinfo);
            CommonUtil.ComboBinding(cboGroup, eqpgroup, "STD_STEP_NAME", "STD_STEP_NAME");
            CommonUtil.ComboBinding(cboStep, eqpgroup, "STD_STEP_ID", "STD_STEP_ID");
            CommonUtil.ComboBinding(cboLine, Lineinfo, "LINE_ID", "LINE_ID");
            CommonUtil.ComboBinding(cboSite, Lineinfo, "SITE_ID", "SITE_ID");

        }

        private async void dgvLoad()
        {
            dgvSetup.DataSource = null;
            SetupList = await srv.GetListAsync("api/SETUP_TIME/SetupList", SetupList);
            dgvSetup.DataSource = SetupList;
        }

        private void DataLoad()
        {
            DataGridViewUtil.AddGridTextColumn(dgvSetup, "사이트ID", "SITE_ID", colWidth: 70);
            DataGridViewUtil.AddGridTextColumn(dgvSetup, "라인ID", "LINE_ID", colWidth: 105);
            DataGridViewUtil.AddGridTextColumn(dgvSetup, "설비처리그룹", "EQP_GROUP", colWidth: 110);
            DataGridViewUtil.AddGridTextColumn(dgvSetup, "공정ID", "STEP_ID", colWidth: 105);
            DataGridViewUtil.AddGridTextColumn(dgvSetup, "소요시간", "TIME", colWidth: 100);

            dgvLoad();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if ((cboGroup.Text == "") && (cboStep.Text == "") && (cboLine.Text == "") && (cboSite.Text == "") )
            {
                MessageBox.Show("항목을 선택해주세요.");

                return;
            }
            getSearchSetUpList();
        }

        private void getSearchSetUpList()  // 검색함수
        {
            dgvSetup.DataSource = null;
            dgvSetup.DataSource = SetupList.FindAll(p => p.EQP_GROUP.Contains(cboGroup.Text) && p.STEP_ID.Contains(cboStep.Text) && p.LINE_ID.Contains(cboLine.Text) && p.SITE_ID.Contains(cboSite.Text));
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            dgvSetup.DataSource = null;
            cboGroup.SelectedIndex = 0;
            cboStep.SelectedIndex = 0;
            cboLine.SelectedIndex = 0;
            cboSite.SelectedIndex = 0;
            dgvLoad();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SETUP_REG frm = new SETUP_REG();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
            dgvLoad();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (temp != null)
            {
                SetupVO vo = new SetupVO();

                vo.SITE_ID = dgvSetup.Rows[temp.RowIndex].Cells[0].Value.ToString();
                vo.LINE_ID = dgvSetup.Rows[temp.RowIndex].Cells[1].Value.ToString();
                vo.EQP_GROUP = dgvSetup.Rows[temp.RowIndex].Cells[2].Value.ToString();
                vo.STEP_ID = dgvSetup.Rows[temp.RowIndex].Cells[3].Value.ToString();
                vo.TIME = Convert.ToInt32(dgvSetup.Rows[temp.RowIndex].Cells[4].Value.ToString());

                

                SETUP_REG frm = new SETUP_REG(vo);
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
                dgvLoad();
            }
            else
            {
                MessageBox.Show("수정할 설비를 선택해주세요");
            }
        }
        DataGridViewCellEventArgs temp;

        
        private void dgvSetup_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            temp = e;
        }
    }
}
