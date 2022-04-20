package clients;

import java.util.Properties;
import org.apache.kafka.clients.producer.KafkaProducer;
import org.apache.kafka.clients.producer.ProducerRecord;

public class BasicProducer{
    public static void main(String[] args){
        System.out.println("starting producer");

    

Properties settings = new Properties();
settings.put("client.id","");
settings.put("bootstrap.servers","");
settings.put("key.serializer","");
settings.put("value.serializer","");

//create the producer
final KafkaProducer<String, String> producer = new KafkaProducer<>(settings);

// shutdown behaviour
Runtime.getRuntime().addShutdownHook(new Thread(() -> {
    System.out.println("Stopping producer");
    producer.close();

}));

final String topic = "hello_world_topic";
for(int i=1; i<=5; i++){
    final String key = "key-" + i;
    final String value = "value-" + i;
    final ProducerRecord<String, String> record = new ProducerRecord<>(topic, key, value);
    producer.send(record);

    }
}
}