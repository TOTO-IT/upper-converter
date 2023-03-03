El proyecto consiste en Agarrar una tanda de x clientes (base de dato totolink)
Que cumplan con las condicionales dadas por la consulta general, y se les realiza un ToUpper a los campos seleccionados. (Nombre, Domicilio y Localidad2).


Consulta usada en traer a los clientes:

SELECT distinct top 10 cli.IdCliente, cli.Nombre, cli.IdClienteAlternativo, 
cli.FechaNacimiento, cli.Teléfonos, cli.Celular, 
cli.EMail, cli.Sexo, cliP.IdClientesClase, cli.Domicilio, 
cli.Localidad2, cli.IdDepartamento
FROM totolink.dbo.clientes cli 
join poscomm.dbo.clientes cliP on clip.IdCliente = cli.IdCliente
WHERE cli.IdCliente not in (SELECT isnull(IdCliente,0) 
                            FROM TOTOLINK.DBO.LogClienteParseMayus) AND
ISNULL(cli.Domicilio, '*****') <> '*****' AND
ISNULL(cli.Localidad2, '*****') <> '*****' AND
cliP.IdClientesClase > 200 AND
cli.IdClientesClase = clip.IdClientesClase AND
cli.IdRegistro = (SELECT TOP 1 IdRegistro 
                  FROM totolink.dbo.clientes 
                  WHERE IdCliente = cli.IdCliente 
                  ORDER BY IdRegistro DESC) AND
UPPER(cli.Nombre) != cli.Nombre COLLATE SQL_Latin1_General_CP1_CS_AS AND
UPPER(cli.Domicilio) != cli.Domicilio COLLATE SQL_Latin1_General_CP1_CS_AS AND 
UPPER(cli.Localidad2) != cli.Localidad2 COLLATE SQL_Latin1_General_CP1_CS_AS
