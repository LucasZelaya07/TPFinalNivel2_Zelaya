using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;
using accesoDatos;
using System.Net;

namespace negocio
{
    public class ArticuloData
    {
        private AccesoDatos datos = new AccesoDatos();
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true;";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl, C.Descripcion Categoria, M.Descripcion Marca, Precio, A.IdMarca, A.IdCategoria From ARTICULOS A, MARCAS M, CATEGORIAS C Where C.Id = A.IdCategoria and M.Id = A.IdMarca";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulo auxiliar = new Articulo();
                    auxiliar.Marca = new Marca();
                    auxiliar.Categoria = new Categoria();
                    auxiliar.Id = (int)lector["Id"];
                    auxiliar.Codigo = (string)lector["Codigo"];
                    auxiliar.Nombre = (string)lector["Nombre"];
                    auxiliar.Descripcion = (string)lector["Descripcion"];
                    auxiliar.Marca.Id = (int)lector["IdMarca"];
                    auxiliar.Marca.Descripcion = (string)lector["Marca"];
                    auxiliar.Categoria.Id = (int)lector["IdCategoria"];
                    auxiliar.Categoria.Descripcion = (string)lector["Categoria"];
                    if (!(lector["ImagenUrl"] is DBNull))
                        auxiliar.ImagenUrl = (string)lector["ImagenUrl"];
                    auxiliar.Precio = (decimal)lector["Precio"];
                    lista.Add(auxiliar);
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { conexion.Close(); }
        }
        public void agregarArticulo(Articulo nuevo)
        {

            try
            {
                datos.setearConsulta("Insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)values(@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @ImagenUrl, @Precio )");
                datos.setearParametros("@Codigo", nuevo.Codigo);
                datos.setearParametros("@Nombre", nuevo.Nombre);
                datos.setearParametros("@Descripcion", nuevo.Descripcion);
                datos.setearParametros("@IdMarca", nuevo.Marca.Id);
                datos.setearParametros("@IdCategoria", nuevo.Categoria.Id);
                datos.setearParametros("@ImagenUrl", nuevo.ImagenUrl);
                datos.setearParametros("@Precio", nuevo.Precio);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void modificarArticulo(Articulo modificado)
        {

            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @ImagenUrl, Precio = @Precio where Id = @Id");
                datos.setearParametros("@Codigo", modificado.Codigo);
                datos.setearParametros("@Nombre", modificado.Nombre);
                datos.setearParametros("@Descripcion", modificado.Descripcion);
                datos.setearParametros("@IdMarca", modificado.Marca.Id);
                datos.setearParametros("@IdCategoria", modificado.Categoria.Id);
                datos.setearParametros("@ImagenUrl", modificado.ImagenUrl);
                datos.setearParametros("@Precio", modificado.Precio);
                datos.setearParametros("@Id", modificado.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void eliminarArticulo(int id)
        {
            try
            {
                datos.setearConsulta("delete from ARTICULOS where Id = @Id");
                datos.setearParametros("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();

            try
            {
                string consulta = "select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl, C.Descripcion Categoria, M.Descripcion Marca, Precio, A.IdMarca, A.IdCategoria From ARTICULOS A, MARCAS M, CATEGORIAS C Where C.Id = A.IdCategoria and M.Id = A.IdMarca and ";
                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Mayor a ":
                            consulta += "Precio > " + filtro;
                            break;
                        case "Menor a ":
                            consulta += "Precio < " + filtro;
                            break;
                        default:
                            consulta += "Precio = " + filtro;
                            break;
                    }
                }
                else
                {
                    switch (campo)
                    {
                        case "Código":
                            switch (criterio)
                            {
                                case "Comienza con":
                                    consulta += "Codigo like '" + filtro + "%'";
                                    break;
                                case "Termina con":
                                    consulta += "Codigo like '%" + filtro + "'";
                                    break;
                                default:
                                    consulta += "Codigo like '%" + filtro + "%'";
                                    break;
                            }
                            break;
                        case "Nombre":
                            switch (criterio)
                            {
                                case "Comienza con":
                                    consulta += "Nombre like '" + filtro + "%'";
                                    break;
                                case "Termina con":
                                    consulta += "Nombre like '%" + filtro + "'";
                                    break;
                                default:
                                    consulta += "Nombre like '%" + filtro + "%'";
                                    break;
                            }
                            break;
                        case "Marca":
                            switch (criterio)
                            {
                                case "Comienza con":
                                    consulta += "M.Descripcion like '" + filtro + "%'";
                                    break;
                                case "Termina con":
                                    consulta += "M.Descripcion like '%" + filtro + "'";
                                    break;
                                default:
                                    consulta += "M.Descripcion like '%" + filtro + "%'";
                                    break;
                            }
                            break;
                        default:
                            switch (criterio)
                            {
                                case "Comienza con":
                                    consulta += "C.Descripcion like '" + filtro + "%'";
                                    break;
                                case "Termina con":
                                    consulta += "C.Descripcion like '%" + filtro + "'";
                                    break;
                                default:
                                    consulta += "C.Descripcion like '%" + filtro + "%'";
                                    break;
                            }
                            break;
                    }
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo auxiliar = new Articulo();
                    auxiliar.Marca = new Marca();
                    auxiliar.Categoria = new Categoria();
                    auxiliar.Id = (int)datos.Lector["Id"];
                    auxiliar.Codigo = (string)datos.Lector["Codigo"];
                    auxiliar.Nombre = (string)datos.Lector["Nombre"];
                    auxiliar.Descripcion = (string)datos.Lector["Descripcion"];
                    auxiliar.Marca.Id = (int)datos.Lector["IdMarca"];
                    auxiliar.Marca.Descripcion = (string)datos.Lector["Marca"];
                    auxiliar.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    auxiliar.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        auxiliar.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    auxiliar.Precio = (decimal)datos.Lector["Precio"];
                    lista.Add(auxiliar);
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
