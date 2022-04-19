# kafka_101

Kafka is not made for ad-hoc queries, it is made for quick inserts and reads.

Kafka is composed of the broker. It listens for connections (port 9092) 
We have the producer, which produces content, and consumer which consumes content from the broker.

There is a connection from the producer to the broker, it is bi-directional. (TCP)

Topics are logical partitions that you write content to. A producer has to specify which topic it wants to write to.
Consumers specify which topic they want to consume.

You can only append data to kafka, not delete.
The position is also specified when you append data.

When you consume, it will read sequentially from position zero. 

When a table grows larger, we shard. (called partitions in kafka)

So, producers and consumers need to specify a topic, partition and position. 

#Queue vs Pub Sub

Queue: Message is published once, and then consumed once. Good for job execution. Jobs won't be executed twice.
Pub sub: Published once, but consumed many times. (It isn't removed from the source). 

Kafka asks, how can we do both.
Answer: Consumer group 
It removes the awareness of the partition from the user. 
By default it acts like a queue.
You create a consumer, add it to a group, it will consume all content from that topic, and either all partitions (if there is only one consumer) or the partitions will be split amongst the number of consumers in the group. (up to the limit of partitions).

