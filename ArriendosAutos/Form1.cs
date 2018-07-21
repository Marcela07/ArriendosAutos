using DatosAcceso;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArriendosAutos
{
    public partial class Form1 : Form
    {
        Autos _auto;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void CargarLoad(object sender, EventArgs e)
        {
            CargarGrilla();
            DesbloquearBoton();

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (var contexto = new VehiculosEntities())
            {
                contexto.Autos.Add(new Autos()
                {
                    Patente = txtPatente.Text,
                    Marca = txtMarca.Text,
                    Modelo = txtModelo.Text,
                    TipoCombustible = cmbTipo.Text,
                    Año = Convert.ToInt32(txtAño.Text),


                });

                contexto.SaveChanges();
            }
            CargarGrilla();
            LimpiarFormulario();

        }



        private void CargarGrilla()
        {

            dgvListado.Rows.Clear();



            using (VehiculosEntities contexto = new VehiculosEntities())
            {

                List<Autos> autos = contexto.Autos.ToList();


                foreach (Autos auto in autos)
                {

                    dgvListado.Rows.Add(new object[] {
                                auto.Id,
                                auto.Patente,
                                auto.Marca,
                                auto.Modelo,
                                auto.TipoCombustible,
                                auto.Año
                            }
                        );


                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            DesbloquearBoton();
        }

        private void CargarInformacionAutosEnFormulario()
        {
            txtPatente.Text = _auto.Patente;
            txtMarca.Text = _auto.Marca;
            txtModelo.Text = _auto.Modelo;
            cmbTipo.Text = _auto.TipoCombustible;
            txtAño.Text = _auto.Año.ToString();
        }
        private void LimpiarFormulario()
        {
            txtPatente.Clear();
            txtMarca.Clear();
            txtModelo.Clear();
            cmbTipo.SelectedIndex = 0;
            txtAño.Clear();
        }

        private void GrillaHacerClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            string id = dgvListado.Rows[row].Cells[0].Value.ToString();


            int idAutos = Convert.ToInt32(id);
            using (VehiculosEntities contexto = new VehiculosEntities())
            {
                _auto = contexto.Autos.Find(idAutos);

                CargarInformacionAutosEnFormulario();

                BloquearBoton();
            }

        }

        private void BloquearBoton()
        {
            EstadoBoton(false);
        }
        private void DesbloquearBoton()
        {
            EstadoBoton(true);
        }
        private void EstadoBoton(bool estado)
        {
            btnActualizar.Enabled = !estado;
            btnEliminar.Enabled = !estado;
            btnGuardar.Enabled = estado;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {

            _auto.Patente = txtPatente.Text;
            _auto.Marca = txtMarca.Text;
            _auto.Modelo = txtModelo.Text;
            _auto.TipoCombustible = cmbTipo.Text;
            _auto.Año = Convert.ToInt32(txtAño.Text);
            

            using (VehiculosEntities contexto = new VehiculosEntities())
            {
                contexto.Entry(_auto).State = System.Data.Entity.EntityState.Modified;
                contexto.SaveChanges();

            }
            CargarGrilla();
            LimpiarFormulario();
            DesbloquearBoton();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            

                DialogResult respuesta = MessageBox.Show("Esta seguro de eliminar auto " + _auto.Patente, "Sistema Renta Car", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    using (VehiculosEntities contexto = new VehiculosEntities())
                    {
                        contexto.Entry(_auto).State = System.Data.Entity.EntityState.Deleted;
                        contexto.SaveChanges();
                    }
                    CargarGrilla();
                    LimpiarFormulario();
                    DesbloquearBoton();
                }
                else
                {
                    txtPatente.Select();
                    txtPatente.Focus();
                }


            
        }
    }
}
