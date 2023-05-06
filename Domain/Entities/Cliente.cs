using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cliente : AuditableBaseEntity
    {
        private int _edad;

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public int Edad 
        { 
            get
            {
                if (_edad <= 0)
                {
                    _edad = new DateTime(DateTime.Now.Subtract(FechaNacimiento).Ticks).Year - 1;
                }
                return _edad;
            }
        }
    }
}
