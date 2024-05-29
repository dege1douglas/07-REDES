// DOUGLAS DA COSTA GOMES - TURMA 2N NOITE - INTERNET, PROTOCOLOS E SEGURANÇA DE SISTEMAS DA INFORMAÇÃO

using System;  // Importa o namespace System que contém as classes fundamentais do .NET
using MimeKit;  // Importa o namespace MimeKit, necessário para criar e manipular mensagens de e-mail
using MailKit.Net.Smtp;  // Importa o namespace MailKit.Net.Smtp, necessário para enviar e-mails via SMTP
using MailKit.Security;  // Importa o namespace MailKit.Security, necessário para configurações de segurança

class Program
{
    static void Main(string[] args)
    {
        EnviarEmail();  // Chama a função para enviar um e-mail
    }

    static void EnviarEmail()
    {
        // Cria uma nova mensagem de e-mail
        var mensagem = new MimeMessage();

        // Define o remetente do e-mail (seu nome e e-mail)
        mensagem.From.Add(new MailboxAddress("Seu Nome", "seu-email@gmail.com"));

        // Define o destinatário do e-mail (nome do professor e e-mail do professor)
        mensagem.To.Add(new MailboxAddress("Professor", "email-do-professor@example.com"));

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
    }
}
