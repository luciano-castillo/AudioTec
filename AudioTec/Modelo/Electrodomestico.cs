using AudioTec.Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioTec.Modelo
{
    public class Electrodomestico
    {

        public string ElectrodomesticoID { get; set; }
        public Cliente Dueno { get; set; }
        public string Articulo { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public string Observacion { get; set; }


        // Guardar Electrodomestico
        public bool Crear()
        {
            bool respuesta = true;

            if (Dueno != null)
            {
                if (!ElectrodomesticoLogica.Existe(ElectrodomesticoID))
                {
                    respuesta = ElectrodomesticoLogica.GuardarElectrodomestico(this);
                }
                
            }

            return respuesta;
        }

        public bool CompararElectrodomestico(Electrodomestico electro)
        {
            bool respuesta = false;

            if (Articulo != electro.Articulo || 
                Modelo != electro.Modelo || 
                Marca != electro.Marca)
            {
                respuesta = true;
            }

            return respuesta;
        }

        public void EditarElectrodomestico(Electrodomestico electro)
        {

            if (ElectrodomesticoLogica.EditarElectrodomestico(electro, ElectrodomesticoID))
            {
                Articulo = electro.Articulo;
                Modelo = electro.Modelo;
                Marca = electro.Marca;
            }
            else
            {
                MessageBox.Show("No se pudo editar el electrodomestico!");
            }
        }

        public bool Vacio()
        {
            bool respuesta = false;

            if (Articulo != null)
            {
                respuesta = true;
            }

            return respuesta;
        }

    }
}
