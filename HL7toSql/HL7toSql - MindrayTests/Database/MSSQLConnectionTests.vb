Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports HL7toSql

Namespace HL7toSql.Tests
    <TestClass()> Public Class MSSQLConnectionTests

        <TestMethod()> Public Sub execQueryTest()
            Dim sqlConn = New MSSQLConnection("Initial Catalog=testes;Data Source=NETM4NULTRABOOK;Integrated Security=SSPI")
            Dim query As String = "insert into Paciente (IdPaciente, [Frist_Name_Paciente] ,[Last_Name_Paciente])" & _
                                  "VALUES ('-2','a','b')"
            Assert.IsTrue(sqlConn.execQuery(query))
            query = "delete from Paciente where IdPaciente like '-2'"
            Assert.IsTrue(sqlConn.execQuery(query))

            'se a query nao for valida
            query = "delete from qwerty where IdPaciente like '-2'"
            Assert.IsFalse(sqlConn.execQuery(query))
        End Sub

        <TestMethod()> Public Sub sendQueryTest()
            Dim sqlConn = New MSSQLConnection("Initial Catalog=testes;Data Source=NETM4NULTRABOOK;Integrated Security=SSPI")
            Dim query As String = "insert into Paciente (IdPaciente, [Frist_Name_Paciente] ,[Last_Name_Paciente])" & _
                                  "VALUES ('-2','a','b')"
            Assert.IsTrue(sqlConn.execQuery(query))
            query = "select IdPaciente from Paciente"
            Dim tb As DataTable = sqlConn.sendQuery(query)
            Dim r = tb.Rows(1).Item(0)
            Assert.AreEqual(r, "-2")
            query = "delete from Paciente where IdPaciente like '-2'"
            Assert.IsTrue(sqlConn.execQuery(query))
        End Sub
    End Class


End Namespace


