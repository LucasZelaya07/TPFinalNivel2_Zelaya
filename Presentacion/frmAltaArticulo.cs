using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using dominio;
using negocio;
using helper;

namespace Presentacion
{
    public partial class frmAltaArticulo : Form
    {
        private Articulo articulo = null;
        private Validaciones validaciones = new Validaciones();
        private OpenFileDialog archivo = null;
        public frmAltaArticulo()
        {
            InitializeComponent();
        }
        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar articulo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloData articuloNuevo = new ArticuloData();
            try
            {
                if (validarAlta())
                    return;
                if (articulo == null)
                    articulo = new Articulo();
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.ImagenUrl = txtImagenUrl.Text;
                articulo.Precio = Decimal.Parse(txtPrecio.Text);

                if (articulo.Id != 0)
                {
                    articuloNuevo.modificarArticulo(articulo);
                    MessageBox.Show("Su articulo fue modificado exitosamente");
                }
                else
                {
                    articuloNuevo.agregarArticulo(articulo);
                    MessageBox.Show("Su articulo fue agregado exitosamente");
                }

                if (archivo != null && !(txtImagenUrl.Text.ToLower().Contains("http")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["deposito-app"] + archivo.SafeFileName);
                }
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            MarcaData marca = new MarcaData();
            CategoriaData categoria = new CategoriaData();
            try
            {
                cboMarca.DataSource = marca.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = categoria.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtImagenUrl.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    cboMarca.SelectedValue = articulo.Marca.Id;
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    txtPrecio.Text = articulo.Precio.ToString("0.00");

                }
            }
            catch (Exception)
            {

                MessageBox.Show("Hubo un error");
            }

        }

        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagenUrl.Text);
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

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg; |png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtImagenUrl.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
            }
        }
        private bool validarAlta()
        {
            
            if (string.IsNullOrEmpty(txtCodigo.Text) || string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtDescripcion.Text) || string.IsNullOrEmpty(txtImagenUrl.Text) || string.IsNullOrEmpty(txtPrecio.Text) || string.IsNullOrEmpty(cboMarca.Text) || string.IsNullOrEmpty(cboCategoria.Text))
            {
                MessageBox.Show("Por favor, debes cargar todos los campos para dar el alta.");
                return true;
            }
            if (!(validaciones.sinEspeciales(txtCodigo.Text)) || !(validaciones.sinEspeciales(cboMarca.Text)) || !(validaciones.sinEspeciales(cboCategoria.Text)))
            {
                MessageBox.Show("Por favor, no ingrese caracteres especiales en el alta.");
                return true;
            }
            if (!(validaciones.soloNumeros(txtPrecio.Text)))
            {
                MessageBox.Show("Por favor, ingrese solo numeros para el precio.");
                return true;
            }
            return false;
        }
    }
}
