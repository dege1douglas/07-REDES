// DOUGLAS DA COSTA GOMES - TURMA 2N NOITE - INTERNET, PROTOCOLOS E SEGURANÇA DE SISTEMAS DA INFORMAÇÃO

using System;  // Importa o namespace System que contém as classes fundamentais do .NET
using MimeKit;  // Importa o namespace MimeKit, necessário para criar e manipular mensagens de e-mail
using MailKit.Net.Smtp;  // Importa o namespace MailKit.Net.Smtp, necessário para enviar e-mails via SMTP
using MailKit.Net.Imap;  // Importa o namespace MailKit.Net.Imap, necessário para receber e-mails via IMAP
using MailKit.Security;  // Importa o namespace MailKit.Security, necessário para configurações de segurança

class Program
{
    static void Main(string[] args)
    {
        EnviarEmail();  // Chama a função para enviar um e-mail
        ReceberEmails();  // Chama a função para receber e-mails
    }

    static void EnviarEmail()
    {
        // Cria uma nova mensagem de e-mail
        var mensagem = new MimeMessage();

        // Define o remetente do e-mail (seu nome e e-mail)
        mensagem.From.Add(new MailboxAddress("DOUGLAS DEV", "testesdevdouglas@gmail.com"));

        // Define o destinatário do e-mail (nome e e-mail)
        mensagem.To.Add(new MailboxAddress("Douglas", "dglscosta007@gmail.com"));

        // Define o assunto do e-mail
        mensagem.Subject = "Teste de envio de e-mail";

        // Define o corpo do e-mail
        mensagem.Body = new TextPart("plain")
        {
            Text = @"Olá Professor,
                    Este é um e-mail de teste enviado usando C# e MailKit.
                    Atenciosamente,
                    Seu Nome"
        };

        // Usa o cliente SMTP para enviar o e-mail
        using (var cliente = new SmtpClient())
        {
            try
            {
                // Conecta ao servidor SMTP do Gmail usando STARTTLS (TLS sobre uma conexão iniciada sem criptografia)
                cliente.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                // Autentica no servidor SMTP usando seu e-mail e senha
                cliente.Authenticate("seu-email@gmail.com", "sua-senha");

                // Envia a mensagem de e-mail
                cliente.Send(mensagem);

                // Desconecta do servidor SMTP
                cliente.Disconnect(true);

                // Informa que o e-mail foi enviado com sucesso
                Console.WriteLine("E-mail enviado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao enviar e-mail: " + ex.Message);
            }
        }
    }

    static void ReceberEmails()
    {
        // Usa o cliente IMAP para receber e-mails
        using (var cliente = new ImapClient())
        {
            try
            {
                // Conecta ao servidor IMAP do Gmail usando SSL
                cliente.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

                // Autentica no servidor IMAP usando seu e-mail e senha
                cliente.Authenticate("seu-email@gmail.com", "sua-senha");

                // Seleciona a caixa de entrada
                cliente.Inbox.Open(MailKit.FolderAccess.ReadOnly);

                // Lê as primeiras 10 mensagens da caixa de entrada
                for (int i = 0; i < Math.Min(10, cliente.Inbox.Count); i++)
                {
                    var mensagem = cliente.Inbox.GetMessage(i);
                    Console.WriteLine("Assunto: {0}", mensagem.Subject);
                    Console.WriteLine("De: {0}", mensagem.From);
                    Console.WriteLine("Data: {0}", mensagem.Date);
                    Console.WriteLine("Corpo: {0}\n", mensagem.TextBody);
                }

                // Desconecta do servidor IMAP
                cliente.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao receber e-mails: " + ex.Message);
            }
        }
    }
}