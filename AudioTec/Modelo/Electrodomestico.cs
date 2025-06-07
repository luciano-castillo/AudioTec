using AudioTec.Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
