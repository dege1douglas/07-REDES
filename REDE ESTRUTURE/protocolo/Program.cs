// DOUGLAS DA COSTA GOMES - TURMA 2N NOITE - INTERNET, PROTOCOLOS E SEGURANÇA DE SISTEMAS DA INFORMAÇÃO

using System;  // Importa as classes fundamentais do .NET
using System.Net;  // Importa classes para redes (endereços IP, etc.)
using System.Net.Sockets;  // Importa classes para operações de sockets (TCP/UDP)
using System.Text;  // Importa classes para manipulação de texto e strings
using System.Diagnostics;  // Importa classes para trabalhar com processos do sistema (como o tracert)
using MailKit.Net.Smtp;  // Importa classes para enviar e-mails via SMTP usando MailKit
using MailKit.Security;  // Importa classes para configurações de segurança de MailKit
using MimeKit;  // Importa classes para criar e-mails usando MimeKit
using System.Net.NetworkInformation;  // Importa classes para operações de rede como ICMP (ping)

class ProgramaPrincipal
{
    static void Main(string[] args)
    {
        // Apresenta opções para o usuário
        Console.WriteLine("Selecione a opção:");
        Console.WriteLine("1. Iniciar Servidor");
        Console.WriteLine("2. Iniciar Cliente");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                IniciarServidor();
                break;
            case "2":
                IniciarCliente();
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }

    static void IniciarServidor()
    {
        // Apresenta opções de protocolo para o servidor
        Console.WriteLine("Selecione o protocolo para iniciar:");
        Console.WriteLine("1. TCP");
        Console.WriteLine("2. UDP");
        Console.WriteLine("3. Chat");
        Console.WriteLine("4. ICMP");
        Console.WriteLine("5. Traceroute");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                IniciarTCP();
                break;
            case "2":
                IniciarUDP();
                break;
            case "3":
                IniciarChat();
                break;
            case "4":
                TestarICMP();
                break;
            case "5":
                RealizarTraceroute();
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }

    static void IniciarCliente()
    {
        // Apresenta opções de protocolo para o cliente
        Console.WriteLine("Selecione o protocolo para testar:");
        Console.WriteLine("1. TCP");
        Console.WriteLine("2. UDP");
        Console.WriteLine("3. Chat");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                TestarTCP();
                break;
            case "2":
                TestarUDP();
                break;
            case "3":
                TestarChat();
                break;
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }

    // Métodos para o Servidor
    static void IniciarTCP()
    {
        // Cria um listener TCP na porta 5000
        TcpListener server = new TcpListener(IPAddress.Any, 5000);
        server.Start();
        Console.WriteLine("Servidor TCP iniciado na porta 5000.");
        while (true)
        {
            // Aceita um cliente TCP
            TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Recebido: " + message);
            byte[] response = Encoding.ASCII.GetBytes("Resposta do servidor TCP");
            stream.Write(response, 0, response.Length);
            client.Close();
        }
    }

    static void IniciarUDP()
    {
        // Cria um listener UDP na porta 5001
        UdpClient server = new UdpClient(5001);
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        Console.WriteLine("Servidor UDP iniciado na porta 5001.");
        while (true)
        {
            // Recebe dados de um cliente UDP
            byte[] data = server.Receive(ref remoteEP);
            string message = Encoding.ASCII.GetString(data);
            Console.WriteLine("Recebido: " + message);
            byte[] response = Encoding.ASCII.GetBytes("Resposta do servidor UDP");
            server.Send(response, response.Length, remoteEP);
        }
    }

    static void IniciarChat()
    {
        // Cria um listener TCP para chat na porta 5002
        TcpListener server = new TcpListener(IPAddress.Any, 5002);
        server.Start();
        Console.WriteLine("Servidor de Chat iniciado na porta 5002.");
        while (true)
        {
            // Aceita um cliente de chat TCP
            TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Chat: " + message);
            byte[] response = Encoding.ASCII.GetBytes("Resposta do servidor de Chat");
            stream.Write(response, 0, response.Length);
            client.Close();
        }
    }

    static void TestarICMP()
    {
        // Solicita o IP para testar ICMP (ping)
        Console.WriteLine("Digite o IP para realizar o teste ICMP:");
        string ipAddress = Console.ReadLine();
        Ping pingSender = new Ping();
        PingReply reply = pingSender.Send(ipAddress);

        if (reply.Status == IPStatus.Success)
        {
            // Exibe o resultado do ping
            Console.WriteLine("Resposta de: {0}", reply.Address.ToString());
            Console.WriteLine("Tempo de resposta: {0}ms", reply.RoundtripTime);
        }
        else
        {
            Console.WriteLine("Falha no ping.");
        }
    }

    static void RealizarTraceroute()
    {
        // Solicita o IP ou HOST para realizar o traceroute
        Console.WriteLine("Digite o IP ou HOST para realizar o Traceroute:");
        string host = Console.ReadLine();
        using (Process p = new Process())
        {
            p.StartInfo.FileName = "tracert";
            p.StartInfo.Arguments = host;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            // Exibe o resultado do traceroute
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            Console.WriteLine(output);
        }
    }

    // Métodos para o Cliente

    static void TestarTCP()
    {
        // Conecta ao servidor TCP na porta 5000
        TcpClient client = new TcpClient("127.0.0.1", 5000);
        NetworkStream stream = client.GetStream();
        byte[] data = Encoding.ASCII.GetBytes("Mensagem do Cliente TCP");
        stream.Write(data, 0, data.Length);

        // Lê a resposta do servidor TCP
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Recebido: " + response);

        client.Close();
    }

    static void TestarUDP()
    {
        // Conecta ao servidor UDP na porta 5001
        UdpClient client = new UdpClient();
        client.Connect("127.0.0.1", 5001);
        byte[] data = Encoding.ASCII.GetBytes("Mensagem do Cliente UDP");
        client.Send(data, data.Length);

        // Lê a resposta do servidor UDP
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        byte[] response = client.Receive(ref remoteEP);
        Console.WriteLine("Recebido: " + Encoding.ASCII.GetString(response));

        client.Close();
    }

    static void TestarChat()
    {
        // Conecta ao servidor de Chat TCP na porta 5002
        TcpClient client = new TcpClient("127.0.0.1", 5002);
        NetworkStream stream = client.GetStream();
        byte[] data = Encoding.ASCII.GetBytes("Mensagem do Cliente de Chat");
        stream.Write(data, 0, data.Length);

        // Lê a resposta do servidor de Chat
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Recebido: " + response);

        client.Close();
    }
}
