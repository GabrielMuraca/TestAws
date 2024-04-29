using BarcodeLib;
using Microsoft.Reporting.WinForms;
using PuntosTateti.GeneradorTarjeta;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace PuntosTateti
{
    public class MetodosCredencial
    {
        public void MostrarCredencial(Cliente cliente)
        {

            Barcode Codigo = new Barcode();
            Codigo.IncludeLabel = true;
            Image ImagenCodigo = Codigo.Encode(TYPE.CODE128, cliente.Codigo, Color.Black, Color.White, 400, 100);

            string urlImagenCodigo = ConstantesGeneradorTarjeta.RUTA_CODIGO_TEMPORAL + cliente.Codigo.ToString() + "-tmp.jpeg";
            try
            {
                ImagenCodigo.Save(urlImagenCodigo, ImageFormat.Jpeg);

                GeneradorCredencial GenCre = new GeneradorCredencial();

                ReportParameter pNombre = new ReportParameter("pNombre", cliente.Nombre + " " + cliente.Apellido);
                ReportParameter pSocio = new ReportParameter("pSocio", cliente.NroSocio.ToString());
                ReportParameter pImagen = new ReportParameter("pImagen", new Uri(urlImagenCodigo).AbsoluteUri);


                GenCre.reportViewer1.LocalReport.EnableExternalImages = true;
                GenCre.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { pImagen, pNombre, pSocio });
                GenCre.reportViewer1.RefreshReport();
                GenCre.ShowDialog();

                File.Delete(urlImagenCodigo);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al generar la imagen en la ruta de destino: " + e);
            }
            
        }
    }
}
