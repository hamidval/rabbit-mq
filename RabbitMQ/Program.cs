// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

Timer timer1;
Console.WriteLine("Hello, World!");

//Sender
ConnectionFactory factory = new ConnectionFactory();

factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit sender app";

IConnection connection = factory.CreateConnection();

IModel channel = connection.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";



timer1 = new Timer(TimerCallback, null, 0, (int) 1000);

Console.ReadLine();

void TimerCallback(Object stateInfo) 
{
    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
    channel.QueueDeclare(queueName, false, false, false, null);
    channel.QueueBind(queueName, exchangeName, routingKey);

    byte[] messageBodyBytes = Encoding.UTF8.GetBytes("hello youtube");
    channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);

   // channel.Close();
   // connection.Close();
}
