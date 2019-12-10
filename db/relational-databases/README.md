# Relational databases

Relational database is a collection of data items organized in tables. Relational databases implement 'Strong consistency' pattern of 'Consistency' patterns.

## Table of contents

1. Normalization
1. Indexes
1. Transactions
1. Scaling technics
1. Useful questions/answers

## Normalization

Normalization is the process of organizing the data in the database.

### 1NF

A relation is in 1NF if it contains an atomic value. If 1 cell contains several values first NF is violated.

**Violation of 1NF.** EMP_PHONE contains more then 1 value.

| EMP_ID  | EMP_NAME |        EMP_PHONE       | EMP_STATE |
| --------| -------- | ---------------------- | --------- |
|    14   |   John   | 7272826385, 9064738238 | UP        |
|    20   |   Hary   | 8574783832             | Bihar     |
|    12   |   Samy   | 7390372389, 8589830302 | Punjab    |

**Correct 1NF.** EMP_PHONE contains atomic value.

| EMP_ID  | EMP_NAME | EMP_PHONE  | EMP_STATE |
| --------| -------- | ---------- | --------- |
|    14   |   John   | 9064738238 | UP        |
|    14   |   John   | 7272826385 | UP        |
|    20   |   Hary   | 8574783832 | Bihar     |
|    12   |   Samy   | 7390372389 | Punjab    |
|    12   |   Samy   | 8589830302 | Punjab    |

### 2NF

A relation will be in 2NF if:

- it is in 1NF and
- all non-key attributes are fully functional dependent on the primary key.

**Violation of 2NF.** Non-prime attribute TEACHER_AGE is dependent on TEACHER_ID which is a proper subset of a candidate key.

| TEACHER_ID  |  SUBJECT  | TEACHER_AGE |
| ------------| --------- | ----------- |
|      25     | Chemistry | 30          |
|      25     | Biology   | 30          |
|      47     | English   | 35          |
|      83     | Math      | 38          |
|      83     | Computer  | 38          |

**Correct 2NF.** TEACHER_AGE depends only on the primary key.

| TEACHER_ID  | TEACHER_AGE |
| ------------| ----------- |
|      25     | 30          |
|      47     | 35          |
|      83     | 38          |

| TEACHER_ID  |  SUBJECT  |
| ------------| --------- |
|      25     | Chemistry |
|      25     | Biology   |
|      47     | English   |
|      83     | Math      |
|      83     | Computer  |

### 3NF

Relation will be in 3NF if it is in 2NF and no transition dependency exists.

3NF is used to reduce the data duplication. It is also used to achieve the data integrity. If there is no transitive dependency for non-prime attributes, then the relation must be in third normal form.

### 4NF

A relation will be in 4NF if it is in Boyce Codd normal form and has no multi-valued dependency.

### 5NF

A relation is in 5NF if it is in 4NF and not contains any join dependency and joining should be lossless

## Indexes

[Source of information](https://habr.com/ru/post/247373/)

## Transactions

Transaction is unit of work performed within database management system.

ACID principles [source](https://searchsqlserver.techtarget.com/definition/ACID):

- **Atomicity.** Means that you can guarantee that all of a transaction happens, or none of it does.
- **Consistency.** Means that you guarantee that your data will be consistent; none of the constraints you have on related data will ever be violated.
- **Isolation.** Means that one transaction cannot read data from another transaction that is not yet completed.
- **Durability.** Means that once a transaction is complete, it is guaranteed that all of the changes have been recorded to a durable medium (such as a hard disk).

## Scaling technics

**Scaling** is the process of increasing or decreasing the capacity of the system by changing the number of processes available to service requests. **Scalability** is when we add resources to a server, we improve performance.

**Replication** is copying of data to other server.

**Master replica** is a server that can read and write.

**Slave replica** is a server that can only read.

**Server cluster** is a group of servers working as single instance.

### Replications

**Master-slave** replication is approach when there's a master and many slaves. When master shuts down the server allows only reading until a slave is not promoted to master.

Disadvantage: logic and delay for promoting a slave to master is required.

**Master-master** allows writing and reading to each instance. If a master goes down, another master can continue handling of request.

Disadvantages:

- A load balancer or additional logic in your app is required to determine where to write;
- Most master-master systems are either loosely consistent (violating ACID) or have increased write latency due to synchronization.
- Conflict resolution comes more into play as more write nodes are added and as latency increases.

Disadvantages of replications:

- There is a potential for loss of data when the master fails before any newly written data can be replicated to other nodes;
- If there are a lot of writes, the read replicas can get bogged down with replaying writes and can't do as many reads;
- The more read slaves, the more you have to replicate (leads to greater replication lag).
- On some systems, writing to the master can spawn multiple threads to write in parallel, whereas read replicas only support writing sequentially with a single thread.
- Replication adds more hardware and additional complexity.

### Federation

**Federation** (or functional partitioning) splits up databases by function. For example, instead of a single, monolithic database, you could have three databases: forums, users, and products.

Disadvantages of federation:

- federation is not effective if your schema requires huge functions or tables.
- you'll need to update your application logic to determine which database to read and write.
- joining data from two databases is more complex with a server link.

### Sharding

**Sharding** distributes data across different databases such that each database can only manage a subset of the data. Taking a users database as an example, as the number of users increases, more shards are added to the cluster.

Disadvantages of federation:

- You'll need to update your application logic to work with shards, which could result in complex SQL queries;
- Data distribution can become lopsided in a shard. For example, a set of power users on a shard could result in increased load to that shard compared to others;
- Joining data from multiple shards is more complex.

### Denormalization

**Denormalization** attempts to improve read performance at the expense of some write performance. Redundant copies of the data are written in multiple tables to avoid expensive joins. Some RDBMS such as PostgreSQL and Oracle support materialized views which handle the work of storing redundant information and keeping redundant copies consistent.

Disadvantages of denormalization:

- Data is duplicated.
- Constraints can help redundant copies of information stay in sync, which increases complexity of the database design.
- A denormalized database under heavy write load might perform worse than its normalized counterpart.

### SQL tuning

It's important to benchmark and profile to simulate and uncover bottlenecks.

**Benchmark** - Simulate high-load situations with tools such as ab.

**Profile** - Enable tools such as the slow query log to help track performance issues.

[TBD](https://github.com/donnemartin/system-design-primer#sql-tuning)

## Useful questions/answers

**When to open/close connection to database?**

> Keep connections for a long time, but keep transactions as short as possible.
> Don't open and close connections too often. Hang onto them, but be prepared for them to vanish out from under you.