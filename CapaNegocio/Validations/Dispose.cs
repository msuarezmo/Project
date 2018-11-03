//-----------------------------------------------------------------------
// <copyright file="Dispose.cs" company="COEF">
//    Todos los derechos reservados
// </copyright>
//-----------------------------------------------------------------------

namespace CapaNegocio.Validations
{
    /// <summary>
    /// Defines the <see cref="Dispose" />
    /// </summary>
    public class Dispose : Datamodel
    {
        /// <summary>
        /// The Liberate
        /// </summary>
        public void Liberate()
        {
            db.Dispose();
        }
    }
}
