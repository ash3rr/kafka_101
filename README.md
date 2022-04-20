# kafka_101

Real-time event streaming at a global scale, with persistent storage and stream processing instead of ad-hoc queries like in a relational database. 

>It's a distributed log.

We can grab those events from anywhere and integrate them anywhere. 

### Motivation

There are things that happen in the world and its our job to process those events. 

This leads to:
Single platform to connect everyone to every event. Not just a file system to store things.

Real-time stream of events.

All events stored for historical view.

#### Use Cases:
Event driven architecture. 
The paradigm shift is from static snapshots of data to a continuous stream of events. 

Eg., An occasional call from a friend > a constant feed about the activities of all your friends.

Daily news report > Real time news feeds, accessible online anytime, anywhere. 


Kafka is not made for ad-hoc queries, it is made for quick inserts and reads.

### Zookeeper
Cluster management
Failure detection and recovery
Store ACLS and secrets


Kafka is composed of the broker. It listens for connections (port 9092) 
We have the producer, which produces content, and consumer which consumes content from the broker.

There is a connection from the producer to the broker, it is bi-directional. (TCP)

### topics
Streams of related messages/events.
Logical representation.
Categorises messages into groups.
Developers define topics.
Unlimited number of topics.
Producer <> topic: N to N relation.

A topic is basically a log. (technically a partition is a log)
A topic can have partitions, within a partition you have multiple segments. (like a rolling file) these partitions can then be allocated to different brokers, which enables us to scale out read/write performance. 

Partition location is handled automatically by Kafka.

A segment exists on physical disk as a file (a bunch of them actually).

Topics are logical partitions that you write content to. A producer has to specify which topic it wants to write to.
Consumers specify which topic they want to consume.
You can only add to the end of the log. 

When you consume, it will read sequentially from position zero. 

When a table grows larger, we shard. (called partitions in kafka)

So, producers and consumers need to specify a topic, partition and position. 

## Data Structure

Every event is a key value pair. 
Every message has a timestamp.
Optional headers. (metadata).

## Brokers
Brokers manage partitions.
It does storage and pub/sub.

Producers send messages to brokers.
Brokers receive and store messages.
Kafka cluster can have many brokers.
Each broker manages multiple partitions.

### broker replication

Each partition is replicated a configurable number of times on other brokers.
The replication factor is typically 3.
One of these is called the leader, the others the followers. 
>When you produce, you do so to the leader

## Queue vs Pub Sub

Queue: Message is published once, and then consumed once. Good for job execution. Jobs won't be executed twice.
Pub sub: Published once, but consumed many times. (It isn't removed from the source). 

### Kafka asks, how can we do both.
Answer: Consumer group 
It removes the awareness of the partition from the user. 
By default it acts like a queue.
You create a consumer, add it to a group, it will consume all content from that topic, and either all partitions (if there is only one consumer) or the partitions will be split amongst the number of consumers in the group. (up to the limit of partitions).
Content from that partition will only be visible to consumers that match the partition.
To act like pub/sub, put each consumer in a unique group.

### Result: we get parallel processing for free

## Distributed Systems

In a system with multiple brokers, there is the abstraction of a leader and a follower.
You can write to a leader, but not a follower. A follower is for reading.

Topics are replicated to both machines, but the partition is only designated to be a leader on machine 1, and a follower on machine 2.

The zookeeper herds the 'cats' to the appropriate leader.

## how to spin up zookeeper and kafka (using docker obviously)

open terminal:

and run: `docker compose -f docker-compose.yml up -d`

shell into your kafka:
`docker exec -it kafka /bin/sh `

nav to folder:
`cd opt/kafka/bin`

in this folder is a list of kafka shell scripts, to create a new topic run this command:
`kafka-topic.sh --create --zookeeper zookeeper:2181 --replication-factor 1 --partitions 1 --topic first_kafka_topic`

to list topics:
`kafka-topic.sh --list --zookeeper zookeeper:2181`


Note: I tried to spin up my docker environment with the below commands but I could not resolve the hostname of my machine. (docker compose was my way around this)
`docker run --name zookeeper  -p 2181:2181 -d zookeeper`
`docker run -p 9092:9092 --name kafka  -e KAFKA_ZOOKEEPER_CONNECT=localhost:2181 -e KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://localhost:9092 -e KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=1 -d confluentinc/cp-kafka`


## How to connect visual studio code to docker

Install docker extension
Under containers right click and click attach to visual studio code
You can also attach shell in the same way

## Kafka Shell Commands

Kafka Topics: Create, list, describe, delete
Kafka Producers: send data to topic
Kafka consumers: read data from topic

Create another topic in the shell:
`kafka-topics.sh --create --zookeeper zookeeper:2181 --replication-factor 1 --partitions 1 --topic dummy_topic`

List the topics:
`kafka-topics.sh --list --zookeeper zookeeper:2181`

List info on topic:
`kafka-topics.sh --describe --zookeeper zookeeper:2181 --topic dummy_topic`

Delete a topic:
`kafka-topics.sh --delete --zookeeper zookeeper:2181 --topic dummy_topic`

Create a new message topic:
`kafka-topics.sh --create --zookeeper zookeeper:2181 --replication-factor 1 --partitions 1 --topic messages`

`kafka-console-producer.sh --broker-list kafka:9092 --topic messages`

`{'user_id': 1, 'recipient_id':2, 'message': 'Hi.'}
{'user_id': 2, 'recipient_id':1, 'message': 'Hello there.'}`

Ctrl-c to escape.

Open consumer to read messages:
New terminal window > `docker exec -it kafka /bin/sh` 
`cd opt/kafka/bin`
`kafka-console-consumer.sh --bootstrap-server kafka:9092 --topic messages`

It will not show existing messages, but only new ones that arrive, so add another message on the producer and it will appear in the consumer console:

`kafka-console-producer.sh --broker-list kafka:9092 --topic messages`
`{'user_id': 1, 'recipient_id':2, 'message': 'Hi.'}
{'user_id': 2, 'recipient_id':1, 'message': 'Hello there.'}`

To list all messages:
`kafka-console-consumer.sh --bootstrap-server kafka:9092 --topic messages --from-beginning`

## Consumers and producers in Python

Install kafka python
`python3 -m pip install kafka-python`

`touch data_generator.py`
`touch producer.py`
`touch consumer.py`


