using accesoDatos;
using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class frmDetalleArticulo : Form
    {
        private Articulo articulo = null;
        public frmDetalleArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Detalles del articulo";
        }

        private void frmDetalleArticulo_Load(object sender, EventArgs e)
        {
            MarcaData marca = new MarcaData();
            CategoriaData categoria = new CategoriaData();

            try
            {

                if (articulo != null)
                {
                    lblDetalleCodigo.Text = articulo.Codigo;
                    lblDetalleNombre.Text = articulo.Nombre;
                    lblDetalleDescripcion.Text = articulo.Descripcion;
                    cargarImagen(articulo.ImagenUrl);
                    lblDetalleMarca.Text = articulo.Marca.Descripcion;
                    lblDetalleCategoria.Text = articulo.Categoria.Descripcion;
                    lblDetallePrecio.Text = articulo.Precio.ToString("0.00");

                }
            }
            catch (Exception)
            {

                MessageBox.Show("Hubo un error");
            }
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception)
            {

                pbxArticulo.Load("https://editorial.unc.edu.ar/wp-content/uploads/sites/33/2022/09/placeholder.png");
            }
        }

        private void btnDetalleAceptar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
