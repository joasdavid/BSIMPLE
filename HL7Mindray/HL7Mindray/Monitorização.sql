CREATE TABLE [dbo].[Monitorização]
(
	[IdOBX] INT NOT NULL PRIMARY KEY, 
    [IdPaciente] INT NOT NULL, 
    [Descrição_Id] VARCHAR(15) NULL, 
    CONSTRAINT [FK_Monitorização_ToTable] FOREIGN KEY (IdPaciente) REFERENCES Paciente(IdPaciente)

)

