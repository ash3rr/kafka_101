namespace consumer_net{

Class Program{

    static void Main(string[] args){
        Console.WriteLine("starting consumer");
        var config = new Dictionary<string,object>{
            {"group.id", "dot-net-consumer-group"},
            {"bootsrap.servers","localhost:9092"},
            {"auto.commit.interval.ms",5000},
            {"auto.offset.reset","earliest"}
        };

        var deserializer = new StringDeserializer (Encoding.UTF8);
        using (var consumer = new consumer_net<string, string>(config,deserializer,deserializer))
        consumer.OnMessage += (consumer_net,msg) =>
            Console.WriteLine ($"Read ('{msg.Key}', '{msg.Value}') from: {msg.TopicPartitionOffset}");

            consumer.OnError += (consumer_net, error) =>
                Console.WriteLine ($"Error: {error}");

            consumer.OnConsumeError += (consumer_net, msg) =>
                Console.WriteLine ($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");

            consumer.Subscribe ("hello_world_topic");

            while(true){
                consumer.Poll (TimeSpan.FromMilliseconds(100));
            }
    }

}

