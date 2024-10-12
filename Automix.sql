use automix;

CREATE TABLE Moradas (
    IDMorada     INT           IDENTITY (1, 1) NOT NULL,
    Morada       VARCHAR (300) NULL,
    Localidade   VARCHAR (200) NULL,
    CodigoPostal VARCHAR (8)   NULL,
    PRIMARY KEY CLUSTERED (IDMorada ASC)
);

CREATE TABLE Contactos (
    IDContacto          INT           IDENTITY (1, 1) NOT NULL,
    Email               VARCHAR (100) NOT NULL,
    Contacto            INT           NULL,
    ContactoAlternativo INT           NULL,
    PRIMARY KEY CLUSTERED (IDContacto ASC),
    UNIQUE NONCLUSTERED (Email ASC),
);

CREATE TABLE Documentos (
    IDDocumento INT             IDENTITY (1, 1) NOT NULL,
    Documento   VARBINARY (MAX) NOT NULL,
    Extensao    CHAR (4)        NOT NULL,
    Nome        VARCHAR (100)   NOT NULL,
    CONSTRAINT [PK_IDDocumento] PRIMARY KEY CLUSTERED (IDDocumento ASC)
);

CREATE TABLE Alunos (
    IDAluno        INT           IDENTITY (1, 1) NOT NULL,
    NIFAluno       INT           NOT NULL,  
    IDContacto     INT           NOT NULL,
	  IDMorada       INT           NOT NULL,
    IDDocumento    INT           NOT NULL,
    Nome           VARCHAR (200) NOT NULL,
    Foto           VARBINARY (MAX) NULL,      
    PRIMARY KEY CLUSTERED (IDAluno ASC),
    CONSTRAINT [FK_IDContacto] FOREIGN KEY (IDContacto) REFERENCES Contactos (IDContacto) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_Morada] FOREIGN KEY (IDMorada) REFERENCES Moradas (IDMorada) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_IDDocumento] FOREIGN KEY (IDDocumento) REFERENCES Documentos (IDDocumento) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Percursos (
    IDPercurso INT IDENTITY (1, 1) NOT NULL,
    Nome       VARCHAR (200) NOT NULL,
    Status     BIT NOT NULL,
    PRIMARY KEY CLUSTERED (IDPercurso ASC)
);

CREATE TABLE PercursosAlunos (
    IDPercursoAluno  INT IDENTITY (1, 1) NOT NULL,
    IDAluno          INT NOT NULL,
    IDPercurso       INT NOT NULL,
    PRIMARY KEY CLUSTERED (IDPercursoAluno ASC),
    CONSTRAINT [FK_IDAluno2] FOREIGN KEY (IDAluno) REFERENCES Alunos (IDAluno), 
    CONSTRAINT [FK_IDPercurso] FOREIGN KEY (IDPercurso) REFERENCES Percursos (IDPercurso) 
);

CREATE TABLE Cargos (
    IDCargo    INT           IDENTITY (1, 1) NOT NULL,
    Cargo      VARCHAR (100) NOT NULL,
    Modulos    VARCHAR (300) NULL,
    PRIMARY KEY CLUSTERED (IDCargo ASC)
);


CREATE TABLE Funcionarios (
	IDFuncionario  INT           IDENTITY (1, 1) NOT NULL,
    NIFFuncionario INT           NOT NULL,
    IDCargo        INT           NOT NULL,
    IDContacto     INT           NOT NULL,
	  IDMorada       INT           NOT NULL,
    Nome           VARCHAR (200) NOT NULL,
    Foto           VARBINARY (MAX) NULL,
    PRIMARY KEY CLUSTERED (IDFuncionario ASC),
    CONSTRAINT [FK_IDCargo] FOREIGN KEY (IDCargo) REFERENCES Cargos (IDCargo),
    CONSTRAINT [FK_IDContacto2] FOREIGN KEY (IDContacto) REFERENCES Contactos (IDContacto) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_Morada2] FOREIGN KEY (IDMorada) REFERENCES Moradas (IDMorada) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Logins (
    [IDLogin]          INT           IDENTITY (1, 1) NOT NULL,
    [IDFuncionario]    INT           NOT NULL,
    [Password]         VARCHAR (255) NOT NULL,
    [AccountSecretKey] VARCHAR (10)  NULL,
    [Status]           INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([IDLogin] ASC),
    CONSTRAINT [FK_IDFuncionario] FOREIGN KEY (IDFuncionario) REFERENCES [dbo].[Funcionarios] (IDFuncionario) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Aulas (
    IDAula               INT           IDENTITY (1, 1) NOT NULL,
    IDFuncionario        INT           NOT NULL,
    IDAluno              INT           NOT NULL,
    Data                 DATETIME      NOT NULL,
    CONSTRAINT [PK_IDAula] PRIMARY KEY CLUSTERED (IDAula ASC),
    CONSTRAINT [FK_IDFuncionario2] FOREIGN KEY (IDFuncionario) REFERENCES Funcionarios (IDFuncionario),
    CONSTRAINT [FK_IDAluno] FOREIGN KEY (IDAluno) REFERENCES Alunos (IDAluno)
);
