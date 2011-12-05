using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DemoDropOut.Apps.Objects;

namespace DemoDropOut.Options
{
    public partial class F005_PreprocessingOptions : Form
    {
        public F005_PreprocessingOptions()
        {
            InitializeComponent();
        }

        private CategoricalEncoding m_cate_enc;

        public CategoricalEncoding CategoricalEncoding
        {
            get { return m_cate_enc; }
            set { m_cate_enc = value; }
        }

        public void SetCategoricalEnc(CategoricalEncoding enc)
        {
            try
            {
                if (enc == CategoricalEncoding.OneOfN)
                {
                    chkOneOfNEncoding.Checked = true;
                }
                else if (enc == CategoricalEncoding.Binary)
                {
                    chkBinaryEncoding.Checked = true;
                }
                else if (enc == CategoricalEncoding.Numeric)
                {
                    chkNumericEncoding.Checked = true;
                }
                m_cate_enc = enc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkOneOfNEncoding.Checked == true)
                {
                    m_cate_enc = CategoricalEncoding.OneOfN;
                }
                else if (chkBinaryEncoding.Checked == true)
                {
                    m_cate_enc = CategoricalEncoding.Binary;
                }
                else if(chkNumericEncoding.Checked == true)
                {
                    m_cate_enc = CategoricalEncoding.Numeric;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
