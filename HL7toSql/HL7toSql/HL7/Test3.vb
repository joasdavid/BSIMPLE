Imports System.Net

Module Test3
    Private Sub dec2IpAndIp2Dec()
        Console.WriteLine("IP")
        Dim dec As Int64 = "3232235811" 'Console.ReadLine()
        Dim bytes As Byte() = BitConverter.GetBytes(dec)
        Array.Reverse(bytes)
        Dim s As String
        For Each ipval In bytes
            If ipval <> 0 Then
                s += ipval & "."
            End If
        Next
        s = s.Substring(0, s.Length - 1)
        Dim ip As IPAddress = IPAddress.Parse(s)
        Dim bytes1 As Byte() = ip.GetAddressBytes()
        Array.Reverse(bytes1)
        Dim Ip2int As Int64 = BitConverter.ToUInt32(bytes1, 0)
        Console.WriteLine("{0} {1}", s, Ip2int)
        Console.ReadKey()

    End Sub
    Private Sub ip2dec()
        Dim ip As IPAddress = IPAddress.Parse("192.168.1.31")
        Dim bytes1 As Byte() = ip.GetAddressBytes()
        Array.Reverse(bytes1)
        Dim Ip2int As Int64 = BitConverter.ToUInt32(bytes1, 0)
        Console.WriteLine("{0}", Ip2int)
        Console.ReadKey()
    End Sub
    Sub main()
        'dec2IpAndIp2Dec()
        ip2dec()
    End Sub
End Module