﻿using capa_negocio;
using capa_entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Data;

namespace capa_presentacion
{
    /// <summary>
    /// Lógica de interacción para Pedidos.xaml
    /// </summary>
    public partial class Pedidos : UserControl
    {
        private Negocio neg;

        private CollectionViewSource vistaPedidos;
        private List<Pedido> listaPedidos;
        private CollectionViewSource vistaLinpeds;
        private List<Linped> listaLinpeds;

        private List<Usuario> listaUsuarios;
        private List<Articulo> listaArticulos;
        private List<TipoArticulo> listaTipos;

        private Pedido selectedOrder;

        public Pedidos(Negocio neg)
        {
            this.neg = neg;
            InitializeComponent();

            vistaPedidos = (CollectionViewSource)this.Resources["lista_pedidos"];
            listaPedidos = new List<Pedido>();

            vistaLinpeds = (CollectionViewSource)this.Resources["lista_linpeds"];
            listaLinpeds = new List<Linped>();

            listaUsuarios = neg.GetUsuarios();
            cbUsu.ItemsSource = listaUsuarios;
            cbUsu.SelectedIndex = 0;

            LeerPedidos();

            dataGridLinpeds.ItemsSource = null;

            listaArticulos = new List<Articulo>();

            listaTipos = neg.GetTiposArticulo();
            cbTipoArt.Items.Add("");
            foreach(TipoArticulo tipo in listaTipos)
            {
                cbTipoArt.Items.Add(tipo);
            }
            cbTipoArt.SelectedIndex = 0;

            dpFechaB.DisplayDateEnd = DateTime.Today;

            dpFecha.SelectedDate = DateTime.Today;
            dpFecha.DisplayDateEnd = DateTime.Today;

            btnIns.IsEnabled = true;
            btnMod.IsEnabled = false;
            btnEli.IsEnabled = false;
            btnDel.IsEnabled = false;
        }

        private Usuario ObtenerUsuario(string id)
        {
            foreach(Usuario u in listaUsuarios)
            {
                if (u.UsuarioID == id)
                    return u;
            }
            return null;
        }

        private void LeerPedidos()
        {
            listaPedidos.Clear();

            foreach (Pedido p in neg.GetPedidos())
            {
                Pedido ped = new Pedido();
                ped.PedidoID = p.PedidoID;
                ped.UsuarioID = p.UsuarioID;
                ped.Fecha = p.Fecha.Substring(0,10);
                ped.UsuarioNombre = ObtenerUsuario(p.UsuarioID).Nombre;

                listaPedidos.Add(ped);
            }

            vistaPedidos.Source = listaPedidos;
            selectedOrder = null;
        }

        private void LeerLinpeds(string pedidoId)
        {
            listaLinpeds.Clear();

            foreach(Linped l in neg.GetLinpeds(pedidoId))
            {
                Linped lin = new Linped();
                lin.Linea = l.Linea;
                lin.ArticuloNombre = neg.GetArticulo(l.ArticuloID).Nombre;
                lin.Importe = l.Importe;
                lin.Cantidad = l.Cantidad;

                listaLinpeds.Add(lin);
            }

            dataGridLinpeds.ItemsSource = listaLinpeds;
        }

        private void BuscarPedido()
        {
            string nombreUsu = txbUsuB.Text.Trim().ToUpper();
            string fecha = (dpFechaB.SelectedDate != null ? ((DateTime)dpFechaB.SelectedDate).ToString("yyyy-MM-dd") : "");

            List<Pedido> pedidos = (from pedido in neg.GetPedidos().AsParallel()
                                    where (ObtenerUsuario(pedido.UsuarioID).Nombre.Trim().ToUpper().StartsWith(nombreUsu) || nombreUsu.Length == 0) &&
                                        (Convert.ToDateTime(pedido.Fecha).ToString("yyyy-MM-dd").Equals(fecha) || fecha.Length == 0)
                                    select pedido).ToList();

            listaPedidos.Clear();
            foreach (Pedido p in pedidos)
            {
                Pedido ped = new Pedido();
                ped.PedidoID = p.PedidoID;
                ped.UsuarioID = p.UsuarioID;
                ped.Fecha = p.Fecha.Substring(0, 10);
                ped.UsuarioNombre = ObtenerUsuario(p.UsuarioID).Nombre;

                listaPedidos.Add(ped);
            }

            selectedOrder = null;
        }

        private void btnAplicarFiltros_Click(object sender, RoutedEventArgs e)
        {
            BuscarPedido();
        }

        private void btnBorrarFiltros_Click(object sender, RoutedEventArgs e)
        {
            txbUsuB.Clear();
            dpFechaB.SelectedDate = null;
            BuscarPedido();
        }

        private void VaciarCamposPedido()
        {
            cbUsu.SelectedIndex = 0;
            dpFecha.SelectedDate = DateTime.Today;
        }

        public void VaciarCamposArticulos()
        {
            cbTipoArt.SelectedIndex = 0;
            upCant.Value = 1;
        }

        public void VaciarEtiquetasTotales()
        {
            lblTotalSinIVA.Content = "0.00 €";
            lblIVA.Content = "0.00 €";
            lblTotal.Content = "0.00 €";
        }

        private void dataGridPedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridPedidos.SelectedItems.Count == 0)
            {
                VaciarCamposPedido();
                VaciarCamposArticulos();
                VaciarEtiquetasTotales();
                selectedOrder = null;
                listaLinpeds.Clear();
                dataGridLinpeds.ItemsSource = null;
                dataGridLinpeds.IsEnabled = false;
                btnIns.IsEnabled = true;
                btnMod.IsEnabled = false;
                btnEli.IsEnabled = false;
                cbUsu.IsEnabled = true;
                dpFecha.IsEnabled = true;
            }
            else
            {
                string id = ((Pedido)dataGridPedidos.SelectedItem).PedidoID;
                selectedOrder = neg.GetPedido(id);

                VaciarCamposPedido();
                VaciarCamposArticulos();
                VaciarEtiquetasTotales();

                cbUsu.SelectedItem = ObtenerUsuario(selectedOrder.UsuarioID);
                dpFecha.SelectedDate = DateTime.Parse(selectedOrder.Fecha.Substring(0, 10));

                listaLinpeds.Clear();
                dataGridLinpeds.ItemsSource = null;
                LeerLinpeds(selectedOrder.PedidoID);
                ActualizarTotales();

                btnAdd.IsEnabled = true;
                btnIns.IsEnabled = false;
                btnMod.IsEnabled = true;
                btnEli.IsEnabled = true;
                dataGridLinpeds.IsEnabled = true;

                cbUsu.IsEnabled = false;
                dpFecha.IsEnabled = false;
            }
        }

        public void ActualizarTotales()
        {
            float[] importes = new float[dataGridLinpeds.Items.Count];
            int[] cantidades = new int[dataGridLinpeds.Items.Count];

            int i = 0;
            foreach (Linped linea in dataGridLinpeds.Items)
            {
                importes[i] = Convert.ToSingle(linea.Importe);
                cantidades[i] = Convert.ToInt32(linea.Cantidad);
                i++;
            }

            float[] totales = neg.CalcFactura(importes, cantidades);
            lblTotalSinIVA.Content = totales[0].ToString("0.00") + " €";
            lblIVA.Content = totales[1].ToString("0.00") + " €";
            lblTotal.Content = totales[2].ToString("0.00") + " €";
        }

        private void cbTipoArt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listaArticulos.Clear();

            if (cbTipoArt.SelectedIndex == 0)
            {
                listaArticulos = neg.GetProductos();
            }
            else
            {
                foreach (Articulo a in neg.GetProductos())
                {
                    if (a.TipoArticuloID == cbTipoArt.SelectedIndex.ToString())
                    {
                        listaArticulos.Add(a);
                    }
                }
            }

            cbArt.ItemsSource = listaArticulos;
            cbArt.SelectedItem = listaArticulos[0];
        }

        private string BuscarProductoId(string nombre)
        {
            foreach (Articulo a in listaArticulos)
            {
                if (a.Nombre.Equals(nombre))
                {
                    return a.ArticuloID;
                }
            }

            return "";
        }

        private void btnIns_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridLinpeds.Items.Count == 0)
            {
                MessageBox.Show("Debe añadir al menos un artículo", "Insertar pedido",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (selectedOrder != null)
                {
                    MessageBox.Show("Ya existe el pedido. Si desea modificarlo pulse el botón 'Modificar'", "Insertar pedido",
                           MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string pedidoId = (listaPedidos.Count + 2).ToString();
                    string usuarioId = listaUsuarios[cbUsu.SelectedIndex].UsuarioID;
                    string fecha = dpFecha.SelectedDate.Value.ToString("yyyy-MM-dd");

                    if (neg.InsertarPedido(pedidoId, usuarioId, fecha))
                    {
                        MessageBox.Show("Pedido insertado", "Insertar pedido",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        int lineasInsertadas = 0;
                        foreach (Linped l in dataGridLinpeds.Items)
                        {
                            string linea = (lineasInsertadas + 1).ToString();
                            string productoId = l.ArticuloID;
                            int importe = l.Importe;
                            int cantidad = l.Cantidad;

                            if (neg.InsertarLinped(pedidoId, linea, productoId, importe, cantidad))
                            {
                                lineasInsertadas += 1;
                            }
                        }

                        if (lineasInsertadas == dataGridLinpeds.Items.Count)
                        {
                            MessageBox.Show("Lineas de pedido insertadas", "Insertar lineas de pedido",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Error al insertar lineas de pedido", "Insertar lineas de pedido",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        LeerPedidos();
                        selectedOrder = null;
                        VaciarCamposPedido();
                        VaciarCamposArticulos();
                        VaciarEtiquetasTotales();
                        listaLinpeds.Clear();
                        dataGridLinpeds.ItemsSource = listaLinpeds;
                        dataGridLinpeds.IsEnabled = false;
                        cbUsu.IsEnabled = true;
                        dpFecha.IsEnabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Error al insertar el pedido", "Insertar pedido",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private bool InsertarLinpeds(string pedidoId)
        {
            int errores = 0;

            for (int i = 0; i < dataGridLinpeds.Items.Count; i++)
            {
                Linped linea = (Linped) dataGridLinpeds.Items[i];
                if (!neg.InsertarLinped(pedidoId, (i + 1).ToString(), linea.ArticuloID, linea.Importe, linea.Cantidad))
                    errores += 1;
            }

            return (errores == 0 ? true : false);
        }

        private void btnMod_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridLinpeds.Items.Count == 0)
            {
                MessageBox.Show("Debe añadir al menos un artículo", "Insertar pedido",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string pedidoId = selectedOrder.PedidoID;
                bool error = false;

                if (neg.GetLinpeds(pedidoId).Count != 0)
                {
                    error = !neg.EliminarLinpeds(pedidoId);
                }

                error = !InsertarLinpeds(pedidoId);

                if (error)
                {
                    MessageBox.Show("Error al modificar el pedido", "Modificar pedido",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Pedido modificado", "Modificar pedido",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                    LeerPedidos();
                    selectedOrder = null;
                    VaciarCamposPedido();
                    VaciarCamposArticulos();
                    VaciarEtiquetasTotales();
                    listaLinpeds.Clear();
                    dataGridLinpeds.ItemsSource = listaLinpeds;
                    dataGridLinpeds.IsEnabled = false;
                }
            }
        }

        private void btnEli_Click(object sender, RoutedEventArgs e)
        {
            string pedidoId = selectedOrder.PedidoID;

            MessageBoxResult result = MessageBox.Show("¿Seguro que quiere eliminar este pedido?\n\n" +
                    "ID: " + selectedOrder.PedidoID + "\nUsuario: " + neg.GetUsuario(selectedOrder.UsuarioID).Nombre
                    + "\nFecha: " + selectedOrder.Fecha + "\nLíneas de pedido: " + dataGridLinpeds.Items.Count
                    , "Eliminar pedido " + pedidoId, MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                bool error = false;

                if (neg.GetLinpeds(pedidoId).Count != 0)
                {
                    error = !neg.EliminarLinpeds(pedidoId);
                }

                error = !neg.EliminarPedido(pedidoId);

                if (error)
                {
                    MessageBox.Show("Error al eliminar el pedido", "Eliminar pedido",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Pedido eliminado", "Eliminar pedido",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                    LeerPedidos();
                    selectedOrder = null;
                    VaciarCamposPedido();
                    VaciarCamposArticulos();
                    VaciarEtiquetasTotales();
                    dataGridLinpeds.ItemsSource = listaLinpeds;
                    dataGridLinpeds.IsEnabled = false;
                }
            }
            else if (result == MessageBoxResult.No) { }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Articulo art = (Articulo) cbArt.SelectedItem;
            int importe = (art.Pvp == null ? 0 : Convert.ToInt32(art.Pvp));

            Linped linea = new Linped();
            linea.PedidoID = (selectedOrder != null ? selectedOrder.PedidoID : null);
            linea.ArticuloID = art.ArticuloID;
            linea.Linea = (dataGridLinpeds.Items.Count + 1).ToString();
            linea.ArticuloNombre = art.Nombre;
            linea.Importe = importe;
            linea.Cantidad = Convert.ToInt32(upCant.Value);

            listaLinpeds.Add(linea);

            ActualizarTotales();
            dataGridLinpeds.ItemsSource = listaLinpeds;
            dataGridLinpeds.IsEnabled = true;
            cbArt.SelectedItem = 0;
            upCant.Value = 1;
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridLinpeds.SelectedItems.Count == 1)
            {
                listaLinpeds.RemoveAt(dataGridLinpeds.SelectedIndex);
                dataGridLinpeds.ItemsSource = listaLinpeds;
                vistaLinpeds.Source = listaLinpeds;
                ActualizarTotales();
            }
        }

        private void dataGridLinpeds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridLinpeds.SelectedItems.Count != 0)
                btnDel.IsEnabled = true;
        }
    }
}
