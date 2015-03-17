CREATE TABLE [dbo].[Valores]
(
	[IdValores] INT NOT NULL IDENTITY , 
    [IdPaciente] INT NOT NULL, 
    [IdOBX] INT NOT NULL,
	[Sub_OBX_id] INT NULL, 
    [Sub_Descrição] VARCHAR(50) NULL, 
    [Parametro] NCHAR(10) NULL, 
    [TimeStamp] TEXT NULL, 
    CONSTRAINT [FK_idPaciente] FOREIGN KEY (IdPaciente) REFERENCES Paciente(IdPaciente),
	CONSTRAINT [FK_idOBX] FOREIGN KEY (IdOBX) REFERENCES Monitorização(IdOBX),
	CONSTRAINt [PK_Valores] PRIMARY KEY(IdPaciente, IdOBX,IdValores)
)
