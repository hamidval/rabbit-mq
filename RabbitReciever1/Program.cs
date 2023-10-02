// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Sender
ConnectionFactory factory = new ConnectionFactory();

factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "Rabbit reciever 1 app";

IConnection connection = factory.CreateConnection();

IModel channel = connection.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey);

channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);

consumer.Received += (sender, args) =>
{
    var body = args.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);
    Console.WriteLine(message);

    channel.BasicAck(args.DeliveryTag, false);
};

string consumerTag = channel.BasicConsume(queueName, false, consumer);
Console.ReadLine();
channel.BasicCancel(consumerTag);

channel.Close();
connection.Close();