-- Base de datos OFICIAL convertida de SQL Server a SQLite
-- Nota: SQLite no admite CREATE PROCEDURE. Los procedimientos almacenados
-- deben implementarse como métodos C# usando Microsoft.Data.Sqlite o System.Data.SQLite.
-- Ejecutar con foreign_keys habilitado.
PRAGMA foreign_keys = ON;
BEGIN TRANSACTION;

CREATE TABLE IF NOT EXISTS DatosEmpresa (
    IdDatosEmpresa INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreEmpresa TEXT NOT NULL,
    CorreoEmpresa TEXT NOT NULL,
    TelefonoEmpresa TEXT NOT NULL,
    DireccionEmpresa TEXT NOT NULL,
    LogoEmpresa TEXT,
    FechaConfiguracion TEXT NOT NULL DEFAULT (datetime('now','localtime'))
);

CREATE TABLE IF NOT EXISTS TipoUsuario (
    IdTipoUsuario INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreTipoUsuario TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Gerente (
    IdGerente INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreGerente TEXT NOT NULL,
    ApellidoGerente TEXT NOT NULL,
    CorreoGerente TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS EstadoEmpleado (
    IdEstadoEmpleado INTEGER PRIMARY KEY AUTOINCREMENT,
    EstadoEmpleado TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Empleado (
    IdEmpleado INTEGER PRIMARY KEY AUTOINCREMENT,
    DUIEmpleado TEXT NOT NULL UNIQUE CHECK (DUIEmpleado GLOB '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]-[0-9]'),
    NombreEmpleado TEXT NOT NULL,
    ApellidoEmpleado TEXT NOT NULL,
    TelefonoEmpleado TEXT NOT NULL UNIQUE CHECK (TelefonoEmpleado GLOB '[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]'),
    CargoEmpleado TEXT NOT NULL,
    CorreoEmpleado TEXT NOT NULL UNIQUE,
    CuentaBancariaEmpleado TEXT NOT NULL,
    IdEstadoEmpleado INTEGER REFERENCES EstadoEmpleado(IdEstadoEmpleado),
    FotoEmpleado TEXT,
    IdGerente INTEGER REFERENCES Gerente(IdGerente),
    EliminadoEmpleado INTEGER NOT NULL DEFAULT 0 CHECK (EliminadoEmpleado IN (0,1))
);

CREATE TABLE IF NOT EXISTS Usuario (
    IdUsuario INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreUsuario TEXT NOT NULL UNIQUE,
    ContrasenaUsuario TEXT NOT NULL,
    IdTipoUsuario INTEGER NOT NULL REFERENCES TipoUsuario(IdTipoUsuario),
    IdGerente INTEGER REFERENCES Gerente(IdGerente),
    IdEmpleado INTEGER REFERENCES Empleado(IdEmpleado),
    EliminadoUsuario INTEGER NOT NULL DEFAULT 0 CHECK (EliminadoUsuario IN (0,1)),
    CodigoRecuperacion TEXT,
    ExpiracionCodigo TEXT
);

CREATE TABLE IF NOT EXISTS Documentacion (
    IdDocumentacion INTEGER PRIMARY KEY AUTOINCREMENT,
    FechaDocumentacion TEXT NOT NULL,
    FechaUltimoAcceso TEXT,
    NombreDocumentacion TEXT NOT NULL,
    ArchivoDocumentacion TEXT,
    DocumentacionFavorita INTEGER NOT NULL DEFAULT 0 CHECK (DocumentacionFavorita IN (0,1)),
    DocumentacionPapelera INTEGER NOT NULL DEFAULT 0 CHECK (DocumentacionPapelera IN (0,1)),
    IdGerente INTEGER REFERENCES Gerente(IdGerente)
);

CREATE TABLE IF NOT EXISTS Proveedor (
    IdProveedor INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreProveedor TEXT NOT NULL,
    CorreoProveedor TEXT NOT NULL,
    TelefonoProveedor TEXT NOT NULL UNIQUE CHECK (TelefonoProveedor GLOB '[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]'),
    FotoProveedor TEXT,
    IdGerente INTEGER REFERENCES Gerente(IdGerente),
    EliminadoProveedor INTEGER NOT NULL DEFAULT 0 CHECK (EliminadoProveedor IN (0,1))
);

CREATE TABLE IF NOT EXISTS Reporte (
    IdReporte INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreReporte TEXT NOT NULL,
    DescripcionReporte TEXT NOT NULL,
    FechaReporte TEXT NOT NULL,
    IdGerente INTEGER REFERENCES Gerente(IdGerente)
);

CREATE TABLE IF NOT EXISTS TipoCliente (
    IdTipoCliente INTEGER PRIMARY KEY AUTOINCREMENT,
    TipoCliente TEXT
);

CREATE TABLE IF NOT EXISTS Cliente (
    IdCliente INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreCliente TEXT NOT NULL,
    ApellidoCliente TEXT NOT NULL,
    NCRCliente TEXT CHECK (NCRCliente IS NULL OR length(NCRCliente)=8),
    DUICliente TEXT CHECK (DUICliente IS NULL OR DUICliente GLOB '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]-[0-9]'),
    NITCliente TEXT CHECK (NITCliente IS NULL OR NITCliente GLOB '[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9][0-9][0-9]-[0-9][0-9][0-9]-[0-9]'),
    TelefonoCliente TEXT NOT NULL UNIQUE CHECK (TelefonoCliente GLOB '[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]'),
    CorreoCliente TEXT NOT NULL UNIQUE,
    IdTipoCliente INTEGER NOT NULL REFERENCES TipoCliente(IdTipoCliente),
    EstadoCliente INTEGER NOT NULL DEFAULT 0 CHECK (EstadoCliente IN (0,1)),
    EliminadoCliente INTEGER NOT NULL DEFAULT 0 CHECK (EliminadoCliente IN (0,1))
);

CREATE TABLE IF NOT EXISTS Fecha (
    IdFecha INTEGER PRIMARY KEY AUTOINCREMENT,
    DiaFecha TEXT NOT NULL,
    LugarFecha TEXT NOT NULL,
    HoraFecha TEXT NOT NULL,
    AsuntoFecha TEXT NOT NULL,
    IdCliente INTEGER NOT NULL REFERENCES Cliente(IdCliente),
    IdUsuario INTEGER REFERENCES Usuario(IdUsuario)
);

CREATE TABLE IF NOT EXISTS Servicio (
    IdServicio INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreServicio TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Producto (
    IdProducto INTEGER PRIMARY KEY AUTOINCREMENT,
    NombreProducto TEXT NOT NULL,
    CantidadProducto INTEGER NOT NULL CHECK (CantidadProducto >= 0),
    PrecioCostoProducto NUMERIC NOT NULL,
    PrecioALquilerProducto NUMERIC NOT NULL,
    PrecioPerdidaProducto NUMERIC NOT NULL,
    IdServicio INTEGER NOT NULL REFERENCES Servicio(IdServicio),
    IdProveedor INTEGER NOT NULL REFERENCES Proveedor(IdProveedor),
    ImagenProducto TEXT,
    EliminadoProducto INTEGER NOT NULL DEFAULT 0 CHECK (EliminadoProducto IN (0,1))
);

CREATE TABLE IF NOT EXISTS EstadoVenta (
    IdEstadoVenta INTEGER PRIMARY KEY AUTOINCREMENT,
    EstadoVenta TEXT
);

CREATE TABLE IF NOT EXISTS TipoPago (
    IdTipoPago INTEGER PRIMARY KEY AUTOINCREMENT,
    TipoPago TEXT
);

CREATE TABLE IF NOT EXISTS Venta (
    IdVenta INTEGER PRIMARY KEY AUTOINCREMENT,
    FechaVenta TEXT NOT NULL,
    FechaUso TEXT NOT NULL,
    IdEmpleado INTEGER NOT NULL REFERENCES Empleado(IdEmpleado),
    IdCliente INTEGER NOT NULL REFERENCES Cliente(IdCliente),
    SubtotalVenta NUMERIC NOT NULL DEFAULT 0,
    IVA NUMERIC NOT NULL DEFAULT 0,
    DescuentoVenta NUMERIC DEFAULT 0,
    TotalVenta NUMERIC NOT NULL DEFAULT 0,
    IdEstadoVenta INTEGER NOT NULL REFERENCES EstadoVenta(IdEstadoVenta),
    IdTipoPago INTEGER NOT NULL REFERENCES TipoPago(IdTipoPago)
);

CREATE TABLE IF NOT EXISTS DetalleVenta (
    IdDetalleVenta INTEGER PRIMARY KEY AUTOINCREMENT,
    IdVenta INTEGER NOT NULL REFERENCES Venta(IdVenta),
    IdProducto INTEGER NOT NULL REFERENCES Producto(IdProducto),
    Cantidad INTEGER NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario NUMERIC NOT NULL,
    Descuento NUMERIC DEFAULT 0,
    TotalDetalleventa NUMERIC NOT NULL
);

CREATE TABLE IF NOT EXISTS Factura (
    IdFactura INTEGER PRIMARY KEY AUTOINCREMENT,
    NumeroFactura TEXT NOT NULL UNIQUE,
    NumeroControlFactura INTEGER UNIQUE,
    SelloRecepcion TEXT,
    Fecha TEXT NOT NULL,
    Hora TEXT,
    TipoDocumento TEXT NOT NULL,
    Sucursal TEXT,
    IdVenta INTEGER NOT NULL REFERENCES Venta(IdVenta)
);

CREATE TABLE IF NOT EXISTS RegistoPago (
    IdRegistroPago INTEGER PRIMARY KEY AUTOINCREMENT,
    EstadoPago TEXT,
    FechaPago TEXT NOT NULL,
    IdServicio INTEGER NOT NULL REFERENCES Servicio(IdServicio),
    IdVenta INTEGER NOT NULL REFERENCES Venta(IdVenta),
    IdCliente INTEGER NOT NULL REFERENCES Cliente(IdCliente)
);

-- Datos iniciales que estaban en los INSERT de SQL Server.
INSERT INTO TipoUsuario (NombreTipoUsuario) SELECT 'Gerente' WHERE NOT EXISTS (SELECT 1 FROM TipoUsuario WHERE NombreTipoUsuario='Gerente');
INSERT INTO TipoUsuario (NombreTipoUsuario) SELECT 'Empleado' WHERE NOT EXISTS (SELECT 1 FROM TipoUsuario WHERE NombreTipoUsuario='Empleado');
INSERT INTO Gerente (NombreGerente, ApellidoGerente, CorreoGerente) SELECT 'Carlos ', 'Martínez', 'gerente@eventosmaquilishuat.com' WHERE NOT EXISTS (SELECT 1 FROM Gerente WHERE CorreoGerente='gerente@eventosmaquilishuat.com');
INSERT INTO EstadoEmpleado (EstadoEmpleado) SELECT 'Activo' WHERE NOT EXISTS (SELECT 1 FROM EstadoEmpleado WHERE EstadoEmpleado='Activo');
INSERT INTO EstadoEmpleado (EstadoEmpleado) SELECT 'Inactivo' WHERE NOT EXISTS (SELECT 1 FROM EstadoEmpleado WHERE EstadoEmpleado='Inactivo');
INSERT INTO EstadoEmpleado (EstadoEmpleado) SELECT 'Descanso' WHERE NOT EXISTS (SELECT 1 FROM EstadoEmpleado WHERE EstadoEmpleado='Descanso');
INSERT INTO EstadoEmpleado (EstadoEmpleado) SELECT 'Permiso' WHERE NOT EXISTS (SELECT 1 FROM EstadoEmpleado WHERE EstadoEmpleado='Permiso');
INSERT INTO EstadoEmpleado (EstadoEmpleado) SELECT 'Vacaciones' WHERE NOT EXISTS (SELECT 1 FROM EstadoEmpleado WHERE EstadoEmpleado='Vacaciones');
INSERT INTO TipoCliente (TipoCliente) SELECT 'Jurídico' WHERE NOT EXISTS (SELECT 1 FROM TipoCliente WHERE TipoCliente='Jurídico');
INSERT INTO TipoCliente (TipoCliente) SELECT 'Natural' WHERE NOT EXISTS (SELECT 1 FROM TipoCliente WHERE TipoCliente='Natural');
INSERT INTO Servicio (NombreServicio) SELECT 'Alquiler de mobiliario' WHERE NOT EXISTS (SELECT 1 FROM Servicio WHERE NombreServicio='Alquiler de mobiliario');
INSERT INTO Servicio (NombreServicio) SELECT 'Cristaleria' WHERE NOT EXISTS (SELECT 1 FROM Servicio WHERE NombreServicio='Cristaleria');
INSERT INTO Servicio (NombreServicio) SELECT 'Cuberteria' WHERE NOT EXISTS (SELECT 1 FROM Servicio WHERE NombreServicio='Cuberteria');
INSERT INTO Servicio (NombreServicio) SELECT 'Discomóvil e iluminación' WHERE NOT EXISTS (SELECT 1 FROM Servicio WHERE NombreServicio='Discomóvil e iluminación');
INSERT INTO Servicio (NombreServicio) SELECT 'Pista de baile y servicio de animación' WHERE NOT EXISTS (SELECT 1 FROM Servicio WHERE NombreServicio='Pista de baile y servicio de animación');
INSERT INTO Servicio (NombreServicio) SELECT 'Decoración' WHERE NOT EXISTS (SELECT 1 FROM Servicio WHERE NombreServicio='Decoración');
INSERT INTO EstadoVenta (EstadoVenta) SELECT 'En Proceso' WHERE NOT EXISTS (SELECT 1 FROM EstadoVenta WHERE EstadoVenta='En Proceso');
INSERT INTO EstadoVenta (EstadoVenta) SELECT 'Pendiente' WHERE NOT EXISTS (SELECT 1 FROM EstadoVenta WHERE EstadoVenta='Pendiente');
INSERT INTO EstadoVenta (EstadoVenta) SELECT 'Cancelada' WHERE NOT EXISTS (SELECT 1 FROM EstadoVenta WHERE EstadoVenta='Cancelada');
INSERT INTO EstadoVenta (EstadoVenta) SELECT 'Finalizada' WHERE NOT EXISTS (SELECT 1 FROM EstadoVenta WHERE EstadoVenta='Finalizada');
INSERT INTO TipoPago (TipoPago) SELECT 'Efectivo' WHERE NOT EXISTS (SELECT 1 FROM TipoPago WHERE TipoPago='Efectivo');
INSERT INTO TipoPago (TipoPago) SELECT 'Transferencia' WHERE NOT EXISTS (SELECT 1 FROM TipoPago WHERE TipoPago='Transferencia');
INSERT INTO TipoPago (TipoPago) SELECT 'Tarjeta' WHERE NOT EXISTS (SELECT 1 FROM TipoPago WHERE TipoPago='Tarjeta');

-- Vistas adaptadas a la sintaxis SQLite.
CREATE VIEW IF NOT EXISTS VistaEmpleadoProductos AS
SELECT P.IdProducto, P.NombreProducto, P.CantidadProducto, P.PrecioAlquilerProducto, P.ImagenProducto, P.EliminadoProducto
FROM Producto P INNER JOIN Proveedor Pr ON P.IdProveedor=Pr.IdProveedor;

CREATE VIEW IF NOT EXISTS VistaProductosGerente AS
SELECT P.IdProducto, P.NombreProducto, P.CantidadProducto, P.PrecioCostoProducto, P.PrecioAlquilerProducto, P.PrecioPerdidaProducto, S.NombreServicio, Pr.NombreProveedor
FROM Producto P INNER JOIN Servicio S ON P.IdServicio=S.IdServicio INNER JOIN Proveedor Pr ON P.IdProveedor=Pr.IdProveedor;

CREATE VIEW IF NOT EXISTS VistaEmpleadoServicios AS
SELECT P.IdServicio, P.ImagenProducto, P.NombreProducto, P.CantidadProducto, P.PrecioAlquilerProducto, S.NombreServicio, P.IdProducto, P.EliminadoProducto
FROM Producto P INNER JOIN Servicio S ON P.IdServicio=S.IdServicio;

CREATE VIEW IF NOT EXISTS VistaServicioEmpleado AS SELECT NombreServicio FROM Servicio;

CREATE VIEW IF NOT EXISTS VistaDatosCliente AS
SELECT C.IdCliente, C.NombreCliente, C.ApellidoCliente, C.TelefonoCliente, C.NCRCliente, C.DUICliente, C.NITCliente, C.CorreoCliente, TC.TipoCliente, C.EstadoCliente, C.EliminadoCliente,
CASE WHEN EXISTS (SELECT 1 FROM Venta V WHERE V.IdCliente=C.IdCliente AND (V.IdEstadoVenta IN (1,2) OR V.FechaUso >= date('now','localtime'))) THEN 1 ELSE 0 END AS EsClienteActivo
FROM Cliente C INNER JOIN TipoCliente TC ON TC.IdTipoCliente=C.IdTipoCliente WHERE C.EliminadoCliente=0;

CREATE VIEW IF NOT EXISTS VistaClientesActivos AS SELECT * FROM Cliente WHERE EstadoCliente=1;
CREATE VIEW IF NOT EXISTS VistaClientesInactivos AS SELECT * FROM Cliente WHERE EstadoCliente=0;

CREATE VIEW IF NOT EXISTS VistaRegistroPagoGerente AS
SELECT RP.IdRegistroPago, C.NombreCliente, C.ApellidoCliente, C.CorreoCliente, C.TelefonoCliente, S.NombreServicio, V.TotalVenta, RP.FechaPago, RP.EstadoPago
FROM RegistoPago RP INNER JOIN Cliente C ON RP.IdCliente=C.IdCliente INNER JOIN Servicio S ON RP.IdServicio=S.IdServicio INNER JOIN Venta V ON RP.IdVenta=V.IdVenta;

CREATE VIEW IF NOT EXISTS VistaDocumentacionGerente AS SELECT IdDocumentacion, NombreDocumentacion, FechaDocumentacion, ArchivoDocumentacion, DocumentacionFavorita, DocumentacionPapelera FROM Documentacion WHERE DocumentacionPapelera=0;
CREATE VIEW IF NOT EXISTS VistaDocumentosFavoritos AS SELECT IdDocumentacion, NombreDocumentacion, FechaDocumentacion, ArchivoDocumentacion, DocumentacionFavorita, DocumentacionPapelera FROM Documentacion WHERE DocumentacionPapelera=0 AND DocumentacionFavorita=1;
CREATE VIEW IF NOT EXISTS VistaDocumentosPapelera AS SELECT IdDocumentacion, NombreDocumentacion, FechaDocumentacion, ArchivoDocumentacion, DocumentacionFavorita, DocumentacionPapelera FROM Documentacion WHERE DocumentacionPapelera=1;

CREATE VIEW IF NOT EXISTS VistaEmpleadoGerente AS
SELECT E.IdEmpleado, E.NombreEmpleado, E.ApellidoEmpleado, E.DUIEmpleado, E.TelefonoEmpleado, E.CorreoEmpleado, E.CargoEmpleado, E.IdEstadoEmpleado, EE.EstadoEmpleado, E.FotoEmpleado, E.EliminadoEmpleado
FROM Empleado E INNER JOIN EstadoEmpleado EE ON E.IdEstadoEmpleado=EE.IdEstadoEmpleado;

CREATE VIEW IF NOT EXISTS VistaProveedorGerente AS SELECT IdProveedor, NombreProveedor, CorreoProveedor, TelefonoProveedor, FotoProveedor FROM Proveedor WHERE EliminadoProveedor=0;
CREATE VIEW IF NOT EXISTS VistaReporteGerente AS SELECT IdReporte, NombreReporte, DescripcionReporte, FechaReporte FROM Reporte;

CREATE VIEW IF NOT EXISTS VistaFactura AS
SELECT F.IdFactura, F.NumeroFactura, F.NumeroControlFactura, F.SelloRecepcion, F.Fecha AS FechaFactura, F.Hora AS HoraFactura, F.TipoDocumento, F.Sucursal,
V.IdVenta, V.FechaVenta, V.FechaUso, V.SubtotalVenta, V.IVA, V.DescuentoVenta, V.TotalVenta,
CASE WHEN V.FechaUso > date('now','localtime') THEN 'Pendiente' WHEN V.FechaUso=date('now','localtime') THEN 'En Curso' WHEN V.FechaUso < date('now','localtime') THEN 'Finalizado' ELSE 'No Valido' END AS EstadoEvento,
C.NombreCliente, C.ApellidoCliente, C.DUICliente, C.NITCliente, C.CorreoCliente, E.NombreEmpleado, TP.TipoPago, EV.EstadoVenta
FROM Factura F INNER JOIN Venta V ON F.IdVenta=V.IdVenta INNER JOIN Cliente C ON V.IdCliente=C.IdCliente INNER JOIN Empleado E ON V.IdEmpleado=E.IdEmpleado INNER JOIN TipoPago TP ON V.IdTipoPago=TP.IdTipoPago INNER JOIN EstadoVenta EV ON V.IdEstadoVenta=EV.IdEstadoVenta;

CREATE VIEW IF NOT EXISTS VistaRegistroVentaGerente AS
SELECT V.IdVenta, E.NombreEmpleado || ' ' || E.ApellidoEmpleado AS NombreEmpleado, C.NombreCliente, C.ApellidoCliente, EV.EstadoVenta, V.FechaVenta, V.FechaUso, V.TotalVenta,
(SELECT COUNT(*) FROM DetalleVenta DV WHERE DV.IdVenta=V.IdVenta) AS ItemsVendidos
FROM Venta V INNER JOIN Cliente C ON V.IdCliente=C.IdCliente INNER JOIN Empleado E ON V.IdEmpleado=E.IdEmpleado INNER JOIN EstadoVenta EV ON V.IdEstadoVenta=EV.IdEstadoVenta;

-- Importante: los triggers INSTEAD OF de SQL Server no se pueden crear sobre tablas en SQLite.
-- Para conservar el borrado lógico, el código C# debe ejecutar UPDATE tabla SET Eliminado...=1.
-- Las validaciones de cliente/producto/empleado también deben ejecutarse en métodos C#
-- dentro de una transacción antes de cambiar el indicador de eliminado.

COMMIT;
