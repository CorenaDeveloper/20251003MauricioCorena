using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PruebaTecnica.Migrations
{
    /// <inheritdoc />
    public partial class AgregarProcedimientoAlmacenado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_ObtenerProductos
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    SELECT 
                        Id,
                        Nombre,
                        Descripcion,
                        PrecioBase,
                        PrecioConDescuento,
                        ISNULL(PrecioConDescuento, PrecioBase) AS PrecioActual,
                        Imagen,
                        FechaCreacion,
                        FechaModificacion
                    FROM Productos
                    ORDER BY FechaCreacion DESC;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ObtenerProductos");
        }
    }
}
