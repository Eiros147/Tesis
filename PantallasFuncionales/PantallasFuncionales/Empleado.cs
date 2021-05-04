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
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace PantallasFuncionales
{
    public partial class Empleado : Form
    {
        DataTable dtDatos;
        SqlDataAdapter datos, datosUpdate;
        int selectedRow;
        SqlCommandBuilder dtBuild;
        DataSet ds;

        public Empleado()
        {
            InitializeComponent();
        }

        SqlConnection conexion = new SqlConnection("server=localhost;user id=root;persistsecurityinfo=True;database=tesis; password=123456");

        private void Empleado_Load(object sender, EventArgs e)
        {
            this.empleadoTableAdapter.Fill(this.tesisDataSet.empleado);

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            borrar();
            cargar();
        }

        void cargar()
        {


            string cadenaSelect = "SELECT empID, empNombre, empApellido, empDireccion, empTelefono, empDNI, empRol FROM empleado";

            dtDatos = new DataTable();

            datos = new SqlDataAdapter(cadenaSelect, conexion);

            datos.Fill(dtDatos);

            foreach (DataRow item in dtDatos.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[1].Value = item["empID"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["empNombre"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["empApellido"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["empDireccion"].ToString();
                dataGridView1.Rows[n].Cells[5].Value = item["empTelefono"].ToString();
                dataGridView1.Rows[n].Cells[6].Value = item["empDNI"].ToString();
                dataGridView1.Rows[n].Cells[7].Value = item["empRol"].ToString();
                dataGridView1.Rows[n].Cells[0].Value = "false";
            }
            dtDatos.Rows.Add();
        }

        void borrar()
        {
            conexion.Open();
            SqlCommand cmd;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Cells[0].Value != null)
                    if (bool.Parse(item.Cells[0].Value.ToString()))
                    {
                        //MessageBox.Show("Lineas seleccionadas:" + item.Cells[4].RowIndex.ToString());
                        string cadenaBorrar = ("Delete From empleado Where empID ='" + item.Cells[1].Value.ToString() + "'");
                        dtBuild = new SqlCommandBuilder(datos);
                        cmd = new SqlCommand(cadenaBorrar, conexion);
                        cmd.ExecuteNonQuery();
                    }
            }

            conexion.Close();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Cells[0].Value != null)
                    if (bool.Parse(item.Cells[0].Value.ToString()))
                    {

                        txtID.Text = item.Cells[1].Value.ToString();
                        txtNombre.Text = item.Cells[2].Value.ToString();
                        txtApellido.Text = item.Cells[3].Value.ToString();
                        txtDireccion.Text = item.Cells[4].Value.ToString();
                        txtTelefono.Text = item.Cells[5].Value.ToString();
                        txtDNI.Text = item.Cells[6].Value.ToString();
                        txtRol.Text = item.Cells[7].Value.ToString();
                    }
            }
        }

        void busqueda()
        {
            DataTable dtDatos = new DataTable();

            if (txtID.Text.Length > 0)
            {
                string cadena = ("SELECT empID, empNombre, empApellido, empDireccion, empTelefono, empDNI, empRol FROM empleado Where empID Like '" + txtID.Text + "%'");
                SqlDataAdapter data = new SqlDataAdapter(cadena, conexion);
                data.Fill(dtDatos);
            }
            else if (txtNombre.Text.Length > 0)
            {
                string cadena = ("SELECT empID, empNombre, empApellido, empDireccion, empTelefono, empDNI, empRol FROM empleado Where empNombre Like '" + txtNombre.Text + "%'");
                SqlDataAdapter data = new SqlDataAdapter(cadena, conexion);
                data.Fill(dtDatos);
            }
            else
            {
                string cadena = ("SELECT empID, empNombre, empApellido, empDireccion, empTelefono, empDNI, empRol FROM empleado Where empApellido Like '" + txtApellido.Text + "%'");
                SqlDataAdapter data = new SqlDataAdapter(cadena, conexion);
                data.Fill(dtDatos);
            }
            dataGridView1.DataSource = dtDatos;
        }
    }
}
