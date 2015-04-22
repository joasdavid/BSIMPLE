Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports HL7toSql

Namespace HL7toSql.Tests
    <TestClass()> Public Class MessageTests
#Region "Valide testes"
        <TestMethod()> Public Sub Valide_103_Test()
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|103|P|2.3.1|" & Chr(10) & _
                           "PID|||000f1404-d361-720c-180d27150010f910||joas^v.pereira||19900206|F|" & Chr(10) & _
                           "PV1||I|^^UCIP&8&169722888&4601&&1|||||||||||||||A|" & Chr(10) & _
                           "OBR||||Mindray Monitor|||0|" & Chr(10) & _
                           "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                           "OBX||NM|51^||54.0||||||F" & Chr(10) & _
                           "OBX||ST|2301^||753455||||||F" & Chr(10) & _
                           "OBX||CE|2302^Sangue||0^N||||||F" & Chr(10) & _
                           "OBX||CE|2303^Mpasso||2^||||||F" & Chr(10) & _
                           "OBX||ST|2308^BedNoStr||08||||||F" & Chr(10) & _
                           Chr(28)
            Dim msg As New Message(msgtest)
            Assert.IsTrue(msg.Valide)

        End Sub
        <TestMethod()> Public Sub Valide_503_Test()
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|503|P|2.3.1|" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           "OBX||NM|172^Mean|2105|89||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           "OBX||NM|170^Sys|2105|135||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           Chr(28)

            Dim msg As New Message(msgtest)
            Assert.IsTrue(msg.Valide)

        End Sub
        <TestMethod()> Public Sub Valide_53_Test()
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|53|P|2.3.1|" & Chr(10) & _
                           "OBX||CE|2009^|170|2^||||||F" & Chr(10) & _
                           "OBX||CE|2009^|172|2^||||||F" & Chr(10) & _
                           "OBX||CE|2009^|171|2^||||||F" & Chr(10) & _
                           Chr(28)

            Dim msg As New Message(msgtest)
            Assert.IsTrue(msg.Valide)

            'msgtest = Chr(11) & _
            '               "MSH|^~\&|||||||ORU^R01|53|P|2.3.1|" & Chr(10) & _
            '               Chr(28)

            'Dim msg2 As New Message(msgtest)
            'Assert.IsFalse(msg2.Valide)

        End Sub
        <TestMethod()> Public Sub Valide_MSH1_Test()
            'não tem enconding chars
            Dim msgtest = Chr(11) & _
                           "MSH||||||||ORU^R01|53|P|2.5|" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           Chr(28)

            Dim msg As New Message(msgtest)
            Assert.IsFalse(msg.Valide)

            'tem mais que 4 enconding chars
            msgtest = Chr(11) & _
                           "MSH|^~\&1|||||||ORU^R01|53|P|2.5|" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           Chr(28)

            Dim msg2 As New Message(msgtest)
            Assert.IsFalse(msg2.Valide)

            'carateres repetidos
            msgtest = Chr(11) & _
                           "MSH|^&\&|||||||ORU^R01|53|P|2.5|" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           Chr(28)

            Dim msg3 As New Message(msgtest)
            Assert.IsFalse(msg3.Valide)

            'numero de carateres infalido
            msgtest = Chr(11) & _
                           "MSH|^|||||||ORU^R01|53|P|2.5|" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           Chr(28)

            Dim msg4 As New Message(msgtest)
            Assert.IsFalse(msg4.Valide)
        End Sub
        <TestMethod()> Public Sub Valide_MSH9_Test()
            'não tem control id
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01||P|2.3.1|" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           Chr(28)

            Dim msg As New Message(msgtest)
            Assert.IsFalse(msg.Valide)

            'correto
            msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|53|P|2.3.1|" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           Chr(28)
            Dim msg2 As New Message(msgtest)
            Assert.IsTrue(msg2.Valide)
        End Sub
        <TestMethod()> Public Sub Valide_MSH10_Test()
            'não tem processing id
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&||||||||53||2.3.1|" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           Chr(28)

            Dim msg As New Message(msgtest)
            Assert.IsFalse(msg.Valide)

            'correto
            msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|53|P|2.3.1|" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           Chr(28)
            Dim msg2 As New Message(msgtest)
            Assert.IsTrue(msg2.Valide)
        End Sub
        <TestMethod()> Public Sub Valide_MSH11_Test()
            'não tem versão
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&||||||||53|P||" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           Chr(28)

            Dim msg As New Message(msgtest)
            Assert.IsFalse(msg.Valide)

            'tem versão errada
            msgtest = Chr(11) & _
                           "MSH|^~\&||||||||53|P|2.4|" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           Chr(28)

            Dim msg2 As New Message(msgtest)
            Assert.IsFalse(msg2.Valide)

            'correto
            msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|53|P|2.3.1|" & Chr(10) & _
                           "OBX||NM|171^Dia|2105|67||||||F||APERIODIC|20141229081629" & Chr(10) & _
                           Chr(28)
            Dim msg3 As New Message(msgtest)
            Assert.IsTrue(msg3.Valide)
        End Sub
        <TestMethod()> Public Sub Valide_PID2_Test()
            'não tem versão
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|103|P|2.3.1|" & Chr(10) & _
                           "PID|||000f1404-d361-720c-180d27150010f910||jo4s^v.pereira||19900206|F|" & Chr(10) & _
                           "PV1||I|^^UCIP&8&169722888&4601&&1|||||||||||||||A|" & Chr(10) & _
                           "OBR||||Mindray Monitor|||0|" & Chr(10) & _
                           "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                           Chr(28)

            Dim msg As New Message(msgtest)
            Assert.IsFalse(msg.Valide)
        End Sub
        <TestMethod()> Public Sub Valide_PID4_Test()
            'tem numero
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|103|P|2.3.1|" & Chr(10) & _
                           "PID||11111101664|||joas^3||19900206|F|" & Chr(10) & _
                           "PV1||I|^^UCIP&8&169722888&4601&&1|||||||||||||||A|" & Chr(10) & _
                           "OBR||||Mindray Monitor|||0|" & Chr(10) & _
                           "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                           Chr(28)

            Dim msg As New Message(msgtest)
            Assert.IsFalse(msg.Valide)

            'so tem o primeiro nome
            msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|103|P|2.3.1|" & Chr(10) & _
                           "PID||11111101664|||joas^||19900206|F|" & Chr(10) & _
                           "PV1||I|^^UCIP&8&169722888&4601&&1|||||||||||||||A|" & Chr(10) & _
                           "OBR||||Mindray Monitor|||0|" & Chr(10) & _
                           "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                           Chr(28)

            Dim msg2 As New Message(msgtest)
            Assert.IsFalse(msg2.Valide)
        End Sub
        <TestMethod()> Public Sub Valide_PV11_Test()
            'nao tem Patient Class
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|103|P|2.3.1|" & Chr(10) & _
                           "PID||11111101664|||joas^3||19900206|F|" & Chr(10) & _
                           "PV1|||^^UCIP&8&169722888&4601&&1|||||||||||||||A|" & Chr(10) & _
                           "OBR||||Mindray Monitor|||0|" & Chr(10) & _
                           "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                           Chr(28)

            Dim msg As New Message(msgtest)
            Assert.IsFalse(msg.Valide)
        End Sub
        <TestMethod()> Public Sub ValideOBR3_Test()
            ' nao tem universal service id(Monitor MindRay)
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|103|P|2.3.1|" & Chr(10) & _
                           "PID|||000f1404-d361-720c-180d27150010f910||joas^v.pereira||19900206|F|" & Chr(10) & _
                           "PV1||I|^^UCIP&8&169722888&4601&&1|||||||||||||||A|" & Chr(10) & _
                           "OBR|||||||0|" & Chr(10) & _
                           "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                           "OBX||NM|51^||54.0||||||F" & Chr(10) & _
                           "OBX||ST|2301^||753455||||||F" & Chr(10) & _
                           "OBX||CE|2302^Sangue||0^N||||||F" & Chr(10) & _
                           "OBX||CE|2303^Mpasso||2^||||||F" & Chr(10) & _
                           "OBX||ST|2308^BedNoStr||08||||||F" & Chr(10) & _
                           Chr(28)
            Dim msg As New Message(msgtest)
            Assert.IsFalse(msg.Valide)

        End Sub
        <TestMethod()> Public Sub ValideOBX1_Test()
            ' nao tem value type
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|203|P|2.3.1|" & Chr(10) & _
                           "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                           "OBX|||51^||54.0||||||F" & Chr(10) & _
                           "OBX||ST|2301^||753455||||||F" & Chr(10) & _
                           Chr(28)
            Dim msg As New Message(msgtest)
            Assert.IsFalse(msg.Valide)

        End Sub
        <TestMethod()> Public Sub ValideOBX2_Test()
            ' nao tem id
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|203|P|2.3.1|" & Chr(10) & _
                           "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                           "OBX||NM|51^||54.0||||||F" & Chr(10) & _
                           "OBX||ST|^2301||753455||||||F" & Chr(10) & _
                           Chr(28)
            Dim msg As New Message(msgtest)
            Assert.IsFalse(msg.Valide)

        End Sub
        <TestMethod()> Public Sub ValideOBX4_Test()
            ' nao tem Observation Results
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|203|P|2.3.1|" & Chr(10) & _
                           "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                           "OBX||NM|51^||54.0||||||F" & Chr(10) & _
                           "OBX||ST|2301^||||||||F" & Chr(10) & _
                           Chr(28)
            Dim msg As New Message(msgtest)
            Assert.IsFalse(msg.Valide)
        End Sub
        <TestMethod()> Public Sub ValideOBX10_Test()
            ' nao tem Observation Results Status
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|203|P|2.3.1|" & Chr(10) & _
                           "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                           "OBX||NM|51^||54.0||||||" & Chr(10) & _
                           Chr(28)
            Dim msg As New Message(msgtest)
            Assert.IsFalse(msg.Valide)

            'tem Observation Results Status diferente de F
            msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|203|P|2.3.1|" & Chr(10) & _
                           "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                           "OBX||NM|51^||54.0||||||A" & Chr(10) & _
                           Chr(28)
            Dim msg2 As New Message(msgtest)
            Assert.IsFalse(msg2.Valide)
        End Sub
#End Region

        <TestMethod()> Public Sub parseDataTest()
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|103|P|2.3.1|" & Chr(10) & _
                           "PID|||000f1404-d361-720c-180d27150010f910||joas^v.pereira||19900206|F|" & Chr(10) & _
                           "PV1||I|^^UCIP&8&169722888&4601&&1|||||||||||||||A|" & Chr(10) & _
                           "OBR||||Mindray Monitor|||0|" & Chr(10) & _
                           "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                           "OBX||NM|51^||54.0||||||F" & Chr(10) & _
                           "OBX||ST|2301^||753455||||||F" & Chr(10) & _
                           "OBX||CE|2302^Sangue||0^N||||||F" & Chr(10) & _
                           "OBX||CE|2303^Mpasso||2^||||||F" & Chr(10) & _
                           "OBX||ST|2308^BedNoStr||08||||||F" & Chr(10) & _
                           Chr(28)
            Dim msg As New Message()
            Dim result As String = msg.parseData(msgtest)
            Dim test = "ORU^R01"
            Assert.AreEqual(test, result)
        End Sub

        <TestMethod()> Public Sub getTimeTest()
            Dim msgtest = Chr(11) & _
                           "MSH|^~\&|||||||ORU^R01|103|P|2.3.1|" & Chr(10) & _
                           "PID|||000f1404-d361-720c-180d27150010f910||joas^v.pereira||19900206|F|" & Chr(10) & _
                           "PV1||I|^^UCIP&8&169722888&4601&&1|||||||||||||||A|" & Chr(10) & _
                           "OBR||||Mindray Monitor|||0|" & Chr(10) & _
                           "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                           "OBX||NM|51^||54.0||||||F" & Chr(10) & _
                           "OBX||ST|2301^||753455||||||F" & Chr(10) & _
                           "OBX||CE|2302^Sangue||0^N||||||F" & Chr(10) & _
                           "OBX||CE|2303^Mpasso||2^||||||F" & Chr(10) & _
                           "OBX||ST|2308^BedNoStr||08||||||F" & Chr(10) & _
                           Chr(28)
            'Dim test = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")
            Dim msg As New Message(msgtest)
            Dim result As String = msg.getTime
            'Assert.AreEqual(test, result)
            Assert.AreNotSame(result, "")
        End Sub

        <TestMethod()> Public Sub getSegmentFieldTest()
            Dim msgtest = Chr(11) & _
                          "MSH|^~\&|||||||ORU^R01|103|P|2.3.1|" & Chr(10) & _
                          "PID|||000f1404-d361-720c-180d27150010f910||joas^v.pereira||19900206|F|" & Chr(10) & _
                          "PV1||I|^^UCIP&8&169722888&4601&&1|||||||||||||||A|" & Chr(10) & _
                          "OBR||||Mindray Monitor|||0|" & Chr(10) & _
                          "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                          "OBX||NM|51^||54.0||||||F" & Chr(10) & _
                          "OBX||ST|2301^||753455||||||F" & Chr(10) & _
                          "OBX||CE|2302^Sangue||0^N||||||F" & Chr(10) & _
                          "OBX||CE|2303^Mpasso||2^||||||F" & Chr(10) & _
                          "OBX||ST|2308^BedNoStr||08||||||F" & Chr(10) & _
                          Chr(28)
            Dim msg As New Message(msgtest)
            Dim result As String = msg.getSegmentField("PID", 2)
            Dim test = "000f1404-d361-720c-180d27150010f910"
            Assert.AreEqual(test, result)
        End Sub

        <TestMethod()> Public Sub getSegmentCountTest()
            Dim msgtest = Chr(11) & _
                         "MSH|^~\&|||||||ORU^R01|103|P|2.3.1|" & Chr(10) & _
                         "PID|||000f1404-d361-720c-180d27150010f910||joas^v.pereira||19900206|F|" & Chr(10) & _
                         "PV1||I|^^UCIP&8&169722888&4601&&1|||||||||||||||A|" & Chr(10) & _
                         "OBR||||Mindray Monitor|||0|" & Chr(10) & _
                         "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                         "OBX||NM|51^||54.0||||||F" & Chr(10) & _
                         "OBX||ST|2301^||753455||||||F" & Chr(10) & _
                         "OBX||CE|2302^Sangue||0^N||||||F" & Chr(10) & _
                         "OBX||CE|2303^Mpasso||2^||||||F" & Chr(10) & _
                         "OBX||ST|2308^BedNoStr||08||||||F" & Chr(10) & _
                         Chr(28)
            Dim msg As New Message(msgtest)
            Dim result As Integer = msg.getSegmentCont("PID")
            Dim test = 1
            Assert.AreEqual(test, result)

            Dim msg2 As New Message(msgtest)
            Dim result2 As Integer = msg2.getSegmentCont("OBX")
            Dim test2 = 6
            Assert.AreEqual(test2, result2)
        End Sub

        <TestMethod()> Public Sub getSegmentFieldOBXTest()
            Dim msgtest = Chr(11) & _
                          "MSH|^~\&|||||||ORU^R01|103|P|2.3.1|" & Chr(10) & _
                          "PID|||000f1404-d361-720c-180d27150010f910||joas^v.pereira||19900206|F|" & Chr(10) & _
                          "PV1||I|^^UCIP&8&169722888&4601&&1|||||||||||||||A|" & Chr(10) & _
                          "OBR||||Mindray Monitor|||0|" & Chr(10) & _
                          "OBX||NM|52^||165.0||||||F" & Chr(10) & _
                          "OBX||NM|51^||54.0||||||F" & Chr(10) & _
                          "OBX||ST|2301^||753455||||||F" & Chr(10) & _
                          "OBX||CE|2302^Sangue||0^N||||||F" & Chr(10) & _
                          "OBX||CE|2303^Mpasso||2^||||||F" & Chr(10) & _
                          "OBX||ST|2308^BedNoStr||08||||||F" & Chr(10) & _
                          Chr(28)
            Dim msg As New Message(msgtest)
            Dim result As String = msg.getSegmentField("OBX", 0, 1)
            Dim test = "NM"
            Assert.AreEqual(test, result)
        End Sub
    End Class


End Namespace


