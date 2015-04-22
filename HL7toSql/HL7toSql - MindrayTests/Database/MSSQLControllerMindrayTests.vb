Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports HL7toSql

Namespace HL7toSql.Tests
    <TestClass()> Public Class MSSQLControllerMindrayTests

        <TestMethod()> Public Sub addMSGtoDBTest()
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|503|P|2.3.1|" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           "OBX||NM|172^Mean|2105|89||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           "OBX||NM|170^Sys|2105|135||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           Chr(28)

            Dim c As New MSSQLControllerMindray()
            c.setidPaciente("-1") 'paciente teste
            Dim m As New Message(msgtest)
            c.addMSGtoDB(m)

            Dim r = c.getTable("Monitorizacao").Tables(0).Rows(0).Item(5).ToString
            Dim tSize = c.getTable("Monitorizacao").Tables(0).Rows.Count

            Assert.AreEqual(r, _
                           "171")

            Assert.AreEqual(tSize, _
                           3)
            Limpar()
        End Sub

        <TestMethod()> Public Sub addMSGtoDBTest_AgruparRepetidos()
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|503|P|2.3.1|" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           "OBX||NM|172^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           Chr(28)

            Dim c As New MSSQLControllerMindray()
            c.setidPaciente("-1") 'paciente teste
            Dim m As New Message(msgtest)
            c.addMSGtoDB(m)

            Dim r = c.getTable("Monitorizacao").Tables(0).Rows(0).Item(5).ToString
            Dim tSize = c.getTable("Monitorizacao").Tables(0).Rows.Count

            Assert.AreEqual(r, _
                           "171")

            Assert.AreEqual(tSize, _
                           2)
            Limpar()
        End Sub

        Private Sub Limpar()
            Dim sqlConn = New MSSQLConnection("Initial Catalog=testes;Data Source=NETM4NULTRABOOK;Integrated Security=SSPI")
            Dim query As String = "delete from Monitorizacao where IdPaciente like '-1'"
            Assert.IsTrue(sqlConn.execQuery(query))
            'Assert.Fail()
        End Sub
    End Class


End Namespace


