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

namespace PantallasFuncionales
{
    public partial class Actividades : Form
    {
        DataTable dtDatos;
        MySqlDataAdapter datos, datosUpdate;
        int selectedRow;
        MySqlCommandBuilder dtBuild;
        DataSet ds;

        public Actividades()
        {
            InitializeComponent();
        }

        MySqlConnection conexion = new MySqlConnection("server=localhost;user id=root;persistsecurityinfo=True;database=tesis; password=123456");

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            //txtBusqueda.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            //txtBusquedaNombre.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            //txtCosto.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            //txtDescripcion.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();

        }

        private void Actividades_Load(object sender, EventArgs e)
        {
            cargar();            
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            guardar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            borrar();
            dataGridView1.Rows.Clear();
            cargar();
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            //(dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("Nombre LIKE '%{0}%'", txtBusqueda.Text);
            
            //txtBusquedaNombre.Text = "";
            //txtCosto.Text = "";
            //txtDescripcion.Text = "";
            //busqueda();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //txtBusqueda.Text = "";
            //txtCosto.Text = "";
            //txtDescripcion.Text = "";
            //busqueda();
        }

        void busqueda()
        {
            DataTable dtDatos = new DataTable();

            if (txtBusqueda.Text.Length > 0)
            {
                string cadena = ("SELECT actID, actNombre, actDescripcion, actCosto FROM actividad Where actID Like '" + txtBusqueda.Text + "%'");
                MySqlDataAdapter data = new MySqlDataAdapter(cadena, conexion);
                data.Fill(dtDatos);
            }
            else if (txtBusquedaNombre.Text.Length > 0)
            {
                string cadena = ("SELECT actID, actNombre, actDescripcion, actCosto FROM actividad Where actNombre Like '" + txtBusquedaNombre.Text + "%'");
                MySqlDataAdapter data = new MySqlDataAdapter(cadena, conexion);
                data.Fill(dtDatos);
            }
            else
            {
                string cadena = ("SELECT actID, actNombre, actDescripcion, actCosto FROM actividad Where actNombre Like '" + txtBusquedaNombre.Text + "%'");
                MySqlDataAdapter data = new MySqlDataAdapter(cadena, conexion);
                data.Fill(dtDatos);
            }
            dataGridView1.DataSource = dtDatos;
        }

        void borrar()
        {
            conexion.Open();
            MySqlCommand cmd;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Cells[0].Value != null)
                    if (bool.Parse(item.Cells[0].Value.ToString()))
                {
                    //MessageBox.Show("Lineas seleccionadas:" + item.Cells[4].RowIndex.ToString());
                    string cadenaBorrar = ("Delete From actividad Where actID ='" + item.Cells[1].Value.ToString() + "'");
                    dtBuild = new MySqlCommandBuilder(datos);
                    cmd = new MySqlCommand(cadenaBorrar, conexion);
                    cmd.ExecuteNonQuery();
                }
            }

            conexion.Close();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if(item.Cells[0].Value != null)
                if (bool.Parse(item.Cells[0].Value.ToString()))
                {

                    txtBusqueda.Text = item.Cells[1].Value.ToString();
                    txtBusquedaNombre.Text = item.Cells[2].Value.ToString();
                    txtCosto.Text = item.Cells[3].Value.ToString();
                    txtDescripcion.Text = item.Cells[4].Value.ToString();
                }
            }

        }

        void cargar()
        {
            
            string cadenaSelect = "SELECT actID, actNombre, actDescripcion, actCosto FROM actividad";

            dtDatos = new DataTable();

            datos = new MySqlDataAdapter(cadenaSelect, conexion);

            datos.Fill(dtDatos);

            foreach (DataRow item in dtDatos.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[1].Value = item["actID"].ToString();
                dataGridView1.Rows[n].Cells[2].Value = item["actNombre"].ToString();
                dataGridView1.Rows[n].Cells[3].Value = item["actDescripcion"].ToString();
                dataGridView1.Rows[n].Cells[4].Value = item["actCosto"].ToString();
                dataGridView1.Rows[n].Cells[0].Value = "false";
            }
            dtDatos.Rows.Add();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            update();
        }

        void guardar()
        {
            conexion.Open();
            MySqlCommand cmd;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Cells[0].Value != null)
                    if (bool.Parse(item.Cells[0].Value.ToString()))
                    {
                        //MessageBox.Show("Lineas seleccionadas:" + item.Cells[4].RowIndex.ToString());
                        string cadenaNuevo = ("INSERT INTO actividad (actID, actNombre, actDescripcion, actCosto) VALUES('" + item.Cells[1].Value.ToString() + "','" + item.Cells[2].Value.ToString() + "','" + item.Cells[3].Value.ToString() + "','" + item.Cells[4].Value.ToString() + "')");
                        dtBuild = new MySqlCommandBuilder(datos);
                        cmd = new MySqlCommand(cadenaNuevo, conexion);
                        cmd.ExecuteNonQuery();
                    }
            }

            conexion.Close();
        }

        void update()
        {
            conexion.Open();
            MySqlCommand cmd;
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Cells[0].Value != null)
                    if (bool.Parse(item.Cells[0].Value.ToString()))
                    {
                        //MessageBox.Show("Lineas seleccionadas:" + item.Cells[4].RowIndex.ToString());
                        string cadenaActualizar = ("UPDATE actividad SET actNombre ='" + item.Cells[2].Value.ToString() + "', actDescripcion ='" + item.Cells[3].Value.ToString() + "', actCosto ='" + item.Cells[4].Value.ToString() + "' WHERE actID ='" + item.Cells[1].Value.ToString() + "')");
                        dtBuild = new MySqlCommandBuilder(datos);
                        cmd = new MySqlCommand(cadenaActualizar, conexion);
                        cmd.ExecuteNonQuery();
                    }
            }

            conexion.Close();
        }
    }
}
