using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace PantallasFuncionales
{

    public partial class Socio : Form
    {
        DataTable dtDatos;
        SqlDataAdapter datos, datosUpdate;
        int selectedRow;
        SqlCommandBuilder dtBuild;
        DataSet ds;

        public Socio()
        {
            InitializeComponent();
        }

        SqlConnection conexion = new SqlConnection("server=localhost;user id=root;persistsecurityinfo=True;database=tesis; password=123456");

        private void Socio_Load(object sender, EventArgs e)
        {
            cargar();
        }

        void cargar()
        {
            string cadenaSelect = "SELECT socID, socDNI, socNombre, socDireccion, socTelefono, socActividades, socEstado FROM socio";

            dtDatos = new DataTable();

            datos = new SqlDataAdapter(cadenaSelect, conexion);

            datos.Fill(dtDatos);

            foreach (DataRow item in dtDatos.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[1].Value = item["socID"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["socDNI"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["socNombre"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["socDireccion"].ToString();
                dataGridView1.Rows[n].Cells[5].Value = item["socTelefono"].ToString();
                dataGridView1.Rows[n].Cells[6].Value = item["socEstado"].ToString();
                dataGridView1.Rows[n].Cells[0].Value = "false";
            }
            dtDatos.Rows.Add();
        }

        void busqueda()
        {
            DataTable dtDatos = new DataTable();

            if (txtID.Text.Length > 0)
            {
                string cadena = ("SELECT socID, socDNI, socNombre, socDireccion, socTelefono, socActividades, socEstado FROM socio Where socID Like '" + txtID.Text + "%'");
                SqlDataAdapter data = new SqlDataAdapter(cadena, conexion);
                data.Fill(dtDatos);
            }
            else if (txtDNI.Text.Length > 0)
            {
                string cadena = ("SELECT socID, socDNI, socNombre, socDireccion, socTelefono, socActividades, socEstado FROM socio Where socDNI Like '" + txtDNI.Text + "%'");
                SqlDataAdapter data = new SqlDataAdapter(cadena, conexion);
                data.Fill(dtDatos);
            }
            else
            {
                string cadena = ("SELECT socID, socDNI, socNombre, socDireccion, socTelefono, socActividades, socEstado FROM socio Where socNombre Like '" + txtNombre.Text + "%'");
                SqlDataAdapter data = new SqlDataAdapter(cadena, conexion);
                data.Fill(dtDatos);
            }
            dataGridView1.DataSource = dtDatos;
        }

        void borrar()
        {
            conexion.Open();
            SqlCommand cmd;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Cells[0].Value != null)
                    if (bool.Parse(item.Cells[4].Value.ToString()))
                {
                    //MessageBox.Show("Lineas seleccionadas:" + item.Cells[7].RowIndex.ToString());
                    string cadenaBorrar = ("Delete From socio Where socID ='" + item.Cells[1].Value.ToString() + "'");
                    dtBuild = new SqlCommandBuilder(datos);
                    cmd = new SqlCommand(cadenaBorrar, conexion);
                    cmd.ExecuteNonQuery();
                }
            }

            conexion.Close();
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {
            //txtDNI.Text = "";
            //txtNombre.Text = "";
            //busqueda();
        }

        private void txtDNI_TextChanged(object sender, EventArgs e)
        {
            //txtID.Text = "";
            //txtNombre.Text = "";
            //busqueda();
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            //txtID.Text = "";
            //txtDNI.Text = "";
            //busqueda();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            borrar();
            cargar();
        }
    }
}
